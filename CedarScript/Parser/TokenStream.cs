using System.Reflection;

namespace CedarScript.Parser;

/// <summary>
/// 
/// </summary>
public class TokenStream
{
    private readonly List<Token> _tokens;
    private int _index = 0;

    public TokenStream(List<Token> tokens)
    {
        _tokens = tokens;
    }

    /// <summary>
    /// Peek per default returns the _next_ token which has not been consumed. However you can add a offset to peek even further. Those operations do NOT modify the index state
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public Token Peek(int offset = 0)
    {
        return _tokens[_index + offset];
    }

    public Token ConsumeNext(int offset = 0)
    {
        if(_index + offset >= _tokens.Count) throw new EndOfStreamException();
        var token = _tokens[_index + offset];
        _index++;
        _index += offset;
        return token;
    }

    public void Reset()
    {
        _index = 0;
    }

    public void Skip(int count)
    {
        if(_index + count > _tokens.Count) _index = _tokens.Count;
        else _index += count;
    }

    public void Rewind(int count)
    {
        if (count > _index) _index = 0;
        else _index -= count;
    }

    /// <summary>
    /// This will return the index of a token specified ahead in the index
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public int Match(Token token)
    {
        for (int i = _index; i < _tokens.Count; i++)
        {
            if(_tokens[i].Equals(token)) return i;
        }

        return -1;
    }
    /// <summary>
    /// Checks if a next token is available. Offset will look into the next tokens
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public bool IsTokenAvailable(int offset = 0)
    {
        return _index + offset < _tokens.Count;
    }

    public int GetNextScopeClosureTokenIndex(int baseIndex = -1)
    {
        if (baseIndex == -1) baseIndex = _index;

        int openingScopesFound = 0;
        int closingScopeIndex = baseIndex;
        for (int i = baseIndex; i < _tokens.Count; i++)
        {
            if (_tokens[i].Value.Equals("}"))
            {
                if(openingScopesFound == 0) return i;
                openingScopesFound--;
                continue;
            }

            if (_tokens[i].Value.Equals("{"))
            {
                openingScopesFound++;
            }
        }

        return _tokens.Count - 1;
    }

    public bool IsTokenAvailableWithMaxIndex(int maxIndex)
    {
        if (IsTokenAvailable())
        {
            return _index < maxIndex;
        }

        return false;
    }

    public List<Token> ConsumeUntilType(TokenType type, List<TokenType>? disallowedTokens = null)
    {
        if(disallowedTokens == null) disallowedTokens = new List<TokenType>();
        var consumed = new List<Token>();
        for (int i = _index; i < _tokens.Count; i++)
        {
            if(disallowedTokens.Contains(_tokens[i].Type)) throw new ApplicationException("Token type disallowed: " + _tokens[i].Type);
            consumed.Add(_tokens[i]);
            if (_tokens[i].Type == type)
            {
                _index = ++i;
                return consumed;
            }
        }
        return consumed;
    }
}