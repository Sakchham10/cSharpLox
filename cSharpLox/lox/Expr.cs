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
    }
}
public class Binary : Expr
{
    private Binary(Expr _left, Token _operator, Expr _right)
    {
        left = _left;
        oper = _operator;
        right = _right;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitBinaryExpr(this);
    }

    public static Binary Create(Expr left, Token Operator, Expr right)
    {
        return new Binary(left, Operator, right);
    }
    public Expr left;
    public Token oper;
    public Expr right;
}
public class Grouping : Expr
{
    private Grouping(Expr _expression)
    {
        expression = _expression;
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
    private Literal(Object _value)
    {
        value = _value;
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
    private Unary(Token _operator, Expr _right)
    {
        oper = _operator;
        right = _right;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitUnaryExpr(this);
    }

    public static Unary Create(Token Operator, Expr right)
    {
        return new Unary(Operator, right);
    }
    public Token oper;
    public Expr right;
}
