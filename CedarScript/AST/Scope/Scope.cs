using CedarScript.AST.Nodes;

namespace CedarScript.AST.Scope;
public enum ScopeType
{
    Global,
    Function,
    Block,
    Module,
    Lexical
}
public class Scope
{
    public ScopeType ScopeType { get; set; } = ScopeType.Global;
    public Scope? OuterScope { get; set; }
    public Scope? InnerScope { get; set; }
    public List<FunctionDeclaration> FunctionDeclarations { get; set; } = new();
    public List<VariableDeclaration> VariableDeclarations { get; set; } = new();
}