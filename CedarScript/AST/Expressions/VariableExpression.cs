using CedarScript.AST.Nodes;
using CedarScript.Parser;

namespace CedarScript.AST.Expressions;

public class VariableExpression : Expression
{
    public required string Name { get; set; }

    public override ValueNode Execute(Scope.Scope scope)
    {
        var declaration = scope.VariableDeclarations.FirstOrDefault(s => s.VariableName == Name);
        if(declaration is null) throw new Exception("Cannot find variable named " + Name);

        return declaration.Value ?? ValueNode.FromInt(0);
    }

    public new static VariableExpression FromToken(Token token, TokenStream tokenStream)
    {
        var variableExpression = new VariableExpression()
        {
            Name = token.Value,
            DoesAutoExecute = false,
        };
        return variableExpression;
    }
}