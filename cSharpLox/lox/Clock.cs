using interpreter.lox;
public class Clock : LoxCallable
{
    public int arity()
    {
        return 0;
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
        return (double)DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000.0;
    }
}
