using DotNetCodeGenerator.Ast;

namespace DotNetCodeGenerator;

public class CodeGenerator : IExpressionVisitor, IStatementVisitor
{
    private readonly TextWriter _textWriter;
    private int _indentLevel = 0;

    public CodeGenerator(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }

    public void GenerateCode(Statement statement)
    {
        statement.Accept(this);
    }

    private void Indent() => _indentLevel++;
    private void DeIndent() => _indentLevel--;

    private void WriteNoIndent(string content)
    {
        _textWriter.Write(content);
    }

    private void Write(string content)
    {
        for (var i = 0; i < _indentLevel; i++)
        {
            WriteNoIndent("    ");
        }
        WriteNoIndent(content);
    }

    private void WriteLine(string content)
    {
        Write(content + Environment.NewLine);
    }

    private void NewLine()
    {
        _textWriter.WriteLine();
    }

    private void WriteComa()
    {
        WriteNoIndent(";");
    }

    private static string GetStringFromToken(Token token)
    {
        return $"{token.Lexeme} {token.Literal}";
    }

    private void WriteNoIndent(Token token)
    {
        WriteNoIndent(GetStringFromToken(token));
    }

    private void Write(Token token)
    {
        Write(GetStringFromToken(token));
    }

    private void BeginScope()
    {
        WriteLine("{");
        Indent();
    }

    private void EndScope()
    {
        DeIndent();
        WriteLine("}");
    }

    private void BeginParenthesis()
    {
        WriteNoIndent("(");
    }

    private void EndParenthesis()
    {
        WriteNoIndent(")");
    }


    public void VisitAssignExpression(Assign expression)
    {
        throw new NotImplementedException();
        //return $"Assign {expression.name.lexeme}";
    }

    public void VisitBinaryExpression(Binary expression)
    {
        expression.Left.Accept(this);
        Write($" {expression.Op.Lexeme} ");
        expression.Right.Accept(this);
    }

    public void VisitGroupingExpression(Grouping expression)
    {
        throw new NotImplementedException();
    }

    public void VisitLiteralExpression(Literal expression)
    {
        Write(expression.Value == null ? "null" : expression.Value.ToString());
    }

    public void VisitLogicalExpression(Logical expression)
    {
        throw new NotImplementedException();
        //return $"{expression.op.lexeme}";
    }

    public void VisitUnaryExpression(Unary expression)
    {
        BeginParenthesis();
        Write($"{expression.Op.Lexeme}{expression.Right}");
        EndParenthesis();
    }

    public void VisitVariableExpression(VariableExpression expression)
    {
        Write($"{expression.Name.Literal}");
    }

    public void VisitStatement(Block blockStatement)
    {
        foreach (var statement in blockStatement.Statements)
        {
            statement.Accept(this);
        }
    }

    public void VisitStatement(Scope scopeStatement)
    {
        BeginScope();

        foreach (var statement in scopeStatement.Statements)
        {
            statement.Accept(this);
        }

        EndScope();
    }

    public void VisitStatement(StatementExpression statement)
    {
        statement.Accept(this);
    }

    public void VisitStatement(If statement)
    {
        Write("if ");

        BeginParenthesis();
        statement.Condition.Accept(this);
        EndParenthesis();

        BeginScope();
        statement.ThenBranch.Accept(this);
        EndScope();

        if (statement.ElseBranch != null)
        {
            Write("else");
            BeginScope();
            statement.ElseBranch.Accept(this);
            EndScope();
        }
    }

    public void VisitStatement(UsingDeclaration statement)
    {
        Write(statement.Name);
        WriteComa();
        NewLine();
    }

    public void VisitStatement(ClassDeclaration statement)
    {
        Write(statement.Name);

        if (!string.IsNullOrEmpty(statement.Inhereted))
        {
            WriteNoIndent($" : {statement.Inhereted}");
        }

        NewLine();
        statement.Body.Accept(this);
    }

    public void VisitStatement(NamespaceDeclaration statement)
    {
        Write(statement.Name);
        WriteComa();
        NewLine();
    }

    public void VisitStatement(LiteralAccessor statement)
    {
        string? content = statement.Literal.Value switch
        {
            null => "null",
            string => $"\"{statement.Literal.Value}\"",
            Token token => $"{token.Literal}",
            _ => $"{statement.Literal.Value}"
        };

        WriteNoIndent(content);
    }

    public void VisitStatement(VariableStatement statement)
    {
        Write(statement.Name);
        Write(" = ");
        statement.Initializer.Accept(this);
        WriteComa();
        NewLine();
    }

    public void VisitStatement(While statement)
    {
        throw new NotImplementedException();
    }

    public void VisitStatement(Break statement)
    {
        throw new NotImplementedException();
    }

    public void VisitStatement(PropertyDeclaration statement)
    {
        //public override int ExternalComponentId => (int)ScriptIds.ArcBallCamera;
        Write("public ");
        WriteNoIndent(statement.Name);
        WriteNoIndent(" => -1");
        WriteComa();
        NewLine();
    }

    public void VisitStatement(FunctionCall statement)
    {
        Write(statement.Name);
        BeginParenthesis();

        var first = true;
        foreach (var parameter in statement.Parameters)
        {
            if (!first)
            {
                WriteNoIndent(", ");
            }
            parameter.Accept(this);
            first = false;
        }

        EndParenthesis();
        WriteComa();
        NewLine();
    }

    public void VisitStatement(FunctionDeclaration statement)
    {
        Write("public ");

        if (statement.IsOverride)
        {
            WriteNoIndent("override ");
        }

        WriteNoIndent(statement.Name);
        BeginParenthesis();

        var first = true;
        foreach (var parameter in statement.Parameters)
        {
            if (!first)
            {
                Write(", ");
            }
            Write(parameter);
            first = false;
        }

        EndParenthesis();
        NewLine();
        statement.Body.Accept(this);
    }
}