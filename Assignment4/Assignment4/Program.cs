using Assignment4;
using System.Collections;

var fileParser = new FileParser("C:\\C# projects\\assignment4_havva_liesnikov\\sherlock.txt");
var frequencyTable = fileParser.GetFrequencyTable(); // за допомогою класа fileparser тримали frequencyTable

foreach (var variable in frequencyTable) // виводимо frequencyTable
{
    Console.WriteLine($"{variable.Key}, {variable.Value}");
}

var tree = new HuffmanTree(frequencyTable); // ініціалізуємо об'єкт класса HuffmanTree
BitArray bitArray = tree.Encode(fileParser.StrText, frequencyTable); // отримали бітовий масив(кодування)

foreach (bool bit in bitArray) // виводимо бітовий масив
{
    Console.Write(bit ? "1" : "0");
}

Console.WriteLine(tree.Decode(bitArray)); // декодуємо назад бітовий масив, виводимо текст

