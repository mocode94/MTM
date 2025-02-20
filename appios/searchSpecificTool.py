import sqlite3
from config import paths
import sys

# Function to search for the tool based on toolID
def search_tool(toolID):


    # Convert toolID to uppercase for case-insensitive comparison
    toolID = toolID.upper()

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
        # Convert row[2] to uppercase for case-insensitive comparison
        row_tool_name = row[2].upper() if row[2] else ""

        # Match exactly or if the toolID is a substring of row[2]
        if toolID == row_tool_name or toolID in row_tool_name:
            results.append(
                f"T: {row[1]} - Name: {row_tool_name} D: {row[4]} - L: {row[10]} Platz: {row[19]}\n_________________\n"
            )

    return results

# Main code execution
if __name__ == "__main__":
    print(sys.argv[1])
    # Ensure a toolID argument is provided
    if len(sys.argv) > 1:
        tool_id = sys.argv[1]  # Get the first argument passed to the script


        # Call the function with the toolID
        results = search_tool(tool_id)

        # Print the results or a "not found" message
        if results:
            for result in results:
                print(result)
        else:
            print("No matching tools found.")
    else:
        print("No tool ID provided.")
