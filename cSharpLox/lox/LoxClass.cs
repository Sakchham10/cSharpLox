
namespace interpreter.lox;

public class LoxClass : LoxCallable
{
    public readonly string _name;
    private readonly Dictionary<string, LoxFunction> _methods;

    public LoxClass(string name, Dictionary<string, LoxFunction> methods)
    {
        _name = name;
        _methods = methods;
    }

    public int arity()
    {
        return 0;
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
        LoxInstance instance = new LoxInstance(this);
        return instance;
    }

    public override string ToString()
    {
        return _name;
    }

    public LoxFunction? findMethod(string name)
    {
        if (_methods.TryGetValue(name, out LoxFunction value))
        {
            return value;
        }
        return null;
    }
}
