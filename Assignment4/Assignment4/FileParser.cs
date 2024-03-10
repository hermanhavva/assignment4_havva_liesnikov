namespace Assignment4;

public class FileParser
{
    public Dictionary<char, int> GetFrequencyTable(string filePath)
    {
        var frequencyTable = new Dictionary<char, int>();

        using var reader = new StreamReader(filePath);
        var line = reader.ReadLine();
        while (line != null)
        {
            var charArray = line.ToCharArray();
            foreach (var symbol in charArray)
                if (!frequencyTable.TryAdd(symbol, 1))
                    frequencyTable[symbol]++;
            line = reader.ReadLine();
        }

        return frequencyTable;
    }
}