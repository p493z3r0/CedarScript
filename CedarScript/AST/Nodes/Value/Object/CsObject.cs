using CedarScript.Parser;

namespace CedarScript.AST.Nodes.Value.Object;

public class CsObject
{
    public Dictionary<string, ValueNode> Properties { get; set; } = new();

    public ValueNode GetProperty(string name)
    {
        return Properties.TryGetValue(name, out var property) ? property : UndefinedValueNode.Create();
    }

    public void SetProperty(string name, ValueNode value)
    {
        Properties[name] = value;
    }

    public static CsObject FromToken(Token token, TokenStream tokenStream)
    {
        if (token.Value != "{") throw new Exception("Object from Token must start with an opening scope");

        int objectDefinitionEndIndex = tokenStream.GetNextScopeClosureTokenIndex();

        /*
         * {
         *   property : "value",
         *   property2 : 12,
         * }
         *
         * 
         */
        
     
        var csObject = new CsObject();
        while (tokenStream.IsTokenAvailableWithMaxIndex(objectDefinitionEndIndex-1))
        {
            var property = tokenStream.ConsumeUntilType(TokenType.Identifier).Last();
            if (!tokenStream.Peek().Value.Equals(":"))
            {
                throw new Exception("Object property must have a following :");
            }
            tokenStream.ConsumeNext(); // :
            var value = tokenStream.ConsumeNext();
            
            csObject.SetProperty(property.Value, ValueNode.FromValue(value.Value));
        }

        if (tokenStream.Peek().Value.Equals(","))
        {
            // Fix up weird formatting of weird users and consume this token
            tokenStream.ConsumeNext();
        }
        
        tokenStream.ConsumeNext(); // consumes the closing scope
        

        return csObject;
    }
}