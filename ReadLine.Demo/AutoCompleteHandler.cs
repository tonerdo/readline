namespace ReadLine.Demo
{
    internal class AutoCompletionHandler : IAutoCompleteHandler
    {
        public char[] Separators { get; set; } =
        {
            ' ',
            '.',
            '/',
            '\\',
            ':'
        };


        public string[] GetSuggestions(string text, int index)
        {
            return text.StartsWith("git ")
                ? new[]
                {
                    "init",
                    "clone",
                    "pull",
                    "push"
                }
                : null;
        }
    }
}
