# import pyodbc
# import time
# import tkinter as tk
# import shutil
# import os
# import tkinter.font as font
# from tkinter import font
# from PIL import Image, ImageTk
# import sqlite3
# import tkinter.messagebox as messagebox
# from tkinter import messagebox
# from config import winconfig,paths
# import sys



# class main:
#     def __init__(self, query):
#         if query:
#             pass
#         else:
#             print("Error: PROZESS FEHLGESCHLAGEN.")
#             return False

#         self.mdb_file_path = paths["haimerDB"]
#         self.copied_mdb_file_path = paths["haimerBackup"]
#         self.password = "!1+2§3pp"
#         self.query = "SELECT X, Z, MeasuredAt FROM tCutterCurrentValues WHERE KeyToolId = ?"
#         self.previous_X = None
#         self.previous_Z = None
#         self.previous_MeasuredAt = None
#         self.continue_monitoring = True  # Flag to control monitoring
#         self.conn = None
#         self.cursor = None

#         self.search_value = query
#         self.start_monitoring(query)
#         print("Fetig.")
#         vv=input("Press Enter to continue...")
#         if vv=="1":
#             print("come 1")
#         else:
#             print("come 2")
#         return

#     def start_monitoring(self, searchValue):
#         """Start monitoring the MDB file and updating values (runs once)"""
#         self.search_value = searchValue

#         try:
#             # Initial file copy
#             self.copy_mdb_file()

#             if not self.check_copied_file():
#                 raise FileNotFoundError(f"Copied file not found at {self.copied_mdb_file_path}")

#             # Setup database connection
#             self.setup_db_connection()

#             # # Perform the file copy and query
#             # self.copy_mdb_file()

#             if self.check_copied_file():
#                 if self.conn and not self.conn.closed and self.cursor:
#                     try:
#                         self.cursor.execute(self.query, (self.search_value,))
#                         row = self.cursor.fetchone()

#                         if row:
#                             self.previous_X = row.X
#                             self.previous_Z = row.Z
#                             self.previous_MeasuredAt = row.MeasuredAt
#                             print(f"Radius: {self.previous_X} \nLaenge: {self.previous_Z} \nZeit: {self.previous_MeasuredAt}")
#                         else:
#                             print(f"Kein Daten fuer {self.search_value} wurde gefunden.")
#                             # messagebox.showerror("NOT FOUND", "NOT FOUND")
#                             return
#                     except pyodbc.Error as db_err:
#                         print(f"Database error: {db_err}")
#                 else:
#                     print("Database connection or cursor is not properly initialized or already closed.")
#                     return
#             else:
#                 print(f"Copied file not found at {self.copied_mdb_file_path}")
#                 return

#         except Exception as e:
#             print(f"An error occurred: {e}")
#         finally:
#             if self.conn and not self.conn.closed:
#                 self.conn.close()



#     def copy_mdb_file(self):
#         """Function to copy the MDB file to the new location"""
#         try:
#             shutil.copy(self.mdb_file_path, self.copied_mdb_file_path)
#             # print(f"File copied to {self.copied_mdb_file_path}")
#         except Exception as e:
#             print(f"Error copying file: {e}")

#     def check_copied_file(self):
#         """Check if the copied file exists"""
#         return os.path.exists(self.copied_mdb_file_path)

#     def setup_db_connection(self):
#         """Establish the database connection to the copied MDB file"""
#         try:
#             conn_str = (
#                 r"DRIVER={Microsoft Access Driver (*.mdb, *.accdb)};"
#                 r"DBQ=" + self.copied_mdb_file_path + ";"
#                 r"PWD=" + self.password + ";"
#             )
#             self.conn = pyodbc.connect(conn_str)
#             self.cursor = self.conn.cursor()
#         except Exception as e:
#             print(f"Error setting up database connection: {e}")







