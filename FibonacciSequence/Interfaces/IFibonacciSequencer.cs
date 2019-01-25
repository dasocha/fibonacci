namespace FibonacciSequence.Interfaces
{
    public interface IFibonacciSequencer
    {
        int NextElement(int n_1, int n_2);
        int[] GenerateSequence(int n);
        string GetSequenceString(int n, char separator);
    }
}
