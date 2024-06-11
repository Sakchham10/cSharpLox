using System.Collections.Generic;
using System.IO;
using System;
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
            defineAst(outputDir, "Expr", new List<string>{
                    "Binary : Expr left, Token Operator, Expr right",
                    "Grouping : Expr expression",
                    "Literal : Object value",
                    "Unary : Token Operator, Expr right",
                    });

        }

        private static void defineAst(string outputDir, string baseName, List<string> types)
        {
            string path = outputDir + "/" + baseName + ".cs";
            StreamWriter writer = new StreamWriter(path, false);

            writer.WriteLine("namespace interpreter.lox ;");
            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("public abstract class " + baseName + " {");
            writer.WriteLine("}");
            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                defineType(writer, baseName, className, fields);
            }
            writer.WriteLine("}");
            writer.Close();
        }

        private static void defineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine("  public class " + className + " : " + baseName + " {");

            // Constructor.
            writer.WriteLine("  private " + className + "(" + fieldList + ") {");

            // Store parameters in fields.
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields)
            {
                string name = field.Split(" ")[1];
                writer.WriteLine("_" + name + " = " + name + ";");
            }

            writer.WriteLine("    }");

            // Fields.
            writer.WriteLine();

            writer.WriteLine("   public static " + className + " Create" + "(" + fieldList + ") {");
            string fieldNames = new string("");
            foreach (string field in fields)
            {
                string name = field.Split(" ")[1];
                fieldNames = String.Concat(fieldNames, name, ",");
            }
            fieldNames = fieldNames.Remove(fieldNames.Length - 1);
            writer.WriteLine("   return new " + className + "(" + fieldNames + ") ;}");
            foreach (string field in fields)
            {
                string type = field.Split(" ")[0];
                string name = field.Split(" ")[1];
                writer.WriteLine("    static " + type + " " + "_" + name + ";");
            }

            writer.WriteLine("  }");

        }
    }
}
