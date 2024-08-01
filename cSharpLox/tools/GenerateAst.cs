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
            defineAst(outputDir, "Expr", new List<string>
                    {
                                "Assign : Token name, Expr value",
                                "Binary : Expr left, Token oper, Expr right",
                                "Call : Expr callee, Token paren, List<Expr> arguments",
                                "Get : Expr expression, Token name",
                                "Grouping : Expr expression",
                                "Literal : Object value",
                                "Logical : Expr left, Token oper, Expr right",
                                "Set : Expr expression, Token name, Expr value",
                                "Unary : Token oper, Expr right",
                                "Variable : Token name"
                                });
            defineAst(outputDir, "Stmt", new List<string>
            {
            "Block: List<Stmt> statements",
            "Class: Token name, List<Function> methods",
            "Expression : Expr expression",
            "Function : Token name, List<Token> parameters, List<Stmt> body",
            "If : Expr condition, Stmt thenBranch," + " Stmt elseBranch",
            "Returns : Token keyword, Expr value",
            "Print : Expr expression",
            "Var : Token name, Expr initializer",
            "While : Expr condition, Stmt body"
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
            writer.WriteLine();
            writer.WriteLine("  public abstract T accept<T>(Visitor<T> visitor);");
            defineVisitor(writer, baseName, types);
            writer.WriteLine("}");
            foreach (string type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                defineType(writer, baseName, className, fields);
            }
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
            // Visitor pattern.
            writer.WriteLine();
            writer.WriteLine("    public override T accept<T>(Visitor<T> visitor) {");
            writer.WriteLine("      return visitor.visit" +
                className + baseName + "(this);");
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
                writer.WriteLine("public " + type + " " + "_" + name + ";");
            }

            writer.WriteLine("  }");

        }

        private static void defineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine(" public interface Visitor<T> {");
            foreach (string type in types)
            {
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine(" T visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
            }
            writer.WriteLine("}");
        }
    }
}
