namespace interpreter.lox;


public abstract class Expr
{

    public abstract T accept<T>(Visitor<T> visitor);
    public interface Visitor<T>
    {
        T visitAssignExpr(Assign expr);
        T visitBinaryExpr(Binary expr);
        T visitCallExpr(Call expr);
        T visitGetExpr(Get expr);
        T visitGroupingExpr(Grouping expr);
        T visitLiteralExpr(Literal expr);
        T visitLogicalExpr(Logical expr);
        T visitSetExpr(Set expr);
        T visitUnaryExpr(Unary expr);
        T visitVariableExpr(Variable expr);
    }
}
public class Assign : Expr
{
    private Assign(Token name, Expr value)
    {
        _name = name;
        _value = value;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitAssignExpr(this);
    }

    public static Assign Create(Token name, Expr value)
    {
        return new Assign(name, value);
    }
    public Token _name;
    public Expr _value;
}
public class Binary : Expr
{
    private Binary(Expr left, Token oper, Expr right)
    {
        _left = left;
        _oper = oper;
        _right = right;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitBinaryExpr(this);
    }

    public static Binary Create(Expr left, Token oper, Expr right)
    {
        return new Binary(left, oper, right);
    }
    public Expr _left;
    public Token _oper;
    public Expr _right;
}
public class Call : Expr
{
    private Call(Expr callee, Token paren, List<Expr> arguments)
    {
        _callee = callee;
        _paren = paren;
        _arguments = arguments;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitCallExpr(this);
    }

    public static Call Create(Expr callee, Token paren, List<Expr> arguments)
    {
        return new Call(callee, paren, arguments);
    }
    public Expr _callee;
    public Token _paren;
    public List<Expr> _arguments;
}
public class Get : Expr
{
    private Get(Expr expression, Token name)
    {
        _expression = expression;
        _name = name;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitGetExpr(this);
    }

    public static Get Create(Expr expression, Token name)
    {
        return new Get(expression, name);
    }
    public Expr _expression;
    public Token _name;
}
public class Grouping : Expr
{
    private Grouping(Expr expression)
    {
        _expression = expression;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitGroupingExpr(this);
    }

    public static Grouping Create(Expr expression)
    {
        return new Grouping(expression);
    }
    public Expr _expression;
}
public class Literal : Expr
{
    private Literal(Object value)
    {
        _value = value;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitLiteralExpr(this);
    }

    public static Literal Create(Object value)
    {
        return new Literal(value);
    }
    public Object _value;
}
public class Logical : Expr
{
    private Logical(Expr left, Token oper, Expr right)
    {
        _left = left;
        _oper = oper;
        _right = right;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitLogicalExpr(this);
    }

    public static Logical Create(Expr left, Token oper, Expr right)
    {
        return new Logical(left, oper, right);
    }
    public Expr _left;
    public Token _oper;
    public Expr _right;
}
public class Set : Expr
{
    private Set(Expr expression, Token name, Expr value)
    {
        _expression = expression;
        _name = name;
        _value = value;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitSetExpr(this);
    }

    public static Set Create(Expr expression, Token name, Expr value)
    {
        return new Set(expression, name, value);
    }
    public Expr _expression;
    public Token _name;
    public Expr _value;
}
public class Unary : Expr
{
    private Unary(Token oper, Expr right)
    {
        _oper = oper;
        _right = right;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitUnaryExpr(this);
    }

    public static Unary Create(Token oper, Expr right)
    {
        return new Unary(oper, right);
    }
    public Token _oper;
    public Expr _right;
}
public class Variable : Expr
{
    private Variable(Token name)
    {
        _name = name;
    }

    public override T accept<T>(Visitor<T> visitor)
    {
        return visitor.visitVariableExpr(this);
    }

    public static Variable Create(Token name)
    {
        return new Variable(name);
    }
    public Token _name;
}
