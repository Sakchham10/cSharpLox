namespace interpreter.lox;


public abstract class Expr
{

    public abstract T accept<T>(Visitor<T> visitor);
    public interface Visitor<T>
    {
        T visitBinaryExpr(Binary expr);
        T visitGroupingExpr(Grouping expr);
        T visitLiteralExpr(Literal expr);
        T visitUnaryExpr(Unary expr);
        T visitVariableExpr(Variable expr);
    }
}
public class Binary : Expr
{
    private Binary(Expr left, Token oper, Expr right)
    {
        left = left;
        oper = oper;
        right = right;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitBinaryExpr(this);
    }

    public static Binary Create(Expr left, Token oper, Expr right)
    {
        return new Binary(left, oper, right);
    }
    public Expr left;
    public Token oper;
    public Expr right;
}
public class Grouping : Expr
{
    private Grouping(Expr expression)
    {
        expression = expression;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitGroupingExpr(this);
    }

    public static Grouping Create(Expr expression)
    {
        return new Grouping(expression);
    }
    public Expr expression;
}
public class Literal : Expr
{
    private Literal(Object value)
    {
        value = value;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitLiteralExpr(this);
    }

    public static Literal Create(Object value)
    {
        return new Literal(value);
    }
    public Object value;
}
public class Unary : Expr
{
    private Unary(Token oper, Expr right)
    {
        oper = oper;
        right = right;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitUnaryExpr(this);
    }

    public static Unary Create(Token oper, Expr right)
    {
        return new Unary(oper, right);
    }
    public Token oper;
    public Expr right;
}
public class Variable : Expr
{
    private Variable(Token name)
    {
        name = name;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitVariableExpr(this);
    }

    public static Variable Create(Token name)
    {
        return new Variable(name);
    }
    public Token name;
}