# if __name__ == "__main__":
#  # Read the first two lines from move.txt
#     try:
#         with open(r"X:\Projekt\mtmgithub\MTM\appios\measure.txt", 'r') as file:
#             lines = file.readlines()
#             query = lines[0].strip()  # First line

#     except Exception as e:
#         print(f"Error reading measure.txt: {e}")
#         sys.exit(1)

#     my_instance = main(query)



import pyodbc
import time
import tkinter as tk
import shutil
import os
import tkinter.font as font
from tkinter import font
from PIL import Image, ImageTk
import sqlite3
import tkinter.messagebox as messagebox
from tkinter import messagebox
from config import winconfig, paths
import sys
sys.stdout.reconfigure(encoding='utf-8')

class main:
    def __init__(self, query, vv=None):
        if query:
            pass
        else:
            print("Error: PROZESS FEHLGESCHLAGEN.")
            return False

        self.mdb_file_path = paths["haimerBackup"]
        self.copied_mdb_file_path = paths["haimerBackup"]
        self.password = "!1+2§3pp"
        self.query = "SELECT X, Z, MeasuredAt FROM tCutterCurrentValues WHERE KeyToolId = ?"
        self.previous_X = None
        self.previous_Z = None
        self.previous_MeasuredAt = None
        self.continue_monitoring = True  # Flag to control monitoring
        self.conn = None
        self.cursor = None
        self.search_value = query
        self.start_monitoring(query)



        if vv == None:
            self.printNewValues()
            print("Wollen Sie diese Messwerte uebernehmen?")
            return
        elif vv == "1":
            self.newValuesSubmit(query)
            print("UEBERNOMMEN")
            return
        else: 
            print("come 2")
        return






    def start_monitoring(self, searchValue):
        """Start monitoring the MDB file and updating values (runs once)"""
        self.search_value = searchValue

        try:
            # Initial file copy
            self.copy_mdb_file()

            if not self.check_copied_file():
                raise FileNotFoundError(f"Copied file not found at {self.copied_mdb_file_path}")

            # Setup database connection
            self.setup_db_connection()

            if self.check_copied_file():
                if self.conn and not self.conn.closed and self.cursor:
                    try:
                        self.cursor.execute(self.query, (self.search_value,))
                        row = self.cursor.fetchone()

                        if row:
                            self.previous_X = row.X
                            self.previous_Z = row.Z
                            self.previous_MeasuredAt = row.MeasuredAt
                            # print(f"Radius: {self.previous_X} \nLaenge: {self.previous_Z} \nZeit: {self.previous_MeasuredAt}")
                        else:
                            print(f"Kein Daten fuer {self.search_value} wurde gefunden.")
                            return
                    except pyodbc.Error as db_err:
                        print(f"Database error: {db_err}")
                else:
                    print("Database connection or cursor is not properly initialized or already closed.")
                    return
            else:
                print(f"Copied file not found at {self.copied_mdb_file_path}")
                return

        except Exception as e:
            print(f"An error occurred: {e}")
        finally:
            if self.conn and not self.conn.closed:
                self.conn.close()


    def newValuesSubmit(self,query):
        lengthOnly = [1, 2, 3]
        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()

        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        conn.close()
        # Iterate over the rows and update the 'Radius' and 'toolIstLaenge' fields if idCode matches the query
        for row in currentToolsData:
            if row[0] == query:  # Check if the first column matches the query
                type=row[3]
                self.place = row[19]
                if type in lengthOnly:
                            # Connect to the MTMDB.db SQLite database
                    conn = sqlite3.connect(paths["MTMDB"])
                    cur = conn.cursor()
                    # Update the Radius and toolIstLaenge values using newrow4 and newrow11
                    cur.execute("""
                        UPDATE currentTools
                        SET 
                            toolIstLaenge = ?
                        WHERE idCode = ?;
                    """, (self.previous_Z, query))  # Use the variables in the query
                    # Commit changes and close the connection
                    conn.commit()  # Commit the transaction first
                    cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                    conn.close()
                    self.callToolMovefunction(query)
                    break
                else:
                                        # Update the Radius and toolIstLaenge values using newrow4 and newrow11
                                        # Connect to the MTMDB.db SQLite database
                    conn = sqlite3.connect(paths["MTMDB"])
                    cur = conn.cursor()
                    cur.execute("""
                        UPDATE currentTools
                        SET toolRadius = ?, 
                            toolIstLaenge = ?
                        WHERE idCode = ?;
                    """, (self.previous_X, self.previous_Z, query))  # Use the variables in the query
                # Commit changes and close the connection
                    conn.commit()  # Commit the transaction first
                    cur.execute("PRAGMA wal_checkpoint(NORMAL);")  # Force WAL to merge changes
                    conn.close()
                    self.callToolMovefunction(query)
                    break


    def callToolMovefunction(self,query):
        import requests

        # Define the URL of the PHP file
        php_url = f"http://{paths['pcip']}/appios/create_txt.php"  # Single slash, not double


        # Define the parameters to send to the PHP file
        params = {
            "searchedValue": query,  # Replace with the actual value you want to send
            "selectedPlace": self.place,  # Replace with the actual place you want to send
        }

        # Make the GET request to the PHP file
        try:
            response = requests.get(php_url, params=params)
            
            # Check if the request was successful
            if response.status_code == 200:
                # Print the response text
                print(response.text)
            else:
                print(f"Error: Received status code {response.status_code}")
        except requests.RequestException as e:
            print(f"Request failed: {e}")




    def printNewValues(self):


        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()

        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        conn.close()
        # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
        for row in currentToolsData:
            if row[0] == query:
                oldValuex = row[4]
                oldValueZ = row[11]
        # print(f"alte Radius: {oldValuex}    neue Radius: {self.previous_X}\nalte Laenge: {oldValueZ}    neue Laenge: {self.previous_Z}\nletzte Zeit: {self.previous_MeasuredAt}\nRadiusdifferenz={oldValuex-self.previous_X}\nLaengedifferenz={oldValueZ - self.previous_Z:.4f}")
        label_width = 20

        print(f"{self.previous_MeasuredAt}\n\n")
        # Format radius and length values with 4 digits after the decimal point
        print(f"{'neue Radius':<{label_width}}{self.previous_X:.4f}")
        print(f"{'neue Laenge':<{label_width}}{self.previous_Z:.4f}")

        print(f"{'Radiusdifferenz':<{label_width}}{oldValuex - self.previous_X:.4f}")
        print(f"{'Laengedifferenz':<{label_width}}{oldValueZ - self.previous_Z:.4f}")


        conn.close()
    def copy_mdb_file(self):
        """Function to copy the MDB file to the new location"""
        try:
            shutil.copy(self.mdb_file_path, self.copied_mdb_file_path)
        except Exception as e:
            print(f"Error copying file: {e}")

    def check_copied_file(self):
        """Check if the copied file exists"""
        return os.path.exists(self.copied_mdb_file_path)

    def setup_db_connection(self):
        """Establish the database connection to the copied MDB file"""
        try:
            conn_str = (
                r"DRIVER={Microsoft Access Driver (*.mdb, *.accdb)};"
                r"DBQ=" + self.copied_mdb_file_path + ";"
                r"PWD=" + self.password + ";"
            )
            self.conn = pyodbc.connect(conn_str)
            self.cursor = self.conn.cursor()
        except Exception as e:
            print(f"Error setting up database connection: {e}")


if __name__ == "__main__":
    try:
        with open(r"..\appios\measure.txt", 'r') as file:
            lines = file.readlines()
            query = lines[0].strip()  # First line

    except Exception as e:
        print(f"Error reading measure.txt: {e}")
        sys.exit(1)

    vv = sys.argv[1] if len(sys.argv) > 1 else None
    my_instance = main(query, vv)
