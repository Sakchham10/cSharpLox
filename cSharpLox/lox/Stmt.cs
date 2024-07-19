namespace interpreter.lox;


public abstract class Stmt
{

    public abstract T accept<T>(Visitor<T> visitor);
    public interface Visitor<T>
    {
        T visitExpressionStmt(Expression stmt);
        T visitPrintStmt(Print stmt);
        T visitVarStmt(Var stmt);
    }
}
public class Expression : Stmt
{
    private Expression(Expr expression)
    {
        expression = expression;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitExpressionStmt(this);
    }

    public static Expression Create(Expr expression)
    {
        return new Expression(expression);
    }
    public Expr expression;
}
public class Print : Stmt
{
    private Print(Expr expression)
    {
        expression = expression;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitPrintStmt(this);
    }

    public static Print Create(Expr expression)
    {
        return new Print(expression);
    }
    public Expr expression;
}
public class Var : Stmt
{
    private Var(Token name, Expr initializer)
    {
        name = name;
        initializer = initializer;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitVarStmt(this);
    }

    public static Var Create(Token name, Expr initializer)
    {
        return new Var(name, initializer);
    }
    public Token name;
    public Expr initializer;
}
