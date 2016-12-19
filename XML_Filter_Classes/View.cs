using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XMLFilter_Classes
{
    public class View
    {
        public string Pathname { get; private set; }
        
        private void ErrorMessage(string Message)
        {
            Console.WriteLine(Message);
            Console.ReadKey();
        }

        public string ReadXmlPath()
        {
            GetPathName();
            do
            {
                if (File.Exists(Pathname))
                {
                    if (ConfirmPath())
                    {
                        break;
                    }
                }
                else if (Directory.Exists(Pathname))
                {
                    SetPathname();
                    if (Pathname != "false")
                    {

                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid path, press any key");
                    }
                }
            } while (true);
            return Pathname;
        }

        private string GetPathName()
        {
            Console.Clear();
            Console.WriteLine("Type the path to the XML file, or folder");
            Pathname = Console.ReadLine();
            return Pathname;
        }

        private void SetPathname()
        {
            Pathname = ShowFilesWithTheSameExtension();
        }

        public bool ConfirmPath()
        {
            Console.WriteLine("The path is correct!\nAre you sure you want to use this file? (If not, type: NO, no)");
            Console.WriteLine(Pathname);
            if (!GetOption().ToUpper().Equals("NO"))
                return true;
            return false;
        }

        private string GetOption()
        {
            string option = Console.ReadLine();
            return option;
        }

        private string ShowFilesWithTheSameExtension()
        {
            if (!Pathname.EndsWith("\\"))
            {
                Pathname += "\\";
            }
            string[] files = Directory.GetFiles(Pathname, "*.coveragexml");
            if (files.Length != 0)
            {
                Console.WriteLine("\nFiles with the extension .coveragexml");
                int i = 0;
                foreach (string s in files)
                {
                    i++;
                    Console.WriteLine(i + ". " + s);
                }
                Console.WriteLine("\nPlease write the number of the file you want to load.\nTo Cancel, write a number different from those above");
                int number;
                Int32.TryParse(GetOption(), out number);
                if (number <= i && number > 0)
                    //Console.WriteLine(files[number - 1]);
                    return files[number - 1];
                
                else return "false";
            }
            else
            {
                ErrorMessage("No file was found with the extension .coveragexml in this directory,\nPlease, press any key");
                return "false";
            }
        }

        public string GetFileMaskToIgnore()
        {
            Console.Clear();
            string string_compared = "test.dll";
            Console.WriteLine("Which ending do you want to neglect? (by default: test.dll)");
            string input_Value = "";
            input_Value = Console.ReadLine().ToString();
            if (!(input_Value.Equals(null) || input_Value.Length == 0))
            {
                string_compared = input_Value;
            }
            return string_compared;
        }
    }
}
