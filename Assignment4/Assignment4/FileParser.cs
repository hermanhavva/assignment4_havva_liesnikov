namespace Assignment4;

public class FileParser
{
    public string StrText { get; }

    public FileParser(string filePath)
    {
        using var reader = new StreamReader(filePath);
        StrText = reader.ReadToEnd();
    }

    public Dictionary<string, int> GetFrequencyTable()
    {
        var frequencyTable = new Dictionary<string, int>();
        foreach (var symbol in StrText)
            if (!frequencyTable.TryAdd(symbol.ToString(), 1))
                frequencyTable[symbol.ToString()]++;
        return frequencyTable;
    }
}