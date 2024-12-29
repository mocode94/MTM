from tkinter import Toplevel, Label, Entry, Button, StringVar
import tkinter as tk
from PIL import Image, ImageTk
import sys
import subprocess
import time
import threading
from queue import Queue, Empty
from config import paths,places
import sqlite3


# _________________________________________________________________________
from config import imgpaths,winconfig,paths

class WelcomeWindow:
    def __init__(self, master):
        self.welcome = master
        self.welcome_window = Toplevel(master)
        self.welcome_window.overrideredirect(True)  # Remove title bar


        # Load and resize the image (replace with your image path)
        image_path = f"{imgpaths}mtmlogo.png"
        image = Image.open(image_path)
        resized_image = image.resize((300, 300))
        self.photo = ImageTk.PhotoImage(resized_image)

        # Create label to display image
        label = Label(self.welcome_window, image=self.photo)
        label.pack()

        # Calculate window position to center it on the screen
        screen_width = self.welcome_window.winfo_screenwidth()
        screen_height = self.welcome_window.winfo_screenheight()
        window_width = resized_image.width
        window_height = resized_image.height
        position_right = int(screen_width / 2 - window_width / 2)
        position_down = int(screen_height / 2 - window_height / 2)
        self.welcome_window.geometry(f"{window_width}x{window_height}+{position_right}+{position_down}")

        # Schedule window to close after 5 seconds
        self.welcome_window.after(2500, self.close_welcome)

    def close_welcome(self):
        
        self.welcome_window.destroy()
        # After closing welcome window, create main application window
        self.welcome.deiconify()
        main_window = HauptFenster(self.welcome)

