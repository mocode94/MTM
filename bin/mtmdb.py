# ''########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ############THIS CODE TO PUT MASCHINECSV.CSV IN TABLE IN THE DB 
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################




import sqlite3
import csv

# Define database file and CSV file paths
db_file = "MTMDB.db"
csv_file = r"F:\withoutCol20and21.csv"

# Column data types based on your description
column_types = [
    "TEXT", "INTEGER", "TEXT", "INTEGER", "REAL", "REAL", "REAL", "REAL", 
    "INTEGER", "REAL", "REAL", "REAL", "REAL", "REAL", "TEXT", "TEXT", 
    "TEXT", "TEXT", "TEXT", "TEXT"
]

# Connect to the SQLite database (or create it if it doesn't exist)
conn = sqlite3.connect(db_file)
cur = conn.cursor()

# Define the table creation SQL command
create_table_query = """
CREATE TABLE IF NOT EXISTS currentTools (
    idCode TEXT,
    toolNummer INTEGER,
    toolName TEXT,
    toolTyp INTEGER,
    toolRadius REAL,
    toolEckenRadius REAL,
    toolSpitzenWinkel REAL,
    toolEintauchWinkel REAL,
    toolSchneiden INTEGER,
    toolSchnittLaenge REAL,
    toolAusspannlaenge REAL,
    tooIstLaenge REAL,
    toolSollLaenge REAL,
    toolSteigung REAL,
    komponent1 TEXT,
    komponent2 TEXT,
    komponent3 TEXT,
    komponent4 TEXT,
    komponent5 TEXT,
    platz TEXT
);
"""

# Create the table
cur.execute(create_table_query)

# Function to convert row values with empty strings to None based on column type
def convert_value(value, col_type):
    if value == "":
        return None
    if col_type == "INTEGER":
        return int(value)
    elif col_type == "REAL":
        return float(value)
    return value  # Default to TEXT

# Read and insert CSV data into the table
with open(csv_file, newline='') as csvfile:
    reader = csv.reader(csvfile, delimiter=';')  # Adjust delimiter if needed
    next(reader)  # Skip header row if it exists
    
    # Track if any rows are read from the CSV
    rows_inserted = False
    
    # Insert each row into the database
    for row in reader:
        # Convert the row based on the column types and handle empty strings as NULL
        row_data = [convert_value(row[i], column_types[i]) for i in range(20)]
        
        # Debugging output: Print each row data before insertion
        print("Inserting row:", row_data)
        
        # Insert the row into the table
        cur.execute("INSERT INTO currentTools VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", row_data)
        rows_inserted = True

# Check if any rows were actually inserted
if rows_inserted:
    print("Data was successfully inserted into the table.")
else:
    print("No data was inserted. Please check the CSV file or delimiter.")

# Commit changes and close the connection
conn.commit()
conn.close()


# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ############THIS CODE TO PUT MASCHINECSV.CSV IN TABLE IN THE DB 
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################
# ########################################################################################''




# import requests
# from bs4 import BeautifulSoup
# from PIL import Image, ImageTk
# import tkinter as tk
# from io import BytesIO
# import urllib.parse
# import re
# import os
# import threading

# class YourClass:
#     def __init__(self, root):
#         self.root = root
#         self.root.geometry("500x500")
#         self.stop_download_flag = False
        
#         # Create Tkinter Label to display the image
#         self.image_label = tk.Label(self.root)
#         self.image_label.pack()

#         # Entry field for the item value
#         self.entry_label = tk.Label(self.root, text="Enter Item:")
#         self.entry_label.pack()

#         self.entry = tk.Entry(self.root)
#         self.entry.pack()

#         # Button to trigger the search
#         self.search_button = tk.Button(self.root, text="Search", command=self.search_image)
#         self.search_button.pack()

#         # Label for loading spinner (initially hidden)
#         self.loading_label = tk.Label(self.root, text="Loading...", font=("Helvetica", 14), fg="blue")
#         self.loading_label.pack_forget()

#     def update_output(self, message):
#         print(message)  # You can modify this to update a GUI element if needed

#     def show_image(self, img_url):
#         """Load an image from a URL and show it in Tkinter window"""
#         try:
#             img_response = requests.get(img_url)
#             if img_response.status_code == 200:
#                 img = Image.open(BytesIO(img_response.content))
#                 img.thumbnail((200, 200))  # Resize for display (optional)
#                 img_tk = ImageTk.PhotoImage(img)
#                 self.image_label.config(image=img_tk)
#                 self.image_label.image = img_tk  # Keep reference to avoid garbage collection
#                 self.update_output(f"Displayed image from {img_url}")
#             else:
#                 self.update_output(f"Failed to load image: {img_url}")
#                 self.show_default_image()  # Show the default image if the requested image isn't found
#         except Exception as e:
#             self.update_output(f"Error displaying image from {img_url}: {e}")
#             self.show_default_image()  # Show the default image in case of an error

#     def show_default_image(self):
#         """Display the default image if the requested image is not found or there's an error"""
#         not_found_image_path = r"X:\Projekt\mtm\img\NOTFOUND.png"  # Path to default image
#         if os.path.exists(not_found_image_path):
#             img = Image.open(not_found_image_path)
#             img.thumbnail((200, 200))  # Resize for display (optional)
#             img_tk = ImageTk.PhotoImage(img)
#             self.image_label.config(image=img_tk)
#             self.image_label.image = img_tk  # Keep reference to avoid garbage collection
#             self.update_output(f"Displayed default 'not found' image from {not_found_image_path}")
#         else:
#             self.update_output("Default 'not found' image not found.")

