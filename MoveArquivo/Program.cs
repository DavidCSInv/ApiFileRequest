using MoveArquivo.Classes;


#region Parametros de iniciamento
Render render = new();

var filePaterns = new Dictionary<int, string>
{
    {1,"*Delta*" },
    {2,"*Inter*" }
};

var directoryOrig = @$"P:";
var delta = "e11.xlsx";
var inter = "tinter01.xlsx";

var fileDest = @"C:\Carga\";
var directoryCopy = @"C:\Carga\Copia";

#endregion
Render.RenderScreen();
int option = int.Parse(Console.ReadLine());

switch (option)
{
    case 1:
        render.UploadMinasGerais();
        break;
    case 2:
        render.UploadEspiritoSantos();
        FileUpload(directoryOrig, fileDest, directoryCopy, filePaterns[1]);
        break;
    case 3:
        render.UploadMGxES();
        FileUpload(directoryOrig, fileDest, directoryCopy, filePaterns[2]);
        break;
    case 4:
        render.UploadESxMG();
        FileUpload(directoryOrig, fileDest, directoryCopy, filePaterns[2]);
        break;
}

void FileUpload(string directoryOrig, string FileDest, string directoryCopy, string filePaterns)
{
    string[] filesGeneric = Directory.GetFiles(directoryOrig, $"{filePaterns}.xlsx", SearchOption.AllDirectories);

    if (filesGeneric.Length > 0)
    {
        try
        {
            foreach (string fileOrig in filesGeneric)
            {
                string fileCopy;
                int copyNumber = 1;

                File.Copy(fileOrig, fileDest, true);

                do
                {
                    if (!Directory.Exists(directoryCopy))
                    {
                        Directory.CreateDirectory(directoryCopy);
                    }

                    fileCopy = Path.Combine(directoryCopy, $"Delta{copyNumber}.xlsx");
                    copyNumber++;
                } while (File.Exists(fileCopy));

                File.Copy(fileOrig, fileCopy);
                File.Delete(fileOrig);
                Console.WriteLine("Arquivo Movido com Sucesso");
            }

        }

        catch (Exception ex)
        {
            throw new ApplicationException("Erro: algo não correu como o planejado", ex);
        }
    }
    else
    {
        Console.WriteLine("Não há arquivos com este nome");
    }
}