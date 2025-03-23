namespace CedarScript.Parser.Extensions;

public static class StringExtensions
{
    public static bool IsSeperator(this char character)
    {
        var separators = new List<char> { ' ', '\t', '\n', '\r' };
        bool flag = separators.Contains(character) || Tokenizer.punctuators.Contains(character);

        if (character == '+')
        {
            
        }
        return separators.Contains(character) || Tokenizer.punctuators.Contains(character);
    }
}