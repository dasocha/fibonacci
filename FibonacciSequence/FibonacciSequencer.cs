using System;
namespace FibonacciSequence
{
    public class FibonacciSequencer : Interfaces.IFibonacciSequencer
    {
        public FibonacciSequencer()
        {
        }

        private int NextElement(int a, int b)
        {
            //if (a < 0 || b < 1 || a > b)
            //{
            //    throw new Exception("invalid argument");
            //}
            return a + b;
        }

        public int[] NElements(int n)
        {
            var elements = new int[n];
            elements.SetValue(0, 0);
            elements.SetValue(1, 1);

            for (int i = 2; i < n; i++)
            {
                var next = NextElement(elements[i - 2], elements[i - 1]);
                elements.SetValue(next, i);
            }

            return elements;
        }
    }
}
