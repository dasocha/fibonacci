using System;
using System.Collections.Generic;
using System.Linq;

namespace FibonacciSequence
{
    public class ArgsParser : Interfaces.IArgsParser
    { 
        private readonly List<ProgramArgument> argsDict;

        public ArgsParser()
        {
            argsDict = new List<ProgramArgument>();
        }

        public List<ProgramArgument> Parse(string[] args, List<ProgramArgument> argsDef)
        {
            foreach (var argDef in argsDef)
            {
                try
                {
                    var arg = args.SingleOrDefault(x => x == argDef.ConsoleSymbol);

                    if (arg != null)
                        argsDict.Add(argDef);

                    if (argDef.HasParameter)
                    {
                        var index = Array.IndexOf(args, arg);
                        if (args.Length> index + 2)
                        {
                            var param = args[index + 1];
                            try
                            {
                                var argParam = Convert.ChangeType(param, argDef.ParameterType);
                                argDef.ArgumentParameter = argParam;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Error while parsing parameter {0} of argument {1}, reason: {2}", param, arg, ex.Message), ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ThrowDuplicatedArg(argDef.Name, ex.Message);
                }

            }

            return argsDict;
        }

        private void ThrowInvalidArgError(string name)
        {
            throw new Exception(string.Format("Invalid {0} parameter!", name));
        }

        private void ThrowDuplicatedArg(string name, string message = "")
        {
            throw new Exception(string.Format("Duplicated {0} argument!", name));
        }
    }
}
