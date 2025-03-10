from tkinter import  Button,Checkbutton,IntVar,font,Label
import tkinter as tk
from tkinter import ttk
from PIL import Image, ImageTk
import os
import tkinter.messagebox as messagebox
import json
import sys
import time
from config import winconfig, paths, places, settings
from addPlace import AddPlace as AddPlace
import subprocess
from tkinter import ttk, filedialog, messagebox
import sqlite3
#                                        Einstellungen
#==========================================================================================================
# Custom ToolTip class to show the info message on hover
class ToolTip:
    def __init__(self, widget, text):
        self.widget = widget
        self.text = text
        self.tooltip = None
        self.widget.bind("<Enter>", self.show_tooltip)
        self.widget.bind("<Leave>", self.hide_tooltip)
    
    def show_tooltip(self, event):
        # Position the tooltip near the cursor
        x, y, _, _ = self.widget.bbox("insert")
        self.tooltip = tk.Toplevel(self.widget)
        self.tooltip.wm_overrideredirect(True)
        self.tooltip.geometry(f"+{event.x_root + 20}+{event.y_root + 20}")  # Adjust position for tooltip

        label = tk.Label(self.tooltip, text=self.text, background="lightyellow", relief="solid", padx=5, pady=5)
        label.pack()

    def hide_tooltip(self, event):
        if self.tooltip:
            self.tooltip.destroy()
            self.tooltip = None


class Settings:
    def __init__(self,UserRights):

        self.setting = tk.Toplevel()
        self.setting.title("Tkinter Window with Tabs")
        self.setting.geometry("950x700")
        # self.setting.attributes("-topmost", True)
        # Create a style
        style = ttk.Style()
        style.configure('TNotebook', tabposition='nw')  # Position of tabs
        style.configure('TNotebook.Tab', 
                        font=(winconfig["fonttype"], winconfig["fontsize"]),  # Change font and size
                        background=winconfig["fontcolor"],  # Set background color for tabs
                        foreground=winconfig["bgcolor"],  # Set font color for tabs
                        padding=[10, 5])  # Adjust padding for the tabs
        


        self.UserRights=UserRights
        self.status="normal"
        if self.UserRights != "Admin":
            self.status="disabled"

        # Create a notebook (tab control)
        notebook = ttk.Notebook(self.setting, style='TNotebook')
        # Create frames for each tab
        self.tab1 = tk.Frame(notebook, background=winconfig["bgcolor"])
        self.tab2 = tk.Frame(notebook, background=winconfig["bgcolor"])
        self.tab3 = tk.Frame(notebook, background=winconfig["bgcolor"])
        self.tab4 = tk.Frame(notebook, background=winconfig["bgcolor"])

        # Add the frames to the notebook
        notebook.add(self.tab1, text='  Einstellungen  ')
        notebook.add(self.tab2, text='  System  ')
        notebook.add(self.tab3, text='  andere  ')
        notebook.add(self.tab4, text='  Über  ')

        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+2, weight="bold")

        # Load the image
        self.addPlaceButtonImage = Image.open(paths["imgpaths"]+"addplace.png")  # Update this with your image path
        self.addPlaceButtonImage = self.addPlaceButtonImage.resize((120, 120), Image.LANCZOS)  # Resize if needed
        self.addPlaceButtonImage = ImageTk.PhotoImage(self.addPlaceButtonImage)  # Convert to PhotoImage


        # Load the image
        self.addUserButtonImage = Image.open(paths["imgpaths"]+"usersbutton.png")  # Update this with your image path
        self.addUserButtonImage = self.addUserButtonImage.resize((120, 120), Image.LANCZOS)  # Resize if needed
        self.addUserButtonImage = ImageTk.PhotoImage(self.addUserButtonImage)  # Convert to PhotoImage

        # Create a button with an image
        self.addPlaceButton = Button(self.tab1, image=self.addPlaceButtonImage, command=self.open_add_place)
        self.addPlaceButton.grid(padx=20, pady=20, row=0, column=0)  # Adjust padding as needed

        # Create a button with an image
        self.addUserButton = Button(self.tab1, image=self.addUserButtonImage, command=self.userManager,state=self.status)
        self.addUserButton.grid(padx=20, pady=20, row=0, column=1)  # Adjust padding as needed



