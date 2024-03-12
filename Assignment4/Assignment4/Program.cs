using Assignment4;
using System.Collections;

var fileParser = new FileParser("C:\\C# projects\\assignment4_havva_liesnikov\\sherlock.txt");
var frequencyTable= fileParser.GetFrequencyTable();
foreach (var variable in frequencyTable)
{
    Console.WriteLine($"{variable.Key}, {variable.Value}");
}

var tree = new HuffmanTree();
tree.InitialiseTree(frequencyTable);
BitArray bitArray = tree.Encode(fileParser.StrText, frequencyTable);
foreach (bool bit in bitArray)
{
    Console.WriteLine(bit ? "1" : "0");
}
Console.WriteLine("fifnish");

