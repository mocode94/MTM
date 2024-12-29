import subprocess
import time
import threading
from queue import Queue, Empty
from config import paths,places
import sqlite3





def monitor_output(process, output_queue):
    """
    Continuously read the output of the process and add it to the queue.
    """
    try:
        for line in iter(process.stdout.readline, ''):
            output_queue.put(line.strip())
    except Exception as e:
        print(f"Error reading process output: {e}")

for place in places:
    if place["status"] == "machine":
        ip_address = place["link"]
        place_name = place["placename"]
        # Path to the TNCCmd executable
        tnc_cmd_path = paths["TNCcmd"]  # Replace with the actual path

        try:
            print(f"Connecting with {place_name} with Ip Address : {ip_address}.....")

            # Start the TNCCmd process
            process = subprocess.Popen(
                tnc_cmd_path,
                stdin=subprocess.PIPE,
                stdout=subprocess.PIPE,
                stderr=subprocess.PIPE,
                text=True
            )

            # Allow the program to start
            time.sleep(1)

            # Monitor process output using a queue
            output_queue = Queue()
            output_thread = threading.Thread(target=monitor_output, args=(process, output_queue))
            output_thread.daemon = True
            output_thread.start()

            # Send the 'connect' command
            process.stdin.write("connect\n")
            process.stdin.flush()
            time.sleep(1)

            # Send the 'i' command
            process.stdin.write("i\n")
            process.stdin.flush()
            time.sleep(1)

            # Send the IP address
            process.stdin.write(f"{ip_address}\n")
            process.stdin.flush()
            time.sleep(1)

            start_time = time.time()
            connected = False

            while time.time() - start_time < 5:  # Wait up to 5 seconds
                try:
                    line = output_queue.get(timeout=0.1)
                    print(line)  # Print output for debugging purposes

                    if "Connection established" in line:
                        connected = True
                        print(f"Connection to {place_name} with Ip Address: {ip_address} established.")
                        break
                    elif "abort with ESC!" in line:
                        print(f"Connection to {place_name} with Ip Address: {ip_address} in progress...")
                except Empty:
                    continue

            if not connected:
                print(f"Connection to {ip_address} timed out. Aborting...")
                process.stdin.write("\x1b\n")  # Send ESC key
                process.stdin.flush()
                process.terminate()
                continue

            # Proceed with file operations if connected
            print(f"Successfully connected to {place_name} with Ip Address: {ip_address}. Proceeding with file operations...")

            # Get tool.t
            process.stdin.write(f"get TNC:\\table\\tool.t X:\\Projekt\\{ip_address}.t\n")
            process.stdin.flush()
            time.sleep(1)

            # Get tool_p.tch
            process.stdin.write(f"get TNC:\\table\\tool_p.tch X:\\Projekt\\{ip_address}.tch\n")
            process.stdin.flush()
            time.sleep(1)

            # Close the stdin to signal end of input
            process.stdin.close()

            # Read and print the output (optional)
            output, error = process.communicate()
            # print("Output:\n", output)
            print("Error:\n", error if error else "No errors.")

        except FileNotFoundError:
            print("Error: TNCCmd executable not found. Please check the path.")
            break
        except Exception as e:
            print(f"An error occurred: {e}")
            break


        # Path to the generated .tch file
        tch_file_path = f"X:\\Projekt\\{ip_address}.tch"

        # List to store the extracted data from .tch
        tchData = []

        try:
            # Open the .tch file for reading
            with open(tch_file_path, 'r') as tch_file:
                # Skip the first two lines
                next(tch_file)
                next(tch_file)

                # Process the remaining lines
                for line in tch_file:
                    # Extract columns based on fixed widths
                    tchCol1 = line[:8].strip()  # First column (ignored)
                    tchCol2 = line[8:14].strip()  # Second column
                    tchCol3 = line[14:46].strip()  # Third column

                    # Skip lines where the third column is empty
                    if tchCol3:
                        tchData.append((tchCol2, tchCol3))

            print("Extracted Data from .tch:", tchData ,"\n")

        except FileNotFoundError:
            print(f"Error: File {tch_file_path} not found.")
            break
        except Exception as e:
            print(f"An error occurred: {e}")
            break

        # Path to the generated .t file
        t_file_path = f"X:\\Projekt\\{ip_address}.t"

        # List to store the extracted data from .t
        tData = []

        try:
            # Open the .t file for reading
            with open(t_file_path, 'r') as t_file:
                # Skip the first two lines
                next(t_file)
                next(t_file)

                # Process the remaining lines
                for line in t_file:
                    # Extract columns based on fixed widths
                    tCol1 = line[:8].strip()  # First column
                    tCol2 = line[8:40].strip()  # Second column
                    tCol3 = line[40:52].strip()  # Third column
                    tCol4 = line[52:64].strip()  # Fourth column

                    # Check if the first and second columns match any entry in tchData
                    if any(tCol1 == tch[0] and tCol2 == tch[1] for tch in tchData):
                        tData.append((tCol1, tCol2, tCol3, tCol4))

            print("Extracted Data from .t:", tData)

        except FileNotFoundError:
            print(f"Error: File {t_file_path} not found.")
            break
        except Exception as e:
            print(f"An error occurred: {e}")
            break


        # Connect to the MTMDB.db SQLite database
        conn = sqlite3.connect(paths["MTMDB"])
        cur = conn.cursor()

        try:
            counter=0
            for t in tData:
                toolNummer, toolName, toolIstLaenge, toolRadius = t

                # Debugging: Check what we're comparing
                # print(f"Comparing toolNummer: {toolNummer}, toolName: {toolName}, platz: {place_name}")

                # Debugging: SELECT query to check existing rows
                cur.execute("""
                    SELECT * FROM currentTools
                    WHERE LOWER(TRIM(toolNummer)) = LOWER(?) 
                    AND LOWER(TRIM(toolName)) = LOWER(?) 
                    AND LOWER(TRIM(platz)) = LOWER(?)
                """, (toolNummer.strip(), toolName.strip(), place_name.strip()))

                rows = cur.fetchall()
                if rows:
                    print(f"Matching rows for {toolNummer}, {toolName}, {place_name}: {rows}")
                    counter=counter+1
                else:
                    # print(f"No matching rows for {toolNummer}, {toolName}, {place_name}")
                    pass

                # Execute the UPDATE query
                cur.execute("""
                    UPDATE currentTools
                    SET toolRadius = ?,
                        toolIstLaenge = ?
                    WHERE LOWER(TRIM(toolNummer)) = LOWER(?) 
                    AND LOWER(TRIM(toolName)) = LOWER(?) 
                    AND LOWER(TRIM(platz)) = LOWER(?)
                """, (toolRadius, toolIstLaenge, toolNummer.strip(), toolName.strip(), place_name.strip()))

            conn.commit()
            print(f"Updated {counter} rows in the database for {place_name}.")
        except sqlite3.Error as e:
            print(f"An error occurred while updating the database: {e}")

        finally:
            # Close the connection
            conn.close()

    else:
        pass

