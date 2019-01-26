using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FibonacciSequence
{
    class Program
    {
        private static readonly string argN = "-n";
        private static readonly string argC = "-c";
        private static readonly string argF = "-f";
        private static readonly string argS = "-s";

        private static List<ProgramArgument> ArgsDefinition = new List<ProgramArgument>
        {
            new ProgramArgument { ConsoleSymbol = argN, IsRequired = true, ParameterType = typeof(int),     Name = "Sequence Length",       HasParameter = true, ParameterIsRequired = true },
            new ProgramArgument { ConsoleSymbol = argF, IsRequired = true, ParameterType = typeof(string),  Name = "File path",             HasParameter = true, ParameterIsRequired = true, MutualExplition = new List<string>{ argC } },
            new ProgramArgument { ConsoleSymbol = argC, IsRequired = true, ParameterType = null,            Name = "Sequence to console",   MutualExplition = new List<string>{ argF } },
            new ProgramArgument { ConsoleSymbol = argS, IsRequired = true, ParameterType = typeof(char),    Name = "Sequence separator",    HasParameter = true, ParameterIsRequired = true },
        };

        public static void Main(string[] args)
        {
            var argsParser = new ArgsParser();
            List<ProgramArgument> argsList = null;
            try
            {
                argsList = argsParser.Parse(args, ArgsDefinition);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            if (ProgramArgumetnValidator.ValidateArguments(argsList, ArgsDefinition))
            {
                // TODO message when invalid arguments
                return;
            }

            var n = GetArgumentParameterOrDefault<int>(argN, argsList, 0);
            var separator = GetArgumentParameterOrDefault(argS, argsList, ' ');

            var sequencer = new FibonacciSequencer();
            var sequenceString = sequencer.GetSequenceString(n, separator);

            var filePath = GetArgumentParameterOrDefault<string>(argF, argsList, null);

            if (string.IsNullOrEmpty(filePath))
                WriteToConsole(sequenceString, argsList.Any(x => x.ConsoleSymbol == argC));
            else
                SaveToDisk(filePath, sequenceString);

            return;
        }

        private static T GetArgumentParameterOrDefault<T>(string argSymbol, List<ProgramArgument> argsList, T defaultValue)
        {
            var arg = argsList.FirstOrDefault(x => x.ConsoleSymbol == argSymbol);
            if (arg == null || arg.ArgumentParameter == null)
                return defaultValue;

            return (T)arg.ArgumentParameter;
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
                return false;

            try
            {
                File.WriteAllText(filePath, sequence);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("File saving error: {0}", ex.Message));
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
