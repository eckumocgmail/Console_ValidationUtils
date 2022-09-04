 

using System;
using System.Collections.Generic;

namespace ApplicationCommon.CommonTypes
{
    /// <summary>
    /// Модель директории в файловой системе.
    /// При инициаллизации считывает все внутрении файлы.
    /// </summary>
    public class TypeCatalog: TypeNode<Dictionary<string, TypeFile>>
    {
        /// <summary>
        /// Конструктор корня иерархии
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rootName"></param>
        public TypeCatalog( string path, string rootName ) : base( rootName,new Dictionary<string, TypeFile>(), null)
        {
            ReadFiles(path);
            foreach (string dir in System.IO.Directory.GetDirectories(path))
            {
                string name = dir.Substring(path.Length + 1);
                TypeCatalog subcatalog = new TypeCatalog(dir, name, this);
            }
        }

        /// <summary>
        /// Конструктор дочернего узла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        private TypeCatalog(string path, string name, TypeCatalog parent) : base(name, new Dictionary<string, TypeFile>(), parent)
        {
            ReadFiles(path);
            foreach (string dir in System.IO.Directory.GetDirectories(path))
            {
                string childName = dir.Substring(path.Length + 1);
                TypeCatalog subcatalog = new TypeCatalog(dir, childName, this);
            }
        }


        /// <summary>
        /// Считывание файлов
        /// </summary>
        /// <param name="path">путь к каталогу</param>
        private void ReadFiles(string path)
        {
            foreach (string filepath in System.IO.Directory.GetFiles(path))
            {
                string name = filepath.Substring(path.Length + 1);
                int i = name.LastIndexOf(".");
                string ext = "text/plain";
                if ( i!=-1)
                {
                    ext = name.Substring(i + 1).ToLower();
                }
                Item[name] = new TypeFile()
                {
                    Name = name,
                    Mime = $"text\\{ext}",
                    Data = System.IO.File.ReadAllBytes(filepath),
                    Changed = System.IO.File.GetLastWriteTime(filepath)
                };
            };
        }
    }
}
