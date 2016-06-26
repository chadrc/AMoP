using UnityEngine;
using System.Collections;
using System;

public class Range : IEnumerable
{
    private int max;
    private int min;

    public Range(int max)
    {
        min = 0;
        this.max = max;
    }

    public Range(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public IEnumerator GetEnumerator()
    {
        return new RangeEnumerator(this);
    }

    private class RangeEnumerator : IEnumerator
    {
        private Range range;
        private int current;

        public RangeEnumerator(Range range)
        {
            this.range = range;
            current = range.min;
        }

        public object Current
        {
            get
            {
                return current;
            }
        }

        public bool MoveNext()
        {
            return current++ < range.max;
        }

        public void Reset()
        {
            current = range.min;
        }
    }
}
