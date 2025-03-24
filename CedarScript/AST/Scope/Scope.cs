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
    
    /// <summary>
    /// This traverses upwards in the scope and finds a matching declaration. If more than one are found, the first is returned
    /// </summary>
    /// <param name="name"></param>
    /// <returns>First found declaration or null</returns>
    public VariableDeclaration? FindVariableDeclarationByName(string name)
    {
        var currentScopeDeclarations = VariableDeclarations.FirstOrDefault(d => d.VariableName == name);
        if(currentScopeDeclarations != null) return currentScopeDeclarations;
        
        return OuterScope?.FindVariableDeclarationByName(name);
    }

    /// <summary>
    /// This traverses upwards in the scope and finds a matching declaration. If more than one are found, the first is returned
    /// </summary>
    /// <param name="name"></param>
    /// <returns>First found declaration or null</returns>
    public FunctionDeclaration? FindFunctionDeclarationByName(string name)
    {
        var currentScopeDeclarations = FunctionDeclarations.FirstOrDefault(d => d.Name == name);
        if(currentScopeDeclarations != null) return currentScopeDeclarations;
        return OuterScope?.FindFunctionDeclarationByName(name);
    }
    
}