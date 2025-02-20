import pyodbc
import time
import tkinter as tk
import shutil
import os
import tkinter.font as font
from tkinter import font
from PIL import Image, ImageTk
import sqlite3
import tkinter.messagebox as messagebox
from tkinter import messagebox
from config import winconfig,paths

class HaimerInterface:
    def __init__(self,mainFrame,iDEntry):
        self.mainFrame=mainFrame
        mainFrame.config(width=1900, height=1000)

        self.mdb_file_path = paths["haimerDB"]
        self.copied_mdb_file_path = paths["haimerBackup"]
        self.password = "!1+2§3pp"
        self.query = "SELECT X, Z, MeasuredAt FROM tCutterCurrentValues WHERE KeyToolId = ?"
        self.previous_X = None
        self.previous_Z = None
        self.previous_MeasuredAt = None
        self.continue_monitoring = True  # Flag to control monitoring
        self.conn = None
        self.cursor = None

        self.search_value = iDEntry
        self.measureFunction()


    def measureFunction(self):

        self.clearFrame()

        self.haimerInterfaceWindow= tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.haimerInterfaceWindow.place(relx=0.5, rely=0.5, anchor=tk.CENTER)



        # Load images
        imgAddToolButton = Image.open(paths["imgpaths"]+"\\button12.png")
        self.imgAddtoolButton = ImageTk.PhotoImage(imgAddToolButton)


        imgEditToolButton = Image.open(paths["imgpaths"]+"\\button13.png")
        self.imgEditToolButton = ImageTk.PhotoImage(imgEditToolButton)


        addToolButton = tk.Button(self.haimerInterfaceWindow, image=self.imgAddtoolButton, command= lambda: self.start_monitoring(self.search_value), width=250, height=250)
        addToolButton.pack(side="left", padx=10, pady=10)

        editToolButton = tk.Button(self.haimerInterfaceWindow, image=self.imgEditToolButton, command= lambda: self.toolMeasureEditControl(self.search_value), width=250, height=250)
        editToolButton.pack(side="left", padx=10, pady=10)

        
    def clearFrame(self):
        # Clear all widgets from the mainFrame
        for widget in self.mainFrame.winfo_children():
            widget.destroy()

    def copy_mdb_file(self):
        """Function to copy the MDB file to the new location"""
        try:
            shutil.copy(self.mdb_file_path, self.copied_mdb_file_path)
            print(f"File copied to {self.copied_mdb_file_path}")
        except Exception as e:
            print(f"Error copying file: {e}")

    def check_copied_file(self):
        """Check if the copied file exists"""
        return os.path.exists(self.copied_mdb_file_path)

    def setup_db_connection(self):
        """Establish the database connection to the copied MDB file"""
        try:
            conn_str = (
                r"DRIVER={Microsoft Access Driver (*.mdb, *.accdb)};"
                r"DBQ=" + self.copied_mdb_file_path + ";"
                r"PWD=" + self.password + ";"
            )
            self.conn = pyodbc.connect(conn_str)
            self.cursor = self.conn.cursor()
        except Exception as e:
            print(f"Error setting up database connection: {e}")

    def show_value_window(self, old_x, old_z, old_measured_at):
        """Create and display the Tkinter window for values"""

        def countdown(counter):
            if counter > 0:
                button_nochmal.config(text=f"Aktualisieren ({counter}s)")
                self.haimerInterfaceWindow.after(1000, countdown, counter - 1)
            else:
                button_nochmal.config(state=tk.NORMAL, text="Aktualisieren")

        def check_and_update_values():
            self.copy_mdb_file()
            
            if self.check_copied_file():
                self.cursor.execute(self.query, (self.search_value,))
                row = self.cursor.fetchone()

                if row and ((row.X != old_x) or (row.Z != old_z) or (row.MeasuredAt != old_measured_at)):
                    label_new_values.config(text=f"neue Werte:\nX: {row.X}\nZ: {row.Z}\nMeasured At: {row.MeasuredAt}",command=lambda: self.currentMeasurmentClick(row.X,row.Z))
                    # Update previous values
                    self.previous_X = row.X
                    self.previous_Z = row.Z
                    self.previous_MeasuredAt = row.MeasuredAt
                    # Stop the on_nochmal method
                    self.stop_nochmal = True  # Flag to stop the countdown
                    button_nochmal.config(state=tk.NORMAL)  # Enable the button immediately
                    return  # Exit the method

            # Schedule the next check after 5 seconds
            if button_nochmal.cget('state') == tk.DISABLED:
                self.haimerInterfaceWindow.after(3000, check_and_update_values)

        def on_nochmal():
            self.stop_nochmal = False  # Reset the stop flag
            button_nochmal.config(state=tk.DISABLED)  # Disable for 20 seconds
            countdown(60)  # Start the countdown
            check_and_update_values()


        def countdown(seconds):
            if seconds > 0 and not self.stop_nochmal:  # Stop if values change
                self.haimerInterfaceWindow.after(1000, countdown, seconds - 1)
            else:
                button_nochmal.config(state=tk.NORMAL)  # Enable button after countdown

        def on_uebernehmen():
            self.continue_monitoring = False
            self.haimerInterfaceWindow.destroy()

        def on_closing():
            self.continue_monitoring = False
            self.haimerInterfaceWindow.destroy()

        import tkinter.messagebox as messagebox
        found=False 

        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        
        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()

        for row in currentToolsData:
            if row[0]==self.search_value:
                self.editedTool=row
                found=True
                break
                
            else:
                found=False

        label_font = font.Font(family=winconfig["fonttype"], size=14, weight="bold")
        iDLabelFont= font.Font(family=winconfig["fonttype"], size=18, weight="bold")
        # Create a frame to hold both buttons
        button_frame = tk.Frame(self.haimerInterfaceWindow)
        button_frame.config(bg=winconfig["bgcolor"])
        button_frame.grid(row=1, column=2)
        self.newXvalue=old_x
        self.newZvalue=old_z


        # Bind the self.haimerInterfaceWindow close event
        # self.haimerInterfaceWindow.protocol("WM_DELETE_WINDOW", on_closing)
        iDLabel=tk.Label(self.haimerInterfaceWindow,text=f"iD: {self.search_value}\nName: {self.editedTool[2]}\nRadius: {float(row[4])}\nLenght: {float(row[11])}",font=iDLabelFont,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        iDLabel.grid(row=0,column=0,padx=20,pady=60)
        try:
            # Load and display image next to the iDLabel
            image_path = paths["toolCapturedimages"]+self.search_value+".png"  # Replace with the correct path to your image
            image = Image.open(image_path)
            image = image.resize((400, 350))  # Resize image if necessary
            photo = ImageTk.PhotoImage(image)

            image_label = tk.Label(self.haimerInterfaceWindow, image=photo, bg=winconfig["bgcolor"])
            image_label.grid(row=0, column=1, padx=10, pady=60)

            # Store a reference to the image to prevent garbage collection
            image_label.image = photo
        except FileNotFoundError:
                        # Load and display image next to the iDLabel
            image_path = paths["imgpaths"]+"NOTFOUND.png"  # Replace with the correct path to your image
            image = Image.open(image_path)
            image = image.resize((400, 350))  # Resize image if necessary
            photo = ImageTk.PhotoImage(image)

            image_label = tk.Label(self.haimerInterfaceWindow, image=photo, bg=winconfig["bgcolor"])
            image_label.grid(row=0, column=1, padx=10, pady=60)

            # Store a reference to the image to prevent garbage collection
            image_label.image = photo

        # Display old values
        label_old_values = tk.Button(self.haimerInterfaceWindow, text=f"aktuelle Werte von Haimer:\nX: {old_x}\nZ: {old_z}\nMeasured At: {old_measured_at}",command=lambda: self.currentMeasurmentClick(old_x,old_z),font=label_font,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        label_old_values.grid(row=1,column=0,padx=20,pady=60)

        # Display new values (initially empty)
        label_new_values = tk.Button(self.haimerInterfaceWindow, text="neue Werte:\nX: ....... \nZ: ....... \nMeasured At: ",font=label_font,bg=winconfig["bgcolor"],fg="lightgreen")
        label_new_values.grid(row=1,column=1,padx=20,pady=60)

        # Nochmal button inside the frame
        button_nochmal = tk.Button(button_frame, text="Aktualisieren", command=on_nochmal,width=20,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        button_nochmal.pack(side=tk.TOP,pady=5,padx=10)


        # Übernehmen button inside the frame, below the Nochmal button
        button_uebernehmen = tk.Button(button_frame, text="Zurück", command=self.measureFunction,width=20,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        button_uebernehmen.pack(side=tk.TOP,pady=5,padx=10)

        self.haimerInterfaceWindow.mainloop()

    def currentMeasurmentClick(self, x, z):
        process=tk.Toplevel()
        process.geometry("660x130")
        process.title("Process")
        processCLick=False

        button1=tk.Button(process,text="übernehmen u.\nschicken",command=lambda: printO("1"),width=20,height=5,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg="lightgreen")
        button1.grid(row=0,column=0,padx=5,pady=5)

        button2=tk.Button(process,text="NUR übernehmen",command=lambda: printO("2"),width=20,height=5,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg="yellow")
        button2.grid(row=0,column=1,padx=5,pady=5)

        button3=tk.Button(process,text="abbrechen",command=lambda: printO("3"),width=20,height=5,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg="tomato")
        button3.grid(row=0,column=2,padx=5,pady=5)

        found = None
        updated_rows = []

        def printO(processNumber):
            lengthOnly = [1, 2, 3]

            if processNumber == "1":
                print("we are here in prosses 1")

                # Connect to the MTMDB.db SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
                
                # Fetch all data from the currentTools table
                cur.execute("SELECT * FROM currentTools")
                currentToolsData = cur.fetchall()

                found = False

                for row in currentToolsData:
                    if row[0] == self.search_value:
                        if row[3] in lengthOnly:
                            print(f"here is {row[3]} with type {type(row[3])}")
                            print(f"her is lengthOnly {lengthOnly} with type {type(lengthOnly[0])}")
                            # Update the Z value in column 11
                            z_value = float(z)
                            cur.execute(
                                "UPDATE currentTools SET toolIstLaenge = ? WHERE idCode = ?",
                                (z_value, row[0])
                            )
                            rowID = row[0]
                            rowPlace = row[19]
                            found = True

                        elif row[3] not in lengthOnly:
                            print("prozess 1 row[3]  not in lengthOnly")
                            print(f"here is {row[3]} with type {type(row[3])}")
                            print(f"her is lengthOnly {lengthOnly} with type {type(lengthOnly[0])}")
                            # Update both X (column 4) and Z (column 11)
                            x_value = float(x)
                            z_value = float(z)
                            cur.execute(
                                "UPDATE currentTools SET toolRadius = ?, toolIstLaenge = ? WHERE idCode = ?",
                                (x_value, z_value, row[0])
                            )
                            rowID = row[0]
                            rowPlace = row[19]
                            found = True

                # Commit changes and close the connection
                conn.commit()  # Commit the transaction first
                cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                conn.close()


                if found:
                    print(f"Updated X value to {x} and Z value to {z} in the CSV file.")

                else:
                    print(f"No matching row found for {self.search_value}. X and Z values not updated.")


                from toolModule import toolModule
                methodePlatzierenCall=toolModule(self.mainFrame)
                methodePlatzierenCall.platzierenCallFromHaimerSchnittstelle(rowPlace,[rowID])
                process.destroy()
                self.measureFunction()

            elif processNumber=="2":
                # Connect to the MTMDB.db SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
                print("we are here in prosses 2")
                # Fetch all data from the currentTools table
                cur.execute("SELECT * FROM currentTools")
                currentToolsData = cur.fetchall()

                found = False

                for row in currentToolsData:
                    if row[0] == self.search_value:
                        if row[3] in lengthOnly:
                            print("row[3] in lengthOnly")
                            print(f"here is {row[3]} with type {type(row[3])}")
                            print(f"her is lengthOnly {lengthOnly} with type {type(lengthOnly[0])}")
                            # Update the Z value in column 11
                            z_value = float(z)
                            cur.execute(
                                "UPDATE currentTools SET toolIstLaenge = ? WHERE idCode = ?",
                                (z_value, row[0])
                            )
                            rowID = row[0]
                            rowPlace = row[19]
                            found = True

                        elif row[3] not in lengthOnly:
                            print("row[3] not in lengthOnly")
                            print(f"here is {row[3]} with type {type(row[3])}")
                            print(f"her is lengthOnly {lengthOnly} with type {type(lengthOnly[0])}")
                            # Update both X (column 4) and Z (column 11)
                            x_value = float(x)
                            z_value = float(z)
                            cur.execute(
                                "UPDATE currentTools SET toolRadius = ?, toolIstLaenge = ? WHERE idCode = ?",
                                (x_value, z_value, row[0])
                            )
                            rowID = row[0]
                            rowPlace = row[19]
                            found = True

                # Commit changes and close the connection
                conn.commit()  # Commit the transaction first
                cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                conn.close()

                if found:
                    print(f"Updated X value to {x} and Z value to {z} in the CSV file.")
                    messagebox.showinfo("Success","Messwerte wurden eingetragen")
                else:
                    print(f"No matching row found for {self.search_value}. X and Z values not updated.")
                process.destroy()
                self.measureFunction()

            else:
                process.destroy()
                return



    def start_monitoring(self, searchValue):
        """Start monitoring the MDB file and updating values"""
        self.search_value = searchValue
        self.clearFrame()
        self.haimerInterfaceWindow = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.haimerInterfaceWindow.place(relx=0.5, rely=0.5, anchor=tk.CENTER)

        try:
            # Initial file copy
            self.copy_mdb_file()

            if not self.check_copied_file():
                raise FileNotFoundError(f"Copied file not found at {self.copied_mdb_file_path}")

            # Setup database connection
            self.setup_db_connection()

            while self.continue_monitoring:
                # Copy the file and query every 5 seconds
                self.copy_mdb_file()

                if self.check_copied_file():
                    if self.conn and not self.conn.closed and self.cursor:
                        try:
                            self.cursor.execute(self.query, (self.search_value,))
                            row = self.cursor.fetchone()

                            if row:
                                self.show_value_window(row.X, row.Z, row.MeasuredAt)
                                self.previous_X = row.X
                                self.previous_Z = row.Z
                                self.previous_MeasuredAt = row.MeasuredAt
                            else:
                                print(f"No rows found with the KeyToolId {self.search_value}")
                                self.__init__(self.haimerInterfaceWindow)
                                messagebox.showerror("NOT FOUND", "NOT FOUND")
                                return
                        except pyodbc.Error as db_err:
                            print(f"Database error: {db_err}")
                    else:
                        print("Database connection or cursor is not properly initialized or already closed.")
                        return

                    time.sleep(5)
                else:
                    print(f"Copied file not found at {self.copied_mdb_file_path}")
                    return

        except Exception as e:
            print(f"An error occurred: {e}")
        finally:
            if self.conn and not self.conn.closed:
                self.conn.close()





    def toolMeasureEditControl(self,iDEntry):
        import tkinter.messagebox as messagebox
        found=False

        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        
        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        for row in currentToolsData:
            if row[0]==iDEntry:
                self.editedTool=row
                found=True
                break
                
            else:
                found=False

        if found==True:
            self.toolMeasureEditWindow(self.editedTool)

        else:
            messagebox.showerror("NOT FOUND","NOT FOUND")
            return
        

    def toolMeasureEditWindow(self, editedTool):
        self.clearFrame()
        self.haimerInterfaceWindow = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.haimerInterfaceWindow.place(relx=0.5, rely=0.5, anchor=tk.CENTER)

        label_font = font.Font(family=winconfig["fonttype"], size=14, weight="bold")
        iDLabelFont = font.Font(family=winconfig["fonttype"], size=18, weight="bold")

        # Place iDLabel on the left side
        iDLabel = tk.Label(self.haimerInterfaceWindow, text=f"iD: {self.search_value}\nName: {editedTool[2]}", font=iDLabelFont, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
        iDLabel.grid(row=0, column=0, padx=20, pady=60)

        # Load and display image next to the iDLabel
        image_path = paths["toolCapturedimages"] + self.search_value + ".png"
        image = Image.open(image_path)
        image = image.resize((400, 350))  # Resize image if necessary
        photo = ImageTk.PhotoImage(image)

        image_label = tk.Label(self.haimerInterfaceWindow, image=photo, bg=winconfig["bgcolor"])
        image_label.grid(row=0, column=1, padx=10, pady=60)
        image_label.image = photo  # Prevent garbage collection

        # Create a frame for the values
        value_frame = tk.Frame(self.haimerInterfaceWindow, bg=winconfig["bgcolor"])
        value_frame.grid(row=1, column=0, columnspan=2, pady=20)

        # Label for "aktuelle Werte" with X and Z
        label_old_values = tk.Label(value_frame, text=f"aktuelle Werte:\nX: {editedTool[4]}\nZ: {editedTool[11]}", font=label_font, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
        label_old_values.grid(row=0, column=0, padx=20)

        # Frame for the new values (neue Werte)
        new_values_frame = tk.Frame(value_frame, bg=winconfig["bgcolor"])
        new_values_frame.grid(row=0, column=1, padx=20)

        # Labels and entry fields for neue Werte X and Z
        label_new_value_x = tk.Label(new_values_frame, text="neue Werte X:", font=label_font, bg=winconfig["bgcolor"], fg="lightgreen")
        label_new_value_x.grid(row=0, column=0, padx=10)
        xEntryValue = tk.Entry(new_values_frame)
        xEntryValue.grid(row=0, column=1, padx=10)

        label_new_value_z = tk.Label(new_values_frame, text="neue Werte Z:", font=label_font, bg=winconfig["bgcolor"], fg="lightgreen")
        label_new_value_z.grid(row=1, column=0, padx=10)
        zEntryValue = tk.Entry(new_values_frame)
        zEntryValue.grid(row=1, column=1, padx=10)

        # Button for "Zurück"
        button_frame = tk.Frame(self.haimerInterfaceWindow, bg=winconfig["bgcolor"])
        button_frame.grid(row=2, column=0, columnspan=2, pady=20)
        
        button_zurueck = tk.Button(button_frame, text="Übernehmen", command=lambda: self.currentMeasurmentClick(xEntryValue.get(),zEntryValue.get()), width=20, font=(winconfig["fonttype"], winconfig["fontsize"], "bold"), bg=winconfig["fontcolor"])
        button_zurueck.grid(row=0,column=0,padx=10, pady=5)
        button_zurueck = tk.Button(button_frame, text="Zurück", command=self.measureFunction, width=20, font=(winconfig["fonttype"], winconfig["fontsize"], "bold"), bg=winconfig["fontcolor"])
        button_zurueck.grid(row=0,column=1,padx=10, pady=5)


    # def addNewMeasureValues(self,editedTool):

