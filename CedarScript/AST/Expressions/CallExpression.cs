using CedarScript.AST.Expressions;
using CedarScript.AST.Nodes;
using CedarScript.AST.Scope;


public class CallExpression : Expression
{
    public string Name { get; set; }
    public List<ValueNode> Arguments { get; set; } = new();
    public override ValueNode Execute(Scope scope)
    {
        var function = scope.FunctionDeclarations.FirstOrDefault(x => x.Name == Name);
        if(function == null) throw new Exception($"Function {Name} not defined in current scope");
        return function.Execute(scope);
    }
}