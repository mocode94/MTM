import os
import tkinter as tk
from tkinter import filedialog, messagebox

def process_csv(file_path):
    root = tk.Tk()
    root.title("Mastercsv")
    root.geometry("350x200")

    # Add a button to select the CSV file
    btn_select = tk.Button(root, text="Wählen Sie CSV Datei aus", command=select_file)
    btn_select.pack(pady=20)

    label=tk.Label(root,text="das gewählte Datei wird zusortiert und gehandelt für MTM.\nDas Datei wird unter mastercsv gespeichert.")
    label.pack(pady=20)

    label2=tk.Label(root,text="Vor Auswählen, bitte unnötige Zeile vom CSV.file löschen",fg="red")
    label2.pack(pady=5)
    
    try:
        # Read the CSV file
        with open(file_path, 'r') as f:
            lines = f.readlines()
        
        # Initialize a list to store the processed rows
        processed_rows = []
        buffer = ""
        header_processed = False
        header = ""

        for line in lines:
            # Remove all double-quote characters
            line = line.replace('"', '')
            row = (buffer + line).strip().split(',')
            
            # Strip whitespace from each cell
            row = [cell.strip() for cell in row]

            # Process the header separately to change its delimiter
            if not header_processed:
                header = ";".join(row)
                header_processed = True
                continue

            # Ensure row has at least 48 columns
            if len(row) < 48:
                row.extend([''] * (48 - len(row)))

            # Set column 48 to "None" if empty
            if not row[47]:  # column 48 is at index 47
                row[47] = "0"
            
            if len(row) >= 68:
                processed_rows.append(row[:68])
                buffer = ",".join(row[68:])
            else:
                buffer += line.strip()

        # Handle the case where the buffer is not empty after the loop
        if buffer:
            final_row = buffer.strip().split(',')
            final_row = [cell.strip() for cell in final_row]  # Strip whitespace from each cell
            processed_rows.append(final_row)
        
        # Sort rows based on the first column
        processed_rows.sort(key=lambda x: int(x[0]))

        # Convert rows to semicolon-delimited strings
        processed_rows = [";".join(row) for row in processed_rows]

        # Define the output file path in the same directory as the original file
        output_directory = os.path.dirname(file_path)
        output_file = os.path.join(output_directory, "MASTER.csv")
        
        # Write the processed rows to the new CSV file
        with open(output_file, 'w') as f:
            f.write(header + '\n')
            for row in processed_rows:
                f.write(row + '\n')

        messagebox.showinfo("Success", f"Processed CSV saved as {output_file}")
        root.destroy()

    except Exception as e:
        messagebox.showerror("Error", str(e))

def select_file():
    file_path = filedialog.askopenfilename(filetypes=[("CSV Files", "*.csv")])
    if file_path:
        process_csv(file_path)

# Create the Tkinter application
root = tk.Tk()
root.title("Mastercsv")
root.geometry("350x200")

# Add a button to select the CSV file
btn_select = tk.Button(root, text="Wählen Sie CSV Datei aus", command=select_file)
btn_select.pack(pady=20)

label=tk.Label(root,text="das gewählte Datei wird zusortiert und gehandelt für MTM.\nDas Datei wird unter mastercsv gespeichert.")
label.pack(pady=20)

label2=tk.Label(root,text="Vor Auswählen, bitte unnötige Zeile vom CSV.file löschen",fg="red")
label2.pack(pady=5)

# Run the application
root.mainloop()
