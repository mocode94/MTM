import tkinter as tk
from tkinter import Label, Entry, Checkbutton
from tkinter import ttk  # Import ttk for the Combobox
import csv
from config import winconfig,paths,places,settings

class TNC640Laser:

    def __init__(self,code, holder, toolLenght):
        # Create the main window
        TNC_win = tk.Toplevel()
        TNC_win.title("TNC640_Daten")

        # Validation function to ensure input is a valid number, including decimals, negative/positive signs
        def validate_numeric_input(new_value, max_value=None):
            if new_value == "" or new_value == "-" or new_value == "+":  # Allow empty string, just sign
                return True
            try:
                # Try to convert the input into a float
                value = float(new_value)
                if max_value is not None and value > max_value:
                    return False
                return True
            except ValueError:
                return False

        def make_entry(toolLenght):
            global entries, check_vars  # Declare as global to access in button functions

            labels = [
                "CODE", "NMAX", "TIME1", "TIME2", "CURTIME", "LOFFS", "ROFFS", "LTOL", "RTOL",
                "LBREAK", "RBREAK", "DIRECT", "KD", "KL", "P2", "BC", "IKZ", "ML", "MLR", "AM", "KINEMATIC"
            ]

            code_value = code
            cfg=holder
            cfg=cfg.replace(' ', '_')
            cfg = cfg + ".CFG"

            label = Label(TNC_win, text="TT-Daten", font=("Arial", 12, "bold"))
            label.grid(row=3, column=1, padx=10, pady=20, sticky='w')

            label = Label(TNC_win, text="PLC-Daten", font=("Arial", 12, "bold"))
            label.grid(row=3, column=3, padx=10, pady=20, sticky='w')

            # Register the validation function
            validate_cmd_all = TNC_win.register(validate_numeric_input)
            validate_cmd_p2 = TNC_win.register(lambda new_value: validate_numeric_input(new_value, max_value=80))
            validate_cmd_nmax = TNC_win.register(lambda new_value: validate_numeric_input(new_value, max_value=18000))

            # Define BooleanVars for Checkbuttons
            bc_var = tk.BooleanVar()
            ikz_var = tk.BooleanVar()
            ml_var = tk.BooleanVar()
            mlr_var = tk.BooleanVar()
            am_var = tk.BooleanVar()

            check_vars = {
                "BC": bc_var,
                "IKZ": ikz_var,
                "ML": ml_var,
                "MLR": mlr_var,
                "AM": am_var
            }

            entries = {}

            # Define the font for the entries
            entry_font = ("Arial", 10, "bold")

            for idx, label_text in enumerate(labels):
                # Define the font for the labels
                label_font = ("Arial", 10, "bold")

                if label_text == "CODE":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx, column=0, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=idx, column=1, padx=10, pady=5)
                    entry.insert(0, code_value)  # Set the default value
                    entry.config(state='disabled')  # Make the entry disabled
                    entries["CODE"] = entry

                elif label_text == "NMAX":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx, column=0, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_nmax, '%P'), font=entry_font)
                    entry.grid(row=idx, column=1, padx=10, pady=5)
                    entries["NMAX"] = entry

                elif label_text == "DIRECT":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx, column=0, padx=10, pady=5, sticky='e')

                    # Create a combobox for the DIRECT field with two options: -1 and +1
                    combobox = ttk.Combobox(TNC_win, values=[-1, 1], width=18, font=entry_font)
                    combobox.grid(row=idx, column=1, padx=10, pady=5)
                    combobox.set(-1)  # Set the default value (optional)
                    
                    entries["DIRECT"] = combobox

                elif label_text == "LBREAK":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx, column=0, padx=10, pady=5, sticky='e')
                    
                    # Create the entry field for LBREAK and set the default value to "0.5"
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=idx, column=1, padx=10, pady=5)
                    entry.insert(0, "0.5")  # Set "0.5" as the default value
                    
                    entries["LBREAK"] = entry

                elif label_text == "RBREAK":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx, column=0, padx=10, pady=5, sticky='e')
                    
                    # Create the entry field for LBREAK and set the default value to "0.5"
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=idx, column=1, padx=10, pady=5)
                    entry.insert(0, "0.5")  # Set "0.5" as the default value
                    
                    entries["RBREAK"] = entry


                elif label_text == "TIME1":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=0, column=2, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=0, column=3, padx=10, pady=5)
                    entries["TIME1"] = entry

                elif label_text == "TIME2":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=1, column=2, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=1, column=3, padx=10, pady=5)
                    entries["TIME2"] = entry

                elif label_text == "CURTIME":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=2, column=2, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=2, column=3, padx=10, pady=5)
                    entries["CURTIME"] = entry

                elif label_text == "KINEMATIC":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=12, column=0, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', font=entry_font)
                    entry.grid(row=12, column=1, padx=10, pady=5)
                    entry.insert(0, cfg)
                    entry.config(state='disabled')
                    entries["KINEMATIC"] = entry

                elif label_text == "KD":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')

                    # Create a combobox for the KD field
                    combobox = ttk.Combobox(TNC_win, values=[35, 80, 120], width=20)
                    combobox.grid(row=idx - 7, column=3, padx=10, pady=5)
                    combobox.set(35)  # Set the default value (optional)
                    entries["KD"] = combobox

                elif label_text == "KL":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=idx - 7, column=3, padx=10, pady=5)
                    toolLenght=float(toolLenght)
                    KLValue=toolLenght+10
                    KLValue=str(KLValue)
                    entry.insert(0, KLValue)  # Set "0.5" as the default value
                    entries["KL"] = entry

                elif label_text == "P2":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')

                    # Create an entry field with validation for the P2 field
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_p2, '%P'), font=entry_font)
                    entry.grid(row=idx - 7, column=3, padx=10, pady=5)
                    entries["P2"] = entry

                elif label_text == "BC":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')
                    checkbutton = Checkbutton(TNC_win, variable=bc_var)
                    checkbutton.grid(row=idx - 7, column=3, padx=10, pady=5)
                    entries["BC"] = checkbutton

                elif label_text == "IKZ":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')
                    checkbutton = Checkbutton(TNC_win, variable=ikz_var)
                    checkbutton.grid(row=idx - 7, column=3, padx=10, pady=5)
                    entries["IKZ"] = checkbutton

                elif label_text == "ML":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')
                    checkbutton = Checkbutton(TNC_win, variable=ml_var)
                    checkbutton.grid(row=idx - 7, column=3, padx=10, pady=5)
                    entries["ML"] = checkbutton

                elif label_text == "MLR":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')
                    checkbutton = Checkbutton(TNC_win, variable=mlr_var)
                    checkbutton.grid(row=idx - 7, column=3, padx=10, pady=5)
                    entries["MLR"] = checkbutton

                elif label_text == "AM":
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx - 7, column=2, padx=10, pady=5, sticky='e')
                    checkbutton = Checkbutton(TNC_win, variable=am_var)
                    checkbutton.grid(row=idx - 7, column=3, padx=10, pady=5)
                    entries["AM"] = checkbutton

                else:
                    label = Label(TNC_win, text=label_text, font=label_font)
                    label.grid(row=idx, column=0, padx=10, pady=5, sticky='e')
                    entry = Entry(TNC_win, validate='key', validatecommand=(validate_cmd_all, '%P'), font=entry_font)
                    entry.grid(row=idx, column=1, padx=10, pady=5)
                    entries[label_text] = entry

            # Define buttons
            def clear_all():
                for key, entry in entries.items():
                    if isinstance(entry, Entry):
                        entry.delete(0, tk.END)
                    elif isinstance(entry, Checkbutton):
                        check_vars[key].set(False)
                    elif isinstance(entry, ttk.Combobox):
                        entry.set('')
                for var in check_vars.values():
                    var.set(False)



            def save_all():
                # Define the header based on your requirement
                header = [
                    "CODE", "NMAX", "TIME1", "TIME2", "CURTIME", "LOFFS", "ROFFS", "LTOL", "RTOL",
                    "LBREAK", "RBREAK", "DIRECT", "KD", "KL", "P2", "BC", "IKZ", "ML", "MLR", "AM", "KINEMATIC", "PLC"
                ]
                
                # Create a dictionary for easy lookup
                data = {}
                
                for key, widget in entries.items():
                    if isinstance(widget, Entry):
                        data[key] = widget.get()
                    elif isinstance(widget, Checkbutton):
                        # Retrieve the value of the associated BooleanVar
                        data[key] = '1' if check_vars[key].get() else '0'
                    elif isinstance(widget, ttk.Combobox):
                        data[key] = widget.get()
                
                # Ensure all header columns are present in the data dictionary
                for col in header:
                    if col not in data:
                        data[col] = '0'

                # Determine the PLC value based on the conditions
                BC = int(data.get("BC", "0"))
                IKZ = int(data.get("IKZ", "0"))
                ML = int(data.get("ML", "0"))
                MLR = int(data.get("MLR", "0"))
                AM = int(data.get("AM", "0"))

                if BC == 1 and IKZ == 0 and ML == 0 and MLR == 0 and AM == 0:
                    data["PLC"] = "%00000001"
                elif BC == 0 and IKZ == 1 and ML == 0 and MLR == 0 and AM == 0:
                    data["PLC"] = "%00000010"
                elif BC == 1 and IKZ == 1 and ML == 0 and MLR == 0 and AM == 0:
                    data["PLC"] = "%00000011"
                elif BC == 0 and IKZ == 0 and ML == 1 and MLR == 0 and AM == 0:
                    data["PLC"] = "%00000100"
                elif BC == 0 and IKZ == 1 and ML == 1 and MLR == 0 and AM == 0:
                    data["PLC"] = "%00000110"
                elif BC == 1 and IKZ == 1 and ML == 1 and MLR == 0 and AM == 0:
                    data["PLC"] = "%00000111"
                elif BC == 1 and IKZ == 0 and ML == 1 and MLR == 0 and AM == 0:
                    data["PLC"] = "%00000101"
                elif BC == 0 and IKZ == 1 and ML == 0 and MLR == 1 and AM == 0:
                    data["PLC"] = "%00001010"
                elif BC == 1 and IKZ == 0 and ML == 0 and MLR == 1 and AM == 0:
                    data["PLC"] = "%00001001"
                elif BC == 0 and IKZ == 1 and ML == 0 and MLR == 0 and AM == 1:
                    data["PLC"] = "%00010010"
                elif BC == 1 and IKZ == 0 and ML == 0 and MLR == 0 and AM == 1:
                    data["PLC"] = "%00010001"
                elif BC == 1 and IKZ == 1 and ML == 0 and MLR == 0 and AM == 1:
                    data["PLC"] = "%00010011"
                elif BC == 1 and IKZ == 1 and ML == 0 and MLR == 1 and AM == 0:
                    data["PLC"] = "%00001011"
                elif BC == 0 and IKZ == 0 and ML == 0 and MLR == 0 and AM == 1:
                    data["PLC"] = "%00010000"
                else:
                    data["PLC"] = "%00000000"  # Default value if none of the conditions match

                # Read existing rows
                rows = []
                try:
                    with open(paths["TNC640_Daten"], 'r', newline='', encoding='utf-8') as file:
                        reader = csv.reader(file, delimiter=';')
                        rows = list(reader)
                except FileNotFoundError:
                    # If the file doesn't exist, initialize with header only
                    rows.append(header)

                # Search for the row to update or determine if a new row should be added
                row_found = False

                # Create new row, replace empty strings and None with '0'
                new_row = [data.get(col, '0') if data.get(col) in (None, '') else data.get(col) for col in header]

                # Ensure no empty values are left in new_row (replaces any non-string empty values like None)
                new_row = ['0' if item in (None, '', ' ') else item for item in new_row]

                for i, row in enumerate(rows):
                    if row[0] == data.get('CODE', ''):
                        rows[i] = new_row  # Update existing row
                        row_found = True
                        break

                if not row_found:
                    if rows and rows[0] == header:
                        rows.append(new_row)  # Add new row if header exists
                    else:
                        rows = [header, new_row]  # Create file with header and new row if file was empty

                # Write updated rows back to the file
                with open(paths["TNC640_Daten"], 'w', newline='', encoding='utf-8') as file:
                    writer = csv.writer(file, delimiter=';')
                    writer.writerows(rows)

                print("Data saved to TNC640_Daten.csv")
                TNC_win.destroy()

            def cancel():
                TNC_win.destroy()

            # Add buttons
            button_frame = tk.Frame(TNC_win)
            button_frame.grid(row=len(labels) + 5, column=0, columnspan=4, pady=10)

            clear_button = tk.Button(button_frame, text="Entleeren", command=clear_all)
            clear_button.grid(row=0, column=0, padx=10, pady=10)

            save_button = tk.Button(button_frame, text="Speichern", command=save_all)
            save_button.grid(row=0, column=1, padx=10, pady=10)

            cancel_button = tk.Button(button_frame, text="Abbrechen", command=cancel)
            cancel_button.grid(row=0, column=2, padx=10, pady=10)

        # Call the function to create the labeled entry fields and buttons
        make_entry(toolLenght)

        # # Start the main event loop
        TNC_win.mainloop()

