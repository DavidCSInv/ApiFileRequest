namespace MoveArquivo.Classes
{
    public class Render
    {
        public static void RenderScreen()
        {
            Console.WriteLine("\t\t\t ---------------------------------------------------------   \n\t\t\t" +
                              "|                                                          |\n\t\t\t" +
                              "|              Select an option :                          |\n\t\t\t" +
                              "|                                                          |\n\t\t\t" +
                              "|           1 - Product type request                       |\n\t\t\t" +
                              "|           2 - File type request                          |\n\t\t\t" +
                              " ----------------------------------------------------------");
            Console.WriteLine();
        }

        public static void UploadProductTypeRequest()
        {
            Console.WriteLine("Product type request:");
            Console.WriteLine("WARNING,If no numbers are entered,all products will be requested");
            Console.WriteLine("Enter or paste the product IDs:");
        }
        public static void UploadFileTypeRequest()
        {
            Console.WriteLine("File type request");
            Console.WriteLine("Processing API request...");

        }
        public static void ReturnToStart()
        {
            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadKey();
            Console.Clear();
        }

        public class ConsoleSpinner
        {
            private int _counter;
            private readonly string[] _spinnerSymbols = { "/", "-", "\\", "|" };

            public ConsoleSpinner()
            {
                _counter = 0;
            }

            public void Turn()
            {
                // Select spinner symbol based on counter
                Console.Write(_spinnerSymbols[_counter % 4]);

                // Move cursor back to overwrite previous symbol
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                // Increment counter for next symbol
                _counter++;
            }
        }

    }
}
