using CedarScript.AST.Expressions;
using CedarScript.AST.Nodes;
using CedarScript.AST.Scope;
using CedarScript.Parser;


public class CallExpression : Expression
{
    public string Name { get; set; }
    public List<ValueNode> Arguments { get; set; } = new();
    public override ValueNode Execute(Scope scope)
    {
        var function = scope.FindFunctionDeclarationByName(Name);
        if(function == null) throw new Exception($"Function {Name} not defined in current scope");
        return function.Execute(scope);
    }


    public new static CallExpression FromToken(Token token, TokenStream tokenStream)
    {
        var callExpression = new CallExpression();
        callExpression.Name = token.Value;

        if (tokenStream.Peek().Value != "(" && tokenStream.Peek(1).Value != ")")
        {
            throw new Exception($"Function {token.Value} has no parameters");
        }

        tokenStream.ConsumeNext();
        tokenStream.ConsumeNext();
        Parser.ConsumeSemicolonIfNeeded(tokenStream);
        return callExpression;
    }
}