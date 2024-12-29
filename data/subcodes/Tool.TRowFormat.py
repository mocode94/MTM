# import os
# import fileinput

# def format_column(value, length):
#     """Format the value to a fixed length by padding with spaces on the right."""
#     return value.ljust(length)[:length]

# def process_line(line, t_nr, column_lengths, new_r,col47):
#     """Replace old_name with new_name in a line, preserving column widths."""
#     # Extract the existing columns
#     columns = []
#     start = 0
#     for length in column_lengths:
#         end = start + length
#         columns.append(line[start:end].strip())
#         start = end

#     # Find and replace the old name
#     if columns[0].strip() == t_nr:
#         columns[3] = new_r
#         columns[47]=col47
#         print(columns)

#     # Format the columns according to the fixed lengths
#     new_line = ''.join(format_column(col, length) for col, length in zip(columns, column_lengths))
#     return new_line

# def update_file(filename, t_nr, new_r,col47):
#     # Define the column lengths as provided
#     column_lengths = [
#         8, 32, 12, 12, 12, 10, 10, 10, 3, 8, 6, 6, 9, 9, 32, 10, 12, 8, 4, 7, 7, 7, 7, 12, 12, 7, 7, 10, 8, 7, 8, 20, 
#         5, 8, 10, 7, 7, 7, 7, 7, 7, 13, 4, 8, 9, 10, 14, 20, 16, 8, 30, 32, 20
#     ]

#     # Create a temporary file
#     temp_filename = filename + '.tmp'
    
#     with open(filename, 'r') as file, open(temp_filename, 'w') as temp_file:
#         for line in file:
#             if line[:8].strip() == t_nr:
#                 new_line = process_line(line, t_nr, column_lengths, new_r,col47)
#                 temp_file.write(new_line + '\n')
#             else:
#                 temp_file.write(line)

#     # Replace the original file with the modified temporary file
#     os.replace(temp_filename, filename)

# # Example usage
# filename = r"C:\Users\Mo_ab\Desktop\tool.t"
# t_nr = '33'
# new_r = "+3"
# col47="aufnahme"

# update_file(filename, t_nr, new_r,col47)








import os

def format_column(value, length):
    """Format the value to a fixed length by padding with spaces on the right."""
    return value.ljust(length)[:length]

def process_line(line,  column_lengths, row):
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
        columns[2]=row[2]
        columns[3] = row[3]
        columns[4] = row[4]
        columns[12]=row[12]
        columns[13] = row[13]
        columns[14] = row[14]
        columns[16]=row[16]
        columns[17] = row[17]
        columns[18] = row[18]
        columns[30]=row[30]
        columns[34] = row[34]
        columns[41] = row[41]
        columns[44]=row[44]
        columns[48] = row[48]

        print(f"Updated line: {columns}")

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
    
    print(f"Created new line: {new_line}")
    return new_line

def update_file(filename,row):
    # Define the column lengths as provided
    column_lengths = [
        8, 32, 12, 12, 12, 10, 10, 10, 3, 8, 6, 6, 9, 9, 32, 10, 12, 8, 4, 7, 7, 7, 7, 12, 12, 7, 7, 10, 8, 7, 8, 20, 
        5, 8, 10, 7,7, 7, 7, 7, 7, 7, 10, 4, 8, 9, 10, 10, 20, 16, 8, 30, 32, 20
    ]

    # Create a temporary file
    temp_filename = filename + '.tmp'

    with open(filename, 'r') as file:
        lines = file.readlines()

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
            print(f"Inserting new line after index {nearest_index} (line_nr: {lines[nearest_index][:8].strip()})")
            lines.insert(nearest_index + 1, new_line + '\n')
        else:
            print("Appending new line at the end")
            lines.append(new_line + '\n')

    with open(temp_filename, 'w') as temp_file:
        temp_file.writelines(lines)

    # Replace the original file with the modified temporary file
    os.replace(temp_filename, filename)
