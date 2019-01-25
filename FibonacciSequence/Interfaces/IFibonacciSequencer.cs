using System;
namespace FibonacciSequence.Interfaces
{
    public interface IFibonacciSequencer
    {
        //int NextElement(int a, int b);
        int[] GenerateSequence(int n);
    }
}
