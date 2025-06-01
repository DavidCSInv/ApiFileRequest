using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static ApiFileRequest.Classes.Render;

namespace ApiFileRequest.Classes
{
    public class Request
    {

        private static readonly HttpClient _client = new()
        {
            Timeout = TimeSpan.FromMinutes(30) // Define o timeout para 30 minutos
        };

        private static async Task SendRequestAsync(object requestBody, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Initiating API request...");
                Uri requestUri = new("https://api.example.com/base-path/");

                ConsoleSpinner _spin = new();
                Stopwatch stopWatch = new();

                using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
                {
                    Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                };

                // Start the API request and spin the spinner asynchronously
                using var responseTask = _client.SendAsync(requestMessage, cancellationToken);

                stopWatch.Start();

                // Spinner animation task
                var spinnerTask = Task.Run(() =>
                {
                    while (!responseTask.IsCompleted)
                    {
                        _spin.Turn();
                        Thread.Sleep(300); // Adjust the speed of the spinner
                    }
                }, cancellationToken);


                // Timer task
                var timerTask = Task.Run(async () =>
                {
                    while (!responseTask.IsCompleted && !cancellationToken.IsCancellationRequested)
                    {
                        TimeSpan ts = stopWatch.Elapsed;

                        string elapsedTime = string.Format("{0:00}H:{1:00}M:{2:00}S.{3:00}MS  -- ",
                                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                        // Clear the previous output and overwrite it
                        Console.Write("\rElapsed Time: " + elapsedTime);  // `\r` moves cursor to the start of the line
                        await Task.Delay(100, cancellationToken);  // Update the timer every 100ms
                    }
                }, cancellationToken);

                // Wait for the response to complete
                HttpResponseMessage response = await responseTask;

                stopWatch.Stop();

                // Ensure the spinner stops once the request is done
                await spinnerTask;

                //Handle response
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    Console.WriteLine($"Server response: {response.StatusCode} - Success.");
                }
                else
                {
                    Console.WriteLine($"Request error: {response.StatusCode}");
                    string errorResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                    Console.WriteLine($"Error details: {errorResponse}");
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Request was canceled (timeout or interruption).");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

        }

        public static async Task HttpRequestByNumberAsync(int[] products, CancellationToken cancellationToken)
        {
            // Processes manually entered product codes
            // (User provides code list directly in UI)
            // supplierId remains empty as filtering happens via input
            var requestBody = new
            {
                ProductId = products,
                supplierId = Array.Empty<int>()
            };

            await SendRequestAsync(requestBody, cancellationToken);
        }

        public static async Task HttpRequesFileSAsync(CancellationToken cancellationToken)
        {

            // File Upload Mode Request
            // (Selected by user in previous UI screen)
            // Uses empty filters as this processes the uploaded file contents
            var requestBody = new
            {
                produtoCodigos = Array.Empty<int>(),
                fornecedorCodigos = Array.Empty<int>()
            };

            await SendRequestAsync(requestBody, cancellationToken);
        }

    }
}
