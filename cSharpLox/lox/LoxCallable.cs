namespace interpreter.lox
{
    public interface LoxCallable
    {
        object call(Interpreter interpreter, List<object> arguments);
        int arity();
    }
}
