import tkinter as tk
from tkinter import ttk
from PIL import Image, ImageTk
import tkinter.messagebox as messagebox
import tkinter.font as font
from collections import Counter
import smtplib
import ssl
from email.message import EmailMessage
import sqlite3
from config import winconfig,paths



#                                        Aufnahme registrieren
#==========================================================================================================
#==========================================================================================================
#==========================================================================================================
class holderModule:
        def __init__(self,mainFrame):

            self.mainFrame = mainFrame
            mainFrame.config(width=2000, height=1250)

            # Clear the frame before adding new widgets
            self.clearFrame()

            # Create a new frame inside the mainFrame with a specific size
            self.centerFrame = tk.Frame(mainFrame, bg=winconfig["bgcolor"])
            self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)

            img_aufnahme_registrieren = Image.open(paths["imgpaths"]+"button10.png")
            self.img_aufnahme_registrieren = ImageTk.PhotoImage(img_aufnahme_registrieren)

            img_register = Image.open(paths["imgpaths"]+"button11.png")
            self.img_register = ImageTk.PhotoImage(img_register)


            # Create buttons with the images
            self.button_werkzeug_bauen = tk.Button(self.centerFrame, image=self.img_aufnahme_registrieren, command=self.login_seite, width=250, height=250)
            self.button_werkzeug_bauen.grid(pady=10, padx=10, row=0, column=0)

            self.button_werkzeug_editieren = tk.Button(self.centerFrame, image=self.img_register, command=self.lagestand, width=250, height=250)
            self.button_werkzeug_editieren.grid(pady=10, padx=10, row=0, column=1)

        def clearFrame(self):
            # Clear all widgets from the mainFrame
            for widget in self.mainFrame.winfo_children():
                widget.destroy()

        def goHome(self):
            self.centerFrame.destroy()
            # self.rightFrame.destroy()
            self.__init__(self.mainFrame)


        def login_seite(self):

            self.clearFrame()
            self.center= tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
            self.center.place(relx=0.5, rely=0.5, anchor=tk.CENTER)


            label_font = font.Font(family=winconfig["fonttype"], size=20, weight="bold")

            self.username_label = tk.Label(self.center, text="Username:",font=label_font,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            self.username_label.pack(pady=5)
            
            self.username_entry = tk.Entry(self.center,font=(winconfig["bgcolor"], winconfig["fontsize"]))
            self.username_entry.pack(pady=5)
            self.username_entry.focus_set()
        

            self.password_label = tk.Label(self.center, text="Password:",font=label_font,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            self.password_label.pack(pady=5)

            self.password_entry = tk.Entry(self.center, show="*",font=(winconfig["fonttype"], winconfig["fontsize"]))
            self.password_entry.pack(pady=5)

            self.login_button = tk.Button(self.center, text="Login", command=self.login_pruefen,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
            self.login_button.pack(pady=20)
            self.password_entry.bind('<Return>', lambda event: self.login_button.invoke())

        def login_pruefen(self):
            username = self.username_entry.get()
            password = self.password_entry.get()

            if username == "a" and password == "a":  # Simple check for demonstration
                # self.center.pack_forget()
                self.framehalter()
            else:
                messagebox.showerror("Login Failed", "Invalid username or password")
            # self.center.destroy()

        def framehalter(self,event=None):
            

            self.clearFrame()
            self.center= tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
            self.center.place(relx=0.5, rely=0.5, anchor=tk.CENTER)

            txt_file = paths["MTMitems"]
            self.options = set()
            self.entries = []
            self.txt_file = txt_file



            try:
                # Connect to the database
                with sqlite3.connect(txt_file) as conn:
                    cursor = conn.cursor()
                    
                    # Assuming the table is named 'items' and has a column 'name'
                    cursor.execute("SELECT artikelNumber FROM items;")
                    
                    # Fetch and process rows
                    for row in cursor.fetchall():
                        self.options.update([row[0].strip()])  # Add the value to options
                        
            except sqlite3.Error as e:
                print(f"Error: {e}")
            except FileNotFoundError:
                print(f"Error: File '{txt_file}' not found.")


            # try:
            #     with open(self.txt_file, 'r') as file:
            #         for line in file:
            #             self.options.update([line.strip()])
            # except FileNotFoundError:
            #     print(f"Error: File '{self.txt_file}' not found.")


            self.options = list(self.options)

            label_font = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")


            self.label_QR = tk.Label(self.center, text="QR Code scannen",font=label_font,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            self.label_QR.grid(row=2,column=2,padx=10,pady=10)

            self.entry = tk.Entry(self.center,font=(winconfig["fonttype"], winconfig["fontsize"]))
            self.entry.grid(row=2,column=3,padx=10,pady=10)
            self.entry.focus_set()

            self.label_Aufnwahl = tk.Label(self.center, text="Aufn. ArtNr",font=label_font,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            self.label_Aufnwahl.grid(row=2,column=0)

            self.combo_halter = ttk.Combobox(self.center,font=(winconfig["fonttype"], winconfig["fontsize"]))
            self.combo_halter.grid(row=2,column=1)

            self.add_button = tk.Button(self.center, text="Einfügen", command=self.add_entry,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
            self.add_button.grid(row=2,column=4,padx=10,pady=10)
            self.entry.bind('<Return>', lambda event: self.add_button.invoke())


            self.print_button = tk.Button(self.center, text="Aufn. registrieren", command=self.add_halter,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
            self.print_button.place(x=220,y=120)

            self.print_button = tk.Button(self.center, text="letzte QR löschen", command=self.delete_last_entry,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
            self.print_button.place(x=220,y=180)


            self.listbox = tk.Listbox(self.center, bg='white',font=(winconfig["fonttype"], winconfig["fontsize"]))
            self.listbox.place_forget()

            self.label_aufliste = tk.Label(self.center, text="Liste",font=label_font,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            self.label_aufliste.grid(row=3,column=0)

            self.listbox2 = tk.Listbox(self.center, height=25, width=20,font=(winconfig["fonttype"], winconfig["fontsize"]))  # Adjust height and width as needed
            self.listbox2.grid(row=4,column=0,padx=15)

            self.image_label_halter = tk.Label(self.center)  # Label to display the image
            self.image_label_halter.place(x=500,y=100)
            

            self.combo_halter.bind('<KeyRelease>', self.update_listbox)
            self.listbox.bind('<ButtonRelease-1>', self.select_option)
            self.combo_halter.bind('<FocusOut>', self.hide_listbox)
            self.listbox.bind('<FocusOut>', self.hide_listbox)
            self.combo_halter.bind("<<ValueChanged>>",self.select_option)

        def update_listbox(self, event):
            typed_text = self.combo_halter.get()
            if typed_text == '':
                self.listbox.place_forget()
            else:
                filtered_options = [option for option in self.options if typed_text.lower() in option.lower()]
                self.listbox.delete(0, tk.END)
                for option in filtered_options:
                    self.listbox.insert(tk.END, option)
                if filtered_options:
                    self.listbox.place(x=self.combo_halter.winfo_x(), y=self.combo_halter.winfo_y() + self.combo_halter.winfo_height(), width=self.combo_halter.winfo_width())

        def select_option(self, event):
            self.clicked_index = self.listbox.nearest(event.y)
            self.click_index=self.combo_halter.get()
            self.selected_option = self.listbox.get(self.clicked_index)
            self.combo_halter.set(self.selected_option)
            self.listbox.place_forget()



            try:
                # Connect to the database
                with sqlite3.connect(self.txt_file) as conn:
                    cursor = conn.cursor()
                    
                    # Assuming the table is named 'items' and has a column 'artikelNumber'
                    cursor.execute("SELECT artikelNumber FROM items;")
                    
                    # Fetch and process rows
                    for line in cursor.fetchall():
                        line_value = line[0].strip()  # Extract the first value and strip whitespace
                        if line_value == self.combo_halter.get():
                            self.aufnahme_img = line_value
                            im = paths["compimgs"] + self.aufnahme_img + '.jpg'
                            self.show_image(im, width=400, height=400)
                            
            except sqlite3.Error as e:
                print(f"Error: {e}")
            except FileNotFoundError:
                print(f"Error: File '{self.txt_file}' not found.")




            # with open(self.txt_file, "r") as file:  # Assuming werkzeugetxt is the path to the .txt file
            #     for line in file:
            #         line = line.strip()
            #         if line == self.combo_halter.get():
            #             self.aufnahme_img = line
            #             im = paths["compimgs"] + self.aufnahme_img+'.jpg'
            #             self.show_image(im, width=400, height=400)
            

            # file.close()
    
        def show_image(self, image_path, width, height):
            try:
                # Attempt to load the specified image
                image = Image.open(image_path)
                image = image.resize(( width, height), Image.LANCZOS)  # Resize the image to fit the label
                image = image.rotate(270, expand=True)
            except (FileNotFoundError):
            # Load the image
                image = Image.open(paths["compimgs"]+"NOTFOUND.png")
                image = image.resize(( 200, height), Image.LANCZOS)  # Resize the image to fit the label
                image = image.rotate(0, expand=True)
            photo = ImageTk.PhotoImage(image)
            
            # Update the image label
            self.image_label_halter.config(image=photo)
            self.image_label_halter.image = photo

        def hide_listbox(self, event):
            self.listbox.place_forget()

        def delete_last_entry(self):
            if self.entries:
                self.entries.pop()  # Remove the last item from the list
                self.listbox2.delete(tk.END)

        def add_entry(self):
                entry_text = self.entry.get()
                self.combo_halter.state(["disabled"])
                # self.entry1.config(state='disabled')
                if entry_text:
                    self.entries.append(entry_text)
                    self.listbox2.insert(tk.END, entry_text)
                    self.entry.delete(0, tk.END)

        def add_halter(self):
            import sqlite3

            # Get the selected aufnartnr
            aufnartnr = self.combo_halter.get() if self.listbox.curselection() == () else self.listbox.get(self.clicked_index)

            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()
            
            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()

            # Process each QR in self.entries
            for QR in self.entries:
                qr_found = False
                for row in currentToolsData:
                    if len(row) > 0 and row[0] == QR:  # Check if QR code is already registered
                        messagebox.showerror("Warning", f"Dieses Aufnahme {QR} ist schon registriert !!!")
                        qr_found = True
                        break
                
                # If QR not found, ask for confirmation and insert into the database
                if not qr_found:
                    if messagebox.askyesno("Bestätigung", f"Wollen Sie die Aufnahme {QR} mit Aufnahme_ArtNr. {aufnartnr} registrieren?"):
                        # Prepare the new tool data to insert into the database
                        neutool = [
                            QR,        # idCode
                            None,        # toolNummer (INTEGER)
                            None,        # toolName
                            None,        # toolTyp (INTEGER)
                            None,        # toolRadius (REAL)
                            None,        # toolEckenRadius (REAL)
                            None,        # toolSpitzenWinkel (REAL)
                            None,        # toolEintauchWinkel (REAL)
                            None,        # toolSchneiden (INTEGER)
                            None,        # toolSchnittLaenge (REAL)
                            None,        # toolAusspannlaenge (REAL)
                            None,        # tooIstLaenge (REAL)
                            None,        # toolSollLaenge (REAL)
                            None,        # toolSteigung (REAL)
                            None,        # komponent1
                            aufnartnr, # komponent2 (contains Aufnahme_ArtNr)
                            None,        # komponent3
                            None,        # komponent4
                            None,        # komponent5
                            None         # platz
                        ]

                        # Insert the new entry into the currentTools table
                        try:
                            cur.execute("""
                                INSERT INTO currentTools (
                                    idCode, toolNummer, toolName, toolTyp, toolRadius, toolEckenRadius,
                                    toolSpitzenWinkel, toolEintauchWinkel, toolSchneiden, toolSchnittLaenge,
                                    toolAusspannlaenge, toolIstLaenge, toolSollLaenge, toolSteigung,
                                    komponent1, komponent2, komponent3, komponent4, komponent5, platz
                                )
                                VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                            """, neutool)
                            conn.commit()  # Commit the transaction first
                            cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                            messagebox.showinfo("Registriert", f"Die Aufnahme {QR} ist erfolgreich registriert.")
                        except Exception as e:
                            messagebox.showerror("Fehler", f"Fehler beim Einfügen in die Datenbank: {e}")

                # Prompt the user if they want to continue registering more entries
                if not messagebox.askyesno("weitere Halter", "Wollen Sie weitere Halter registrieren?"):
                    self.center.destroy()
                    self.goHome()
                    return

            # Reset the UI after registration
            self.listbox2.delete(0, tk.END)
            self.entries = []
            self.combo_halter.state(["!disabled"])
            self.show_image("IMAGE_RESET", width=200, height=150)
            self.combo_halter.set('')
            
            # Close the database connection
            conn.close()






        def lagestand(self):


            self.clearFrame()
            self.center= tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
            self.center.place(relx=0.5, rely=0.5, anchor=tk.CENTER)


            artno = []

            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()
            
            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()

            for row in currentToolsData:
                if len(row) > 16:  # Ensure there are at least 13 columns in the row
                    artno.append(row[15])


            self.artno = artno  # Convert set to list for use in Combobox



            self.column_values = []
            self.column_values_free = []
        
            for row in currentToolsData:
                if len(row) > 16 and row [15]!="":
                    self.column_values.append(row[15])
                if len(row) > 16 and row[2] ==None and row [15]!="" :
                    self.column_values_free.append(row[15])
                

            self.value_counts = Counter(self.column_values)
            self.value_counts_free = Counter(self.column_values_free)
            style = ttk.Style()
            style.configure("Treeview.Heading", font=(winconfig["fonttype"], winconfig["fontsize"], 'bold'), background="lightblue")

            # Create Treeview with columns for Item, Count, and Count2
            self.tree = ttk.Treeview(self.center, columns=("Aufnahme", "Anzahl", "Frei"), show='headings', height=20)
            self.tree.heading("Aufnahme", text="Aufnahme")
            self.tree.heading("Anzahl", text="Anzahl")
            self.tree.heading("Frei", text="Frei")

            self.tree.column("Aufnahme", anchor=tk.CENTER)
            self.tree.column("Anzahl", anchor=tk.CENTER)
            self.tree.column("Frei", anchor=tk.CENTER)

            self.tree.grid(row=1, column=0, pady=50, padx=50)

            all_items = set(self.value_counts.keys()).union(self.value_counts_free.keys())

            for item in all_items:
                count = self.value_counts.get(item, 0)
                count2 = self.value_counts_free.get(item, 0)
                self.tree.insert("", tk.END, values=(item, count, count2))

            # Create an entry widget for searching


            # self.search_entry = tk.Entry(self.center)
            # self.search_entry.grid(row=0, column=0, padx=(150, 50), pady=10, sticky="w")

            entry_var = tk.StringVar(value=self.artno)
            self.search_entry = ttk.Combobox(self.center, textvariable=entry_var, font=(winconfig["fonttype"], winconfig["fontsize"]))
            self.search_entry['values'] = self.artno
            self.search_entry.grid(row=0, column=0, sticky='w')

            self.search_entry.bind('<KeyRelease>', self.filter_combobox)

            search_button = tk.Button(self.center, text="Search", command=self.search_item,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
            search_button.grid(row=0, column=0, padx=(250, 50), pady=10, sticky="w")

            search_lage = tk.Button(self.center, text="Wo?", command=self.aufnahme_wo,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
            search_lage.grid(row=2, column=1, padx=(20, 50), pady=10, sticky="w")

            mail_button = tk.Button(self.center, text="Nachfragen", command=self.bestellung,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
            mail_button.grid(row=2, column=1, padx=(100, 20), pady=10, sticky="w")

            self.listbox = tk.Listbox(self.center, bg='white',font=(winconfig["fonttype"], winconfig["fontsize"]))
            self.listbox.place_forget()


            self.image_label_lage = tk.Label(self.center)  # Label to display the image
            self.image_label_lage.grid(row=1,column=1)

            conn.close()

            def show_value(event):
                if self.tree.selection():  # Ensure there is a selection
                    selected_item = self.tree.selection()[0]
                    values =self.tree.item(selected_item, 'values')
                    value_in_second_column = values[0]
                    self.search_entry.set(value_in_second_column)
                    self.search_item()



            # Bind the select event to the show_value function
            self.tree.bind('<<TreeviewSelect>>', show_value)
        

        def filter_combobox(self, event):
            search_text = self.search_entry.get().lower()
            filtered_values = [value for value in self.artno if search_text in value.lower()]
            self.search_entry['values'] = filtered_values


        def search_item(self):
            search_term = self.search_entry.get().strip().lower()
            
            # Remove existing highlights
            for item in self.tree.get_children():
                self.tree.item(item, tags="")

            if not search_term:
                return

            # Highlight matching items and scroll to the first match
            first_match = None
            for item in self.tree.get_children():
                values = self.tree.item(item, "values")
                if search_term in str(values[0]).lower():
                    self.tree.item(item, tags=("match",))
                    if first_match is None:
                        first_match = item

            # Configure the matching tag to change the background color
            self.tree.tag_configure("match", background=winconfig["fontcolor"])

            # Scroll to the first matching item or show info message box if not found
            if first_match is not None:
                self.tree.see(first_match)
            else:
                messagebox.showinfo("Info", "Aufnahme nicht gefunden")

            self.lagestandimage()


        def lagestandimage(self):
            aufnahme=(self.search_entry.get()[:6])
            
            im=paths["compimgs"]+aufnahme+'.jpg'          
            self.show_image_lage(im, width=425, height=300)

            # with open(self.werkzeugecsv, "r") as file:
            #     csv_reader = csv.reader(file, delimiter=";")
            #     for row in csv_reader:
            #         if row[5] ==self.search_entry.get():
            #             self.aufnahme=(row[3])
            #             im=self.perschmanndbimage+self.aufnahme+'.jpg'          
            #             self.show_image_lage(im, width=400, height=200)
            # file.close()



        def show_image_lage(self, image_path, width, height):
            print(image_path)
            try:
                # Attempt to load the specified image
                image = Image.open(image_path)
                image = image.resize(( width, height), Image.LANCZOS)  # Resize the image to fit the label
                image = image.rotate(270, expand=True)
            except (FileNotFoundError):
            # Load the image
                image = Image.open(paths["compimgs"]+"NOTFOUND.png")
                image = image.resize(( 200, height), Image.LANCZOS)  # Resize the image to fit the label
                image = image.rotate(0, expand=True)
            photo = ImageTk.PhotoImage(image)
            
            # Update the image label
            self.image_label_lage.config(image=photo)
            self.image_label_lage.image = photo

        def bestellung(self):
            self.bestellung_win = tk.Toplevel()
            self.bestellung_win.title("Bestellung")



            label1 = tk.Label(self.bestellung_win, text="Menga:")
            label1.grid(row=0, column=0, padx=10, pady=5)

            self.entry_menga = tk.Entry(self.bestellung_win)
            self.entry_menga.grid(row=0, column=1, padx=10, pady=5)

            # Create and place the second label and entry widget
            label2 = tk.Label(self.bestellung_win, text="Bemerkungen:")
            label2.grid(row=1, column=0, padx=10, pady=5)

            self.entry_bemerkung = tk.Text(self.bestellung_win, height=5, width=30)
            self.entry_bemerkung.grid(row=1, column=1, padx=10, pady=5)

            search_button = tk.Button(self.bestellung_win, text="bestellen", command=self.send_email)
            search_button.grid(row=2, column=0, padx=(50, 50), pady=10, sticky="w")



        def send_email(self):
            artnummer=self.search_entry.get()
            menga=self.entry_menga.get()
            bemerkung=self.entry_bemerkung.get("1.0", tk.END).strip()
            self.bestellung_win.destroy()

            sender_email = "xpercent0@gmail.com"
            sender_password = "malh icho ljvy oqtq"  # Update this to your actual password or use environment variables for better security
            recipient_email = "abdelraof.mo@gmail.com"
            subject = "MTM Nachfrage"
            body = (f"Guten Tag,\nder User Admin braucht {menga} Aufnahme\\n {artnummer}\nBemerkung: {bemerkung} \nMit freundlichen Grüßen\nMTM") 

            # Create the email message
            email_message = EmailMessage()
            email_message["From"] = sender_email
            email_message["To"] = recipient_email
            email_message["Subject"] = subject
            email_message.set_content(body)

            # Create a secure SSL context
            context = ssl.create_default_context()

            try:
                with smtplib.SMTP_SSL('smtp.gmail.com', 465, context=context) as smtp:
                    smtp.login(sender_email, sender_password)
                    smtp.sendmail(sender_email, recipient_email, email_message.as_string())
                    print("Email sent successfully")
            except Exception as e:
                print(f"Failed to send email. Error: {e}")


        def aufnahme_wo(self):
            self.wo = tk.Toplevel()
            self.wo.title("Die Aufnahme ist an: ")
            
            self.data_dict = {}
            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()
            
            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()


            for row in currentToolsData:
                if row[15] == self.search_entry.get():  # Ensure there are at least 13 columns in the row
                    key = row[15]  # Assuming row[8] is the key for your dictionary
                    if row[19]==None:
                        values_tuple = (row[0],row[2],"LAGE")
                    else:
                        values_tuple = (row[0],row[2],row[19])

                        # Store the three values as a tuple
                    if key in self.data_dict:
                        self.data_dict[key].append(values_tuple)
                    else:
                        self.data_dict[key] = [values_tuple]


    # Assuming self.data_dict is your dictionary containing the data
            self.treeview = ttk.Treeview(self.wo, columns=('QR', 'Werkzeugsname','Lage'), show='headings')
            self.treeview.pack(padx=10, pady=10)

            # Define headings for columns
            self.treeview.heading('QR', text='QR')
            self.treeview.heading('Werkzeugsname', text='Werkzeugsname')
            self.treeview.heading('Lage', text='Lage')

            self.treeview.column("QR", anchor=tk.CENTER)
            self.treeview.column("Werkzeugsname", anchor=tk.CENTER)
            self.treeview.column("Lage", anchor=tk.CENTER)
            

            self.treeview.tag_configure('light_green', background='#90EE90')  # Light green color

            # Insert data into the Treeview
            for key, items in self.data_dict.items():
                for qr_item, name_item, lage_item in items:
                    if lage_item == "LAGE":
                        self.treeview.insert('', tk.END, values=(qr_item, name_item, lage_item), tags=('light_green',))
                    else:
                        self.treeview.insert('', tk.END, values=(qr_item, name_item, lage_item))
