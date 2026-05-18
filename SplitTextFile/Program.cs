// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

Console.WriteLine("Hello, World!");

bool isValid = false;
string _sourceFilePath = string.Empty;

string _sourceFileName = string.Empty;
string _sourceFileDirectory = string.Empty;
string _sourceFileExtension = string.Empty;
int rowsPerFile = 40000;
do
{
    if (args.Length <= 0)
    {
        Console.WriteLine("input filePath [rowsPerFile]");
        isValid = false;

        string userInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(userInput))
        {
            args = userInput.Split(' ');
        }
        continue;
    }

    _sourceFilePath = args[0];
    if(args.Length > 0) isValid = true;
    if (args.Length >= 2) rowsPerFile = int.Parse(args[1]);

    if (!File.Exists(_sourceFilePath))
    {
        Console.WriteLine($"File: {_sourceFilePath} not EXISTS");
        isValid = false;
        continue;
    }
    else
    {
        _sourceFileName = Path.GetFileName(_sourceFilePath);
        _sourceFileDirectory = Path.GetDirectoryName(_sourceFilePath);
        _sourceFileExtension = Path.GetExtension(_sourceFilePath);
        isValid = true;
    }

    if (!isValid)
    {
        // Environment.Exit(1);
    }
} while (!isValid);

Console.WriteLine($"argument: {JsonConvert.SerializeObject(args)}");

using (var reader = new StreamReader(_sourceFilePath))
{
    // Capture linePointer to repeat in every split file
    string linePointer = reader.ReadLine();
    int fileCounter = 1;
    bool isFirstLine = true;
    string writeFileName = string.Empty;
    while (!reader.EndOfStream)
    {
        writeFileName = $"split_{fileCounter++}";
        if(!string.IsNullOrEmpty(_sourceFileExtension)) writeFileName+=$"{_sourceFileExtension}";

        string _destinationFilePath = Path.Combine(_sourceFileDirectory, writeFileName);
        using (var writer = new StreamWriter(_destinationFilePath))
        {
            if (isFirstLine) writer.WriteLine(linePointer);

            for (int i = 0; i < rowsPerFile && !reader.EndOfStream; i++)
            {
                writer.WriteLine(reader.ReadLine());
            }
        }
        isFirstLine = false;
        Console.WriteLine($"Write: {writeFileName}");
    }
}
