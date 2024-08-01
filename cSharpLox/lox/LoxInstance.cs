namespace interpreter.lox;

public class LoxInstance
{
    private LoxClass _klass;
    private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

    public LoxInstance(LoxClass klass)
    {
        _klass = klass;
    }

    public object get(Token name)
    {
        if (fields.TryGetValue(name.lexeme, out object value))
        {
            return value;
        }
        LoxFunction method = _klass.findMethod(name.lexeme);
        if (method != null) return method;
        throw new RuntimeError(name, "Undefined property '" + name.lexeme + "'.");
    }

    public void set(Token name, object value)
    {
        if (fields.TryGetValue(name.lexeme, out object existing))
        {
            fields[name.lexeme] = value;
        }
        else
        {
            fields.Add(name.lexeme, value);
        }
    }

    public override string ToString()
    {
        return _klass._name + " instance";
    }


}
