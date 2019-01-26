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
        public List<string> MutuallyExcluded { get; set; }
        public bool HasParameter { get; set; }
        public bool ParameterIsRequired { get; set; }
        public object ArgumentParameter { get; set; }
    
        public static ValidateResponse ValidateArguments(List<ProgramArgument> args, List<ProgramArgument> argsDefinition)
        {
            var response = new ValidateResponse { Success = true };

            try
            {
                CheckRequiredArguments(args, argsDefinition, response);

                CheckMutuallyExclusiveArguments(args, argsDefinition, response);

                 CheckMissingParameters(args, argsDefinition, response);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Exc = ex;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        private static void CheckRequiredArguments(List<ProgramArgument> args, List<ProgramArgument> argsDefinition, ValidateResponse response)
        {
            var requiredArgs = argsDefinition.Where(x => x.IsRequired);
            foreach (var argDef in requiredArgs)
            {
                var arg = args.FirstOrDefault(x => x.ConsoleSymbol == argDef.ConsoleSymbol);
                if (arg == null)
                {
                    response.Success = false;
                    response.ErrorMessage = string.Format("{2}Required argument {0}({1}) not provided! \n", argDef.ConsoleSymbol, argDef.Name, response.ErrorMessage);
                }
            }
        }

        private static void CheckMutuallyExclusiveArguments(List<ProgramArgument> args, List<ProgramArgument> argsDefinition, ValidateResponse response)
        {
            var mutuallyExclusiveArgs = args.Where(x => x.MutuallyExcluded != null && x.MutuallyExcluded.Any());
            foreach (var mExArg in mutuallyExclusiveArgs)
            {
                foreach (var mx in mExArg.MutuallyExcluded)
                {
                    if (args.Any(x => x.ConsoleSymbol == mx))
                    {
                        response.Success = false;
                        response.ErrorMessage = string.Format("{3}Forbiden use of mutual explicit arguments {0}({1}) and {2})!\n", mExArg.ConsoleSymbol, mExArg.Name, mx, response.ErrorMessage);
                    }
                }
            }
        }

        private static void CheckMissingParameters(List<ProgramArgument> args, List<ProgramArgument> argsDefinition, ValidateResponse response)
        {
            var argsWithMissingParam = args.Where(x => x.ParameterIsRequired && x.ArgumentParameter == null).ToList();
            if (argsWithMissingParam.Count > 0)
            {
                response.Success = false;
                foreach (var item in argsWithMissingParam)
                {
                    response.ErrorMessage = string.Format("{0}Parameter for argument {1}({2}) not provided! \n", response.ErrorMessage, item.ConsoleSymbol, item.Name);
                }
            }
        }

        public static T GetArgumentParameterOrDefault<T>(string argSymbol, List<ProgramArgument> argsList, T defaultValue)
        {
            var arg = argsList.FirstOrDefault(x => x.ConsoleSymbol == argSymbol);
            if (arg == null || arg.ArgumentParameter == null)
                return defaultValue;

            return (T)arg.ArgumentParameter;
        }
    }

    public class ValidateResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exc { get; set; }
    }
}
