using DotNetCodeGenerator.Ast;

namespace DotNetCodeGenerator;

public class CSharpWriter
{
    private readonly CodeGenerator _codeGenerator;

    public CSharpWriter(TextWriter textWriter)
    {
        _codeGenerator = new CodeGenerator(textWriter);
    }

    public void GenerateClassCode(IEnumerable<Statement> methodsStatement)
    {
        var rootStatement = new Block();
        rootStatement.Statements.Add(new UsingDeclaration("System"));
        rootStatement.Statements.Add(new NamespaceDeclaration("generatedScope"));
        var classDeclaration = new ClassDeclaration("GeneratedClass");
        rootStatement.Statements.Add(classDeclaration);
        classDeclaration.Body.Statements.AddRange(methodsStatement);
        _codeGenerator.GenerateCode(rootStatement);
    }
}