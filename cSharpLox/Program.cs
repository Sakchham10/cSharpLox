using System.Text;
namespace interpreter.cSharpLox
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
            tokens.ForEach((token) => Console.WriteLine(token.toString()));
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
    }
}
