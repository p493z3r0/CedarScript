using System.Runtime.CompilerServices;
using CedarScript.AST.Expressions;
using CedarScript.AST.Scope;
using CedarScript.Parser;

namespace CedarScript.AST.Nodes;

public class BlockNode
{
    /// <summary>
    /// Gets the current or inner scope (if defined)
    /// </summary>
    /// <param name="scope">Your current scope</param>
    /// <param name="type">Sets the type of the innerscope if not null</param>
    /// <returns></returns>
    public Scope.Scope GetScope(Scope.Scope scope, ScopeType type = Scope.ScopeType.Global)
    {
        if (scope.InnerScope != null)
        {
            scope.InnerScope.ScopeType = type;
            return scope.InnerScope;
        }
        return scope;
    }
    public List<BlockNode> Body { get; set; } = new();
    public virtual bool DoesAutoExecute { get; set; } = true;

    public virtual ValueNode Execute(Scope.Scope scope)
    {
        if (Body.Count == 0) throw new NotImplementedException();
        ValueNode lastReturn = ValueNode.FromInt(0);
        foreach (var block in Body)
        {
            if (block.DoesAutoExecute)
            {
                lastReturn = block.Execute(GetScope(scope));
            }
        }
        return lastReturn;
    }

    
    public static BlockNode FromToken(Token token, TokenStream tokenStream)
    {
        var blockNode = new BlockNode();
        var tokenIndex = tokenStream.GetNextScopeClosureTokenIndex();
        if(tokenIndex < 0) throw new ArgumentException("Block scope is missing closure");

        while (tokenStream.IsTokenAvailableWithMaxIndex(tokenIndex))
        {
            var consumedToken = tokenStream.ConsumeNext();
            var node = Parser.Parser.TokenHandler(consumedToken, tokenStream);
            blockNode.Body.Add(node);
        }
        
        return blockNode;
    }
}