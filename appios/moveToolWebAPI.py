import sys  # Make sure to import sys

import xml.etree.ElementTree as ET
import os
import csv
# import cv2
import tkinter.messagebox as messagebox
from PIL import Image, ImageTk
from tkinter import Toplevel, Label, Entry, Button, StringVar,ttk,font
import tkinter as tk
# import requests
# import io
import os
from config import winconfig,paths,places
from TNC640_Daten import TNC640Laser as TNC640Laser

import subprocess

import time
import sqlite3

import sys
sys.stdout.reconfigure(encoding='utf-8')


class main:
    def __init__(self, query, toolPlace):
        if self.platzieren(query, toolPlace):
            print("Fetig.")

        else:
            print("Error: PROZESS FEHLGESCHLAGEN.")

    def platzieren(self,query,toolPlace):

        selectedPlaces=[]
        dmCodes=[]
   
        for place in places:
            if place["status"] == "place" and place.get("subplace"):  
                # Add only subplace names if subplaces exist
                for subplace in place["subplace"]:
                    selectedPlaces.append(place["placename"] + " - " + subplace["subplacename"])
            else:
                # Add the placename directly if no subplaces
                selectedPlaces.append(place["placename"])


        if toolPlace in selectedPlaces:
            print("Platz geprueft \u2713")
            time.sleep(2)

        else:
            print("Error, unbekannter Platz")
            return False

        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()

        # Fetch all data from the currentTools table
        cur.execute("SELECT * FROM currentTools")
        currentToolsData = cur.fetchall()
        # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
        for row in currentToolsData:
            if row[0] == query:
                dmCodes.append(row[0])

        
        if query in dmCodes:
            print("Code gefunden \u2713")
            time.sleep(2)
        else:
            print("Error, unbekannter Code")
            return False
        

        maschinenNamen=[]

        for place in places:
            if place["placename"] == toolPlace and place["status"]=="machine":
                maschinenNamen.append(place["placename"])

        rows = []
        platzierte_rows=[]
        TNC640DATEN_Rows=[]
        found = False
         


        if toolPlace in maschinenNamen:
            if self.machineStatus(toolPlace):
                print(f"Verbindung zu {toolPlace} gefunden \u2713")
            else:
                print(f"Keine Verbindung zu {toolPlace} gefunden\nbzw. {toolPlace} ist offline")
                return False
        else:
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()


            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()
            # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
            for row in currentToolsData:
                if row[0] in query:
                    row = list(row)  # Convert tuple to list to modify
                    row[19] = toolPlace  # Update the 'platz' field
                    platzierte_rows.append(row)
                    found = True
                    break

            # Update the database if any matching rows were found
            if found:
                for row in platzierte_rows:
                    cur.execute("""
                        UPDATE currentTools
                        SET platz = ?
                        WHERE idCode = ?
                    """, (row[19], row[0]))
                # Commit the changes after all updates are done
                conn.commit()
                # Ensure the connection is closed properly
                conn.close()
                print("Das Werkzeug ist erfolgreich platziert \u2713")
                return True
            else:
                print("FEHLER", f"{row[0]} nicht in die Datenbank")
                return False


        # Process IP addresses
        for dictionary in places:
            if dictionary["placename"] == toolPlace and dictionary["status"]=="machine":
                ip_address = dictionary["link"]
                print(f"Verbindung zu {toolPlace} etabliert \u2713")
                # Clean and validate IP address format
                parts = ip_address.split('.')
                
                if len(parts) == 4 and all(part.isdigit() for part in parts):
                    formatted_ip = f"[{ip_address}]"
                    TNC640DATEN_Rows.append(formatted_ip)  # Append formatted IP address
                else:
                    print("FEHLER", f"falsche IP Adresseformat: {ip_address}")
                    return False
        rowData=[]


        try:
            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()


            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()

            for machineCsvRow in currentToolsData:
                if machineCsvRow:
                    if machineCsvRow[0] == query:
                        # Convert the tuple to a list to modify it
                        machineCsvRow = list(machineCsvRow)
                        machineCsvRow[15]=machineCsvRow[14]+" + "+machineCsvRow[15]

                        try:
                            # Connect to the SQLite database
                            conn = sqlite3.connect(paths["MTMDB"])
                            cursor = conn.cursor()

                            # Query the database for the matching QR code
                            cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (query,))
                            tnc640Row = cursor.fetchone()

                            if tnc640Row:
                                # Join elements from machineCsvRow (excluding the first element) with tnc640Row (excluding the first element)
                                combined_row = machineCsvRow[1:] + list(tnc640Row[1:])
                                rowData.append(f"[{','.join(map(str, combined_row))}]")
                                print(f"PLC-Daten für {query} gefunden \u2713")
                                Standart=False
                            else:
                                print(f"FEHLER, Keine PLC-Daten für {query} gefunden")
                                if  Standart:
                                    plcDataRow = [
                                        query, "0", "0", "0", "0", "0", "0", "0", "0", "0.5", "0.5",
                                                "-1", "80", "0", "0", "0", "0", "0", "0", "0", "", "%00000000"
                                    ]

                                    try:
                                        # Connect to the SQLite database
                                        conn = sqlite3.connect(paths["MTMDB"])
                                        cursor = conn.cursor()

                                        # Insert the default PLC data
                                        insert_query = '''
                                        INSERT INTO tnc640Data (
                                            CODE, NMAX, TIME1, TIME2, CURTIME, LOFFS, ROFFS, LTOL, RTOL,
                                            LBREAK, RBREAK, DIRECT, Max_Durchmesser, P2, BC,
                                            IKZ, ML, MLR, AM, PLC
                                        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                                        '''
                                        cursor.execute(insert_query, plcDataRow)
                                        conn.commit()
                                        # Connect to the SQLite database
                                        conn = sqlite3.connect(paths["MTMDB"])
                                        cursor = conn.cursor()

                                        # Query the database for the matching QR code
                                        cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (query,))
                                        tnc640Row = cursor.fetchone()


                                        combined_row = machineCsvRow[1:] + list(tnc640Row[1:])
                                        rowData.append(f"[{','.join(map(str, combined_row))}]")
                                        print(f"Kein PLC Daten gefunden\nStandard PLC für {query} erstellt.\u2713")

                                    except sqlite3.OperationalError as e:
                                        print(f"Fehler beim Zugriff auf die Datenbank: {e}")
                                        return False
                                    except Exception as e:
                                        print(f"Fehler beim Erstellen der PLC-Daten: {e}")
                                        return False
                                    finally:
                                        # Close the database connection
                                        conn.close()
                                else:
                                    print("Kein PLC-Datensatz erstellt")
                                    return False

                        except sqlite3.OperationalError as e:
                            print(f"Fehler beim Zugriff auf die Datenbank: {e}")
                            return False
                        except Exception as e:
                            print(f"Fehler: {str(e)}")
                            return False
                        finally:
                            # Close the database connection
                            conn.close()



                else:
                    print(f"{query} nicht in der Datenbank gefunden")
                    return False

        except Exception as e:
            print("Error2", f"An error occurred while reading the second file: {e}")
            return False
            


        # for row in rowData:
        #     print(f"{formatted_ip}{row}")


        # Write the combined formatted rows to the DNC_INPUT file
        try:
            with open(paths["DNCINPUT"] + "DNCinput.txt", mode='w', newline='') as file:
                for row in rowData:
                    file.write(f"{formatted_ip}{row}\n")  # Join list elements with newlines and write to the file
                    print(f"Daten zu der Schnittstelle vorbereitet \u2713")
        except Exception as e:
            print("Error3", f"An error occurred while writing to DNC_INPUT: {e}")
            return False


        import subprocess

        program_path = paths["DNCSchnittstelle"]
        # Running the program
        try:
            result = subprocess.run([program_path], check=True)
            print(f"Daten zu {toolPlace} gescheckt \u2713")
        except subprocess.CalledProcessError as e:
            print(f"Program failed with return code: {e.returncode}")
            return False
        except FileNotFoundError:
            print("The specified program was not found.")
            return False

        try:
            # Connect to the MTMDB.db SQLite database
            conn = sqlite3.connect(paths["MTMDB"])
            cur = conn.cursor()

            # Fetch all data from the currentTools table
            cur.execute("SELECT * FROM currentTools")
            currentToolsData = cur.fetchall()

            platzierte_rows = []
            found = False

            # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
            for row in currentToolsData:
                if row[0] in query:
                    row = list(row)  # Convert tuple to list to modify
                    row[19] = toolPlace  # Update the 'platz' field
                    platzierte_rows.append(row)
                    found = True

            # Update the database if any matching rows were found
            if found:
                for row in platzierte_rows:
                    cur.execute("""
                        UPDATE currentTools
                        SET platz = ?
                        WHERE idCode = ?
                    """, (row[19], row[0]))
                # Commit the changes after all updates are done
                conn.commit()
                # Ensure the connection is closed properly
                conn.close()



                for i in platzierte_rows:
                    if i[1] is not None:  # Check if the row has a non-None value
                        try:
                            # Connect to the SQLite database
                            conn = sqlite3.connect(paths["MTMDB"])
                            cursor = conn.cursor()

                            # Query the database for the matching PLC data
                            cursor.execute("SELECT * FROM tnc640Data WHERE CODE = ?", (i[0],))
                            PlcDataRow = cursor.fetchone()

                            if PlcDataRow:
                                # Pass the current row and fetched PLC data to the addToolRowInToolTableBackup method
                                success = self.addToolRowInToolTableBackup(i, PlcDataRow)
                                if success:
                                    print(
                                        "ERFOLG",
                                        f"Das Werkzeug ist erfolgreich platziert \u2713\nUND\nsie wurde in toolTableBackup eingetragen \u2713"
                                    )
                                    return True
                                else:
                                    print(
                                        "FEHLER",
                                        f"Das Werkzeug konnte nicht platziert werden!!\nMÖGLICHE FEHLER\n-falsche Daten\nOder\nfehlende PLC-Daten."
                                    )
                                    return False
                            else:
                                # No matching PLC data found for the CODE
                                print(f"Keine passenden PLC-Daten für CODE {i[0]} gefunden.")
                                return False
                        except sqlite3.OperationalError as e:
                            print("Fehler", f"Fehler beim Zugriff auf die Datenbank: {e}")
                            return False
                        except Exception as e:
                            print("Fehler", f"Fehler: {str(e)}")
                            return False
                        finally:
                            # Close the database connection
                            conn.close()
                    else:
                        continue

            else:
                print("Erro3", "Die Aufnahme ist nicht registriert")
                return False
        
        except Exception as e:
           print("Erro4", f"An error occurred: {e}")
        return False


    def machineStatus(self, toolPlace):
        # Function to check if IP is reachable using Tnccmd
        def ping_ip(ip):
            try:
                # Full path to Tnccmd (adjust the path if necessary)
                tnccmd_cmd = paths["TNCcmd"]
                output = subprocess.run(
                    [tnccmd_cmd, "ping", ip],
                    stdout=subprocess.PIPE, stderr=subprocess.PIPE
                )
                return output.returncode == 0  # Return True if ping was successful
            except Exception as e:
                print(f"Error pinging {ip} with Tnccmd: {e}")
                return False

        # Finding the corresponding IP for the given place (toolPlace)
        for place in places:
            if place["placename"] == toolPlace and place["status"]=="machine":
                ip_address = place["link"]
                connectionTest=ping_ip(ip_address)
                break
            elif place["placename"] == toolPlace and place["status"]=="place":
                connectionTest=True
                break
            else:
                connectionTest=False

        # Check the ping status and return the result
        return connectionTest



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

                    #cancel print(f"Updated line: {columns}")

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
                
                #cancel print(f"Created new line: {new_line}")
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
                    print(f"Keine toolTablle-Backup Datei für {toolDataEntriesRow[19]} gefunden")
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
                        #cancel print(f"Inserting new line after index {nearest_index} (line_nr: {lines[nearest_index][:8].strip()})")
                        lines.insert(nearest_index + 1, new_line + '\n')
                    else:
                        #cancel print("Appending new line at the end")
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

            toolRow = [t_nr, t_name, t_l, t_r, t_r2, "+0", "+0", "+0", "0", "", t_time1, t_time2, t_curtime, t_typ, t_doc, t_plc, t_lcut, t_angle, t_cut, t_ltol, t_rtol, "0", t_direct, t_roffs, t_loffs, t_lbreak, t_rbreak, t_nmax, "0", "", t_tangle, "", "0", "0", t_p1, t_p2, "0", "0", "0", "0", "0", t_p8, "", "0", t_pitch, "", "", "", t_kinematic, "", "", "", ""]
            #cancel print(toolRow)
            #cancel print(paths["toolTableBackup"])
            #cancel print(toolDataEntriesRow[19])
            #cancel print(str(toolDataEntriesRow[19]))
            #cancel print(paths["toolTableBackup"]+str(toolDataEntriesRow[19])+".t")
            update_file(paths["toolTableBackup"]+str(toolDataEntriesRow[19])+".t", toolRow)
            return True

