using System.Runtime.CompilerServices;
using CedarScript.AST.Expressions;
using CedarScript.Parser;

namespace CedarScript.AST.Nodes;

public class BlockNode
{
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
                lastReturn = block.Execute(scope);
            }
        }
        return lastReturn;
    }

    
    public static BlockNode FromToken(Token token, TokenStream tokenStream)
    {
        var blockNode = new BlockNode();
        var tokenIndex = tokenStream.Match(new Token()
        {
            Type = TokenType.Punctuator,
            Value = "}"
        });
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