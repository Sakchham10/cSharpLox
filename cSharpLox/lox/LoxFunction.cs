namespace interpreter.lox;
public class LoxFunction : LoxCallable
{
    private readonly Function _declaration;
    private readonly Env _closure;
    public LoxFunction(Function declaration, Env closure)
    {
        _declaration = declaration;
        _closure = closure;
    }

    public int arity()
    {
        return _declaration._parameters.Count;
    }

    public override string ToString()
    {
        return "<fn " + _declaration._name.lexeme + " >";
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
        Env env = new Env(_closure);
        for (int i = 0; i < _declaration._parameters.Count; i++)
        {
            env.define(_declaration._parameters.ElementAt(i).lexeme, arguments.ElementAt(i));
        }
        try
        {
            interpreter.executeBlock(_declaration._body, env);
        }
        catch (Returnval returnVal)
        {
            return returnVal.value;
        }
        return null;
    }
}
