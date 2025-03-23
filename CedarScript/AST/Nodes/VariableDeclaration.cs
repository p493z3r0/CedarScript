using System.Diagnostics;
using System.Runtime.CompilerServices;
using CedarScript.Parser;

namespace CedarScript.AST.Nodes;

public class VariableDeclaration : BlockNode
{
    public override bool DoesAutoExecute { get; set; } = false;
    public string VariableName { get; set; } = "";
    public ValueNode? Value { get; set; }
    public bool IsConstant { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsNullable { get; set; }

    public new static VariableDeclaration FromToken(Token token, TokenStream stream)
    {
        var variableDeclaration = new VariableDeclaration();
        var identifierUntilType = stream.ConsumeUntilType(TokenType.Identifier, [
            TokenType.String,
            TokenType.Keyword,
            TokenType.Numeric,
            TokenType.Punctuator
        ]);
        if (identifierUntilType.Count > 1) throw new Exception("Identifier was not next to keyword. bailing");
        variableDeclaration.VariableName = identifierUntilType.First().Value;
        
        // check if we have an assignement

        if (!stream.Peek().Value.Equals("="))
        {
            variableDeclaration.IsNullable = true;
            Parser.Parser.ConsumeSemicolonIfNeeded(stream);
            return variableDeclaration;
        }

        var value = stream.ConsumeNext(1);
        
        Debug.Assert(value.Type == TokenType.Numeric);
        
        variableDeclaration.Value = ValueNode.FromInt(int.Parse(value.Value));
        Parser.Parser.ConsumeSemicolonIfNeeded(stream);
        return variableDeclaration;
    }
    public override ValueNode Execute(Scope.Scope scope)
    {
        throw new NotImplementedException("Variable Declaration can not be executed, tried executing " + VariableName);
    }
}