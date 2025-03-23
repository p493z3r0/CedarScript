using CedarScript.AST.Nodes;

namespace CedarScript.AST.Interpreter;

public class Interpreter
{
    public void Execute(ProgramNode program)
    {
        program.Execute();
    }
}