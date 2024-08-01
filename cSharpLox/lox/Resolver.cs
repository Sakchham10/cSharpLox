namespace interpreter.lox;

public class Resolver : Expr.Visitor<object?>, Stmt.Visitor<object?>
{
    private readonly Interpreter _interpreter;
    private readonly Stack<Dictionary<string, bool>> scopes = new Stack<Dictionary<string, bool>>();
    private FunctionType currentFunction = FunctionType.NONE;
    public Resolver(Interpreter interpreter)
    {
        _interpreter = interpreter;
    }

    public object? visitAssignExpr(Assign expr)
    {
        resolve(expr._value);
        resolveLocal(expr, expr._name);
        return null;
    }

    public object? visitBinaryExpr(Binary expr)
    {
        resolve(expr._left);
        resolve(expr._right);
        return null;
    }

    public object? visitBlockStmt(Block stmt)
    {
        beginScope();
        resolve(stmt._statements);
        endScope();
        return null;
    }

    public object? visitCallExpr(Call expr)
    {
        resolve(expr._callee);
        foreach (Expr argument in expr._arguments)
        {
            resolve(argument);
        }
        return null;
    }

    public object? visitExpressionStmt(Expression stmt)
    {
        resolve(stmt._expression);
        return null;
    }

    public object? visitFunctionStmt(Function stmt)
    {
        declare(stmt._name);
        define(stmt._name);
        resolveFunction(stmt, FunctionType.FUNCTION);
        return null;
    }

    public object? visitGroupingExpr(Grouping expr)
    {
        resolve(expr._expression);
        return null;
    }

    public object? visitIfStmt(If stmt)
    {
        resolve(stmt._condition);
        resolve(stmt._thenBranch);
        if (stmt._elseBranch != null)
        {
            resolve(stmt._elseBranch);
        }
        return null;
    }

    public object? visitLiteralExpr(Literal expr)
    {
        return null;
    }

    public object? visitLogicalExpr(Logical expr)
    {
        resolve(expr._left);
        resolve(expr._right);
        return null;

    }

    public object? visitPrintStmt(Print stmt)
    {
        resolve(stmt._expression);
        return null;
    }


    public object? visitUnaryExpr(Unary expr)
    {
        resolve(expr._right);
        return null;
    }

    public object? visitVariableExpr(Variable expr)
    {
        if (scopes.Any() && scopes.Peek()[expr._name.lexeme] == false)
        {
            Lox.error(expr._name, "Can't read local variable in its own initializer.");
        }
        resolveLocal(expr, expr._name);
        return null;
    }

    public object? visitVarStmt(Var stmt)
    {
        declare(stmt._name);
        if (stmt._initializer != null)
        {
            resolve(stmt._initializer);
        }
        define(stmt._name);
        return null;

    }

    public object? visitWhileStmt(While stmt)
    {
        resolve(stmt._condition);
        resolve(stmt._body);
        return null;
    }

    public void resolve(List<Stmt> statements)
    {
        foreach (Stmt statement in statements)
        {
            resolve(statement);
        }
    }

    private void resolve(Stmt statement)
    {
        statement.accept(this);
    }

    private void resolve(Expr expression)
    {
        expression.accept(this);
    }

    private void beginScope()
    {
        scopes.Push(new Dictionary<string, bool>());
    }

    private void endScope()
    {
        scopes.Pop();
    }

    private void declare(Token name)
    {
        if (!scopes.Any()) return;
        Dictionary<string, bool> scope = scopes.Peek();
        if (scope.TryGetValue(name.lexeme, out bool value))
        {
            Lox.error(name, "Variable with this name already exists in the scope.");
        }
        scope.Add(name.lexeme, false);
    }

    private void define(Token name)
    {
        if (!scopes.Any()) return;
        scopes.Peek()[name.lexeme] = true;
    }

    private void resolveLocal(Expr expression, Token name)
    {
        for (int i = scopes.Count - 1; i >= 0; i--)
        {
            if (scopes.ElementAt(i).ContainsKey(name.lexeme))
            {
                _interpreter.resolve(expression, scopes.Count - i - 1);
                return;
            }
        }
    }

    private void resolveFunction(Function function, FunctionType type)
    {
        FunctionType enclosingFunction = currentFunction;
        currentFunction = type;
        beginScope();
        foreach (Token param in function._parameters)
        {
            declare(param);
            define(param);
        }
        resolve(function._body);
        currentFunction = enclosingFunction;
        endScope();
    }

    public object? visitClassStmt(Class stmt)
    {
        declare(stmt._name);
        define(stmt._name);
        foreach (Function method in stmt._methods)
        {
            FunctionType declaration = FunctionType.METHOD;
            resolveFunction(method, declaration);
        }
        return null;
    }

    public object? visitGetExpr(Get expr)
    {
        resolve(expr._expression);
        return null;
    }

    public object? visitSetExpr(Set expr)
    {
        resolve(expr._value);
        resolve(expr._expression);
        return null;
    }

    public object? visitReturnsStmt(Returns stmt)
    {
        if (currentFunction == FunctionType.NONE)
        {
            Lox.error(stmt._keyword, "Can't return from top-level code.");
        }
        if (stmt._value != null)
        {
            resolve(stmt._value);
        }
        return null;
    }
}
