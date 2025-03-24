
using CedarScript.AST.Expressions;
using CedarScript.AST.Nodes;
namespace CedarScript.Parser;

public class Parser
{
    private TokenStream _tokenStream = new TokenStream(new List<Token>());
    public ProgramNode ParseProgram(string program)
    {
        var tokens = Tokenizer.Tokenize(program);
        _tokenStream = new TokenStream(tokens);
        
        var programNode = new ProgramNode();

        while (_tokenStream.IsTokenAvailable())
        {
            var token = _tokenStream.ConsumeNext();
            programNode.Nodes.Add(TokenHandler(token, _tokenStream));
        }

        return programNode;
    }
    
    public static void ConsumeSemicolonIfNeeded(TokenStream stream)
    {
        if (stream.Peek().Value.Equals(";"))
        {
            stream.ConsumeNext();
        }
    }

    public static BlockNode TokenHandler(Token token, TokenStream tokenStream)
    {
        switch (token.Type)
        {
            case TokenType.Keyword:
                return HandleKeyword(token, tokenStream);
            case TokenType.Identifier:
                return Expression.FromToken(token, tokenStream);
            case TokenType.Punctuator:
                return new StubNode(token);
            case TokenType.Numeric:
                return Expression.FromToken(token, tokenStream);
            case TokenType.String:
                return new StubNode(token);
            case TokenType.Unknown:
                return new StubNode(token);
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    public static BlockNode HandleKeyword(Token token, TokenStream tokenStream)
    {
        if(token.Type != TokenType.Keyword) throw new Exception("Token is not keyword");
        if (token.Value == "var")
        {
           return VariableDeclaration.FromToken(token, tokenStream);
        }

        if (token.Value == "function")
        {
            return FunctionDeclaration.FromToken(token, tokenStream);
        }

        if (token.Value == "return")
        {
            return ReturnStatement.FromToken(token, tokenStream);
        }
        return new VariableDeclaration(); // default for now
    }
}