namespace MoveArquivo.Classes
{
    public class Render
    {
        public static void RenderScreen()
        {
            Console.WriteLine("\t\t\t-----------------------------------------------------\n\t\t\t" +
                              "|                                                   |\n\t\t\t" +
                              "|              ESCOLHA A SUA OPÇÃO                  |\n\t\t\t" +
                              "|                                                   |\n\t\t\t" +
                              "|           1 - Minas Gerais                        |\n\t\t\t" +
                              "|           2 - Delta                               |\n\t\t\t" +
                              "|           3 - Inter MGxES                         |\n\t\t\t" +
                              "|           4 - Inter ESxMG                         |\n\t\t\t" +
                              "|                                                   |\n\t\t\t" +
                              "\"----------------------------------------------------");
        }

        public void UploadMinasGerais()
        {
            Console.WriteLine("Minas Gerais : ");
            Console.WriteLine("Digite os codigos dos produtos somente numeros com virgulas:");
            Console.WriteLine("Pressione enter para voltar ao inicio.");
        }

        public void UploadEspiritoSantos()
        {
            Console.WriteLine("Espirto Santos - Delta.");
            Console.WriteLine("Fazendo Upload para o Servidor...");
            Console.WriteLine("Subida concluida.");
            Console.WriteLine("Subindo o tributo...");
            Console.WriteLine("Subida finalizada.");
            Console.WriteLine("Pressione enter para voltar ao inicio.");
        }
        public void UploadMGxES()
        {
            Console.WriteLine("Espirto Santos - Inter MGxES.");
            Console.WriteLine("Fazendo Upload para o Servidor...");
            Console.WriteLine("Subida concluida.");
            Console.WriteLine("Subindo o tributo...");
            Console.WriteLine("Subida finalizada.");
            Console.WriteLine("Pressione enter para voltar ao inicio.");
        }
        public void UploadESxMG()
        {
            Console.WriteLine("Espirto Santos - Inter ESxMG.");
            Console.WriteLine("Fazendo Upload para o Servidor...");
            Console.WriteLine("Subida concluida.");
            Console.WriteLine("Subindo o tributo...");
            Console.WriteLine("Subida finalizada.");
            Console.WriteLine("Pressione enter para voltar ao inicio.");
        }
    }
}
