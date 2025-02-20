import sqlite3
import sys
from config import paths

# Get parameters from the command-line arguments
diameter = float(sys.argv[1])
typ = int(sys.argv[2])
tiefe = float(sys.argv[3])



# diameter = float(4)
# typ = int(9)
# tiefe = float(30)


# Connect to the MTMDB.db SQLite database
conn = sqlite3.connect(paths["MTMDB"])
cur = conn.cursor()

# Fetch all data from the currentTools table
cur.execute("SELECT * FROM currentTools")
currentToolsData = cur.fetchall()
conn.close()

# Filter the rows based on the input parameters
results = []
for row in currentToolsData:
    if row[3]==typ:
        if row[3] == typ == 10 :
                if float(row[4]) <= float(diameter) and float(row[10]) >= float(tiefe):
                    results.append(f"Ts: {row[1]} - Name: {row[2]}\n D: {row[4]} - L: {row[10]} Platz: {row[19]}\n_________________\n")
        elif row[3] == typ == 9:
                if float(row[4]) <= float(diameter) and float(row[10]) >= float(tiefe):
                    results.append(f"T: {row[1]} - Name: {row[2]}\n D: {row[4]} - L: {row[10]} Platz: {row[19]}\n_________________\n")
        elif row[3] == typ ==1 or row[3] == typ ==2 or row[3] == typ ==3:
            # if row[3] == typ :
            #     if typ == 1 or typ == 2 or typ == 3:
                    if row[4] == (diameter/2) and row[10] >= tiefe:
                        results.append(f"T: {row[1]} - Name: {row[2]}\n D: {row[4]} - L: {row[10]} Platz: {row[19]}\n_________________\n")
            # elif float(row[4]) <= (diameter) and row[10] >= tiefe:
            #     results.append(f"T: {row[1]} - Name: {row[2]}\n D: {row[4]} - L: {row[10]} Platz: {row[19]}\n_________________\n")

# Print the results
for result in results:
    print(result)
