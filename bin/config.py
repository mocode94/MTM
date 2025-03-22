import json

# Load the JSON data from the config file
with open('../config/config.json', 'r') as file:  # Replace 'config.json' with the actual path to your JSON file
    config_data = json.load(file)

# Access the paths
imgpaths = config_data['paths']['imgpaths']
compimgs = config_data['paths']['compimgs']
toolstl = config_data['paths']['toolstl']
mastercsv = config_data['paths']['mastercsv']
TNC640_Daten=config_data['paths']['TNC640_Daten']
paths=config_data["paths"]
toolsremark=config_data["paths"]["toolsremark"]
# Access the machines list
places= config_data["places"]
winconfig= config_data['winconfig']
settings=config_data["settings"]
  











# import sqlite3

# # Connect to the database
# db_path = r"X:\Projekt\mtmgithub\MTM\data\MTMDB.db"  # Replace with the path to your database file
# conn = sqlite3.connect(db_path)
# cursor = conn.cursor()

# # Query to fetch idCode values
# cursor.execute("SELECT rowid, idCode FROM currentTools")  # Use rowid as a unique identifier
# rows = cursor.fetchall()

# # Iterate through each row and check the idCode length
# for row in rows:
#     row_id = row[0]  # Use rowid as the unique identifier
#     id_code = str(row[1])  # Convert idCode to a string
    
#     if len(id_code) < 12:  # Check if the length is less than 12
#         # Pad with leading zeros to make it 12 digits
#         updated_id_code = id_code.zfill(12)
        
#         # Update the database with the corrected value
#         cursor.execute("UPDATE currentTools SET idCode = ? WHERE rowid = ?", (updated_id_code, row_id))
#         print(f"Updated idCode for rowid {row_id}: {id_code} -> {updated_id_code}")

# # Commit changes and close the connection
# conn.commit()
# conn.close()

# print("Database updated successfully!")

# from pylibdmtx.pylibdmtx import encode
# from PIL import Image, ImageDraw, ImageFont
# import os
# import math

# def generate_a4_pages_with_data_matrix(items, output_directory, dpi=400, margin=1):
#     # Calculate A4 dimensions in pixels
#     a4_width, a4_height = int(8.27 * dpi), int(11.69 * dpi)

#     # Desired size of the Data Matrix codes
#     code_size = (a4_width-5 * margin) // 8  # Dynamically adjust for 3 codes per row
#     font_size = int(code_size * 0.06)  # Adjust font size based on code size

#     # Create the output directory
#     os.makedirs(output_directory, exist_ok=True)

#     # Load font
#     try:
#         font = ImageFont.truetype("arial.ttf", font_size)
#     except IOError:
#         font = ImageFont.load_default()

#     # Calculate grid layout (rows and columns)
#     cols = 3  # 3 codes per row
#     rows = (a4_height - margin * 2) // (code_size + font_size + margin)  # Calculate rows based on available height
#     codes_per_page = cols * rows

#     # Generate A4 pages
#     total_pages = math.ceil(len(items) / codes_per_page)
#     print(f"Total pages to generate: {total_pages}")

#     for page in range(total_pages):
#         # Create a blank A4 canvas
#         a4_image = Image.new("RGB", (a4_width, a4_height), "white")
#         draw = ImageDraw.Draw(a4_image)

#         # Get the codes for the current page
#         start_index = page * codes_per_page
#         end_index = min(start_index + codes_per_page, len(items))
#         page_items = items[start_index:end_index]

#         for i, item in enumerate(page_items):
#             # Encode the item
#             encoded = encode(item.encode("utf-8"))
#             original_image = Image.frombytes("RGB", (encoded.width, encoded.height), encoded.pixels)

#             # Resize to fit the cell
#             resized_code = original_image.resize((code_size, code_size), Image.Resampling.LANCZOS)

#             # Calculate position on the grid
#             col = i % cols
#             row = i // cols
#             x = margin + col * (code_size + margin)
#             y = margin + row * (code_size + font_size + margin)

#             # Paste the code onto the A4 canvas
#             a4_image.paste(resized_code, (x, y))

#             # Add label below the code
#             text_bbox = draw.textbbox((0, 0), item, font=font)
#             text_width = text_bbox[2] - text_bbox[0]
#             text_x = x + (code_size - text_width) // 2
#             text_y = y + code_size + 5
#             draw.text((text_x, text_y), item, fill="black", font=font)

#         # Save the current page
#         output_path = os.path.join(output_directory, f"DataMatrix_Page_{page + 1}.png")
#         a4_image.save(output_path, "PNG", dpi=(dpi, dpi))
#         print(f"Saved: {output_path}")

# # List of items to encode
# items = [
#     # "C400", 
#     # "C42", 
#     # "DMU", 
#     # "ALL", 
#     # "C32",
#     "Haimer"
#     # "C32_Tool_Wagen",
#     # "C400_Arbeitsplatz",
#     # "C42_Arbeitsplatz",
#     # "DMU_Arbeitsplatz", 
#     # "C400Schrank - Fach_1",
#     # "C400Schrank - Fach_2",
#     # "C400Schrank - Fach_3", 
#     # "C400Schrank - Fach_4", 
#     # "C400Schrank - Dach",
#     # "C42Schrank - Fach_1",
#     # "C42Schrank - Fach_2",
#     # "C42Schrank - Fach_3",
#     # "C42Schrank - Fach_4",
#     # "C42Schrank - Dach"

#     # "C32Schrank - Fach_1_Vorne", 
#     # "C32Schrank - Fach_1_Mittel", 
#     # "C32Schrank - Fach_1_Hinten", 
#     # "C32Schrank - Fach_2_Vorne", 
#     # "C32Schrank - Fach_2_Mitte", 
#     # "C32Schrank - Fach_2_Hinten", 
#     # "C32Schrank - Fach_3_Vorne", 
#     # "C32Schrank - Fach_3_Mitte", 
#     # "C32Schrank - Fach_3_Hinten"
# ]

# # Output directory for the A4 sheets
# output_directory = "output_data_matrices_a4_3_per_row"

# # Generate A4 pages
# generate_a4_pages_with_data_matrix(items, output_directory)

