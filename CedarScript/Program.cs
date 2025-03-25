// See https://aka.ms/new-console-template for more information

using CedarScript.AST.Globals;
using CedarScript.AST.Interpreter;

namespace CedarScript;

static class Program
{
    public static void Main(string[] args)
    {
        if(args.Length == 0 && !Settings.IsDebugEnabled) throw new ArgumentException("Please specify at least 1 argument"); 
        string programText = "";
        if (!Settings.IsDebugEnabled && File.Exists(args[0]))
        {
            programText = File.ReadAllText(args[0]);
        }
        else
        {
            if (Settings.IsDebugEnabled)
            {
                programText = File.ReadAllText("../../../Parser/Test/main.cedar");
            }else throw new FileNotFoundException(args[0]);
            
        }
        var parser = new Parser.Parser();

        var ast = parser.ParseProgram(programText);

        var interpreter = new Interpreter();

        interpreter.Execute(ast);

    }
}