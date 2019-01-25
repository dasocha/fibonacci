using System;
using System.Collections.Generic;

namespace FibonacciSequence.Interfaces
{
    public interface IArgsParser
    {
        void Parse(string[] args, Dictionary<string, Type> argsDef);
        bool HasArg(string name);
        T GetArgumentParameter<T>(string name, T defaultValue);
    }
}
