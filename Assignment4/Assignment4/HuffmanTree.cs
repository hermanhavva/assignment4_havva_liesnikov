namespace Assignment4;
using System.Text;
using System.Collections;



public class HuffmanTree
{
    
    public void SaveEncodedDataToFile(BitArray bitArray, string filePath)
        {
            byte[] bytes = new byte[(bitArray.Length - 1) / 8 + 1];
            bitArray.CopyTo(bytes, 0);
            
            // Збереження байтів у файл
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Запис таблиці кодування
                var encodeTableSerialized = SerializeEncodeTable();
                var encodeTableBytes = Encoding.UTF8.GetBytes(encodeTableSerialized);
                fileStream.Write(BitConverter.GetBytes(encodeTableBytes.Length), 0, sizeof(int));
                fileStream.Write(encodeTableBytes, 0, encodeTableBytes.Length);

                // Запис закодованих даних
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        private string SerializeEncodeTable()
        {
            return string.Join(";", EncodeTable.Select(kv => $"{kv.Key}:{kv.Value}"));
        }

        // Метод для зчитування закодованих даних і таблиці кодування з файлу
        public static (BitArray bitArray, Dictionary<string, string> encodeTable) ReadEncodedDataFromFile(string filePath)
        {
            byte[] encodeTableBytes;
            byte[] encodedDataBytes;

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                // Читання довжини таблиці кодування
                byte[] encodeTableLengthBytes = new byte[sizeof(int)];
                fileStream.Read(encodeTableLengthBytes, 0, sizeof(int));
                int encodeTableLength = BitConverter.ToInt32(encodeTableLengthBytes, 0);

                // Читання таблиці кодування
                encodeTableBytes = new byte[encodeTableLength];
                fileStream.Read(encodeTableBytes, 0, encodeTableLength);

                // Читання закодованих даних
                encodedDataBytes = new byte[fileStream.Length - fileStream.Position];
                fileStream.Read(encodedDataBytes, 0, encodedDataBytes.Length);
            }

            var encodeTableString = Encoding.UTF8.GetString(encodeTableBytes);
            var encodeTable = encodeTableString.Split(';').Select(part => part.Split(':')).ToDictionary(split => split[0], split => split[1]);

            var bitArray = new BitArray(encodedDataBytes);

            return (bitArray, encodeTable);
        }
    
    //  private readonly List<HuffmanTreeNode> _totalNodes= new ();  // do not know if need this thing or not
    private readonly MyPriorityQueue _priorityQueue = new();
    private readonly Dictionary<string, HuffmanTreeNode> _childNodes = new();
    public Dictionary<string, string?> EncodeTable { get; } = new(); // key - string symbol, value - encoded symbol
    private readonly HuffmanTreeNode _mainParent;

    public HuffmanTree(Dictionary<string, int> frequencyTable) // get the last layer of nodes 
    {
        // into list and into priorityQueue
        foreach (var pair in frequencyTable) // get all the child nodes into priority queue 
        {
            var node = new HuffmanTreeNode(pair.Key, pair.Value);
            _childNodes[node.Data] = node;
            _priorityQueue.Enqueue(new KeyValuePair<HuffmanTreeNode, int>(node, pair.Value));
        }

        while (!_priorityQueue.IsEmpty()) // forming the tree
        {
            var pair = _priorityQueue.Dequeue();
            var curNode1 = pair.Key;
            var frequency = pair.Value;
            pair = _priorityQueue.Dequeue();
            var curNode2 = pair.Key;
            frequency += pair.Value;
            var newData = curNode1.Data + curNode2.Data; // now got to know what is right and left
            var newNode = new HuffmanTreeNode(newData, frequency, curNode1, curNode2);
            if (newNode.LeftChild != null)
                if (newNode.RightChild != null)
                    (newNode.LeftChild.Parent, newNode.RightChild.Parent) =
                        (newNode, newNode); // both children have the same parent
            if (!_priorityQueue.IsEmpty())
                _priorityQueue.Enqueue(new KeyValuePair<HuffmanTreeNode, int>(newNode, newNode.Frequency));
            else
                _mainParent = newNode; // keep track of the main Parent
        }

        GetEncodeTable(frequencyTable);
    }

    private void GetEncodeTable(Dictionary<string, int> frequencyTable)
    {
        foreach (var symbol in frequencyTable.Keys) // building the Encode table
        {
            var encodedData = "";
            var childNode = _childNodes[symbol];
            while (childNode.Parent != null)
            {
                var curNode = childNode.Parent;
                if (childNode == curNode.LeftChild) // if left -> 0
                    encodedData += '0';
                else if (childNode == curNode.RightChild) // if right -> 1 
                    encodedData += '1';
                childNode = curNode;
            }

            var encodedDataArray = encodedData.ToCharArray(); // reverse the string so that we move from 
            Array.Reverse(encodedDataArray); // the greatest parent to smallest child in terms  
            EncodeTable.TryAdd(symbol, new string(encodedDataArray)); // of encoding
        }
    }

    public BitArray Encode(string strToEncode, Dictionary<string, int> frequencyTable)
    {
        if (_childNodes.Count < 1) throw new Exception("the Tree is not initialised ");
        StringBuilder encodedData = new();
        foreach (var symbol in strToEncode)
            if (EncodeTable.TryGetValue(symbol.ToString(), out var symbolCode))
                encodedData.Append(symbolCode);
        var binaryString = encodedData.ToString();
        var bitArray = new BitArray(binaryString.Select(ch => ch == '1').ToArray());
        return bitArray;
    }

    public string Decode(BitArray bitArray)
    {
        StringBuilder stringBuilder = new();
        var curNode = _mainParent;
        foreach (bool bit in bitArray)
        {
            if (bit && curNode.RightChild != null)
                curNode = curNode.RightChild;
            else if (!bit && curNode.LeftChild != null) curNode = curNode.LeftChild;
            if (curNode.RightChild == null || curNode.LeftChild == null)
            {
                stringBuilder.Append(curNode.Data);
                curNode = _mainParent;
            }
        }

        return stringBuilder.ToString();
    }
}

public class HuffmanTreeNode
{
    public HuffmanTreeNode? Parent { get; set; }
    public HuffmanTreeNode? LeftChild { get; }
    public HuffmanTreeNode? RightChild { get; }
    public string Data { get; }
    public readonly int Frequency;

    public HuffmanTreeNode(string data, int frequency, HuffmanTreeNode? rightChild = null,
        HuffmanTreeNode? leftChild = null, HuffmanTreeNode? parent = null)
    {
        Data = data;
        Parent = parent;
        LeftChild = leftChild;
        RightChild = rightChild;
        Frequency = frequency;
    }
}