if __name__ == "__main__":
 # Read the first two lines from move.txt
    try:
        with open(r"..\appios\move.txt", 'r') as file:
            lines = file.readlines()
            query = lines[0].strip()  # First line
            toolPlace = lines[1].strip()  # Second line
    except Exception as e:
        print(f"Error reading move.txt: {e}")
        sys.exit(1)

    my_instance = main(query, toolPlace)
    
    # my_instance = main()








        # def platzieren(self,query,toolPlace):
    #         try:
    #             combobox_value = toolPlace  # Get the value from the combobox
    #         except:
    #             combobox_value=toolPlace
    #         print(query,toolPlace)
    #         if self.machineStatus(combobox_value):
    #             print("Connection found")
    #         elif not self.machineStatus(combobox_value):
    #             for dictionary in places:
    #                 if dictionary["placename"] == combobox_value and dictionary["status"]=="place":
    #                     print ("itis place")
    #                     continue
    #         else:
    #             # messagebox.showerror("No Connection",f"{combobox_value} is offline")
    #             print("NO Connection found\nNo Connection",f"{combobox_value} is offline")
                
    #             return "offline"

    #         # file_path = paths["machinecsv"]  # Set your CSV file path here
            
    #         if not combobox_value:
    #             # messagebox.showerror("Error", "bitte wählen Sie einen Platz aus!")
    #             print("Error", "bitte wählen Sie einen Platz aus!")

    #             return
    #         try:
    #             # Get the values in the first column from the second Treeview
                
    #             self.movingTools = query

    #         except:
    #             self.movingTools=query




    #         rows = []
    #         platzierte_rows=[]
    #         TNC640DATEN_Rows=[]
    #         found = False
    #         machines=[]
    #         # Process IP addresses
    #         for dictionary in places:
    #             if dictionary["placename"] == combobox_value and dictionary["status"]=="place":
    #                 # Connect to the MTMDB.db SQLite database
    #                 conn = sqlite3.connect(paths["MTMDB"])
    #                 cur = conn.cursor()


    #                 # Fetch all data from the currentTools table
    #                 cur.execute("SELECT * FROM currentTools")
    #                 currentToolsData = cur.fetchall()
    #                 # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
    #                 for row in currentToolsData:
    #                     if row[0] in self.movingTools:
    #                         row = list(row)  # Convert tuple to list to modify
    #                         row[19] = combobox_value  # Update the 'platz' field
    #                         platzierte_rows.append(row)
    #                         found = True

    #                 # Update the database if any matching rows were found
    #                 if found:
    #                     for row in platzierte_rows:
    #                         cur.execute("""
    #                             UPDATE currentTools
    #                             SET platz = ?
    #                             WHERE idCode = ?
    #                         """, (row[19], row[0]))
    #                     # Commit the changes after all updates are done
    #                     conn.commit()
    #                     # Ensure the connection is closed properly
    #                     conn.close()
    #                     # messagebox.showinfo("Success", f"Das Werkzeug ist erfolgreich platziert")
    #                     print("Success", f"Das Werkzeug ist erfolgreich platziert")

    #                     return 0
    #                 else:
    #                     # messagebox.showerror("ERROR", f"UNBEKANNTER FEHLER")
    #                     print("ERROR", f"UNBEKANNTER FEHLER")

    #                     return
    #             else:
    #                 # print("Platz ist Maschine")
    #                 pass





    #         # Process IP addresses
    #         for dictionary in places:
    #             if dictionary["placename"] == combobox_value and dictionary["status"]=="machine":
    #                 ip_address = dictionary["link"]
    #                 print(f"this is ip: {ip_address}")
                    
    #                 # Clean and validate IP address format
    #                 parts = ip_address.split('.')
                    
    #                 if len(parts) == 4 and all(part.isdigit() for part in parts):
    #                     formatted_ip = f"[{ip_address}]"
    #                     TNC640DATEN_Rows.append(formatted_ip)  # Append formatted IP address
    #                 else:
    #                     # messagebox.showerror("Error", f"Invalid IP address format: {ip_address}")
    #                     print("Error", f"Invalid IP address format: {ip_address}")

    #         rowData=[]

    #         for treeViewValuesRow in self.movingTools:
    #             QRcode=treeViewValuesRow
    #             print(QRcode)
    #             try:
    #                 # Connect to the MTMDB.db SQLite database
    #                 conn = sqlite3.connect(paths["MTMDB"])
    #                 cur = conn.cursor()


    #                 # Fetch all data from the currentTools table
    #                 cur.execute("SELECT * FROM currentTools")
    #                 currentToolsData = cur.fetchall()
                    
    #                 for machineCsvRow in currentToolsData:
    #                     if machineCsvRow[0]==QRcode:
    #                         # Convert the tuple to a list to modify it
    #                         machineCsvRow = list(machineCsvRow)
    #                         machineCsvRow[15]=machineCsvRow[14]+" + "+machineCsvRow[15]

    #                         with open(paths["TNC640_Daten"], mode='r', newline='') as file:
    #                             readerTnc = csv.reader(file, delimiter=";")
    #                             for tnc640Row in readerTnc:
    #                                 if tnc640Row[0]==QRcode:
    #                                     # Join elements without quotes and add to rowData
    #                                     combined_row = machineCsvRow[1:] + tnc640Row[1:]
    #                                     rowData.append(f"[{','.join(map(str, combined_row))}]")
    #                                     print("2 lists created")
    #                                 else:
    #                                     continue
    #                     else:
                            
    #                         # print("no match in machineCsvRow")
    #                         continue

    #             except Exception as e:
    #                 # messagebox.showerror("Error2", f"An error occurred while reading the second file: {e}")
    #                 print("Error2", f"An error occurred while reading the second file: {e}")
    #                 return
                


    #         for row in rowData:
    #             # print(f"{formatted_ip}{row}")
    #             pass


    #         # Write the combined formatted rows to the DNC_INPUT file
    #         try:
    #             with open(paths["DNCINPUT"] + "DNCinput.txt", mode='w', newline='') as file:
    #                 for row in rowData:
    #                     file.write(f"{formatted_ip}{row}\n")  # Join list elements with newlines and write to the file
    #         except Exception as e:
    #             # messagebox.showerror("Error3", f"An error occurred while writing to DNC_INPUT: {e}")
    #             print("Error3", f"An error occurred while writing to DNC_INPUT: {e}")

    #         import subprocess

    #         program_path = paths["DNCSchnittstelle"]
    #         # Running the program
    #         try:
    #             result = subprocess.run([program_path], check=True)
    #             print(f"Program executed successfully with return code: {result.returncode}")
    #         except subprocess.CalledProcessError as e:
    #             print(f"Program failed with return code: {e.returncode}")
    #         except FileNotFoundError:
    #             print("The specified program was not found.")

    #         for dictionary in places :

    #             if dictionary["status"]=="machine":
    #                 machines.append(dictionary["placename"])
        

    #         try:
    #             # Connect to the MTMDB.db SQLite database
    #             conn = sqlite3.connect(paths["MTMDB"])
    #             cur = conn.cursor()

    #             # Fetch all data from the currentTools table
    #             cur.execute("SELECT * FROM currentTools")
    #             currentToolsData = cur.fetchall()

    #             platzierte_rows = []
    #             found = False

    #             # Iterate over the rows and update the 'platz' field if idCode is found in movingTools
    #             for row in currentToolsData:
    #                 if row[0] in self.movingTools:
    #                     row = list(row)  # Convert tuple to list to modify
    #                     row[19] = combobox_value  # Update the 'platz' field
    #                     platzierte_rows.append(row)
    #                     found = True

    #             # Update the database if any matching rows were found
    #             if found:
    #                 for row in platzierte_rows:
    #                     cur.execute("""
    #                         UPDATE currentTools
    #                         SET platz = ?
    #                         WHERE idCode = ?
    #                     """, (row[19], row[0]))
    #                 # Commit the changes after all updates are done
    #                 conn.commit()
    #                 # Ensure the connection is closed properly
    #                 conn.close()


    #                 for i in platzierte_rows:
    #                     if i[1]!=None:
    #                         with open(paths["TNC640_Daten"], mode='r', newline='') as file:
    #                             reader = csv.reader(file, delimiter=";")
    #                             for PlcDataRow in reader:
    #                                 if PlcDataRow[0]==i[0]:
    #                                     self.addToolRowInToolTableBackup(i,PlcDataRow)
    #                                     if self.addToolRowInToolTableBackup:
    #                                         # messagebox.showinfo("Success", f"Das Werkzeug {i[2]} ist erfolgreich platziert\nUND\nsie wurde in toolTableBackup eingetragen")
    #                                         print("Success", f"Das Werkzeug {i[2]} ist erfolgreich platziert\nUND\nsie wurde in toolTableBackup eingetragen")

    #                                         break
    #                                     else:
    #                                         # messagebox.showerror("Error", f"Das Werkzeug {i[2]} konnte nicht platziert werden!!\nMÖGLICHE FEHLER\n-falsche Daten\nOder\nfehledne PLC-TT-Daten")
    #                                         print("Error", f"Das Werkzeug {i[2]} konnte nicht platziert werden!!\nMÖGLICHE FEHLER\n-falsche Daten\nOder\nfehledne PLC-TT-Daten")

    #                                 else:
    #                                     continue
    #                     else:
    #                         continue
                            

    #             else:
    #                 # messagebox.showerror("Erro3", "Die Aufnahme ist nicht registriert")
    #                 print("Erro3", "Die Aufnahme ist nicht registriert")
            
    #         except Exception as e:
    #             # messagebox.showerror("Erro4", f"An error occurred: {e}")
    #            print("Erro4", f"An error occurred: {e}")
    #         return 0

    # def machineStatus(self, combobox_value):
    #         # Function to check if IP is reachable using Tnccmd
    #         def ping_ip(ip):
    #             try:
    #                 # Full path to Tnccmd (adjust the path if necessary)
    #                 tnccmd_cmd = paths["TNCcmd"]
    #                 output = subprocess.run(
    #                     [tnccmd_cmd, "ping", ip],
    #                     stdout=subprocess.PIPE, stderr=subprocess.PIPE
    #                 )
    #                 return output.returncode == 0  # Return True if ping was successful
    #             except Exception as e:
    #                 print(f"Error pinging {ip} with Tnccmd: {e}")
    #                 return False

    #         # Finding the corresponding IP for the given place (combobox_value)
    #         for place in places:
    #             if place["placename"] == combobox_value:
    #                 ip_address = place["link"]

    #         # Check the ping status and return the result
    #         return ping_ip(ip_address)