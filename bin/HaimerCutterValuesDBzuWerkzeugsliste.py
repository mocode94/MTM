# i have 2 csv files 1. output.csv 2. TEDI.csv.
# i want to read both of them then search for the output file column 1 in the TEDI file column 1  
# if it found control TEDI column 4 
# if its value == 1 put the value from table output column 3 in TEDI column 12 .
# but if column 4 in TEDI !=1 put the value of column 2 from output in TEDI column 5 and column 3 from output in TEDI column 12 



import pandas as pd
import numpy as np
from config import paths

# Read the CSV files into DataFrames as strings
output_df = pd.read_csv(paths["haimerDB"], dtype=str)
tedi_df = pd.read_csv(paths["haimerBackup"], dtype=str, delimiter=';', header=None)

# Diagnostics: Print shapes and columns
print("Output DataFrame Shape:", output_df.shape)
print("TEDI DataFrame Shape:", tedi_df.shape)
print("TEDI DataFrame Columns:", tedi_df.columns)

# Make sure to handle sparse data
# Convert only existing columns to the correct data types, handling errors
output_df[output_df.columns[0]] = pd.to_numeric(output_df[output_df.columns[0]], errors='coerce').astype('Int64')
output_df[output_df.columns[1]] = pd.to_numeric(output_df[output_df.columns[1]], errors='coerce')
output_df[output_df.columns[2]] = pd.to_numeric(output_df[output_df.columns[2]], errors='coerce')

# Check if tedi_df has the required columns
if len(tedi_df.columns) < 12:
    print("Warning: TEDI DataFrame does not have at least 12 columns. Adjusting indexing strategy.")

# Apply type conversion only if columns exist
if len(tedi_df.columns) > 0:
    tedi_df[tedi_df.columns[0]] = pd.to_numeric(tedi_df[tedi_df.columns[0]], errors='coerce').astype('Int64')
if len(tedi_df.columns) > 3:
    tedi_df[tedi_df.columns[3]] = pd.to_numeric(tedi_df[tedi_df.columns[3]], errors='coerce').astype('Int64')
if len(tedi_df.columns) > 4:
    tedi_df[tedi_df.columns[4]] = pd.to_numeric(tedi_df[tedi_df.columns[4]], errors='coerce')
if len(tedi_df.columns) > 11:
    tedi_df[tedi_df.columns[11]] = pd.to_numeric(tedi_df[tedi_df.columns[11]], errors='coerce')

# Track if any updates are made
updates_made = False

# Iterate through each row of the output DataFrame
for index, output_row in output_df.iterrows():
    # Get the value of the first column of the output row using .iloc[]
    search_value = output_row.iloc[0]
    
    # Find rows in the TEDI DataFrame where column 1 matches the search value
    matching_rows = tedi_df[tedi_df.iloc[:, 0] == search_value]

    # If there are matching rows, iterate through them
    if not matching_rows.empty:
        for tedi_index, tedi_row in matching_rows.iterrows():
            if len(tedi_row) > 3 and tedi_row.iloc[3] == 1:  # Check if the value in column 4 of TEDI is 1
                if len(tedi_row) > 11:
                    # Update column 12 with the value from column 3 of output
                    tedi_df.at[tedi_index, tedi_df.columns[11]] = output_row.iloc[2]
                    updates_made = True
                    print(f"Updated column 12 at row {tedi_index} with {output_row.iloc[2]}")
            else:
                if len(tedi_row) > 4:
                    # Update column 5 with the value from column 2 of output
                    tedi_df.at[tedi_index, tedi_df.columns[4]] = output_row.iloc[1]
                if len(tedi_row) > 11:
                    # Update column 12 with the value from column 3 of output
                    tedi_df.at[tedi_index, tedi_df.columns[11]] = output_row.iloc[2]
                updates_made = True
                print(f"Updated column 5 at row {tedi_index} with {output_row.iloc[1]}")
                print(f"Updated column 12 at row {tedi_index} with {output_row.iloc[2]}")
    else:
        print(f"No matching row found in TEDI for search value: {search_value}")

# Save the modified TEDI DataFrame back to a CSV file if updates were made
if updates_made:
    tedi_df.to_csv(r'E:\haimer 30_08_24\Bin\TEDI_modified.csv', index=False, sep=';')
    print("TEDI file has been updated and saved as 'TEDI_modified.csv'")
else:
    print("No updates were made to the TEDI file.")
