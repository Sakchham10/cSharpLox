namespace interpreter.lox;

public class Returnval : RuntimeError
{
    public object value { get; private set; }
    public Returnval(object value) : base(null, null)
    {
        this.value = value;
    }
}