class HauptFenster:
    def __init__(self, master):
        self.master=master
        master.title("MTM_TEDI")
        master.geometry("1200x800")
        master.configure(bg="#202020")
        master.attributes('-fullscreen', winconfig["fullscreen"])
        self.font_type="Helvetica"
        self.font_size=13
        self.font_color="#facf0e"
        self.bg_color="#202020"

        self.links_frame = tk.Frame(master, bg=self.bg_color)
        self.links_frame.place(relx=0, rely=0.5, anchor=tk.W)
        self.center= tk.Frame(master, bg=self.bg_color)
        self.center.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        self.rechts_frame = tk.Frame(master, bg=self.bg_color)
        self.rechts_frame.place(relx=1.0, rely=0.5, anchor=tk.E)
        
        image = Image.open(imgpaths+"mtmlogo.png")
        width, height = 400, 400
        image = image.resize((width, height))  
        photo = ImageTk.PhotoImage(image)
        img_label = Label(self.center, image=photo, bg=self.bg_color)
        img_label.grid(row=0, column=0, padx=10, pady=20)
        img_label.image_ = photo
        
        image_einfuegenbutton = Image.open(imgpaths+"button1.png")
        self.photo10 = ImageTk.PhotoImage(image_einfuegenbutton)  
        button_Add_Win = Button(self.center, image=self.photo10, command= self.tool, bd=0, bg=self.bg_color,highlightthickness=0, activebackground=self.bg_color)
        button_Add_Win.grid(row=1, column=0, padx=50, pady=10)

        image_suche_maschine_button = Image.open(imgpaths+"button6.png")
        photo_suche_maschine_button = ImageTk.PhotoImage(image_suche_maschine_button)
        button_Suche = Button(self.center, image=photo_suche_maschine_button, command= self.maschinewz, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_Suche.image = photo_suche_maschine_button  # Prevent garbage collection
        button_Suche.grid(row=2, column=0, padx=50, pady=10)
      
        image_register_button = Image.open(imgpaths+"button2.png")
        photo_register_button = ImageTk.PhotoImage(image_register_button)
        button_register = Button(self.center, image=photo_register_button, command=self.aufnahme, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_register.image = photo_register_button  # Prevent garbage collection
        button_register.grid(row=4, column=0, padx=50, pady=10)
       
        image_machines_button = Image.open(imgpaths+"button4.png")
        photo_machines_button = ImageTk.PhotoImage(image_machines_button)
        button_machines = Button(self.center, image=photo_machines_button, command=self.list, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_machines.image = photo_machines_button  # Prevent garbage collection
        button_machines.grid(row=5, column=0, padx=50, pady=10)
        
        image_start_button = Image.open(imgpaths+"button3.png")
        photo_start_button = ImageTk.PhotoImage(image_start_button)
        button_start = Button(self.center, image=photo_start_button, command=self.auftrag_werkzeugsliste, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_start.image = photo_start_button  # Prevent garbage collection
        button_start.grid(row=6, column=0, padx=50, pady=10)

        image_start_button = Image.open(imgpaths+"button5.png")
        photo_start_button = ImageTk.PhotoImage(image_start_button)
        button_start = Button(self.center, image=photo_start_button, command=self.toolMeasure, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_start.image = photo_start_button  # Prevent garbage collection
        button_start.grid(row=7, column=0, padx=50, pady=10)

        image_setting_button = Image.open(imgpaths+"homebutton.png")
        photo_setting_button = ImageTk.PhotoImage(image_setting_button)
        button_setting = Button(master, image=photo_setting_button, command=self.home, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_setting.image = photo_setting_button  # Prevent garbage collection
        button_setting.place(relx=1.0, rely=0.0, anchor="ne", x=-150, y=10)

        image_setting_button = Image.open(imgpaths+"settingsbutton.png")
        photo_setting_button = ImageTk.PhotoImage(image_setting_button)
        button_setting = Button(master, image=photo_setting_button, command=self.settingwin, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_setting.image = photo_setting_button  # Prevent garbage collection
        button_setting.place(relx=1.0, rely=0.0, anchor="ne", x=-80, y=10)

        image_close_button = Image.open(imgpaths+"closebutton.png")
        photo_close_button = ImageTk.PhotoImage(image_close_button)
        button_close = Button(master, image=photo_close_button, command=lambda: self.cleanExit(master), bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_close.image = photo_close_button  # Prevent garbage collection
        button_close.place(relx=1.0, rely=0.0, anchor="ne", x=-10, y=10)

        master.bind("<Escape>", lambda e: master.quit())
                # Start the database update thread
        # threading.Thread(target=self.callUpdate, daemon=True).start()



        
    def callUpdate(self):
        while True:
            self.databaseUpdate()

    def cleanExit(self,master):
        """Clean exit function to terminate the program"""
        # Destroy all open frames/windows
        for widget in master.winfo_children():
            widget.destroy()

        # Close the main window
        master.destroy()

        # Exit the program cleanly
        sys.exit(0)

    def tool(self):
        print("komplette_werkzeug")
        from toolModule import toolModule as ToolWin
        
        # Clear the center frame before loading new content
        for widget in self.center.winfo_children():
            widget.destroy()
        
        # Initialize the AddToolWindow class with the center frame
        add_tool = ToolWin(self.center)


    def maschinewz(self):
        print("maschinewz")



    def aufnahme(self):
        from holderModule import holderModule as HolderWin

        print("komplette Aufnahme")
        
        # Clear the center frame before loading new content
        for widget in self.center.winfo_children():
            widget.destroy()
        
        # Initialize the AddToolWindow class with the center frame
        addHolder = HolderWin(self.center)





    def list(self):
        print("zeig_liste")
        from listModule import listModule as ListWin


        
        # Clear the center frame before loading new content
        for widget in self.center.winfo_children():
            widget.destroy()
        
        # Initialize the AddToolWindow class with the center frame
        List = ListWin(self.center)


    def auftrag_werkzeugsliste(self):
        from startOrder import startOrder as startOrderWin

        for widget in self.center.winfo_children():
            widget.destroy()
        
        # Initialize the AddToolWindow class with the center frame
        startOrder = startOrderWin(self.center)


    def toolMeasure(self):
        import tkinter.font as font


        print("Messen")

        # Clear the frame before adding new widgets
        self.clearFrame()

        # Create a new frame inside the mainFrame with a specific size
        # self.centerFrame = tk.Frame(self.master, bg=winconfig["bgcolor"])
        # self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)

        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")
        labelCode = Label(self.center, text='QR_CODE',font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        labelCode.grid(row=0, column=0, padx=10, pady=5, sticky='e')
        self.iDCode = StringVar()
        self.iDCodeEntry = Entry(self.center, textvariable=self.iDCode,font=(winconfig["bgcolor"], winconfig["fontsize"]))
        self.iDCodeEntry.grid(row=0, column=1, padx=10, pady=5, sticky='w')
        self.iDCodeEntry.focus_set()

        # Submit button to check QR_Code
        self.checkCodeButton = Button(self.center, text="Prüfen", command= lambda: self.measureModule(self.iDCodeEntry.get()),font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        self.checkCodeButton.grid(row=0, column=2, padx=10, pady=5, sticky='w')
        self.iDCodeEntry.bind('<Return>', lambda event: self.checkCodeButton.invoke())


    def measureModule(self,iDEntry):
        # import csv
        import tkinter.messagebox as messagebox
        found=False
        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        
        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        for tool in currentToolsData:
            if tool[0]==iDEntry:
                found=True
                break
                
            else:
                found=False


        # with open(paths["machinecsv"],"r") as machineCsvReader:
        #     machineCsvReader=csv.reader(machineCsvReader, delimiter=";")
        #     next(machineCsvReader)
        #     for row in machineCsvReader:
        #         if row[0]==iDEntry:
        #             found=True
        #             break
                    
        #         else:
        #             found=False

        if found==True:
            from haimerInterface import HaimerInterface as HaimerInterFace

            for widget in self.center.winfo_children():
                widget.destroy()
            
            # Initialize the AddToolWindow class with the center frame
            HaimerInterface = HaimerInterFace(self.center,iDEntry)


        else:
                messagebox.showerror("NOT FOUND","NOT FOUND")
                return


    def home(self):
        for widget in self.center.winfo_children():
            widget.destroy()
        self.center.destroy()
        self.rechts_frame.destroy()
        self.links_frame.destroy()
        self.__init__(self.master)


    def settingwin(self):
        from settings import Settings as Settings
        settingsWin=Settings()



    def close(self):
        print("close")


    def clearFrame(self):
        if self.center:
            for widget in self.center.winfo_children():
                widget.destroy()
            # self.center.destroy()





    def databaseUpdate(self):
        def monitor_output(process, output_queue):
            """
            Continuously read the output of the process and add it to the queue.
            """
            try:
                for line in iter(process.stdout.readline, ''):
                    output_queue.put(line.strip())
            except Exception as e:
                print(f"Error reading process output: {e}")

        for place in places:
            if place["status"] == "machine":
                ip_address = place["link"]
                place_name = place["placename"]
                # Path to the TNCCmd executable
                tnc_cmd_path = paths["TNCcmd"]  # Replace with the actual path

                try:
                    print(f"Connecting with {place_name} with Ip Address : {ip_address}.....")

                    # Start the TNCCmd process
                    process = subprocess.Popen(
                        tnc_cmd_path,
                        stdin=subprocess.PIPE,
                        stdout=subprocess.PIPE,
                        stderr=subprocess.PIPE,
                        text=True
                    )

                    # Allow the program to start
                    time.sleep(1)

                    # Monitor process output using a queue
                    output_queue = Queue()
                    output_thread = threading.Thread(target=monitor_output, args=(process, output_queue))
                    output_thread.daemon = True
                    output_thread.start()

                    # Send the 'connect' command
                    process.stdin.write("connect\n")
                    process.stdin.flush()
                    time.sleep(1)

                    # Send the 'i' command
                    process.stdin.write("i\n")
                    process.stdin.flush()
                    time.sleep(1)

                    # Send the IP address
                    process.stdin.write(f"{ip_address}\n")
                    process.stdin.flush()
                    time.sleep(1)

                    start_time = time.time()
                    connected = False

                    while time.time() - start_time < 5:  # Wait up to 5 seconds
                        try:
                            line = output_queue.get(timeout=0.1)
                            print(line)  # Print output for debugging purposes

                            if "Connection established" in line:
                                connected = True
                                print(f"Connection to {place_name} with Ip Address: {ip_address} established.")
                                break
                            elif "abort with ESC!" in line:
                                print(f"Connection to {place_name} with Ip Address: {ip_address} in progress...")
                        except Empty:
                            continue

                    if not connected:
                        print(f"Connection to {ip_address} timed out. Aborting...")
                        process.stdin.write("\x1b\n")  # Send ESC key
                        process.stdin.flush()
                        process.terminate()
                        continue

                    # Proceed with file operations if connected
                    print(f"Successfully connected to {place_name} with Ip Address: {ip_address}. Proceeding with file operations...")

                    # Get tool.t
                    process.stdin.write(f"get TNC:\\table\\tool.t X:\\Projekt\\temp\\{ip_address}.t\n")
                    process.stdin.flush()
                    time.sleep(1)

                    # Get tool_p.tch
                    process.stdin.write(f"get TNC:\\table\\tool_p.tch X:\\Projekt\\temp\\{ip_address}.tch\n")
                    process.stdin.flush()
                    time.sleep(1)

                    # Close the stdin to signal end of input
                    process.stdin.close()

                    # Read and print the output (optional)
                    output, error = process.communicate()
                    # print("Output:\n", output)
                    print("Error:\n", error if error else "No errors.")

                except FileNotFoundError:
                    print("Error: TNCCmd executable not found. Please check the path.")
                    break
                except Exception as e:
                    print(f"An error occurred: {e}")
                    break


                # Path to the generated .tch file
                tch_file_path = f"X:\\Projekt\\temp\\{ip_address}.tch"

                # List to store the extracted data from .tch
                tchData = []

                try:
                    # Open the .tch file for reading
                    with open(tch_file_path, 'r') as tch_file:
                        # Skip the first two lines
                        next(tch_file)
                        next(tch_file)

                        # Process the remaining lines
                        for line in tch_file:
                            # Extract columns based on fixed widths
                            tchCol1 = line[:8].strip()  # First column (ignored)
                            tchCol2 = line[8:14].strip()  # Second column
                            tchCol3 = line[14:46].strip()  # Third column

                            # Skip lines where the third column is empty
                            if tchCol3:
                                tchData.append((tchCol2, tchCol3))

                    print("Extracted Data from .tch:", tchData ,"\n")

                except FileNotFoundError:
                    print(f"Error: File {tch_file_path} not found.")
                    break
                except Exception as e:
                    print(f"An error occurred: {e}")
                    break

                # Path to the generated .t file
                t_file_path = f"X:\\Projekt\\temp\\{ip_address}.t"

                # List to store the extracted data from .t
                tData = []

                try:
                    # Open the .t file for reading
                    with open(t_file_path, 'r') as t_file:
                        # Skip the first two lines
                        next(t_file)
                        next(t_file)

                        # Process the remaining lines
                        for line in t_file:
                            # Extract columns based on fixed widths
                            tCol1 = line[:8].strip()  # First column
                            tCol2 = line[8:40].strip()  # Second column
                            tCol3 = line[40:52].strip()  # Third column
                            tCol4 = line[52:64].strip()  # Fourth column

                            # Check if the first and second columns match any entry in tchData
                            if any(tCol1 == tch[0] and tCol2 == tch[1] for tch in tchData):
                                tData.append((tCol1, tCol2, tCol3, tCol4))

                    print("Extracted Data from .t:", tData)

                except FileNotFoundError:
                    print(f"Error: File {t_file_path} not found.")
                    break
                except Exception as e:
                    print(f"An error occurred: {e}")
                    break


                # Connect to the MTMDB.db SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()

                try:
                    counter=0
                    for t in tData:
                        toolNummer, toolName, toolIstLaenge, toolRadius = t

                        # Debugging: Check what we're comparing
                        # print(f"Comparing toolNummer: {toolNummer}, toolName: {toolName}, platz: {place_name}")

                        # Debugging: SELECT query to check existing rows
                        cur.execute("""
                            SELECT * FROM currentTools
                            WHERE LOWER(TRIM(toolNummer)) = LOWER(?) 
                            AND LOWER(TRIM(toolName)) = LOWER(?) 
                            AND LOWER(TRIM(platz)) = LOWER(?)
                        """, (toolNummer.strip(), toolName.strip(), place_name.strip()))

                        rows = cur.fetchall()
                        if rows:
                            print(f"Matching rows for {toolNummer}, {toolName}, {place_name}: {rows}")
                            counter=counter+1
                        else:
                            # print(f"No matching rows for {toolNummer}, {toolName}, {place_name}")
                            pass

                        # Execute the UPDATE query
                        cur.execute("""
                            UPDATE currentTools
                            SET toolRadius = ?,
                                toolIstLaenge = ?
                            WHERE LOWER(TRIM(toolNummer)) = LOWER(?) 
                            AND LOWER(TRIM(toolName)) = LOWER(?) 
                            AND LOWER(TRIM(platz)) = LOWER(?)
                        """, (toolRadius, toolIstLaenge, toolNummer.strip(), toolName.strip(), place_name.strip()))

                    conn.commit()
                    print(f"Updated {counter} rows in the database for {place_name}.")
                except sqlite3.Error as e:
                    print(f"An error occurred while updating the database: {e}")

                finally:
                    # Close the connection
                    conn.close()

            else:
                pass


if __name__ == "__main__":
    root = tk.Tk()
    root.withdraw()  # Hide the main root window
    welcome_window = WelcomeWindow(root)
    root.mainloop()