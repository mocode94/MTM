from tkinter import Toplevel, Label, Entry, Button, StringVar, filedialog
import tkinter as tk
from tkinter import ttk
from PIL import Image, ImageTk
# import os
import csv
# import cv2
# import tkinter.messagebox as messagebox
import json
import sys
import tkinter.font as font
import threading
import time
import platform
import subprocess
# from collections import Counter
# import smtplib
# import ssl
# from email.message import EmailMessage
# import time
# import requests
# from bs4 import BeautifulSoup
# import urllib.parse
# from PyPDF2 import PdfReader
# import trimesh
# import numpy as np
# import re
# import xml.etree.ElementTree as ET
from config import winconfig,paths,places,settings
import sqlite3
import csv
from queue import Queue, Empty




# #                                        Werkzeugslisten
# #==========================================================================================================
# #==========================================================================================================
# #==========================================================================================================

class listModule:
    def __init__(self,mainFrame):
        self.mainFrame = mainFrame
        mainFrame.config(width=2000, height=1250)

        # Clear the frame before adding new widgets
        self.clearFrame()

        # Create a new frame inside the mainFrame with a specific size
        self.centerFrame = tk.Frame(mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        self.image_refs = []
        default_image_path=paths["imgpaths"]+"NOTFOUND.png"
        self.value_window = None
        

        for dictionary in places:
            lage=dictionary.values()
            lage2=list(lage)
            bild=lage2[0]
            befehl=lage2[1]
            row=lage2[2]
            column=lage2[3]
            self.create_button_with_image(paths["imgpaths"]+bild, default_image_path,lambda befehl=befehl: self.liste_aufruf(befehl),row,column)
                

        self.create_button_with_image(paths["imgpaths"]+"solidcamlogo.png",default_image_path, self.showmaster,0,0)


    def clearFrame(self):
        # Clear all widgets from the mainFrame
        for widget in self.mainFrame.winfo_children():
            widget.destroy()

        
    def create_button_with_image(self, image_path, default_image_path, command, row, column):
        try:
            original_image = Image.open(image_path)
        except IOError:
            original_image = Image.open(default_image_path)
        
        original_image.thumbnail((150, 150))  
        image = ImageTk.PhotoImage(original_image)

        button = tk.Button(self.mainFrame, image=image, command=command, width=200, height=120, compound=tk.CENTER,bg=winconfig["bgcolor"])
        button.image = image  
        button.grid(row=row, column=column, padx=20, pady=20)

        self.image_refs.append(image)

    def tool24(self):
        print("Tool24")

    def showmaster(self):
        master_table = tk.Toplevel()
        master_table.title("List in Table")
        master_table.geometry("2000x800")
        self.wz_info_frame = tk.Frame(master_table)
        self.wz_info_frame.pack(side=tk.RIGHT, fill=tk.Y, padx=10, pady=10)
        
        # Create the Treeview widget
        self.table_master = ttk.Treeview(master_table, columns=('T_Nummer', 'T_Name', 'T_Radius',"Schnittlänge",'Ausspann_Laenge','SOLL_Laenge','Wzg_ArtNr','Aufn_ArtNr','zus_Komp','zus_Komp2','Spann_Sys'))

        self.table_master.heading('T_Nummer', text='T_Nummer', anchor='center')
        self.table_master.heading('T_Name', text='T_Name', anchor='center')
        self.table_master.heading('T_Radius', text='T_Radius', anchor='center')
        self.table_master.heading('Schnittlänge', text='Schnittlänge', anchor='center')
        self.table_master.heading('Ausspann_Laenge', text='Ausspann_Laenge', anchor='center')
        self.table_master.heading('SOLL_Laenge', text='SOLL_Laenge', anchor='center')
        self.table_master.heading('Wzg_ArtNr', text='Wzg_ArtNr', anchor='center')
        self.table_master.heading('Aufn_ArtNr', text='Aufn_ArtNr', anchor='center')
        self.table_master.heading('zus_Komp', text='zus_Komp', anchor='center')
        self.table_master.heading('zus_Komp2', text='zus_Komp2', anchor='center')
        self.table_master.heading('Spann_Sys', text='Spann_Sys', anchor='center')
        self.table_master.column("#0", width=0, stretch=tk.NO)
        self.table_master.column('T_Nummer', width=100, anchor='center')
        self.table_master.column('T_Name', width=150, anchor='center')
        self.table_master.column('T_Radius', width=100, anchor='center')
        self.table_master.column('Schnittlänge', width=100, anchor='center')
        self.table_master.column('Ausspann_Laenge', width=100, anchor='center')
        self.table_master.column('SOLL_Laenge', width=100, anchor='center')
        self.table_master.column('Wzg_ArtNr', width=100, anchor='center')
        self.table_master.column('Aufn_ArtNr', width=100, anchor='center')
        self.table_master.column('zus_Komp', width=100, anchor='center')
        self.table_master.column('zus_Komp2', width=100, anchor='center')
        self.table_master.column('Spann_Sys', width=100, anchor='center')

        # Configure tag for row colors
        self.table_master.tag_configure('evenrow', background='#7daad4')
        self.table_master.tag_configure('oddrow', background='#a4b2bf')
        self.table_master.tag_configure('highlight', background='yellow')  # Change highlight color to yellow
        self.table_master.pack(expand=True, fill="both")

        # Read data from the CSV file
        tabelle_data = []
        with open(paths["mastercsv"], "r") as file:
            reader = csv.reader(file, delimiter=";")
            next(reader)
            for row in reader:
                radius = float(row[6]) / 2
                row = [row[0], row[1], str(radius),row[21],row[18], row[22], row[45], row[46], row[47], row[48], row[49]]
                tabelle_data.append(row)

        # Insert rows into the Treeview
        for i, row in enumerate(tabelle_data):
            if i % 2 == 0:
                self.table_master.insert('', 'end', values=row, tags=('evenrow',))
            else:
                self.table_master.insert('', 'end', values=row, tags=('oddrow',))



        search_frame = tk.Frame(master_table)
        search_frame.pack(side=tk.BOTTOM, fill=tk.X, padx=10, pady=10)
        
        self.search_var_master = tk.StringVar()
        search_entry = tk.Entry(search_frame, textvariable=self.search_var_master)
        search_entry.pack(side=tk.LEFT, padx=5)
        search_entry.focus_set()
        
        search_button = tk.Button(search_frame, text="Search", command=self.search_treeview)
        search_button.pack(side=tk.LEFT, padx=5)

        self.table_master.bind("<ButtonRelease-1>", self.show_value)
        search_entry.bind('<Return>', lambda event: search_button.invoke())





        master_table.mainloop()

    def show_value(self,event):
        if self.table_master.selection():  # Ensure there is a selection
            selected_item = self.table_master.selection()[0]
            values = self.table_master.item(selected_item, 'values')
            value_in_second_column = values[1]

            # Clear existing widgets in wz_info_frame
            for widget in self.wz_info_frame.winfo_children():
                widget.destroy()

            # Add new content
            tk.Label(self.wz_info_frame, text=value_in_second_column).pack()

            button_schnittdaten = Button(self.wz_info_frame, text="Die Schnittdaten aus SolidCAM",command=lambda: self.schnittdaten(values), width=30, bg="coral").pack(pady=5)
            stl = values[1] + '.stl'
            button_3d = Button(self.wz_info_frame, text='Das 3D-Model',command=lambda: self.visualize_stl(paths["toolstl"] + stl), width=30).pack(pady=5)
            button_komp = Button(self.wz_info_frame, text="Die Komponenten",command=lambda: self.komp_images(values), width=30).pack(pady=5)
            button_info = Button(self.wz_info_frame, text="Die Info",command=lambda: self.wz_info(values), width=30).pack(pady=5)

    def search_treeview(self):
        search_value = self.search_var_master.get().strip().lower()
        
        # Überprüfen, ob sich der Suchwert geändert hat
        if search_value != getattr(self, 'last_search_value', None):
            # Suchergebnisse und Index zurücksetzen, wenn der Suchwert sich ändert
            self.search_results = [item for item in self.table_master.get_children() if any(search_value in str(value).lower() for value in self.table_master.item(item, "values")[:2])]
            self.current_search_index = 0  # Startindex auf 0 setzen
            self.last_search_value = search_value  # Den letzten Suchwert speichern
        
        # Überprüfen, ob es Ergebnisse gibt
        if self.search_results:
            self.show_next_search_result()
        else:
            print("No matching rows found.")

    def show_next_search_result(self):
        if self.search_results:
            item = self.search_results[self.current_search_index]
            self.table_master.selection_set(item)
            self.table_master.focus(item)
            self.table_master.see(item)
            self.show_value(item)
            
            # Index für das nächste Ergebnis erhöhen
            self.current_search_index += 1
            
            # Wenn der Index das Ende der Liste erreicht, zurück auf 0 setzen
            if self.current_search_index >= len(self.search_results):
                self.current_search_index = 0  # Zurücksetzen auf den Anfang


    def komp_images(self,master_row):
        komp_images_win=Toplevel()
        komp_images_win.title(master_row[1])
        komp_images_win.geometry("465x170")
        # image_path=self.perschmanndbimage+master_row[5][:6]+".jpg"
        width=150
        height=150
        komp=[master_row[6],master_row[8],master_row[7]]
        for i, item in enumerate(komp):
            image_path=paths["compimgs"]+item[:6]+".jpg"
            try:
                # Attempt to load the specified image
                image = Image.open(image_path)
                image = image.resize(( width, height), Image.LANCZOS)  # Resize the image to fit the label
                image = image.rotate(0, expand=True)

            except (FileNotFoundError):
            # Load the image
                image = Image.open(paths["compimgs"]+"NOTFOUND.png")
                image = image.resize(( 150, height), Image.LANCZOS)  # Resize the image to fit the label
                image = image.rotate(0, expand=True)
            photo = ImageTk.PhotoImage(image)

            label = Label(komp_images_win, image=photo)
            label_komp=Label(komp_images_win,text=item)
            label.image = photo  # Keep a reference to the image to prevent it from being garbage collected
            label_komp.grid(row=0,column=i)
            label.grid(row=1, column=i)


    def visualize_stl(self, file_path, width=235, height=600):
        import vtk

        # Create a reader for the STL file
        reader = vtk.vtkSTLReader()
        reader.SetFileName(file_path)

        # Create a mapper
        mapper = vtk.vtkPolyDataMapper()
        mapper.SetInputConnection(reader.GetOutputPort())

        # Create an actor
        actor = vtk.vtkActor()
        actor.SetMapper(mapper)

        # Create a renderer and render window
        renderer = vtk.vtkRenderer()
        render_window = vtk.vtkRenderWindow()
        render_window.AddRenderer(renderer)

        # Set the desired window size (width, height)
        render_window.SetSize(width, height)  # Adjust as needed

        # Set the position of the render window to the bottom right
        import tkinter as tk
        root = tk.Tk()
        
        screen_width = root.winfo_screenwidth()
        screen_height = root.winfo_screenheight()
        root.destroy()

        # Calculate position for bottom right
        x_position = screen_width - width
        y_position = screen_height -150- height

        # Set the position
        render_window.SetPosition(x_position, y_position)

        # Create an interactor
        render_window_interactor = vtk.vtkRenderWindowInteractor()
        render_window_interactor.SetRenderWindow(render_window)

        # Add the actor to the scene
        renderer.AddActor(actor)
        renderer.SetBackground(1, 1, 1)  # Background color white

        # Set up interactor style to allow rotation
        style = vtk.vtkInteractorStyleTrackballActor()
        render_window_interactor.SetInteractorStyle(style)

        # Start the rendering loop
        render_window.Render()
        render_window_interactor.Start()







    def schnittdaten(self, master_row):
        schnittdaten_win = Toplevel()
        schnittdaten_win.title("SCHNITTDATEN")
        schnittdaten_win.geometry("1300x200")

        def read_json_file(file_path):
            with open(file_path, 'r') as file:
                data = json.load(file)
            return data

        def search_item(data, item):
            return data.get(item, None)

        file_path = paths["feeddata"]
        data = read_json_file(file_path)
        item_key = master_row[0]  # Replace with the key you want to search for
        radius = float(master_row[2])

        item_values = search_item(data, item_key)

        # Create a Treeview widget
        tree = ttk.Treeview(schnittdaten_win, columns=("Material","Schnittdaten zum Schruppen","Schnittdaten zum Schlichten"), show='headings')
        tree.heading("Material", text="Material")
        tree.heading("Schnittdaten zum Schruppen", text="Schnittdaten zum Schruppen")
        tree.heading("Schnittdaten zum Schlichten", text="Schnittdaten zum Schlichten")


        tree.column("Material",width=30,anchor='center')
        tree.column("Schnittdaten zum Schruppen",anchor='w')
        tree.column("Schnittdaten zum Schlichten",anchor='w')


        tree.pack(expand=True, fill='both')


        if item_values:
            for obj in item_values:
                for material, values in obj.items():
                    drehzahlschruppen = int((values[4]) / (3.14 * (radius / 1000)))
                    vorschubschruppen= int(drehzahlschruppen*values[1])
                    drehzahlschlichten = int((values[5]) / (3.14 * (radius / 1000)))
                    vorschubschlichten=int(drehzahlschlichten*values[3])
                    schruppen=f"Vc= {values[4]} m/min      f/U= {values[1]} mm/min-1      S={drehzahlschruppen} U/min-1      f={vorschubschruppen} mm/min"
                    schlichten=f"Vc= {values[5]} m/min      f/U= {values[3]} mm/min-1      S={drehzahlschlichten} U/min-1      f={vorschubschlichten} mm/min"

                    tree.insert('', 'end', values=( values[0], schruppen,schlichten ))
        else:
            tree.insert('', 'end', values=( f"Item {item_key} not found", "", ""))


    def wz_info(self,master_row):
        wz_info_win=Toplevel()
        wz_info_win.title(master_row[1])
        wz_info_win.geometry("250x250")


        lag_value = set()
        name_value = set()
        nr_value = set()
        index_value = set()
        name=master_row[1]

 

        found = False
        with open(paths["machinecsv"], "r") as file:
            csv_reader = csv.reader(file, delimiter=";")
            for row in csv_reader:
                if name==row[2]:
                    found = True
                    index_value.add(row[0])
                    nr_value.add(row[1])
                    lag_value.add(row[19])
        if not found:
            index_value = {"NOTFOUND"}
            nr_value = {"NOTFOUND"}
            lag_value = {"NOTFOUND"}
                    

                    


        index_value_str = ' , '.join(map(str, index_value))
        nr_value_str = ' , '.join(map(str, nr_value))
        lag_value_str = ' , '.join(map(str, lag_value))




        label_font = font.Font(family="Helvetica", size=11, weight="bold")
        if index_value is not None:  # Check if any matching rows were found
            label_index = Label(wz_info_win, text="Wz QR ist : " + str(index_value_str),font=label_font)
            label_index.grid(row=0, column=0, sticky=tk.W)
            label_tnum = Label(wz_info_win, text="Wz Nummer ist : " + str(nr_value_str),font=label_font)
            label_tnum.grid(row=1, column=0, sticky=tk.W)
            label_lag = Label(wz_info_win, text="Wo: " + str(lag_value_str),font=label_font)
            label_lag.grid(row=3, column=0, sticky=tk.W)

    def showsonstiges(self):
        sonstiges_table = tk.Toplevel()
        sonstiges_table.title("List in Table")
        sonstiges_table.geometry("2000x800")
        
        table = ttk.Treeview(sonstiges_table, columns=('QR', 'T_Nummer', 'T_Name','Typ', 'T_Radius', 'Eckenradius','Spitzenwinkel','Eintauchwinkel','Schneiden','Schnittlaenge' ,'Ausspann_Laenge'
                                                  ,'IST_Laenge', 'SOLL_Laenge', 'Steigung','Wzg_ArtNr', 'Aufn_ArtNr', 'zus_Komp', 'zus_Komp2', 'Spann_Sys', 'Wo'))
        table.column('#0', width=0, stretch=tk.NO)
        table.heading('QR', text='QR', anchor='center')
        table.heading('T_Nummer', text='T_Nummer', anchor='center')
        table.heading('T_Name', text='T_Name', anchor='center')
        table.heading('Typ', text='Typ', anchor='center')
        table.heading('T_Radius', text='T_Radius', anchor='center')
        table.heading('Eckenradius', text='Eckenradius', anchor='center')
        table.heading('Spitzenwinkel', text='Spitzenwinkel', anchor='center')
        table.heading('Eintauchwinkel', text='Eintauchwinkel', anchor='center')
        table.heading('Schneiden', text='Schneiden', anchor='center')
        table.heading('Schnittlaenge', text='Schnittlaenge', anchor='center')
        table.heading('Ausspann_Laenge', text='Ausspann_Laenge', anchor='center')
        table.heading('IST_Laenge', text='IST_Laenge', anchor='center')
        table.heading('SOLL_Laenge', text='SOLL_Laenge', anchor='center')
        table.heading('Steigung', text='Steigung', anchor='center')
        table.heading('Wzg_ArtNr', text='Wzg_ArtNr', anchor='center')
        table.heading('Aufn_ArtNr', text='Aufn_ArtNr', anchor='center')
        table.heading('zus_Komp', text='zus_Komp', anchor='center')
        table.heading('zus_Komp2', text='zus_Komp2', anchor='center')
        table.heading('Spann_Sys', text='Spann_Sys', anchor='center')
        table.heading('Wo', text='Wo', anchor='center')

        table.column('QR', width=50, anchor='center')
        table.column('T_Nummer', width=60, anchor='center')
        table.column('T_Name', width=110, anchor='center')
        table.column('Typ', width=30, anchor='center')
        table.column('T_Radius', width=80, anchor='center')
        table.column('Eckenradius', width=90, anchor='center')
        table.column('Spitzenwinkel', width=110, anchor='center')
        table.column('Eintauchwinkel', width=110, anchor='center')
        table.column('Schneiden', width=80, anchor='center')
        table.column('Schnittlaenge', width=100, anchor='center')
        table.column('Ausspann_Laenge', width=110, anchor='center')
        table.column('IST_Laenge', width=90, anchor='center')
        table.column('SOLL_Laenge', width=90, anchor='center')
        table.column('Steigung', width=80, anchor='center')
        table.column('Wzg_ArtNr', width=90, anchor='center')
        table.column('Aufn_ArtNr', width=90, anchor='center')
        table.column('zus_Komp', width=80, anchor='center')
        table.column('zus_Komp2', width=80, anchor='center')
        table.column('Spann_Sys', width=80, anchor='center')
        table.column('Wo', width=40, anchor='center')

        table.tag_configure('evenrow', background='#ad1010')
        table.tag_configure('oddrow', background='#ffffff')
        table.pack(expand=True, fill="both")

        # Read data from the first CSV file
        tabelle_data = []
        with open(paths["machinecsv"], "r") as file:
            reader = csv.reader(file, delimiter=";")
            next(reader)
            for row in reader:
                    if row[19]!='Maschine':
                     tabelle_data.append(row)


        tabelle_data.sort(key=lambda x: int(x[1]) if x[1].isdigit() else float('inf')) 
            

        # Compare tradius values and mark rows where tradius in Werkzeugsliste_Maschine.csv is smaller than in Werkzeugsliste_Master.csv
        marked_rows = self.compare_and_mark_SONSTIGES(tabelle_data)

        # Insert rows into the treeview
        for i, t_row in enumerate(tabelle_data[1:], start=1):
         if i in marked_rows:
    
            table.insert('', 'end', values=t_row, tags=('redcell',))
        tabelle_data.sort(key=lambda x: int(x[1]) if x[1].isdigit() else float('inf'))    

        # Configure tag for cell background color (red)
        table.tag_configure('redcell', background='lightblue')

        sonstiges_table.mainloop()



    def compare_and_mark_SONSTIGES(self, tabelle_data):
        file_path = self.lageort

        # Step 1: Read the JSON file
        with open(file_path, 'r') as f:
            data = json.load(f)

        # Step 2: Extract the 'befehl' values and store them in a list
        lagen = [item['befehl'] for item in data]

        marked_rows = []

        for i in range(1, len(tabelle_data)):  # Start from index 1 to skip header row
            if tabelle_data[i][21] not in lagen:  # Check if value is not in 'lagen' list
                marked_rows.append(i)

        return marked_rows


    def liste_aufruf(self,lageort):

        Liste(lageort)





class Liste:
    
    def __init__(self, lageort):
        self.liste = tk.Toplevel()
        self.liste.title(f"{lageort} Liste")
        self.liste.geometry("2000x800")

        self.info_frame = tk.Frame(self.liste)
        self.info_frame.pack(side=tk.RIGHT, fill=tk.Y, padx=10, pady=10)




        self.table = ttk.Treeview(self.liste, columns=('QR', 'T_Nummer', 'T_Name','Typ', 'T_Radius', 'Eckenradius','Spitzenwinkel','Eintauchwinkel','Schneiden','Schnittlaenge' ,'Ausspann_Laenge'
                                                  ,'IST_Laenge', 'SOLL_Laenge', 'Steigung','Wzg_ArtNr', 'Aufn_ArtNr', 'zus_Komp', 'zus_Komp2', 'Spann_Sys', 'Wo'))

        self.table.heading('QR', text='QR', anchor='center')
        self.table.heading('T_Nummer', text='T_Nummer', anchor='center')
        self.table.heading('T_Name', text='T_Name', anchor='center')
        self.table.heading('Typ', text='Typ', anchor='center')
        self.table.heading('T_Radius', text='T_Radius', anchor='center')
        self.table.heading('Eckenradius', text='Eckenradius', anchor='center')
        self.table.heading('Spitzenwinkel', text='Spitzenwinkel', anchor='center')
        self.table.heading('Eintauchwinkel', text='Eintauchwinkel', anchor='center')
        self.table.heading('Schneiden', text='Schneiden', anchor='center')
        self.table.heading('Schnittlaenge', text='Schnittlaenge', anchor='center')
        self.table.heading('Ausspann_Laenge', text='Ausspann_Laenge', anchor='center')
        self.table.heading('IST_Laenge', text='IST_Laenge', anchor='center')
        self.table.heading('SOLL_Laenge', text='SOLL_Laenge', anchor='center')
        self.table.heading('Steigung', text='Steigung', anchor='center')
        self.table.heading('Wzg_ArtNr', text='Wzg_ArtNr', anchor='center')
        self.table.heading('Aufn_ArtNr', text='Aufn_ArtNr', anchor='center')
        self.table.heading('zus_Komp', text='zus_Komp', anchor='center')
        self.table.heading('zus_Komp2', text='zus_Komp2', anchor='center')
        self.table.heading('Spann_Sys', text='Spann_Sys', anchor='center')
        self.table.heading('Wo', text='Wo', anchor='center')

        self.table.column("#0", width=0, stretch=tk.NO)
        self.table.column('QR', width=50, anchor='center')
        self.table.column('T_Nummer', width=50, anchor='center')
        self.table.column('T_Name', width=170, anchor='center')
        self.table.column('Typ', width=50, anchor='center')
        self.table.column('T_Radius', width=40, anchor='center')
        self.table.column('Eckenradius', width=40, anchor='center')
        self.table.column('Spitzenwinkel', width=40, anchor='center')
        self.table.column('Eintauchwinkel', width=40, anchor='center')
        self.table.column('Schneiden', width=20, anchor='center')
        self.table.column('Schnittlaenge', width=20, anchor='center')
        self.table.column('Ausspann_Laenge', width=20, anchor='center')
        self.table.column('IST_Laenge', width=40, anchor='center')
        self.table.column('SOLL_Laenge', width=40, anchor='center')
        self.table.column('Steigung', width=20, anchor='center')
        self.table.column('Wzg_ArtNr', width=100, anchor='center')
        self.table.column('Aufn_ArtNr', width=100, anchor='center')
        self.table.column('zus_Komp', width=100, anchor='center')
        self.table.column('zus_Komp2', width=100, anchor='center')
        self.table.column('Spann_Sys', width=30, anchor='center')
        self.table.column('Wo', width=20, anchor='center')

        self.table.tag_configure('yellowcell', background='yellow')
        self.table.tag_configure('greencell', background='lightgreen')
        self.table.tag_configure('bluecell', background='lightblue')
        self.table.tag_configure('orangecell', background='orange')
        self.table.tag_configure('defaultcell', background='white')
        self.table.pack(expand=True, fill="both")

        self.listname=lageort

        self.current_image = None
        self.photo_images_list = []  # Store images in a list to keep references

        # Add search field and button
        self.search_frame = tk.Frame(self.liste)
        self.search_frame.pack(side=tk.BOTTOM, fill=tk.X, padx=10, pady=10)

        self.search_var = tk.StringVar()
        search_entry = tk.Entry(self.search_frame, textvariable=self.search_var)
        search_entry.pack(side=tk.LEFT, padx=5)
        search_entry.focus_set()
        
        search_button = tk.Button(self.search_frame, text="Search", command=self.search_treeview)
        search_button.pack(side=tk.LEFT, padx=5)


        button_check=tk.Button(self.search_frame,text="Check-up",command=self.checkUP,width=25)
        button_check.pack(side='right', padx=25)

        self.populate_table( paths["mastercsv"], lageort)
        self.table.bind("<ButtonRelease-1>", self.on_row_click)
        search_entry.bind('<Return>', lambda event: search_button.invoke())
        self.machineStatus(lageort)
        self.liste.mainloop()






    def machineStatus(self, lageort):
        # Function to check if IP is reachable using Tnccmd
        def ping_ip(ip):
            try:
                # Full path to Tnccmd
                tnccmd_cmd = paths["TNCcmd"]
                output = subprocess.run(
                    [tnccmd_cmd, "ping", ip],
                    stdout=subprocess.PIPE, stderr=subprocess.PIPE
                )
                return output.returncode == 0  # Return True if ping was successful
            except Exception as e:
                print(f"Error pinging {ip} with Tnccmd: {e}")
                return False

        # Function to update the circle color and text based on ping status
        def update_status(ip, label, canvas, circle, text_item):
            while True:
                status = ping_ip(ip)
                # Schedule the UI update on the main thread
                self.search_frame.after(0, update_ui, status, canvas, circle, text_item)
                time.sleep(3)  # Wait for 3 seconds before pinging again

        # Function to update the UI safely on the main thread
        def update_ui(status, canvas, circle, text_item):
            if canvas.winfo_exists():  # Check if the widget still exists
                if status:
                    canvas.itemconfig(circle, fill='lightgreen')  # Set the circle color to green
                    canvas.itemconfig(text_item, text='Online')  # Set the text to Online
                else:
                    canvas.itemconfig(circle, fill='red')  # Set the circle color to red
                    canvas.itemconfig(text_item, text='Offline')  # Set the text to Offline

        # Function to start checking ping in a separate thread
        def start_ping(ip, label, canvas, circle, text_item):
            thread = threading.Thread(target=update_status, args=(ip, label, canvas, circle, text_item), daemon=True)
            thread.start()

        # Finding the corresponding IP for the given place (lageort)
        for place in places:
            if place["placename"] == lageort:
                ip_addresses = place["link"]

        # Create a Frame to hold the Label and Canvas side by side
        frame = tk.Frame(self.search_frame)
        frame.pack(side="top", fill="x")

        # Create a Canvas widget to draw a circle and the "Online/Offline" text
        canvas = tk.Canvas(frame, width=60, height=60)
        circle = canvas.create_oval(10, 10, 56, 56, fill="gray")  # Initial color is gray
        text_item = canvas.create_text(32, 32, text="....")  # Initial text is Online
        canvas.pack(side="right")

        # Create the Label for the IP address with bold font
        bold_font = font.Font(size=11, weight="bold")  # Create a font object with bold weight
        label = Label(frame, text=f"Der Status der {lageort} ist ", padx=10, font=bold_font)
        label.pack(side="right")

        # Start pinging the IP address
        start_ping(ip_addresses, label, canvas, circle, text_item)




    def populate_table(self,mastercsv, lageort):
        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        
        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        
        # Sort currentToolsData by the second column (assuming it's an integer field)
        currentToolsData.sort(key=lambda x: int(x[1]) if str(x[1]).isdigit() else float('inf'))
        
        # Read master CSV file into master_data list
        master_data = []
        with open(mastercsv, "r") as file:
            reader = csv.reader(file, delimiter=";")
            for row in reader:
                master_data.append(row)
        
        # Iterate through the data and apply the same conditions as before
        for i, t_row in enumerate(currentToolsData):
            if t_row[19] == lageort:
                # Apply conditions and insert rows into the table with appropriate tags
                if t_row[2] in [row[1] for row in master_data] and \
                        float(t_row[11]) + float([row[21] for row in master_data if row[1] == t_row[2]][0]) * 0.2 > float([row[22] for row in master_data if row[1] == t_row[2]][0]) and \
                        t_row[14] == [row[45] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[15] == [row[46] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[16] == [row[47] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[15][:6] in ["304272", "304450"]:
                    self.table.insert('', 'end', values=t_row, tags=('yellowcell',))

                elif t_row[2] in [row[1] for row in master_data] and \
                        t_row[11] >= float([row[22] for row in master_data if row[1] == t_row[2]][0]) and \
                        t_row[14] == [row[45] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[15] == [row[46] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[16] == [row[47] for row in master_data if row[1] == t_row[2]][0]:
                    self.table.insert('', 'end', values=t_row, tags=('greencell',))

                elif t_row[2] not in [row[1] for row in master_data]:
                    self.table.insert('', 'end', values=t_row, tags=('bluecell',))

                elif t_row[11] < float([row[22] for row in master_data if row[1] == t_row[2]][0]) or \
                        t_row[14] != [row[45] for row in master_data if row[1] == t_row[2]][0] or \
                        t_row[15] != [row[46] for row in master_data if row[1] == t_row[2]][0] or \
                        t_row[16] != [row[47] for row in master_data if row[1] == t_row[2]][0]:
                    self.table.insert('', 'end', values=t_row, tags=('orangecell',))

            elif lageort == "ALL":
                # Apply the same logic for "ALL" condition
                if t_row[2] in [row[1] for row in master_data] and \
                        float(t_row[11]) + float([row[21] for row in master_data if row[1] == t_row[2]][0]) * 0.2 > float([row[22] for row in master_data if row[1] == t_row[2]][0]) and \
                        t_row[14] == [row[45] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[15] == [row[46] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[16] == [row[47] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[15][:6] in ["304272", "304450"]:
                    self.table.insert('', 'end', values=t_row, tags=('yellowcell',))

                elif t_row[2] in [row[1] for row in master_data] and \
                        t_row[11] >= float([row[22] for row in master_data if row[1] == t_row[2]][0]) and \
                        t_row[14] == [row[45] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[15] == [row[46] for row in master_data if row[1] == t_row[2]][0] and \
                        t_row[16] == [row[47] for row in master_data if row[1] == t_row[2]][0]:
                    self.table.insert('', 'end', values=t_row, tags=('greencell',))

                elif t_row[2] not in [row[1] for row in master_data]:
                    self.table.insert('', 'end', values=t_row, tags=('bluecell',))

                elif t_row[11] < float([row[22] for row in master_data if row[1] == t_row[2]][0]) or \
                        t_row[14] != [row[45] for row in master_data if row[1] == t_row[2]][0] or \
                        t_row[15] != [row[46] for row in master_data if row[1] == t_row[2]][0] or \
                        t_row[16] != [row[47] for row in master_data if row[1] == t_row[2]][0]:
                    self.table.insert('', 'end', values=t_row, tags=('orangecell',))

        # Close the database connection after finishing
        conn.close()





    def search_treeview(self):
        search_value = self.search_var.get().strip().lower()
        
        # Überprüfen, ob sich der Suchwert geändert hat
        if search_value != getattr(self, 'last_search_value', None):
            # Suchergebnisse und Index zurücksetzen, wenn der Suchwert sich ändert
            self.search_results = [item for item in self.table.get_children() if any(search_value in str(value).lower() for value in self.table.item(item, "values")[:3])]
            self.current_search_index = 0  # Startindex auf 0 setzen
            self.last_search_value = search_value  # Den letzten Suchwert speichern
        
        # Überprüfen, ob es Ergebnisse gibt
        if self.search_results:
            self.show_next_search_result()
        else:
            print("No matching rows found.")

    def show_next_search_result(self):
        if self.search_results:
            item = self.search_results[self.current_search_index]
            self.table.selection_set(item)
            self.table.focus(item)
            self.table.see(item)
            self.on_row_click_simulated(item)
            
            # Index für das nächste Ergebnis erhöhen
            self.current_search_index += 1
            
            # Wenn der Index das Ende der Liste erreicht, zurück auf 0 setzen
            if self.current_search_index >= len(self.search_results):
                self.current_search_index = 0  # Zurücksetzen auf den Anfang

    def on_row_click_simulated(self, item):
        selected_row = self.table.item(item, "values")
        for widget in self.info_frame.winfo_children():
            widget.destroy()
        bold_font = ("Helvetica", 10, "bold")
        bold_font2 = ("Helvetica", 50, "bold")

        master_data = []
        with open(paths["mastercsv"], "r") as file:
            reader = csv.reader(file, delimiter=";")
            for row in reader:
                master_data.append(row)

        matching_rows = [row for row in master_data if row[1] == selected_row[2]]
        if matching_rows:
            master_row = matching_rows[0]

            img1_path = (paths["toolCapturedimages"]+selected_row[0]+".png")
            tk.Label(self.info_frame, text=f"Tool: {selected_row[2]}\n \n",font=bold_font).grid(row=0)
            button=tk.Button(self.info_frame, text="+", font=("Helvetica", 14),command= lambda: self.im_liste(img1_path))
            button.grid(row=1,column=2)
            try:
                # Load and display the first image
                img1 = Image.open(img1_path)
                img1 = img1.resize((200, 150), Image.LANCZOS)
                img1_photo = ImageTk.PhotoImage(img1)
                img1_label = Label(self.info_frame, image=img1_photo)
                img1_label.image = img1_photo  # Keep a reference to avoid garbage collection
                img1_label.grid(row=1,column=0,pady=20)
                self.photo_images_list.append(img1_photo)  # Store reference in the list
            except FileNotFoundError:
                # If image 1 is not found, show a default image or handle as desired
                default_img1 = Image.open(paths["imgpaths"]+"NOTFOUND.png")
                default_img1 = default_img1.resize((200, 200), Image.LANCZOS)
                default_img1_photo = ImageTk.PhotoImage(default_img1)
                img1_label = Label(self.info_frame, image=default_img1_photo)
                img1_label.image = default_img1_photo
                img1_label.grid(row=1,column=0,pady=20)
                self.photo_images_list.append(default_img1_photo)  # Store reference in the list
                
            if selected_row[11] < master_row[22]:
                tk.Label(self.info_frame, text=f"Die Gesamtlänge soll: {selected_row[12]} mm",fg="red",font=bold_font).grid()
            if selected_row[14] != master_row[45]:
                tk.Label(self.info_frame, text=f"Das Wz soll: {master_row[45]}",fg="red",font=bold_font).grid()
            if selected_row[15] != master_row[46]:
                tk.Label(self.info_frame, text=f"Das Wz soll in der Aufnahme: {master_row[46]} sein",fg="red",font=bold_font).grid()
            if selected_row[16] != master_row[47]:
                tk.Label(self.info_frame, text=f"Das Wz soll mit der Zus_Komp1: {master_row[47]} sein",fg="red",font=bold_font).grid()
            if float(selected_row[9])<=(float(master_row[21])-float(master_row[21])*0.2):
                tk.Label(self.info_frame, text=f"Das Werkzeug nicht mehr nachschleifen lassen",fg="orange",font=bold_font).grid()

            if selected_row[16] == master_row[47] and selected_row[15] == master_row[46] and selected_row[14] == master_row[45] and selected_row[11] >= master_row[22]:
                thumb_up = u'\u2713'
                label_text = "Dieses Wz. ist mit SolidCAM identisch"


                tk.Label(self.info_frame, text=label_text, fg="green",font=bold_font).grid()
                tk.Label(self.info_frame, text=thumb_up, fg="green",font=bold_font2).grid()
            button_schnittdaten=tk.Button(self.info_frame,text="Schnittdaten aus SolidCAM",command= lambda: self.schnittdaten(master_row),width=25)
            button_schnittdaten.grid(pady=10)


            button_bemerkungen=tk.Button(self.info_frame,text="Bemerkungen",command= lambda: self.bemerkungen_lesen(selected_row[0]),width=25)
            button_bemerkungen.grid(pady=10)



        else:
            img1_path = (paths["toolCapturedimages"]+selected_row[0]+".png")
            tk.Label(self.info_frame, text=f"Tool: {selected_row[2]}\n \n",font=bold_font).grid(row=0)
            button=tk.Button(self.info_frame, text="+", font=("Helvetica", 14),command= lambda: self.im_liste(img1_path))
            button.grid(row=1,column=2)
            try:
                # Load and display the first image
                img1 = Image.open(img1_path)
                img1 = img1.resize((200, 150), Image.LANCZOS)
                img1_photo = ImageTk.PhotoImage(img1)
                img1_label = Label(self.info_frame, image=img1_photo)
                img1_label.image = img1_photo  # Keep a reference to avoid garbage collection
                img1_label.grid(row=1,column=0,pady=20)
                self.photo_images_list.append(img1_photo)  # Store reference in the list
            except FileNotFoundError:
                # If image 1 is not found, show a default image or handle as desired
                default_img1 = Image.open(paths["imgpaths"]+"NOTFOUND.png")
                default_img1 = default_img1.resize((200, 200), Image.LANCZOS)
                default_img1_photo = ImageTk.PhotoImage(default_img1)
                img1_label = Label(self.info_frame, image=default_img1_photo)
                img1_label.image = default_img1_photo
                img1_label.grid(row=1,column=0,pady=20)
                self.photo_images_list.append(default_img1_photo)  # Store reference in the list
            tk.Label(self.info_frame, text="Das ist ein Sonderwerkzeug",font=bold_font).grid()
            tk.Label(self.info_frame, text="Keine Schnittdaten vorhanden",font=bold_font).grid()



    def checkUP(self):
        print("Checkup")

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
            if place["status"] == "machine" and place["placename"] == self.listname :
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

                    # # Get tool.t
                    # process.stdin.write(f"get TNC:\\table\\tool.t X:\\Projekt\\{ip_address}.t\n")
                    # process.stdin.flush()
                    # time.sleep(1)

                    # Get tool_p.tch
                    process.stdin.write(f"get TNC:\\table\\tool_p.tch X:\\Projekt\\{ip_address}.tch\n")
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
                tch_file_path = f"X:\\Projekt\\{ip_address}.tch"

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
                                tchData.append([tchCol2, tchCol3])

                    print("Extracted Data from .tch:", tchData ,"\n")

                except FileNotFoundError:
                    print(f"Error: File {tch_file_path} not found.")
                    break
                except Exception as e:
                    print(f"An error occurred: {e}")
                    break

                # Connect to the MTMDB.db SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
                
                # Fetch all data from the currentTools table
                cur.execute("SELECT * FROM currentTools")
                currentToolsData = cur.fetchall()
                currentToolDataList=[]

                for tool in currentToolsData:
                    if tool[19]==self.listname:
                        currentToolDataList.append([str(tool[1]),tool[2]])
                    else:
                        pass

                for i in currentToolDataList:
                    print(i)


        # Sort and align matched items
        matched_items = []
        unmatched_tch = []
        unmatched_tools = []

        # Find matched and unmatched items
        for tch_item in tchData:
            if tch_item in currentToolDataList:
                matched_items.append((tch_item, tch_item))
            else:
                unmatched_tch.append((tch_item, None))

        for tool_item in currentToolDataList:
            if tool_item not in tchData:
                unmatched_tools.append((None, tool_item))

        # Combine sorted data
        sorted_data = matched_items + unmatched_tch + unmatched_tools
        # Create the Tkinter window
        top = tk.Toplevel()
        top.title("Vergleich zwischen Maschine u. MTM")
        top.geometry("800x400")

        # Canvas for custom rendering
        canvas = tk.Canvas(top)
        canvas.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)

        # Scrollbar for vertical scrolling
        v_scroll = ttk.Scrollbar(top, orient=tk.VERTICAL, command=canvas.yview)
        v_scroll.pack(side=tk.RIGHT, fill=tk.Y)
        canvas.configure(yscrollcommand=v_scroll.set)

        # Frame inside the canvas
        frame = ttk.Frame(canvas)
        canvas.create_window((0, 0), window=frame, anchor="nw")

        # Ensure scroll region updates
        frame.bind("<Configure>", lambda e: canvas.configure(scrollregion=canvas.bbox("all")))

        # Titles for columns (centered)
        ttk.Label(frame, text="Maschine Tools", font=("Arial", 14, "bold"), anchor="w", width=20).grid(row=0, column=0, padx=5, pady=5)
        ttk.Label(frame, text="Status", font=("Arial", 14, "bold"), anchor="center", width=10).grid(row=0, column=1, padx=5, pady=5)
        ttk.Label(frame, text="MTM Tools", font=("Arial", 14, "bold"), anchor="w", width=20).grid(row=0, column=2, padx=5, pady=5)

        # Populate the sorted data into the table
        for i, (tch_item, tool_item) in enumerate(sorted_data, start=1):
            # MTM Tools column
            if tch_item:
                ttk.Label(frame, text=f"{tch_item[0]} - {tch_item[1]}", width=30, anchor="w",font=("Arial", 12, "bold")).grid(row=i, column=0, padx=5, pady=5)
            else:
                ttk.Label(frame, text="nicht vorhanden", width=30, anchor="w", foreground="gray",font=("Arial", 12, "bold")).grid(row=i, column=0, padx=5, pady=5)

            # Status column
            if tch_item and tool_item and tch_item == tool_item:
                ttk.Label(frame, text="     ✔️", foreground="green", anchor="center",font=("Arial", 13, "bold")).grid(row=i, column=1, padx=5, pady=5)
            else:
                ttk.Label(frame, text="❌", foreground="red", anchor="center",font=("Arial", 12, "bold")).grid(row=i, column=1, padx=5, pady=5)

            # Machine Tools column
            if tool_item:
                ttk.Label(frame, text=f"{tool_item[0]} - {tool_item[1]}", width=30, anchor="w",font=("Arial", 12, "bold")).grid(row=i, column=2, padx=5, pady=5)
            else:
                ttk.Label(frame, text="nicht vorhanden", width=30, anchor="w", foreground="gray",font=("Arial", 12, "bold")).grid(row=i, column=2, padx=5, pady=5)



    def showlists(self):
            top = tk.Toplevel()
            top.title("Tool Data Viewer")
            top.geometry("600x400")

            # Create frames for the listboxes
            self.frame_tch = ttk.Frame(top)
            self.frame_tch.pack(side=tk.LEFT, fill=tk.BOTH, expand=True, padx=10, pady=10)

            self.frame_tools = ttk.Frame(top)
            self.frame_tools.pack(side=tk.RIGHT, fill=tk.BOTH, expand=True, padx=10, pady=10)

            # Label and Listbox for tchData
            self.lbl_tch = ttk.Label(self.frame_tch, text="TCH Data")
            self.lbl_tch.pack()

            self.listbox_tch = tk.Listbox(self.frame_tch, selectmode=tk.SINGLE)
            self.listbox_tch.pack(fill=tk.BOTH, expand=True)

            # Label and Listbox for currentToolDataList
            self.lbl_tools = ttk.Label(self.frame_tools, text="Current Tool Data")
            self.lbl_tools.pack()

            self.listbox_tools = tk.Listbox(self.frame_tools, selectmode=tk.SINGLE)
            self.listbox_tools.pack(fill=tk.BOTH, expand=True)

    def bemerkungen_lesen(self, qr):
        bemerkungen_win = Toplevel()
        bemerkungen_win.title("Bemerkungen")
        bemerkungen_win.geometry("250x250")

        # Use raw string for Windows file paths
        file_path = (paths["toolsremark"]+qr+".txt")

        bemerkung_field = tk.Text(bemerkungen_win, height=15, width=25, font=("Helvetica", 14))
        bemerkung_field.pack(expand=True, fill=tk.BOTH, padx=10, pady=10)

        try:
            with open(file_path, "r") as bemerkung_file:
                text_content = bemerkung_file.read()  # Read content of the file
            
            # Set the widget to normal state to insert text
            bemerkung_field.config(state="normal")
            bemerkung_field.insert("1.0", text_content)  # Insert text at the start of the Text widget
            
            # Set the widget to disabled state to make it readonly
            bemerkung_field.config(state="disabled")
            
        except FileNotFoundError:
            print(f"{file_path} keine Bemerkungen gefunden.")
        except IOError as e:
            print(f"An error occurred while reading the file: {e}")  

    def on_row_click(self, event):
        item = self.table.identify_row(event.y)
        if item:
            self.on_row_click_simulated(item)


    def schnittdaten(self, master_row):
        schnittdaten_win = Toplevel()
        schnittdaten_win.title("SCHNITTDATEN")
        schnittdaten_win.geometry("1300x200")

        def read_json_file(file_path):
            with open(file_path, 'r') as file:
                data = json.load(file)
            return data

        def search_item(data, item):
            return data.get(item, None)

        file_path = paths["feeddata"]
        data = read_json_file(file_path)
        item_key = master_row[0]  # Replace with the key you want to search for
        radius = float(master_row[6])

        item_values = search_item(data, item_key)

        # Create a Treeview widget
        tree = ttk.Treeview(schnittdaten_win, columns=("Material","Schnittdaten zum Schruppen","Schnittdaten zum Schlichten"), show='headings')
        tree.heading("Material", text="Material")
        tree.heading("Schnittdaten zum Schruppen", text="Schnittdaten zum Schruppen")
        tree.heading("Schnittdaten zum Schlichten", text="Schnittdaten zum Schlichten")


        tree.column("Material",width=30,anchor='center')
        tree.column("Schnittdaten zum Schruppen",anchor='w')
        tree.column("Schnittdaten zum Schlichten",anchor='w')


        tree.pack(expand=True, fill='both')

        RED = "\033[31m"
        RESET = "\033[0m"

        if item_values:
            for obj in item_values:
                for material, values in obj.items():
                    drehzahlschruppen = int((values[4]) / (3.14 * (radius / 1000)))
                    vorschubschruppen= int(drehzahlschruppen*values[1])
                    drehzahlschlichten = int((values[5]) / (3.14 * (radius / 1000)))
                    vorschubschlichten=int(drehzahlschlichten*values[3])
                    schruppen=f"Vc= {values[4]} m/min      f/U= {values[1]} mm/min-1      S={drehzahlschruppen} U/min-1      f={vorschubschruppen} mm/min"
                    schlichten=f"Vc= {values[5]} m/min      f/U= {values[3]} mm/min-1      S={drehzahlschlichten} U/min-1      f={vorschubschlichten} mm/min"

                    tree.insert('', 'end', values=( values[0], schruppen,schlichten ))
        else:
            tree.insert('', 'end', values=( f"Item {item_key} not found", "", ""))




    def im_liste(self,imgpath):
        im_list=tk.Toplevel()
        im_list.title("IMAGE")
        im_list.geometry("800x600")


        try:
            # Load and display the first image
            img1 = Image.open(imgpath)
            img1 = img1.resize((800, 600), Image.LANCZOS)
            img1_photo = ImageTk.PhotoImage(img1)
            img1_label = Label(im_list, image=img1_photo)
            img1_label.image = img1_photo  # Keep a reference to avoid garbage collection
            img1_label.grid(row=1,column=0,pady=20)
            self.photo_images_list.append(img1_photo)  # Store reference in the list
        except FileNotFoundError:
            # If image 1 is not found, show a default image or handle as desired
            default_img1 = Image.open(paths["imgpaths"]+"NOTFOUND.png")
            default_img1 = default_img1.resize((800, 800), Image.LANCZOS)
            default_img1_photo = ImageTk.PhotoImage(default_img1)
            img1_label = Label(im_list, image=default_img1_photo)
            img1_label.image = default_img1_photo
            img1_label.grid(row=1,column=0,pady=20)
            self.photo_images_list.append(default_img1_photo)  # Store reference in the list






    # def load_variables_from_json(self,path):
    #     with open(f"D:\\Projekt\\Mo Tool Manager\\motoolmanager\\{path}", 'r') as f:
    #         file_paths = json.load(f)
    #     return file_paths
    