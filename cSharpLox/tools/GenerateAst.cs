namespace interpreter.tools
{
    public class GenerateAst
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage : generate_ast <output directory>");
                Environment.Exit(65);
            }
            string outputDir = args[0];
        }
    }
}
