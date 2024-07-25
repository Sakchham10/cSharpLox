using static interpreter.lox.TokenType;
namespace interpreter.lox
{
    public class Interpreter : Expr.Visitor<Object>, Stmt.Visitor<Expression?>
    {
        public readonly static Env global = new Env();
        private Env environment = global;
        public Interpreter()
        {
            global.define("Clock", new Clock());

        }
        public void interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    execute(statement);
                }
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
            Object left = evaluate(expr._left);
            Object right = evaluate(expr._right);
            switch (expr._oper.type)
            {
                case (GREATER_THAN):
                    checkNumberOperands(expr._oper, left, right);
                    return (double)left > (double)right;
                case (GREATER_EQUAL):
                    checkNumberOperands(expr._oper, left, right);
                    return (double)left >= (double)right;
                case (LESS_THAN):
                    checkNumberOperands(expr._oper, left, right);
                    return (double)left < (double)right;
                case (LESS_EQUAL):
                    return (double)left <= (double)right;
                case (MINUS):
                    checkNumberOperands(expr._oper, left, right);
                    return (double)left - (double)right;
                case (BANG_EQUAL):
                    return !isEqual(left, right);
                case (EQUAL_EQUAL):
                    checkNumberOperands(expr._oper, left, right);
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
                    throw new RuntimeError(expr._oper, "Operands must be two numbers or two strings");
                case (SLASH):
                    checkNumberOperands(expr._oper, left, right);
                    if ((double)right == (double)0)
                        throw new RuntimeError(expr._oper, "Cannot divide by 0");
                    return (double)left / (double)right;
                case (STAR):
                    checkNumberOperands(expr._oper, left, right);
                    return (double)left * (double)right;
            }
            return null;
        }

        public object visitGroupingExpr(Grouping expr)
        {
            return evaluate(expr._expression);
        }

        public object visitLiteralExpr(Literal expr)
        {
            return expr._value;
        }

        public object visitUnaryExpr(Unary expr)
        {
            Object right = evaluate(expr._right);
            switch (expr._oper.type)
            {
                case (BANG):
                    return !isTruthy(right);
                case (MINUS):
                    checkNumberOperand(expr._oper, right);
                    return -(double)right;
            }
            return null;

        }

        public object visitAssignExpr(Assign expr)
        {
            object value = evaluate(expr._value);
            environment.assign(expr._name, value);
            return value;
        }

        private void checkNumberOperand(Token oper, Object operand)
        {
            if (operand is double) return;
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

        private void execute(Stmt stmt)
        {
            stmt.accept(this);
        }

        //Using nullable expression because c# doesn't allow void to be a generic function type return
        public Expression? visitExpressionStmt(Expression stmt)
        {
            evaluate(stmt._expression);
            return null;
        }

        public Expression? visitPrintStmt(Print stmt)
        {
            Object value = evaluate(stmt._expression);
            Console.WriteLine(stringfy(value));
            return null;
        }

        public Expression? visitVarStmt(Var stmt)
        {
            Object value = null;
            if (stmt._initializer != null)
            {
                value = evaluate(stmt._initializer);
            }
            environment.define(stmt._name.lexeme, value);
            return null;
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

        public void executeBlock(List<Stmt> statements, Env enlosing)
        {
            Env previous = this.environment;
            try
            {
                this.environment = enlosing;
                foreach (Stmt statement in statements)
                {
                    execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }

        }

        public object visitVariableExpr(Variable expr)
        {
            return environment.get(expr._name);
        }

        public Expression? visitBlockStmt(Block stmt)
        {
            executeBlock(stmt._statements, new Env(environment));
            return null;
        }

        public Expression? visitIfStmt(If stmt)
        {
            if (isTruthy(evaluate(stmt._condition)))
            {
                execute(stmt._thenBranch);
            }
            else if (isTruthy(stmt._elseBranch != null))
            {
                execute(stmt._elseBranch);
            }
            return null;

        }

        public object visitLogicalExpr(Logical expr)
        {
            object left = evaluate(expr._left);
            if (expr._oper.type == OR)
            {
                if (isTruthy(left)) return left;
            }
            else
            {
                if (!isTruthy(left)) return left;
            }
            return evaluate(expr._right);
        }

        public Expression? visitWhileStmt(While stmt)
        {
            while (isTruthy(evaluate(stmt._condition)))
            {
                execute(stmt._body);
            }
            return null;
        }

        public object visitCallExpr(Call expr)
        {
            object callee = evaluate(expr._callee);
            List<object> arguments = new List<object>();
            foreach (Expr argument in expr._arguments)
            {
                arguments.Add(evaluate(argument));
            }
            if (!(callee is LoxCallable))
            {
                throw new RuntimeError(expr._paren, "Can only call functions and classes.");
            }
            LoxCallable function = (LoxCallable)callee;
            if (arguments.Count != function.arity())
            {
                throw new RuntimeError(expr._paren, "Expected " + function.arity() + " arguments but got " + arguments.Count + ".");
            }
            return function.call(this, arguments);
        }

        public Expression? visitFunctionStmt(Function stmt)
        {
            LoxFunction function = new LoxFunction(stmt, environment);
            environment.define(stmt._name.lexeme, function);
            return null;
        }

        public Expression? visitReturnStmt(Returns stmt)
        {
            object value = null;
            if (stmt._value != null) value = evaluate(stmt._value);
            throw new Returnval(value);
        }
    }
}

