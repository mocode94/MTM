using System;
using System.Runtime.InteropServices;

namespace ConnectRequest4
{
    class Program
    {
        static void Main(string[] args)
        {

            HeidenhainDNCLib.JHMachine machine = null;
            HeidenhainDNCLib.IJHConnection3 connection = null;

            // Use a secure password as this is the key for the private key file.
            // Optional: Use a CONNECTIONLib reference to generate a random password.
            // Make sure that you store the password in an appropriate way if you intend to re-use the key file.

            //CONNECTIONLib.Connection conobject;
            //CONNECTIONLib.IJHSecureConnectionHelper helper;
            //int passwordLength = 20;

            //conobject = new CONNECTIONLib.Connection();
            //helper = (CONNECTIONLib.IJHSecureConnectionHelper)conobject;

            //string SshIdentPassword = helper.newRandomPassword[passwordLength];

            string SshIdentPassword = ",f]+vPF2XY?SuuTp";

            // Hostname can be your hostname OR an IP-address.

            string hostname = "hostname";

            // User as defined on the control (user-administration)
            // If user management is inactive (legacy-mode) and you want to use a ssh connection you can use user:user (username:password) for login credentials

            string remoteuser = "username";

            try
            {

                machine = new HeidenhainDNCLib.JHMachine();

                // 1. Create a connection object using a cast on the machine object

                connection = (HeidenhainDNCLib.IJHConnection3)machine;


                // 2. Configure the new connection for given control type and protocol

                connection.Configure(HeidenhainDNCLib.DNC_CNC_TYPE.DNC_CNC_TYPE_TNC6xx_NCK, HeidenhainDNCLib.DNC_PROTOCOL.DNC_PROT_RPC_SECURE);

                // 3. Set property values for the connection object

                connection.propertyValue[HeidenhainDNCLib.DNC_CONNECTION_PROPERTY.DNC_CP_HOST] = hostname;
                connection.propertyValue[HeidenhainDNCLib.DNC_CONNECTION_PROPERTY.DNC_CP_SSH_REMOTE_USER] = remoteuser;
                connection.propertyValue[HeidenhainDNCLib.DNC_CONNECTION_PROPERTY.DNC_CP_SSH_IDENT] = connection.bstrDefaultKeysDir + $"/{remoteuser}@{hostname}_rpc";
                connection.propertyValue[HeidenhainDNCLib.DNC_CONNECTION_PROPERTY.DNC_CP_SSH_IDENT_PASSWORD] = SshIdentPassword;

                // 4. Read the host key from the remote host and store result in the property. 

                connection.GetHostKeyToProperty();

                Console.WriteLine("Remote fingerprint: " + connection.ConnectionProperty[HeidenhainDNCLib.DNC_CONNECTION_PROPERTY.DNC_CP_SSH_REMOTE_HOST_KEY_FINGERPRINT].value);
                Console.WriteLine("SSH path: " + connection.ConnectionProperty[HeidenhainDNCLib.DNC_CONNECTION_PROPERTY.DNC_CP_SSH_IDENT].value);


                // 5. Create a new SSH key pair with preset name and store results in the given keys directory. 

                connection.GenerateKeyPairToProperties(connection.bstrDefaultKeysDir, 1, SshIdentPassword);

                // 6. Transfer to and register the public key on the remote host for the defined user. 

                connection.RegisterKeyFromProperties();

                // 7. Connect using created connection object

                // If you want to re-use an existing connection, leave out steps 5 and 6.

                machine.ConnectRequest4(connection);

                Console.WriteLine("Connection established...");
                Console.WriteLine($"Machine state: {machine.GetState()}");

                machine.Disconnect();
            }

            catch (COMException cex)
            {
                Console.WriteLine("Com ERROR: " + cex.Message.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message.ToString());
            }
            finally
            {

                // 8. Release COM-Objects

                if (connection != null) Marshal.ReleaseComObject(connection);

                if (machine != null) Marshal.ReleaseComObject(machine);

                Console.WriteLine("Objects released + connection was set-up");
                Console.ReadKey();

            }
        }
    }
}
