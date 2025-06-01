using MoveArquivo.Classes;

internal class Program
{
    /// <summary>
    /// Processes product data through either manual code entry or automated file upload
    /// 1 - Product Type request : Direct product code input
    /// 2 - File Type request : Automated processing of pattern-matched files from configured locations
    /// <remarks>
    ///  Known bugs :
    ///  1 - ESC key cancellation requires 500ms delay between presses.
    ///  2 - Alphabet characters are filtered out (by design).
    ///  3 - Input editing requires cancellation and restart.
    /// </remarks>
    /// </summary>

    static bool _hasFile;
    private static async Task Main(string[] args)
    {
        #region Configuration 
        //UI and service handlers
        Render render = new();
        UploadFile upload = new();
        Request request = new();

        //For distinction of the file that is needed
        var filePatterns = new Dictionary<int, string>
        {
            {1,"*.xlsx" }, // Example : Excel iles
            {2,"*.csv*" }  // Example: CSV files
        };

        // Path configuration (replace with your actual paths)
        var directoryOrig = @"C:\SourceFiles";        //Where files originate
        var directoryCopy = @"C:\AppData\Copy";       // Working copy location
        var destinationDir = @"C:\AppData\";          // Final destination

        #endregion

        while (true)
        {
            _hasFile = true;

            // CancelationToken Timeout configuration
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(30));
            var cancellationToken = cts.Token;

            // Listens ESC in the background,if the users needs to cancel the process
            _ = EscListenerAsync(cts);

            try
            {
                Render.RenderScreen();
                Console.Write("Select a number: ");
                int option = -1;

                var inputTask = Task.Run(() => Console.ReadLine());
                var completedTask = await Task.WhenAny(inputTask, Task.Delay(Timeout.Infinite, cancellationToken));

                if (completedTask == inputTask)
                {
                    var input = inputTask.Result;
                    if (!int.TryParse(input, out option))
                        option = -1;

                    await OptionChooser(upload, filePatterns, directoryOrig, directoryCopy, destinationDir, option, cancellationToken);
                }
                else
                {
                    Console.WriteLine("Operation canceled.");
                    await Task.Delay(1000);
                    Render.ReturnToStart();
                    continue;
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Operation canceled by timeout or ESC was pressed.");
                await Task.Delay(1000);
                Render.ReturnToStart();
            }
        }

        #region Functions

        async Task OptionChooser(UploadFile upload, Dictionary<int, string> filePatterns, string directoryOrig, string directoryCopy, string destinationDir, int option, CancellationToken cancellationToken)
        {
            switch (option)
            {
                case 1: // Product code input mode.
                    Render.UploadProductTypeRequest();
                    int[] productArray = CheckNumber();
                    await Request.HttpRequestByNumberAsync(productArray, cancellationToken);
                    await Task.Delay(1000, cancellationToken);
                    Render.ReturnToStart();
                    break;

                case 2: // File process mode.
                    Render.UploadFileTypeRequest();
                    await FileMoveAsync(directoryOrig, destinationDir, directoryCopy, filePatterns[1], 1, cancellationToken);
                    if (_hasFile)
                    {
                        await Request.HttpRequesFileSAsync(cancellationToken);
                    }
                    await Task.Delay(1000, cancellationToken);
                    Render.ReturnToStart();
                    break;

                default:
                    Console.WriteLine("Please select a valid option (1 or 2). Press any key to continue...");
                    Console.ReadKey();
                    await Task.Delay(1000, cancellationToken);
                    Console.Clear();
                    break;
            }
        }

        static int[] CheckNumber()
        {
            HashSet<string> lines = [];

            while (true)
            {
                string? line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;

                lines.Add(line);
            }

            string concatenatedNumbers = string.Join(",", lines);

            int[] productArray = [.. concatenatedNumbers
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => int.TryParse(p, out _))
                .Select(int.Parse)];

            return productArray;
        }

        async Task FileMoveAsync(string directoryOrig, string destinationDir, string directoryCopy, string filePaterns, int fileTypeIndex, CancellationToken cancellationToken)
        {
            //Search for files matching the pattern
            string[] filesGeneric = Directory.GetFiles(directoryOrig, $"{filePaterns}.xlsx", SearchOption.AllDirectories);
            Console.WriteLine("Searching for files...");

            //Defines the new pattern for the file
            string newPatter = "";
            var newFilePatter = new Dictionary<int, string>
            {
                {1,"newPatter.xlsx" },
                {2,"newPatter.csv" }
            };

            //If the pattern wasn't defined or its invalid it will never do the next process

            if (fileTypeIndex == 1)
                newPatter = newFilePatter[1];
            else if (fileTypeIndex == 2)
                newPatter = newFilePatter[2];

            if (filesGeneric.Length > 0)
            {
                try
                {
                    //Ensures directories exists
                    Directory.CreateDirectory(destinationDir);
                    Directory.CreateDirectory(directoryCopy);

                    //Ensures that it will process one file by call
                    Console.WriteLine("File found, processing...");
                    string firstFile = filesGeneric.First();

                    string fileCopy;
                    string fileOriginCopy = Path.Combine(destinationDir, $"{newPatter}.xlsx");
                    File.Copy(firstFile, fileOriginCopy, true);

                    //Upload the processed file
                    await upload.FileUploadAsync(fileOriginCopy, newPatter, cancellationToken);

                    // Create versioned backup
                    int copyNumber = 1;
                    do
                    {
                        fileCopy = Path.Combine(directoryCopy, $"{newPatter}_{copyNumber}.xlsx");
                        copyNumber++;
                    } while (File.Exists(fileCopy));

                    File.Copy(firstFile, fileCopy);
                    File.Delete(firstFile);
                    Console.WriteLine("File moved succsefully");
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Unexpected error:", ex);
                }
            }
            else
            {
                Console.WriteLine("No file were found");
                _hasFile = false;
                await Task.Delay(200, cancellationToken);
            }
        }

        static async Task EscListenerAsync(CancellationTokenSource cts)
        {
            while (!cts.Token.IsCancellationRequested)
            {
                if (Console.KeyAvailable && Console.ReadKey(intercept: true).Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nCanceling the operation...(Timeout or ESC was pressed)");
                    cts.Cancel();
                    break;
                }

                try
                {
                    await Task.Delay(200, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        #endregion
    }
}
