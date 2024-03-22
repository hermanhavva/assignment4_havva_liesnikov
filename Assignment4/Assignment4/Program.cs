using Assignment4;
using System.Collections;
using System.IO; // Для роботи з файлами

var fileParser = new FileParser("C:\\C# projects\\assignment4_havva_liesnikov\\sherlock.txt");
var frequencyTable = fileParser.GetFrequencyTable();

// foreach (var variable in frequencyTable)
// {
//     Console.WriteLine($"{variable.Key}, {variable.Value}");
// }

var tree = new HuffmanTree(frequencyTable);
BitArray bitArray = tree.Encode(fileParser.StrText, frequencyTable);
var encodingTable = tree.EncodeTable;
Console.WriteLine("Getting Encoding Table");
foreach (var variable in encodingTable)
{
    Console.WriteLine($"{variable.Key}, {variable.Value}");
}


byte[] bytes = new byte[(bitArray.Length - 1) / 8 + 1]; 
bitArray.CopyTo(bytes, 0);
// foreach (bool bit in bitArray) // виводимо бітовий масив
// {
//     Console.Write(bit ? "1" : "0");
// }
string outputPath = "C:\\C# projects\\assignment4_havva_liesnikov\\sherlock_encoded.bin";
File.WriteAllBytes(outputPath, bytes);
Console.WriteLine($"Encoded data saved to {outputPath}");

// Розшифровка
// Перетворення масиву байтів назад у BitArray
byte[] encodedBytes = File.ReadAllBytes(outputPath);
BitArray encodedBitArray = new BitArray(encodedBytes);

// Використання тієї ж таблиці частот для побудови дерева (у реальному сценарії цю таблицю можна зберігати та відновлювати)
HuffmanTree decodeTree = new HuffmanTree(frequencyTable);
string decodedText = decodeTree.Decode(encodedBitArray);

Console.WriteLine("Decoded text:");
Console.WriteLine(decodedText);
