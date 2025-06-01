using Renci.SshNet;

namespace ApiFileRequest.Classes
{
    public class UploadFile
    {
        public Task FileUploadAsync(string filePath, string patter, CancellationToken cancellationToken)
        {
            var host = "999.999.999.999";                                   // IP Address
            var port = 22;                                                  // SSH Port - Obs:The deafult port is 22
            var username = "";                                              // username
            var password = "";                                              // password
            var remotePath = $"/folder0/folder1/folder2/{patter}.xlsx";     // Remote destination path


            //This catch will connect to a linux server and upload a file to it,always overwriting it, and after that will disconect
            try
            {
                var client = new SftpClient(host, port, username, password);

                Console.WriteLine("Connecting to SFTP server...");
                client.Connect();

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    Console.WriteLine("Connection established. Starting file upload..");

                    client.UploadFile(fileStream, remotePath);

                    Console.WriteLine("File upload completed successfully.");
                }

                Console.WriteLine("Disconnecting from SFTP server...");
                client.Disconnect();
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Failed to establish connection to the server.");
            }

            return Task.CompletedTask;
        }
    }
}
