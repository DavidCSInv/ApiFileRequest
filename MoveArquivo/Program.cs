using MoveArquivo.Classes;


#region Parametros de iniciamento
Render render = new();

var filePaterns = new Dictionary<int, string>
{
    {1,"*Delta*" },
    {2,"*Inter*" }
};

var directoryOrig = @$"P:";

var fileDest = @"C:\Carga\";
var directoryCopy = @"C:\Carga\Copia";

#endregion
Render.RenderScreen();
int option = int.Parse(Console.ReadLine());

#region Option chooser
switch (option)
{
    case 1:
        render.UploadMinasGerais();
        break;
    case 2:
        render.UploadEspiritoSantos();
        FileMove(directoryOrig, fileDest, directoryCopy, filePaterns[1], 1);
        break;
    case 3:
        render.UploadMGxES();
        FileMove(directoryOrig, fileDest, directoryCopy, filePaterns[2], 2);
        break;
    case 4:
        render.UploadESxMG();
        FileMove(directoryOrig, fileDest, directoryCopy, filePaterns[2], 2);
        break;
    default:
        Render.RenderScreen();
        break;
}
#endregion

void FileMove(string directoryOrig, string FileDest, string directoryCopy, string filePaterns, int indFile)
{
    string[] filesGeneric = Directory.GetFiles(directoryOrig, $"{filePaterns}.xlsx", SearchOption.AllDirectories);
    Console.WriteLine("Procurando arquivo...");

    var fileNewPatter = new Dictionary<int, string>
    {
       {1,"e11" },
       {2,"tinter01" }
    };

    if (filesGeneric.Length > 0)
    {
        try
        {
            Console.WriteLine("Arquivo encontrando movendo...");

            foreach (string fileOrig in filesGeneric)
            {
                string fileCopy;
                int copyNumber = 1;

                File.Copy(fileOrig, fileDest, true);
                string fileBaseName = fileNewPatter.ContainsKey(1) ? fileNewPatter[1] : fileNewPatter[2]; // Ajuste aqui para garantir que sempre tenha um valor válido

                if (!Directory.Exists(directoryCopy))
                {
                    Directory.CreateDirectory(directoryCopy);
                }

                do
                {
                    fileCopy = Path.Combine(directoryCopy, $"{fileNewPatter}_{copyNumber}.xlsx");
                    copyNumber++;
                } while (File.Exists(fileCopy));

                File.Copy(fileOrig, fileCopy);
                File.Delete(fileOrig);
                Console.WriteLine("Arquivo ovido com sucesso");
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