#                 # Load Image
#         try:
#             img = Image.open(paths["imgpaths"]+"mtmlogo.png")  # Ensure the image is in the same directory
#             img = img.resize((200, 200), Image.Resampling.LANCZOS)
#             img = ImageTk.PhotoImage(img)
            
#             img_label = Label(self.tab4, image=img)
#             img_label.image = img
#             img_label.pack(pady=10)
#         except Exception as e:
#             Label(self.tab4, text="Image not found", fg="red").pack(pady=10)
        
#         # About Info
#         about_text = """MTM - MoToolManager
# Version: 1.0.0
# Developer: Your Name
# Description: MTM helps CNC machinists manage tools efficiently.
# License: MIT
# """
#         Label(self.tab4, text=about_text, justify="left", font=("Arial", 12)).pack(pady=10)

#         # Close Button
#         tk.Button(self.tab4, text="Close", command=self.tab4.destroy).pack(pady=10)



        self.aboutTab()

        self.fullScreenCheckbox()
        self.autodataupdateCheckbox()   

        # Pack the notebook widget
        notebook.pack(expand=True, fill='both')
        
        self.setting.mainloop()

    def aboutTab(self):
        
                # Load Image
        try:
            img = Image.open(paths["imgpaths"]+"mtmlogo.png")  # Ensure the image is in the same directory
            img = img.resize((200, 200), Image.Resampling.LANCZOS)
            img = ImageTk.PhotoImage(img)
            
            img_label = Label(self.tab4, image=img)
            img_label.image = img
            img_label.pack(pady=10)
        except Exception as e:
            Label(self.tab4, text="Image not found", fg="red").pack(pady=10)
        
        # About Info
        about_text = """MTM - MoToolManager\n
        Version: 1.0.0\n
        Developer: Mo-Tools\n
        Description: MTM helps CNC machinists manage tools efficiently.\n
        E-Mail: mo_ab@outlook.de\n
        License: 00001111"""
        Label(self.tab4, text=about_text, justify="left", font=(winconfig["fonttype"], winconfig["fontsize"]+3,"bold"), bg=winconfig["bgcolor"], fg=winconfig["fontcolor"]).pack(pady=10)

        # Close Button
        tk.Button(self.tab4, text="Close", command=self.tab4.destroy).pack(pady=10)



    def open_add_place(self):
        """Method to open the AddPlace window."""
        add_place_window = AddPlace()  # Create an instance of AddPlace
    
    def fullScreenCheckbox(self):
        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+2, weight="bold")

            # Create a variable to store the checkbox state
        self.screenVar = IntVar()
        if winconfig["fullscreen"]==True:
            self.screenVar.set(1)   
        else    :
            self.screenVar.set(0)
        # Create the checkbox
 
        self.anzeigeButton = Checkbutton(self.tab2, text="Vollbild",variable=self.screenVar, command=self.fullscreen,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"],selectcolor="black")
        self.anzeigeButton.grid(padx=20, pady=5, row=0, column=1,sticky="w") 


        # Create the info icon (using ℹ️ for simplicity)
        self.info_icon = tk.Label(self.tab2, text="\u2753", font=("Helvetica", 11), fg="green",bg=winconfig["bgcolor"])  
        self.info_icon.grid(row=1, column=2, padx=2, pady=2, sticky="w")
        # Tooltip for the info icon
        self.tooltip = ToolTip(self.info_icon, "Wenn es aktiviert ist, wird die Datenbank mit den Maschinenwerkzeuglisten synchronisiert.")
    def fullscreen(self):

        if self.screenVar.get() == 1:
            # Load current config
            with open(paths["configJson"], "r") as f:
                config = json.load(f)

            # Update the fullscreen value
            config["winconfig"]["fullscreen"] = True

            # Save back to the JSON file
            with open(paths["configJson"], "w") as f:
                json.dump(config, f, indent=4)

            self.setting.attributes("-topmost", False)
            response = messagebox.askyesno("Info", "Möchten Sie das Programm jetzt neu starten?")
            if response:
                sys.exit()  # If 'Yes', exit the program
            else:
                self.setting.attributes("-topmost", True)
                self.setting.attributes("-topmost", False)

        else:
            # Load current config
            with open(paths["configJson"], "r") as f:
                config = json.load(f)

            # Update the fullscreen value
            config["winconfig"]["fullscreen"] = False

            # Save back to the JSON file
            with open(paths["configJson"], "w") as f:
                json.dump(config, f, indent=4)

            self.setting.attributes("-topmost", False)
            response = messagebox.askyesno("Info", "Möchten Sie das Programm jetzt neu starten?")
            if response:
                sys.exit()  # Close the current script
            else:
                self.setting.attributes("-topmost", True)
                self.setting.attributes("-topmost", False)

    def autodataupdateCheckbox(self):
        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+2, weight="bold")

        # Create a variable to store the checkbox state
        self.autoDataUpdateVar = IntVar()
        if settings["autodataupdate"]==True:
            self.autoDataUpdateVar.set(1)   
        else:
            self.autoDataUpdateVar.set(0)
        # Create the checkbox
        self.anzeigeButton = Checkbutton(self.tab2, text="Auto-Daten-Aktualisierung",variable=self.autoDataUpdateVar, command=self.autodataupdate,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"],selectcolor="black",state=self.status)
        self.anzeigeButton.grid(padx=20, pady=5, row=1, column=1,sticky="w") 

        # Create the info icon (using ℹ️ for simplicity)
        self.info_icon = tk.Label(self.tab2, text="\u2753", font=("Helvetica", 11), fg="green",bg=winconfig["bgcolor"])  
        self.info_icon.grid(row=1, column=2, padx=2, pady=2, sticky="w")
        # Tooltip for the info icon
        self.tooltip = ToolTip(self.info_icon, "Wenn es aktiviert ist, wird die Datenbank mit den Maschinenwerkzeuglisten synchronisiert.")
    def autodataupdate(self):
        if self.autoDataUpdateVar.get() == 1:
            # Load current config
            with open(paths["configJson"], "r") as f:
                config = json.load(f)

            # Update the fullscreen value
            config["settings"]["autodataupdate"] = True

            # Save back to the JSON file
            with open(paths["configJson"], "w") as f:
                json.dump(config, f, indent=4)

            self.setting.attributes("-topmost", False)
            response = messagebox.askyesno("Info", "Möchten Sie das Programm jetzt neu starten?")
            if response:
                sys.exit()  # If 'Yes', exit the program
            else:
                self.setting.attributes("-topmost", True)
                self.setting.attributes("-topmost", False)

        else:
            # Load current config
            with open(paths["configJson"], "r") as f:
                config = json.load(f)

            # Update the fullscreen value
            config["settings"]["autodataupdate"] = False

            # Save back to the JSON file
            with open(paths["configJson"], "w") as f:
                json.dump(config, f, indent=4)

            self.setting.attributes("-topmost", False)
            response = messagebox.askyesno("Info", "Möchten Sie das Programm jetzt neu starten?")
            if response:
                sys.exit()  # If 'Yes', exit the program
            else:
                self.setting.attributes("-topmost", True)
                self.setting.attributes("-topmost", False)

  


    def userManager(self):
        self.userManagerWindow = tk.Toplevel()
        self.userManagerWindow.title("Benutzerverwaltung")
        self.userManagerWindow.geometry("830x350")
        self.userManagerWindow.configure(bg="#f0f0f0")
        self.userManagerWindow.attributes("-topmost", True)

        # Labels & Entries
        ttk.Label(self.userManagerWindow, text="ID:").grid(row=0, column=0, padx=10, pady=5, sticky="w")
        self.id_entry = ttk.Entry(self.userManagerWindow)
        self.id_entry.grid(row=0, column=1, padx=10, pady=5)

        ttk.Label(self.userManagerWindow, text="Name:").grid(row=1, column=0, padx=10, pady=5, sticky="w")
        self.name_entry = ttk.Entry(self.userManagerWindow)
        self.name_entry.grid(row=1, column=1, padx=10, pady=5)

        ttk.Label(self.userManagerWindow, text="Password:").grid(row=2, column=0, padx=10, pady=5, sticky="w")
        self.pw_entry = ttk.Entry(self.userManagerWindow)
        self.pw_entry.grid(row=2, column=1, padx=10, pady=5)

        ttk.Label(self.userManagerWindow, text="Rechte:").grid(row=3, column=0, padx=10, pady=5, sticky="w")
        self.rights_combo = ttk.Combobox(self.userManagerWindow, values=["Admin", "User"], state="readonly")
        self.rights_combo.grid(row=3, column=1, padx=10, pady=5)
        self.rights_combo.current(0)  # Default to "Admin"

        ttk.Label(self.userManagerWindow, text="Profilbild:").grid(row=4, column=0, padx=10, pady=5, sticky="w")
        self.file_entry = ttk.Entry(self.userManagerWindow, width=30)
        self.file_entry.grid(row=4, column=1, padx=10, pady=5)

        # File Explorer Button
        self.browse_button = ttk.Button(self.userManagerWindow, text="📂", command=self.browse_file)
        self.browse_button.grid(row=4, column=2, padx=5, pady=5)

        # Buttons
        self.submit_button = ttk.Button(self.userManagerWindow, text="Übernehmen", command=self.submit_data)
        self.submit_button.grid(row=5, column=0, padx=10, pady=20, sticky="w")

        self.update_button = ttk.Button(self.userManagerWindow, text="Ändern", command=self.update_user)
        self.update_button.grid(row=5, column=1, padx=10, pady=20, sticky="w")

        self.delete_button = ttk.Button(self.userManagerWindow, text="Löschen", command=self.delete_user)
        self.delete_button.grid(row=5, column=2, padx=10, pady=20, sticky="w")

        self.cancel_button = ttk.Button(self.userManagerWindow, text="Abbrechen", command=self.userManagerWindow.destroy)
        self.cancel_button.grid(row=5, column=3, padx=10, pady=20, sticky="e")

        # Listbox to display users
        self.user_list = ttk.Treeview(self.userManagerWindow, columns=("ID", "Name"), show="headings")
        self.user_list.heading("ID", text="ID")
        self.user_list.heading("Name", text="Name")
        self.user_list.column("ID", width=100)
        self.user_list.column("Name", width=200)
        self.user_list.grid(row=0, column=4, rowspan=5, padx=10, pady=5, sticky="nsew")

        self.user_list.bind("<ButtonRelease-1>", self.fill_fields)  # Bind click event

        # Load users into the list
        self.load_users()

        self.id_entry.focus()

    def update_user(self):
        """Update user details in the database."""
        id_value = self.id_entry.get().strip()
        name_value = self.name_entry.get().strip()
        pw_value = self.pw_entry.get().strip()
        rights_value = self.rights_combo.get().strip()
        profile_path = self.file_entry.get().strip()

        if not id_value or not name_value:
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showerror("Fehler", "ID und Name dürfen nicht leer sein!")
            self.userManagerWindow.attributes("-topmost", True)
            return

        db_path = paths["MTMDB"]
        try:
            conn = sqlite3.connect(db_path)
            cursor = conn.cursor()
            cursor.execute("UPDATE Users SET Name = ?, Password = ?,rights = ?, profileImage = ? WHERE ID = ?",
                        (name_value,pw_value, rights_value, profile_path, id_value))
            conn.commit()
            conn.close()
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showinfo("Erfolg", "Benutzerdaten erfolgreich aktualisiert!")
            self.userManagerWindow.attributes("-topmost", True)
            self.load_users()  # Refresh list

        except sqlite3.Error as e:
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showerror("Datenbankfehler", f"Fehler: {e}")
            self.userManagerWindow.attributes("-topmost", True)


    def load_users(self):
        """Load users from the database into the listbox."""
        self.user_list.delete(*self.user_list.get_children())  # Clear existing entries

        db_path = paths["MTMDB"]
        if not os.path.exists(db_path):
            return

        try:
            conn = sqlite3.connect(db_path)
            cursor = conn.cursor()
            cursor.execute("SELECT ID, Name FROM Users")
            for row in cursor.fetchall():
                self.user_list.insert("", "end", values=row)
            conn.close()
        except sqlite3.Error as e:
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showerror("Datenbankfehler", f"Fehler: {e}")
            self.userManagerWindow.attributes("-topmost", True)

    def fill_fields(self, event):
        """Fill entry fields when selecting a user from the list."""
        selected = self.user_list.focus()
        if not selected:
            return

        values = self.user_list.item(selected, "values")
        if not values:
            return

        id_value, name_value  = values
        db_path = paths["MTMDB"]

        try:
            conn = sqlite3.connect(db_path)
            cursor = conn.cursor()
            cursor.execute("SELECT Password,rights, profileImage FROM Users WHERE ID = ?", (id_value,))
            result = cursor.fetchone()
            conn.close()

            if result:
                pw_value ,rights_value, profile_path = result
                self.id_entry.delete(0, tk.END)
                self.id_entry.insert(0, id_value)
                self.name_entry.delete(0, tk.END)
                self.name_entry.insert(0, name_value)
                self.pw_entry.delete(0, tk.END)
                self.pw_entry.insert(0, pw_value)
                self.rights_combo.set(rights_value)  # Select rights
                self.file_entry.delete(0, tk.END)
                self.file_entry.insert(0, profile_path)  # Set profile image path

        except sqlite3.Error as e:
            self.userManagerWindow.attributes("-topmost", False)

            messagebox.showerror("Datenbankfehler", f"Fehler: {e}")
            self.userManagerWindow.attributes("-topmost", True)


    def delete_user(self):
        """Delete selected user from the database."""
        id_value = self.id_entry.get().strip()
        if not id_value:
            self.userManagerWindow.attributes("-topmost", False)

            messagebox.showerror("Fehler", "Bitte zuerst einen Benutzer auswählen!")
            self.userManagerWindow.attributes("-topmost", True)

            return
        self.userManagerWindow.attributes("-topmost", False)

        confirm = messagebox.askyesno("Löschen bestätigen", f"Sind Sie sicher, dass Sie Benutzer {id_value} löschen möchten?")
        if not confirm:
            self.userManagerWindow.attributes("-topmost", True)
            return
        self.userManagerWindow.attributes("-topmost", True)

        db_path = paths["MTMDB"]
        try:
            conn = sqlite3.connect(db_path)
            cursor = conn.cursor()
            cursor.execute("DELETE FROM Users WHERE ID = ?", (id_value,))
            conn.commit()
            conn.close()
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showinfo("Erfolg", "Benutzer erfolgreich gelöscht!")
            self.userManagerWindow.attributes("-topmost", True)

            self.load_users()  # Refresh list
            self.id_entry.delete(0, tk.END)
            self.name_entry.delete(0, tk.END)
            self.pw_entry.delete(0, tk.END)
            self.rights_combo.current(0)
            self.file_entry.delete(0, tk.END)

        except sqlite3.Error as e:
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showerror("Datenbankfehler", f"Fehler: {e}")
            self.userManagerWindow.attributes("-topmost", True)

    def browse_file(self):
        file_path = filedialog.askopenfilename(title="Datei auswählen", filetypes=[("Bilder", "*.png;*.jpg;*.jpeg;*.bmp;*.gif"), ("Alle Dateien", "*.*")])
        if file_path:
            self.file_entry.delete(0, tk.END)
            self.file_entry.insert(0, file_path)

    def submit_data(self):
        id_value = self.id_entry.get().strip()
        name_value = self.name_entry.get().strip()
        pw_value = self.pw_entry.get().strip()
        rights_value = self.rights_combo.get()
        profile_path = self.file_entry.get().strip()

        if not id_value or not name_value:
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showerror("Fehler", "ID und Name dürfen nicht leer sein!")
            self.userManagerWindow.attributes("-topmost", True)

            return

        db_path = paths["MTMDB"]
        if not os.path.exists(db_path):
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showerror("Fehler", "Datenbank nicht gefunden!")
            self.userManagerWindow.attributes("-topmost", True)

            return

        try:
            conn = sqlite3.connect(db_path)
            cursor = conn.cursor()

            # Check if the ID already exists
            cursor.execute("SELECT COUNT(*) FROM Users WHERE ID = ? OR Password = ?", (id_value, pw_value))
            result = cursor.fetchone()

            if result and result[0] > 0:
                self.userManagerWindow.attributes("-topmost", False)
                messagebox.showerror("Fehler", f"Die ID {id_value} existiert bereits!")
                self.userManagerWindow.attributes("-topmost", True)

                conn.close()
                return
            

            # Insert data into Users table
            cursor.execute("""
                INSERT INTO Users (ID, Name, Password,rights, profileImage) 
                VALUES (?, ?, ?,?, ?)
            """, (id_value,name_value, pw_value, rights_value, profile_path))

            conn.commit()
            conn.close()
            
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showinfo("Erfolg", "Benutzer erfolgreich hinzugefügt!")
            self.userManagerWindow.destroy()

        except sqlite3.Error as e:
            self.userManagerWindow.attributes("-topmost", False)
            messagebox.showerror("Datenbankfehler", f"Fehler: {e}")
            self.userManagerWindow.attributes("-topmost", True)



if __name__ == "__main__":
    app = Settings()
