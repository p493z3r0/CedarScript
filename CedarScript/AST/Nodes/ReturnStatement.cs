
using CedarScript.AST.Expressions;
using CedarScript.Parser;

namespace CedarScript.AST.Nodes;

public class ReturnStatement : BlockNode
{
    public Expression Argument { get; set; }

    public override bool DoesAutoExecute { get; set; } = true;

    public new static ReturnStatement FromToken(Token token, TokenStream tokenStream)
    {
        var returnStatement = new ReturnStatement();
        if(!token.Value.Equals("return")) throw new InvalidOperationException("Expected return statement");
        
        var nextSemicolonIndex = tokenStream.Match(new Token()
        {
            Value = ";",
            Type = TokenType.Punctuator
        });
        if(nextSemicolonIndex < 0) throw new InvalidOperationException("Expected return statement to have an expression and end with an index");

        var nodes = new List<BlockNode>();
        while (tokenStream.IsTokenAvailableWithMaxIndex(nextSemicolonIndex))
        {
            var consumedToken = tokenStream.ConsumeNext();
            if(consumedToken.Value == ";") continue;
            var node = Parser.Parser.TokenHandler(consumedToken, tokenStream);
            nodes.Add(node);
        }
        
        // lets make my life easy
        
        if(nodes.Count > 1) throw new InvalidOperationException("Expected return statement to have only one node");
        var parsedNode = nodes[0];
        if (parsedNode is Expression expression)
        {
            returnStatement.Argument = expression;
            return returnStatement;
        }
        
        throw new InvalidOperationException("Return argument invalid");
        
        
        return new ReturnStatement();
    }
    public override ValueNode Execute(Scope.Scope scope)
    {
        return Argument.Execute(scope);
    }
}