# t_row=["0016","276","D4_R0.4_TORUS_STAHL","23","4","0.4","0","2.5","3","20","23","103","103","0","206357 4 0.4","308172 6","","309880 63","HSK63","35","110","C32"]
t_row=["0013","173","VHM_BOHRER_D3.7_M4","1","3.7","0","140","0","1","25","32","152","152","0","122715 3.7","308192 6","","309880 63","HSK63","35","160","C42"]


# Example usage
filename = rf"C:\Users\Mo_ab\Desktop\Neuer Ordner (3)\{t_row[21]}.t"
t_nr = t_row[1]
t_name=t_row[2]
t_l=f"+{t_row[11]}"
t_r = f"+{t_row[4]}"
t_r2=f"+{t_row[5]}"
t_typ=t_row[3]
t_doc=f"{t_row[14]} + {t_row[15]}"
t_lcut=f"+{t_row[9]}"
t_cut=t_row[8]
t_angle=f"+{t_row[7]}"
t_tangle=f"+{t_row[6]}"
t_p1=t_row[19]
t_p8=t_row[20]
t_pitch=f"+{t_row[13]}"
t_kinematic=f"{t_row[15].replace(' ', '_')}.CFG"

# T       NAME                            L           R           R2          DL        DR        DR2       TL RT      TIME1 TIME2 CUR_TIME TYP      DOC                             PLC       LCUTS       ANGLE   CUT LTOL   RTOL   R2TOL  DIRECT R-OFFS      L-OFFS      LBREAK RBREAK NMAX      LIFTOFF TP_NO  T-ANGLE LAST_USE            PTYP PLC-VAL P1        P2     P3     P4     P5     P6     P7     P8     AFC       ACC PITCH   AFC-LOAD AFC-OVLD1 AFC-OVLD2 KINEMATIC           DR2TABLE        OVRTIME INV-NR                        TMAT                            CUTDATA             
# t_nr    t_name                          t_l         t_r         t_r2                                                                      t_typ    t_doc                                     t_lcut      t_angle t_cut                                                                                          t_tangle                                 t_p1                                                t_p8                 t_pitch                              t_kinematic
                                                                                                                                                                                                                                                                                                                                                                                                            #    7,      13,      4,   8,        9,        10,     14,        20,              16,           8      30,                           32                               , 20
# 41                                      +0          +0          +0          +0        +0        +0        0          0     0     0        99                                       %00000000 +0          +0      0   0      0      0      -1     +0          +0          0      0                0              +0                          0    0       0         0      0      0      0      0      0      0                0   +0                                                                                                                                                                 
row=[t_nr , t_name , t_l, t_r , t_r2 ,"+0" ,"+0","+0","0","","0","0","0",t_typ,t_doc,"%00000000",t_lcut,t_angle,t_cut,"0","0","0","-1","+0","+0","0","0","","0","",t_tangle,"","0","0",t_p1,"0","0","0","0","0","0",t_p8,"","0",t_pitch,"","","",t_kinematic,"","","","",""]
update_file(filename,row)



# # T       NAME                            L           R           R2          DL        DR        DR2       TL RT      TIME1 TIME2 CUR_TIME TYP      DOC                             PLC       LCUTS       ANGLE   CUT LTOL   RTOL   R2TOL  DIRECT R-OFFS      L-OFFS      LBREAK RBREAK NMAX      LIFTOFF TP_NO  T-ANGLE LAST_USE            PTYP PLC-VAL P1        P2     P3     P4     P5     P6     P7     P8     AFC       ACC PITCH   AFC-LOAD AFC-OVLD1 AFC-OVLD2 KINEMATIC           DR2TABLE        OVRTIME INV-NR                        TMAT                            CUTDATA             

#   8,       32,                           12,           12,         12,        10,        10,       10,      3, 8,        6,     6,     9,     9,      32,                             10,       12,         8,      4,   7,    7,      7,     7,     12,         12,         7,     7,    10,         8,     7,      8,       20,              5,    8,    10,             7,      7,    7,     7,      7,      7,    10,       4,    8,      9,      10,       10,      20,                    16,            8,      30,                         32,                               20

