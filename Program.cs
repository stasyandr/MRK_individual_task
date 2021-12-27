using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using System.Linq;
using System.Text;
using SimpleLang.Visitors;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimpleCompiler
{
    class SimpleCompilerMain
    {
        static void Main(string[] args)
        {
            /*string test = "adgvc[10]";
            int c = test.IndexOf('[');
            Console.WriteLine(test.Substring(0, c));
            Console.WriteLine(test.Substring(c+1, test.Length-c-2));*/
            string FileName = @"..\..\a.txt";
            string OutputFileName = @"..\..\a.json";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");
                    JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                    jsonSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    jsonSettings.TypeNameHandling = TypeNameHandling.All;
                    string output = JsonConvert.SerializeObject(parser.root, jsonSettings);
                    File.WriteAllText(OutputFileName, output);
                }
                var visitor = new CorrectLengthVisitor();
                parser.root.Visit(visitor);
                Console.WriteLine("Ошибок с размерностями массивов нет");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }
            
            Console.ReadLine();            
        }
    }
}
