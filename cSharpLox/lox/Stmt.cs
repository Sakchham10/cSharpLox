namespace interpreter.lox;


public abstract class Stmt
{

    public abstract T accept<T>(Visitor<T> visitor);
    public interface Visitor<T>
    {
        T visitBlockStmt(Block stmt);
        T visitClassStmt(Class stmt);
        T visitExpressionStmt(Expression stmt);
        T visitFunctionStmt(Function stmt);
        T visitIfStmt(If stmt);
        T visitReturnsStmt(Returns stmt);
        T visitPrintStmt(Print stmt);
        T visitVarStmt(Var stmt);
        T visitWhileStmt(While stmt);
    }
}
public class Block : Stmt
{
    private Block(List<Stmt> statements)
    {
        _statements = statements;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitBlockStmt(this);
    }

    public static Block Create(List<Stmt> statements)
    {
        return new Block(statements);
    }
    public List<Stmt> _statements;
}
public class Class : Stmt
{
    private Class(Token name, List<Function> methods)
    {
        _name = name;
        _methods = methods;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitClassStmt(this);
    }

    public static Class Create(Token name, List<Function> methods)
    {
        return new Class(name, methods);
    }
    public Token _name;
    public List<Function> _methods;
}
public class Expression : Stmt
{
    private Expression(Expr expression)
    {
        _expression = expression;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitExpressionStmt(this);
    }

    public static Expression Create(Expr expression)
    {
        return new Expression(expression);
    }
    public Expr _expression;
}
public class Function : Stmt
{
    private Function(Token name, List<Token> parameters, List<Stmt> body)
    {
        _name = name;
        _parameters = parameters;
        _body = body;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitFunctionStmt(this);
    }

    public static Function Create(Token name, List<Token> parameters, List<Stmt> body)
    {
        return new Function(name, parameters, body);
    }
    public Token _name;
    public List<Token> _parameters;
    public List<Stmt> _body;
}
public class If : Stmt
{
    private If(Expr condition, Stmt thenBranch, Stmt elseBranch)
    {
        _condition = condition;
        _thenBranch = thenBranch;
        _elseBranch = elseBranch;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitIfStmt(this);
    }

    public static If Create(Expr condition, Stmt thenBranch, Stmt elseBranch)
    {
        return new If(condition, thenBranch, elseBranch);
    }
    public Expr _condition;
    public Stmt _thenBranch;
    public Stmt _elseBranch;
}
public class Returns : Stmt
{
    private Returns(Token keyword, Expr value)
    {
        _keyword = keyword;
        _value = value;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitReturnsStmt(this);
    }

    public static Returns Create(Token keyword, Expr value)
    {
        return new Returns(keyword, value);
    }
    public Token _keyword;
    public Expr _value;
}
public class Print : Stmt
{
    private Print(Expr expression)
    {
        _expression = expression;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitPrintStmt(this);
    }

    public static Print Create(Expr expression)
    {
        return new Print(expression);
    }
    public Expr _expression;
}
public class Var : Stmt
{
    private Var(Token name, Expr initializer)
    {
        _name = name;
        _initializer = initializer;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitVarStmt(this);
    }

    public static Var Create(Token name, Expr initializer)
    {
        return new Var(name, initializer);
    }
    public Token _name;
    public Expr _initializer;
}
public class While : Stmt
{
    private While(Expr condition, Stmt body)
    {
        _condition = condition;
        _body = body;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitWhileStmt(this);
    }

    public static While Create(Expr condition, Stmt body)
    {
        return new While(condition, body);
    }
    public Expr _condition;
    public Stmt _body;
}
