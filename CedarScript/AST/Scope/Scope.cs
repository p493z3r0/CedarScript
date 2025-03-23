using CedarScript.AST.Nodes;

namespace CedarScript.AST.Scope;

public class Scope
{
    public List<FunctionDeclaration> FunctionDeclarations { get; set; } = new();
    public List<VariableDeclaration> VariableDeclarations { get; set; } = new();
}