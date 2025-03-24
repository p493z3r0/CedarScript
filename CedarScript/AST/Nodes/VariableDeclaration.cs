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
    public bool IsReference { get; set; } = false;

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

        if (value.Type == TokenType.Identifier)
        {
            variableDeclaration.IsReference = true;
        }
        
        variableDeclaration.Value = ValueNode.FromValue(value.Value);
        Parser.Parser.ConsumeSemicolonIfNeeded(stream);
        return variableDeclaration;
    }
    public override ValueNode Execute(Scope.Scope scope)
    {
        if (!IsReference) throw new Exception("Variable declaration can only be executed if its a reference");
        if(Value == null) throw new NullReferenceException("Variable " + VariableName + " cannot be null on execution");
        var declaration = scope.FindVariableDeclarationByName(Value.AsString());
        if(declaration == null) throw new NullReferenceException("Variable " + VariableName + " has an unresolved reference with value " + Value.AsString());
        if (declaration.IsReference)
        {
            return declaration.Execute(scope);
        }

        return declaration.Value ?? ValueNode.FromInt(0);
    }
}