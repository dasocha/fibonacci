using System;
using System.Collections.Generic;
using System.Linq;

namespace FibonacciSequence
{
    public class ProgramArgument
    {
        public string Name { get; set; }
        public string ConsoleSymbol { get; set; }
        public Type ParameterType { get; set; }
        public bool IsRequired { get; set; }
        public List<string> MutualExplition { get; set; }
        public bool HasParameter { get; set; }
        public bool ParameterIsRequired { get; set; }
        public object ArgumentParameter { get; set; }
    }

    public static class ProgramArgumetnValidator
    {
        public static bool ValidateArguments(List<ProgramArgument> args, List<ProgramArgument> argsDefinition)
        {
            var requiredArgs = argsDefinition.Where(x => x.IsRequired);
            foreach (var argDef in requiredArgs)
            {
                var arg = args.FirstOrDefault(x => x.ConsoleSymbol == argDef.ConsoleSymbol);
                if (arg == null)
                    return false;
            }

            var mutualExplitArgs = argsDefinition.Where(x => x.MutualExplition != null && x.MutualExplition.Any());
            foreach (var mExArg in mutualExplitArgs)
            {
                foreach (var mx in mExArg.MutualExplition)
                {
                    if (args.Any(x => x.ConsoleSymbol == mx))
                        return false;
                }
            }

            if (args.Any(x => x.ParameterIsRequired && x.ArgumentParameter == null))
                return false;

            return true;
        }
    }
}
