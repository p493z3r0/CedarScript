using CedarScript.AST.Expressions;
using CedarScript.AST.Scope;
using CedarScript.Parser;

namespace CedarScript.AST.Nodes;

public class FunctionDeclaration : BlockNode
{
    public override bool DoesAutoExecute { get; set; } = false;

    public required string Name { get; set; }
    public List<Expression> Arguments { get; set; } = new();
    
    public Scope.Scope FunctionScope { get; set; } = new Scope.Scope();
    
    public Action<List<Expression>>? Function { get; set; }

    public new static FunctionDeclaration FromToken(Token token, TokenStream tokenStream)
    {
        if(token.Value != "function") throw new Exception("expected function but got " + token.Value);
        if (tokenStream.Peek().Type != TokenType.Identifier)
        {
            throw new Exception("expected function name but got " + tokenStream.Peek().Type);
        }

        var functionName = tokenStream.ConsumeNext();
        var functionDeclaration = new FunctionDeclaration()
        {
            Name = functionName.Value
        };
        functionDeclaration.FunctionScope = new Scope.Scope()
        {
            ScopeType = ScopeType.Function
        };
        
       
        
        var t1 = tokenStream.Peek();
        var t2 = tokenStream.Peek(1);
        if (tokenStream.Peek().Value == "(" && tokenStream.Peek(1).Value == ")")
        {
            // We consume the empty arguments into void
            tokenStream.ConsumeNext(1);
            
        }
        
        if (tokenStream.Peek().Value == "(" && tokenStream.Peek(1).Value != ")")
        {
            // we have some arguments, juhe..
            var closingBracketsIndex = tokenStream.Match(new Token()
            {
                Type = TokenType.Punctuator,
                Value = ")"
            });
            
            // consume the (
            tokenStream.ConsumeNext();
                
            while (tokenStream.IsTokenAvailableWithMaxIndex(closingBracketsIndex))
            {
                var argumentToken = tokenStream.ConsumeNext();
                var node = Expression.FromToken(argumentToken, tokenStream);
                functionDeclaration.Arguments.Add(node);
                
            }
            
            tokenStream.ConsumeNext();


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
            Function(this.Arguments);
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