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

        private static readonly List<ProgramArgument> ArgsDefinition = new List<ProgramArgument>
        {
            new ProgramArgument { ConsoleSymbol = argN, IsRequired = true,  ParameterType = typeof(int),     Name = "Sequence Length",       HasParameter = true, ParameterIsRequired = true },
            new ProgramArgument { ConsoleSymbol = argF, IsRequired = false, ParameterType = typeof(string),  Name = "File path",             HasParameter = true, ParameterIsRequired = true, MutuallyExcluded = new List<string>{ argC } },
            new ProgramArgument { ConsoleSymbol = argC, IsRequired = false, ParameterType = null,            Name = "Sequence to console",   MutuallyExcluded = new List<string>{ argF } },
            new ProgramArgument { ConsoleSymbol = argS, IsRequired = false, ParameterType = typeof(char),    Name = "Sequence separator",    HasParameter = true, ParameterIsRequired = true },
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

            var result = ProgramArgument.ValidateArguments(argsList, ArgsDefinition);
            if (!result.Success)
            {
                Console.WriteLine(result.ErrorMessage);
                return;
            }

            var n = ProgramArgument.GetArgumentParameterOrDefault<int>(argN, argsList, 0);
            var separator = ProgramArgument.GetArgumentParameterOrDefault(argS, argsList, ' ');

            var sequencer = new FibonacciSequencer();
            var sequenceString = sequencer.GetSequenceString(n, separator);

            var filePath = ProgramArgument.GetArgumentParameterOrDefault<string>(argF, argsList, null);

            if (string.IsNullOrEmpty(filePath))
                WriteToConsole(sequenceString, argsList.Any(x => x.ConsoleSymbol == argC));
            else
                SaveToDisk(filePath, sequenceString);

            return;
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
