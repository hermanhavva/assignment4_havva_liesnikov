namespace Assignment4;

public class MyPriorityQueue
{
    private readonly List<KeyValuePair<HuffmanTreeNode, int>> _myHeap = new(); // наша куча з точками
    private int _index;

    public void Print()
    {
        foreach (var element in _myHeap)
        {
            Console.WriteLine(
                $"{element.Key},priority: {element.Value}");
        }
    }

    public bool IsEmpty()
    {
        return _index == 0;
    }
    
    private void HeapifyUp() // виконується коли елемент додали позаду
    {
        int childIndex = _index - 1;
        if (childIndex % 2 != 0) // якщо непарне  
        {
            while (childIndex != 0)
            {
                int parentIndex = (childIndex - 1) / 2;
                var parent = _myHeap[parentIndex].Value;
                var child = _myHeap[childIndex].Value;
                if (parent > child)
                {
                    Swap(parentIndex, childIndex);
                }

                childIndex = parentIndex;
            }
        }
        else // якщо парне 
        {
            while (childIndex != 0)
            {
                int parentIndex = (childIndex - 2) / 2;
                var parent = _myHeap[parentIndex].Value;
                var child = _myHeap[childIndex].Value;
                if (parent > child)
                {
                    Swap(parentIndex, childIndex);
                }

                childIndex = parentIndex;
            }
        }
    }

    private void Swap(int index1, int index2)
    {
        (_myHeap[index1], _myHeap[index2]) = (_myHeap[index2], _myHeap[index1]);
    }

    private void HeapifyDown()
    {
        int parentIndex = 0;
        _myHeap[0] = _myHeap[_index - 1];
        _myHeap.RemoveAt(_index - 1);
        _index--;
        while (HasLeftChild(parentIndex))
        {
            int smallestChildIndex = GetSmallestChildIndex(parentIndex);
            if (_myHeap[parentIndex].Value <= _myHeap[smallestChildIndex].Value) break;
            Swap(parentIndex, smallestChildIndex);
            parentIndex = smallestChildIndex;
        }
    }

    private bool HasLeftChild(int parentIndex)
    {
        return 2 * parentIndex + 1 < _index;
    }

    private int GetSmallestChildIndex(int parentIndex)
    {
        int leftChildIndex = 2 * parentIndex + 1;
        int rightChildIndex = 2 * parentIndex + 2;
        if (rightChildIndex < _index &&
            _myHeap[rightChildIndex].Value < _myHeap[leftChildIndex].Value)
            return rightChildIndex;
        return leftChildIndex;
    }

    public void Enqueue(KeyValuePair<HuffmanTreeNode, int> node)
    {
        _myHeap.Add(node);
        _index++;
        HeapifyUp();
    }

    public KeyValuePair<HuffmanTreeNode, int> Dequeue()
    {
        if (_index == 0)
        {
            throw new Exception("Index out of range");
        }

        var priorityElement = _myHeap[0];
        HeapifyDown();
        // var point = priorityElement.Keys.First(); 
        return priorityElement;
    }

    public int Count()
    {
        return _index;
    }
}