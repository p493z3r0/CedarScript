using CedarScript.AST.Nodes;
using CedarScript.Parser;

namespace CedarScript.AST.Expressions;

public class VariableExpression : Expression
{
    public required string Name { get; set; }
    public List<string> Path { get; set; } = new List<string>();

    public override ValueNode Execute(Scope.Scope scope)
    {
        var declaration = scope.FindVariableDeclarationByName(Name);
        if(declaration is null) throw new Exception("Cannot find variable named " + Name);

        if (declaration.IsReference)
        {
            return declaration.Execute(scope);
        }

        if (Path.Any() && declaration.IsObject)
        {
            return declaration.Object.GetPropertyByPath(Path);
        }
      
        return declaration.Value ?? ValueNode.FromInt(0);
    }

    private static List<string> ParsePropertyAccessPath(TokenStream stream, List<string> currentPath)
    {
        if(!stream.Peek().Value.Equals(".")) return currentPath;
        stream.ConsumeNext(); // .
        var accessor = stream.ConsumeNext();
        if(accessor.Type != TokenType.Identifier) throw new Exception($"Unexpected variable accessor subpath: {accessor}");
        currentPath.Add(accessor.Value);
        return ParsePropertyAccessPath(stream, currentPath);
    }
    public new static VariableExpression FromToken(Token token, TokenStream tokenStream)
    {
        var variableName = token.Value;
        var path = new List<string>();
        if (tokenStream.Peek().Value.Equals("."))
        {
            path = ParsePropertyAccessPath(tokenStream, path);
        }
        var variableExpression = new VariableExpression()
        {
            Name = variableName,
            DoesAutoExecute = false,
            Path = path
        };
        return variableExpression;
    }

}