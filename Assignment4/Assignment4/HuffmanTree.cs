namespace Assignment4;
using System.Text;
using System.Collections;

public class HuffmanTree
{

    private readonly List<HuffmanTreeNode> _totalNodes= new ();  // do not know if need this thing or not
    private MyPriorityQueue _priorityQueue = new();
    private Dictionary<string, HuffmanTreeNode> _childNodes = new();
    public Dictionary<string, string> encodeTable { get; }




    /*public override string ToString()
    {
        StringBuilder totalList = new(); // StringBuilder is faster than ordinary concatenation
        var currentNode = _firstNode; // can be null
        while (currentNode != null)
        {
            totalList.Append($"{currentNode.Pair} ");
            currentNode = currentNode.Next;
        }

        return totalList.ToString();
    }*/

    public void InitialiseTree(Dictionary<string, int> frequencyTable)  // get the last layer of nodes 
    {                                                                   // into list and into priorityQueue
        //_priorityQueue = new MyPriorityQueue();
        foreach (var pair in frequencyTable)
        {
            var node = new HuffmanTreeNode(pair.Key, pair.Value);
            _totalNodes?.Add(node);
            _childNodes[node.Data] = node; 
            _priorityQueue.Enqueue(new KeyValuePair<HuffmanTreeNode, int>(node, pair.Value));
        }

        
        while (!_priorityQueue.IsEmpty())
        {
            var pair = _priorityQueue.Dequeue();
            var curNode1 = pair.Key;
            var frequency = pair.Value;
            
            pair = _priorityQueue.Dequeue();
            var curNode2 = pair.Key;
            frequency += pair.Value;
            
            var newData = curNode1.Data + curNode2.Data;  // now got to know what is right and left
            
            var newNode = new HuffmanTreeNode(newData, frequency, curNode1, curNode2 );
            (newNode.LeftChild.Parent,  newNode.RightChild.Parent) = (newNode, newNode);  // both children have the same parent
            _totalNodes?.Add(newNode);
            if (!_priorityQueue.IsEmpty())
                _priorityQueue.Enqueue(new KeyValuePair<HuffmanTreeNode, int>(newNode,newNode.Frequency));
        }
    }

    public BitArray Encode(string strToEncode, Dictionary<string,int> frequencyTable)
    {
        if (_childNodes.Count < 1)
            throw new Exception("the Tree is not initialised ");
        StringBuilder encodedData = new();
        foreach (var symbol in strToEncode)
        {
            var childNode = _childNodes[symbol.ToString()];
            
            while (childNode.Parent != null)
            {
                var curNode = childNode.Parent;
                if (childNode == curNode.LeftChild)  // if left -> 0
                {
                    encodedData.Append('0');
                }
                else if (childNode == curNode.RightChild) // if right -> 1 
                    encodedData.Append('1');
                
                childNode = curNode;
            }
        }

        var binaryString = encodedData.ToString();
        
        BitArray bitArray = new BitArray(binaryString.Select(c => c == '1').ToArray());
        return bitArray;
    }

    public string Decode(BitArray bitArray)
    {
        
    }
    
    /*public void RemoveLast()
    {
        if (_lastNode != null)
        {
            if (_lastNode.LeftChild != null)
            {
                _lastNode.LeftChild.Parent = null;
                _lastNode = _lastNode.LeftChild;
            }
            else
            {
                _lastNode = null;
                _firstNode = null;
            }
            _size--;
        }
    }*/
    /*
    public KeyValuePair<string, string> Pop()
    {
        var pair = _firstNode?.Pair;
        if (pair == null) throw new Exception("List is empty when tried to pop");
    
        _firstNode = _firstNode?.Parent;
        if (_firstNode != null) 
            _firstNode.LeftChild = null;
        _size--;
        if (_firstNode?.Pair == null)  // if there is no elements in list -> the last must be null
            _lastNode = null; 
        return pair;
    }

    public int Size()
    {
        return _size;
    }
    */

    /*
    public void Add(string data, )
    {
        var currentNode = _firstNode;
        if (_size == 0) // adding first element
        {
            
            _firstNode = new HuffmanTreeNode(myPair);
            _lastNode = _firstNode;
        }
        else
        {
            _lastNode!.Parent = new HuffmanTreeNode(myPair);
            _lastNode.Parent.LeftChild = _lastNode;  // adding pointer to a previous element
            _lastNode = _lastNode.Parent;
        }

        _size++;
    }
    */
    
    // public bool Contains(string key)
    // {
    //     var currentNode = _firstNode;
    //     while (currentNode?.Pair.Key != key && currentNode?.Parent != null) currentNode = currentNode.Parent;
    //
    //     return currentNode?.Pair.Key == key;
    // }

    // public bool IsEmpty()
    // {
    //     return _firstNode == null;
    // }

    /*public void RemoveByKey(string key)
    {
        var currentNode = _firstNode;
        if (currentNode?.Pair.Key == key) // if the first element is to be removed
        {
            _firstNode = currentNode.Next;
            if (_firstNode != null) 
                _firstNode.Previous = null;
                
            if (_firstNode?.Pair == null) 
                _lastNode = null; 
            
            _size--;
        }
        else
        {
            while (currentNode?.Next?.Pair.Key != key && currentNode?.Next?.Next != null)
                currentNode = currentNode.Next;

            if (currentNode?.Next?.Pair.Key == key) // if there is such a key
            {
                currentNode.Next = currentNode.Next.Next; // omit the element 
                currentNode.Next!.Previous = currentNode;
                _size--;
            }
        }
    }*/

    // public KeyValuePair<string, string>? GetItemByKey(string key)
    // {
    //     // get pair with provided key, return null if not found
    //
    //     var currentNode = _firstNode;
    //     while (currentNode?.Pair.Key != key && currentNode?.Parent != null) currentNode = currentNode.Parent;
    //     if (currentNode?.Pair.Key == key)
    //         return currentNode.Pair;
    //
    //     return null;
    // }
}

public class HuffmanTreeNode
{
    // public KeyValuePair<string, string> Pair { get; }

    public HuffmanTreeNode? Parent { get; set; }
    public HuffmanTreeNode? LeftChild { get; set; }
    public HuffmanTreeNode? RightChild { get; set; }
    public string Data { get; }
    public int Frequency;


    public HuffmanTreeNode(string data, int frequency, HuffmanTreeNode? rightChild = null, HuffmanTreeNode? leftChild = null, HuffmanTreeNode? parent = null)
    {
        Data = data;
        Parent = parent;
        LeftChild = leftChild;
        RightChild = rightChild;
        Frequency = frequency;
    }
}