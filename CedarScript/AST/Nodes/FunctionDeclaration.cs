using CedarScript.AST.Expressions;
using CedarScript.AST.Scope;
using CedarScript.Parser;

namespace CedarScript.AST.Nodes;

public class FunctionDeclaration : BlockNode
{
    public override bool DoesAutoExecute { get; set; } = false;

    public string Name { get; set; }
    public List<Expression> Arguments { get; set; } = new();
    
    public Scope.Scope FunctionScope { get; set; } = new Scope.Scope();
    
    public Action? Function { get; set; }

    public new static FunctionDeclaration FromToken(Token token, TokenStream tokenStream)
    {
        if(token.Value != "function") throw new Exception("expected function but got " + token.Value);
        var functionDeclaration = new FunctionDeclaration();
        functionDeclaration.FunctionScope = new Scope.Scope()
        {
            ScopeType = ScopeType.Function
        };
        
        if (tokenStream.Peek().Type != TokenType.Identifier)
        {
            throw new Exception("expected function name but got " + tokenStream.Peek().Type);
        }

        var functionName = tokenStream.ConsumeNext();
        functionDeclaration.Name = functionName.Value;
        if (tokenStream.Peek().Value == "(" || tokenStream.Peek(1).Value == ")")
        {
            // We consume the arguments into void
            tokenStream.ConsumeNext(1);
        }
        if (tokenStream.Peek().Value != "{")
        {
            throw new Exception("function must have a body");
        }

        functionDeclaration.Body = [BlockNode.FromToken(tokenStream.ConsumeNext(), tokenStream)];

        
        return functionDeclaration;
    }

    public override ValueNode Execute(Scope.Scope scope)
    {
       
        if (Function != null)
        {
            // appears to be an internal impl.
            Function();
            return ValueNode.FromInt(0);
        }
       
        ValueNode lastReturnValue = ValueNode.FromInt(0);

        foreach (var node in Body)
        {
            lastReturnValue = node.Execute(GetScope(scope, ScopeType.Function));
        }
        
        return lastReturnValue;
    }
    
}