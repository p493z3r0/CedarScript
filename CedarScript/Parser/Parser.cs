using System.Data;
using System.Linq.Expressions;
using CedarScript.AST.Expressions;
using CedarScript.AST.Nodes;
using BinaryExpression = CedarScript.AST.Expressions.BinaryExpression;
using Expression = CedarScript.AST.Expressions.Expression;

namespace CedarScript.Parser;

public class Parser
{
    public ProgramNode ParseProgram(string program)
    {
        var tokens = Tokenizer.Tokenize(program);

        var ast = new ProgramNode();
        
        if (!Tokenizer.ValidateTokens(tokens))
        {
            throw new SyntaxErrorException("Preliminary check failed. You suck");
        }

        for (int i = 0; i < tokens.Count; i++)
        {
            var currentNode = new BlockNode();
            var token = tokens[i];
            if (token.Type == TokenType.Keyword)
            {
                if (token.Value == "function")
                {
                    var functionDeclaration = ParseFunctionDeclaration(tokens, i);
                    ast.Nodes.Add(functionDeclaration.Item1);
                    i = functionDeclaration.Item2;
                }

            }

            if (token.Type == TokenType.Identifier)
            {
                //maybe function call
                Console.WriteLine(token.Value);
                
                //check for function call 

                var leftBracket = GetTokenAtIndexOrNull(tokens, i + 1);
                var rightBracket = GetTokenAtIndexOrNull(tokens, i + 2);
                if(leftBracket == null || rightBracket == null) continue;
                if (leftBracket.Value == "(" && rightBracket.Value == ")")
                {
                    ast.Nodes.Add(new CallExpression()
                    {
                        Arguments = [],
                        Name = token.Value,
                    });
                } 


            }
        }

        return ast;
    }

    private Token? GetTokenAtIndexOrNull(List<Token> tokens, int index)
    {
        if (tokens.Count < index) return null;
        return tokens[index];
    }
    private (FunctionDeclaration, int) ParseFunctionDeclaration(List<Token> tokens, int index)
    {

        var functionName = GetTokenAtIndexOrNull(tokens, index+1);
        if(functionName == null) throw new SyntaxErrorException("Unexpected end of program");
        if (functionName.Type != TokenType.Identifier)
            throw new SyntaxErrorException("Expected identifier but got: " + nameof(functionName.Type));
        // we do skip arguments as they are not implemented yet but still check the validity
        
        var functionArgumentStart = GetTokenAtIndexOrNull(tokens, index+2);
        var functionArgumentEnd = GetTokenAtIndexOrNull(tokens, index+3);
        
        if(functionArgumentStart == null || functionArgumentEnd == null) throw new SyntaxErrorException("Unexpected end of program");
        if(functionArgumentStart.Type != TokenType.Punctuator) throw new SyntaxErrorException("Expected punctuator but got: " + (functionArgumentStart.Type));
        if(functionArgumentEnd.Type != TokenType.Punctuator) throw new SyntaxErrorException("Expected punctuator but got: " + (functionArgumentEnd.Type));
        
        // now we should get a block to parse we can check if the next one is a punctuator AND a {
       


        var block = ParseBlock(tokens, index + 4);

        var functionDeclaration = new FunctionDeclaration()
        {
            Name = functionName.Value,
            Body = block.Item1
        };


        return new ValueTuple<FunctionDeclaration, int>(functionDeclaration, block.Item2);
    }

