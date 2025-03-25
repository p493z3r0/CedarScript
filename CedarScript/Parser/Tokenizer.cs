using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using CedarScript.Parser.Extensions;

namespace CedarScript.Parser;

public class Token
{
    public TokenType Type { get; init; }
    public string Value { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj is Token token)
        {
            return token.Value.Equals(Value) && token.Type == Type;
        }

        return false;
    }

    protected bool Equals(Token other)
    {
        return Type == other.Type && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Type, Value);
    }
}
public enum TokenType
{
    Keyword,
    Identifier,
    Punctuator,
    Numeric,
    String,
    Unknown
}

public class Tokens
{
    private List<Token> StoredTokens { get; init; } = new();

    public void AddToken(Token token)
    {
        if(!string.IsNullOrWhiteSpace(token.Value)) StoredTokens.Add(token);
    }
    
    public List<Token> GetTokens() => StoredTokens;
}
public static class Tokenizer
{
    public static HashSet<char> punctuators = new()
    {
        '.', ',', '+', '[', ']', '{', '}', '(', ')', '*', '`', '^', '\'', '!', '?', ';', ':', '/', '\\', '=', '%'
    };
    public static string[] Keywords = {
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
        "char", "checked", "class", "const", "continue", "decimal", "default",
        "delegate", "do", "double", "else", "enum", "event", "explicit",
        "extern", "false", "finally", "fixed", "float", "for", "foreach",
        "goto", "if", "implicit", "in", "int", "interface", "internal",
        "is", "lock", "long", "namespace", "new", "null", "object",
        "operator", "out", "override", "params", "private", "protected",
        "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
        "sizeof", "stackalloc", "static", "string", "struct", "switch",
        "this", "throw", "true", "try", "typeof", "uint", "ulong",
        "unchecked", "unsafe", "ushort", "using", "virtual", "void",
        "volatile", "while", "function", "var"
    };
    private static bool IsPunctuator(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty)
            return false;

        if (input.Length > 1) return false;
        
        return punctuators.Contains(input[0]);
    }
    public static bool IsNumber(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty)
            return false;

        return int.TryParse(input, out _) ||
               long.TryParse(input, out _) ||
               decimal.TryParse(input, out _) ||
               double.TryParse(input, out _);
    }
    
    private static  bool IsValidIdentifier(ReadOnlySpan<char> identifier)
    {
        if (identifier.IsEmpty)
            return false;

        if (!(char.IsLetter(identifier[0]) || identifier[0] == '_'))
            return false;

        for (int i = 1; i < identifier.Length; i++)
        {
            if (!(char.IsLetterOrDigit(identifier[i]) || identifier[i] == '_'))
                return false;
        }

       

        foreach (var keyword in Keywords)
        {
            if (identifier == keyword)
                return false;
        }

        return true;
    }
    private static TokenType TokenTypeFromValue(ReadOnlySpan<char> value, TokenType previousType)
    {
        if(Keywords.AsSpan().Contains(value.ToString())) return TokenType.Keyword;
        if(IsPunctuator(value)) return TokenType.Punctuator;
        if(IsNumber(value)) return TokenType.Numeric;
        if (IsValidIdentifier(value)) return TokenType.Identifier;
        return TokenType.String;
    }

    private static bool IsValueUncompletedString(ReadOnlySpan<char> value)
    {
        // if value has an uncompleted opening " it is yes. That means Count " % 2 > 0. Used span for performance reason. we save whereever we can, right?
        int count = 0;
        foreach (char c in value)
            if (c == '"') count++;
        return count % 2 > 0;
    }
    public static List<Token> Tokenize(ReadOnlySpan<char> text)
    {
        var tokens = new Tokens();
        var currentValue = "";

        foreach (var character in text)
        {
            if (!IsValueUncompletedString(currentValue + character) && character.IsSeperator())
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    var tokenType = TokenTypeFromValue(currentValue, tokens.GetTokens().LastOrDefault()?.Type ?? TokenType.Unknown);
                    tokens.AddToken(new Token { Type = tokenType, Value = currentValue });
                    currentValue = "";
                }

                var separatorTokenType = TokenTypeFromValue(character.ToString(), TokenType.Unknown);
                tokens.AddToken(new Token { Type = separatorTokenType, Value = character.ToString() });
            }
            else
            {
                currentValue += character;
            }
        }

        if (!string.IsNullOrEmpty(currentValue))
        {
            var tokenType = TokenTypeFromValue(currentValue, tokens.GetTokens().LastOrDefault()?.Type ?? TokenType.Unknown);
            tokens.AddToken(new Token { Type = tokenType, Value = currentValue });
        }

        return tokens.GetTokens();
    }
 }