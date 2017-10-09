namespace System
{
    interface IAutoCompleteHandler
    {
        string[] Separators { get; set; }
        string[] GetSuggestions(string text, int index);
    }
}