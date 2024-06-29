using interpreter.lox.expr;
using static interpreter.lox.tokentype.TokenType;

namespace interpreter.lox.interpreter
{
    public class Interpreter : Expr.Visitor<Object>
    {
        public object visitBinaryExpr(Binary expr)
        {
            Object left = evaluate(expr.left);
            Object right = evaluate(expr.right);
            switch (expr.oper.type)
            {
                case (MINUS):
                    return (double)left - (double)right;
                case (PLUS):
                    if (left.GetType().Equals(typeof(double)) && right.GetType().Equals(typeof(double)))
                    {
                        return (double)left + (double)right;
                    }
                    if (left.GetType().Equals(typeof(string)) && right.GetType().Equals(typeof(string)))
                    {
                        return (string)left + (string)right;
                    }
                    break;
                case (SLASH):
                    return (double)left / (double)right;
                case (STAR):
                    return (double)left * (double)right;
            }
        }

        public object visitGroupingExpr(Grouping expr)
        {
            return evaluate(expr.expression);
        }

        public object visitLiteralExpr(Literal expr)
        {
            return expr.value;
        }

        public object visitUnaryExpr(Unary expr)
        {
            Object right = evaluate(expr.right);
            switch (expr.oper.type)
            {
                case (BANG):
                    return !isTruthy(right);
                case (MINUS):
                    return -(double)right;
            }
            return null;

        }

        private object evaluate(Expr expr)
        {
            return expr.accept(this);
        }

        private bool isTruthy(Object obj)
        {
            if (obj == null) return false;
            if ((obj.GetType().Equals(typeof(bool)))) return (bool)obj;
            return true;
        }
    }
}

