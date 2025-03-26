string fileNamePattern = "*Delta*";
var directoryOrig = @$"C:\Movido arquivo\Delta";
var fileDest = @"C:\Movido arquivo\e11.xlsx";
var directoryCopy = @"C:\Movido arquivo\Copia";
string[] filesGeneric = Directory.GetFiles(directoryOrig, $"{fileNamePattern}.xlsx", SearchOption.AllDirectories);

CriaTela();

if (filesGeneric.Length > 0)
{
    Console.WriteLine("Arquivo Encontrado - Transferindo");
    try
        {
        foreach (string fileOrig in filesGeneric)
        {
            string fileCopy;
            int copyNumber = 1;

            File.Copy(fileOrig, fileDest, true);

            do
            {
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
            throw new ApplicationException("Erro: algo não correu como o planejado",ex);
        }
}
else
{
    Console.WriteLine("Não há arquivos com este nome" );
}

static void CriaTela()
{
    Console.WriteLine("\t\t\t-----------------------------------------------------\n\t\t\t" +
                      "|                                                   |\n\t\t\t" +
                      "|              ESCOLHA O A SUA OPÇÃO                |\n\t\t\t" +
                      "|                                                   |\n\t\t\t" +
                      "|           1 - Delta                               |\n\t\t\t" +
                      "|           2 - Inter MG - ES                       |\n\t\t\t" +
                      "|           3 - Inter ES - MG                       |\n\t\t\t" +
                      "|           4 - Minas Gerais                        |\n\t\t\t" +
                      "|                                                   |\n\t\t\t" +
                      "\"----------------------------------------------------");
    Console.ReadLine();
}
