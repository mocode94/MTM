========================================================================
JHIOsim DLL DEMO APPLICATION: IOSimDemo
========================================================================

This application allows you to examine the functionality of the JHIOsim DLL

You will find the access functions to the JHIOsim DLL in the file 
IOSimDemoDlg.cpp


To compile this test software with Visual Studio 2010 you must:
---------------------------------------------------------------
- Provide a drive r:
  You can use the subst command to assign the drive letter r: to a local folder.
  Example: subst R: C:\IOSimDemo\

- Copy the deployed import library JHIOsim64.lib
  from C:\Program Files (x86)\HEIDENHAIN\SDK\JHIOsim\lib directories
  to r:\x64\bin and r:\x64\bind respectively

- Copy the deployed include files from C:\Program Files (x86)\HEIDENHAIN\SDK\JHIOsim\lib\includes 
  to  C:\CommonJHCN\includes\IOsim

To execute this test software (debug mode) with Visual Studio 2010 you must:
- Copy the DLL C:\Program Files (x86)\HEIDENHAIN\SDK\JHIOsim\Bin\X64\JHIOsim64.dll to the executable path r:\x64\bind