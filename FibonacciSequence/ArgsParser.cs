using System;
using System.Collections.Generic;
using System.Linq;

namespace FibonacciSequence
{
    public class ArgsParser : Interfaces.IArgsParser
    {
        public readonly string argN;
        public readonly string argC;
        public readonly string argF;
        public readonly string argS;

        private readonly Dictionary<string, object> argsDict;

        public ArgsParser()
        {
            argN = "-n";
            argC = "-c";
            argF = "-f";
            argS = "-s";

            argsDict = new Dictionary<string, object>();
        }

        public bool Parse(string[] args)
        {
            var cArgPresent = args.Contains(argC);
            var fArgPresent = args.Contains(argF);

            if (cArgPresent && fArgPresent)
            {
                throw new Exception("The –f and –c are mutually exclusive.");
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == argN && args.Length > i + 1)
                {
                    if (int.TryParse(args[i + 1], out int n))
                    {
                        if (argsDict.TryAdd(argN, n))                       
                            i++;
                        else
                            ThrowDuplicatedArg(argN);
                    }
                    else
                        ThrowInvalidArgError(argN);
                }
                else if (args[i] == argF && args.Length > i+1)
                {
                    var filePath = args[i + 1];
                    if (!filePath.StartsWith('-'))
                    {
                        if (argsDict.TryAdd(argF, filePath))
                            i++;
                        else
                            ThrowDuplicatedArg(argF);
                    }
                    else
                        ThrowInvalidArgError(argF);
                }
                else if (args[i] == argC)
                {
                    if (!argsDict.TryAdd(argC, true))
                    {
                        ThrowDuplicatedArg(argC);
                    }
                }
                else if (args[i] == argS && args.Length > i + 1) 
                {
                    var sep = args[i + 1];
                    if (sep.Length > 1)
                    {
                        ThrowInvalidArgError(argS);
                    }
                    else
                    {
                        var arr = sep.ToCharArray();
                        if (argsDict.TryAdd(argS, arr[0]))
                            i++;
                        else
                            ThrowDuplicatedArg(argS);
                    }
                }
            }

            if (Validate())
            {
                return true;
            }
            return false;
        }

        private bool Validate()
        {
            if (!argsDict.ContainsKey(argN))
            {
                ThrowInvalidArgError(argN);
            }

            return true;
        }

        private void ThrowInvalidArgError(string name)
        {
            throw new Exception(string.Format("Invalid {0} parameter!", name));
        }

        private void ThrowDuplicatedArg(string name)
        {
            throw new Exception(string.Format("Duplicated {0} argument!", name));
        }

        public T GetArg<T>(string name, T defaultValue)
        {
            if (argsDict.TryGetValue(name, out object val))
            {
                return (T)val;
            }
            return defaultValue;
        }
    }
}
