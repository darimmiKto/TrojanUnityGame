using System;
using HiroNetwork.IO;
using System.IO;
using System.Text;

namespace HiroSystemIO.Scripts
{
    internal class TextFile
    {
        static string path = @"C:\Users\Даниил\Desktop\io_test";
        //static void Main(string[] args)
        //{
        //    TextWriter(path + "\\text_writer.txt", "Тест номер 1");
        //    TextWriterLines(path + "\\text_writer_lines.txt", new string[] { "Тест", "номер", "2" });
        //    TextWriterBytes(path + "\\text_writer_bytes.txt", "Тест номер 3");

        //    string text_one = TextRead(path + "\\text_writer.txt");
        //    Console.WriteLine(text_one);


        //    string[] text_two = TextReadLines(path + "\\text_writer_lines.txt");
        //    for (int i = 0; i < text_two.Length; i++)
        //    {
        //        Console.Write(text_two[i] + " ");
        //    }
        //    Console.WriteLine();


        //    string text_three = TextReadBytes(path + "\\text_writer_bytes.txt");
        //    Console.WriteLine(text_three);
        //}


        private static void TextWriter(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        private static void TextWriterLines(string path, string[] text_lines)
        {
            File.WriteAllLines(path, text_lines);
        }

        private static void TextWriterBytes(string path, string text_byte)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text_byte);
            File.WriteAllBytes(path, buffer);
        }

        private static string TextRead(string path)
        {
            return File.ReadAllText(path);
        }

        private static string[] TextReadLines(string path)
        {
            return File.ReadAllLines(path);
        }

        private static string TextReadBytes(string path)
        {
            string buffer = Encoding.UTF8.GetString(File.ReadAllBytes(path));
            return buffer;
        }




    }
}
