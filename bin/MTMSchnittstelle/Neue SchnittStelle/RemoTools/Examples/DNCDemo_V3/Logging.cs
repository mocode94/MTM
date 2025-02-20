// ------------------------------------------------------------------------------------------------
// <copyright file="Logging.cs" company="DR. JOHANNES HEIDENHAIN GmbH">
// Copyright © DR. JOHANNES HEIDENHAIN GmbH - All Rights Reserved.
// The software may be used according to the terms of the HEIDENHAIN License Agreement which is
// available under www.heidenhain.de
// Please note: Software provided in the form of source code is not intended for use in the form
// in which it has been provided. The software is rather designed to be adapted and modified by
// the user for the user’s own use. Here, it is up to the user to check the software for
// applicability and interface compatibility.  
// </copyright>
// <author>Marco Hayler</author>
// <date>15.12.2015</date>
// <summary>
// This file contains a class for asynchronous logging especialy used to
// write the axes position streaming results to a csv file.
// </summary>
// ------------------------------------------------------------------------------------------------

namespace DNC_CSharp_Demo
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class for asynchronous logging.
    /// </summary>
    public class Logging
    {
        #region "fields"
        /// <summary>
        /// The thread to write the logging from queue to the log file.
        /// </summary>
        private Thread loggerThread = null;

        /// <summary>
        /// Thread safe queue used for logging. The main thread puts new lines to the queue,
        /// a separate thread gets the lines from the queue and writes it to the log file.
        /// </summary>
        private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

        /// <summary>
        /// Is logging active.
        /// </summary>
        private bool loggingActive = false;

        /// <summary>
        /// Full path to the log file.
        /// </summary>
        private string logFile = string.Empty;

        /// <summary>
        /// Add time stamp to each logged line.
        /// </summary>
        private bool addTimeStamp = false;

        /// <summary>
        /// The head line for the string array logging function.
        /// This is used for axes position streaming.
        /// </summary>
        private string[] headLine = null;

        /// <summary>
        /// The size of the columns for the string array logging function.
        /// This is used for axes position streaming.
        /// </summary>
        private int[] columnSize = null;

        /// <summary>
        /// To make the max level counter thread save.
        /// </summary>
        private object lockMaxQueueLevelCounter = new object();
        #endregion

        #region "contructor & destructor"
        /// <summary>
        /// Initializes a new instance of the <see cref="Logging"/> class.
        /// </summary>
        public Logging()
        {
            this.AttachDateTimeToFileName = false;
            this.MaxLogLines = 1000000;
            this.MaxReachedLevel = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logging"/> class.
        /// </summary>
        /// <param name="logFile">Log file with full path.</param>
        public Logging(string logFile)
            : this()
        {
            this.logFile = logFile;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Logging"/> class.
        /// </summary>
        ~Logging()
        {
            if (this.loggingActive)
            {
                this.Stop();
            }
        }
        #endregion

        #region "properties"
        /// <summary>
        /// Gets or sets a value indicating whether a date time has to be attached to the filename.
        /// </summary>
        public bool AttachDateTimeToFileName { get; set; }

        /// <summary>
        /// Gets or sets the max amount of allowed lines in the log file.
        /// </summary>
        public int MaxLogLines { get; set; }

        /// <summary>
        /// Gets the maximum reached level of the queue.
        /// </summary>
        public int MaxReachedLevel { get; private set; }

        /// <summary>
        /// Gets or sets the full path to the log file.
        /// </summary>
        /// <value>The full path to the log file.</value>
        public string Logfile
        {
            get
            {
                return this.logFile;
            }

            set
            {
                this.logFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the column header for the string array logging function.
        /// </summary>
        /// <value>The column names for the header.</value>
        public string[] HeadLine
        {
            get
            {
                return this.headLine;
            }

            set
            {
                int length = value.Length;
                this.headLine = new string[length];
                this.columnSize = new int[length];

                value.CopyTo(this.headLine, 0);
                for (int i = 0; i < length; i++)
                {
                    string entry = value[i];
                    this.columnSize[i] = entry.Length;
                }
            }
        }
        #endregion

        #region "public methods"
        /// <summary>
        /// Starts the logging.
        /// </summary>
        /// <param name="addTimeStamp">With additional time stamp.</param>
        public void Start(bool addTimeStamp = false)
        {
            this.addTimeStamp = addTimeStamp;
            this.loggerThread = new Thread(this.MessageLoop);
            this.loggerThread.Start();
            this.loggingActive = true;

            if (this.headLine != null)
            {
                StringBuilder message = new StringBuilder();
                foreach (string cell in this.headLine)
                {
                    message.Append(cell + "; ");
                }

                this.messageQueue.Enqueue(this.TimeStamp() + message.ToString());
            }
        }

        /// <summary>
        /// Stops the logging.
        /// </summary>
        public void Stop()
        {
            if (this.loggingActive == false)
            {
                return;
            }

            this.loggingActive = false;

            this.LogMessage(Environment.NewLine);

            while (!(this.loggerThread.ThreadState == System.Threading.ThreadState.Stopped))
            {
                Thread.Sleep(100);
            }

            this.loggerThread.Abort();
            this.loggerThread = null;
        }

        /// <summary>
        /// Add a new message to the queue.
        /// The message becomes written to file in another thread.
        /// </summary>
        /// <param name="message">The log message.</param>
        public void LogMessage(string message)
        {
            if (this.loggingActive)
            {
                this.messageQueue.Enqueue(this.TimeStamp() + message);

                lock (this.lockMaxQueueLevelCounter)
                {
                    if (this.MaxReachedLevel < this.messageQueue.Count)
                    {
                        this.MaxReachedLevel = this.messageQueue.Count;
                    }
                }
            }
        }

        /// <summary>
        /// Create a formatted and comma separated line from the string array and write it to log file.
        /// </summary>
        /// <param name="line">Array with the values for each column.</param>
        public void LogMessage(string[] line)
        {
            if (this.loggingActive)
            {
                StringBuilder message = new StringBuilder();
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] != null)
                    {
                        message.Append(line[i].PadLeft(this.columnSize[i]) + "; ");
                    }
                    else
                    {
                        string dummy = string.Empty;
                        message.Append(dummy.PadLeft(this.columnSize[i]) + "; ");
                    }
                }

                this.messageQueue.Enqueue(this.TimeStamp() + message.ToString());

                lock (this.lockMaxQueueLevelCounter)
                {
                    if (this.MaxReachedLevel < this.messageQueue.Count)
                    {
                        this.MaxReachedLevel = this.messageQueue.Count;
                    }
                }
            }
        }
        #endregion

        #region "Message Loop"
        /// <summary>
        /// Writes all messages from the internal queue to the log file.
        /// </summary>
        private void MessageLoop()
        {
            string logLine = string.Empty;
            long linesWritten = 0;

            try
            {
                while (true)
                {
                    if (!this.loggingActive)
                    {
                        break;
                    }

                    string fileName;
                    if (this.AttachDateTimeToFileName)
                    {
                        fileName = this.logFile.TrimEnd(".log".ToCharArray()) + "_" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + ".log";
                    }
                    else
                    {
                        fileName = this.logFile;
                    }

                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        sw.AutoFlush = true;
                        while (true)
                        {
                            if (this.messageQueue.TryDequeue(out logLine))
                            {
                                sw.WriteLine(logLine);
                                linesWritten++;
                            }

                            if (linesWritten >= this.MaxLogLines)
                            {
                                linesWritten = 0;
                                break;
                            }

                            if (this.messageQueue.Count == 0 && !this.loggingActive)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Run without logging!", "Logging Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Create time stamp string.
        /// </summary>
        /// <returns>A special formatted time stamp string.</returns>
        private string TimeStamp()
        {
            string timeStamp = string.Empty;
            DateTime dt = DateTime.Now;
            if (this.addTimeStamp)
            {
                timeStamp = "[" + dt.ToShortDateString() + " - " + dt.ToLongTimeString() + "." + dt.Millisecond.ToString("000") + "] ";
            }

            return timeStamp;
        }
        #endregion
    }
}