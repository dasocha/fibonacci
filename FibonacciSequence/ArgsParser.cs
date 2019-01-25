using System;
using System.Collections.Generic;

namespace FibonacciSequence
{
    public class ArgsParser : Interfaces.IArgsParser
    { 
        private readonly Dictionary<string, object> argsDict;

        public ArgsParser()
        {
            argsDict = new Dictionary<string, object>();
        }

        public void Parse(string[] args, Dictionary<string, Type> argsDef)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (!argsDef.ContainsKey(arg))                
                    continue;

                if (argsDef.TryGetValue(arg, out Type argParamType))
                {
                    if (argParamType != null && args.Length > i + 1)
                    {
                        var param = args[i + 1];
                        try
                        {
                            var argParam = Convert.ChangeType(param, argParamType);

                            if (argsDict.TryAdd(arg, argParam))
                                i++;
                            else
                                ThrowDuplicatedArg(arg);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Error while parsing parameter {0} of argument {1}, reason: {2}", param, arg, ex.Message), ex);
                        }
                        continue;
                    }
                }
                if (!argsDict.TryAdd(arg, null))
                    ThrowDuplicatedArg(arg);
            }         
        }

        private void ThrowInvalidArgError(string name)
        {
            throw new Exception(string.Format("Invalid {0} parameter!", name));
        }

        private void ThrowDuplicatedArg(string name)
        {
            throw new Exception(string.Format("Duplicated {0} argument!", name));
        }

        public bool HasArg(string name)
        {
            return argsDict.ContainsKey(name);
        }

        public T GetArgumentParameter<T>(string name, T defaultValue)
        {
            if (argsDict.TryGetValue(name, out object val))
                return (T)val;

            return defaultValue;
        }
    }
}
