using interpreter.lox.token;
namespace interpreter.lox.expr;


public abstract class Expr
{

    protected abstract T accept<T>(Visitor<T> visitor);
    protected interface Visitor<T>
    {
        T visitBinaryExpr(Binary expr);
        T visitGroupingExpr(Grouping expr);
        T visitLiteralExpr(Literal expr);
        T visitUnaryExpr(Unary expr);
    }
}
public class Binary : Expr
{
    private Binary(Expr left, Token Operator, Expr right)
    {
        _left = left;
        _Operator = Operator;
        _right = right;
    }

    protected override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitBinaryExpr(this);
    }

    public static Binary Create(Expr left, Token Operator, Expr right)
    {
        return new Binary(left, Operator, right);
    }
    static Expr _left;
    static Token _Operator;
    static Expr _right;
}
public class Grouping : Expr
{
    private Grouping(Expr expression)
    {
        _expression = expression;
    }

    protected override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitGroupingExpr(this);
    }

    public static Grouping Create(Expr expression)
    {
        return new Grouping(expression);
    }
    static Expr _expression;
}
public class Literal : Expr
{
    private Literal(Object value)
    {
        _value = value;
    }

    protected override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitLiteralExpr(this);
    }

    public static Literal Create(Object value)
    {
        return new Literal(value);
    }
    static Object _value;
}
public class Unary : Expr
{
    private Unary(Token Operator, Expr right)
    {
        _Operator = Operator;
        _right = right;
    }

    protected override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitUnaryExpr(this);
    }

    public static Unary Create(Token Operator, Expr right)
    {
        return new Unary(Operator, right);
    }
    static Token _Operator;
    static Expr _right;
}
