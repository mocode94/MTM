from tkinter import Toplevel, Label, Entry, Button, StringVar, filedialog
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

#                                        Einstellungen
#==========================================================================================================

class Settings:
    def __init__(self):
        self.setting = tk.Toplevel()
        self.setting.title("Tkinter Window with Tabs")
        self.setting.geometry("950x700")
        
        # Create a style
        style = ttk.Style()
        style.configure('TNotebook', tabposition='nw')  # Position of tabs
        style.configure('TNotebook.Tab', 
                        font=(winconfig["fonttype"], winconfig["fontsize"]),  # Change font and size
                        background=winconfig["fontcolor"],  # Set background color for tabs
                        foreground=winconfig["bgcolor"],  # Set font color for tabs
                        padding=[10, 5])  # Adjust padding for the tabs

        # Create a notebook (tab control)
        notebook = ttk.Notebook(self.setting, style='TNotebook')

        # Create frames for each tab
        self.tab1 = tk.Frame(notebook, background=winconfig["bgcolor"])
        self.tab2 = tk.Frame(notebook, background=winconfig["bgcolor"])
        self.tab3 = tk.Frame(notebook, background=winconfig["bgcolor"])

        # Add the frames to the notebook
        notebook.add(self.tab1, text='  Einstellungen  ')
        notebook.add(self.tab2, text='  System  ')
        notebook.add(self.tab3, text='  andere  ')

        # Load the image
        self.addPlaceButtonImage = Image.open(paths["imgpaths"]+"addplace.png")  # Update this with your image path
        self.addPlaceButtonImage = self.addPlaceButtonImage.resize((120, 120), Image.LANCZOS)  # Resize if needed
        self.addPlaceButtonImage = ImageTk.PhotoImage(self.addPlaceButtonImage)  # Convert to PhotoImage

        # Load the image
        self.anzeigeButtonImage = Image.open(paths["imgpaths"]+"anzeige.png")  # Update this with your image path
        self.anzeigeButtonImage = self.anzeigeButtonImage.resize((120, 120), Image.LANCZOS)  # Resize if needed
        self.anzeigeButtonImage = ImageTk.PhotoImage(self.anzeigeButtonImage)  # Convert to PhotoImage

        # Create a button with an image
        self.addPlaceButton = Button(self.tab1, image=self.addPlaceButtonImage, command=self.open_add_place)
        self.addPlaceButton.grid(padx=20, pady=20, row=0, column=0)  # Adjust padding as needed
        # Create a button with an image
        self.anzeigeButton = Button(self.tab1, image=self.anzeigeButtonImage, command=self.open_add_place)
        self.anzeigeButton.grid(padx=20, pady=20, row=0, column=1)  # Adjust padding as needed

        # Pack the notebook widget
        notebook.pack(expand=True, fill='both')
        
        self.setting.mainloop()

    def open_add_place(self):
        """Method to open the AddPlace window."""
        add_place_window = AddPlace()  # Create an instance of AddPlace


if __name__ == "__main__":
    app = Settings()
