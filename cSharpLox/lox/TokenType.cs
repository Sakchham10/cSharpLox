namespace interpreter.lox.tokentype
{
    public enum TokenType
    {
        //single-character-tokens
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        COMMA, DOT, MINUS, PLUS, STAR, SEMICOLON, SLASH,
        //one or two character tokens
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER_EQUAL, LESS_EQUAL,
        LESS_THAN, GREATER_THAN,
        //literals
        IDENTIFIER, STRING, NUMBER,
        //keywords
        AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
        PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

        EOF,
    }
}
