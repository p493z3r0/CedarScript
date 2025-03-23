namespace CedarScript.AST.Nodes;

public class VariableDeclaration : BlockNode
{
    public string VariableName { get; set; } = "";
    public ValueNode? Value { get; set; }
    public bool IsConstant { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsNullable { get; set; }
}