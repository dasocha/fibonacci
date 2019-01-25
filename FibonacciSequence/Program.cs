using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FibonacciSequence
{
    class Program
    {
        public static int Main(string[] args)
        {
            var argsParser = new ArgsParser();
            if (!argsParser.Parse(args))
            {
                return 0; //error on parsing args
            }

            var n = argsParser.GetArg<int>(argsParser.argN, 2);
            var sequencer = new FibonacciSequencer();
            var sequence = sequencer.NElements(n);

            var separator = argsParser.GetArg<char>(argsParser.argS, ' ');
            var sequenceString = GetSequenceString(sequence, separator);

            var filePath = argsParser.GetArg<string>(argsParser.argF, null);
            if (string.IsNullOrEmpty(filePath))
            {
                var waitForInput = argsParser.GetArg<bool>(argsParser.argC, false);
                WriteToConsole(sequenceString, waitForInput);
            }
            else
            {
                SaveToDisk(filePath, sequenceString);
            }

            return -1;
        }

        private static string GetSequenceString(int[] sequence, char separator)
        {
            var sb = new StringBuilder();
            foreach (var element in sequence)
            {
                sb.Append(element + separator.ToString());
            }
            return sb.ToString().Trim();
        }

        private static void WriteToConsole(string sequence, bool waitForInput)
        {
            Console.WriteLine(sequence);
            if (waitForInput)
            {
                var input = Console.Read();
            }
        }

        private static bool SaveToDisk(string filePath, string sequence)
        {
            if (File.Exists(filePath) && !ShouldOverwriteFile())
            {
                return false;
            }

            try
            {
                File.WriteAllText(filePath, sequence);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool ShouldOverwriteFile()
        {
            var input = AskForOverwrite();

            switch (input)
            {
                case 'y':
                    return true;
                case 'n':
                    return false;
                default:
                    return ShouldOverwriteFile();
            }
        }

        private static char AskForOverwrite()
        {
            Console.Write("A file with the given name already exists. Do you want to overwrite it? (y/n)");
            var input = Console.Read();
            return (char)input;
        }
    }
}
