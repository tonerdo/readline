namespace ReadLine.Tests
{
    internal class AutoCompleteHandler : IAutoCompleteHandler
    {
        public char[] Separators { get; set; } = { ' ', '.', '/', '\\', ':' };
        public string[] GetSuggestions(string text, int index) => new[] { "World", "Angel", "Love" };
    }
}