#     def download_images(self, urls):
#         if not urls:
#             self.update_output("No URLs to process.")
#             return

#         url = urls.pop(0)
#         self.update_output(f"Processing URL: {url}")

#         response = requests.get(url)
#         soup = BeautifulSoup(response.text, 'html.parser')
#         img_tags = soup.find_all('img')

#         if not img_tags:
#             self.update_output(f'No images found at {url}')
#             self.show_default_image()  # Show default image if no images are found
#         else:
#             for img in img_tags:
#                 img_url = img.get('src')
#                 if img_url is None or img_url.startswith('data:'):
#                     continue

#                 img_url = urllib.parse.urljoin(url, img_url)
#                 img_name = os.path.basename(img_url)
#                 pattern = r'^jpg_600_b[\d_]+(?:_\d+|_m\d+)?\.jpg$'

#                 if re.match(pattern, img_name):
#                     if self.stop_download_flag:
#                         self.update_output("Download stopped by user.")
#                         return

#                     self.show_image(img_url)  # Show image in Tkinter window
#                     return  # Stop after displaying the first valid image

#             # If no valid image is found, show the default image
#             self.show_default_image()

#         if urls and not self.stop_download_flag:
#             self.after(100, self.download_images, urls)
#         else:
#             if self.stop_download_flag:
#                 self.update_output("Download process stopped.")
#                 self.stop_download_flag = False

#     def after(self, ms, func, *args):
#         """Method to run after a delay (like Tkinter's after method)"""
#         self.root.after(ms, func, *args)

#     def search_image(self):
#         item = self.entry.get()  # Get the entered item value
#         if item:
#             # Show loading symbol
#             self.loading_label.pack()
#             self.image_label.pack_forget()  # Hide the image initially

#             # Run the image download in a separate thread so the UI remains responsive
#             threading.Thread(target=self.start_download, args=(item,)).start()
#         else:
#             self.update_output("Please enter a valid item.")

#     def start_download(self, item):
#         # Construct URL and call download_images
#         urls = [f"https://www.hoffmann-group.com/DE/de/hom/p/{item}"]
#         self.download_images(urls)

#         # Once download completes (either image or default), hide loading label
#         self.loading_label.pack_forget()
#         self.image_label.pack()  # Show the image label again


# if __name__ == "__main__":
#     root = tk.Tk()
#     app = YourClass(root)
#     root.mainloop()





# import sqlite3

# def create_db_from_text(file_path):
#     # Database file path
#     db_path = r'X:\Projekt\mtm\data\MTMitems.db'
    
#     # Connect to the SQLite database (creates it if it doesn't exist)
#     conn = sqlite3.connect(db_path)
#     cursor = conn.cursor()
    
#     # Create the table if it doesn't exist
#     cursor.execute('''
#         CREATE TABLE IF NOT EXISTS Items (
#             artikelNumber TEXT,
#             Hersteller TEXT,
#             Link TEXT
#         )
#     ''')
    
#     # Open the text file and insert each line into the database
#     with open(file_path, 'r', encoding='utf-8') as file:
#         lines = [line.strip() for line in file.readlines()]
        
#     # Prepare data for insertion
#     data = [(item, 'Hoffmann', f'https://www.hoffmann-group.com/DE/de/hom/p/') for item in lines]
    
#     # Insert the data into the table
#     cursor.executemany('INSERT INTO Items (artikelNumber, Hersteller, Link) VALUES (?, ?, ?)', data)
    
#     # Commit the changes and close the connection
#     conn.commit()
#     conn.close()

# # Path to your text file
# text_file_path = r'X:\Projekt\mtm\data\items.txt'

# # Run the function
# create_db_from_text(text_file_path)

# print("Database created and data inserted successfully!")





# import sqlite3

# def create_spannmittel_table(db_path, text_file_path):
#     # Connect to the SQLite database (creates it if it doesn't exist)
#     conn = sqlite3.connect(db_path)
#     cursor = conn.cursor()
    
#     # Create the Spannmittel table if it doesn't exist
#     cursor.execute('''
#         CREATE TABLE IF NOT EXISTS Spannmittel (
#             Spannmittel TEXT
#         )
#     ''')
    
#     # Open the text file and insert each line into the Spannmittel table
#     with open(text_file_path, 'r', encoding='utf-8') as file:
#         lines = [line.strip() for line in file.readlines() if line.strip()]
    
#     # Insert each line as a new row in the table
#     cursor.executemany('INSERT INTO Spannmittel (Spannmittel) VALUES (?)', [(line,) for line in lines])
    
#     # Commit the changes and close the connection
#     conn.commit()
#     conn.close()

# # Paths to your database and text file
# db_path = r'X:\Projekt\mtm\data\MTMitems.db'
# text_file_path = r'X:\Projekt\mtm\data\spannmittel.txt'

# # Run the function to create the table and insert data
# create_spannmittel_table(db_path, text_file_path)

# print("Table 'Spannmittel' created and data inserted successfully!")
