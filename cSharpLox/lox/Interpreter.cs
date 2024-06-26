using static interpreter.lox.TokenType;

namespace interpreter.lox
{
    public class Interpreter : Expr.Visitor<Object>
    {
        public void interpret(Expr expression)
        {
            try
            {
                Object value = evaluate(expression);
                Console.WriteLine(stringfy(value));
            }
            catch (RuntimeError err)
            {
                Lox.runTimeError(err);
            }
        }
        private string stringfy(Object value)
        {
            if (value == null) return "nill";
            if (value.GetType().Equals(typeof(double)))
            {
                string text = value.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }
            return value.ToString();
        }
        public object visitBinaryExpr(Binary expr)
        {
            Object left = evaluate(expr.left);
            Object right = evaluate(expr.right);
            switch (expr.oper.type)
            {
                case (GREATER_THAN):
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left > (double)right;
                case (GREATER_EQUAL):
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left >= (double)right;
                case (LESS_THAN):
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left > (double)right;
                case (LESS_EQUAL):
                    return (double)left >= (double)right;
                case (MINUS):
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left - (double)right;
                case (BANG_EQUAL):
                    return !isEqual(left, right);
                case (EQUAL_EQUAL):
                    checkNumberOperands(expr.oper, left, right);
                    return isEqual(left, right);
                case (PLUS):
                    if (left.GetType().Equals(typeof(double)) && right.GetType().Equals(typeof(double)))
                    {
                        return (double)left + (double)right;
                    }
                    if (left.GetType().Equals(typeof(string)) && right.GetType().Equals(typeof(string)))
                    {
                        return (string)left + (string)right;
                    }
                    throw new RuntimeError(expr.oper, "Operands must be two numbers or two strings");
                case (SLASH):
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left / (double)right;
                case (STAR):
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left * (double)right;
            }
            return null;
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
                    checkNumberOperand(expr.oper, right);
                    return -(double)right;
            }
            return null;

        }

        private void checkNumberOperand(Token oper, Object operand)
        {
            if (operand.GetType().Equals(typeof(double))) return;
            throw new RuntimeError(oper, "Operand must be a number");
        }

        private void checkNumberOperands(Token oper, Object operand1, Object operand2)
        {
            if (operand1.GetType().Equals(typeof(double)) && operand2.GetType().Equals(typeof(double))) return;
            throw new RuntimeError(oper, "Operands must be numbers");
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

        private bool isEqual(Object a, Object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            if (b == null) return false;
            return a.Equals(b);
        }
    }
}

