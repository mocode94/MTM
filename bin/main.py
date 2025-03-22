from tkinter import Toplevel, Label, Entry, Button, StringVar,simpledialog,messagebox
import tkinter as tk
from PIL import Image, ImageTk
import sys
import subprocess
import time
import threading
from queue import Queue, Empty
from config import paths,places,settings,imgpaths,winconfig
import sqlite3
from datetime import datetime
import os
import socket
import logging



# _________________________________________________________________________


class WelcomeWindow:
    def __init__(self, master):
        self.welcome = master
        self.welcome_window = Toplevel(master)
        self.welcome_window.overrideredirect(True)  # Remove title bar
        self.welcome_window.configure(bg=winconfig["bgcolor"])
        self.userID = None
        self.checkResult=True
        self.checkListe=[]

        # Load and resize the image
        image_path = f"{imgpaths}mtmlogo.png"
        image = Image.open(image_path)
        resized_image = image.resize((300, 300))
        self.photo = ImageTk.PhotoImage(resized_image)

        # Create label to display image
        image_label = Label(self.welcome_window, image=self.photo,bg=winconfig["bgcolor"])
        image_label.pack()

        # Create a single status label (it will be updated dynamically)
        self.status_label = Label(self.welcome_window, text="", font=(winconfig["fonttype"],winconfig["fontsize"]-2), bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.status_label.pack()

        # Center the window
        screen_width = self.welcome_window.winfo_screenwidth()
        screen_height = self.welcome_window.winfo_screenheight()
        window_width = resized_image.width
        window_height = resized_image.height + 50  # Extra space for label
        position_right = int(screen_width / 2 - window_width / 2)
        position_down = int(screen_height / 2 - window_height / 2)
        self.welcome_window.geometry(f"{window_width}x{window_height}+{position_right}+{position_down}")

        # Ensure the logs directory exists
        log_dir = os.path.join("..", "logs")
        os.makedirs(log_dir, exist_ok=True)  # Creates the directory if it doesn't exist

        # Configure logging
        log_file = os.path.join(log_dir, "MTMLOGS.log")
        logging.basicConfig(
            filename=log_file,
            level=logging.DEBUG,  
            format="%(asctime)s - %(levelname)s - %(message)s",
            datefmt="%Y-%m-%d %H:%M:%S"
        )
        logging.getLogger("PIL").setLevel(logging.INFO)

        # Start checking paths
        self.path_keys = list(paths.keys())  # Store keys for iteration
        self.current_index = 0  # Track which path is being checked
        self.welcome_window.after(1, self.MTMLoad)  # Start checking

    def MTMLoad(self):
        """Check paths one by one, updating a single label dynamically."""
        if self.current_index < len(self.path_keys):
            key = self.path_keys[self.current_index]
            path = paths[key]
            current_ip = socket.gethostbyname(socket.gethostname())  # Get the current IP of the PC


            if key == "pcip":
                if path == current_ip:
                    self.status_label.config(text=f"{key} ✅", fg=winconfig["fontcolor"])
                    logging.info(f"{key} Path found: {path}")

                    self.checkListe.append(f"{key} ✅")
                else:
                    self.status_label.config(text=f"{key} ❌", fg="red")
                    # self.checkResult = False  # Prevent further execution
                    logging.warning(f"{key} Path not found: {path}")

                    # messagebox.showwarning("Error",f"{key} Path not found: {path}")
                    self.checkListe.append(f"{key} ❌")
                    
                    # self.welcome_window.destroy()  # Close the welcome window
                    # self.welcome.quit()  # Exit the program
                    # return  # Stop execution
            elif os.path.exists(path):
                self.status_label.config(text=f"{key} ✅", fg=winconfig["fontcolor"])
                logging.info(f"{key} Path found: {path}")

                self.checkListe.append(f"{key} ✅")

            else:
                self.status_label.config(text=f"{key} ❌", fg="red")
                # self.checkResult = False  # Prevent further execution
                logging.warning(f"{key} Path not found: {path}")

                # messagebox.showwarning("Error",f"{key} Path not found: {path}")
                self.checkListe.append(f"{key} ❌")
                # self.welcome_window.destroy()  # Close the welcome window
                # self.welcome.quit()  # Exit the program
                # return  # Stop execution
            self.current_index += 1  # Move to the next path
            self.welcome_window.after(100, self.MTMLoad)  # Schedule next check

        elif self.checkResult:
            logging.info("Application started")
            # If all paths are valid, close the window after 5 seconds
            self.welcome_window.after(1, self.close_welcome)
            



    def center_window(self,window, width=300, height=200):
        """Centers a given Tkinter window on the screen."""
        screen_width = window.winfo_screenwidth()
        screen_height = window.winfo_screenheight()
        x = (screen_width // 2) - (width // 2)
        y = (screen_height // 2) - (height // 2)
        window.geometry(f"{width}x{height}+{x}+{y}")

    def ask_id(self):
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()   
        cur.execute("SELECT * FROM Users")  
        users = cur.fetchall()
        conn.close()
        userID = [iDs[2] for iDs in users ] + [iDs[0] for iDs in users]  # Extract user IDs
  
        # Ensure `self.welcome` exists and is centered
        self.welcome.withdraw()  # Hide the main window temporarily
        self.center_window(self.welcome)

        while True:
            user_id = simpledialog.askstring(
                "ID", 
                "Geben Sie Ihre ID ein:", 
                parent=self.welcome, 
                show='*'
            )
            self.userID = user_id

            if user_id is None:  # User pressed cancel
                self.welcome.deiconify()  # Show the main window again
                return
            if user_id in userID:
                self.lastIn(userID)
                self.welcome.deiconify()
                HauptFenster(self.welcome, user_id, self.checkListe)  # Start the main application
                break
            else:
                messagebox.showerror("Access Denied", "Falsche ID. Versuchen Sie nochmal.", parent=self.welcome)



    def lastIn(self,userID):

        conn = sqlite3.connect(paths["MTMDB"])
        cursor = conn.cursor()

        # Get the current timestamp
        now = datetime.now().strftime("%d.%m.%Y %H:%M:%S")

        # Ensure userID is a string
        if isinstance(userID, list):
            userID = userID[0]  # Take the first element if it's a list

        cursor.execute("""
            UPDATE Users
            SET lastIn = ?
            WHERE LOWER(TRIM(ID)) = LOWER(?)
        """, (now, str(userID)))  # Convert userID to a string

        conn.commit()
        conn.close()
        logging.info(f"User {userID} logged in at {now}")

    def close_welcome(self):
        
        self.welcome_window.destroy()
        # After closing welcome window, create main application window
        self.welcome.deiconify()
        self.ask_id()  # Ask for ID after welcome window closes

        # main_window = HauptFenster(self.welcome)



class HauptFenster:
    def __init__(self, master,userID,checkListe):
        self.checkListe=checkListe  
        self.userID=userID
        self.master=master
        master.title("MTM")
        master.geometry("1200x800")
        master.configure(bg="#202020")
        master.attributes('-fullscreen', winconfig["fullscreen"])
        self.font_type="Helvetica"
        self.font_size=13
        self.font_color="#facf0e"
        self.bg_color="#202020"
        self.inactivity_time = 120000  # 1 minute (in milliseconds)
        # self.timer = master.after(self.inactivity_time, self.noMotion)  # Start timer
        # self.master.bind("<Motion>", self.reset_timer)  # Detect mouse movement
        # self.bind_motion_events()


        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        # Fetch all data from the Users table
        cur.execute("SELECT * FROM Users")
        users = cur.fetchall()
        # Close the database connection
        conn.close()

        for user in users:
            if user[2] == self.userID or user[0] == self.userID:
                self.userCode = user[0]
                profileImage = user[6]  # Keep `user` unchanged to avoid errors
                self.UserRights=user[5]
                break

        if checkListe != None:
            self.checkListeListe(checkListe)

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

        labelUserName = Label(master, text="Hallo "+str(user[1]), font=(self.font_type, self.font_size+3,"bold"), bg=self.bg_color, fg=self.font_color)
        labelUserName.place(relx=1.0, rely=0.0, anchor="ne", x=-320, y=22)
        try:
            image_setting_button = Image.open(profileImage)
            photo_setting_button = ImageTk.PhotoImage(image_setting_button)
            button_setting = Button(master, image=photo_setting_button, command=self.home, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
            button_setting.image = photo_setting_button  # Prevent garbage collection
            button_setting.place(relx=1.0, rely=0.0, anchor="ne", x=-260, y=10)
        except:
            image_setting_button = Image.open(paths["imgpaths"]+"/users/defaultuser.png")
            photo_setting_button = ImageTk.PhotoImage(image_setting_button)
            button_setting = Button(master, image=photo_setting_button, command=self.home, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
            button_setting.image = photo_setting_button  # Prevent garbage collection
            button_setting.place(relx=1.0, rely=0.0, anchor="ne", x=-260, y=10)

        image_setting_button = Image.open(imgpaths+"homebutton.png")
        photo_setting_button = ImageTk.PhotoImage(image_setting_button)
        button_setting = Button(master, image=photo_setting_button, command=self.home, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_setting.image = photo_setting_button  # Prevent garbage collection
        button_setting.place(relx=1.0, rely=0.0, anchor="ne", x=-200, y=10)


        image_setting_button = Image.open(imgpaths+"logoutbutton.png")
        photo_setting_button = ImageTk.PhotoImage(image_setting_button)
        button_setting = Button(master, image=photo_setting_button, command=self.noMotion, bd=0, bg=self.bg_color, highlightthickness=0, activebackground=self.bg_color)
        button_setting.image = photo_setting_button  # Prevent garbage collection
        button_setting.place(relx=1.0, rely=0.0, anchor="ne", x=-140, y=10)

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
                # Check if autodataupdate is true

        # if settings["autodataupdate"] == True:
        #     threading.Thread(target=self.callUpdate, daemon=True).start()
        # # threading.Thread(target=self.callUpdate, daemon=True).start()
        self.checkautodataupdate()



    def bind_motion_events(self):
        """ Bind <Motion> event to all windows (Master + Toplevels) """
        self.master.bind("<Motion>", self.reset_timer)  # Bind to main window
        
        # Bind motion event to all existing and future Toplevel windows
        self.track_toplevels()

    def track_toplevels(self):
        """ Bind motion detection to all open Toplevel windows """
        for window in self.master.winfo_children():
            if isinstance(window, tk.Toplevel):  # Check if it's a Toplevel window
                window.bind("<Motion>", self.reset_timer)

        # Schedule the function to run again (to detect new Toplevels)
        self.master.after(1000, self.track_toplevels) 

    def checkListeListe(self, checkListe):

        checkListeListe = Toplevel()
        checkListeListe.title("Check Liste")

        # Window dimensions
        width = 400
        height = 400

        # Get screen width and height
        screen_width = checkListeListe.winfo_screenwidth()
        screen_height = checkListeListe.winfo_screenheight()

        # Calculate position x, y to center the window
        x = (screen_width // 2) - (width // 2)
        y = (screen_height // 2) - (height // 2)

        # Set window size and position
        checkListeListe.geometry(f"{width}x{height}+{x}+{y}")
        # Create text widget
        text_widget = tk.Text(checkListeListe, width=50, height=20)
        text_widget.pack(pady=10, padx=10, fill=tk.BOTH, expand=True)

        # Define tag styles
        text_widget.tag_configure("green", background="lightgreen")
        text_widget.tag_configure("red", background="lightcoral")

        has_red = False  # Flag to check if there's a ❌ row

        for item in checkListe:
            if "✅" in item:
                text_widget.insert(tk.END, item + "\n", "green")
            elif "❌" in item:
                text_widget.insert(tk.END, item + "\n", "red")
                has_red = True  # Mark that we have a red row
            else:
                text_widget.insert(tk.END, item + "\n")

        text_widget.config(state=tk.DISABLED)  # Make it read-only

        # Frame for buttons
        button_frame = tk.Frame(checkListeListe)
        button_frame.pack(pady=10)

        if has_red:
            # Button: "Trotzdem Weiter" (closes the window)
            weiter_button = tk.Button(button_frame, text="Trotzdem Weiter", command=checkListeListe.destroy)
            weiter_button.pack(side=tk.LEFT, padx=10)

            # Button: "Abbrechen" (calls self.cleanClose)
            abbrechen_button = tk.Button(button_frame, text="Abbrechen", command=lambda:self.cleanExit(self.master))
            abbrechen_button.pack(side=tk.RIGHT, padx=10)
        else:
            # Show only "OK" button if everything is ✅
            ok_button = tk.Button(button_frame, text="OK", command=checkListeListe.destroy, width=15)
            ok_button.pack(expand=True, fill=tk.X, padx=10)
        self.checkListe=None

    def checkautodataupdate(self):
        if settings["autodataupdate"] == True:
            threading.Thread(target=self.callUpdate, daemon=True).start()
    def callUpdate(self):
        while True:
            self.databaseUpdate()

    def cleanExit(self,master):
        """Clean exit function to terminate the program"""
        # Destroy all open frames/windows
        for widget in master.winfo_children():
            widget.destroy()
        self.lastOut()
        # Close the main window
        master.destroy()

        # Exit the program cleanly
        sys.exit(0)


    def lastOut(self):
        conn = sqlite3.connect(paths["MTMDB"])
        cursor = conn.cursor()

        # Get the current timestamp
        now = datetime.now().strftime("%d.%m.%Y %H:%M:%S")

        # Ensure userID is a string
        if isinstance(self.userID, list):
            self.userID = self.userID[0]  # Take the first element if it's a list

        cursor.execute("""
            UPDATE Users
            SET lastOut = ?
            WHERE LOWER(TRIM(ID)) = LOWER(?)
        """, (now, str(self.userID)))  # Convert userID to a string
        conn.commit()
        conn.close()
        logging.info(f"User {self.userID} logged out at {now}")


    def noMotion(self):
        self.lastOut()
        self.logInWindow()

    def logInWindow(self):
                # Destroy all open frames/windows
        for widget in self.master.winfo_children():
            widget.destroy()

        self.logInFrame = tk.Frame(self.master,bg=winconfig["bgcolor"])
        self.logInFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)

        # Load and resize the image (replace with your image path)
        image_path = f"{imgpaths}mtmlogo.png"
        image = Image.open(image_path)
        resized_image = image.resize((300, 300))
        self.photo = ImageTk.PhotoImage(resized_image)

        # Create label to display image
        label = Label(self.logInFrame, image=self.photo)    
        label.pack()

        self.UserIDEntryValue = StringVar()
        self.UserIDEntry = Entry(self.logInFrame,font=(winconfig["bgcolor"], winconfig["fontsize"]),show="*")
        self.UserIDEntry.pack(padx=10, pady=5)
        self.UserIDEntry.focus_set()

        self.button1 = Button(self.logInFrame, text="Login",  command=self.askUserForID,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        self.button1.pack(padx=10, pady=5)
        self.UserIDEntry.bind('<Return>', lambda event: self.button1.invoke())
        logging.info("User logged out due to inactivity")
    def reset_timer(self, event=None):
        """Reset inactivity timer when mouse moves."""
        self.master.after_cancel(self.timer)  # Cancel previous timer
        self.timer = self.master.after(self.inactivity_time, self.logInWindow)  # Restart timer


    def askUserForID(self):
        userIDEntry=self.UserIDEntry.get()
        conn= sqlite3.connect(paths["MTMDB"])
        cur=conn.cursor()   
        cur.execute("SELECT * FROM Users")  
        users = cur.fetchall()
        conn.close()
        userID=[]
        for iDs in users:
            userID.append(iDs[2]) 
            userID.append(iDs[0]) 


        if userIDEntry is None:  # User pressed cancel
            return
        if userIDEntry in userID:
            self.lastIn(userIDEntry)
            # self.welcome.deiconify()
            self.logInFrame.destroy()
            HauptFenster(self.master,userIDEntry,self.checkListe)  # Start the main application
            # self.__init__(self.master,userIDEntry) 
            
        else:
            messagebox.showerror("Access Denied", "falshe ID. Versuchen Sie nochmal.")


    def lastIn(self,userID):

        conn = sqlite3.connect(paths["MTMDB"])
        cursor = conn.cursor()

        # Get the current timestamp
        now = datetime.now().strftime("%d.%m.%Y %H:%M:%S")

        # Ensure userID is a string
        if isinstance(userID, list):
            userID = userID[0]  # Take the first element if it's a list

        cursor.execute("""
            UPDATE Users
            SET lastIn = ?
            WHERE LOWER(TRIM(ID)) = LOWER(?)
        """, (now, str(userID)))  # Convert userID to a string

        conn.commit()
        conn.close()
        logging.info(f"User {userID} logged in at {now}")
    def tool(self):
        print("komplette_werkzeug")
        from toolModule import toolModule as ToolWin
        
        # Clear the center frame before loading new content
        for widget in self.center.winfo_children():
            widget.destroy()
        
        # Initialize the AddToolWindow class with the center frame
        add_tool = ToolWin(self.center,self.userCode)


    def maschinewz(self):
        print("maschinewz")


    def aufnahme(self):
        from holderModule import holderModule as HolderWin

        print("komplette Aufnahme")
        
        # Clear the center frame before loading new content
        for widget in self.center.winfo_children():
            widget.destroy()
        
        # Initialize the AddToolWindow class with the center frame
        addHolder = HolderWin(self.center,self.UserRights)


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
        conn.close()
        for tool in currentToolsData:
            if tool[0]==iDEntry:
                found=True
                break
                
            else:
                found=False



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
        self.__init__(self.master,self.userID,checkListe=None)


    def settingwin(self):
        from settings import Settings as Settings
        settingsWin=Settings(self.UserRights)



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

                    # # Start the TNCCmd process
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

                    # # Get tool.t
                    # process.stdin.write(f"get TNC:\\table\\tool.t {paths["temp"]}{ip_address}.t\n")
                    # process.stdin.flush()
                    # time.sleep(1)



                    # Get tool.t
                    process.stdin.write(f"get TNC:\\table\\tool.t {paths['temp']}{ip_address}.t\n")
                    process.stdin.flush()

                    # Wait for the command to execute completely
                    output, error = process.communicate()

                    print("Output:\n", output if output else "No output.")
                    print("Error:\n", error if error else "No errors.")

                    # Ensure the process is properly closed
                    process.wait()


                    # # Get tool_p.tch
                    # process.stdin.write(f"get TNC:\\table\\tool_p.tch {paths["temp"]}{ip_address}.tch\n")
                    # process.stdin.flush()
                    # time.sleep(1)



                    # Get tool_p.tch
                    process.stdin.write(f"get TNC:\\table\\tool_p.tch {paths["temp"]}{ip_address}.tch\n")
                    process.stdin.flush()

                    # Wait for the command to execute completely
                    output, error = process.communicate()

                    print("Output:\n", output if output else "No output.")
                    print("Error:\n", error if error else "No errors.")

                    # Ensure the process is properly closed
                    process.wait()




                    # Close the stdin to signal end of input
                    process.stdin.close()

                    # Read and print the output (optional)
                    output, error = process.communicate()
                    # print("Output:\n", output)
                    print("Error:\n", error if error else "No errors.")

                except FileNotFoundError:
                    print("Error: TNCCmd executable not found. Please check the path.")
                    if process:
                        process.terminate()  # Ensure process is closed
                except Exception as e:
                    print(f"An error occurred: {e}")
                    if process:
                        process.terminate()  # Ensure process is closed

                finally:
                    if process:
                        try:
                            process.terminate()  # Ensure process is fully closed
                        except Exception:
                            pass  # Ignore errors if the process is already terminated

                # Path to the generated .tch file
                tch_file_path = paths["temp"]+ip_address+".tch"

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


                except FileNotFoundError:
                    print(f"Error: File {tch_file_path} not found.")
                    break
                except Exception as e:
                    print(f"An error occurred: {e}")
                    break

                # Path to the generated .t file
                t_file_path =paths["temp"]+ip_address+".t"

                # List to store the extracted data from .t
                tData = []
                CUR_TIME_StartPosition=int()
                CUR_TIME_EndPosition=int()

                try:
                    # Open the .t file for reading
                    with open(t_file_path, 'r', encoding="utf-8") as t_file:

                        lines = t_file.readlines()
                        if len(lines) > 1:
                            second_line = lines[1]  # Read the second line
                            CUR_TIME_StartPosition = second_line.find("CUR_TIME")  # Find "CUR_TIME"

                            if CUR_TIME_StartPosition != -1:
                                CUR_TIME_EndPosition = CUR_TIME_StartPosition + len("CUR_TIME")  # Move to the end of "CUR_TIME"
                                
                                # Skip spaces
                                while CUR_TIME_EndPosition < len(second_line) and second_line[CUR_TIME_EndPosition] == " ":
                                    CUR_TIME_EndPosition += 1


                                if CUR_TIME_EndPosition < len(second_line):
                                    print(f'"CUR_TIME" starts at position: {CUR_TIME_StartPosition}')
                                    print(f'The next word starts at position: {CUR_TIME_EndPosition}')
                                else:
                                    print("No next word found after CUR_TIME.")
                            else:
                                print('"CUR_TIME" not found in the second line.')

                            LCUTS_StartPosition = second_line.find("LCUTS")  # Find "CUR_TIME"

                            if LCUTS_StartPosition != -1:
                                LCUTS_EndPosition = LCUTS_StartPosition + len("LCUTS")  # Move to the end of "CUR_TIME"
                                
                                # Skip spaces
                                while LCUTS_EndPosition < len(second_line) and second_line[LCUTS_EndPosition] == " ":
                                    LCUTS_EndPosition += 1


                                if LCUTS_EndPosition < len(second_line):
                                    print(f'"LCUTS" starts at position: {LCUTS_StartPosition}')
                                    print(f'The next word starts at position: {LCUTS_EndPosition}')
                                else:
                                    print("No next word found after CUR_TIME.")
                            else:
                                print('"LCUTS" not found in the second line.')


                            R_OFFS_StartPosition = second_line.find("R-OFFS")  # Find "CUR_TIME"

                            if R_OFFS_StartPosition != -1:
                                R_OFFS_EndPosition = R_OFFS_StartPosition + len("R-OFFS")  # Move to the end of "CUR_TIME"
                                
                                # Skip spaces
                                while R_OFFS_EndPosition < len(second_line) and second_line[R_OFFS_EndPosition] == " ":
                                    R_OFFS_EndPosition += 1


                                if R_OFFS_EndPosition < len(second_line):
                                    print(f'"R-OFFS" starts at position: {R_OFFS_StartPosition}')
                                    print(f'The next word starts at position: {R_OFFS_EndPosition}')
                                else:
                                    print("No next word found after CUR_TIME.")
                            else:
                                print('"R-OFFS" not found in the second line.')



                            L_OFFS_StartPosition = second_line.find("L-OFFS")  # Find "CUR_TIME"

                            if L_OFFS_StartPosition != -1:
                                L_OFFS_EndPosition = L_OFFS_StartPosition + len("L-OFFS")  # Move to the end of "CUR_TIME"
                                
                                # Skip spaces
                                while L_OFFS_EndPosition < len(second_line) and second_line[L_OFFS_EndPosition] == " ":
                                    L_OFFS_EndPosition += 1


                                if L_OFFS_EndPosition < len(second_line):
                                    print(f'"L-OFFS" starts at position: {L_OFFS_StartPosition}')
                                    print(f'The next word starts at position: {L_OFFS_EndPosition}')
                                else:
                                    print("No next word found after CUR_TIME.")
                            else:
                                print('"L-OFFS" not found in the second line.')

                        else:
                            print("The file has less than two lines.")

                    with open(t_file_path, 'r') as t_file:
                        # Skip the first two lines
                        next(t_file)
                        next(t_file)
                        # Process the remaining lines
                        for line in t_file:
                            # Extract columns based on fixed widths
                            tCol1 = line[:8].strip()  # T-Nr.
                            tCol2 = line[8:40].strip()  # T-Name
                            tCol3 = line[40:52].strip()  # T-Länge
                            tCol4 = line[52:64].strip()  # T-Radius
                            tColCurTime=line[CUR_TIME_StartPosition:CUR_TIME_EndPosition].strip() # Current Time
                            tLCUTS=line[LCUTS_StartPosition:LCUTS_EndPosition].strip() # LCUTS
                            tROFFS=line[R_OFFS_StartPosition:R_OFFS_EndPosition].strip()
                            tLOFFS=line[L_OFFS_StartPosition:L_OFFS_EndPosition].strip()

                            # Check if the first and second columns match any entry in tchData
                            if any(tCol1 == tch[0] and tCol2 == tch[1] for tch in tchData):
                                tData.append((tCol1, tCol2, tCol3, tCol4,tColCurTime,tLCUTS,tROFFS,tLOFFS))


                except FileNotFoundError:
                    print(f"Error: File {t_file_path} not found.")
                    break
                except Exception as e:
                    print(f"An error occurred: {e}")
                    break


                # # Connect to the MTMDB.db SQLite database
                # conn = sqlite3.connect(paths["MTMDB"])
                # cur = conn.cursor()

                try:
                    counter=0
                    # Connect to the MTMDB.db SQLite database
                    conn = sqlite3.connect(paths["MTMDB"])
                    cur = conn.cursor()
                    for t in tData:
  
                        toolNummer, toolName, toolIstLaenge, toolRadius, currentTime,LCUTS,ROFFS,LOFFS = t

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
                                toolIstLaenge = ?,
                                toolSchnittLaenge = ?
                                    
                            WHERE LOWER(TRIM(toolNummer)) = LOWER(?) 
                            AND LOWER(TRIM(toolName)) = LOWER(?) 
                            AND LOWER(TRIM(platz)) = LOWER(?)
                        """, (toolRadius, toolIstLaenge, LCUTS,toolNummer.strip(), toolName.strip(), place_name.strip()))
                        
                        # Update `tnc640Data`
                        if rows:  # Only update if rows exist
                            cur.execute("""
                                UPDATE tnc640Data
                                SET CURTIME = ?,
                                    ROFFS=?,
                                    LOFFS=?
                                WHERE LOWER(TRIM(CODE)) = LOWER(?) 
                            """, (currentTime,ROFFS,LOFFS, rows[0][0]))  # Use rows[0][0] instead of rows[0].strip() to get the idCode

                    conn.commit()  # Commit the transaction first
                    cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                    print(f"Updated {counter} rows in the database for {place_name}.")
                    # Close the connection
                    conn.close()



                except sqlite3.Error as e:
                    print(f"An error occurred while updating the database: {e}")

                finally:
                    if conn:
                    # Close the connection
                      conn.close()






            else:
                pass


# if __name__ == "__main__":
#     root = tk.Tk()
#     root.withdraw()  # Hide the main root window
#     welcome_window = WelcomeWindow(root)
#     root.mainloop()
    


if __name__ == "__main__":
    root = tk.Tk()
    root.withdraw()  # Hide the main root window
    welcome_window = WelcomeWindow(root)
    root.mainloop()