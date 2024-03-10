using Assignment4;

var fileParser = new FileParser();
var frequencyTable= fileParser.GetFrequencyTable("C:\\C# projects\\assignment-four-liesnikov-havva\\sherlock.txt");
foreach (var varieble in frequencyTable)
{
    Console.WriteLine($"{varieble.Key}, {varieble.Value}");
}
