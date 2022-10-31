using System;
using System.Collections.Generic;
using System.IO;
using FastSearchLibrary;

namespace Assets.Scripts.BinartFile
{
    internal class WorkingFiles
    {
        private static WorkingFiles search_file_instance = null;
        public static WorkingFiles search_file
        {
            get
            {
                if (search_file_instance == null)
                {
                    search_file_instance = new WorkingFiles();
                }
                return search_file_instance;
            }
        }

        public string DeleteFile(string path_file)
        {
            try
            {
                if (File.Exists(path_file))
                {
                    File.Delete(path_file);
                    return "Файл по пути: " + path_file + ", был успешно удалён";
                }
            }
            catch (Exception) 
            {
                return "Произошёл сбой удаления";
            }
            return null;
        }


        public (List<FileInfo>, string) SearchFile(string name_directory, string pattern)
        {
            var files_list = FileSearcher.GetFilesAsync(name_directory, pattern).Result;
            if (files_list == null)
            {
                return (null, "В этой директории с расширением '" + pattern + "' ничего нет");
            }
            return (files_list, null);
        }


        public (DirectoryInfo[], string) DirectoriesInfo(string path_directory)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(path_directory);
                if (directory.Exists)
                {
                    DirectoryInfo[] directories = directory.GetDirectories();
                    return (directories, null);

                }
                else { return (null, "Такой директории не существует"); }
            }
            catch (Exception)
            {
                return (null, "Произошёл сбой");
            }
        }

        public (FileInfo[], string) FilesInfo(string path_directory)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(path_directory);
                if (directory.Exists)
                {
                    FileInfo[] files = directory.GetFiles();
                    return (files, null);
                }
                else { return (null, "Такой директории не существует"); }
            }
            catch (Exception)
            {
                return (null, "Произошёл сбой");
            }
        }
    }
}
