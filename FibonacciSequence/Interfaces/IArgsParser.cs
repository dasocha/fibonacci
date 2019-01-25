using System;
using System.Collections.Generic;

namespace FibonacciSequence.Interfaces
{
    public interface IArgsParser
    {
        bool Parse(string[] args);
        T GetArg<T>(string name, T defaultValue);
    }
}
