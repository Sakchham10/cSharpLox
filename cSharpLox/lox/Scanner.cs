namespace interpreter.lox
{
    public class Scanner
    {
        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;
        private static readonly Dictionary<String, TokenType> keywords;

        static Scanner()
        {
            keywords = new Dictionary<String, TokenType>();
            keywords.Add("class", TokenType.CLASS);
            keywords.Add("else", TokenType.ELSE);
            keywords.Add("false", TokenType.FALSE);
            keywords.Add("for", TokenType.FOR);
            keywords.Add("fun", TokenType.FUN);
            keywords.Add("if", TokenType.IF);
            keywords.Add("nil", TokenType.NIL);
            keywords.Add("or", TokenType.OR);
            keywords.Add("print", TokenType.PRINT);
            keywords.Add("return", TokenType.RETURN);
            keywords.Add("super", TokenType.SUPER);
            keywords.Add("this", TokenType.THIS);
            keywords.Add("true", TokenType.TRUE);
            keywords.Add("var", TokenType.VAR);
            keywords.Add("while", TokenType.WHILE);
            keywords.Add("and", TokenType.AND);
        }

        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                start = current;
                scanToken();
            }
            Token newToken = new Token(TokenType.EOF, "", null, line);
            tokens.Add(newToken);
            return tokens;
        }

        private bool isAtEnd()
        {
            return current >= source.Length;
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(':
                    addToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    addToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    addToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    addToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    addToken(TokenType.COMMA);
                    break;
                case '.':
                    addToken(TokenType.DOT);
                    break;
                case '*':
                    addToken(TokenType.STAR);
                    break;
                case '+':
                    addToken(TokenType.PLUS);
                    break;
                case '-':
                    addToken(TokenType.MINUS);
                    break;
                case ';':
                    addToken(TokenType.SEMICOLON);
                    break;
                case '!':
                    addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '>':
                    addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER_THAN);
                    break;
                case '<':
                    addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS_THAN);
                    break;
                case '/':
                    if (match('/'))
                    {
                        //A comment goes until the end of line
                        while (peek() != '\n' && !isAtEnd()) advance();
                    }
                    else
                    {
                        addToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    //Ignore white space
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    stringFunc();
                    break;


                default:
                    if (isDigit(c))
                    {
                        numberFunc();
                        break;
                    }
                    else if (isAlpha(c))
                    {
                        identifierFunc();
                        break;
                    }
                    Lox.error(line, "Unexpected character.");
                    break;
            }
        }

        private char peek()
        {
            if (isAtEnd()) return '\0';
            return source.ElementAt(current);
        }

        private bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (expected != source.ElementAt(current)) return false;
            current++;
            return true;
        }

        private char advance()
        {
            return source.ElementAt(current++);
        }

        private void addToken(TokenType type)
        {
            addToken(type, null);
        }

        private void addToken(TokenType type, Object literal)
        {
            String text = subString(source, start, current);
            tokens.Add(new Token(type, text, literal, line));
        }

        private string subString(string text, int start, int end)
        {
            return text.Substring(start, end - start);
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void stringFunc()
        {
            while (peek() != '"' && !isAtEnd())
            {
                if (peek() == '\n') line++;
                advance();
            }

            if (isAtEnd())
            {
                Lox.error(line, "Unterminated string");
                return;
            }
            //Consume the ending " character
            advance();
            String value = subString(source, start + 1, current - 1);
            addToken(TokenType.STRING, value);
        }

        private void numberFunc()
        {
            while (isDigit(peek())) advance();
            if (peek() == '.' && isDigit(peekNext()))
            {
                advance();
                while (isDigit(peek())) advance();
            }
            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
        }

        private void identifierFunc()
        {
            while (isAlphaNumeric(peek()))
            {
                advance();
            }
            String text = source.Substring(start, current - start);
            keywords.TryGetValue(text, out TokenType type);
            if ((int)type == 0) type = TokenType.IDENTIFIER;
            addToken(type);
        }

        private bool isAlpha(char c)
        {
            return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c == '_'));
        }

        private bool isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        private char peekNext()
        {
            if (current + 1 > source.Length) return '\0';
            return source.ElementAt(current + 1);
        }

    }
}