    private (BlockNode, int) ParseBlock(List<Token> tokens, int index, bool isRoot = false)
    {
         
        var functionBodyStart = GetTokenAtIndexOrNull(tokens, index);
        if(functionBodyStart == null) throw new SyntaxErrorException("Unexpected end of program, function must have a body");
        if(functionBodyStart.Value != "{" && !isRoot) throw new SyntaxErrorException("Unexpected end of program, function must have a body indicated by { but got a" + functionBodyStart.Value);
        // now we have to find the ending token -> we have to make sure that we skip inner closures
        int encounteredScopeOpenings = 0;
        int closingScopeIndex = index+1;
        for (int i = index+1; i < tokens.Count; i++)
        {
            var token = GetTokenAtIndexOrNull(tokens, i);
            if(token == null) throw new SyntaxErrorException("Unexpected end of program, block must have an ending ");
            if (token.Value == "{")
            {
                encounteredScopeOpenings++;
                continue;
            }

            if (token.Value == "}")
            {
                if (encounteredScopeOpenings == 0)
                {
                    closingScopeIndex = i;
                    break;
                }
                encounteredScopeOpenings--;
            }
        }
        var firstTokenInsideBlock = GetTokenAtIndexOrNull(tokens, index+1);
        if(firstTokenInsideBlock == null) throw new SyntaxErrorException("Unexpected end of program, block must not be empty");

        if (firstTokenInsideBlock.Type != TokenType.Keyword)
            throw new SyntaxErrorException("First token in block must be a keyword. Got " + firstTokenInsideBlock.Type);
        
        var node = BlockNodeFromToken(firstTokenInsideBlock);

        if (node is FunctionDeclaration)
        {
           ParseFunctionDeclaration(tokens, index + 1);
        }

        if (node is VariableDeclaration declaration)
        {
            // do it inline since im lazy
            
            var name = GetTokenAtIndexOrNull(tokens, index+2);
            var possibleEqualsOrSemicolon = GetTokenAtIndexOrNull(tokens, index+3);
            var possibleAssignement = GetTokenAtIndexOrNull(tokens, index+4);

            if (name == null) throw new SyntaxErrorException("There must be an identifier after a var statement.");
            if(name.Type != TokenType.Identifier) throw new SyntaxErrorException("Expected identifier but got: " + name.Value);
            declaration.VariableName = name.Value;

            if(possibleEqualsOrSemicolon == null) throw new SyntaxErrorException("Unexpected end of program");
            if(possibleEqualsOrSemicolon.Type != TokenType.Punctuator) throw new SyntaxErrorException("Token after Identifier of a var must be punctuator.");
            if (possibleEqualsOrSemicolon.Value == "=")
            {
                if(possibleAssignement == null) throw new SyntaxErrorException("Unexpected end of program, = found but no assignement done");
                //we stupid and only support numbers
                if (possibleAssignement.Type != TokenType.Numeric)
                    throw new SyntaxErrorException(
                        "As the most advanced programming language we only support numeric variables for now");
                declaration.Value = ValueNode.FromInt(int.Parse(possibleAssignement.Value));
                declaration.DoesAutoExecute = true;
            }
            else
            {
                declaration.IsNullable = true;
            }



        }
        if (node is ReturnStatement statement)
        {
            var tokenAfterReturnStatement = GetTokenAtIndexOrNull(tokens, index+2);
            if(tokenAfterReturnStatement == null) throw new SyntaxErrorException("Unexpected end of program, block must have a return statement");
            if(tokenAfterReturnStatement.Type == TokenType.Identifier) throw new NotImplementedException("Variables not implemented, therefore not usable in a return statement.");
            // we assume that it must be an expression
            
            var expression = ParseExpression(tokens, index+2);
            statement.Argument = expression;
            
        }
        
        return new ValueTuple<BlockNode, int>(node, closingScopeIndex);
    }

    public BlockNode BlockNodeFromToken(Token token)
    {
        if(token.Type != TokenType.Keyword) throw new SyntaxErrorException("Tried to parse block node but its not a keyword");

        if (token.Value == "function") return new FunctionDeclaration();
        if (token.Value == "return") return new ReturnStatement();
        if (token.Value == "var") return new VariableDeclaration();
        
        throw new SyntaxErrorException("Unexpected value for block node got: " + token.Value);
    }

    public Expression ParseExpression(List<Token> tokens, int index)
    {
        var leftToken = GetTokenAtIndexOrNull(tokens, index);
        var operatorToken = GetTokenAtIndexOrNull(tokens, index+1);
        var rightToken = GetTokenAtIndexOrNull(tokens, index+2);
        if(leftToken == null) throw new SyntaxErrorException("Unexpected end of program");
        if(leftToken.Type != TokenType.Numeric) throw new SyntaxErrorException("Expression must start with a nummeric value");
        if(operatorToken == null) throw new SyntaxErrorException("Unexpected end of program");
        
        if(operatorToken.Value == ";") return LiteralExpression.FromInt(int.Parse(leftToken.Value)); // make sure we accept return 2;
        
        if(rightToken == null) throw new SyntaxErrorException("Unexpected end of program");
        if(operatorToken.Type != TokenType.Punctuator) throw new SyntaxErrorException("Expected punctuator but got: " + operatorToken.Type);
        var op = operatorToken.Value;
        var left = int.Parse(leftToken.Value);
        var right = int.Parse(rightToken.Value);

        var leftExpression = LiteralExpression.FromInt(left);
        var rightExpression = LiteralExpression.FromInt(right);
        var binaryExpression = BinaryExpression.WithLeftAndRight(leftExpression, rightExpression, op);
        
        return binaryExpression;
    }
}