﻿using System;
using System.Text;
using FibonacciSequence.Interfaces;

namespace FibonacciSequence
{
    public class FibonacciSequencer : IFibonacciSequencer
    {
        public ulong NextElement(ulong n_1, ulong n_2)
        {
            return n_1 + n_2;
        }

        public ulong[] GenerateSequence(int n) // TODO consider of use library like BigMath to handle very big  numbers
        {
            if (n < 1)
                throw new Exception("n parameter must be higher than 0");

            var elements = new ulong[n];

            elements.SetValue((ulong)0, 0);
            if (n == 1)
                return elements;

            elements.SetValue((ulong)1, 1);
            if (n == 2)
                return elements;

            for (int i = 2; i < n; i++)
            {
                var next = NextElement(elements[i - 2], elements[i - 1]);
                elements.SetValue(next, i);
            }

            return elements;
        }

        public string GetSequenceString(int n, char separator)
        {
            var sequence = GenerateSequence(n);

            var sb = new StringBuilder();
            foreach (var element in sequence)
            {
                sb.Append(element + separator.ToString());
            }
            return sb.ToString().Trim();
        }
    }
}
