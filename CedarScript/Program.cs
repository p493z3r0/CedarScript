// See https://aka.ms/new-console-template for more information

using CedarScript.AST.Interpreter;
using CedarScript.Parser;

class Program
{
    public static void Main(string[] args)
    {
        
        var parser = new Parser();
        var ast = parser.ParseProgram("function main() { return 35; } var a = 12; main()");
        var interpreter = new Interpreter();
        interpreter.Execute(ast);
    }
}
