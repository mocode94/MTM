
# from config import places

# toolPlace = "C32"
# TNC640DATEN_Rows=[]


# # Process IP addresses
# for dictionary in places:
#     if dictionary["placename"] == toolPlace and dictionary["status"]=="machine":
#         ip_address = dictionary["link"]
#         print(f"Verbindung zu {toolPlace} etabliert \u2713")
#         # Clean and validate IP address format
#         parts = ip_address.split('.')
        
#         if len(parts) == 4 and all(part.isdigit() for part in parts):
#             formatted_ip = f"[{ip_address}]"
#             TNC640DATEN_Rows.append(formatted_ip)  # Append formatted IP address
#         else:
#             print("FEHLER", f"falsche IP Adresseformat: {ip_address}")


# for i in TNC640DATEN_Rows:
#     print(i)    





# for dictionary in places:
#     if dictionary["placename"] == toolPlace and dictionary["status"] == "machine":
#         ip_address = [dictionary["link"]]
#         print(f"Verbindung zu {toolPlace} etabliert \u2713")
#         # Clean and validate IP address format

# # Manually format the list for printing
# formatted_ip_address = f"[{', '.join(ip_address)}]"
# print(f"IP Adressen: {formatted_ip_address}")

# import sqlite3

# # Path to your SQLite database
# db_path = r"F:\MTMDB.db"

# # Connect to the database
# conn = sqlite3.connect(db_path)
# cursor = conn.cursor()

# # Fetch all PLC values
# cursor.execute("SELECT rowid, PLC FROM tnc640Data")
# rows = cursor.fetchall()

# # Process and update each row
# for rowid, plc_value in rows:
#     if plc_value.startswith("%"):
#         binary_str = plc_value[1:]  # Remove '%'
#         decimal_value = int(binary_str, 2)  # Convert binary to decimal
#         cursor.execute("UPDATE tnc640Data SET PLC = ? WHERE rowid = ?", (decimal_value, rowid))

# # Commit changes and close connection
# conn.commit()
# conn.close()

# print("PLC column updated successfully.")




# import sqlite3

# # Path to your database file
# db_path = r"X:\Projekt\mtmgithub\MTM\data\MTMDB.db"

# def create_users_table():
#     conn = sqlite3.connect(db_path)
#     cursor = conn.cursor()

#     # Create table if it doesn't exist
#     cursor.execute("""
#         CREATE TABLE IF NOT EXISTS Users (
#             ID TEXT PRIMARY KEY,
#             Name TEXT NOT NULL,
#             lastIn TEXT, 
#             lastOut TEXT, 
#             rights TEXT
#         );
#     """)

#     conn.commit()
#     conn.close()

# # Run the function to create the table
# create_users_table()
# print("Users table created successfully.")




# import sqlite3
# from datetime import datetime

# # Path to your database file
# db_path = r"X:\Projekt\mtmgithub\MTM\data\MTMDB.db"

# def insert_first_user():
#     conn = sqlite3.connect(db_path)
#     cursor = conn.cursor()

#     # Get the current timestamp
#     now = datetime.now().strftime("%d.%m.%Y %H:%M:%S")

#     # Insert the first row
#     cursor.execute("""
#         INSERT INTO Users (ID, Name, lastIn, lastOut, rights) 
#         VALUES (?, ?, ?, ?, ?)
#     """, ("2", "Nico", now, None, "Admin"))  # lastOut is None for now

#     conn.commit()
#     conn.close()
#     print("First user inserted successfully.")

# # Run the function to insert the first user
# insert_first_user()



# import sqlite3

# def update_tnc640data(db_path):
#     try:
#         conn = sqlite3.connect(db_path)
#         cursor = conn.cursor()

#         # Update IKZ column
#         cursor.execute("""
#             UPDATE tnc640Data
#             SET IKZ = 1
#             WHERE IKZ = 0
#         """
#         )

#         # Update PLC column
#         cursor.execute("""
#             UPDATE tnc640Data
#             SET PLC = CASE 
#                 WHEN PLC = 0 THEN 2
#                 WHEN PLC = 9 THEN 11
#                 WHEN PLC = 1 THEN 3
#                 WHEN PLC = 5 THEN 7
#                 ELSE PLC
#             END
#         """
#         )

#         conn.commit()
#         print("Database updated successfully.")
#     except sqlite3.Error as e:
#         print("SQLite error:", e)
#     finally:
#         conn.close()

# if __name__ == "__main__":
#     update_tnc640data("X:\Projekt\mtmgithub\MTM\data\MTMDB.db")






# import cadquery as cq

# def count_holes_in_step(file_path):
#     # Load STEP file
#     model = cq.importers.importStep(file_path)

#     # Find all circular holes
#     hole_count = 0
#     for face in model.faces():
#         edges = face.edges()

#         # Check if the face has a single circular edge (indicating a hole)
#         for edge in edges:
#             if edge.geomType() == "CIRCLE":
#                 hole_count += 1
#                 break  # Count each hole only once

#     return hole_count

# # Example usage
# step_file = r"C:\Users\Mo_ab\Desktop\W1151-01-01-04.STEP"
# holes = count_holes_in_step(step_file)
# print(f"Number of holes detected: {holes}")

with open(r"X:\Projekt\mtmgithub\MTM\temp\192.168.81.12.t", "r", encoding="utf-8") as file:
    lines = file.readlines()


if len(lines) > 1:
    second_line = lines[1]  # Read the second line
    start_pos = second_line.find("CUR_TIME")  # Find "CUR_TIME"

    if start_pos != -1:
        next_word_pos = start_pos + len("CUR_TIME")  # Move to the end of "CUR_TIME"
        
        # Skip spaces
        while next_word_pos < len(second_line) and second_line[next_word_pos] == " ":
            next_word_pos += 1
        
        if next_word_pos < len(second_line):
            print(f'"CUR_TIME" starts at position: {start_pos}')
            print(f'The next word starts at position: {next_word_pos}')
        else:
            print("No next word found after CUR_TIME.")
    else:
        print('"CUR_TIME" not found in the second line.')
else:
    print("The file has less than two lines.")