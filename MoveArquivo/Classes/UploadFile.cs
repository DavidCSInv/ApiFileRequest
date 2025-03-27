using Renci.SshNet;

namespace MoveArquivo.Classes
{
    public class UploadFile
    {
        public void FileUpload()
        {
            var host = "your.sftp.host";
            var port = 22;
            var username = "your.username";
            var password = "your.password";

            var client = new SftpClient(host, port, username, password);
            client.Connect();
        }
    }
}
