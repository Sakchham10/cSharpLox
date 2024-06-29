using interpreter.lox.expr;
namespace interpreter.lox
{
    class AstPrinter : Expr.Visitor<string>
    {
        public string print(Expr expr)
        {
            return expr.accept<string>(this);
        }

        public string visitBinaryExpr(Binary expr)
        {
            return paranthesize(expr.oper.lexeme, expr.left, expr.right);
        }

        public string visitGroupingExpr(Grouping expr)
        {
            return paranthesize("group", expr.expression);
        }

        public string visitLiteralExpr(Literal expr)
        {
            if (expr.value == null) return "nil";
            return expr.value.ToString();
        }

        public string visitUnaryExpr(Unary expr)
        {
            return paranthesize(expr.oper.lexeme, expr.right);
        }

        private string paranthesize(String name, params Expr[] expr)
        {
            StringWriter writer = new StringWriter();
            writer.Write("(");
            writer.Write(name);
            foreach (Expr expression in expr)
            {
                writer.Write(" ");
                writer.Write(expression.accept(this));
            }
            writer.Write(")");
            return writer.ToString();
        }
    }
}

