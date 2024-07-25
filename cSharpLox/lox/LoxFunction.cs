namespace interpreter.lox;
public class LoxFunction : LoxCallable
{
    private readonly Function _declaration;
    public LoxFunction(Function declaration)
    {
        _declaration = declaration;
    }

    public int arity()
    {
        return _declaration._parameters.Count;
    }

    public string toString()
    {
        return "<fn " + _declaration._name.lexeme + " >";
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
        Env env = new Env(Interpreter.global);
        for (int i = 0; i < _declaration._parameters.Count; i++)
        {
            env.define(_declaration._parameters.ElementAt(i).lexeme, arguments.ElementAt(i));
            interpreter.executeBlock(_declaration._body, env);
        }
        return null;
    }
}
