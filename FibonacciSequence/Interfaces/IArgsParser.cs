using System;
using System.Collections.Generic;

namespace FibonacciSequence.Interfaces
{
    public interface IArgsParser
    {
        List<ProgramArgument> Parse(string[] args, List<ProgramArgument> argsDef);
    }
}
