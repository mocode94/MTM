
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

import sqlite3

# Path to your SQLite database
db_path = r"F:\MTMDB.db"

# Connect to the database
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# Fetch all PLC values
cursor.execute("SELECT rowid, PLC FROM tnc640Data")
rows = cursor.fetchall()

# Process and update each row
for rowid, plc_value in rows:
    if plc_value.startswith("%"):
        binary_str = plc_value[1:]  # Remove '%'
        decimal_value = int(binary_str, 2)  # Convert binary to decimal
        cursor.execute("UPDATE tnc640Data SET PLC = ? WHERE rowid = ?", (decimal_value, rowid))

# Commit changes and close connection
conn.commit()
conn.close()

print("PLC column updated successfully.")
