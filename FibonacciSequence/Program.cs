﻿using System;
using System.Collections.Generic;
using System.IO;

namespace FibonacciSequence
{
    class Program
    {
        private static readonly string argN = "-n";
        private static readonly string argC = "-c";
        private static readonly string argF = "-f";
        private static readonly string argS = "-s";

        private static Dictionary<string, Type> ArgsDefinition = new Dictionary<string, Type>
        {
            { argN, typeof(int) },
            { argF, typeof(string) },
            { argC, null },
            { argS, typeof(char) },
        };

        public static void Main(string[] args)
        {
            var argsParser = new ArgsParser();
            try
            {
                argsParser.Parse(args, ArgsDefinition);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            // TODO add validation of arguments

            var n = argsParser.GetArgumentParameter(argN, 2);
            var separator = argsParser.GetArgumentParameter(argS, ' ');

            var sequencer = new FibonacciSequencer();
            var sequenceString = sequencer.GetSequenceString(n, separator);

            var filePath = argsParser.GetArgumentParameter<string>(argF, null);

            if (string.IsNullOrEmpty(filePath))
                WriteToConsole(sequenceString, argsParser.HasArg(argC));
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
