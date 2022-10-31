using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HiroNetwork.Encryption;
using System.Xml.Serialization;

namespace HiroSystemIO.Scripts
{
    static class BinaryFile
    {
        // Class Directory

        // CreateDirectory(path): создает каталог по указанному пути path

        // Delete(path): удаляет каталог по указанному пути path

        // Exists(path): определяет, существует ли каталог по указанному пути path.
        // Если существует, возвращается true, если не существует, то false

        // GetCurrentDirectory(): получает путь к текущей папке

        // GetDirectories(path): получает список подкаталогов в каталоге path

        // GetFiles(path): получает список файлов в каталоге path

        // GetFileSystemEntries(path): получает список подкаталогов и файлов в каталоге path

        // Move(sourceDirName, destDirName): перемещает каталог

        // GetParent(path): получение родительского каталога

        // GetLastWriteTime(path): возвращает время последнего изменения каталога

        // GetLastAccessTime(path): возвращает время последнего обращения к каталогу

        // GetCreationTime(path): возвращает время создания каталога

        //================================================================

        // Class DirectoryInfo

        // Create(): создает каталог

        // CreateSubdirectory(path): создает подкаталог по указанному пути path

        // GetDirectories(): получает список подкаталогов папки в виде массива DirectoryInfo

        // GetFiles(): получает список файлов в папке в виде массива FileInfo

        // MoveTo(destDirName): перемещает каталог

        // MoveTo(destDirName): перемещает каталог

        //================================================================

        // Основные свойства класса DirectoryInfo:

        // CreationTime: представляет время создания каталога

        // LastAccessTime: представляет время последнего доступа к каталогу

        // LastWriteTime: представляет время последнего изменения каталога

        // Exists: определяет, существует ли каталог

        // Parent: получение родительского каталога

        // Root: получение корневого каталога

        // Name: имя каталога

        // FullName: полный путь к каталогу

        //================================================================

        // Основные свойства класса FileInfo:

        // Name: имя файла

        // FullName: полное имя

        // Attributes: Атрибут файла(тип)

        // Length: Размер файла

        // CreationTime: Время создания данного файла

        // LastAccessTime: Время последнего доступа к файлу

        // Extension: Расширение файла

        // Directory: Директория файла

        //================================================================


        // Class BinaryWriter 

        // Close(): - закрывает поток

        // Flush(): - очищает буффер, дописывая из него оставшие данные в файл

        // Seek(): - останавливает позицию в потоке
        // 
        // Write(): - записывает данные в поток

        //================================================================

        // Class BinaryReader

        // Close(): - закрывает поток

        // Read(String, Byte и т.д.)ц // Читает данные из потока

        //================================================================

        // Class Path

        // GetFileName(path): имя файла с его расширением(text.txt)

        // GetExtension(path): расширение файла(.txt)

        // GetPathRoot(path): на каком диске находится файл(D:\)

        // Combine(path1, path2): соединяет два пути(1 - C:\, 2 - \test. С:\test)

        //static void Main(string[] args)
        //{
        //    string salt = Generic.EncryptString("Hello world", "123poi1231231231", 128);
        //    Console.WriteLine(salt);
        //}

        private static DirectoryInfo[] DirectoriesInfo(string path_directory)
        {
            DirectoryInfo directory = new DirectoryInfo(path_directory);
            if (directory.Exists)
            {
                DirectoryInfo[] directories = directory.GetDirectories();
                return directories;

            }
            else { return null; }

        }

        private static FileInfo[] FilesInfo(string path_directory)
        {
            DirectoryInfo directory = new DirectoryInfo(path_directory);
            if (directory.Exists)
            {
                FileInfo[] files = directory.GetFiles();
                return files;
            }
            else { return null; }
        }

        public static byte[] SerializeToByteArray<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return null;
            }
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null)
            {
                return null;
            }
            using (var memStream = new MemoryStream(byteArray))
            {
                var serializer = new XmlSerializer(typeof(T));
                var obj = (T)serializer.Deserialize(memStream);
                return obj;
            }
        }
    }
}
