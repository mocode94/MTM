from tkinter import Toplevel, Label, Entry, Button, StringVar, filedialog,Listbox
import tkinter as tk
from tkinter import ttk
from PIL import Image, ImageTk
import os
import csv
import json
import tkinter.font as font
import trimesh
import numpy as np
import re
import sqlite3
from config import mastercsv,winconfig,places,paths


class startOrder():
    def __init__(self, mainFrame):

        self.mainFrame = mainFrame
        mainFrame.config(width=2000, height=1250)

        # Clear the frame before adding new widgets
        self.clearFrame()

        # Create a new frame inside the mainFrame with a specific size
        self.centerFrame = tk.Frame(mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)



        imgNcCodeSearchButton = Image.open(paths["imgpaths"]+"\\button14.png")
        self.imgNcCodeSearchButton = ImageTk.PhotoImage(imgNcCodeSearchButton)

        imgNcCodeSearchButton = tk.Button(self.centerFrame, image=self.imgNcCodeSearchButton, command=self.selectNcFile, width=250, height=250)
        imgNcCodeSearchButton.pack(side="left", padx=10, pady=10)


    def selectNcFile (self):

        # Open the selected text file
        self.PGM_file = filedialog.askopenfilename()

        if self.PGM_file:
            with open(self.PGM_file, 'r') as file:
                lines = file.readlines()
                
                
                # Filter lines that contain "TOOL CALL" (case-insensitive)
                tool_call_lines = [line for line in lines if "TOOL CALL" in line.upper()]

                # Extract strings within quotes from the filtered lines
                extracted_strings = []
                for line in tool_call_lines:
                    parts = line.split('"')
                    for i in range(1, len(parts), 2):  # Get every second element (strings within quotes)
                        extracted_strings.append(parts[i])
   
                # print(extracted_strings)
                vocabulary_file = mastercsv

                # Read the vocabulary from the third column in the CSV file
                vocabulary = []
                try:
                    with open(vocabulary_file, 'r', newline='', encoding='utf-8') as csvfile:
                        csv_reader = csv.reader(csvfile, delimiter=';')  # Specify delimiter
                        next(csv_reader)  # Skip header if exists
                        for row in csv_reader:
                            if len(row) > 1:  # Ensure row has at least 3 columns
                                word = row[1].strip().upper()
                                vocabulary.append(word)
                except FileNotFoundError:
                    print("Vocabulary CSV file not found.")
                except Exception as e:
                    print("Error reading vocabulary CSV file:", e)

                # Now search for vocabulary in the text file
                self.found_vocabulary = [word for word in vocabulary if word.upper() in extracted_strings]
                self.sonder_vocabulary = [word for word in extracted_strings if word.upper() not in vocabulary]

                self.clearFrame()



                labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")


                # Create a listbox to display the found vocabulary
                label_results = Label(self.mainFrame, text="Standard-Werkzeuge",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
                label_results.grid(row=0, column=0, padx=10, pady=5)
                listbox_results = Listbox(self.mainFrame, width=40,height=30,font=labelFontConfiguration)
                listbox_results.grid(row=1, column=0, padx=10, pady=10)

                # Create and grid a label above the second listbox
                label_results_2 = Label(self.mainFrame, text="Sonder-Werkzeuge",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
                label_results_2.grid(row=0, column=1, padx=10, pady=5)
                listbox_results_2 = Listbox(self.mainFrame, width=40,height=30,font=labelFontConfiguration)
                listbox_results_2.grid(row=1, column=1, padx=10, pady=10)

                # Create and grid a label above the third listbox
                label_spannmittel = Label(self.mainFrame, text="Spannmittel",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
                label_spannmittel.grid(row=0, column=2, padx=10, pady=5)
                self.listbox_spannmittel = Listbox(self.mainFrame, width=40,height=5,font=labelFontConfiguration)
                self.listbox_spannmittel.grid(row=1, column=2, padx=10, pady=10)

                # Display the found vocabulary in the listbox
                if self.found_vocabulary:
                    for word in self.found_vocabulary:
                        listbox_results.insert(tk.END, word)
                else:
                    listbox_results.insert(tk.END, "No matching tools found in the NC_PGM file.")

                if self.sonder_vocabulary:
                    for word in self.sonder_vocabulary:
                        listbox_results_2.insert(tk.END, word)
                else:
                    listbox_results_2.insert(tk.END, "No matching tools found in the NC_PGM file.")

                # Connect to the SQLite database
                conn = sqlite3.connect(paths["MTMitems"])
                cursor = conn.cursor()
                
                # Query to fetch all values from the Spannmittel column in the Spannmittel table
                cursor.execute('SELECT Spannmittel FROM Spannmittel')
                results = cursor.fetchall()
                
                # Extract the values and strip any whitespace
                spannmittel = [row[0].strip() for row in results]
                
                # Close the database connection
                conn.close()


                # Assuming 'lines' is defined somewhere in your code
                # Strip newline characters from each line in lines
                lines = [line.strip() for line in lines]


                found = False
                for mittel in spannmittel:
                    for line in lines:
                        if mittel in line:
                            spann = mittel + ".stl"
                            found = True
                            break
                    if found:
                        break

                if not found:
                    spann = "KEIN SPANNMITTEL IST VORGEGEBEN"
                    print("Kein Spannmittel ist vorgegeben")

                            
                

                
                if spann:
                    print(spann)
                    self.listbox_spannmittel.insert(tk.END, spann)
                else:
                    self.listbox_spannmittel.insert(tk.END, "No matching tools found in the NC_PGM file.")

                self.listbox_spannmittel.bind("<<ListboxSelect>>", self.on_spannmittel_select)

                with open(r"D:\Projekt\Mo Tool Manager\motoolmanager\lageort.json", 'r') as json_file:
                    lageort_data = places
                    maschine = []

                    for dictionary in lageort_data :
                        if dictionary["status"] == "machine":
                            maschine.append(dictionary["placename"])

                self.maschine_combobox = ttk.Combobox(self.mainFrame, values=maschine,font=labelFontConfiguration)
                self.maschine_combobox.set("Maschine auswählen")
                self.maschine_combobox.grid(row=1, column=3, padx=10, pady=10)
                self.maschine_combobox_start = str(self.maschine_combobox.get())

                show_button = Button(self.mainFrame, text="Starten", command=lambda: self.auftrag_maschine (str(self.maschine_combobox.get())),font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
                show_button.grid(row=1, column=4, padx=10, pady=10)

    def clearFrame(self):
        # Clear all widgets from the mainFrame
        for widget in self.mainFrame.winfo_children():
            widget.destroy()

    def on_spannmittel_select(self, event):
        selected_index = self.listbox_spannmittel.curselection()
        if selected_index:
            selected_item = self.listbox_spannmittel.get(selected_index)
            
            # Show a Toplevel info window to indicate processing
            self.info_window = tk.Toplevel(self.mainFrame)
            self.info_window.title("Please Wait")
            self.info_window.geometry("300x100")
            self.info_label=tk.Label(self.info_window, text="Processing, please wait...", pady=20)
            self.info_label.pack()
            self.info_window.grab_set()  # Make sure the user can't interact with the main window

            def process_files():
                try:
                    def extract_values(line):
                        x_match = re.search(r'X([+-]?\d+(\.\d+)?)', line)
                        y_match = re.search(r'Y([+-]?\d+(\.\d+)?)', line)
                        z_match = re.search(r'Z([+-]?\d+(\.\d+)?)', line)
                        
                        x = float(x_match.group(1)) if x_match else None
                        y = float(y_match.group(1)) if y_match else None
                        z = float(z_match.group(1)) if z_match else None
                        
                        return x, y, z

                    def extract_verschieben(file_path):
                        with open(file_path, 'r') as file:
                            lines = file.readlines()

                        # Adjusted regex pattern to handle varying whitespace and formats
                        line_pattern = r'\d+\s*;\s*BZP:\s*X\s*=\s*([+-]?\d+(\.\d+)?)\s*Y\s*=\s*([+-]?\d+(\.\d+)?)\s*Z\s*=\s*([+-]?\d+(\.\d+)?)'

                        verschieben_x = None
                        verschieben_y = None
                        verschieben_z = None
                        # Debug: Print lines to ensure correct reading
                        print("Reading lines from file:")
                        for line in lines:
                            match = re.search(line_pattern, line.strip())
                            if match:
                                verschieben_x = float(match.group(1))
                                verschieben_y = float(match.group(3))
                                verschieben_z = float(match.group(5))
                                break
                        
                        if verschieben_x is None:
                            raise ValueError("The required line with X value was not found in the file.")
                        
                        return verschieben_x, verschieben_y, verschieben_z

                    def extract_pgm_name(file_path):
                        with open(file_path, 'r') as file:
                            lines = file.readlines()

                        # Adjusted regex pattern to handle varying whitespace and formats
                        pgm_pattern = r'0\s+BEGIN\s+PGM\s+([^\s]+)\s+MM'

                        pgm_name = None
                        # Debug: Print lines to ensure correct reading
                        print("Reading lines from file to find PGM name:")
                        for line in lines:
                            match = re.search(pgm_pattern, line.strip())
                            if match:
                                pgm_name = match.group(1)
                                break
                        
                        if pgm_name is None:
                            raise ValueError("The required line with PGM name was not found in the file.")
                        
                        return pgm_name

                    def calculate_differences(file_path):
                        with open(file_path, 'r') as file:
                            lines = file.readlines()

                        # Updated patterns to match possible variations in whitespace
                        line1_pattern = r'1\s+BLK\s+FORM\s+0\.1\s+Z\s+X[+-]?\d+(\.\d+)?\s+Y[+-]?\d+(\.\d+)?\s+Z[+-]?\d+(\.\d+)?'
                        line2_pattern = r'2\s+BLK\s+FORM\s+0\.2\s+X[+-]?\d+(\.\d+)?\s+Y[+-]?\d+(\.\d+)?\s+Z[+-]?\d+(\.\d+)?'
                        
                        line1 = None
                        line2 = None
                        
                        for line in lines:
                            if re.match(line1_pattern, line):
                                line1 = line.strip()
                            elif re.match(line2_pattern, line):
                                line2 = line.strip()
                            
                            if line1 and line2:
                                break

                        if not line1 or not line2:
                            raise ValueError("The required lines were not found in the file.")

                        x1, y1, z1 = extract_values(line1)
                        x2, y2, z2 = extract_values(line2)

                        x_diff = x2 - x1
                        y_diff = y2 - y1
                        z_diff = z2 - z1

                        return x_diff, y_diff, z_diff

                    def create_centered_box(width, length, height):
                        print(f"Creating centered box with dimensions (width={width}, length={length}, height={height})")
                        # Define the vertices of the box relative to the center
                        vertices = np.array([
                            [-width / 2, -length / 2, -height / 2],  # Bottom face
                            [ width / 2, -length / 2, -height / 2],
                            [ width / 2,  length / 2, -height / 2],
                            [-width / 2,  length / 2, -height / 2],
                            [-width / 2, -length / 2,  height / 2],  # Top face
                            [ width / 2, -length / 2,  height / 2],
                            [ width / 2,  length / 2,  height / 2],
                            [-width / 2,  length / 2,  height / 2]
                        ])
                        
                        # Define the faces of the box using the vertices
                        faces = np.array([
                            [0, 3, 1], [1, 3, 2],   # Bottom face
                            [4, 5, 7], [5, 6, 7],   # Top face
                            [0, 1, 4], [1, 5, 4],   # Front face
                            [1, 2, 5], [2, 6, 5],   # Right face
                            [2, 3, 6], [3, 7, 6],   # Back face
                            [3, 0, 7], [0, 4, 7]    # Left face
                        ])
                        
                        # Create the mesh
                        box_mesh = trimesh.Trimesh(vertices=vertices, faces=faces)
                        
                        return box_mesh

                    # File path to your text file
                    file_path = self.PGM_file
                    bzp_file_path = file_path  # Use the same file since it contains the BZP line

                    try:
                        # Calculate differences
                        x_diff, y_diff, z_diff = calculate_differences(file_path)
                        print(f"X Difference: {x_diff}")
                        print(f"Y Difference: {y_diff}")
                        print(f"Z Difference: {z_diff}")

                        # Create the box using the calculated differences
                        centered_box = create_centered_box(x_diff, y_diff, z_diff)
                        bauteil_dir=paths["temp"]
                        os.makedirs(bauteil_dir, exist_ok=True)

                        # Export the box as an STL file
                        centered_box_file =  os.path.join(bauteil_dir, 'bauteil.stl')
                        centered_box.export(centered_box_file)
                        print(f"Box STL file created successfully at {os.path.abspath(centered_box_file)}.")
                    except Exception as e:
                        print(f"Error during box creation: {e}")

                    try:
                        # Load the STL files
                        mesh1 = trimesh.load(centered_box_file)
                        mesh2 = trimesh.load(paths["toolstl"]+selected_item)
                        print("STL files loaded successfully.")

                        # Extract verschieben value from the BZP line
                        verschieben_x, verschieben_y, verschieben_z = extract_verschieben(bzp_file_path)
                        print(f"Verschieben values: X={verschieben_x}, Y={verschieben_y}, Z={verschieben_z}")

                        # Calculate the height of mesh2 (max z - min z)
                        mesh2_min_z = mesh2.bounds[0][2]
                        mesh2_max_z = mesh2.bounds[1][2]
                        mesh2_height = mesh2_max_z - mesh2_min_z
                        print(f"Height of mesh2: {mesh2_height}")

                        # Calculate the height of mesh1 (max z - min z)
                        mesh1_min_z = mesh1.bounds[0][2]
                        mesh1_max_z = mesh1.bounds[1][2]
                        mesh1_height = mesh1_max_z - mesh1_min_z
                        print(f"Height of mesh1: {mesh1_height}")

                        # Extract PGM name
                        pgm_name = extract_pgm_name(file_path)
                        print(f"PGM Name: {pgm_name}")

                        # Translate mesh1 to place it on top of mesh2, with verschieben value
                        transform = trimesh.transformations.translation_matrix([verschieben_x, verschieben_y, (mesh2_max_z - mesh1_min_z) - 3])
                        mesh1.apply_transform(transform)
                        print("Transformation applied to mesh1.")

                        # Combine the meshes
                        combined_mesh = trimesh.util.concatenate([mesh2, mesh1])  # Ensure mesh2 is at the bottom
                        print("Meshes combined successfully.")

            # Define the directory where you want to save the STL file
                        output_directory = paths["temp"]  # Change this path as needed

                        # Ensure the directory exists
                        os.makedirs(output_directory, exist_ok=True)

                        # Construct the full file path with directory and PGM name
                        combined_mesh_file = os.path.join(output_directory, (f'{pgm_name}.stl'))
                        combined_mesh.export(combined_mesh_file)
                        print(f"Combined STL file created successfully at {os.path.abspath(combined_mesh_file)}")
                    except Exception as e:
                        print(f"Error during STL processing: {e}")
                        
                    print("ENDEEEE")

                    print(output_directory+f'\{pgm_name}.stl')
                    

                    self.info_window.destroy()

                    self.visualize_stl(output_directory+f'\{pgm_name}.stl')

                except Exception as e:
                    print(f"Error during processing: {e}")
                finally:
                    # Close the info window after processing is complete
                    self.info_window.destroy()

            # Use after to run the method in the background
            self.mainFrame.after(100, process_files)

    def visualize_stl(self, file_path, width=800, height=600):
        import vtk

        # Path to your STL file
        

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

        # Create an interactor
        render_window_interactor = vtk.vtkRenderWindowInteractor()
        render_window_interactor.SetRenderWindow(render_window)

        # Add the actor to the scene
        renderer.AddActor(actor)
        renderer.SetBackground(0.8, 0.8, 0.8)  # Background color white

        # Set up interactor style to allow rotation
        style = vtk.vtkInteractorStyleTrackballActor()
        render_window_interactor.SetInteractorStyle(style)

        # Start the rendering loop
        render_window.Render()
        render_window_interactor.Start()


    def auftrag_maschine(self,lageort): 
 
        auftrag_werkzeugsliste = tk.Toplevel()
        auftrag_werkzeugsliste.title("List in Table")
        auftrag_werkzeugsliste.geometry("2500x800")



        self.info_frame = tk.Frame(auftrag_werkzeugsliste)
        self.info_frame.pack(side=tk.RIGHT, fill=tk.Y, padx=10, pady=10)

        frame = tk.Frame(auftrag_werkzeugsliste)
        frame.pack(side=tk.BOTTOM, fill=tk.Y)

        unmatched_frame = tk.Frame(frame)
        unmatched_frame.pack(side=tk.LEFT, fill=tk.Y, padx=10, pady=10)

        title_label = tk.Label(unmatched_frame, text="Fehlende Werkzeuge", font=("Arial", 10, "bold"))
        title_label.pack(anchor='w', pady=10,padx=10)

        listbox1 = tk.Listbox(unmatched_frame, height=10, width=50)
        listbox1.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        scrollbar1 = tk.Scrollbar(unmatched_frame, orient=tk.VERTICAL, command=listbox1.yview)
        scrollbar1.pack(side=tk.RIGHT, fill=tk.Y)
        listbox1.config(yscrollcommand=scrollbar1.set)

        # Create the second frame and listbox for "nicht erstellte Werkzeuge"
        unmatched_frame2 = tk.Frame(frame)
        unmatched_frame2.pack(side=tk.LEFT, fill=tk.Y, padx=10, pady=10)
        
        unmatched_frame22 = tk.Frame(frame)
        unmatched_frame22.pack(side=tk.LEFT, fill=tk.Y, padx=10, pady=10)

        title_label2 = tk.Label(unmatched_frame2, text="nicht erstellte Werkzeuge", font=("Arial", 10, "bold"))
        title_label2.pack(anchor='w', pady=10,padx=10)

        listbox2 = tk.Listbox(unmatched_frame2, height=10, width=50)
        listbox2.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        scrollbar2 = tk.Scrollbar(unmatched_frame2, orient=tk.VERTICAL, command=listbox2.yview)
        scrollbar2.pack(side=tk.RIGHT, fill=tk.Y)
        listbox2.config(yscrollcommand=scrollbar2.set)



        listbox1_notiz=Button(unmatched_frame,text="Notizien",command= lambda: self.fehlende_wz(listbox1))
        listbox1_notiz.pack(padx=5,pady=5)

        listbox2_notiz=Button(unmatched_frame2,text="Notizien",command= lambda: self.nicht_erstellte_wz(listbox2))
        listbox2_notiz.pack(padx=5,pady=5)

        kollision=Button(unmatched_frame22,text="Kollision Kontrolle",command= lambda: self.kollision_kontrolle(self.PGM_file,self.lageort_auswahl),width=30)
        kollision.pack(padx=20,pady=20)


    # Create a frame for the Treeview on the right side
        table_frame = tk.Frame(auftrag_werkzeugsliste)
        table_frame.pack(side=tk.RIGHT, expand=True, fill="both")


        self.table = ttk.Treeview(auftrag_werkzeugsliste, columns=('QR', 'T_Nummer', 'T_Name','Typ', 'T_Radius', 'Eckenradius','Spitzenwinkel','Eintauchwinkel','Schneiden','Schnittlaenge' ,'Ausspann_Laenge'
                                                  ,'IST_Laenge', 'SOLL_Laenge', 'Steigung','Wzg_ArtNr', 'Aufn_ArtNr', 'zus_Komp', 'zus_Komp2', 'Spann_Sys', 'Wo'))
        self.table.column('#0', width=0, stretch=tk.NO)
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

        self.table.column('QR', width=50, anchor='center')
        self.table.column('T_Nummer', width=60, anchor='center')
        self.table.column('T_Name', width=110, anchor='center')
        self.table.column('Typ', width=30, anchor='center')
        self.table.column('T_Radius', width=80, anchor='center')
        self.table.column('Eckenradius', width=90, anchor='center')
        self.table.column('Spitzenwinkel', width=110, anchor='center')
        self.table.column('Eintauchwinkel', width=110, anchor='center')
        self.table.column('Schneiden', width=80, anchor='center')
        self.table.column('Schnittlaenge', width=100, anchor='center')
        self.table.column('Ausspann_Laenge', width=110, anchor='center')
        self.table.column('IST_Laenge', width=90, anchor='center')
        self.table.column('SOLL_Laenge', width=90, anchor='center')
        self.table.column('Steigung', width=80, anchor='center')
        self.table.column('Wzg_ArtNr', width=90, anchor='center')
        self.table.column('Aufn_ArtNr', width=90, anchor='center')
        self.table.column('zus_Komp', width=80, anchor='center')
        self.table.column('zus_Komp2', width=80, anchor='center')
        self.table.column('Spann_Sys', width=80, anchor='center')
        self.table.column('Wo', width=40, anchor='center')

        self.table.tag_configure('redcell', background='orange')
        self.table.tag_configure('greencell', background='lightgreen')
        self.table.tag_configure('bluecell', background='lightblue')
        self.table.tag_configure('orangecell', background='orange')
        self.table.tag_configure('defaultcell', background='white')
        self.table.pack(expand=True, fill="both")

        self.current_image = None
        self.photo_images_list = []

        self.lageort_auswahl=lageort

        # Read data from the first CSV file
        tabelle_data = []

        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()


        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()


        for row in currentToolsData:
            if row[19]==lageort:
                tabelle_data.append(row)

        # Sort tabelle_data based on the Nummer column
        tabelle_data.sort(key=lambda x: int(x[1]) if str(x[1]).isdigit() else float('inf'))



        # Read data from the second CSV file
        tabelle2_data = []
        with open(paths["mastercsv"], "r") as file:
            reader = csv.reader(file, delimiter=";")
            for row in reader:
                tabelle2_data.append(row)

        tabelle_data_names = [row[2] for row in tabelle_data]

        # Find and print items in filter_list not in tabelle_data_names
        unmatched_items = [item for item in self.found_vocabulary if item not in tabelle_data_names ] + [item for item in self.sonder_vocabulary if item not in tabelle_data_names ]
        # print("Items not matched in tabelle_data[2]:", unmatched_items)

        def search_csv():
            search_items = unmatched_items
            for item in search_items:
                print(f"here is {item}")
            #print(search_items)
            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()


            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()


            found_items = {}  # Use a dictionary to store items and their corresponding values
            for row in currentToolsData:
                if len(row) > 19:  # Check if the row has enough columns
                    for search_item in search_items:
                        if row[2].strip() == search_item.strip():
                            item = row[2]
                            value = row[19]
                            if item not in found_items:
                                found_items[item] = [value]
                                print(f" if here is item with lager {item} + {value}")
                            else:
                                found_items[item].append(value)
                                print(f" else here is item with lager {item} + {value}")

            keys=list(found_items.keys())
            uni = list(set(search_items) - set(keys)) + [x for x in keys if x not in search_items]

            for item in uni:
                listbox2.insert(tk.END, item)
                
            # for item in uni:
            #     self.listbox1_notiz.insert(tk.END, item)


            return found_items
        


        results = search_csv()

        if results:
            for item, values in results.items():
                listbox1.insert(tk.END, f"{item} in {values}")
               # print("Item:", item, "Value:", ", ".join(values))
        else:
            print("Keine passende Werkzeuge")



        for i, t_row in enumerate(tabelle_data):
            if t_row[2] not in self.found_vocabulary:
                continue

            # self.table.insert('', 'end', values=t_row, tags=('redcell',))
            if t_row[19] == lageort:
                 
                # Check if t_row[2] exists in tabelle2_data[i][2] and t_row[5] matches tabelle2_data[i][5]
                if t_row[2] in [row[1] for row in tabelle2_data] and\
                    t_row[11] >= float([row[22] for row in tabelle2_data if row[1] == t_row[2]][0]) and\
                    t_row[14] == [row[45] for row in tabelle2_data if row[1] == t_row[2]][0]and\
                    t_row[15] == [row[46] for row in tabelle2_data if row[1] == t_row[2]][0]and\
                    t_row[16] == [row[47] for row in tabelle2_data if row[1] == t_row[2]][0] :
                    # t_row[6]=[row[22] for row in tabelle2_data if row[1] == t_row[2]]
                    
                    self.table.insert('', 'end', values=t_row, tags=('greencell',))

                elif t_row[2] not in [row[1] for row in tabelle2_data]:
                    self.table.insert('', 'end', values=t_row, tags=('bluecell',))  

                elif t_row[11] < float([row[22] for row in tabelle2_data if row[1] == t_row[2]][0]) or \
                    t_row[14] != [row[45] for row in tabelle2_data if row[1] == t_row[2]][0] or \
                    t_row[15] != [row[46] for row in tabelle2_data if row[1] == t_row[2]][0]or \
                    t_row[16] != [row[47] for row in tabelle2_data if row[1] == t_row[2]][0]:
                    # t_row[6]=[row[22] for row in tabelle2_data if row[1] == t_row[2]]
                    self.table.insert('', 'end', values=t_row, tags=('orangecell',))

        self.table.bind("<ButtonRelease-1>", self.on_row_click_auftrag)

        auftrag_werkzeugsliste.mainloop()


    def on_row_click_simulated(self, item):
        selected_row = self.table.item(item, "values")
        for widget in self.info_frame.winfo_children():
            widget.destroy()
        bold_font = ("Helvetica", 10, "bold")
        bold_font2 = ("Helvetica", 50, "bold")

        maschine_data = []
        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()


        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()

        for row in currentToolsData:
            maschine_data.append(row)

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
                img1 = img1.resize((200, 200), Image.LANCZOS)
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
                tk.Label(self.info_frame, text=f"Das Werkzeug soll: {master_row[45]}",fg="red",font=bold_font).grid()
            if selected_row[15] != master_row[46]:
                tk.Label(self.info_frame, text=f"Das Werkzeug soll in der Aufnahme: {master_row[46]} gebaut werden",fg="red",font=bold_font).grid()
            if selected_row[16] != master_row[47]:
                tk.Label(self.info_frame, text=f"Das Werkzeug soll mit der Zus_Komp1: {master_row[47]} gebaut werden",fg="red",font=bold_font).grid()
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
                img1 = img1.resize((200, 200), Image.LANCZOS)
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

    def bemerkungen_lesen(self, qr):
        bemerkungen_win = Toplevel()
        bemerkungen_win.title("Bemerkungen")
        bemerkungen_win.geometry("250x250")

        # Use raw string for Windows file paths
        file_path = paths["toolsremark"]+{qr}+".txt"

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
            
            print(file_path)
        except FileNotFoundError:
            print(f"{file_path} keine Bemerkungen gefunden.")
        except IOError as e:
            print(f"An error occurred while reading the file: {e}")  

    def on_row_click_auftrag(self, event):
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
        im_list.geometry("800x800")


        try:
            # Load and display the first image
            img1 = Image.open(imgpath)
            img1 = img1.resize((800, 800), Image.LANCZOS)
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



    def fehlende_wz(self, listbox1):
        fehlende_wz = tk.Toplevel()
        fehlende_wz.title("Fehlende Werkzeuge")
        fehlende_wz.geometry("400x400")
        fehlende_wz.attributes("-topmost", True)

        canvas = tk.Canvas(fehlende_wz)
        scrollbar = tk.Scrollbar(fehlende_wz, orient="vertical", command=canvas.yview)
        scrollable_frame = tk.Frame(canvas)

        scrollable_frame.bind(
            "<Configure>",
            lambda e: canvas.configure(
                scrollregion=canvas.bbox("all")
            )
        )

        canvas.create_window((0, 0), window=scrollable_frame, anchor="nw")
        canvas.configure(yscrollcommand=scrollbar.set)

        canvas.pack(side="left", fill="both", expand=True)
        scrollbar.pack(side="right", fill="y")

        # Create a checkbox and label for each item in listbox1
        for item in listbox1.get(0, tk.END):
            var = tk.IntVar()
            
            # Create a frame for each item to ensure they are stacked vertically
            item_frame = tk.Frame(scrollable_frame)
            item_frame.pack(anchor='w')
            label = tk.Label(item_frame, text=item+"   ")
            label.pack(side="left")
            checkbox = tk.Checkbutton(item_frame, variable=var)
            checkbox.pack(side="left")



            # Define the strike_text function within a closure to capture current var and label
            def strike_text(var=var, label=label):
                if var.get():
                    label.config(font=("Arial", 10, "overstrike"))
                else:
                    label.config(font=("Arial", 10, "normal"))

            var.trace_add("write", lambda *args, var=var, label=label: strike_text(var, label))


    def nicht_erstellte_wz(self, listbox2):
        nicht_erstellte_wz = tk.Toplevel()
        nicht_erstellte_wz.title("Nicht Erstellte Werkzeuge")
        nicht_erstellte_wz.geometry("400x400")
        nicht_erstellte_wz.attributes("-topmost", True)

        canvas = tk.Canvas(nicht_erstellte_wz)
        scrollbar = tk.Scrollbar(nicht_erstellte_wz, orient="vertical", command=canvas.yview)
        scrollable_frame = tk.Frame(canvas)

        scrollable_frame.bind(
            "<Configure>",
            lambda e: canvas.configure(
                scrollregion=canvas.bbox("all")
            )
        )

        canvas.create_window((0, 0), window=scrollable_frame, anchor="nw")
        canvas.configure(yscrollcommand=scrollbar.set)

        canvas.pack(side="left", fill="both", expand=True)
        scrollbar.pack(side="right", fill="y")

        # Create a checkbox and label for each item in listbox2
        for item in listbox2.get(0, tk.END):
            var = tk.IntVar()
            
            # Create a frame for each item to ensure they are stacked vertically
            item_frame = tk.Frame(scrollable_frame)
            item_frame.pack(anchor='w')
            label = tk.Label(item_frame, text=item+"   ")
            label.pack(side="left")
            checkbox = tk.Checkbutton(item_frame, variable=var)
            checkbox.pack(side="left")



            # Define the strike_text function within a closure to capture current var and label
            def strike_text(var=var, label=label):
                if var.get():
                    label.config(font=("Arial", 10, "overstrike"))
                else:
                    label.config(font=("Arial", 10, "normal"))

            var.trace_add("write", lambda *args, var=var, label=label: strike_text(var, label))



   
    def kollision_kontrolle(self,file_path,lage):
        kollision_kontrolle = tk.Tk()
        kollision_kontrolle.title("Z Value Analysis")

        frame = ttk.Frame(kollision_kontrolle, padding="10")
        frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))

        text_widget = tk.Text(frame, height=10, width=100, wrap=tk.WORD, bg="white")
        treeview = ttk.Treeview(frame, columns=("last_tool_name", "t_row_first_z_value", "first_z_value", "t_row_largest_z_value", "largest_z_value"), show="headings", height=10)
        listbox3 = tk.Listbox(frame, height=10, width=100)
        listbox4 = tk.Listbox(frame, height=10, width=100)

        label_text_widget = tk.Label(frame, text="i.O.", font=("Helvetica", 10, "bold"))
        label_text_widget.grid(row=0, column=0)
        label_treeview = tk.Label(frame, text="NIO", font=("Helvetica", 10, "bold"))
        label_treeview.grid(row=2, column=0)
        label_listbox3 = tk.Label(frame, text="Keine Z-Zustellung gefunden", font=("Helvetica", 10, "bold"))
        label_listbox3.grid(row=4, column=0)
        label_listbox4 = tk.Label(frame, text="nicht gefunden in der Maschinen-Wz-Liste", font=("Helvetica", 10, "bold"))
        label_listbox4.grid(row=6, column=0)

        text_widget.grid(pady=10, padx=10, row=1, column=0, sticky=(tk.W, tk.E))
        treeview.grid(pady=10, padx=10, row=3, column=0, sticky=(tk.W, tk.E))
        listbox3.grid(pady=10, padx=10, row=5, column=0, sticky=(tk.W, tk.E))
        listbox4.grid(pady=10, padx=10, row=7, column=0, sticky=(tk.W, tk.E))

        scrollbar1 = tk.Scrollbar(frame, orient=tk.VERTICAL, command=text_widget.yview)
        scrollbar2 = tk.Scrollbar(frame, orient=tk.VERTICAL, command=treeview.yview)
        scrollbar3 = tk.Scrollbar(frame, orient=tk.VERTICAL, command=listbox3.yview)
        scrollbar4 = tk.Scrollbar(frame, orient=tk.VERTICAL, command=listbox4.yview)

        text_widget.configure(yscrollcommand=scrollbar1.set)
        treeview.configure(yscrollcommand=scrollbar2.set)
        listbox3.configure(yscrollcommand=scrollbar3.set)
        listbox4.configure(yscrollcommand=scrollbar4.set)

        scrollbar1.grid(row=1, column=1, sticky=(tk.N, tk.S))
        scrollbar2.grid(row=3, column=1, sticky=(tk.N, tk.S))
        scrollbar3.grid(row=5, column=1, sticky=(tk.N, tk.S))
        scrollbar4.grid(row=7, column=1, sticky=(tk.N, tk.S))

        # Define columns
        treeview.heading("last_tool_name", text="Tool Name")
        treeview.heading("t_row_first_z_value", text="Wz-Schnittlänge")
        treeview.heading("first_z_value", text="die erste Zustellung")
        treeview.heading("t_row_largest_z_value", text="Wz-Aussspannlänge")
        treeview.heading("largest_z_value", text="die größte Zustellung")

        # Configure columns to be center-aligned
        treeview.column("last_tool_name", anchor="w",width=300)
        treeview.column("t_row_first_z_value", anchor="center")
        treeview.column("first_z_value", anchor="center")
        treeview.column("t_row_largest_z_value", anchor="center")
        treeview.column("largest_z_value", anchor="center")

        # Add tags and configure tag styles
        treeview.tag_configure("collision", background="lightcoral")
        text_widget.tag_configure("green_bold", foreground="green", font=("Helvetica", 10, "bold"))

        # Example usage
        print(lage)
        
        def find_negative_z_values(file_path, text_widget, treeview, listbox3, listbox4, lage):
            try:
                # Open and read the text file
                with open(file_path, 'r') as file:
                    lines = file.readlines()
            except FileNotFoundError:
                print("The specified file was not found.")
                return None
            except IOError as e:
                print(f"Error reading file: {e}")
                return None

            # Define the search patterns
            tool_call_pattern = re.compile(r'TOOL CALL')
            tool_call_complete_pattern = re.compile(r'TOOL CALL\s+"([^"]+)"')
            cycl_def_pattern = re.compile(r'CYCL DEF')
            z_value_pattern = re.compile(r'Z\s*(-?\d+(\.\d+)?)')
            q201_pattern = re.compile(r'Q201\s*=\s*(-?\d+(\.\d+)?)')
            line_number_pattern = re.compile(r'^\d')

            results = []
            last_tool_name = None

            i = 0
            line_number = 0
            correct=0
            while i < len(lines):
                line = lines[i].strip()
                if line_number_pattern.match(line):
                    line_number += 1
                
                if tool_call_pattern.search(line):
                    tool_call_line = line.strip()
                    tool_call_match = tool_call_complete_pattern.search(line)
                    
                    if tool_call_match:
                        last_tool_name = tool_call_match.group(1)
                    else:
                        # If the tool name is missing, use the last found tool name
                        if last_tool_name:
                            tool_call_line = re.sub(r'TOOL CALL', f'TOOL CALL "{last_tool_name}"', tool_call_line)

                    # Get the line number for the current tool call
                    correct+=1
                    tool_call_line_number = line_number-correct

                    # Now search for the negative Z values until the next TOOL CALL
                    first_z_value = None
                    largest_z_value = None
                    i += 1
                    while i < len(lines):
                        line = lines[i].strip()
                        if line_number_pattern.match(line):
                            line_number += 1
                        
                        if tool_call_pattern.search(line):
                            break  # Stop searching if another TOOL CALL is found
                        
                        if cycl_def_pattern.search(line):
                            i += 1
                            continue  # Skip lines with CYCL DEF
                        
                        # Check for Z values directly
                        match = z_value_pattern.search(line)
                        if match:
                            z_value = match.group(1)
                            if z_value.startswith('-'):
                                if first_z_value is None:
                                    first_z_value = z_value
                                if largest_z_value is None or float(z_value) < float(largest_z_value):
                                    largest_z_value = z_value

                        # Check for specific Q201 variable representing a negative Z value
                        q201_match = q201_pattern.search(line)
                        if q201_match:
                            q201_value = q201_match.group(1)
                            if q201_value.startswith('-'):
                                if first_z_value is None:
                                    first_z_value = q201_value
                                if largest_z_value is None or float(q201_value) < float(largest_z_value):
                                    largest_z_value = q201_value
                        
                        i += 1

                    if first_z_value is None:
                        first_z_value = "Z Werte nicht gefunden"
                        largest_z_value = "Z Werte nicht gefunden"
                        listbox3.insert(tk.END, f"{last_tool_name}       Satz {tool_call_line_number}")

                    results.append((last_tool_name, first_z_value, largest_z_value, tool_call_line_number))

                else:
                    i += 1

            maschine_line = set()
            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()


            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()

            for row in currentToolsData:
                if len(row) >= 11 and row[19] == lage:  # Ensure there are at least 11 columns in the row
                    maschine_line.add((row[2], row[9], row[10]))  # Add a tuple to the set

            maschine_line = list(maschine_line)

            for last_tool_name, first_z_value, largest_z_value, line_number in results:
                found = False
                for t_row in maschine_line:
                    if last_tool_name == t_row[0]:
                        found = True
                        if first_z_value == "Z Werte nicht gefunden" or largest_z_value == "Z Werte nicht gefunden":
                            listbox3.insert(tk.END, f"{last_tool_name}       Satz {line_number}")
                        else:
                            try:
                                t_row_first_z_value = float(t_row[1]) * -1
                                t_row_largest_z_value = float(t_row[2]) * -1
                                first_z_value = float(first_z_value)
                                largest_z_value = float(largest_z_value)

                                if t_row_first_z_value > first_z_value or t_row_largest_z_value > largest_z_value:
                                    treeview.insert("", "end", values=(f"{last_tool_name}       Satz {line_number}", t_row_first_z_value, first_z_value, t_row_largest_z_value, largest_z_value), tags=("collision",))
                                else:
                                    text_widget.insert(tk.END, f"{last_tool_name}       Satz {line_number} i.O. \n", ("green_bold",))
                            except ValueError as e:
                                print(f"Error converting values to float: {e}")
                        break

                if not found:
                    listbox4.insert(tk.END, f"{last_tool_name}       Satz {line_number}")

        # Call the function with the necessary parameters
        find_negative_z_values(file_path, text_widget, treeview, listbox3, listbox4, lage)
