using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement Element, TPriority Priority)> _elements = new();

    public int Count => _elements.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        _elements.Add((element, priority));
    }

    public TElement Dequeue()
    {
        if (_elements.Count == 0)
            throw new InvalidOperationException("Queue is empty");

        int bestIndex = 0;
        TPriority bestPriority = _elements[0].Priority;
        for (int i = 1; i < _elements.Count; i++)
        {
            if (_elements[i].Priority.CompareTo(bestPriority) < 0)
            {
                bestPriority = _elements[i].Priority;
                bestIndex = i;
            }
        }
        TElement bestElement = _elements[bestIndex].Element;
        _elements.RemoveAt(bestIndex);
        return bestElement;
    }

    public bool Contains(TElement element)
    {
        return _elements.Exists(x => EqualityComparer<TElement>.Default.Equals(x.Element, element));
    }

    public void Clear()
    {
        _elements.Clear();
    }
}
