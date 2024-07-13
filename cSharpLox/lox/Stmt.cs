namespace interpreter.lox;


public abstract class Stmt
{

    public abstract T accept<T>(Visitor<T> visitor);
    public interface Visitor<T>
    {
        T visitExpressionStmt(Expression stmt);
        T visitPrintStmt(Print stmt);
    }
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
