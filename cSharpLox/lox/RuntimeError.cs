namespace interpreter.lox
{
    public class RuntimeError : Exception
    {
        public Token token { get; private set; }
        internal RuntimeError(Token token, string message) : base(message)
        {
            this.token = token;
        }
    }
}
