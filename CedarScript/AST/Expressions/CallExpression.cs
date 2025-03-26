using System.Runtime.CompilerServices;
using CedarScript.AST.Expressions;
using CedarScript.AST.Nodes;
using CedarScript.AST.Scope;
using CedarScript.Parser;


public class CallExpression : Expression
{
    public string Name { get; set; }
    public List<Expression> Arguments { get; set; } = new();
    public override ValueNode Execute(Scope scope)
    {
        var function = scope.FindFunctionDeclarationByName(Name);
        if(function == null) throw new Exception($"Function {Name} not defined in current scope");
        function.Arguments = Arguments;
        return function.Execute(scope);
    }


    public new static CallExpression FromToken(Token token, TokenStream tokenStream)
    {
        var callExpression = new CallExpression
        {
            Name = token.Value
        };

        if (tokenStream.Peek(1).Value != ")")
        {
            // we have to parse the args here 
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
                callExpression.Arguments.Add(node);

            }
            
            tokenStream.ConsumeNext();
            
        }
        else
        {
            tokenStream.ConsumeNext();
            tokenStream.ConsumeNext();
        }

        
        Parser.ConsumeSemicolonIfNeeded(tokenStream);
        return callExpression;
    }
}