import tkinter as tk
from tkinter import filedialog, ttk, StringVar
import json
import os
from config import paths,winconfig
import tkinter.font as font


class AddPlace:
    def __init__(self):
        self.window = tk.Tk()
        self.window.title("Select Option")
        self.window.configure(bg="#202020")


        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")

        # Create two big buttons: Machine and Place
        self.machineButton = tk.Button(self.window, text="Machine", command=self.showAddPlaceWindow, height=5, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.placeButton = tk.Button(self.window, text="Place", command=self.showPlaceWindow, height=5, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.subplaceButton = tk.Button(self.window, text="Sub-Place", command=self.showPlaceWindow, height=5, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        
        # Layout of the buttons
        self.machineButton.grid(row=0, column=0, padx=50, pady=50)
        self.placeButton.grid(row=0, column=1, padx=50, pady=50)
        self.subplaceButton.grid(row=0, column=2, padx=50, pady=50)


        # Run the window loop
        self.window.mainloop()

    def showAddPlaceWindow(self):
        # This function opens the Add Place window (the existing add place interface)
        addPlaceWindow = tk.Toplevel(self.window)
        addPlaceWindow.title("Maschine einfügen")

        # Labels for the Entry fields
        entryLabels = ["Image", "Name", "IP", "Max", "Belegt"]
        self.entries = []

        for i, labelText in enumerate(entryLabels):
            label = tk.Label(addPlaceWindow, text=f"{labelText}:")
            label.grid(row=i, column=0, padx=10, pady=5)
            entry = tk.Entry(addPlaceWindow, width=40)
            entry.grid(row=i, column=1, padx=10, pady=5)
            self.entries.append(entry)

        # File browser for the first entry (Image)
        self.browseButton = tk.Button(addPlaceWindow, text="Browse", command=self.browseFile)
        self.browseButton.grid(row=0, column=2, padx=10, pady=5)

        # Combobox 1 (TNC640, TNC640+Laser)
        self.comboValue1 = StringVar()
        self.comboBox1 = ttk.Combobox(addPlaceWindow, textvariable=self.comboValue1, values=["TNC640", "TNC640+Laser"])
        self.comboBox1.grid(row=6, column=0, padx=10, pady=5)
        self.comboBox1.set("TNC640")  # Default value

        # Combobox 2 (HSK63, SK40)
        self.comboValue2 = StringVar()
        self.comboBox2 = ttk.Combobox(addPlaceWindow, textvariable=self.comboValue2, values=["HSK63", "SK40"])
        self.comboBox2.grid(row=6, column=1, padx=10, pady=5)
        self.comboBox2.set("HSK63")  # Default value

        # Button to open the 5x5 grid window
        self.gridButton = tk.Button(addPlaceWindow, text="Platz", command=self.showGridWindow)
        self.gridButton.grid(row=6, column=2, padx=10, pady=5)

        # Button to save the data into JSON
        self.saveButton = tk.Button(addPlaceWindow, text="Speichern", command=self.saveToJson)
        self.saveButton.grid(row=7, column=0, columnspan=2, pady=10)

        # Variables to store grid position
        self.positionX = None
        self.positionY = None

        # Load existing data from JSON
        self.placesData = self.loadJsonData()

    def showPlaceWindow(self):
        # This function opens a new window with a simple label for "Place"
        placeWindow = tk.Toplevel(self.window)
        placeWindow.title("Platz einfügen")
        # Labels for the Entry fields
        entryLabels = ["Image", "Name", "Regale", "Reihe","Max", "Belegt"]
        self.entries = []

        for i, labelText in enumerate(entryLabels):
            label = tk.Label(placeWindow, text=f"{labelText}:")
            label.grid(row=i, column=0, padx=10, pady=5)
            entry = tk.Entry(placeWindow, width=40)
            entry.grid(row=i, column=1, padx=10, pady=5)
            self.entries.append(entry)

        # File browser for the first entry (Image)
        self.browseButton = tk.Button(placeWindow, text="Browse", command=self.browseFile)
        self.browseButton.grid(row=0, column=2, padx=10, pady=5)


        # Button to open the 5x5 grid window
        self.gridButton = tk.Button(placeWindow, text="Platz", command=self.showGridWindow)
        self.gridButton.grid(row=6, column=2, padx=10, pady=5)

        # Button to save the data into JSON
        self.saveButton = tk.Button(placeWindow, text="Speichern", command=self.saveToJsonPlaceButton)
        self.saveButton.grid(row=7, column=0, columnspan=2, pady=10)

        # Variables to store grid position
        self.positionX = None
        self.positionY = None

        # Load existing data from JSON
        self.placesData = self.loadJsonData()

    def browseFile(self):
        filePath = filedialog.askopenfilename()
        if filePath:
            self.entries[0].delete(0, tk.END)
            self.entries[0].insert(0, filePath)

    def loadJsonData(self):
        """Load the JSON file if it exists, else return an empty structure."""
        if os.path.exists(paths["configJson"]):
            with open(paths["configJson"], "r") as json_file:
                return json.load(json_file)
        return {"places": []}

    def showGridWindow(self):
        gridWindow = tk.Toplevel(self.window)
        gridWindow.title("5x5 Grid")

        def onSquareClick(x, y):
            self.positionX = x
            self.positionY = y
            print(f"Selected square at position X: {self.positionX}, Y: {self.positionY}")

            # Highlight the clicked square in red
            for i in range(5):
                for j in range(5):
                    button = buttons[i][j]
                    if (i, j) == (self.positionX, self.positionY):
                        button.config(bg="red")
                    elif (i, j) not in selectedSquares:
                        button.config(bg="lightgreen")

        # Create the 5x5 grid
        buttons = []
        selectedSquares = set()

        # Mark already selected squares in red from JSON data
        for i in range(5):
            row = []
            for j in range(5):
                # Check if the position is already taken
                isTaken = any(place["position_x"] == i and place["position_y"] == j for place in self.placesData["places"])
                color = "red" if isTaken else "lightgreen"
                state = "disabled" if isTaken else "normal"  # Disable the button if it's taken

                if isTaken:
                    selectedSquares.add((i, j))

                square = tk.Button(gridWindow, text="", width=4, height=2, bg=color,
                                state=state,  # Set the state to disabled if the grid is red
                                command=lambda x=i, y=j: onSquareClick(x, y))
                square.grid(row=i, column=j, padx=5, pady=5)
                row.append(square)
            buttons.append(row)


    def saveToJson(self):
        # Extract values from the Tkinter window
        image_path = self.entries[0].get()
        placename = self.entries[1].get()
        ip_link = self.entries[2].get()
        max_value = self.entries[3].get()
        belegt_value = self.entries[4].get()
        software = self.comboValue1.get()
        holdertype = self.comboValue2.get()

        # Create the place dictionary entry
        newPlace = {
            "image_path": image_path,
            "placename": placename,
            "position_x": self.positionX,
            "position_y": self.positionY,
            "status": "machine",  # Now always 'machine' since we removed the radiobox
            "link": ip_link,
            "software": software,
            "holdertype": holdertype,
            "max": int(max_value),
            "belegt": int(belegt_value)
        }

        # Append the new place entry to the "places" list
        self.placesData["places"].append(newPlace)

        # Write the updated data back to the JSON file
        with open(paths["configJson"], "w") as json_file:
            json.dump(self.placesData, json_file, indent=4)

        print("Data saved to JSON successfully!")


    def saveToJsonPlaceButton(self):
        # Extract values from the Tkinter window
        image_path = self.entries[0].get()
        placename = self.entries[1].get()
        Regale = self.entries[2].get()
        Reihen=self.entries[3].get()
        max_value = self.entries[4].get()
        belegt_value = self.entries[5].get()


        # Create the place dictionary entry
        newPlace = {
            "image_path": image_path,
            "placename": placename,
            "position_x": self.positionX,
            "position_y": self.positionY,
            "status": "place",  # Now always 'machine' since we removed the radiobox
            "Regale": int(Regale),
            "Reihen": int(Reihen),
            "max": int(max_value),
            "belegt": int(belegt_value)
        }

        # Append the new place entry to the "places" list
        self.placesData["places"].append(newPlace)

        # Write the updated data back to the JSON file
        with open(paths["configJson"], "w") as json_file:
            json.dump(self.placesData, json_file, indent=4)

        print("Data saved to JSON successfully!")
        

if __name__ == "__main__":
    app = AddPlace()
