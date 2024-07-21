using static interpreter.lox.TokenType;
namespace interpreter.lox
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;
        private class ParseError : Exception
        {
            public ParseError() : base() { }
        }
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }
        public List<Stmt> parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!isAtEnd())
            {
                statements.Add(declaration());
            }
            return statements;
        }

        private Stmt declaration()
        {
            try
            {
                if (match(VAR)) return varDeclaration();
                return statement();
            }
            catch (ParseError error)
            {
                synchronize();
                return null;
            }
        }

        private Expr expression()
        {
            return assignment();
        }
        private Stmt statement()
        {
            if (match(PRINT)) return printStatement();
            if (match(IF)) return ifStatement();
            if (match(WHILE)) return whileStatement();
            if (match(FOR)) return forStatement();
            if (match(LEFT_BRACE)) return Block.Create(block());
            return expressionStatement();
        }

        private Stmt forStatement()
        {
            consume(LEFT_PAREN, "Expect '(' after 'for'.");
            Stmt initializer;
            if (match(SEMICOLON))
            {
                initializer = null;
            }
            else if (match(VAR))
            {
                initializer = varDeclaration();
            }
            else
            {
                initializer = expressionStatement();
            }
            Expr condition = null;
            if (!check(SEMICOLON))
            {
                condition = expression();
            }
            consume(SEMICOLON, "Expect ';' after loop condition.");
            Expr increament = null;
            if (!check(RIGHT_PAREN))
            {
                increament = expression();
            }
            consume(RIGHT_PAREN, "Expect ')' after loop.");
            Stmt body = statement();
            if (increament != null)
            {
                body = Block.Create(new List<Stmt> { body, Expression.Create(increament) });
            }
            if (condition == null) condition = Literal.Create(true);
            if (initializer != null)
            {
                body = Block.Create(new List<Stmt> { initializer, body });
            }
            body = While.Create(condition, body);
            return body;
        }

        private Stmt whileStatement()
        {
            consume(LEFT_PAREN, "Expect '(' after 'while'.");
            Expr condition = expression();
            consume(RIGHT_PAREN, "Expect ')' after condition.");
            Stmt body = statement();
            return While.Create(condition, body);
        }

        private Stmt ifStatement()
        {
            consume(LEFT_PAREN, "Expect '(' after if.");
            Expr condition = expression();
            consume(RIGHT_PAREN, "Expect ')' after condition.");
            Stmt thenBranch = statement();
            Stmt elsebranch = null;
            if (match(ELSE))
            {
                elsebranch = statement();
            }
            return If.Create(condition, thenBranch, elsebranch);
        }

        private List<Stmt> block()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!check(RIGHT_BRACE) && !isAtEnd())
            {
                statements.Add(declaration());
            }
            consume(RIGHT_BRACE, "Expect '}' after block.");
            return statements;

        }

        private Stmt printStatement()
        {
            Expr expr = expression();
            consume(SEMICOLON, "Expect ';' after value.");
            return Print.Create(expr);
        }

        private Stmt varDeclaration()
        {
            Token name = consume(IDENTIFIER, "Expect variable name.");
            Expr initializer = null;
            if (match(EQUAL))
            {
                initializer = expression();
            }
            consume(SEMICOLON, "Expect ; after variable declaration.");
            return Var.Create(name, initializer);
        }
        private Stmt expressionStatement()
        {
            Expr expr = expression();
            consume(SEMICOLON, "Expect ';' after value.");
            return Expression.Create(expr);
        }

        private Expr assignment()
        {
            Expr expr = or();
            if (match(EQUAL))
            {
                Token equals = previous();
                Expr value = assignment();
                if (expr is Variable)
                {
                    Token name = ((Variable)expr)._name;
                    return Assign.Create(name, value);
                }
                error(equals, "Invalid assignment target.");
            }
            return expr;

        }

        private Expr or()
        {
            Expr expr = and();
            while (match(OR))
            {
                Token oper = previous();
                Expr right = and();
                expr = Logical.Create(expr, oper, right);
            }
            return expr;

        }

        private Expr and()
        {
            Expr expr = equality();
            while (match(AND))
            {
                Token oper = previous();
                Expr right = equality();
                expr = Logical.Create(expr, oper, right);
            }
            return expr;
        }

        private Expr equality()
        {
            Expr expr = comparison();

            while (match(BANG_EQUAL, EQUAL_EQUAL))
            {
                Token oper = previous();
                Expr right = comparison();
                expr = Binary.Create(expr, oper, right);
            }
            return expr;
        }

        private Expr comparison()
        {
            Expr expr = term();
            while (match(GREATER_THAN, GREATER_EQUAL, LESS_THAN, LESS_EQUAL))
            {
                Token oper = previous();
                Expr right = term();
                expr = Binary.Create(expr, oper, right);
            }
            return expr;
        }

        private Expr term()
        {
            Expr expr = factor();
            while (match(MINUS, PLUS))
            {
                Token oper = previous();
                Expr right = factor();
                expr = Binary.Create(expr, oper, right);
            }
            return expr;
        }

        private Expr factor()
        {
            Expr expr = unary();
            while (match(SLASH, STAR))
            {
                Token oper = previous();
                Expr right = unary();
                expr = Binary.Create(expr, oper, right);
            }
            return expr;
        }

        private Expr unary()
        {
            if (match(BANG, MINUS))
            {
                Token oper = previous();
                Expr right = unary();
                return Unary.Create(oper, right);
            }
            return call();
        }

        private Expr call()
        {
            Expr expr = primary();
            while (true)
            {
                if (match(LEFT_PAREN))
                {
                    expr = finishCall(expr);
                }
                else
                {
                    break;
                }
            }
            return expr;
        }

        private Expr finishCall(Expr callee)
        {
            List<Expr> arguments = new List<Expr>();
            if (!check(RIGHT_PAREN))
            {
                do
                {
                    if (arguments.Count >= 255)
                    {
                        error(peek(), "Can't have more than 255 arguments.");
                    }
                    arguments.Add(expression());
                } while (match(COMMA));

            }
            Token paren = consume(RIGHT_PAREN, "Expect ')' after arguments.");
            return Call.Create(callee, paren, arguments);

        }

        private Expr primary()
        {
            if (match(FALSE)) return Literal.Create(false);
            if (match(TRUE)) return Literal.Create(true);
            if (match(NIL)) return Literal.Create(null);
            if (match(NUMBER, STRING))
            {
                return Literal.Create(previous().literal);
            }
            if (match(IDENTIFIER))
            {
                return Variable.Create(previous());
            }
            if (match(LEFT_PAREN))
            {
                Expr expr = expression();
                consume(RIGHT_PAREN, "Expect ')' after expression.");
                return Grouping.Create(expr);
            }
            throw error(peek(), "Expect expression.");
        }

        private Token consume(TokenType type, String message)
        {
            if (check(type)) return advance();
            throw error(peek(), message);
        }

        private ParseError error(Token token, string message)
        {
            Lox.error(token, message);
            return new ParseError();
        }

        private bool match(params TokenType[] tokens)
        {
            foreach (TokenType token in tokens)
            {
                if (check(token))
                {
                    advance();
                    return true;
                }
            }
            return false;
        }

        private bool check(TokenType token)
        {
            if (isAtEnd()) return false;
            return peek().type == token;
        }

        private bool isAtEnd()
        {
            return peek().type == EOF;
        }

        private Token peek()
        {
            return tokens.ElementAt(current);
        }

        private Token advance()
        {
            if (!isAtEnd()) current++;
            return previous();
        }

        private Token previous()
        {
            return tokens.ElementAt(current - 1);
        }

        private void synchronize()
        {
            advance();
            while (isAtEnd())
            {
                if (previous().type == SEMICOLON) return;
                switch (peek().type)
                {
                    case CLASS:
                    case FUN:
                    case VAR:
                    case FOR:
                    case IF:
                    case WHILE:
                    case PRINT:
                    case RETURN:
                        return;
                }
            }
            advance();
        }
    }
}
