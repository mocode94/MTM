The PLC minimum program is intended to demonstrate the functionality of the 
HEIDENHAIN DNC interface IJHPlc by using the RemoTools SDK Demo application.

You can integrate the communication module in the existing PLC program of an
iTNC 530 control or exchange the complete PLC program of an 
iTNC 530 programming station.

Information how to integrate the communication module can be found in the source code of the
file communication.src

Follow these steps to activate the PLC minimum program on an running iTNC 530 programming station:

A) Transmit the files to drive PLC:\
   1) Start the iTNC 530 Programming Station
   2) Start TNCremo on your PC
   3) Enter the connection parameters with command File > Configure connections
   4) Connect to the control with command File > Connect
   5) Change the current directory on the control with command Folder > Change folder/drive
      to PLC: (you will be asked for the key number 807667)
   6) Make a backup copy of file OEM.SYS  with command File > Save as
   7) Transmit the files minimum.plc, minimum.pet and oem.sys with File > Transmit

B) Configure Control
   1) Press the MOD key in Edit Mode
   2) Enter key number 95148
   3) Search MP 7210 with the GOTO key
   4) Check for value 1 (control loop inactive; PLC active)
   5) Search MP 7480 with the GOTO key
   6) Check for value 0 (tool call processing inactive),
   7) Press the end key

C) Reset the control
   1) If you changed one of the above parameters, the control restarts
      automatically; if not: switch to the manual mode with the Manual key
      Press the OFF soft key

D) Cleanup  
  1) If you don't need the functionality of the PLC minimum program any more
     simply replace the file OEM.SYS on PLC:\ by the backup copy made in Step A) 6)
     