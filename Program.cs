using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;

namespace FileSearch
{
    class Program
    {
        static List<string> filetypes = new List<string>
        {
            ".sql",
            ".doc",
            ".docx",
            ".rtf",
            ".cs",
            ".js",
            ".ts",
            ".vue",
            ".text",
            ".vb",
            ".asmx",
            ".readme",
            ".json",
            ".txt",
            ".eslintignore",
            ".npmrc",
            ".prettierignore",
            ".editorconfig",
            ".dockerignore",
            ".browserslistrc",
            ".yaml",
            ".gitignore",
            ".conf",
            ".md",
            ".sh",
            ".config",
            ".yml",
            ".sln",
            ".csproj",
            ".vbproj",
            ".props",
            ".targets",
            ".cache"
        };
        static void Main()
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Ftp.wav";
            player.Load();
            var filepath = Command("file path: ");
            string SearchPhrase = Command("Search Phrase: ");
            if(string.IsNullOrWhiteSpace(filepath) || string.IsNullOrWhiteSpace(SearchPhrase)) return;

            DirectoryInfo dirInfo = new DirectoryInfo(filepath);
            player.PlayLooping();

            var files = dirInfo.GetFiles("*.txt");
            foreach (var type in filetypes)
            {
                files = files.Concat(dirInfo.GetFiles($"*{type}")).ToArray();
            }
            PrintMessage($"Files in FileList: {files.Length}");
            var subFolders = Directory.GetDirectories(filepath);
            foreach (var subFolder in subFolders)
            {
                Console.Clear();
                files = files.Concat(GetFilesFromSubFolders(subFolder)).ToArray();
                PrintMessage($"Files in FileList: {files.Length}");
            }

            PrintMessage("Finished Fetching Files");
            foreach (var file in files)
            {
                var lineNumber = 0;
                using (var sr = file.OpenText()) {
                    while (!sr.EndOfStream) {
                        var line = sr.ReadLine();
                        lineNumber += 1;
                        if (String.IsNullOrEmpty(line)) continue;
                        if (line.IndexOf(SearchPhrase ?? "", StringComparison.CurrentCultureIgnoreCase) >= 0) {
                            PrintMessage($"\nFilePath: {file.DirectoryName}\nFile: {file.Name}\nLineNumber: {lineNumber}\nLine: {line.Trim()}\n");
                        }
                    }
                }
            }
            PrintMessage("SearchFinished");
            // player.Stop();
            Console.ReadKey();
            CountDownExit(3);
        }

        private static void CountDownExit(int counter)
        {
            for (int i = 0; i < counter; i++)
            {
                Console.Clear();
                PrintMessage(i.ToString());
                Thread.Sleep(1000);
            }
        }

        private static string Command(string CommandText)
        {
            if(string.IsNullOrWhiteSpace(CommandText)) return null;
            Console.ForegroundColor = GetRandomConsoleColor();
            Console.Write(CommandText);
            return Console.ReadLine();
        }

        private static void PrintMessage(string message)
        {
            if(string.IsNullOrWhiteSpace(message)) return;
            Console.ForegroundColor = GetRandomConsoleColor();
            Console.WriteLine(message);
        }

        public static FileInfo[] GetFilesFromSubFolders(string filepath)
        {
            var files = new List<FileInfo>();
            var subFolders = Directory.GetDirectories(filepath);
            DirectoryInfo dirInfo = new DirectoryInfo(filepath);
            foreach (var type in filetypes)
            {
                files.AddRange(dirInfo.GetFiles($"*{type}"));
            }

            foreach (var subFolder in subFolders)
            {
                files.AddRange(GetFilesFromSubFolders(subFolder));
            }
            
            return files.ToArray();
        }
        
        private static Random _random = new Random();
        private static ConsoleColor GetRandomConsoleColor()
        {
            List<ConsoleColor> colorList = new List<ConsoleColor>();
            colorList.Add(ConsoleColor.Blue);
            colorList.Add(ConsoleColor.White);
            colorList.Add(ConsoleColor.Cyan);
            colorList.Add(ConsoleColor.Green);
            colorList.Add(ConsoleColor.Magenta);
            colorList.Add(ConsoleColor.Red);
            colorList.Add(ConsoleColor.Yellow);
            colorList.Add(ConsoleColor.DarkBlue);
            colorList.Add(ConsoleColor.DarkCyan);
            colorList.Add(ConsoleColor.DarkGreen);
            colorList.Add(ConsoleColor.DarkMagenta);
            colorList.Add(ConsoleColor.DarkRed);
            colorList.Add(ConsoleColor.DarkYellow);

            var chosenColorBest = colorList[_random.Next(colorList.Count - 1)];
            return chosenColorBest;
        }
        
    }
}
