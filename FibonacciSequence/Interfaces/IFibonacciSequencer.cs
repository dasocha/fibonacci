namespace FibonacciSequence.Interfaces
{
    public interface IFibonacciSequencer
    {
        ulong NextElement(ulong n_1, ulong n_2);
        ulong[] GenerateSequence(int n);
        string GetSequenceString(int n, char separator);
    }
}
