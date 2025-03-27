using MoveArquivo.Classes;

// Só eu e Deus entendiamos esse codigo, agora só Deus.

#region Parametros de iniciamento
Render render = new();

var filePaterns = new Dictionary<int, string>
{
    {1,"*Delta*" },
    {2,"*Inter*" }
};

var directoryOrig = @$"P:";
var directoryCopy = @"C:\Carga\CargaCopia";
var fileDest = @"C:\Carga\";

#endregion

while (true)
{
    try
    {
        Console.Clear();
        Render.RenderScreen();
        int option = int.Parse(Console.ReadLine());
        #region Option chooser
        switch (option)
        {
            case 1:
                render.UploadMinasGerais();
                string produtos = Console.ReadLine();
                Thread.Sleep(1000);
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
                string mensagem = "Favor escolher uma das 4 opções,pressione qualquer tecla para continuar";
                Console.WriteLine(mensagem);
                Console.ReadKey();
                break;
        }
        #endregion
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}


void FileMove(string directoryOrig, string FileDest, string directoryCopy, string filePaterns, int indFile)
{
    string[] filesGeneric = Directory.GetFiles(directoryOrig, $"{filePaterns}.xlsx", SearchOption.AllDirectories);
    Console.WriteLine("Procurando arquivo...");

    string newPatter = "";

    var newFilePatter = new Dictionary<int, string>
    {
       {1,"e11" },
       {2,"tinter01" }
    };

    if (indFile == 1)
        newPatter = newFilePatter[1];
    else if (indFile == 2)
        newPatter = newFilePatter[2];

    if (filesGeneric.Length > 0)
    {
        try
        {
            if (!Directory.Exists(directoryCopy))
            {
                Directory.CreateDirectory(directoryCopy);
            }

            Console.WriteLine("Arquivo encontrando, movendo...");

            foreach (string fileOrig in filesGeneric)
            {
                string fileCopy;
                string fileOriginCopy;
                int copyNumber = 1;

                fileOriginCopy = Path.Combine(fileDest, $"{newPatter}.xlsx");

                File.Copy(fileOrig, fileOriginCopy, true);

                do
                {
                    fileCopy = Path.Combine(directoryCopy, $"{newPatter}_{copyNumber}.xlsx");
                    copyNumber++;
                } while (File.Exists(fileCopy));

                File.Copy(fileOrig, fileCopy);
                File.Delete(fileOrig);
                Console.WriteLine("Arquivo movido com sucesso");
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