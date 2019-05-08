using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace AutoEod3.Models.Helper
{
    static class FileDir
    {
        /// <summary>
        /// Базовый каталог (с программой)
        /// </summary>
        public static readonly string basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);


        /// <summary>
        /// Проверка наличия каталога и его возможное создание
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="createIsNot">Создать каталог, если его нет</param>
        /// <returns>bool</returns>
        public static bool ExistsDir(string path, bool createIsNot = true)
        {
            string ERROR_TEXT_EXISTS_DIR = "Ошибка создания каталога " + path;

            if (Directory.Exists(path))
                return true;

            if (createIsNot)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }               
                catch {}
            }

            return false;

        }

        /// <summary>
        /// Поиск файлов и добавление в дерево файлов
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        public static Node CreateDirectoryNode(DirectoryInfo directoryInfo, Node currentNode = null)
        {
            
            Node node = new Node(directoryInfo.Name, true, directoryInfo.FullName);
           
            if (currentNode != null)
            {
                node.Parent.Add(currentNode);
            }

            // поиск подкаталогов
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {         
                node.Children.Add(CreateDirectoryNode(dir, node));
            }

            // поиск файлов
            foreach (FileInfo file in directoryInfo.GetFiles("*.sql"))
            {
                node.Children.Add(new Node(file.Name, false, file.FullName, node));
            }

            return node;
        }

        /// <summary>
        /// Detects the byte order mark of a file and returns
        /// an appropriate encoding for the file.
        /// </summary>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public static Encoding GetFileEncoding(string scriptName)
        {            
            FileStream file = new FileStream(scriptName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // *** Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;
            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;

            return enc;             
        }

        public static string DirNameDate(string delimiter = "_")
        {
            return DateTime.Now.ToString("yyyy" + delimiter + "MM" + delimiter + "dd");
        }

    }
}
