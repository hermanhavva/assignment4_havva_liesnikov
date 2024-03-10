using Assignment4;

var fileParser = new FileParser();
var frequencyTable= fileParser.GetFrequencyTable("C:\\C# projects\\assignment4_havva_liesnikov\\sherlock.txt");
foreach (var varieble in frequencyTable)
{
    Console.WriteLine($"{varieble.Key}, {varieble.Value}");
}
