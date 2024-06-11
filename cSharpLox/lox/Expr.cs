using interpreter.lox.token;
namespace interpreter.lox
{
    public abstract class Expr
    {
    }
    public class Binary : Expr
    {
        private Binary(Expr left, Token Operator, Expr right)
        {
            _left = left;
            _Operator = Operator;
            _right = right;
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

        public static Unary Create(Token Operator, Expr right)
        {
            return new Unary(Operator, right);
        }
        static Token _Operator;
        static Expr _right;
    }
}
