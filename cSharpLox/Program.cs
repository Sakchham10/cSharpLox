﻿using System.Text;
using interpreter.lox.astPrinter;
using interpreter.lox.expr;
using interpreter.lox.parser;
using interpreter.lox.scanner;
using interpreter.lox.token;
using static interpreter.lox.tokentype.TokenType;

namespace interpreter.lox
{
    public class Lox
    {
        static Boolean hadError = false;
        public static void Main(String[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: jlox [script]");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                runFile(args[0]);
            }
            else
            {
                runPrompt();
            }
        }

        public static void runFile(String path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            String content = Encoding.Default.GetString(bytes);
            run(content);
            if (hadError == true) Environment.Exit(65);
        }

        private static void runPrompt()
        {
            for (; ; )
            {
                Console.Write(">");
                var line = Console.ReadLine();
                if (line == null) break;
                run(line);
                hadError = false;
            }
        }

        private static void run(String source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();
            int lastLine = tokens.Last().line;
            Parser parser = new Parser(tokens);
            Expr expression = parser.parse();
            Console.WriteLine(new AstPrinter().print(expression));
        }

        public static void error(int line, String message)
        {
            report(line, "", message);
        }

        private static void report(int line, string where, string message)
        {
            Console.Error.WriteLine("[line" + line + "] Error" + where + " : " + message);
            hadError = true;
        }

        public static void error(Token token, string message)
        {
            if (token.type == EOF)
            {
                report(token.line, " at end", message);
            }
            else
            {
                report(token.line, " at '" + token.lexeme + "'", message);
            }
        }
    }
}
