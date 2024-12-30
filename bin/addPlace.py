import tkinter as tk
from tkinter import filedialog, ttk, StringVar
import json
import os
from config import paths,winconfig,places
import tkinter.font as font
import tkinter.messagebox as messagebox


class AddPlace:
    def __init__(self):
        self.window = tk.Tk()
        self.window.title("Select Option")
        self.window.configure(bg="#202020")
                # Calculate the window size
        window_width = 950
        window_height = 200

        # Get the screen dimensions
        screen_width = self.window.winfo_screenwidth()
        screen_height = self.window.winfo_screenheight()

        # Calculate the position to center the window
        x = (screen_width // 2) - (window_width // 2)
        y = (screen_height // 3) - (window_height // 1)

        # Set the geometry of the window
        self.window.geometry(f"{window_width}x{window_height}+{x}+{y}")


        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")

        # Create two big buttons: Machine and Place
        self.machineButton = tk.Button(self.window, text="Machine", command=self.showAddPlaceWindow, height=5, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.placeButton = tk.Button(self.window, text="Place", command=self.showPlaceWindow, height=5, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.subplaceButton = tk.Button(self.window, text="Sub-Place", command=self.addSubPlace, height=5, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        
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
        addPlaceWindow.configure(bg="#202020")

        window_width = 700
        window_height = 300
        screen_width = addPlaceWindow.winfo_screenwidth()
        screen_height = addPlaceWindow.winfo_screenheight()
        x = (screen_width // 2) - (window_width // 2)
        y = (screen_height // 2) - (window_height // 2)
        addPlaceWindow.geometry(f"{window_width}x{window_height}+{x}+{y}")

        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")


        # Labels for the Entry fields
        entryLabels = ["Image", "Name", "IP", "Max", "Belegt"]
        self.entries = []

        for i, labelText in enumerate(entryLabels):
            label = tk.Label(addPlaceWindow, text=f"{labelText}:",font=winconfig["fonttype"],bg="#202020",fg=winconfig["fontcolor"])
            label.grid(row=i, column=0, padx=10, pady=5)
            entry = tk.Entry(addPlaceWindow, width=40)
            entry.grid(row=i, column=1, padx=10, pady=5)
            entry.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))

            self.entries.append(entry)

        # File browser for the first entry (Image)
        self.browseButton = tk.Button(addPlaceWindow, text="Browse", command=self.browseFile,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.browseButton.grid(row=0, column=2, padx=10, pady=5)

        # Combobox 1 (TNC640, TNC640+Laser)
        self.comboValue1 = StringVar()
        self.comboBox1 = ttk.Combobox(addPlaceWindow, textvariable=self.comboValue1, values=["TNC640", "TNC640+Laser"])
        self.comboBox1.grid(row=6, column=0, padx=10, pady=5)
        self.comboBox1.set("TNC640")  # Default value
        self.comboBox1.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))
        # Change the font for the dropdown list items
        self.comboBox1.option_add("*TCombobox*Listbox*Font", (winconfig["fonttype"], winconfig["fontsize"]))

        # Combobox 2 (HSK63, SK40)
        self.comboValue2 = StringVar()
        self.comboBox2 = ttk.Combobox(addPlaceWindow, textvariable=self.comboValue2, values=["HSK63", "SK40"])
        self.comboBox2.grid(row=6, column=1, padx=10, pady=5)
        self.comboBox2.set("HSK63")  # Default value
        self.comboBox2.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))
        # Change the font for the dropdown list items
        self.comboBox2.option_add("*TCombobox*Listbox*Font", (winconfig["fonttype"], winconfig["fontsize"]))

        # Button to open the 5x5 grid window
        self.gridButton = tk.Button(addPlaceWindow, text="Platz", command=self.showGridWindow,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.gridButton.grid(row=6, column=2, padx=10, pady=5)

        # Button to save the data into JSON
        self.saveButton = tk.Button(addPlaceWindow, text="Speichern", command=self.saveToJson,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
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
        placeWindow.configure(bg="#202020")


        self.placeWindow=placeWindow
                # Center the window on the screen
        window_width = 570
        window_height = 300
        screen_width = placeWindow.winfo_screenwidth()
        screen_height = placeWindow.winfo_screenheight()
        x = (screen_width // 2) - (window_width // 2)
        y = (screen_height // 2) - (window_height // 2)
        placeWindow.geometry(f"{window_width}x{window_height}+{x}+{y}")
        # Labels for the Entry fields
        entryLabels = ["Image", "Name","Max", "Belegt"]
        self.entries = []
        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")
        for i, labelText in enumerate(entryLabels):
            label = tk.Label(placeWindow, text=f"{labelText}:",font=winconfig["fonttype"],bg="#202020",fg=winconfig["fontcolor"])
            label.grid(row=i, column=0, padx=10, pady=5)
            entry = tk.Entry(placeWindow, width=40)
            entry.grid(row=i, column=1, padx=10, pady=5)
            entry.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))

            self.entries.append(entry)

        # File browser for the first entry (Image)
        self.browseButton = tk.Button(placeWindow, text="Browse", command=self.browseFile,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.browseButton.grid(row=0, column=2, padx=10, pady=5)


        # Button to open the 5x5 grid window
        self.gridButton = tk.Button(placeWindow, text="Platz", command=self.showGridWindow,height=1, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.gridButton.grid(row=6, column=0, columnspan=4, pady=10)

        # Button to save the data into JSON
        self.saveButton = tk.Button(placeWindow, text="Speichern", command=self.saveToJsonPlaceButton,height=1, width=20,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.saveButton.grid(row=7, column=0, columnspan=4, pady=10)

        # Variables to store grid position
        self.positionX = None
        self.positionY = None

        # Load existing data from JSON
        self.placesData = self.loadJsonData()


    def browseFile(self):
        filePath = filedialog.askopenfilename()
        if filePath:
            # Get only the file name, not the full path
            fileName = os.path.basename(filePath)
            
            # Update the entry field with the file name only
            self.entries[0].delete(0, tk.END)
            self.entries[0].insert(0, fileName)

    def loadJsonData(self):
        """Load the JSON file if it exists, else return an empty structure."""
        if os.path.exists(paths["configJson"]):
            with open(paths["configJson"], "r") as json_file:
                return json.load(json_file)
        return {"places": []}

    def showGridWindow(self):
        self.positionX = None
        self.positionY = None
        gridWindow = tk.Toplevel(self.window)
        gridWindow.title("5x5 Grid")
                # Center the window on the screen
        window_width = 240
        window_height = 260
        screen_width = gridWindow.winfo_screenwidth()
        screen_height = gridWindow.winfo_screenheight()
        x = (screen_width // 2) - (window_width // 2)
        y = (screen_height // 2) - (window_height // 2)
        gridWindow.geometry(f"{window_width}x{window_height}+{x}+{y}")

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

            gridWindow.destroy()

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

        # Validation check for missing values
        missing_fields = []
        if not placename:
            missing_fields.append("Place Name")
        if not ip_link:
            missing_fields.append("IP Link")
        if not max_value:
            missing_fields.append("Max Value")
        if not belegt_value:
            missing_fields.append("Belegt Value")
        if not software:
            missing_fields.append("Software")
        if not holdertype:
            missing_fields.append("Holder Type")
        if self.positionX is None or self.positionX == "":
            missing_fields.append("Position X")
        if self.positionY is None or self.positionY == "":
            missing_fields.append("Position Y")

        # Show error message if there are missing fields
        if missing_fields:
            missing_fields_str = ", ".join(missing_fields)
            messagebox.showerror("Validation Error", f"Please fill in the following fields: {missing_fields_str}")
            return

        # Validate max_value and belegt_value are integers
        try:
            max_value = int(max_value)
        except ValueError:
            messagebox.showerror("Validation Error", "Max Value must be an integer.")
            return

        try:
            belegt_value = int(belegt_value)
        except ValueError:
            messagebox.showerror("Validation Error", "Belegt Value must be an integer.")
            return


 
        
        # Validate that max_value is greater than belegt_value
        if max_value <= belegt_value:
            messagebox.showerror("Validation Error", "Max Value must be greater than Belegt Value.")
            return

        # Proceed only if all fields are valid
        try:
            newPlace = {
                "image_path": image_path,
                "placename": placename,
                "position_x": self.positionX,
                "position_y": self.positionY,
                "status": "machine",  # Now always 'machine' since we removed the radiobox
                "link": ip_link,
                "software": software,
                "holdertype": holdertype,
                "max": max_value,
                "belegt": belegt_value
            }

            # Append the new place entry to the "places" list
            self.placesData["places"].append(newPlace)

            # Write the updated data back to the JSON file
            with open(paths["configJson"], "w") as json_file:
                json.dump(self.placesData, json_file, indent=4)

            messagebox.showinfo("Success", f"{placename} created successfully!")
        except Exception as e:
            messagebox.showerror("Error", f"An error occurred while saving the data: {e}")



    def saveToJsonPlaceButton(self):
        # Extract values from the Tkinter window
        image_path = self.entries[0].get()
        placename = self.entries[1].get()
        max_value = self.entries[2].get()
        belegt_value = self.entries[3].get()

        # Validation check for missing values
        missing_fields = []

        if not placename:
            missing_fields.append("Place Name")
        if not max_value:
            missing_fields.append("Max Value")
        if not belegt_value:
            missing_fields.append("Belegt Value")
        if self.positionX is None or self.positionX == "":
            missing_fields.append("Position X")
        if self.positionY is None or self.positionY == "":
            missing_fields.append("Position Y")

        # Show error message if there are missing fields
        if missing_fields:
            missing_fields_str = ", ".join(missing_fields)
            messagebox.showerror("Validation Error", f"Please fill in the following fields: {missing_fields_str}")
            return

        # Validate max_value and belegt_value are integers
        try:
            max_value = int(max_value)
        except ValueError:
            messagebox.showerror("Validation Error", "Max Value must be an integer.")
            return

        try:
            belegt_value = int(belegt_value)
        except ValueError:
            messagebox.showerror("Validation Error", "Belegt Value must be an integer.")
            return

        # Validate that max_value is greater than belegt_value
        if max_value <= belegt_value:
            messagebox.showerror("Validation Error", "Max Value must be greater than Belegt Value.")
            return

        # Proceed only if all fields are valid
        try:
            newPlace = {
                "image_path": image_path,
                "placename": placename,
                "position_x": self.positionX,
                "position_y": self.positionY,
                "status": "place",
                "subplace": [],
                "max": max_value,
                "belegt": belegt_value
            }

            # Append the new place entry to the "places" list
            self.placesData["places"].append(newPlace)

            # Write the updated data back to the JSON file
            with open(paths["configJson"], "w") as json_file:
                json.dump(self.placesData, json_file, indent=4)

            messagebox.showinfo("Success", "Data saved to JSON successfully!")
            self.placeWindow.destroy()
        except Exception as e:
            messagebox.showerror("Error", f"An error occurred while saving the data: {e}")

            
    def addSubPlace(self):
        placesList=[]
        for place in places:
            if place["status"] == "place":
                placesList.append(place["placename"])

            # This function opens the Add Place window (the existing add place interface)
        addSubPlaceWindow = tk.Toplevel(self.window)
        addSubPlaceWindow.title("Subplace einfügen")
        addSubPlaceWindow.configure(bg="#202020")
        addSubPlaceWindow.geometry("500x200")

        # Center the window
        window_width = 500
        window_height = 200
        screen_width = addSubPlaceWindow.winfo_screenwidth()
        screen_height = addSubPlaceWindow.winfo_screenheight()
        x = (screen_width // 2) - (window_width // 2)
        y = (screen_height // 2) - (window_height // 2)
        addSubPlaceWindow.geometry(f"{window_width}x{window_height}+{x}+{y}")


        self.addSubPlaceWindow=addSubPlaceWindow
        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")

        # Center the window
        addSubPlaceWindow.grid_columnconfigure(0, weight=1)
        addSubPlaceWindow.grid_columnconfigure(1, weight=1)

        # Labels for the Entry fields
        selectPlaceLabel = tk.Label(addSubPlaceWindow, text="Platz auswählen",font=winconfig["fonttype"],bg="#202020",fg=winconfig["fontcolor"])
        selectPlaceLabel.grid(row=0, column=0, padx=10, pady=5, sticky='ew')

        self.selectPlaceComboValue = StringVar()
        self.selectPlaceComboValue = ttk.Combobox(addSubPlaceWindow, textvariable=placesList, values=placesList,height=2, width=30,font=labelFontConfiguration)
        # Set the font for the Combobox
        
        self.selectPlaceComboValue.grid(row=1, column=0, padx=10, pady=5, sticky='ew')
        self.selectPlaceComboValue.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))
        # Change the font for the dropdown list items
        addSubPlaceWindow.option_add("*TCombobox*Listbox*Font", (winconfig["fonttype"], winconfig["fontsize"]))

        self.selectPlacebutton = tk.Button(addSubPlaceWindow, text="Auswählen", command=self.createSubplace, height=2, width=40,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        self.selectPlacebutton.grid(row=2, column=0, padx=50, pady=50)

    def createSubplace(self):
        self.addSubPlaceWindow.geometry("700x500")


        # Center the window
        window_width = 700
        window_height = 500
        screen_width = self.addSubPlaceWindow.winfo_screenwidth()
        screen_height = self.addSubPlaceWindow.winfo_screenheight()
        x = (screen_width // 2) - (window_width // 2)
        y = (screen_height // 2) - (window_height // 2)
        self.addSubPlaceWindow.geometry(f"{window_width}x{window_height}+{x}+{y}")
        try:
            selectedPlace = self.selectPlaceComboValue.get()
            # Check for missing values
            errors = []
            if not selectedPlace.strip():
                errors.append("Selected Place")
            # Show error message if there are issues
            if errors:
                error_message = "The following fields are invalid or missing:\n" + "\n".join(errors)
                messagebox.showerror("ERROR",error_message)  # Replace with GUI error message display if needed
                return
        except Exception as e:
            print(f"An unexpected error occurred: {e}")

        labelFontConfiguration = font.Font(family=winconfig["fonttype"], size=winconfig["fontsize"], weight="bold")

   
        subPlaceNameLabel= tk.Label(self.addSubPlaceWindow, text="Name",font=winconfig["fonttype"],bg="#202020",fg=winconfig["fontcolor"])
        subPlaceNameLabel.grid(row=3, column=0, padx=10, pady=5, sticky='ew')
        self.subPlaceNameEntry = tk.Entry(self.addSubPlaceWindow, width=40)
        self.subPlaceNameEntry.grid(row=3, column=1, padx=10, pady=5)
        self.subPlaceNameEntry.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))

        subPlaceCapacityLabel= tk.Label(self.addSubPlaceWindow, text="Kapazität",font=winconfig["fonttype"],bg="#202020",fg=winconfig["fontcolor"])
        subPlaceCapacityLabel.grid(row=4, column=0, padx=10, pady=5, sticky='ew')

        self.subPlaceCapacityEntry = tk.Entry(self.addSubPlaceWindow, width=40)
        self.subPlaceCapacityEntry.grid(row=4, column=1, padx=10, pady=5)
        self.subPlaceCapacityEntry.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))

        subPlaceBusyLabel= tk.Label(self.addSubPlaceWindow, text="Belegt",font=winconfig["fonttype"],bg="#202020",fg=winconfig["fontcolor"])
        subPlaceBusyLabel.grid(row=5, column=0, padx=10, pady=5, sticky='ew')

        self.subPlaceBusyEntry= tk.Entry(self.addSubPlaceWindow, width=40)
        self.subPlaceBusyEntry.grid(row=5, column=1, padx=10, pady=5)
        self.subPlaceBusyEntry.configure(font=(winconfig["fonttype"], winconfig["fontsize"]))
        subPlaceNameButton= tk.Button(self.addSubPlaceWindow, text="Speichern", command=self.saveSubPlace, height=2, width=40,font=labelFontConfiguration,bg=winconfig["bgcolor"],fg=winconfig["fontcolor"])
        subPlaceNameButton.grid(row=6, column=0, padx=50, pady=50)
    def saveSubPlace(self):
        # Retrieve input values
        
        try:
            # Retrieve input values
            selectedPlace = self.selectPlaceComboValue.get()
            subPlaceName = self.subPlaceNameEntry.get()
            subPlaceCapacity = self.subPlaceCapacityEntry.get()
            subPlaceBusy = self.subPlaceBusyEntry.get()

            # Check for missing values
            errors = []
            if not selectedPlace.strip():
                errors.append("Selected Place")
            if not subPlaceName.strip():
                errors.append("Sub Place Name")
            if not subPlaceCapacity.strip():
                errors.append("Sub Place Capacity")
            if not subPlaceBusy.strip():
                errors.append("Sub Place Busy")

            # Validate integer inputs
            try:
                subPlaceCapacity = int(subPlaceCapacity)
            except ValueError:
                errors.append("Sub Place Capacity (must be a number)")
            try:
                subPlaceBusy = int(subPlaceBusy)
            except ValueError:
                errors.append("Sub Place Busy (must be a number)")


            # Validate that max_value is greater than belegt_value
            if subPlaceCapacity <= subPlaceBusy:
                messagebox.showerror("Validation Error", "Capacity Value must be greater than Belegt Value.")
                return


            # Show error message if there are issues
            if errors:
                error_message = "The following fields are invalid or missing:\n" + "\n".join(errors)
                messagebox.showerror("ERROR",error_message)  # Replace with GUI error message display if needed
                return

            # If no errors, proceed with saving
            print("All inputs are valid!")
            # Continue with your existing logic to save subplaces here

        except Exception as e:
            print(f"An unexpected error occurred: {e}")

        # Path to config.json
        config_path ="../config/config.json"

        # Load the existing config.json file
        if os.path.exists(config_path):
            with open(config_path, "r", encoding="utf-8") as file:
                try:
                    config = json.load(file)
                except json.JSONDecodeError:
                    print("Error: Invalid JSON format in config.json.")
                    return
        else:
            print("Error: config.json not found.")
            return

        # Create a new subplace entry
        newSubPlace = {
            "subplacename": subPlaceName,
            "capacity": subPlaceCapacity,
            "busy": subPlaceBusy
        }

        # Find the selected place and update its subplace list
        for place in config.get("places", []):
            if place["placename"] == selectedPlace:
                if "subplace" not in place or not isinstance(place["subplace"], list):
                    place["subplace"] = []  # Initialize subplace as an empty list if missing or invalid
                place["subplace"].append(newSubPlace)
                break
        else:
            print(f"Error: Place '{selectedPlace}' not found in config.json.")
            return

        # Save the updated config back to the file
        with open(config_path, "w", encoding="utf-8") as file:
            json.dump(config, file, indent=4)

        messagebox.showinfo("Subplace",f"Subplace '{subPlaceName}' added to place '{selectedPlace}' successfully!")
        self.addSubPlaceWindow.destroy()
        self.window.destroy()

if __name__ == "__main__":
    app = AddPlace()
