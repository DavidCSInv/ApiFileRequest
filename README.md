📦 Product Data Processor Console App

This is a pure C# Console Application designed to process product data through either manual code input or automated file upload. 
The app communicates with Linux servers via file transfer and interacts with third-party APIs for data processing.

🛠️ Features
    Manual Processing (Type 1):
    Allows direct product code input through the console.
    Automated File Processing (Type 2):
    Automatically locates and processes files based on predefined patterns from configured directories. These files are uploaded to Linux servers and sent to external APIs.

🔄 Usage Flow

  When the program starts, the user selects the request type:
     1 for manual product code input.
     2 for automated file processing.

  Based on the selected mode, the program will:
     Prompt for manual code input and process accordingly.
     Search for matching files, upload them to a server, and send relevant data to external APIs.

⚙️ Technologies Used

  .NET C# (Console App)

  SSH/SFTP (e.g., via SSH.NET) for Linux server file uploads

  HttpClient for third-party API requests

  Regex for file pattern recognition

  Console I/O for user interaction

🐞 Known Bugs & Limitations

  -ESC key cancellation:
  Requires a delay of at least 500ms between key presses to register properly.
  
  -Alphabetic characters are ignored:
    
  -Character filtering is intentional and by design; only numeric input is processed.

  -Editing input requires restart:
   Once input begins, you must cancel and restart the program to edit previous entries.
