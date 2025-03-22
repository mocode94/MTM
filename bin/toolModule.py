import xml.etree.ElementTree as ET
import os
import csv
import tkinter as tk
# import cv2
import tkinter.messagebox as messagebox
import requests

from tkinter import Toplevel, Label, Entry, Button, StringVar,ttk,font,PhotoImage,Listbox,SINGLE
from PIL import Image, ImageTk

# import requests
# import io
import os
from config import winconfig,paths,places,settings
from TNC640_Daten import TNC640Laser as TNC640Laser
import platform
import subprocess
import threading
import time
import sqlite3
import logging
from io import BytesIO
from bs4 import BeautifulSoup
from playwright.sync_api import sync_playwright

#                                        Werkzeug einfügen
#==========================================================================================================
#==========================================================================================================
#==========================================================================================================
class toolModule:
    def __init__(self, mainFrame,userCode):
        self.mainFrame = mainFrame
        # mainFrame.config(width=2000, height=1250)
        mainFrame.config(width=2000, height=800)
        # Connect to the database
        conn = sqlite3.connect(paths["MTMDB"])
        cursor = conn.cursor()
        self.userCode=userCode
        # Execute the query
        cursor.execute("SELECT * FROM Users WHERE ID = ?", (userCode,))

        # Fetch the result
        row = cursor.fetchone()
        self.userName = row[1]
        # Close the connection
        conn.close()


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

        # Clear the frame before adding new widgets
        self.clearFrame()

        # Create a new frame inside the mainFrame with a specific size
        self.centerFrame = tk.Frame(mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)


        self.rightFrame = tk.Frame(mainFrame, bg=winconfig["bgcolor"])
        self.rightFrame.place(relx=1.0, rely=0.5, anchor=tk.E)



        # self.centerFrame.grid_propagate(False)
        print(paths["imgpaths"]+"button7.png")
        
        # Load images
        imgAddToolButton = Image.open(paths["imgpaths"]+"button7.png")
        self.imgAddtoolButton = ImageTk.PhotoImage(imgAddToolButton)

        imgEditToolButton = Image.open(paths["imgpaths"]+"button8.png")
        self.imgEditToolButton = ImageTk.PhotoImage(imgEditToolButton)

        imgMoveToolButton = Image.open(paths["imgpaths"]+"button9.png")
        self.imgMoveToolButton = ImageTk.PhotoImage(imgMoveToolButton)

        addToolButton = tk.Button(self.centerFrame, image=self.imgAddtoolButton, command=self.addToolFunction, width=250, height=250)
        addToolButton.pack(side="left", padx=10, pady=10)

        editToolButton = tk.Button(self.centerFrame, image=self.imgEditToolButton, command=self.editToolFunction, width=250, height=250)
        editToolButton.pack(side="left", padx=10, pady=10)

        moveToolButton = tk.Button(self.centerFrame, image=self.imgMoveToolButton, command=self.moveToolFunction, width=250, height=250)
        moveToolButton.pack(side="left", padx=10, pady=10)

    def clearFrame(self):
        # Clear all widgets from the mainFrame
        for widget in self.mainFrame.winfo_children():
            widget.destroy()

    def clearFrameRight(self):
        if self.rightFrame:
            self.rightFrame.destroy()

    def goHome(self):
        if self.centerFrame:
            self.centerFrame.destroy()

        if self.rightFrame:
            self.rightFrame.destroy()

        self.__init__(self.mainFrame,self.userCode)

    def load_items_from_db(self,db_path):
        # Connect to the SQLite database
        conn = sqlite3.connect(db_path)
        cursor = conn.cursor()

        # Fetch all artikelNumber values from the Items table
        cursor.execute('SELECT artikelNumber FROM Items')
        items = [row[0] for row in cursor.fetchall()]

        # Close the connection
        conn.close()
        return items

        
    def addToolFunction(self):
        self.clearFrame()
        self.centerFrame= tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        



        # Left Frame (Navigation / Tool List)
        self.leftFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.leftFrame.place(x=0, rely=0.5, anchor="w")  # Position on the left center
        self.leftFrame.pack_propagate(False)  # Prevent resizing
 
        buttonleftside = tk.Button(self.leftFrame, text="<< Zurück",command=lambda:self.__init__(self.mainFrame,self.userCode),font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame

        # QR_Code entry
        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")
        labelCode = Label(self.centerFrame, text='QR_CODE',font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        labelCode.grid(row=0, column=0, padx=10, pady=5, sticky='e')
        self.iDCode = StringVar()
        self.iDCodeEntry = Entry(self.centerFrame, textvariable=self.iDCode,font=(winconfig["bgcolor"], winconfig["fontsize"]))
        self.iDCodeEntry.grid(row=0, column=1, padx=10, pady=5, sticky='w')
        self.iDCodeEntry.focus_set()

        # Submit button to check QR_Code
        self.checkCodeButton = Button(self.centerFrame, text="Prüfen", command=self.checkCodeAddTool,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        self.checkCodeButton.grid(row=0, column=2, padx=10, pady=5, sticky='w')
        self.iDCodeEntry.bind('<Return>', lambda event: self.checkCodeButton.invoke())
        

    def checkCodeAddTool(self):

        self.iDCode = self.iDCodeEntry.get()
        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        
        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        # Close the database connection
        conn.close()
        # Check if QR code exists in the table
        for i in range(len(currentToolsData)):
            

            if currentToolsData[i][0] == self.iDCode:
                if (currentToolsData[i][1] == None and currentToolsData[i][2] == None and currentToolsData[i][3] == None and
                    currentToolsData[i][4] == None and currentToolsData[i][5] == None and currentToolsData[i][6] == None and
                    currentToolsData[i][7] == None and currentToolsData[i][8] == None and currentToolsData[i][9] == None and
                    currentToolsData[i][10] == None and currentToolsData[i][11] == None and currentToolsData[i][12] == None and
                    currentToolsData[i][13] == None and currentToolsData[i][14] == None  and
                    currentToolsData[i][16] == None and currentToolsData[i][17] == None and currentToolsData[i][18] == None):
                    
                    self.selectToolType(list(currentToolsData[i][0:12]+currentToolsData[i][13:]))
                    
                    return

                elif (currentToolsData[i][1] != None or currentToolsData[i][2] != None or currentToolsData[i][3] != None or
                    currentToolsData[i][4] != None or currentToolsData[i][5] != None or currentToolsData[i][6] != None or
                    currentToolsData[i][7] != None or currentToolsData[i][8] != None or currentToolsData[i][9] != None or
                    currentToolsData[i][10] != None or currentToolsData[i][11] != None or currentToolsData[i][12] != None or
                    currentToolsData[i][13] != None or currentToolsData[i][14] != None  or
                    currentToolsData[i][16] != None or currentToolsData[i][17] != None or currentToolsData[i][18] != None   ):
                    logging.error("Diese Aufnahme ist schon belegt !!!")
                    messagebox.showerror("Warning", "Diese Aufnahme ist schon belegt !!!")
                    self.iDCodeEntry.delete(0, tk.END)
                    return
        logging.error("Dieses Aufnahme ist nicht registriert !!!")
        # This part of the code will be reached only if none of the rows match self.qr_code
        messagebox.showerror("Warning", "Dieses Aufnahme ist nicht registriert !!!")
        self.iDCodeEntry.delete(0, tk.END)

                
    def editToolFunction(self):
        self.clearFrame()
        self.centerFrame= tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        
        # QR_Code entry
        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")
        labelCode = Label(self.centerFrame, text='QR_CODE',font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        labelCode.grid(row=0, column=0, padx=10, pady=5, sticky='e')
        self.iDCode = StringVar()
        self.iDCodeEntry = Entry(self.centerFrame, textvariable=self.iDCode,font=(winconfig["fonttype"], winconfig["fontsize"]))
        self.iDCodeEntry.grid(row=0, column=1, padx=10, pady=5, sticky='w')
        self.iDCodeEntry.focus_set()



        
        # Submit button to check QR_Code
        self.checkCodeButton = Button(self.centerFrame, text="Prüfen", command=self.checkCodeEditTool,font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        self.checkCodeButton.grid(row=0, column=2, padx=10, pady=5, sticky='w')
        self.iDCodeEntry.bind('<Return>', lambda event: self.checkCodeButton.invoke())
        

        # self.addwz.grid(padx=10, pady=10)

    def checkCodeEditTool(self):
        self.iDCode = self.iDCodeEntry.get()
        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        
        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()

        # Close the database connection
        conn.close()

        # Check if QR code exists in the table
        for i in range(len(currentToolsData)):
            

            if currentToolsData[i][0] == self.iDCode:

                if (currentToolsData[i][1] != None or currentToolsData[i][2] != None or currentToolsData[i][3] != None or
                    currentToolsData[i][4] != None or currentToolsData[i][5] != None or currentToolsData[i][6] != None or
                    currentToolsData[i][7] != None or currentToolsData[i][8] != None or currentToolsData[i][9] != None or
                    currentToolsData[i][10] != None or currentToolsData[i][11] != None or currentToolsData[i][12] != None or
                    currentToolsData[i][13] != None or currentToolsData[i][14] != None  or
                    currentToolsData[i][16] != None or currentToolsData[i][17] != None or currentToolsData[i][18] != None and
                    currentToolsData[i][19] != None ):
                    self.editToolInputWindow(currentToolsData[i][0:12]+currentToolsData[i][13:])
                    return
                
                elif (currentToolsData[i][1] == None and currentToolsData[i][2] == None and currentToolsData[i][3] == None and
                    currentToolsData[i][4] == None and currentToolsData[i][5] == None and currentToolsData[i][6] == None and
                    currentToolsData[i][7] == None and currentToolsData[i][8] == None and currentToolsData[i][9] == None and
                    currentToolsData[i][10] == None and currentToolsData[i][11] == None and currentToolsData[i][12] == None and
                    currentToolsData[i][13] == None and currentToolsData[i][14] == None  and
                    currentToolsData[i][16] == None and currentToolsData[i][17] == None and currentToolsData[i][18] == None ):
                    logging.error("Diese Aufnahme ist nicht belegt !!!")
                    messagebox.showerror("Warning", "Diese Aufnahme ist nicht belegt !!!")
                    self.iDCodeEntry.delete(0, tk.END)
                    return    
                                
        logging.error( "Dieses Aufnahme ist nicht registriert !!!")
        # This part of the code will be reached only if none of the rows match self.qr_code
        messagebox.showerror("Warning", "Dieses Aufnahme ist nicht registriert !!!")
        self.iDCodeEntry.delete(0, tk.END)
   

    def selectToolType(self,data):
        self.clearFrame()
        self.centerFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        # Ensure centerFrame has a size
        self.centerFrame.config(width=1500, height=843)  # Adjust as needed
        self.centerFrame.pack_propagate(False)  # Prevent auto-resizing



        self.clearFrame()
        self.centerFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        self.centerFrame.config(width=1500, height=843)  # Adjust as needed
        self.centerFrame.pack_propagate(False)  # Prevent auto-resizing
        print(f"here is data  {data}")

        # Left Frame (Navigation / Tool List)
        self.leftFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.leftFrame.place(x=0, rely=0.5, anchor="w")  # Position on the left center
        self.leftFrame.pack_propagate(False)  # Prevent resizing



        self.ToolData=data
        buttonleftside = tk.Button(self.leftFrame, text="<< Zurück",command=self.addToolFunction,font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame




        # Load images
        self.img_bohrer = PhotoImage(file=paths["imgpaths"]+"toolicons/Bohrer.png")
        self.img_mk = PhotoImage(file=paths["imgpaths"]+"toolicons/Messerkopf.png")
        self.img_schaft = PhotoImage(file=paths["imgpaths"]+"toolicons/Schlichten.png")
        self.img_gewinde = PhotoImage(file=paths["imgpaths"]+"toolicons/Gewinde.png")
        self.img_kugel = PhotoImage(file=paths["imgpaths"]+"toolicons/Radius.png")
        self.img_nuten = PhotoImage(file=paths["imgpaths"]+"toolicons/Nuten.png")
        self.schuppentool = PhotoImage(file=paths["imgpaths"]+"toolicons/Schruppen.png")
        self.reibentool = PhotoImage(file=paths["imgpaths"]+"toolicons/Reiben.png")
        self.torustool = PhotoImage(file=paths["imgpaths"]+"toolicons/Torus.png")



        # Create buttons with images
        buttonBohrer = Button(self.centerFrame, image=self.img_bohrer, compound="top",
                              command=lambda: self.addToolInputWindow("1"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonBohrer.grid(row=0, column=0, padx=20, pady=20)

        buttonMK = Button(self.centerFrame, image=self.img_mk, compound="top",
                          command=lambda: self.addToolInputWindow("0"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonMK.grid(row=0, column=1, padx=20, pady=20)

        buttonSchaft = Button(self.centerFrame, image=self.img_schaft, compound="top",
                              command=lambda: self.addToolInputWindow("10"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonSchaft.grid(row=0, column=2, padx=20, pady=20)

        buttonGewinde = Button(self.centerFrame, image=self.img_gewinde, compound="top",
                               command=lambda: self.addToolInputWindow("2"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonGewinde.grid(row=1, column=0, padx=20, pady=20)

        buttonKugel = Button(self.centerFrame, image=self.img_kugel, compound="top",
                             command=lambda: self.addToolInputWindow("22"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonKugel.grid(row=1, column=1, padx=20, pady=20)

        buttonNuten = Button(self.centerFrame, image=self.img_nuten, compound="top",
                             command=lambda: self.addToolInputWindow("7"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonNuten.grid(row=1, column=2, padx=20, pady=20)

        buttonSchruppen = Button(self.centerFrame, image=self.schuppentool, compound="top",
                             command=lambda: self.addToolInputWindow("9"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonSchruppen.grid(row=2, column=0, padx=20, pady=20)

        buttonReiben = Button(self.centerFrame, image=self.reibentool, compound="top",
                             command=lambda: self.addToolInputWindow("3"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonReiben.grid(row=2, column=1, padx=20, pady=20)

        buttonTorus = Button(self.centerFrame, image=self.torustool, compound="top",
                             command=lambda: self.addToolInputWindow("23"),width=150,height=150,bg=winconfig["fontcolor"])
        buttonTorus.grid(row=2, column=2, padx=20, pady=20)



    def addToolInputWindow(self,type):
        self.clearFrame()
        self.centerFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        # Ensure centerFrame has a size
        self.centerFrame.config(width=1500, height=843)  # Adjust as needed
        self.centerFrame.pack_propagate(False)  # Prevent auto-resizing
        
        # Left Frame (Navigation / Tool List)
        self.leftFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.leftFrame.place(x=0, rely=0.5, anchor="w")  # Position on the left center
        self.leftFrame.pack_propagate(False)  # Prevent resizing


        # Right Frame (Navigation / Tool List)
        self.rightFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.rightFrame.place(relx=1.0, rely=0.5, anchor="e")  # Corrected Positioning
        self.rightFrame.pack_propagate(False)  # Prevent resizing


        data=[]
        buttonleftside = tk.Button(self.rightFrame, text="Weiter >>",command=self.toolComponants,font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame

        data=[]
        buttonleftside = tk.Button(self.leftFrame, text="<< Zurück",command=lambda: self.selectToolType(data),font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame

        itemsDB=paths["MTMitems"]
        masterCsvFile=paths["mastercsv"]
        self.itemNameExamples = set()
        self.dataToolEntries = []
        self.toolNameExamples=set()
        # self.movingTools=[data[0]]

        try:
            self.itemNameExamples.update(self.load_items_from_db(itemsDB))

        except FileNotFoundError:

            print(f"Error: File {itemsDB} not found.")

        try:
            with open(paths["mastercsv"], 'r') as file:
                dataMasterCsv = csv.reader(file, delimiter=";")
                for row in dataMasterCsv:
                    if len(row) >= 3:
                        self.toolNameExamples.update(row[1].split(';'))
                    else:
                        print(f"Warning: Row {dataMasterCsv.line_num} has less than 9 columns.")
        except FileNotFoundError:
            print(f"Error: File '{masterCsvFile}' not found.")

        self.toolNameExamples = list(self.toolNameExamples)

        toolPlaces = set()
        clampingSystems=set()
        toolTypes=("Bohrer","Schruppfraeser","Schlichtfraeser","","FraesWZ","Reibahle","Gewindebohrer","Nutenfraeser","Torusfraeser","Kugelfraeser")

        for dictionary in places :

            if dictionary:
                toolPlaces.add(dictionary["placename"])

            if dictionary["status"]=="machine":
                clampingSystems.add(dictionary["holdertype"]) 

        toolPlaces = list(toolPlaces)  # Convert set to list for use in Combobox
        clampingSystems=list(clampingSystems)

        self.toolType=type
        self.ToolData[3]=self.toolType
        self.toolItems=self.toolComponants()


    def toolDataInput(self,type):
        self.clearFrame()
        self.centerFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        # Ensure centerFrame has a size
        self.centerFrame.config(width=1500, height=843)  # Adjust as needed
        self.centerFrame.pack_propagate(False)  # Prevent auto-resizing
        




        # Left Frame (Navigation / Tool List)
        self.leftFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.leftFrame.place(x=0, rely=0.5, anchor="w")  # Position on the left center
        self.leftFrame.pack_propagate(False)  # Prevent resizing


        # Right Frame (Navigation / Tool List)
        self.rightFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.rightFrame.place(relx=1.0, rely=0.5, anchor="e")  # Corrected Positioning
        self.rightFrame.pack_propagate(False)  # Prevent resizing

        data=[]
        buttonleftside = tk.Button(self.rightFrame, text="Weiter >>",command=self.PLCINPUT,font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame

        data=[]
        buttonleftside = tk.Button(self.leftFrame, text="<< Zurück",command=self.toolComponants,font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame


        if type=="1":
            self.Bohrerinput=self.bohrerToolInput()
        elif type=="10":
            self.Schlichterinput=self.schaftToolInput()
        elif type=="9":
            self.SchrupperInput=self.schaftToolInput()          
        elif type=="0":
            self.MKInput=self.mkToolInput()
        elif type=="3":
            self.ReibahleInput=self.ReibenToolInput()
        elif type=="2":
            self.GewindeInput=self.gewindeToolInput()
        elif type =="7":
            self.NutenInput=self.NutenToolInput()
        elif type=="23":
            self.TorusInput=self.TorusToolInput()
        elif type=="22":
            self.KugelInput=self.KugelToolInput()

        else:
            messagebox.showerror("Error",f"Type {type} undefiend")
        


    def PLCINPUT(self):
        print("PUT PLC DATA")


    def toolComponants(self):
        self.clearFrame()
        self.centerFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        self.centerFrame.config(width=1500, height=843)  # Adjust as needed
        self.centerFrame.pack_propagate(False)  # Prevent auto-resizing

        # Top Frame (Centered at the top)
        self.topFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)  # Adjust height if needed
        self.topFrame.place(relx=0.5, y=0, anchor="n")  # Centered horizontally at the top
        self.topFrame.pack_propagate(False)  # Prevent resizing

        # Left Frame (Navigation / Tool List)
        self.leftFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.leftFrame.place(x=0, rely=0.5, anchor="w")  # Position on the left center
        self.leftFrame.pack_propagate(False)  # Prevent resizing


        # Right Frame (Navigation / Tool List)
        self.rightFrame = tk.Frame(self.mainFrame, bg="#202020", width=300, height=843)
        self.rightFrame.place(relx=1.0, rely=0.5, anchor="e")  # Corrected Positioning
        self.rightFrame.pack_propagate(False)  # Prevent resizing

        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+4, weight="bold")
        labelFontConfiguration2 = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")

        # Create the label inside topFrame
        labelidCode = Entry(self.topFrame, width=12,font=labelFontConfiguration, justify='center')
        tool_id = self.ToolData[0] if self.ToolData else "No Tool ID"  # Default value if self.ToolData is empty
        labelidCode.insert(0, tool_id)  # Set default value
        labelidCode.config(state='readonly')  # Make it readonly
        labelidCode.grid(row=0, column=0, pady=20)  # Add some padding for better spacing
        print("Tool ID to be inserted:", tool_id)







        # Create the label inside topFrame for tool name
        toolNamelabel = Label(self.topFrame, text="Tool-Name:", bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], font=labelFontConfiguration)
        toolNamelabel.grid(row=0, column=1, padx=5, pady=80)

        # Replace Combobox with an Entry widget for tool name
        toolNameVar = StringVar()
        toolNameEntry = Entry(self.topFrame, textvariable=toolNameVar, font=labelFontConfiguration2, width=32, justify='center')
        toolNameEntry.grid(row=0, column=2, padx=5, pady=5, sticky='w')

        # Create the Listbox for filtered values (hidden by default)
        toolNameListBox = Listbox(self.topFrame, height=5, font=labelFontConfiguration2, bg='white', selectmode=SINGLE)
        toolNameListBox.place_forget()  # Initially hide the Listbox

        # Bind key events for filtering and interacting with the Listbox
        toolNameEntry.bind('<KeyRelease>', lambda event: filter_tool_name(event, toolNameEntry, self.toolNameExamples, toolNameListBox))
        toolNameListBox.bind('<ButtonRelease-1>', lambda event: select_from_listbox(event, toolNameEntry, toolNameListBox))

        def filter_tool_name(event, entry, values, listbox):
            """Filters values based on input and updates the Listbox."""
            typed_value = entry.get()
            if typed_value == "":
                filtered_values = values  # Reset list if empty
            else:
                filtered_values = [item for item in values if typed_value.lower() in item.lower()]

            # Update the Listbox with filtered values
            listbox.delete(0, tk.END)
            for item in filtered_values:
                listbox.insert(tk.END, item)

            # Adjust Listbox width and height dynamically
            listbox_width = entry.winfo_width()  # Match width of the entry
            listbox.config(width=listbox_width)  # Set width to entry width

            listbox_height = min(len(filtered_values), 5)  # Limit the height to 5 items
            listbox.config(height=listbox_height)  # Adjust height based on the number of filtered items

            if filtered_values:  # If there are matches, show the Listbox
                listbox.place(x=entry.winfo_x(), y=entry.winfo_y() + entry.winfo_height(), width=listbox_width)
                listbox.lift()  # Make sure the listbox is on top
            else:
                listbox.place_forget()  # Hide the Listbox if no matches

        def select_from_listbox(event, entry, listbox):
            """Select an option from the Listbox and set it in the Entry."""
            selected_index = listbox.curselection()
            if selected_index:
                selected_value = listbox.get(selected_index[0])
                entry.delete(0, tk.END)  # Clear the Entry field
                entry.insert(0, selected_value)  # Insert the selected value
                listbox.place_forget()  # Hide the Listbox after selection

        # Ensure focus remains on the entry field
        toolNameEntry.focus_set()

        # Create the label inside topFrame for tool number
        toolNumberlabel = Label(self.topFrame, text="Tool-Nr.:", bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], font=labelFontConfiguration)
        toolNumberlabel.grid(row=0, column=3)  # Add some padding for better spacing

        toolNumberentry = Entry(self.topFrame, font=labelFontConfiguration, width=10, justify='center')
        toolNumberentry.grid(row=0, column=4)

        # Function to close the listbox when clicking outside the toolNameEntry box
        def close_listbox(event):
            if toolNameListBox.winfo_ismapped():  # Check if the listbox is visible
                if toolNameEntry.winfo_containing(event.x_root, event.y_root) is None and toolNameListBox.winfo_containing(event.x_root, event.y_root) is None:
                    toolNameListBox.place_forget()  # Hide the Listbox when clicking outside

        # Bind click event to the root window to detect clicks outside the Entry and Listbox
        self.topFrame.bind('<Button-1>', close_listbox)  # Replace self.topFrame with the root window if needed



        # Function to close the listbox if the click is outside the entry and listbox
        def close_listbox(event):
            if toolNameListBox.winfo_ismapped():  # Check if the listbox is visible
                # Get the coordinates of the click and the positions of the entry and listbox
                entry_pos = toolNameEntry.winfo_rootx(), toolNameEntry.winfo_rooty()
                listbox_pos = toolNameListBox.winfo_rootx(), toolNameListBox.winfo_rooty()

                entry_width = toolNameEntry.winfo_width()
                listbox_width = toolNameListBox.winfo_width()
                listbox_height = toolNameListBox.winfo_height()

                # Check if the click is outside the entry or listbox
                if not (entry_pos[0] <= event.x <= entry_pos[0] + entry_width and entry_pos[1] <= event.y <= entry_pos[1] + toolNameEntry.winfo_height()) and \
                not (listbox_pos[0] <= event.x <= listbox_pos[0] + listbox_width and listbox_pos[1] <= event.y <= listbox_pos[1] + listbox_height):
                    toolNameListBox.place_forget()  # Hide the Listbox

        # Bind click event to the root window to detect clicks outside the Entry and Listbox
        self.mainFrame = self.topFrame.winfo_toplevel()
        self.mainFrame.bind('<Button-1>', close_listbox)  # Replace self.topFrame with the root window if needed


        # Create the label inside topFrame for tool number
        toolNumberlabel = Label(self.topFrame, text="Tool-Nr.:", bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], font=labelFontConfiguration)
        toolNumberlabel.grid(row=0, column=3)  # Add some padding for better spacing

        toolNumberentry = Entry(self.topFrame, font=labelFontConfiguration, width=10, justify='center')
        toolNumberentry.grid(row=0, column=4)

        data=[]
        buttonleftside = tk.Button(self.rightFrame, text="Weiter >>",command=lambda: self.toolDataInput(self.toolType),font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame

        data=[]
        buttonleftside = tk.Button(self.leftFrame, text="<< Zurück",command=lambda: self.selectToolType(data),font=(winconfig["fonttype"], winconfig["fontsize"]+4,"bold"),bg=winconfig["fontcolor"])
        buttonleftside.place(relx=0.5, rely=0.5, anchor="center")  # Center in leftFrame


        entries = []
        buttons = []
        entries2 = []

        # Default button image
        fallback_image_path = paths["imgpaths"]+"toolicons/plus.png"
        fallback_image = Image.open(fallback_image_path)
        fallback_image = fallback_image.resize((200, 283), Image.Resampling.LANCZOS)  # Resize if needed
        tk_fallback_image = ImageTk.PhotoImage(fallback_image)


        self.buttonHerstellerComp1 = Button(self.centerFrame,text="Hersteller1", command=lambda: self.herstellerListe(self.buttonHerstellerComp1),font=labelFontConfiguration2, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], width=10, justify='center')
        self.buttonHerstellerComp1.grid(row=1, column=0, padx=5, pady=5)

        self.buttonHerstellerComp2 = Button(self.centerFrame,text="Hersteller2", command=lambda: self.herstellerListe(self.buttonHerstellerComp2),font=labelFontConfiguration2, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], width=10, justify='center')
        self.buttonHerstellerComp2.grid(row=1, column=1, padx=5, pady=5)

        self.buttonHerstellerComp3 = Button(self.centerFrame,text="Hoffmann", command=lambda: self.herstellerListe(self.buttonHerstellerComp3),font=labelFontConfiguration2, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], width=10, justify='center',state="disabled")
        self.buttonHerstellerComp3.grid(row=1, column=2, padx=5, pady=5)

        self.buttonHerstellerComp4 = Button(self.centerFrame,text="Hersteller3", command=lambda: self.herstellerListe(self.buttonHerstellerComp4),font=labelFontConfiguration2, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], width=10, justify='center')
        self.buttonHerstellerComp4.grid(row=1, column=3, padx=5, pady=5)

        for i in range(4):
            entry = Entry(self.centerFrame, font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"], width=10, justify='center')

            if i == 2:
                entry.insert(0, "308196")  # Set default value
                entry.config(state='readonly')  # Make it readonly
            entry.grid(row=2, column=i, padx=5, pady=5)
            entries.append(entry)

            label = Label(self.centerFrame, text="Art.Nr:", bg=winconfig["bgcolor"], fg=winconfig["fontcolor"],font=labelFontConfiguration2)
            label.grid(row=2, column=i, sticky='w', padx=5, pady=5)

        buttonComp1 = Button(
            self.centerFrame,
            image=tk_fallback_image,
            width=250,
            height=250,
            bg=winconfig["fontcolor"]
        )
        buttonComp1.grid(row=3, column=0, padx=20, pady=20)
        buttonComp1.image = tk_fallback_image
        buttonComp1.config(command=lambda e=entries[0], b=buttonComp1 : self.selectedHersteller(e, b,self.buttonHerstellerComp1))  # Fix button reference
        buttons.append(buttonComp1)

        buttonComp2 = Button(
            self.centerFrame,
            image=tk_fallback_image,
            width=250,
            height=250,
            bg=winconfig["fontcolor"]
        )
        buttonComp2.grid(row=3, column=1, padx=20, pady=20)
        buttonComp2.image = tk_fallback_image
        buttonComp2.config(command=lambda e=entries[1], b=buttonComp2 : self.selectedHersteller(e, b,self.buttonHerstellerComp2))  # Fix button reference
        buttons.append(buttonComp2)

        buttonComp3 = Button(
            self.centerFrame,
            image=tk_fallback_image,
            width=250,
            height=250,
            bg=winconfig["fontcolor"]
        )
        buttonComp3.grid(row=3, column=2, padx=20, pady=20)
        buttonComp3.image = tk_fallback_image
        buttonComp3.config(command=lambda e=entries[2], b=buttonComp3 : self.selectedHersteller(e, b,self.buttonHerstellerComp3))  # Fix button reference
        buttons.append(buttonComp3)

        buttonComp4 = Button(
            self.centerFrame,
            image=tk_fallback_image,
            width=250,
            height=250,
            bg=winconfig["fontcolor"]
        )
        buttonComp4.grid(row=3, column=3, padx=20, pady=20)
        buttonComp4.image = tk_fallback_image
        buttonComp4.config(command=lambda e=entries[3], b=buttonComp4 : self.selectedHersteller(e, b,self.buttonHerstellerComp4))  # Fix button reference
        buttons.append(buttonComp4)
        # Automatically click the third button
        self.centerFrame.after(100, lambda: buttons[2].invoke())

    def selectedHersteller(self,entry, button,Herrsteller):
        Herrsteller=Herrsteller["text"]
        if Herrsteller=="Hoffmann":
            self.HoffmannItemImage(entry,button)
        elif Herrsteller=="Ingersoll":
            self.IngersollItemImage(entry,button)
        else:
            self.IngersollItemImage(entry,button)


    def IngersollItemImage(self, entry, button):
        """Handles image fetching and displaying for the given entry."""

        def show_loading_progress( button):
            """Show a loading progress bar below the button."""
            self.progress = ttk.Progressbar(button, orient="horizontal", length=200, mode="determinate")
            self.progress.place(relx=0.5, rely=0.9, anchor="center")  # Position at bottom center

            def update_progress(value=0):
                if value <= 70:
                    self.progress["value"] = value
                    button.after(40, update_progress, value + 1)  # Update every 80ms
                else:
                    print("70%")
                    # self.progress.destroy()  # Remove progress bar after completion

            update_progress()

        def get_image_with_title(url, target_title="Foto / Standardbild"):
            """Fetches the image URL from the webpage."""
            try:
                with sync_playwright() as p:
                    browser = p.chromium.launch(headless=True)
                    page = browser.new_page()
                    page.goto(url, wait_until="load")

                    try:
                        page.wait_for_selector("img[title]", timeout=10000)
                    except:
                        print("No image with title found within timeout.")
                        browser.close()
                        return None

                    page.evaluate("window.scrollTo(0, document.body.scrollHeight);")
                    images = page.locator("img").all()
                    for img in images:
                        title = img.get_attribute("title")
                        print("Found:", title)
                        if title and target_title.lower() in title.lower():
                            img_url = img.get_attribute("src")

                            if img_url and img_url.startswith("/"):
                                img_url = "https://toolsunited.com" + img_url

                            print("Image found:", img_url)
                            browser.close()
                            return img_url  

                    print("No matching image found.")
                    browser.close()
                    return None  

            except Exception as e:
                print("Error:", e)
                return None

        def display_image(button, img_url):
            """Displays the fetched image on the button."""
            try:
                response = requests.get(img_url)
                response.raise_for_status()
                img_data = BytesIO(response.content)

                img = Image.open(img_data)
                
                # Maintain aspect ratio while fitting inside 250x250
                img.thumbnail((250, 250), Image.Resampling.LANCZOS)

                img_tk = ImageTk.PhotoImage(img)

                button.config(image=img_tk)
                button.image = img_tk  # Keep reference to avoid garbage collection

                def update_progress(value=70):
                    if value <= 100:
                        self.progress["value"] = value
                        button.after(10, update_progress, value + 1)  # Update every 10ms
                    else:
                        print("100%")
                        self.progress.destroy()  # Remove progress bar after completion

                update_progress()



            except Exception as e:
                print("Error loading image:", e)
                img = Image.open(self.paths["imgpaths"] + "NOTFOUND.png")
                img = img.resize((250, 250))
                img_tk = ImageTk.PhotoImage(img)

                button.config(image=img_tk)
                button.image = img_tk

        def on_button_click():
            url = f"https://toolsunited.com/DE/TuMenu/ShowResult?search=[[%22Volltext%C2%A7{entry.get()}%22],%22Root%22,0,100,%22default%22,true,[],%22ToolsUnited%22]"

            # Start progress bar immediately BEFORE loading the image
            show_loading_progress(button)

            def fetch_image():
                img_url = get_image_with_title(url)

                def update_ui():
                    if img_url:
                        display_image(button, img_url)
                    else:
                        messagebox.showwarning("Not Found", "Leider Bild ist nicht verfügbar\nbitte kontrollieren Sie die Art.Nr. nochmal")
                        print("No matching image found.")

                        fallback_image_path = paths["imgpaths"] + "toolicons/Plus.png"
                        fallback_image = Image.open(fallback_image_path)
                        fallback_image = fallback_image.resize((200, 283), Image.Resampling.LANCZOS)
                        tk_fallback_image = ImageTk.PhotoImage(fallback_image)

                        button.config(text="Loading...", image=tk_fallback_image)
                        button.image = tk_fallback_image  # Keep reference
                # Update UI safely from main thread
                button.after(0, update_ui)

            # Run the fetching function in a separate thread
            threading.Thread(target=fetch_image, daemon=True).start()

        on_button_click()


    def HoffmannItemImage(self, entry, button):
        def fetch_image():
            url_value = entry.get()
            webpage_url = f"https://www.hoffmann-group.com/DE/de/hom/p/{url_value}"
            fallback_image_path = paths["imgpaths"] + "toolicons/Loading.png"
            
            try:
                fallback_image = Image.open(fallback_image_path)
                
                # Maintain aspect ratio while fitting inside 250x250
                fallback_image.thumbnail((250, 250), Image.Resampling.LANCZOS)
                
                tk_fallback_image = ImageTk.PhotoImage(fallback_image)

                # Update button text and image (Must use after to update UI from main thread)
                button.after(0, lambda: button.config(text="Loading...", image=tk_fallback_image))
                button.after(0, button.update)

                response = requests.get(webpage_url)
                soup = BeautifulSoup(response.text, 'html.parser')
                image_tags = soup.find_all('img')

                largest_image_url = None
                largest_width = 0
                largest_height = 0

                for img_tag in image_tags:
                    if 'src' in img_tag.attrs:
                        img_url = img_tag['src']
                        if not img_url.startswith("http"):
                            img_url = f"https://www.hoffmann-group.com{img_url}"

                        width = img_tag.get('width')
                        height = img_tag.get('height')

                        if width and height:
                            width = int(width)
                            height = int(height)

                            if width * height > largest_width * largest_height:
                                largest_image_url = img_url
                                largest_width = width
                                largest_height = height

                if largest_image_url:
                    response = requests.get(largest_image_url)
                    image_data = response.content
                    image = Image.open(BytesIO(image_data))
                else:
                    image = fallback_image  # Use fallback image if none found

                # Maintain aspect ratio while fitting inside 250x250
                image.thumbnail((250, 250), Image.Resampling.LANCZOS)

                tk_image = ImageTk.PhotoImage(image)

                # Update button image on the main thread
                button.after(0, lambda: button.config(image=tk_image))
                button.after(0, lambda: setattr(button, "image", tk_image))  # Prevent garbage collection


            except requests.exceptions.RequestException:
                tk_fallback_image = ImageTk.PhotoImage(fallback_image)
                button.after(0, lambda: button.config(image=tk_fallback_image))
                button.after(0, lambda: setattr(button, "image", tk_fallback_image))  # Prevent garbage collection

        
        def show_loading_progress( button):
            """Show a loading progress bar below the button."""
            progress = ttk.Progressbar(button, orient="horizontal", length=200, mode="determinate")
            progress.place(relx=0.5, rely=0.9, anchor="center")  # Position at bottom center

            def update_progress(value=0):
                if value <= 100:
                    progress["value"] = value
                    button.after(20, update_progress, value + 1)  # Update every 80ms
                else:
                    print("70%")
                    if progress:
                        progress.destroy()  # Remove progress bar after completion

            update_progress()

        show_loading_progress(button)
        # Run fetch_image in a separate thread
        thread = threading.Thread(target=fetch_image)
        thread.start()


    def herstellerListe(self, buttonHersteller):
        popup = tk.Toplevel()
        popup.overrideredirect(True)  # Remove title bar
        popup.update_idletasks()  # Ensure correct geometry calculations
        popup.config(bg='#202020')

        popup_width = 260
        popup_height = 260

        # Get screen width and height
        screen_width = popup.winfo_screenwidth()
        screen_height = popup.winfo_screenheight()

        # Center the popup in the middle of the screen
        x_position = (screen_width - popup_width) // 2
        y_position = (screen_height - popup_height) // 2

        popup.geometry(f"{popup_width}x{popup_height}+{x_position}+{y_position}")

        # Force focus on the popup window
        popup.focus_force()

        # Function to handle button clicks and close the popup
        def button_action(label):
            print(f"Button clicked: {label}")
            buttonHersteller.config(text=label)
            buttonHersteller.update()
            popup.destroy()  # Close popup when a button is clicked

        # Function to close the popup when clicking outside
        def close_popup(event):
            if not popup.winfo_containing(event.x_root, event.y_root):
                popup.destroy()

        # Bind click event to the root window to check if clicked outside the popup
        root = popup.winfo_toplevel()
        root.bind("<Button-1>", close_popup)

        herstellerimg = paths["imgpaths"] + "/toolicons/"

        # Load images and labels
        button_data = [
            ("Hoffmann", "Hoffmann.png"),
            ("Ingersoll", "Ingersoll.png"),
            ("Walter", "Walter.png"),
            ("Haimer", "Haimer.png"),
            ("Alesa", "Alesa.png"),
            ("Horn", "Horn.png"),
            ("Gühring", "Gühring.png")
        ]

        # Load images and create buttons
        images = []
        buttons = []

        for label, img_name in button_data:
            img_path = herstellerimg + img_name
            img = ImageTk.PhotoImage(Image.open(img_path).resize((70, 70)))
            images.append(img)  # Store reference to avoid garbage collection

            btn = tk.Button(popup, image=img, command=lambda lbl=label: button_action(lbl), width=70, height=70)
            buttons.append(btn)

        # Arrange buttons in a grid (3 in first row, 3 in second row, 1 in third row)
        rows = [3, 3, 1]
        index = 0

        for row, cols in enumerate(rows):
            for col in range(cols):
                if index < len(buttons):
                    buttons[index].grid(row=row, column=col, padx=5, pady=5)
                    index += 1

        popup.image_refs = images  # Prevent garbage collection


    def rest(self):
        print("Halo")
        # toolDataEntriesLabels = ['QR',
        #     'Wzg_Nummer', 'Wzg_Name', 'Typ','Wzg_Radius','Eckenradius','Spitzenwinkel','EinTauchwinkel','Schneiden','Schnittlänge','Wzg_Ausspannlänge','Wzg_gesamtlänge','Steigung',
        #     'Wzg_ArtNr', 'Aufn_ArtNr', 'Zusaufn_1', 'Zusaufn_2', 'Spannsys.', 'Wo?'
        # ]

        # self.dataToolEntries = []
        # for idx,  toolDataEntryLabel in enumerate(toolDataEntriesLabels):
        #     # Calculate the row and column positions based on the index
        #     row = idx % 8
        #     col = (idx // 8) * 2  # Multiplying by 2 to leave space for entries

        #     labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")
        #     addToolEntryLabel = tk.Label(self.centerFrame, text=toolDataEntryLabel, font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
        #     addToolEntryLabel.grid(row=row, column=col, padx=10, pady=5, sticky='e')

        #     if toolDataEntryLabel== 'QR':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         iDCodeEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11), state='disabled')
        #         iDCodeEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
        #     elif toolDataEntryLabel == 'Wo?':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         self.toolPlace=addToolDataEntry
        #         toolPlaceEntry = ttk.Combobox(self.centerFrame, textvariable=addToolDataEntry, font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly') #entry1
        #         toolPlaceEntry['values'] = toolPlaces
        #         toolPlaceEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
        #         self.toolPlace.trace_add('write', self.checkTNC640PlcAndTTDatasButton)
        #     elif toolDataEntryLabel == 'Spannsys.':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         toolClampingSystemEntry = ttk.Combobox(self.centerFrame, textvariable=addToolDataEntry, font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly') #entry2
        #         toolClampingSystemEntry['values'] = clampingSystems
        #         toolClampingSystemEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
        #     elif toolDataEntryLabel== 'Wzg_Name':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         self.toolNameListBox = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]-2))
        #         self.toolNameListBox.place_forget()
        #         self.toolNameComboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]-2))
        #         self.toolNameComboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
        #     elif toolDataEntryLabel== 'Typ':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         toolTypeComboBox = ttk.Combobox(self.centerFrame, textvariable=addToolDataEntry, font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly')
        #         toolTypeComboBox['value'] = toolTypes
        #         toolTypeComboBox.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
        #         self.toolTypeEntry=addToolDataEntry
        #         self.toolTypeEntry.trace_add('write', self.toolTypeCheck)
        #     elif toolDataEntryLabel == 'Wzg_ArtNr':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         self.toolEntry = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]))
        #         self.toolEntry.place_forget()
        #         self.toolComoboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]))
        #         self.toolComoboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
        #     elif toolDataEntryLabel == "Aufn_ArtNr":  # Checking if it's the 7th entry (0-based index)
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         toolHolderEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
        #         toolHolderEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
        #         toolHolderEntry.config(state='disabled')  # Disabling the entry
        #         self.toolHolderEntry=toolHolderEntry.get()
        #     elif toolDataEntryLabel== 'Steigung':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         toolPitchEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
        #         toolPitchEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
        #         self.toolPitch = toolPitchEntry
        #         self.toolPitchEntry=addToolDataEntry
        #         self.toolTypeCheck()
        #     elif toolDataEntryLabel == 'Wzg_gesamtlänge':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         toolLengthEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
        #         toolLengthEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
        #         self.toolLengthEntry = addToolDataEntry  # Store the value in an instance variable for later use
        #     elif toolDataEntryLabel== 'Zusaufn_1':
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         self.holderAddition1ListBox = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]))
        #         self.holderAddition1ListBox.place_forget()
        #         self.holderAddition1ComboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]))
        #         self.holderAddition1ComboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
        #     elif toolDataEntryLabel== 'Zusaufn_2':  # Storing the entry widget of 'Zusaufn_2' (9th entry) for later use
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         self.holderAddition2ListBox = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]))
        #         self.holderAddition2ListBox.place_forget()
        #         self.holderAddition2ComboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]))
        #         self.holderAddition2ComboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
        #         style = ttk.Style()
        #         style.configure("Custom.TCheckbutton", background=winconfig["bgcolor"],foreground=winconfig["fontcolor"],font=(winconfig["fonttype"], winconfig["fontsize"], "bold"))
        #         self.holderAddition2CheckBoxValue = tk.IntVar()
        #         self.holderAddition2CheckBox = ttk.Checkbutton(self.centerFrame, text="IKZ-Röhrchen", variable=self.holderAddition2CheckBoxValue, command=self.coolerTubeImgCheckBox, style="Custom.TCheckbutton")
        #         self.holderAddition2CheckBox.grid(row=row,column=col +2)
        #     else:
        #         addToolDataEntry = tk.StringVar(value=data[idx])
        #         toolDataEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
        #         toolDataEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')

        #     self.dataToolEntries.append(addToolDataEntry)












        # self.TNC640PlcAndTTDatenButton = tk.Button(self.centerFrame, text="TNC640+Laser", command=lambda: TNC640Laser(data[0],self.toolHolderEntry,self.toolLengthEntry.get()),font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        # self.TNC640PlcAndTTDatenButton.grid(row=4, column=5, padx=10, pady=5)
        # self.TNC640PlcAndTTDatenButton.grid_remove()  # Hide it initially


        # # Create a global reference to the subplace Combobox
        # self.subPlace_comboBox = None

        # # Function to handle updates
        # def showSubPlaceComboBox(event=None):
        #     # Get the current value of the combobox
        #     maschineComboBoxValue = self.toolPlace.get()

        #     # Destroy the previous subplace Combobox if it exists
        #     if self.subPlace_comboBox:
        #         self.subPlace_comboBox.destroy()
        #         self.subPlace_comboBox = None

        #     # Search for the selected place in the data
        #     for dictionary in places:
        #         if dictionary["placename"] == maschineComboBoxValue:
        #             if dictionary["status"] == "place":
        #                 # Extract subplace names
        #                 subplace_names = [subplace["subplacename"] for subplace in dictionary.get("subplace", [])]
        #                 if subplace_names:
        #                     # Create a new subplace Combobox
        #                     self.subPlace_Label= tk.Label(self.centerFrame, text="Unterplatz", font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
        #                     self.subPlace_Label.grid(row=3, column=4, padx=10, pady=5, sticky='e')
        #                     self.subPlace_comboBox = ttk.Combobox(self.centerFrame, values=subplace_names,font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly',default=None)
        #                     self.subPlace_comboBox.grid(row=3, column=5, padx=5, pady=5, sticky="ew")
        #                 else:
        #                     print("No subplaces available.")
        #             elif dictionary["status"] == "maschine":
        #                 print("Selected item is a machine. Subplace Combobox removed.")
        #             break
        #     else:
        #         print("No matching place found.")

        # # Bind user selection and trace variable changes
        # toolPlaceEntry.bind("<<ComboboxSelected>>", showSubPlaceComboBox)
        # self.toolPlace.trace_add("write", lambda *args: showSubPlaceComboBox())


        # self.clearFrameRight()
        # self.buttonsFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        # self.buttonsFrame.place(relx=0.95, rely=0.42, anchor=tk.E)
        # self.rightFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        # self.rightFrame.place(relx=0.5, rely=0.8, anchor=tk.CENTER)


        # try:
        #     # Attempt to open the specified image
        #     toolHolderEntryImage = Image.open(paths["compimgs"] +self.toolHolderEntry[:6]+".jpg")

        # except FileNotFoundError:
        #     # If the image is not found, open the default image
        #     toolHolderEntryImage = Image.open(paths["compimgs"]+"NOTFOUND.png")

        # # Resize the image to fit the label
        # toolHolderEntryImage = toolHolderEntryImage.resize((150, 150), Image.LANCZOS)
        # # Rotate the image
        # toolHolderEntryImage = toolHolderEntryImage.rotate(0, expand=True)
        # # Convert the image to a PhotoImage object for Tkinter
        # toolHolderEntryPhoto = ImageTk.PhotoImage(toolHolderEntryImage)

        # toolHolderEntryImageLabel = tk.Label(self.rightFrame, image=toolHolderEntryPhoto)
        # toolHolderEntryImageLabel.image = toolHolderEntryPhoto  # Keep a reference to avoid garbage collection
        # toolHolderEntryImageLabel.grid(row=0,column=2,padx=50)

        # measurementsButton = tk.Button(self.buttonsFrame,width=20, text="Messwerte Übernehmen", command=lambda: self.messwerte_uebernehmen, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        # measurementsButton.grid(row=0, column=7, padx=10, pady=10, sticky='w')

        # takePictureButton = tk.Button(self.buttonsFrame, width=20,text="Bild machen", command=self.openCamera, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        # takePictureButton.grid(row=1, column=7, padx=10, pady=10, sticky='w')

        # toolDatasSubmitButton = tk.Button(self.buttonsFrame, width=20,text="EinFügen", command=self.toolDatasSubmitButton, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        # toolDatasSubmitButton.grid(row=2,column=7, padx=10, pady=10, sticky='w')

        # cancelButton = tk.Button(self.buttonsFrame, width=20,text="Abbrechen", command=self.goHome, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        # cancelButton.grid(row=3, column=7, padx=10, pady=10, sticky='w')

        # self.toolImageLabel = tk.Label(self.rightFrame)  # Label to display the image
        # self.toolImageLabel.grid(row=0,column=0,padx=50)

        # self.holderAddition1ImageLabel = tk.Label(self.rightFrame)  # Label to display the image
        # self.holderAddition1ImageLabel.grid(row=0,column=1,padx=50)
        
        # self.holderAddition2ImageLabel = tk.Label(self.rightFrame)
        # self.holderAddition2ImageLabel.grid(row=0,column=3)

        # toolRemarkLabel = tk.Label(self.centerFrame,text="Bemerkung",font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
        # toolRemarkLabel.grid(row=8,column=0)

        # self.toolRemarkEntry = tk.Text(self.centerFrame, height=4, width=25, font=(winconfig["fonttype"], winconfig["fontsize"]))
        # self.toolRemarkEntry.grid(row=8, column=1, padx=10, pady=10)

        # self.toolComoboBox.bind('<KeyRelease>', self.toolArticlesListBoxUpdate)
        # self.toolEntry.bind('<ButtonRelease-1>', self.toolArticlesAndRadiusSelection)
        # self.toolComoboBox.bind("<<ValueChanged>>",self.toolArticlesAndRadiusSelection)
        # self.toolComoboBox.bind('<FocusOut>', self.toolArticlesListBoxHide)
        # self.toolEntry.bind('<FocusOut>', self.toolArticlesListBoxHide)
        
        # self.toolNameComboBox.bind('<KeyRelease>', self.toolNameListBoxUpdate)
        # self.toolNameListBox.bind('<ButtonRelease-1>', self.toolNameSelection)
        # self.toolNameComboBox.bind("<<ValueChanged>>",self.toolNameSelection)
        # self.toolNameComboBox.bind('<FocusOut>', self.toolNameListBoxHide)
        # self.toolNameListBox.bind('<FocusOut>', self.toolNameListBoxHide)
        
        # self.holderAddition1ComboBox.bind('<KeyRelease>', self.holderAddition1ListBoxUpdate)
        # self.holderAddition1ListBox.bind('<ButtonRelease-1>', self.holderAddition1Selection)
        # self.holderAddition1ComboBox.bind("<<ValueChanged>>",self.holderAddition1Selection)
        # self.holderAddition1ComboBox.bind('<FocusOut>', self.holderAddition1ListBoxHide)
        # self.holderAddition1ListBox.bind('<FocusOut>', self.holderAddition1ListBoxHide)

        # self.holderAddition2ComboBox.bind('<KeyRelease>', self.holderAddition1Update)
        # self.holderAddition2ListBox.bind('<ButtonRelease-1>', self.holderAddition2Selection)
        # self.holderAddition2ComboBox.bind("<<ValueChanged>>",self.holderAddition2Selection)
        # self.holderAddition2ComboBox.bind('<FocusOut>', self.holderAddition2ListBoxHide)
        # self.holderAddition2ListBox.bind('<FocusOut>', self.holderAddition2ListBoxHide)
        

    def schaftToolInput(self):
                # Load and resize the image
            image = Image.open(paths["imgpaths"] + "schaftinput.png")
            resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

            # Convert to PhotoImage
            photo = ImageTk.PhotoImage(resized_image)

            # Create Label for the image
            img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

            # Place in the center using relx and rely
            img_label.place(relx=0.5, rely=0.5, anchor="center")

            # Prevent garbage collection
            img_label.image_ = photo

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")

            label1=Label(self.centerFrame,text="Z",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label1.place(x=400,y=70)
            entry1=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry1.place(x=430,y=70)



            label2=Label(self.centerFrame,text="R2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label2.place(x=440,y=450)
            entry2=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry2.place(x=490,y=450)


            label3=Label(self.centerFrame,text="\u2220",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label3.place(x=400,y=770)
            entry3=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry3.place(x=440,y=770)

            label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label4.place(x=910,y=30)
            entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry4.place(x=940,y=30)

            label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label5.place(x=790,y=200)
            entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry5.place(x=825,y=200)

            label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1060,y=230)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry6.place(x=1095,y=230)


            label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1200,y=400)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
            entry6.place(x=1230,y=400)

    def mkToolInput(self):
                # Load and resize the image
            image = Image.open(paths["imgpaths"] + "mkinput.png")
            resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

            # Convert to PhotoImage
            photo = ImageTk.PhotoImage(resized_image)

            # Create Label for the image
            img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

            # Place in the center using relx and rely
            img_label.place(relx=0.5, rely=0.5, anchor="center")

            # Prevent garbage collection
            img_label.image_ = photo

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")

            label1=Label(self.centerFrame,text="Z",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label1.place(x=370,y=90)
            entry1=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry1.place(x=400,y=90)



            label2=Label(self.centerFrame,text="R2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label2.place(x=440,y=430)
            entry2=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry2.place(x=490,y=430)


            label3=Label(self.centerFrame,text="\u2220",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label3.place(x=370,y=725)
            entry3=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry3.place(x=400,y=725)

            label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label4.place(x=850,y=80)
            entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry4.place(x=880,y=80)

            label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label5.place(x=670,y=185)
            entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry5.place(x=705,y=185)

            label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1050,y=240)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry6.place(x=1085,y=240)


            label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1160,y=400)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
            entry6.place(x=1190,y=400)

    def bohrerToolInput(self):
                # Load and resize the image
            image = Image.open(paths["imgpaths"] + "bohrerinput.png")
            resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

            # Convert to PhotoImage
            photo = ImageTk.PhotoImage(resized_image)

            # Create Label for the image
            img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

            # Place in the center using relx and rely
            img_label.place(relx=0.5, rely=0.5, anchor="center")

            # Prevent garbage collection
            img_label.image_ = photo

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")

            label1=Label(self.centerFrame,text="Z",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label1.place(x=400,y=140)
            entry1=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry1.place(x=430,y=140)



            label3=Label(self.centerFrame,text="\u2220",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label3.place(x=400,y=620)
            entry3=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry3.place(x=430,y=620)

            label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label4.place(x=910,y=25)
            entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry4.place(x=940,y=25)

            label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label5.place(x=770,y=190)
            entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry5.place(x=805,y=190)

            label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1030,y=220)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry6.place(x=1065,y=220)


            label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1160,y=390)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
            entry6.place(x=1190,y=390)

    def gewindeToolInput(self):
                # Load and resize the image
            image = Image.open(paths["imgpaths"] + "gewindeinput.png")
            resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

            # Convert to PhotoImage
            photo = ImageTk.PhotoImage(resized_image)

            # Create Label for the image
            img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

            # Place in the center using relx and rely
            img_label.place(relx=0.5, rely=0.5, anchor="center")

            # Prevent garbage collection
            img_label.image_ = photo

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")



            label3=Label(self.centerFrame,text="P",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label3.place(x=370,y=580)
            entry3=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry3.place(x=400,y=580)

            label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label4.place(x=860,y=10)
            entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry4.place(x=890,y=10)

            label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label5.place(x=730,y=150)
            entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry5.place(x=765,y=150)

            label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=990,y=190)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry6.place(x=1025,y=190)


            label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1140,y=390)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
            entry6.place(x=1175,y=390)

    def KugelToolInput(self):
                # Load and resize the image
            image = Image.open(paths["imgpaths"] + "kugelinput.png")
            resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

            # Convert to PhotoImage
            photo = ImageTk.PhotoImage(resized_image)

            # Create Label for the image
            img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

            # Place in the center using relx and rely
            img_label.place(relx=0.5, rely=0.5, anchor="center")

            # Prevent garbage collection
            img_label.image_ = photo

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")

            label1=Label(self.centerFrame,text="Z",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label1.place(x=360,y=85)
            entry1=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry1.place(x=390,y=85)



            label2=Label(self.centerFrame,text="R2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label2.place(x=440,y=430)
            entry2=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry2.place(x=490,y=430)


            label3=Label(self.centerFrame,text="\u2220",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label3.place(x=360,y=720)
            entry3=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry3.place(x=390,y=720)

            label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label4.place(x=840,y=25)
            entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry4.place(x=870,y=25)

            label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label5.place(x=720,y=160)
            entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry5.place(x=755,y=160)

            label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1020,y=190)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry6.place(x=1055,y=190)


            label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1160,y=350)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
            entry6.place(x=1190,y=350)

    def TorusToolInput(self):
                # Load and resize the image
            image = Image.open(paths["imgpaths"] + "torusinput.png")
            resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

            # Convert to PhotoImage
            photo = ImageTk.PhotoImage(resized_image)

            # Create Label for the image
            img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

            # Place in the center using relx and rely
            img_label.place(relx=0.5, rely=0.5, anchor="center")

            # Prevent garbage collection
            img_label.image_ = photo

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")

            label1=Label(self.centerFrame,text="Z",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label1.place(x=360,y=85)
            entry1=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry1.place(x=390,y=85)



            label2=Label(self.centerFrame,text="R2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label2.place(x=440,y=430)
            entry2=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry2.place(x=490,y=430)


            label3=Label(self.centerFrame,text="\u2220",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label3.place(x=360,y=720)
            entry3=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry3.place(x=390,y=720)

            label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label4.place(x=840,y=25)
            entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry4.place(x=870,y=25)

            label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label5.place(x=720,y=145)
            entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry5.place(x=755,y=145)

            label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1020,y=175)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry6.place(x=1055,y=175)


            label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1160,y=350)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
            entry6.place(x=1190,y=350)

    
    def NutenToolInput(self):
                # Load and resize the image
            image = Image.open(paths["imgpaths"] + "nuteninput.png")
            resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

            # Convert to PhotoImage
            photo = ImageTk.PhotoImage(resized_image)

            # Create Label for the image
            img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

            # Place in the center using relx and rely
            img_label.place(relx=0.5, rely=0.5, anchor="center")

            # Prevent garbage collection
            img_label.image_ = photo

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")

            label1=Label(self.centerFrame,text="Z",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label1.place(x=380,y=250)
            entry1=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry1.place(x=410,y=250)



            label2=Label(self.centerFrame,text="R2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label2.place(x=440,y=505)
            entry2=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
            entry2.place(x=490,y=505)


            label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label4.place(x=840,y=60)
            entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
            entry4.place(x=870,y=60)

            label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label5.place(x=710,y=160)
            entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry5.place(x=745,y=160)

            label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1010,y=215)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
            entry6.place(x=1045,y=215)


            label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
            label6.place(x=1170,y=370)
            entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
            entry6.place(x=1200,y=370)

    def ReibenToolInput(self):
            # Load and resize the image
        image = Image.open(paths["imgpaths"] + "reibahleinput.png")
        resized_image = image.resize((1500, 843), Image.Resampling.LANCZOS)  # Set desired width & height

        # Convert to PhotoImage
        photo = ImageTk.PhotoImage(resized_image)

        # Create Label for the image
        img_label = Label(self.centerFrame, image=photo, bg=winconfig["bgcolor"])

        # Place in the center using relx and rely
        img_label.place(relx=0.5, rely=0.5, anchor="center")

        # Prevent garbage collection
        img_label.image_ = photo

        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]+6, weight="bold")

        label1=Label(self.centerFrame,text="Z",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        label1.place(x=360,y=180)
        entry1=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
        entry1.place(x=390,y=180)




        label3=Label(self.centerFrame,text="\u2220",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        label3.place(x=360,y=600)
        entry3=Entry(self.centerFrame,font=labelFontConfiguration,width=3)
        entry3.place(x=390,y=600)

        label4=Label(self.centerFrame,text="R",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        label4.place(x=840,y=20)
        entry4=Entry(self.centerFrame,font=labelFontConfiguration,width=6)
        entry4.place(x=870,y=20)

        label5=Label(self.centerFrame,text="L2",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        label5.place(x=720,y=160)
        entry5=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
        entry5.place(x=755,y=160)

        label6=Label(self.centerFrame,text="L3",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        label6.place(x=1010,y=200)
        entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=4)
        entry6.place(x=1045,y=200)


        label6=Label(self.centerFrame,text="L",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        label6.place(x=1160,y=350)
        entry6=Entry(self.centerFrame,font=labelFontConfiguration,width=8)
        entry6.place(x=1190,y=350)





    def editToolInputWindow(self, data):
        self.clearFrame()
        self.centerFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)


        itemsDB=paths["MTMitems"]
        masterCsvFile=paths["mastercsv"]
        self.itemNameExamples = set()
        self.dataToolEntries = []
        self.toolNameExamples=set()
        self.movingTools=[data[0]]
        try:
            self.itemNameExamples.update(self.load_items_from_db(itemsDB))

        except FileNotFoundError:

            print(f"Error: File {itemsDB} not found.")
        try:
            with open(paths["mastercsv"], 'r') as file:
                dataMasterCsv = csv.reader(file, delimiter=";")
                for row in dataMasterCsv:
                    if len(row) >= 3:
                        self.toolNameExamples.update(row[1].split(';'))
                    else:
                        print(f"Warning: Row {dataMasterCsv.line_num} has less than 9 columns.")
        except FileNotFoundError:
            print(f"Error: File '{masterCsvFile}' not found.")

        self.toolNameExamples = list(self.toolNameExamples)

        toolPlaces = set()
        clampingSystems=set()
        toolTypes=("Bohrer","Schruppfraeser","Schlichtfraeser","","FraesWZ","Reibahle","Gewindebohrer","Nutenfraeser","Torusfraeser","Kugelfraeser")

        for dictionary in places :

            if dictionary:
                toolPlaces.add(dictionary["placename"])

            if dictionary["status"]=="machine":
                clampingSystems.add(dictionary["holdertype"]) 

        toolPlaces = list(toolPlaces)  # Convert set to list for use in Combobox
        clampingSystems=list(clampingSystems)

        toolDataEntriesLabels = ['CODE',
            'Nummer', 'Name', 'Typ','Radius','Eckenradius','Spitzenwinkel','EinTauchwinkel','Schneiden','Schnittlänge','Freischliff','Wzg_gesamtlänge','Steigung',
            'Wzg_ArtNr', 'Aufn_ArtNr', 'Zusaufn_1', 'Zusaufn_2', 'Spannsys.', 'Platz'
        ]

        self.dataToolEntries = []
        for idx,  toolDataEntryLabel in enumerate(toolDataEntriesLabels):
            # Calculate the row and column positions based on the index
            row = idx % 8
            col = (idx // 8) * 2  # Multiplying by 2 tlo leave space for entries

            labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")
            addToolEntryLabel = tk.Label(self.centerFrame, text=toolDataEntryLabel, font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
            addToolEntryLabel.grid(row=row, column=col, padx=10, pady=5, sticky='e')
            
            if toolDataEntryLabel== 'CODE':
                addToolDataEntry = tk.StringVar(value=data[idx])
                iDCodeEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11), state='disabled')
                iDCodeEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
            elif toolDataEntryLabel == 'Platz':
                addToolDataEntry = tk.StringVar(value=data[idx])
                self.toolPlace=addToolDataEntry
                toolPlaceEntry = ttk.Combobox(self.centerFrame, textvariable=addToolDataEntry, font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly') #entry1
                toolPlaceEntry['values'] = toolPlaces
                toolPlaceEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
                self.toolPlace.trace_add('write', self.checkTNC640PlcAndTTDatasButton)
            elif toolDataEntryLabel == 'Spannsys.':
                addToolDataEntry = tk.StringVar(value=data[idx])
                toolClampingSystemEntry = ttk.Combobox(self.centerFrame, textvariable=addToolDataEntry, font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly') #entry2
                toolClampingSystemEntry['values'] = clampingSystems
                toolClampingSystemEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
            elif toolDataEntryLabel== 'Name':
                addToolDataEntry = tk.StringVar(value=data[idx])
                self.toolNameListBox = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]-2))
                self.toolNameListBox.place_forget()
                self.toolNameComboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]-2))
                self.toolNameComboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
            elif toolDataEntryLabel== 'Typ':
                addToolDataEntry = tk.StringVar(value=data[idx])
                toolTypeComboBox = ttk.Combobox(self.centerFrame, textvariable=addToolDataEntry, font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly')
                toolTypeComboBox['value'] = toolTypes
                toolTypeComboBox.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
                self.toolTypeEntry=addToolDataEntry
                self.toolTypeEntry.trace_add('write', self.toolTypeCheck)
            elif toolDataEntryLabel == 'Wz_ArtNr':
                addToolDataEntry = tk.StringVar(value=data[idx])
                self.toolEntry = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]))
                self.toolEntry.place_forget()
                self.toolComoboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]))
                self.toolComoboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
            elif toolDataEntryLabel == "Aufn_ArtNr":  # Checking if it's the 7th entry (0-based index)
                addToolDataEntry = tk.StringVar(value=data[idx])
                toolHolderEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
                toolHolderEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
                toolHolderEntry.config(state='disabled')  # Disabling the entry
                self.toolHolderEntry=toolHolderEntry.get()
            elif toolDataEntryLabel== 'Steigung':
                addToolDataEntry = tk.StringVar(value=data[idx])
                toolPitchEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
                toolPitchEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
                self.toolPitch = toolPitchEntry
                self.toolPitchEntry=addToolDataEntry
                self.toolTypeCheck()
            elif toolDataEntryLabel == 'Wzg_gesamtlänge':
                addToolDataEntry = tk.StringVar(value=data[idx])
                toolLengthEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
                toolLengthEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')
                self.toolLengthEntry = addToolDataEntry  # Store the value in an instance variable for later use
            elif toolDataEntryLabel== 'Zusaufn_1':
                addToolDataEntry = tk.StringVar(value=data[idx])
                self.holderAddition1ListBox = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]))
                self.holderAddition1ListBox.place_forget()
                self.holderAddition1ComboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]))
                self.holderAddition1ComboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
            elif toolDataEntryLabel== 'Zusaufn_2':  # Storing the entry widget of 'Zusaufn_2' (9th entry) for later use
                addToolDataEntry = tk.StringVar(value=data[idx])
                self.holderAddition2ListBox = tk.Listbox(self.centerFrame ,bg='white', font=(winconfig["fonttype"], winconfig["fontsize"]))
                self.holderAddition2ListBox.place_forget()
                self.holderAddition2ComboBox = ttk.Combobox(self.centerFrame,textvariable=addToolDataEntry,font=(winconfig["fonttype"], winconfig["fontsize"]))
                self.holderAddition2ComboBox.grid(row=row,column=col+1,padx=10, pady=5, sticky='w')
                style = ttk.Style()
                style.configure("Custom.TCheckbutton", background=winconfig["bgcolor"],foreground=winconfig["fontcolor"],font=(winconfig["fonttype"], winconfig["fontsize"], "bold"))
                self.holderAddition2CheckBoxValue = tk.IntVar()
                self.holderAddition2CheckBox = ttk.Checkbutton(self.centerFrame, text="IKZ-Röhrchen", variable=self.holderAddition2CheckBoxValue, command=self.coolerTubeImgCheckBox, style="Custom.TCheckbutton")
                self.holderAddition2CheckBox.grid(row=row,column=col +2)
            else:
                addToolDataEntry = tk.StringVar(value=data[idx])
                toolDataEntry = tk.Entry(self.centerFrame, textvariable=addToolDataEntry, font=("Helvetica", 11))
                toolDataEntry.grid(row=row, column=col + 1, padx=10, pady=5, sticky='w')

            self.dataToolEntries.append(addToolDataEntry)
            




        self.TNC640PlcAndTTDatenButton = tk.Button(self.centerFrame, text="TNC640+Laser", command=lambda: TNC640Laser(data[0],self.toolHolderEntry,self.toolLengthEntry.get()),font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        self.TNC640PlcAndTTDatenButton.grid(row=4, column=5, padx=10, pady=5)
        self.TNC640PlcAndTTDatenButton.grid_remove()  # Hide it initially


        # Create a global reference to the subplace Combobox
        self.subPlace_comboBox = None

        # Function to handle updates
        def showSubPlaceComboBox(event=None):
            # Get the current value of the combobox
            maschineComboBoxValue = self.toolPlace.get()

            # Destroy the previous subplace Combobox if it exists
            if self.subPlace_comboBox:
                self.subPlace_comboBox.destroy()
                self.subPlace_comboBox = None

            # Search for the selected place in the data
            for dictionary in places:
                if dictionary["placename"] == maschineComboBoxValue:
                    if dictionary["status"] == "place":
                        # Extract subplace names
                        subplace_names = [subplace["subplacename"] for subplace in dictionary.get("subplace", [])]
                        if subplace_names:
                            # Create a new subplace Combobox
                            self.subPlace_comboBox = ttk.Combobox(self.centerFrame, values=subplace_names,font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly')
                            self.subPlace_comboBox.grid(row=3, column=5, padx=5, pady=5, sticky="ew")
                        else:
                            print("No subplaces available.")
                    elif dictionary["status"] == "maschine":
                        print("Selected item is a machine. Subplace Combobox removed.")
                    break
            else:
                print("No matching place found.")

        # Bind user selection and trace variable changes
        toolPlaceEntry.bind("<<ComboboxSelected>>", showSubPlaceComboBox)
        self.toolPlace.trace_add("write", lambda *args: showSubPlaceComboBox())




        self.clearFrameRight()
        # self.rightFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        # self.rightFrame.place(relx=1.0, rely=0.5, anchor=tk.E)

        self.buttonsFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.buttonsFrame.place(relx=0.95, rely=0.44, anchor=tk.E)
        self.rightFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.rightFrame.place(relx=0.5, rely=0.8, anchor=tk.CENTER)


        try:
            # Attempt to open the specified image
            toolHolderEntryImage = Image.open(paths["compimgs"] +self.toolHolderEntry[:6]+".jpg")
            
        except FileNotFoundError:
            # If the image is not found, open the default image
            toolHolderEntryImage = Image.open(paths["compimgs"]+"NOTFOUND.png")

        # Resize the image to fit the label
        toolHolderEntryImage = toolHolderEntryImage.resize((150, 150), Image.LANCZOS)
        # Rotate the image
        toolHolderEntryImage = toolHolderEntryImage.rotate(0, expand=True)
        # Convert the image to a PhotoImage object for Tkinter
        toolHolderEntryPhoto = ImageTk.PhotoImage(toolHolderEntryImage)


        toolHolderEntryImageLabel = tk.Label(self.rightFrame, image=toolHolderEntryPhoto)
        toolHolderEntryImageLabel.image = toolHolderEntryPhoto  # Keep a reference to avoid garbage collection
        toolHolderEntryImageLabel.grid(row=0,column=2,padx=50)

        self.dataToolEntriesToDelete=[]
        self.dataToolEntriesToDelete=[iDCodeEntry.get(),toolHolderEntry.get(),toolPlaceEntry.get()]
        self.toolName=self.toolNameComboBox.get()
        self.tplace=self.toolPlace.get()





        measurementsButton = tk.Button(self.buttonsFrame,width=20, text="Messwerte Übernehmen", command=lambda: self.messwerte_uebernehmen, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        measurementsButton.grid(row=0, column=7, pady=10, sticky='w')

        takePictureButton = tk.Button(self.buttonsFrame, width=20,text="Bild machen", command=self.openCamera, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        takePictureButton.grid(row=1, column=7, pady=10, sticky='w')

        toolDatasSubmitButton = tk.Button(self.buttonsFrame, width=20,text="EinFügen", command=self.toolDatasSubmitButton, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        toolDatasSubmitButton.grid(row=2,column=7, pady=10, sticky='w')

        cancelButton = tk.Button(self.buttonsFrame, width=20,text="Abbrechen", command=self.goHome, font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        cancelButton.grid(row=3, column=7, pady=10, sticky='w')

        deleteButton = tk.Button(self.buttonsFrame, width=20,text="Löschen", command=lambda: self.deleteToolDataFunction(self.dataToolEntriesToDelete), font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        deleteButton.grid(row=4, column=7, pady=10, sticky='w')

        self.toolImageLabel = tk.Label(self.rightFrame)  # Label to display the image
        self.toolImageLabel.grid(row=0,column=0,padx=50)

        self.holderAddition1ImageLabel = tk.Label(self.rightFrame)  # Label to display the image
        self.holderAddition1ImageLabel.grid(row=0,column=1,padx=50)
        
        self.holderAddition2ImageLabel = tk.Label(self.rightFrame)
        self.holderAddition2ImageLabel.grid(row=0,column=3)
        toolRemarkLabel = tk.Label(self.centerFrame,text="Bemerkung",font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
        toolRemarkLabel.grid(row=8,column=0)

        self.toolRemarkEntry = tk.Text(self.centerFrame, height=4, width=25, font=(winconfig["fonttype"], winconfig["fontsize"]))
        self.toolRemarkEntry.grid(row=8, column=1, padx=10, pady=10)

        self.toolComoboBox.bind('<KeyRelease>', self.toolArticlesListBoxUpdate)
        self.toolEntry.bind('<ButtonRelease-1>', self.toolArticlesAndRadiusSelection)
        self.toolComoboBox.bind("<<ValueChanged>>",self.toolArticlesAndRadiusSelection)
        self.toolComoboBox.bind('<FocusOut>', self.toolArticlesListBoxHide)
        self.toolEntry.bind('<FocusOut>', self.toolArticlesListBoxHide)
        
        self.toolNameComboBox.bind('<KeyRelease>', self.toolNameListBoxUpdate)
        self.toolNameListBox.bind('<ButtonRelease-1>', self.toolNameSelection)
        self.toolNameComboBox.bind("<<ValueChanged>>",self.toolNameSelection)
        self.toolNameComboBox.bind('<FocusOut>', self.toolNameListBoxHide)
        self.toolNameListBox.bind('<FocusOut>', self.toolNameListBoxHide)
        
        self.holderAddition1ComboBox.bind('<KeyRelease>', self.holderAddition1ListBoxUpdate)
        self.holderAddition1ListBox.bind('<ButtonRelease-1>', self.holderAddition1Selection)
        self.holderAddition1ComboBox.bind("<<ValueChanged>>",self.holderAddition1Selection)
        self.holderAddition1ComboBox.bind('<FocusOut>', self.holderAddition1ListBoxHide)
        self.holderAddition1ListBox.bind('<FocusOut>', self.holderAddition1ListBoxHide)

        self.holderAddition2ComboBox.bind('<KeyRelease>', self.holderAddition1Update)
        self.holderAddition2ListBox.bind('<ButtonRelease-1>', self.holderAddition2Selection)
        self.holderAddition2ComboBox.bind("<<ValueChanged>>",self.holderAddition2Selection)
        self.holderAddition2ComboBox.bind('<FocusOut>', self.holderAddition2ListBoxHide)
        self.holderAddition2ListBox.bind('<FocusOut>', self.holderAddition2ListBoxHide)


    def deleteToolDataFunction(self, dataToolEntriesToDelete):
        self.askPlace = Toplevel()  # Instantiate Toplevel
        self.askPlace.title("Aufnahme platzieren")
        self.askPlace.config(bg=winconfig["bgcolor"])
        
        # Dimensions of the Toplevel window
        window_width = 300
        window_height = 150

        # Get screen width and height
        screen_width = self.askPlace.winfo_screenwidth()
        screen_height = self.askPlace.winfo_screenheight()

        # Calculate x and y coordinates for the Toplevel window
        x = (screen_width // 2) - (window_width // 2)
        y = (screen_height // 2) - (window_height // 2)

        # Set the geometry to center the window
        self.askPlace.geometry(f"{window_width}x{window_height}+{x}+{y}")

        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")


        # Extract unique placenames from 'places' with "status" == "place"
        toolPlaces = set()
        for dictionary in places:
            if dictionary and "placename" in dictionary:
                if dictionary["status"] == "place":
                    toolPlaces.add(dictionary["placename"])

        toolPlaces = list(toolPlaces)  # Convert the set to a list

                # Create a Combobox
        self.toolPlaceEntryTo= tk.StringVar()
        self.toolPlaceEntryToDelete = ttk.Combobox(self.askPlace,font=(winconfig["fonttype"], winconfig["fontsize"]),state='readonly')
        self.toolPlaceEntryToDelete['values'] = toolPlaces  # Assign values to the Combobox
        self.toolPlaceEntryToDelete.grid(row=1,column=1,padx=10, pady=5)  # Center horizontally and vertically




        # Create a global reference to the subplace Combobox
        self.subPlace_comboBox = None

        # Function to handle updates
        def showSubPlaceComboBox(event=None):
            # Get the current value of the combobox
            maschineComboBoxValue = self.toolPlaceEntryToDelete.get()

            # Destroy the previous subplace Combobox if it exists
            if self.subPlace_comboBox:
                self.subPlace_comboBox.destroy()
                self.subPlace_comboBox = None

            # Search for the selected place in the data
            for dictionary in places:
                if dictionary["placename"] == maschineComboBoxValue:
                    if dictionary["status"] == "place":
                        # Extract subplace names
                        subplace_names = [subplace["subplacename"] for subplace in dictionary.get("subplace", [])]
                        if subplace_names:
                            # Create a new subplace Combobox
                            subPlace_Label= tk.Label(self.askPlace, text="Unterplatz", font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
                            subPlace_Label.grid( row=2 , column=0,padx=10, pady=5)
                            subPlace_comboBox = ttk.Combobox(self.askPlace, values=subplace_names,font=(winconfig["fonttype"], winconfig["fontsize"]), state='readonly',default=None)
                            subPlace_comboBox.grid(row=2,column=1 ,padx=5, pady=5)
                        else:
                            print("No subplaces available.")
                    elif dictionary["status"] == "maschine":
                        print("Selected item is a machine. Subplace Combobox removed.")
                    break
            else:
                print("No matching place found.")

        # Bind user selection and trace variable changes
        self.toolPlaceEntryToDelete.bind("<<ComboboxSelected>>", showSubPlaceComboBox)
        self.toolPlaceEntryTo.trace_add("write", lambda *args: showSubPlaceComboBox())








        # Add a delete button
        self.deleteButton = Button(self.askPlace,text="Löschen",command=lambda: self.deleteToolDataButton(dataToolEntriesToDelete),font=labelFontConfiguration, bg=winconfig["bgcolor"], fg=winconfig["fontcolor"])
        self.deleteButton.grid(row=3,column=1,padx=10, pady=5)  # Center horizontally and vertically






    def deleteToolDataButton(self, dataToolEntriesToDelete):
        selected_place = self.toolPlaceEntryToDelete.get()  # Get the selected value from the Combobox
        if not selected_place:  # Check if no value is selected
            logging.error("No place selected.")
            messagebox.showerror("Platz fehlt", "Bitte wählen Sie einen Platz aus.")
            return
        else:
            updated = False
            # Rest of your delete functionality here


            # Connect to the MTMDB.db SQLite database
            try:
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
            except sqlite3.Error as e:
                logging.error(f"Error connecting to the database: {str(e)}")
                messagebox.showerror("Datenbankfehler", f"Fehler beim Verbinden zur Datenbank: {str(e)}")
                return

            # Check if the root window exists
            root = tk._default_root
            if not root:
                root = Toplevel()
                root.withdraw()  # Hide the root window

            try:
                # Prepare the updated row data
                updatedRow = {
                    "platz": self.toolPlaceEntryToDelete.get(),
                    "komponent2": dataToolEntriesToDelete[1],
                }

                # Update the database with the new values
                cur.execute(
                    """
                    UPDATE currentTools
                    SET toolNummer = NULL,
                        toolName = NULL,
                        toolTyp = NULL,
                        toolRadius = NULL,
                        toolEckenRadius = NULL,
                        toolSpitzenWinkel = NULL,
                        toolEintauchWinkel = NULL,
                        toolSchneiden = NULL,
                        toolSchnittLaenge = NULL,
                        toolAusspannlaenge = NULL,
                        toolIstLaenge = NULL,
                        toolSollLaenge = NULL,
                        toolSteigung = NULL,
                        komponent1 = NULL,
                        komponent2 = :komponent2,
                        komponent3 = NULL,
                        komponent4 = NULL,
                        komponent5 = NULL,
                        platz = :platz
                    WHERE idCode = :idCode
                    """,
                    {
                        **updatedRow,
                        "idCode": dataToolEntriesToDelete[0],
                    },
                )

                if cur.rowcount == 0:  # If no rows were updated
                    logging.error(f"The tool {dataToolEntriesToDelete[0]} {self.toolName} data could not be deleted from {self.tplace} to {selected_place}. {self.userName}.")    
                    messagebox.showerror("Fehler", "Diese Aufnahme konnte nicht gelöscht werden.")
                    return
                else:
                    logging.info(f"The tool {dataToolEntriesToDelete[0]} {self.toolName} data has been deleted from {self.tplace} to {selected_place}. {self.userName}")
                    messagebox.showinfo("Löschen", "Diese Aufnahme wurde gelöscht.")
                    conn.commit()  # Commit the transaction first
                    cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                    updated = True

            except sqlite3.Error as e:
                logging.error(f"Error updating the database: {str(e)}")
                messagebox.showerror("Datenbankfehler", f"Fehler beim Aktualisieren: {str(e)}")
                return
            finally:
                conn.close()

            # Handle PLC Data
            plcDataRow = []
            found = False

            #frozzen
            # try:
            #     with open(paths["TNC640_Daten"], "r", newline='') as file:
            #         tnc640DataFile = csv.reader(file, delimiter=';')
            #         for row in tnc640DataFile:
            #             if dataToolEntriesToDelete[0] in row:
            #                 plcDataRow = row
            #                 found = True
            #                 print("PLC Daten gefunden")
            #                 break
            # except FileNotFoundError:
            #     messagebox.showerror("Fehler", "PLC-Datendatei nicht gefunden.")
            #     return
            # except Exception as e:
            #     messagebox.showerror("Fehler", f"Fehler beim Lesen der PLC-Datendatei: {str(e)}")
            #     return



        # Update the logic to access MTMDB.db
        try:
            # Connect to the database
            conn = sqlite3.connect(paths["MTMDB"])
            cursor = conn.cursor()

            # Query the database for the required row
            cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (dataToolEntriesToDelete[0],))
            plcDataRow = cursor.fetchone()
            conn.close()

            if plcDataRow:
                found = True
                print("PLC Daten gefunden:", plcDataRow)
            else:
                found = False
                print("PLC Daten nicht gefunden.")

        except sqlite3.OperationalError as e:
            logging.error(f"Error accessing the database: {str(e)}")    
            messagebox.showerror("Fehler", f"Fehler beim Zugriff auf die Datenbank: {str(e)}")
            return
        except Exception as e:
            logging.error(f"Error processing the database: {str(e)}")
            messagebox.showerror("Fehler", f"Fehler beim Lesen der PLC-Datendatei: {str(e)}")
            return
        finally:
            if conn:# Close the database connection
                conn.close()




        # Create default PLC data if not found
        if found:
            cfgInput = dataToolEntriesToDelete[1].replace(" ", "_") + ".CFG"
            KLValue = str(int(float(400) + 10))
            plcDataRow = [
                dataToolEntriesToDelete[0], "0", "0", "0", "0", "0", "0", "0", "0",
                "0.5", "0.5", "-1", "35", KLValue, "0", "0", "0", "0", "0", "0",
                cfgInput, "0"
            ]



            try:
                # Connect to the SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cursor = conn.cursor()
                # Check if the row exists (assuming CODE is the unique identifier)
                cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (plcDataRow[0],))
                existing_row = cursor.fetchone()
                conn.close()
                # Define the column names in the same order as the values in plcDataRow
                columns = [
                    "CODE", "NMAX", "TIME1", "TIME2", "CURTIME", "LOFFS", "ROFFS", "LTOL", "RTOL",
                    "LBREAK", "RBREAK", "DIRECT", "Max_Durchmesser", "Max_Laenge", "P2", "BC", "IKZ", "ML", "MLR", "AM", "KINEMATIC", "PLC"
                ]
                


                if existing_row:
                    # Connect to the SQLite database
                    conn = sqlite3.connect(paths["MTMDB"])
                    cursor = conn.cursor()
                    # Update the existing row
                    update_query = f'''
                    UPDATE tnc640Data
                    SET {", ".join([f"{columns[i]} = ?" for i in range(1, len(columns))])}  -- Excluding CODE from the update
                    WHERE CODE = ?
                    '''
                    cursor.execute(update_query, (*plcDataRow[1:], plcDataRow[0]))  # Exclude CODE from being updated
                    print(f"PLC-Datensatz aktualisiert für {plcDataRow[0]}")
                    # Commit changes
                    conn.commit()  # Commit the transaction first
                    cursor.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                    conn.close()
                else:
                    # Insert the new row
                    insert_query = f'''
                    INSERT INTO tnc640Data ({", ".join(columns)})
                    VALUES ({", ".join(['?'] * len(columns))})
                    '''
                    cursor.execute(insert_query, plcDataRow)
                    print("Kein PLC-Datensatz gefunden\nStandard-PLC erstellt.")
                    # Commit changes
                    conn.commit()  # Commit the transaction first
                    cursor.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                    conn.close()

            except sqlite3.OperationalError as e:
                logging.error(f"Error accessing the database: {str(e)}")
                messagebox.showerror("Fehler", f"Fehler beim Zugriff auf die Datenbank: {str(e)}")
                return
            except Exception as e:
                logging.error(f"Error processing the database: {str(e)}")
                messagebox.showerror("Fehler", f"Fehler beim Bearbeiten der Datenbank: {str(e)}")
                return
            finally:
                if conn:# Close the database connection
                    conn.close()

        self.askPlace.destroy()
        self.goHome()


    def toolDatasSubmitButton(self):
        self.maschine_combobox=self.toolPlace.get()
        calltoolEntriesDataValidatorResult=self.calltoolEntriesDataValidator()
        print(f"here is calltoolEntriesDataValidatorResult ture or false : {calltoolEntriesDataValidatorResult}")
        if calltoolEntriesDataValidatorResult !=True:
            return
        self.platzieren()

        # if self.calltoolEntriesDataValidator():
        #     self.platzieren()
        #     # self.calltoolEntriesDataValidator()
        #     # self.movingTools=[self.dataToolEntries[0]]
            
        #     print(f"here is tooldataentriesrow 0 : {self.movingTools}")
        #     print(f"here is tooldataentriesrow 19 : {self.maschine_combobox}")
        # else:
        #     return
        





    def checkTNC640PlcAndTTDatasButton(self, *args):
        """ Show or hide the TNC640PlcAndTTDatasButton based on place? selection """
        self.TNC640PlcAndTTDatenButton.grid_remove()  # Hide the button
        for dict in places:
            if dict["status"]=="machine":
                if self.toolPlace.get() == dict["placename"]:
                    if dict["software"]=="TNC640Laser" or dict["software"]=="TNC640" :
                        self.TNC640PlcAndTTDatenButton.grid()  # Show the button
                    else:
                        self.TNC640PlcAndTTDatenButton.grid_remove()  # Hide the button

    def toolTypeCheck(self, *args):
        typ_value = self.toolTypeEntry.get()
        if typ_value == "Gewindebohrer":
            self.toolPitch.config(state='normal')
        else:
            self.toolPitch.config(state='disabled')
            self.toolPitchEntry.set(None)

    def coolerTubeImgCheckBox(self):
        if self.holderAddition2CheckBoxValue.get():
            self.dataToolEntries[16].set("309880 63")  # Set the value
            self.holderAddition2ComboBox.config(state='disabled')  # Disable the entry field
            self.showCoolerTubeImg()
        else:
            self.dataToolEntries[16].set("")
            self.holderAddition2ComboBox.config(state='normal')  # Enable the entry field
            self.HideCoolerTubeImg()

    def showCoolerTubeImg(self):

        self.coolerTubeimage = Image.open(paths["compimgs"]+"309880.jpg")
        self.coolerTubeimage = self.coolerTubeimage.rotate(0, expand=True)
        self.coolerTubeimage = self.coolerTubeimage.resize((150, 150), Image.LANCZOS)
        self.coolerTubePhoto = ImageTk.PhotoImage(self.coolerTubeimage)
        self.holderAddition2ImageLabel.config(image=self.coolerTubePhoto)

    def HideCoolerTubeImg(self):

        self.holderAddition2ImageLabel.config(image=None)

    def toolNameListBoxUpdate(self, event):
        typedText = self.toolNameComboBox.get()
        if typedText == None:
            self.toolNameListBox.place_forget()
        else:
            filteredNames = [toolName for toolName in self.toolNameExamples if typedText.lower() in toolName.lower()]
            self.toolNameListBox.delete(0, tk.END)
            for toolName in filteredNames:
                self.toolNameListBox.insert(tk.END, toolName)
            if filteredNames:
                self.toolNameListBox.place(x=self.toolNameComboBox.winfo_x(), y=self.toolNameComboBox.winfo_y() + self.toolNameComboBox.winfo_height(), width=self.toolNameComboBox.winfo_width())
                self.toolNameListBox.lift()

    def toolNameSelection(self, event):
        self.clickedIndex = self.toolNameListBox.nearest(event.y)
        self.clickIndex=self.toolNameComboBox.get()
        self.selectedOptionToolName = self.toolNameListBox.get(self.clickedIndex)
        self.toolNameComboBox.set(self.selectedOptionToolName)
        self.toolNameListBox.place_forget()

    def toolNameListBoxHide(self, event):
        self.toolNameListBox.place_forget()

    def holderAddition1ListBoxUpdate(self, event):
        typedText = self.holderAddition1ComboBox.get()
        if typedText == None:
            self.holderAddition1ListBox.place_forget()
        else:
            filteredItems = [item for item in self.itemNameExamples if typedText.lower() in item.lower()]
            self.holderAddition1ListBox.delete(0, tk.END)
            for item in filteredItems:
                self.holderAddition1ListBox.insert(tk.END, item)
            if filteredItems:
                self.holderAddition1ListBox.place(x=self.holderAddition1ComboBox.winfo_x(), y=self.holderAddition1ComboBox.winfo_y() + self.holderAddition1ComboBox.winfo_height(), width=self.holderAddition1ComboBox.winfo_width())
                self.holderAddition1ListBox.lift()

    def holderAddition1Selection(self, event):
        self.clickedIndexAddition1 = self.holderAddition1ListBox.nearest(event.y)
        self.clickIndexAddition1=self.holderAddition1ComboBox.get()
        self.selectedOptionAddition1 = self.holderAddition1ListBox.get(self.clickedIndexAddition1)
        self.holderAddition1ComboBox.set(self.selectedOptionAddition1)
        self.holderAddition1ListBox.place_forget()

        # Connect to the SQLite database
        conn = sqlite3.connect(paths["MTMitems"])
        cursor = conn.cursor()

        # Get the value from the holderAddition1ComboBox
        selected_value = self.holderAddition1ComboBox.get()

        # Query to check if the selected value exists in the artikelNumber column
        cursor.execute('SELECT artikelNumber FROM Items WHERE artikelNumber = ?', (selected_value,))
        result = cursor.fetchone()

        # If a matching artikelNumber is found
        if result:
            holderAddition1Article = result[0]
            holderAddition1ImagePath = f"{paths['compimgs']}{holderAddition1Article[:6]}.jpg"
            self.showHolderAddition1Image(holderAddition1ImagePath, width=150, height=150)

        # Close the database connection
        conn.close()


    def holderAddition1ListBoxHide(self, event):
        self.holderAddition1ListBox.place_forget()

    def holderAddition1Update(self, event):
        typed_text = self.holderAddition2ComboBox.get()
        if typed_text == None:
            self.holderAddition2ListBox.place_forget()
        else:
            filteredItems = [item for item in self.itemNameExamples if typed_text.lower() in item.lower()]
            self.holderAddition2ListBox.delete(0, tk.END)
            for item in filteredItems:
                self.holderAddition2ListBox.insert(tk.END, item)
            if filteredItems:
                self.holderAddition2ListBox.place(x=self.holderAddition2ComboBox.winfo_x(), y=self.holderAddition2ComboBox.winfo_y() + self.holderAddition2ComboBox.winfo_height(), width=self.holderAddition2ComboBox.winfo_width())
                self.holderAddition2ListBox.lift()

    def holderAddition2Selection(self, event):
        self.clickedIndexAddition2 = self.holderAddition2ListBox.nearest(event.y)
        self.clickIndexAddition2=self.holderAddition2ComboBox.get()
        self.selectedOptionAddition2 = self.holderAddition2ListBox.get(self.clickedIndexAddition2)
        self.holderAddition2ComboBox.set(self.selectedOptionAddition2)
        self.holderAddition2ListBox.place_forget()




        # Connect to the SQLite database
        conn = sqlite3.connect(paths["MTMitems"])
        cursor = conn.cursor()

        # Get the value from the holderAddition1ComboBox
        selected_value = self.holderAddition1ComboBox.get()

        # Query to check if the selected value exists in the artikelNumber column
        cursor.execute('SELECT artikelNumber FROM Items WHERE artikelNumber = ?', (selected_value,))
        result = cursor.fetchone()

        # If a matching artikelNumber is found
        if result:
            holderAddition2Article = result[0]
            holderAddition2ImagePath = f"{paths['compimgs']}{holderAddition2Article[:6]}.jpg"
            self.showHolderAddition2Image(holderAddition2ImagePath, width=150, height=150)

        # Close the database connection
        conn.close()


    def holderAddition2ListBoxHide(self, event):
        self.holderAddition2ListBox.place_forget()

    def toolArticlesListBoxUpdate(self, event):
        typed_text = self.toolComoboBox.get()
        if typed_text == None:
            self.toolEntry.place_forget()
        else:
            filteredItems = [item for item in self.itemNameExamples if typed_text.lower() in item.lower()]
            self.toolEntry.delete(0, tk.END)
            for item in filteredItems:
                self.toolEntry.insert(tk.END, item)
            if filteredItems:
                self.toolEntry.place(x=self.toolComoboBox.winfo_x(), y=self.toolComoboBox.winfo_y() + self.toolComoboBox.winfo_height(), width=self.toolComoboBox.winfo_width())
                self.toolEntry.lift()

    def toolArticlesAndRadiusSelection(self, event):
        self.clickedIndexArticlesAndRadius = self.toolEntry.nearest(event.y)
        self.clickIndexArticlesAndRadius=self.toolComoboBox.get()
        self.selectedOptionArticlesAndRadius = self.toolEntry.get(self.clickedIndexArticlesAndRadius)
        toolRadius=self.dataToolEntries[4].get()
        toolDiameter=float(toolRadius)*2
        toolDiameter=int(toolDiameter)
        toolDiameter=str(toolDiameter)
        self.toolComoboBox.set(self.selectedOptionArticlesAndRadius+f" {toolDiameter}")
        self.toolEntry.place_forget()
        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"]-4, weight="bold")
        warningArticlesAndRadiusLabel=Label(self.centerFrame,text="Bitte Werkzeugsartikelnummer\n kontrollieren bzw. korrigieren!!!",font=labelFontConfiguration,bg=winconfig["bgcolor"],fg="red")
        warningArticlesAndRadiusLabel.grid(row=10,column=1)



        # Connect to the SQLite database
        conn = sqlite3.connect(paths["MTMitems"])
        cursor = conn.cursor()

        # Get the value from toolComoboBox and truncate to first 6 characters
        tool_article_number = self.toolComoboBox.get()[:6]

        # Query the database to check if the truncated value exists in artikelNumber
        cursor.execute('SELECT artikelNumber FROM Items WHERE artikelNumber LIKE ?', (tool_article_number,))
        result = cursor.fetchone()

        # If a matching artikelNumber is found
        if result:
            self.toolImageName = result[0]
            tool_image_path = f"{paths['compimgs']}{self.toolImageName}.jpg"
            self.showToolImage(tool_image_path, width=150, height=150)

        # Close the database connection
        conn.close()



    def toolArticlesListBoxHide(self, event):
        self.toolEntry.place_forget()

    def showToolImage(self, toolImagePath, width, height):
        try:
            # Attempt to load the specified image
            toolImage = Image.open(toolImagePath)
            toolImage = toolImage.resize(( width, height), Image.LANCZOS)  # Resize the image to fit the label
            toolImage = toolImage.rotate(0, expand=True)
        except (FileNotFoundError):
        # Load the image
            toolImage = Image.open(paths["compimgs"]+"NOTFOUND.png")
            toolImage = toolImage.resize(( 150, height), Image.LANCZOS)  # Resize the image to fit the label
            toolImage = toolImage.rotate(0, expand=True)
        toolPhoto = ImageTk.PhotoImage(toolImage)
        
        # Update the image label
        self.toolImageLabel.config(image=toolPhoto)
        self.toolImageLabel.image = toolPhoto

    def showHolderAddition1Image(self, holderAddition1ImagePath, width, height):
        try:
            # Attempt to load the specified image
            holderAddition1Image = Image.open(holderAddition1ImagePath)
            holderAddition1Image = holderAddition1Image.resize(( width, height), Image.LANCZOS)  # Resize the image to fit the label
            holderAddition1Image = holderAddition1Image.rotate(0, expand=True)
        except (FileNotFoundError):
        # Load the image
            holderAddition1Image = Image.open(paths["compimgs"]+"NOTFOUND.png")
            holderAddition1Image = holderAddition1Image.resize(( 150, height), Image.LANCZOS)  # Resize the image to fit the label
            holderAddition1Image = holderAddition1Image.rotate(0, expand=True)
        holderAddition1Photo = ImageTk.PhotoImage(holderAddition1Image)
        
        # Update the image label
        self.holderAddition1ImageLabel.config(image=holderAddition1Photo)
        self.holderAddition1ImageLabel.image = holderAddition1Photo

    def showHolderAddition2Image(self, holderAddition2ImagePath, width, height):
        try:
            # Attempt to load the specified image
            holderAddition2Image = Image.open(holderAddition2ImagePath)
            holderAddition2Image = holderAddition2Image.resize(( width, height), Image.LANCZOS)  # Resize the image to fit the label
            holderAddition2Image = holderAddition2Image.rotate(0, expand=True)
        except (FileNotFoundError):
        # Load the image
            holderAddition2Image = Image.open(paths["compimgs"]+"NOTFOUND.png")
            holderAddition2Image = holderAddition2Image.resize(( 150, height), Image.LANCZOS)  # Resize the image to fit the label
            holderAddition2Image = holderAddition2Image.rotate(0, expand=True)
        holderAddition2Photo = ImageTk.PhotoImage(holderAddition2Image)
        
        # Update the image label
        self.holderAddition2ImageLabel.config(image=holderAddition2Photo)
        self.holderAddition2ImageLabel.image = holderAddition2Photo

    

    def openCamera(self):
        import imageio
        # Open a new window for the camera without any additional banners or options
        self.cameraWindow = Toplevel(self.mainFrame)
        self.cameraWindow.title("Camera")

        self.openCameraFrame = Label(self.cameraWindow)
        self.openCameraFrame.pack()

        captureImageButton = Button(
            self.cameraWindow, 
            text="Capture", 
            command=self.captureImage, 
            font=(winconfig["fonttype"], winconfig["fontsize"], "bold"), 
            bg=winconfig["fontcolor"]
        )
        captureImageButton.pack()

        # Handle closing event of the camera window
        self.cameraWindow.protocol("WM_DELETE_WINDOW", self.on_closing)

        cameraIndex = "video" + str(settings["camindex"])

        try:
            # Try to open the camera feed
            self.cameraFrame = imageio.get_reader(f"<{cameraIndex}>", format='ffmpeg')
            # If no error occurs, camera is active
            self.cameraActive = True
        except IndexError:
            # Handle error if the camera index is incorrect or not found
            print(f"Error: Camera at index {cameraIndex} not found.")
            self.cameraFrame = None  # No camera feed
            self.cameraActive = False  # Mark camera as inactive

        # Initialize frame index only if the camera is active
        if self.cameraActive:
            self.cameraFrameIndex = 0
            self.showCameraFrame()
        else:
            logging.error("Camera could not be accessed. Please check the camera index.")
            messagebox.showerror("CAM_INDEX","Camera could not be accessed. Please check the camera index.")
            # Inform the user if camera cannot be accessed
            self.cameraWindow.destroy()  # Close the window if camera is not accessible


    def on_closing(self):
        # Stop the camera feed if it's active
        if self.cameraActive:
            self.cameraFrame.close()  # Close the camera feed
            self.cameraActive = False
            
        # Destroy the camera window
        self.cameraWindow.destroy()


    def showCameraFrame(self):
        if not self.cameraActive:
            return  # Stop the camera loop if the camera is inactive
        # Read the next frame from the camera
        try:
            frame = self.cameraFrame.get_data(self.cameraFrameIndex)
            self.cameraFrameIndex += 1  # Increment frame index for the next frame

            # Convert the frame to an Image object and then to ImageTk for displaying
            img = Image.fromarray(frame)
            imgtk = ImageTk.PhotoImage(image=img)
            self.openCameraFrame.imgtk = imgtk
            self.openCameraFrame.configure(image=imgtk)

            # Call showCameraFrame again after 10 ms
            self.openCameraFrame.after(10, self.showCameraFrame)
        except Exception as e:
            print("Error capturing frame:", e)

    def captureImage(self):
        # Capture the current frame
        frame = self.cameraFrame.get_data(self.cameraFrameIndex)

        # Close the camera window
        self.closeCamera()
        self.cameraWindow.destroy()

        # Display the captured image in a new window with Save and Retake buttons
        self.showCapturedImage(frame)

    def showCapturedImage(self, frame):
        # Create a new window to display the captured image
        
        self.showCapturedImageWindow = Toplevel(self.mainFrame)
        self.showCapturedImageWindow.title("Captured Image")

        # Convert the frame to an Image object and then to ImageTk for displaying
        img = Image.fromarray(frame)
        imgtk = ImageTk.PhotoImage(image=img)

        # Display the captured image
        self.capturedImageLabel = Label(self.showCapturedImageWindow, image=imgtk)
        self.capturedImageLabel.image = imgtk  # Keep a reference to avoid garbage collection
        self.capturedImageLabel.pack()

        # Add Save and Retake buttons
        imageSaveButton = Button(self.showCapturedImageWindow, text="Speichern", command=lambda: self.saveCapturedImage(img, frame), font=(winconfig["fonttype"], winconfig["fontsize"], "bold"), bg=winconfig["fontcolor"])
        imageSaveButton.pack(side="left", padx=10, pady=10)

        retakeImageButton = Button(self.showCapturedImageWindow, text="Neu", command=self.retakeImage, font=(winconfig["fonttype"], winconfig["fontsize"], "bold"), bg=winconfig["fontcolor"])
        retakeImageButton.pack(side="right", padx=10, pady=10)

    def saveCapturedImage(self, img, frame):
        # Save the image to a folder named 'aufnahme'
        if not os.path.exists('aufnahme'):
            os.makedirs('aufnahme')
        capturedImageName = os.path.join(paths["datapath"]+'toolCapturedImages', f"{self.iDCode}.png")

        img.save(capturedImageName)
        logging.info(f"Image saved as '{self.iDCode}.png' in folder 'data/toolCapturedImages'. - {self.userName}")
        messagebox.showinfo("Image Saved", f"Image captured and saved in folder 'data/toolCapturedImages' as '{self.iDCode}.png'.")
        self.showCapturedImageWindow.destroy()

    def retakeImage(self):
        # Close the image window and reopen the camera
        self.showCapturedImageWindow.destroy()
        # self.cameraWindow.destroy()
        self.openCamera()

    def closeCamera(self):
        self.cameraActive = False
        # Release the camera resource and close the window
        if self.cameraFrame is not None:
            self.cameraFrame.close()
        if hasattr(self, 'camera_window') and self.cameraWindow is not None:
            self.cameraWindow.destroy()

    def calltoolEntriesDataValidator(self):

        data = [addToolEntry.get() for addToolEntry in self.dataToolEntries]
        self.dataToValidate=data
        validateAndAddToolDataEntryResult=self.validateAndAddToolDataEntry( *data)
        if validateAndAddToolDataEntryResult != True:
            print(f"here is validateAndAddToolDataEntry ture or false : {validateAndAddToolDataEntryResult}")
            return False
        print(f"here is validateAndAddToolDataEntry ture or false after false : {validateAndAddToolDataEntryResult}")

        return True
        
    def validateAndAddToolDataEntry(self, iDCode, toolNumber, toolName,toolType,toolRadius,toolEdgeRadius,toolPointAngle, toolAngle,toolTeeth,cutLenght,toolOverhangLength, toolLenght ,toolPitch,toolArticleNumber, holderArticleNumber, holderAddition1, holderAddition2, tensioningSystem, toolPlace):

        toolTypesDictionary = {
                "Bohrer": 1,
                "Schruppfraeser": 9,
                "Schlichtfraeser": 10,
                "":99,
                "FraesWZ":0,
                "Reibahle":3,
                "Gewindebohrer":2,
                "Nutenfraeser":7,
                "Torusfraeser":23,
                "Kugelfraeser":22
                }
        if toolType in toolTypesDictionary:
            toolType=toolTypesDictionary[toolType]

        # iDCode = iDCode.upper()
        toolNumber = toolNumber.upper()
        toolName = toolName.upper()
        toolRadius = toolRadius.replace(',', '.')
        toolEdgeRadius= toolEdgeRadius.replace(',', '.')
        toolPointAngle=toolPointAngle.replace(',', '.')
        toolAngle=toolAngle.replace(',', '.')
        cutLenght=cutLenght.replace(',', '.')
        toolOverhangLength=toolOverhangLength.replace(',', '.')
        toolLenght=toolLenght.replace(',', '.')
        toolPitch=toolPitch.replace(',', '.')
        toolArticleNumber = toolArticleNumber.replace(',', '.')
        toolArticleNumber = toolArticleNumber.upper()
        tensioningSystem = tensioningSystem.upper()
        # toolPlace = toolPlace.upper()
        
        if holderAddition1 !="" :
            holderAddition1 = holderAddition1.replace(',', '.')
            holderAddition1 = holderAddition1.upper()
        
        if holderAddition2 !="" :
            holderAddition2 = holderAddition2.replace(',', '.')
            holderAddition2 = holderAddition2.upper()        

        def validateAndConvertToolDataEntry(toolDataEntry, toolDataEntryLabel, default="0", showError=True):
            if toolDataEntry is None or toolDataEntry == "":
                return default
            else:
                try:
                    test=float(toolDataEntry)
                    return toolDataEntry
                except ValueError:
                    if showError:
                        logging.error(f"Please check the {toolDataEntryLabel} possible errors\t-No letters or special characters allowed\t-Input is missing")
                        messagebox.showerror(toolDataEntryLabel, f"Bitte {toolDataEntryLabel} kontrollieren \n \n \n mögliche Fehler\n-Keine Buchstaben o. Sonderzeichen erlaubt\n-Eingabe fehlt")
                    return None
            
        def validateNullValueToolEntry(toolDataEntry, default="0"):
            if toolDataEntry is None or toolDataEntry == "":
                return default
            else:
                return toolDataEntry

        toolNumber = validateAndConvertToolDataEntry(toolNumber, "Wzg_Nummer", showError=True)
        if toolNumber is None:
            return
        
        toolName = validateNullValueToolEntry(toolName)
        if toolName is None:
            return

        toolRadius = validateAndConvertToolDataEntry(toolRadius, "Wzg_Radius", showError=True)
        if toolRadius is None:
            return

        toolEdgeRadius = validateAndConvertToolDataEntry(toolEdgeRadius, "Eckenradius")
        if toolEdgeRadius is None:
            return

        toolPointAngle = validateAndConvertToolDataEntry(toolPointAngle, "Spitzenwinkel")
        if toolPointAngle is None:
            return

        toolAngle = validateAndConvertToolDataEntry(toolAngle, "Eintauchwinkel")
        if toolAngle is None:
            return

        toolTeeth = validateAndConvertToolDataEntry(toolTeeth, "Schneiden")
        if toolTeeth is None:
            return

        cutLenght = validateAndConvertToolDataEntry(cutLenght, "Schnittlänge")
        if cutLenght is None:
            return

        toolOverhangLength = validateAndConvertToolDataEntry(toolOverhangLength, "Wzg_ausspannlänge")
        if toolOverhangLength is None:
            return

        toolLenght = validateAndConvertToolDataEntry(toolLenght, "Wzg_gesamtlänge")
        if toolLenght is None:
            return
        
        toolArticleNumber = validateNullValueToolEntry(toolArticleNumber)
        if toolArticleNumber is None:
            return
        
        holderAddition1 = validateNullValueToolEntry(holderAddition1)
        if holderAddition1 is None:
            return
        
        holderAddition2 = validateNullValueToolEntry(holderAddition2)
        if holderAddition2 is None:
            return
        
        tensioningSystem=validateNullValueToolEntry(tensioningSystem)
        if tensioningSystem is None:
            return
        
        if toolType==2:
                try:
                    # Attempt to convert the string to a number
                    test = float(toolPitch)

                except ValueError:
                    # If conversion fails, it's not a number
                    logging.error(f"Please check the pitch possible errors\t-No letters or special characters allowed\t-Input is missing")
                    messagebox.showerror("Steigung","Bitte Steigung kontrollieren \n \n \n mögliche Fehler\n-Keine Buchstaben o. Sonderzeichen erlaubt\n-Eingabe fehlt")
                    return
        else:
            toolPitch="0"



        placenameListe=[]
        # Search for the selected place in the data
        for dictionary in places:
            if dictionary["placename"]:
                placenameListe.append(dictionary["placename"])  




        if toolPlace is None or toolPlace == "":
            result = messagebox.askyesno("Tool_Lage", "Keinen Lageort. Möchten Sie fortfahren?")
            if not result:  # User clicked "No"
                return
            

        if toolPlace in placenameListe:
            # Search for the selected place in the data
            for dictionary in places:
                if dictionary["placename"] == toolPlace:
                    if dictionary["status"] == "place":
                        # Extract subplace names
                        subplace_names = [subplace["subplacename"] for subplace in dictionary.get("subplace", [])]
                        if subplace_names:
                            # Check if a value is selected in the subPlace ComboBox
                            self.selected_subplace = self.subPlace_comboBox.get() # Strip to remove any accidental whitespace
                            if self.selected_subplace == "":  # If no value is selected
                                logging.error("Please select a subplace")
                                messagebox.showerror("Unterplatz", "Bitte Unterplatz auswählen")
                                return False
                            else:
                                toolPlace = toolPlace + " - " + self.selected_subplace
                        else:
                            logging.info("No subplaces available.")


        toolRemarkFilePath = os.path.join(paths["toolsremark"], f"{iDCode}.txt")
        toolRemarkEntry = self.toolRemarkEntry.get("1.0", "end-1c")  # Get text content from the text field
        with open(toolRemarkFilePath, "w") as file:
            file.write(toolRemarkEntry)

        updated = False



        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        # Ensure that the root window is created only once
        root = tk._default_root
        if not root:
            root = tk.Toplevel()
            root.withdraw()  # Hide the root window


        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        conn.close()

        updated = False

        # Iterate through the rows to find and update the matching row
        for i in range(len(currentToolsData)):
            if len(currentToolsData[i]) > 2 and currentToolsData[i][0] == iDCode:
                # Generate paths for tool and holder images
                toolImageName = toolArticleNumber[:6]
                holderImageName = holderArticleNumber[:6]
                toolImagePath = paths["compimgs"] + toolImageName + ".jpg"
                holderImagePath = paths["compimgs"] + holderImageName + ".jpg"

                # Check if the tool and holder can be connected
                if not self.connectToolWithHolder(root, holderArticleNumber, toolArticleNumber, toolImagePath, holderImagePath):
                    return False

                # Open the master CSV file to find the correct 'shouldLenght'
                with open(paths["mastercsv"], "r") as file:
                    masterDataCsv = csv.reader(file, delimiter=";")
                    shouldLenght = "0"
                    for row in masterDataCsv:
                        if row[1] == toolName:
                            shouldLenght = row[22]
                            break

                # Prepare the updated row data
                updatedRow = [
                    iDCode, toolNumber, toolName, toolType, toolRadius, toolEdgeRadius, toolPointAngle, toolAngle,
                    toolTeeth, cutLenght, toolOverhangLength, toolLenght, shouldLenght, toolPitch,
                    toolArticleNumber, holderArticleNumber, holderAddition1, holderAddition2, tensioningSystem, toolPlace
                ]
                # Connect to the MTMDB.db SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
                # Update the database with the new values
                cur.execute("""
                    UPDATE currentTools
                    SET toolNummer = ?, toolName = ?, toolTyp = ?, toolRadius = ?, toolEckenRadius = ?, 
                        toolSpitzenWinkel = ?, toolEintauchWinkel = ?, toolSchneiden = ?, toolSchnittLaenge = ?, 
                        toolAusspannlaenge = ?, toolIstLaenge = ?, toolSollLaenge = ?, toolSteigung = ?, komponent1 = ?, 
                        komponent2 = ?, komponent3 = ?, komponent4 = ?, komponent5 = ?, platz = ?
                    WHERE idCode = ?
                """, updatedRow[1:] + [iDCode])
                conn.commit()  # Commit the transaction first
                cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                conn.close()

                updated = True
                
                break

    # Show error if the row was not found
        if not updated:
            logging.error("Row not found")
            messagebox.showerror("Fehler", "Diese Aufnahme konnte nicht gefunden werden.")
            return False



        # plcDataRow=[]
        #frozzen
        # with open(paths["TNC640_Daten"], "r", newline='') as file:
        #     tnc640DataFile = csv.reader(file, delimiter=';')
        #     for row in tnc640DataFile:
        #         if updatedRow[0] in row:
        #             plcDataRow=row
        #             found = True
        #             print("PLC Date found")
        #             break
        #         else:
        #             found=False
                    


        # try:
        #     # Connect to the SQLite database
        #     conn = sqlite3.connect(paths["MTMDB"])
        #     cursor = conn.cursor()

        #     # Query the database to find the row
        #     cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (updatedRow[0],))
        #     plcDataRow = cursor.fetchone()

        #     if plcDataRow:
        #         found = True
        #         print("PLC Daten gefunden:", plcDataRow)
        #     else:
        #         found = False
        #         print("PLC Daten nicht gefunden.")

        # except sqlite3.OperationalError as e:
        #     messagebox.showerror("Fehler", f"Fehler beim Zugriff auf die Datenbank: {str(e)}")
        #     return
        # except Exception as e:
        #     messagebox.showerror("Fehler", f"Fehler beim Lesen der Datenbank: {str(e)}")
        #     return
        # finally:
        #     # Close the database connection
        #     conn.close()



        #frozzen
        # if found==False:
        #     cfgInput=holderArticleNumber.replace(" ","_")
        #     cfgInput=cfgInput+".CFG"
        #     KLValue=float(toolLenght)+10
        #     KLValue=int(KLValue)
        #     KLValue=str(KLValue)
        #     plcDataRow=[updatedRow[0],"0","0","0","0","0","0","0","0","0.5","0.5","-1","80",KLValue,"0","0","0","0","0","0",cfgInput,"%00000000"]

        #     with open(paths["TNC640_Daten"], 'a', newline='') as file:
        #         writer = csv.writer(file, delimiter=';')
        #         writer.writerow(plcDataRow)
        #         print("No PLC Date found\nDefault PLC created")
        # else:
        #     print("Error no PLC data created")

        # self.addToolRowInToolTableBackup(updatedRow,plcDataRow)

        # messagebox.showinfo("Aktualisiert", "Die Aufnahme wurde erfolgreich aktualisiert.")
        # # self.goHome()
        # return
    

        # if not found:
        #     cfgInput = holderArticleNumber.replace(" ", "_") + ".CFG"
        #     KLValue = int(float(toolLenght) + 10)
        #     plcDataRow = [
        #         updatedRow[0], "0", "0", "0", "0", "0", "0", "0", "0", "0.5", "0.5",
        #         "-1", "80", str(KLValue), "0", "0", "0", "0", "0", "0", cfgInput, "%00000000"
        #     ]

        #     try:
        #         # Connect to the SQLite database
        #         conn = sqlite3.connect(paths["MTMDB"])
        #         cursor = conn.cursor()

        #         # Insert the default PLC data
        #         insert_query = '''
        #         INSERT INTO tnc640Data (
        #             CODE, NMAX, TIME1, TIME2, CURTIME, LOFFS, ROFFS, LTOL, RTOL,
        #             LBREAK, RBREAK, DIRECT, Max_Durchmesser, Max_Laenge, P2, BC,
        #             IKZ, ML, MLR, AM, KINEMATIC, PLC
        #         ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        #         '''
        #         cursor.execute(insert_query, plcDataRow)
        #         conn.commit()  # Commit the transaction first
        #         cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
        #         print("Kein PLC Daten gefunden\nStandard PLC erstellt.")
        #         # Close the database connection
        #         conn.close()
        #     except sqlite3.OperationalError as e:
        #         print(f"Fehler beim Zugriff auf die Datenbank: {e}")
        #         messagebox.showerror("Fehler", f"Fehler beim Zugriff auf die Datenbank: {e}")
        #         return
        #     except Exception as e:
        #         print(f"Fehler beim Erstellen der PLC-Daten: {e}")
        #         messagebox.showerror("Fehler1", f"Fehler beim Erstellen der PLC-Daten: {e}")
        #         return
        #     finally:
        #         # Close the database connection
        #         conn.close()
        # else:
        #     print("gefunden, Kein PLC-Datensatz erstellt")

        # # Add the PLC data to the backup tool table
        # self.addToolRowInToolTableBackup(updatedRow, plcDataRow)

        # Notify the user about the successful update
        # messagebox.showinfo("Aktualisiert", "Die Aufnahme wurde erfolgreich aktualisiert.")
        # self.goHome()
        return True




    def messwerte_uebernehmen(self):

        # Parse the XML file
        tree = ET.parse(paths["haimer"])
        root = tree.getroot()
        # Extract values
        self.schneide = int(root.find('.//CountEdges').text)
        self.z_wert = float(root.find('.//ZNominal').text)
        self.x_wert = float(root.find('.//XNominal').text)
        print(f"CountEdges (schneide): {self.schneide}")
        print(f"ZNominal (z_wert): {self.z_wert}")
        print(f"XNominal (x_wert): {self.x_wert}")
        self.dataToolEntries[8].set(self.schneide)
        self.dataToolEntries[11].set(self.z_wert)
        self.dataToolEntries[4].set(self.x_wert)

    def connectToolWithHolder(self, Frame, holderArticleNumber, toolArticleNumber, toolImagePath, holderImagePath):
        dialog = Toplevel(Frame)
        dialog.title("Bestätigung")

        message = Label(dialog, text=f"Wollen Sie das Wz ArtNr.{toolArticleNumber} mit der Aufnahme ArtNr. {holderArticleNumber} verheiraten?")
        message.pack(pady=10)

        try:
            # Load and display the first image
            img1 = Image.open(toolImagePath)
            img1 = img1.resize((100, 100), Image.LANCZOS)
            img1_photo = ImageTk.PhotoImage(img1)
            img1_label = Label(dialog, image=img1_photo)
            img1_label.image = img1_photo  # Keep a reference to avoid garbage collection
            img1_label.pack(side="left", padx=10)
        except FileNotFoundError:
            # If image 1 is not found, show a default image or handle as desired
            default_img1 = Image.open(paths["compimgs"] +"NOTFOUND.png")
            default_img1 = default_img1.resize((100, 100), Image.LANCZOS)
            img1_photo = ImageTk.PhotoImage(default_img1)
            img1_label = Label(dialog, image=img1_photo)
            img1_label.image = img1_photo
            img1_label.pack(side="left", padx=10)

        try:
            # Load and display the second image
            img2 = Image.open(holderImagePath)
            img2 = img2.resize((100, 100), Image.LANCZOS)
            img2_photo = ImageTk.PhotoImage(img2)
            img2_label = Label(dialog, image=img2_photo)
            img2_label.image = img2_photo  # Keep a reference to avoid garbage collection
            img2_label.pack(side="right", padx=10)
        except FileNotFoundError:
            # If image 2 is not found, show a default image or handle as desired
            default_img2 = Image.open(paths["compimgs"] +"NOTFOUND.png")
            default_img2 = default_img2.resize((100, 100), Image.LANCZOS)
            img2_photo = ImageTk.PhotoImage(default_img2)
            img2_label = Label(dialog, image=img2_photo)
            img2_label.image = img2_photo
            img2_label.pack(side="right", padx=10)
        # Confirmation buttons
        def on_yes():
            dialog.destroy()
            Frame.update_idletasks()  # Ensure any pending operations are completed
            Frame.result = True
            

        def on_no():
            dialog.destroy()
            Frame.update_idletasks()  # Ensure any pending operations are completed
            Frame.result = False

        yes_button = Button(dialog, text="Verheiraten", command=on_yes)
        yes_button.pack(side="left", padx=10, pady=10)

        no_button = Button(dialog, text="Abbrechen", command=on_no)
        no_button.pack(side="right", padx=10, pady=10)

        dialog.transient(Frame)  # Set to be on top of the main window
        dialog.grab_set()  # Ensure all input goes to the dialog
        Frame.wait_window(dialog)  # Block until the dialog is destroyed
        return Frame.result


######################################################################################################

    def moveToolFunction(self):
        def create_treeview(parent, columns, heading, height=10):
            tree = ttk.Treeview(parent, columns=columns, show='headings', height=height)
            for col in columns:
                tree.heading(col, text=col)
            return tree

        def populate_treeview(tree, data):
            for item in data:
                tree.insert('', 'end', values=item)

        def move_selected_item(source_tree, target_tree):
            search_entry.delete(0, 'end')
            selected_items = source_tree.selection()
            if selected_items:
                for item in selected_items:
                    item_values = source_tree.item(item, 'values')
                    target_tree.insert('', 'end', values=item_values)
                    source_tree.delete(item)

        def search_treeview(tree, search_value):
            tree.selection_remove(tree.selection())
            items = tree.get_children()
            global search_index
            if search_value:
                for i in range(search_index, len(items)):
                    item = items[i]
                    item_values = tree.item(item, 'values')
                    if any(search_value.lower() in str(value).lower() for value in item_values):
                        tree.selection_add(item)
                        tree.see(item)
                        search_index = i + 1
                        return
                search_index = 0
                search_treeview(tree, search_value)

        def print_rows(tree):
            for item in tree.get_children():
                item_values = tree.item(item, 'values')
                if len(item_values) > 1:
                    print(item_values[1])

        global search_index
        search_index = 0

        self.clearFrame()
        self.centerFrame = tk.Frame(self.mainFrame, bg=winconfig["bgcolor"])
        self.centerFrame.place(relx=0.5, rely=0.5, anchor=tk.CENTER)
        lager=[]

        for dictionary in places :

            if dictionary["placename"]!="ALL":
                lager.append(dictionary["placename"])

        # with open(places["place"], 'r') as json_file:
        #     lageort_data = json.load(json_file)
        #     maschine = [dictionary["befehl"] for dictionary in lageort_data if dictionary["befehl"]]

        columns = ("QR Code", "Wzg Name", "Lageort")

        left_frame = tk.Frame(self.centerFrame)
        right_frame = tk.Frame(self.centerFrame)

        treeview_height = 10

        left_treeview = create_treeview(left_frame, columns, "Left Treeview", height=treeview_height)
        right_treeview = create_treeview(right_frame, columns, "Right Treeview", height=treeview_height)

        left_scrollbar_y = ttk.Scrollbar(left_frame, orient='vertical', command=left_treeview.yview)
        left_treeview.configure(yscrollcommand=left_scrollbar_y.set)

        right_scrollbar_y = ttk.Scrollbar(right_frame, orient='vertical', command=right_treeview.yview)
        right_treeview.configure(yscrollcommand=right_scrollbar_y.set)

        search_frame = tk.Frame(self.centerFrame, bg=winconfig["bgcolor"])

        # search_label = tk.Label(search_frame, text="Search:")
        search_entry = tk.Entry(search_frame)
        search_button = tk.Button(search_frame, text="Search", command=lambda: search_treeview(left_treeview, search_entry.get()),font=(winconfig["fonttype"], winconfig["fontsize"]-4,"bold"),bg=winconfig["fontcolor"])
        search_entry.focus()

        search_entry.bind('<Return>', lambda event: (search_button.invoke(), button1.invoke()))


        button_frame = tk.Frame(self.centerFrame, bg=winconfig["bgcolor"])

        button1 = tk.Button(button_frame, text="> > >", command=lambda: move_selected_item(left_treeview, right_treeview),font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])
        button2 = tk.Button(button_frame, text="< < <", command=lambda: move_selected_item(right_treeview, left_treeview),font=(winconfig["fonttype"], winconfig["fontsize"],"bold"),bg=winconfig["fontcolor"])

        below_right_buttons_frame = tk.Frame(self.centerFrame, bg=winconfig["bgcolor"])

        button3 = tk.Button(below_right_buttons_frame, text="Änderung übernehmen", command=self.moveToolSubPlaceValidate,font=(winconfig["fonttype"], winconfig["fontsize"]-4,"bold"),bg=winconfig["fontcolor"])
        button4 = tk.Button(below_right_buttons_frame, text="Abbrechen", command=self.goHome,font=(winconfig["fonttype"], winconfig["fontsize"]-4,"bold"),bg=winconfig["fontcolor"])

        # search_label.grid(row=0, column=0, padx=5, pady=5, sticky='w')
        search_entry.grid(row=0, column=1, padx=5, pady=5, sticky='ew')
        search_button.grid(row=0, column=2, padx=5, pady=5)

        left_treeview.grid(row=0, column=0, sticky='nsew')
        left_scrollbar_y.grid(row=0, column=1, sticky='ns')

        right_treeview.grid(row=1, column=0, sticky='nsew')
        right_scrollbar_y.grid(row=1, column=1, sticky='ns')

        left_frame.grid_rowconfigure(0, weight=1)
        left_frame.grid_columnconfigure(0, weight=1)
        right_frame.grid_rowconfigure(1, weight=1)
        right_frame.grid_columnconfigure(0, weight=1)

        search_frame.grid(row=0, column=0, padx=10, pady=5, sticky='ew')
        left_frame.grid(row=1, column=0, padx=10, pady=10, sticky='nsew')
        button_frame.grid(row=1, column=1, padx=5, pady=5)
        right_frame.grid(row=1, column=2, padx=10, pady=10, sticky='nsew')
        below_right_buttons_frame.grid(row=2, column=2, padx=10, pady=5)

        button1.grid(row=0, column=0, padx=2, pady=2)
        button2.grid(row=1, column=0, padx=2, pady=2)

        button3.grid(row=0, column=0, padx=2, pady=2)
        button4.grid(row=0, column=1, padx=2, pady=2)


        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()

        showenRowInLeftTreeView=[]

        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()

        # Close the database connection
        conn.close()
        for treeview_left in currentToolsData:
            row=[treeview_left[0],treeview_left[2],treeview_left[19]]
            showenRowInLeftTreeView.append(row)

        sample_data_right = []

        populate_treeview(left_treeview, showenRowInLeftTreeView)
        populate_treeview(right_treeview, sample_data_right)

        self.centerFrame.columnconfigure(0, weight=1)
        self.centerFrame.columnconfigure(1, weight=0)
        self.centerFrame.columnconfigure(2, weight=1)
        self.centerFrame.rowconfigure(1, weight=1)

        left_treeview.configure(selectmode='extended')
        right_treeview.configure(selectmode='extended')

        # Create a frame for the combobox
        combobox_frame = tk.Frame(right_frame, bg=winconfig["bgcolor"])
        combobox_frame.grid(row=0, column=0, padx=5, pady=5, sticky='ew')
        self.combobox_frame = combobox_frame

        # Create and populate the combobox
        combobox_label = tk.Label(combobox_frame, text="Maschine:",font=(winconfig["fonttype"], winconfig["fontsize"]-4,"bold"),bg=winconfig["fontcolor"])
        combobox_label.grid(row=0, column=0, padx=5, pady=5, sticky='w')
        
        # maschine_combobox = ttk.Combobox(combobox_frame, values=lager)
        # maschine_combobox.grid(row=0, column=1, padx=5, pady=5, sticky='ew')
        # Create a StringVar and the main Combobox
        maschine_var = tk.StringVar()
        maschine_combobox = ttk.Combobox(combobox_frame, textvariable=maschine_var)
        maschine_combobox["values"] = lager
        maschine_combobox.grid(row=0, column=1, padx=5, pady=5)
        # subPlace_comboBox = ttk.Combobox(combobox_frame, values=self.subPlaceComboBoxListValues,state="disabled")
        # subPlace_comboBox.grid(row=0, column=2, padx=5, pady=5, sticky='ew')

        self.maschine_combobox = maschine_combobox  # Reference to the combobox
        
        # Create a global reference to the subplace Combobox
        self.subPlace_comboBox = None

        # Function to handle updates
        def showSubPlaceComboBox(event=None):
            # Get the current value of the combobox
            maschineComboBoxValue = maschine_var.get()

            # Destroy the previous subplace Combobox if it exists
            if self.subPlace_comboBox:
                self.subPlace_comboBox.destroy()
                self.subPlace_comboBox = None

            # Search for the selected place in the data
            for dictionary in places:
                if dictionary["placename"] == maschineComboBoxValue:
                    if dictionary["status"] == "place":
                        # Extract subplace names
                        subplace_names = [subplace["subplacename"] for subplace in dictionary.get("subplace", [])]
                        if subplace_names:
                            # Create a new subplace Combobox
                            self.subPlace_comboBox = ttk.Combobox(combobox_frame, values=subplace_names)
                            self.subPlace_comboBox.grid(row=0, column=2, padx=5, pady=5, sticky="ew")
                        else:
                            print("No subplaces available.")
                    elif dictionary["status"] == "maschine":
                        print("Selected item is a machine. Subplace Combobox removed.")
                    break
            else:
                print("No matching place found.")

        # Bind user selection and trace variable changes
        maschine_combobox.bind("<<ComboboxSelected>>", showSubPlaceComboBox)
        maschine_var.trace_add("write", lambda *args: showSubPlaceComboBox())

        self.right_treeview = right_treeview  # Reference to the right_treeview

        # Configure the right_frame to expand with the Treeview
        right_frame.grid_rowconfigure(1, weight=1)
        right_frame.grid_columnconfigure(0, weight=1)
        right_frame.grid_columnconfigure(1, weight=0)

    def moveToolSubPlaceValidate(self):
                # Search for the selected place in the data

        for dictionary in places:
            if dictionary["placename"] == self.maschine_combobox.get():
                if dictionary["status"] == "place":
                    # Extract subplace names
                    subplace_names = [subplace["subplacename"] for subplace in dictionary.get("subplace", [])]

                    if subplace_names:
                        if self.subPlace_comboBox.get() != "":
                            self.platzieren()
                            return
                        else:
                            logging.error("Bitte wählen Sie einen Unter Platz aus!")
                            messagebox.showerror("Error0", "Bitte wählen Sie einen Unterplatz aus!")
                            return
                    else:
                        self.platzieren()
                elif dictionary["status"] == "machine":
                    self.platzieren()
                break
            else:
                logging.error("Dieser Platz wurde nicht gefunden.")




    def platzieren(self):
        try:
            combobox_value = self.maschine_combobox.get()

        except:
            combobox_value = self.maschine_combobox
        try:
            subplace=self.subPlace_comboBox.get()

        except:
            subplace=False

        if not combobox_value:
            logging.error("Bitte wählen Sie einen Platz aus!")
            messagebox.showerror("Error", "bitte wählen Sie einen Platz aus!")
            return
        
        try:
            # Get the values in the first column from the second Treeview
            treeview_items = self.right_treeview.get_children()
            self.movingTools = [self.right_treeview.item(item, 'values')[0] for item in treeview_items]

        except:
            self.movingTools=self.movingTools


        if self.validateToolPlacement(combobox_value,subplace) == "moveToPlace":
            print("Move to Place")
            self.moveToolToPlaceFunction(combobox_value,subplace)

        elif self.validateToolPlacement(combobox_value,subplace) == "moveToMachine":
            print("Move to Machine")
            if self.machineStatus(combobox_value):
                print("Connection found")
            else:
                logging.error(f"{combobox_value} is offline")
                messagebox.showerror("No Connection",f"{combobox_value} is offline")    
                return False
            
            if self.moveToolToMachineFunction(combobox_value):
                print("Move to Machine Fuction success")
            else:
                logging.error("Move to Machine Fuction failed")
                messagebox.showerror("Error", "Move to Machine Fuction failed")
                print("Move to Machine Fuction failed")
                return False
        else:
            logging.error("Platz ist kein Maschine oder Lagerplatz")
            messagebox.showerror("Error", "Platz ist kein Maschine oder Lagerplatz")
            return False
            
    def validateToolPlacement(self, combobox_value,subplace):
        moveToPlace=False
        moveToMachine=False 


        for dir in places:
            if dir["placename"] == combobox_value:    
                if dir["status"] == "place":
                    print(f"here ist subplaceeee {dir["subplace"]}")
                    if dir.get("subplace"):
                        subplace_list = [subplace["subplacename"] for subplace in dir.get("subplace", [])]  # Get subplace list, default to empty list
                        if subplace in subplace_list:
                            moveToPlace = "moveToPlace"
                            return moveToPlace
                        else:
                            logging.error("bitte wählen Sie einen Unter Platz aus!")
                            messagebox.showerror("Error11", "bitte wählen Sie einen Unterplatz aus!")
                            return False
                    else:
                        moveToPlace ="moveToPlace"
                        return moveToPlace

                elif dir["status"] =="machine":
                    moveToMachine = "moveToMachine"
                    return  moveToMachine
                else:
                    print("Error, Platz Status unbekannt")  

    def moveToolToPlaceFunction(self,combobox_value,subplace):
        found = False
        platzierte_rows=[]
        nicht_platzierte_rows=[]
        print(f"here is combobox_value {combobox_value}")   
        print(f"here is subplace {subplace}")   
        print(f"here is self.movingTools {self.movingTools}")   
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()
        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        # Close the database connection
        conn.close()
        # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
        for movingTool in self.movingTools:
            for row in currentToolsData:
                print(f"here is row[0] {row[0]}")   
                
                if row[0] == movingTool and subplace != False:
                    print("passt")
                    row = list(row)  # Convert tuple to list to modify
                    row[19] = combobox_value+" - "+subplace  # Update the 'platz' field
                    platzierte_rows.append(row)
                    found = True
                    break
                elif row[0] == movingTool and subplace == False:
                    row = list(row)  # Convert tuple to list to modify
                    row[19] = combobox_value  # Update the 'platz' field
                    platzierte_rows.append(row)
                    found = True
                    break
                else:
                    found=False
                    nicht_platzierte_rows.append(row[0])
                

            # Update the database if any matching rows were found
            if found:
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
                for row in platzierte_rows:
                    cur.execute("""
                        UPDATE currentTools
                        SET platz = ?
                        WHERE idCode = ?
                    """, (row[19], row[0]))
                # Commit the changes after all updates are done
                conn.commit()  # Commit the transaction first
                cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                # Ensure the connection is closed properly
                conn.close()
                logging.info(f"Das Werkzeug {movingTool} ist erfolgreich platziert - {self.userName}")
                messagebox.showinfo("Success", f"Das Werkzeug {movingTool} ist erfolgreich platziert")
                ende=True
            else:
                if nicht_platzierte_rows:
                    logging.error(f"Dieses Werkzeug {nicht_platzierte_rows} is nicht in der Datenbank")
                    messagebox.showerror("Error0", f"Dieses Werkzeug {nicht_platzierte_rows} is nicht in der Datenbank")
                    ende=False
        if ende==True:
            self.centerFrame.destroy()
            self.goHome()
            return 
        if ende==False:
            return

    def moveToolToMachineFunction(self,combobox_value):
        TNC640DATEN_Rows=[]

        # Process IP addresses
        for dictionary in places:
            if dictionary["placename"] == combobox_value and dictionary["status"]=="machine":
                ip_address = dictionary["link"]

                
                # Clean and validate IP address format
                parts = ip_address.split('.')
                
                if len(parts) == 4 and all(part.isdigit() for part in parts):
                    formatted_ip = f"[{ip_address}]"
                    TNC640DATEN_Rows.append(formatted_ip)  # Append formatted IP address
                else:
                    logging.error(f"Invalid IP address format: {ip_address}")
                    messagebox.showerror("Error", f"Invalid IP address format: {ip_address}")
                    return
        if self.preparePLCData():
            print("PLC Daten sind bereit")
        else:
            logging.error("Fehler beim Erstellen der PLC-Daten")
            messagebox.showerror("Error", "Fehler beim Erstellen der PLC-Daten")
            return False
        
        if self.prepareDNCData(formatted_ip):
            print("DNC Daten sind bereit")
        else:
            logging.error("DNC Daten sind NICHT bereit")
            messagebox.showerror("DNC Daten sind NICHT bereit")
            return False 
        if self.callDNCSchnittstelle():
            print("DNC Schnittstelle ist bereit")
        else:
            logging.error("DNC Schnittstelle ist NICHT bereit")
            messagebox.showerror("DNC Schnittstelle ist NICHT bereit")
            return False
        if self.changeDataInDatabase(combobox_value):
            print("Nach DNC Erfolg")
            return True
        else:
            logging.error("Nach DNC-SchnittStelle GESCHEITERT") 
            messagebox.showerror("Nach DNC-SchnittStelle GESCHEITERT")
            return False

         
     
        
    def preparePLCData(self):
        for treeViewValuesRow in self.movingTools:
            QRcode=treeViewValuesRow
            print(QRcode)
            try:
                # Connect to the MTMDB.db SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
                # Fetch all data from the currentTools table
                cur.execute("SELECT * FROM currentTools")
                currentToolsData = cur.fetchall()
                conn.close()
                for machineCsvRow in currentToolsData:
                    if machineCsvRow[0] == QRcode:
                        if machineCsvRow[14] != "" or machineCsvRow[14] != None:
                            cfgInput = machineCsvRow[15].replace(" ", "_") + ".CFG"

                            # Convert the tuple to a list to modify it
                            machineCsvRow = list(machineCsvRow)
                            machineCsvRow[15]=machineCsvRow[14]+" + "+machineCsvRow[15]
                            break
            except sqlite3.OperationalError as e:
                print(f"Fehler beim Zugriff auf die Datenbank: {e}")
                return False
            except Exception as e:
                print(f"Fehler: {str(e)}")
                return False
            finally:
                if conn:# Close the database connection
                    conn.close()
            try:
                # Connect to the SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cursor = conn.cursor()

                # Query the database for the matching QR code
                cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (QRcode,))
                plcDataRow = cursor.fetchone()
                conn.close()
                if plcDataRow:
                    # Join elements from machineCsvRow (excluding the first element) with tnc640Row (excluding the first element)
                    print(f"PLC-Daten für {QRcode} gefunden \u2713")
                    Standart=False
                    return True
                else:
                    print(f"Warnung, Keine PLC-Daten für {QRcode} gefunden")
                    Standart=True

                if  Standart:
                    # cfgInput = machineCsvRow[15].replace(" ", "_") + ".CFG"
                    KLValue = int(float(machineCsvRow[11]) + 10)
                    KRValue = int(float(machineCsvRow[4])*2)
                    if KRValue < 35:
                        KRValue = 35
                    elif 35<=KRValue<=80:
                        KRValue = 80
                    elif 80<KRValue<=125:
                        KRValue = 125
                    plcDataRow = [
                        QRcode, "0", "0", "0", "0", "0", "0", "0", "0", "0.5", "0.5",
                                "-1",  str(KRValue),str(KLValue),"0", "0", "0", "0", "0", "0", cfgInput, "0"
                    ]

                    try:
                        # Connect to the SQLite database
                        conn = sqlite3.connect(paths["MTMDB"])
                        cursor = conn.cursor()

                        # # Insert the default PLC data
                        # insert_query = '''
                        # INSERT INTO tnc640Data (
                        #     CODE, NMAX, TIME1, TIME2, CURTIME, LOFFS, ROFFS, LTOL, RTOL,
                        #     LBREAK, RBREAK, DIRECT, Max_Durchmesser, P2, BC,
                        #     IKZ, ML, MLR, AM, PLC
                        # ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                        # '''
                        # Insert the default PLC data
                        insert_query = '''
                        INSERT INTO tnc640Data (
                            CODE, NMAX, TIME1, TIME2, CURTIME, LOFFS, ROFFS, LTOL, RTOL,
                            LBREAK, RBREAK, DIRECT, Max_Durchmesser, Max_Laenge, P2, BC,
                            IKZ, ML, MLR, AM, KINEMATIC, PLC
                        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                        '''
                        cursor.execute(insert_query, plcDataRow,)
                        conn.commit()  # Commit the transaction first
                        cursor.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                        conn.close()
                        print(f"Standard PLC für {QRcode} erstellt.\u2713")
                        return True

                    except sqlite3.OperationalError as e:
                        print(f"Fehler beim Zugriff auf die Datenbank: {e}")
                        return False
                    except Exception as e:
                        print(f"Fehler3 beim Erstellen der PLC-Daten: {e}")
                        return False
                    finally:
                        if conn:# Close the database connection
                            conn.close()
            except sqlite3.OperationalError as e:
                print(f"Fehler beim Zugriff auf die Datenbank: {e}")
                return False
            except Exception as e:
                print(f"Fehler: {str(e)}")
                return False
            finally:
                if conn:# Close the database connection
                    conn.close()

    def prepareDNCData(self,formatted_ip):

        rowData=[]
        for treeViewValuesRow in self.movingTools:
            
            QRcode=treeViewValuesRow
            print(QRcode)
            print(f"self.movingTools {self.movingTools}")

            try:
                # Connect to the MTMDB.db SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cur = conn.cursor()
                # Fetch all data from the currentTools table
                cur.execute("SELECT * FROM currentTools")
                currentToolsData = cur.fetchall()
                conn.close()
                for machineCsvRow in currentToolsData:
                    if machineCsvRow[0] == QRcode:
                        if machineCsvRow[14] != "" or machineCsvRow[14] != None:
                            # Convert the tuple to a list to modify it
                            machineCsvRow = list(machineCsvRow)
                            machineCsvRow[15]=machineCsvRow[14]+" + "+machineCsvRow[15]
                            changedRow=machineCsvRow
                            break
            except sqlite3.OperationalError as e:
                print(f"Fehler beim Zugriff auf die Datenbank: {e}")
                return False
            except Exception as e:
                print(f"Fehler: {str(e)}")
                return False
            finally:
                if conn:# Close the database connection
                    conn.close()


            try :   
                # Connect to the SQLite database
                conn = sqlite3.connect(paths["MTMDB"])
                cursor = conn.cursor()

                # Query the database for the matching QR code
                cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (QRcode,))
                tnc640Row = cursor.fetchone()
                conn.close()
                print(f"here is the wrrong row {changedRow}")
                combined_row = tuple(changedRow[1:]) + tnc640Row[1:]
                rowData.append(f"[{','.join(map(str, combined_row))}]")
                print(f"Standard PLC für {QRcode} erstellt.\u2713")
                self.rowData=rowData
                
            except sqlite3.OperationalError as e:
                print(f"Fehler beim Zugriff auf die Datenbank: {e}")
                return False
            except Exception as e:
                print(f"Fehler2 beim Erstellen der PLC-Daten: {e}")
                return False
            finally:
                if conn:# Close the database connection
                    conn.close()


        # Write the combined formatted rows to the DNC_INPUT file
        try:
            with open(paths["DNCINPUT"] + "DNCinput.txt", mode='w', newline='') as file:
                for row in self.rowData:
                    file.write(f"{formatted_ip}{row}\n")  # Join list elements with newlines and write to the file
                return True
        except Exception as e:
            logging.error(f"An error occurred while writing to DNC_INPUT: {e}")
            messagebox.showerror("Error3", f"An error occurred while writing to DNC_INPUT: {e}")
            return False

    def callDNCSchnittstelle(self):
        import subprocess

        program_path = paths["DNCSchnittstelle"]
        # # Running the program
        # try:
        #     result = subprocess.run([program_path], check=True)
        #     print(f"Program executed successfully with return code: {result.returncode}")
        #     return True
        # except subprocess.CalledProcessError as e:
        #     print(f"Program failed with return code: {e.returncode}")
        #     messagebox.showerror("Error", f"An error occurred while running the program: {e}")
        #     return False
        # except FileNotFoundError:
        #     print("The specified program was not found.")
        #     messagebox.showerror("Error", "The specified program was not found.")
        #     return False

        # program_path = paths.get("DNCSchnittstelle")  # Ensure path exists
        # if not program_path:
        #     print("Error: DNCSchnittstelle path is missing.")
        #     messagebox.showerror("Error", "DNCSchnittstelle path is missing.")
        #     return False
        # try:
        #     # Start process without waiting
        #     process = subprocess.Popen(
        #         [program_path],
        #         stdout=subprocess.PIPE,
        #         stderr=subprocess.PIPE,
        #         text=True
        #     )

        #     # Wait for process to finish (max 10 seconds)
        #     timeout = 10
        #     start_time = time.time()

        #     while process.poll() is None:  # Process still running
        #         if time.time() - start_time > timeout:
        #             print("Program is stuck, likely waiting for user input.")
        #             messagebox.showerror("Error", "Program is not responding.")
        #             process.terminate()  # Force close
        #             return False
        #         time.sleep(0.5)  # Wait before checking again

        #     # Capture stdout & stderr after completion
        #     stdout_output, stderr_output = process.communicate()
        #     stdout_output = stdout_output.strip() if stdout_output else ""
        #     stderr_output = stderr_output.strip() if stderr_output else ""

        #     # Check for errors
        #     if (
        #         process.returncode != 0
        #         or "system.exception" in stdout_output.lower()
        #         or "system.exception" in stderr_output.lower()
        #         or "error" in stdout_output.lower()
        #         or "error" in stderr_output.lower()
        #         or stderr_output  # If stderr has any content, assume failure
        #     ):
        #         print(f"Program failed:\nSTDOUT:\n{stdout_output}\nSTDERR:\n{stderr_output}")
        #         messagebox.showerror("Error", f"Program failed:\n{stdout_output}\n{stderr_output}")
        #         return False

        #     print(f"Program executed successfully:\n{stdout_output}")
        #     return True

        # except FileNotFoundError:
        #     print("Error: The specified program was not found.")
        #     messagebox.showerror("Error", "The specified program was not found.")
        #     return False
        # except Exception as e:
        #     print(f"Unexpected error: {e}")
        #     messagebox.showerror("Error", f"Unexpected error:\n{str(e)}")
        #     return False

        import subprocess

        program_path = paths["DNCSchnittstelle"]
        # Running the program
        try:
            result = subprocess.run([program_path], check=True)
            # print(f"Daten zu {toolPlace} gescheckt \u2713")
            return True
        except subprocess.CalledProcessError as e:
            print(f"Program failed with return code: {e.returncode}")
            return False
        except FileNotFoundError:
            print("The specified program was not found.")
            return False
        
    def changeDataInDatabase(self,combobox_value):

        try:
            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()

            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()
            conn.close()

            platzierte_rows = []
            found = False

            # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
            for row in currentToolsData:
                if row[0] in self.movingTools:
                    row = list(row)  # Convert tuple to list to modify
                    row[19] = combobox_value  # Update the 'platz' field
                    platzierte_rows.append(row)
                    found = True
                    print(f"found {found}")

            # Update the database if any matching rows were found
            if found:
                print(f"if found")
                # conn = None
                import time

                try:
                    start_time = time.time()
                    conn = sqlite3.connect(paths["MTMDB"])
                    conn.execute("PRAGMA busy_timeout = 200;")
                    cur = conn.cursor()
                    print("db opened", time.time() - start_time)
                    for row in platzierte_rows:
                        cur.execute("""
                            UPDATE currentTools
                            SET platz = ?
                            WHERE idCode = ?
                        """, (row[19], row[0]))
                    print("db commit", time.time() - start_time)
                    conn.commit()  # Commit the transaction first
                    cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                
                except Exception as e:
                    print(f"Error after {time.time() - start_time} seconds: {e}")
                    return False	
                finally:
                    if conn:
                        conn.close()
                        print("db closed", time.time() - start_time)
        except Exception as e:
            logging.error(f"An error occurred: {e}")
            messagebox.showerror("Erro4", f"An error occurred: {e}")
            return False
    
        for i in platzierte_rows:
            if i[1] != None:  # Check if the row has a non-None value
                try:
                    # Connect to the SQLite database
                    conn = sqlite3.connect(paths["MTMDB"])
                    cursor = conn.cursor()

                    # Query the database for the matching PLC data
                    cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (i[0],))
                    PlcDataRow = cursor.fetchone()
                    conn.close()

                    if PlcDataRow:
                        # Pass the current row and fetched PLC data to the addToolRowInToolTableBackup method
                        success = self.addToolRowInToolTableBackup(i, PlcDataRow)
                        print (f"this is sucess {success}")
                        if success:
                            logging.info(f"Das Werkzeug {i[2]} ist erfolgreich platziert\nUND\nsie wurde in toolTableBackup eingetragen - {self.userName}")
                            messagebox.showinfo(
                                "Success",
                                f"Das Werkzeug {i[2]} ist erfolgreich platziert\nUND\nsie wurde in toolTableBackup eingetragen"
                            )
                            
                        else:
                            logging.error(f"Das Werkzeug {i[2]} konnte nicht platziert werden!!")
                            messagebox.showerror(
                                "Error",
                                f"Das Werkzeug {i[2]} konnte nicht platziert werden!!\nMÖGLICHE FEHLER\n-falsche Daten\nOder\nfehlende PLC-TT-Daten"
                            )
                    else:
                        logging.error(f"Keine passenden PLC-Daten für CODE {i[0]} gefunden.")
                        # No matching PLC data found for the CODE
                        messagebox.showerror(f"Keine passenden PLC-Daten für CODE {i[0]} gefunden.")
                        return False
                except sqlite3.OperationalError as e:
                    logging.error(f"Fehler beim Zugriff auf die Datenbank: {e}")
                    messagebox.showerror("Fehler", f"Fehler beim Zugriff auf die Datenbank: {e}")
                    return False
                except Exception as e:
                    logging.error(f"Fehler: {str(e)}")
                    messagebox.showerror("Fehler", f"Fehler: {str(e)}")
                    return False    
                finally:
                    if conn:
                        conn.close()
            else:
                continue
        
        self.centerFrame.destroy()
        self.goHome()
        return True

    def addToolRowInToolTableBackup(self, toolDataEntriesRow,plcDataRow):
        def format_column(value, length):
            """Format the value to a fixed length by padding with spaces on the right."""
            return str(value).ljust(length)[:length]

        def process_line(line, column_lengths, row):
            """Replace old_name with new_name in a line, preserving column widths."""
            # Extract the existing columns
            columns = []
            start = 0
            for length in column_lengths:
                end = start + length
                columns.append(line[start:end].strip())
                start = end

            # Find and replace the old name
            if int(columns[0]) == int(row[0]):
                columns[1] = row[1]
                columns[2] = row[2]
                columns[3] = row[3]
                columns[4] = row[4]
                columns[12] = row[12]
                columns[13] = row[13]
                columns[14] = row[14]
                columns[16] = row[16]
                columns[17] = row[17]
                columns[18] = row[18]
                columns[30] = row[30]
                columns[34] = row[34] 
                columns[41] = row[41]
                columns[44] = row[44]
                columns[48] = row[48]

                print(f"Updated line: {columns}")

            # Format the columns according to the fixed lengths
            new_line = ''.join(format_column(col, length) for col, length in zip(columns, column_lengths))
            return new_line

        def create_new_line(row, column_lengths):
            """Create a new line with specified values."""
            # Ensure columns list matches the length of column_lengths
            columns = ['' for _ in column_lengths]
            
            # Iterate over each element in row and assign it to the corresponding column
            for i in range(len(row)):
                if i < len(columns):
                    columns[i] = row[i]
            
            # Create the new line by formatting each column with its specified length
            new_line = ''.join(format_column(col, length) for col, length in zip(columns, column_lengths))
            
            print(f"Created new line: {new_line}")
            return new_line
        
        def update_file(filename, row):
            # Define the column lengths as provided
            column_lengths = [
                8, 32, 12, 12, 12, 10, 10, 10, 3, 8, 6, 6, 9, 9, 32, 10, 12, 8, 4, 7, 7, 7, 7, 12, 12, 7, 7, 10, 8, 7, 8, 20, 
                5, 8, 10, 7, 7, 7, 7, 7, 7, 7, 10, 4, 8, 9, 10, 10, 20, 16, 8, 30, 32, 20
            ]

            # Create a temporary file
            temp_filename = filename + '.tmp'

            try:
                with open(filename, 'r') as file:
                    lines = file.readlines()
            except FileNotFoundError:
                print(f"No toolTable Backup file for {toolDataEntriesRow[19]} found")
                return False

            found = False
            nearest_index = None

            for i, line in enumerate(lines):
                line_start = line[:8].strip()
                if line_start.isdigit():
                    line_nr = int(line_start)
                    if line_nr == int(row[0]):
                        found = True
                        lines[i] = process_line(line, column_lengths, row) + '\n'
                        break
                    if line_nr < int(row[0]):
                        nearest_index = i

            if not found:
                new_line = create_new_line(row, column_lengths)
                if nearest_index is not None:
                    print(f"Inserting new line after index {nearest_index} (line_nr: {lines[nearest_index][:8].strip()})")
                    lines.insert(nearest_index + 1, new_line + '\n')
                else:
                    print("Appending new line at the end")
                    lines.append(new_line + '\n')

            with open(temp_filename, 'w') as temp_file:
                temp_file.writelines(lines)

            # Replace the original file with the modified temporary file
            os.replace(temp_filename, filename)

        t_nr = toolDataEntriesRow[1]
        t_name = toolDataEntriesRow[2]
        t_l = f"+{toolDataEntriesRow[11]}"
        t_r = f"+{toolDataEntriesRow[4]}"
        t_r2 = f"+{toolDataEntriesRow[5]}"
        t_typ = toolDataEntriesRow[3]
        t_doc = f"{toolDataEntriesRow[14]} + {toolDataEntriesRow[15]}"
        t_lcut = f"+{toolDataEntriesRow[9]}"
        t_cut = toolDataEntriesRow[8]
        t_angle = f"+{toolDataEntriesRow[7]}"
        t_tangle = f"+{toolDataEntriesRow[6]}"
        t_nmax=plcDataRow[1]
        t_time1=plcDataRow[2]
        t_time2=plcDataRow[3]
        t_curtime=plcDataRow[4]
        t_loffs=f"+{plcDataRow[5]}"
        t_roffs=f"+{plcDataRow[6]}"
        t_ltol=plcDataRow[7]
        t_rtol=plcDataRow[8]
        t_lbreak=plcDataRow[9]
        t_rbreak=plcDataRow[10]
        t_direct=plcDataRow[11]
        t_p1 = plcDataRow[12]
        t_p2=plcDataRow[14]
        t_p8 =plcDataRow[13]
        t_pitch = f"+{toolDataEntriesRow[13]}"
        t_kinematic = plcDataRow[20]
        t_plc=plcDataRow[21]
        
        toolRow = [t_nr, t_name, t_l, t_r, t_r2, "+0", "+0", "+0", "0", "", t_time1, t_time2, t_curtime, t_typ, t_doc[:31], t_plc, t_lcut, t_angle, t_cut, t_ltol, t_rtol, "0", t_direct, t_roffs, t_loffs, t_lbreak, t_rbreak, t_nmax, "0", "", t_tangle, "", "0", "0", t_p1, t_p2, "0", "0", "0", "0", "0", t_p8, "", "0", t_pitch, "", "", "", t_kinematic, "", "", "", ""]
        print(f"toolrow is {toolRow}")
        update_file(paths["toolTableBackup"]+str(toolDataEntriesRow[19])+".t", toolRow)
        return True

    def machineStatus(self, combobox_value):
        # Function to check if IP is reachable using Tnccmd
        def ping_ip(ip):
            try:
                tnccmd_cmd = paths["TNCcmd"]
                output = subprocess.run(
                    [tnccmd_cmd, "ping", ip],
                    stdout=subprocess.PIPE, stderr=subprocess.PIPE
                )

                if output.returncode == 0:  # Success

                    # return output.returncode == 0  # Return True if ping was successful
                    return True
                else:  # Command failed
                    print(f"Error pinging {ip} with Tnccmd: {e}")
                    return False
            except Exception as e:
                print(f"Exception:ERROR BING {e}")
                return False

        for place in places:
            if place["placename"] == combobox_value and place["status"] == "machine":
                ip_address = place["link"]
        connectionTest=ping_ip(ip_address)

        # Check the ping status and return the result
        return connectionTest


    def platzierenCallFromHaimerSchnittstelle(self,placeName,iDCode):
        self.maschine_combobox=placeName
        self.movingTools=iDCode
        print(f"Here is palcename={placeName}and id={iDCode}")
        self.platzieren()