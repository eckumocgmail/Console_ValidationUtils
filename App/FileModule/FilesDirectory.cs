using Microsoft.Extensions.FileProviders;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootLaunch
{

    public interface IFilesDirectory: IDirectoryContents, IFileInfo
    {
        string GetName();


        FileController<T> Bind<T>(string filename) where T : class;


        bool Copy(string directory);

        FileResource CreateFile(string filename);
        FileResource CreateTextFile(string filename, string text);
        FileResource[] GetDllFiles();
        FileResource[] GetExeFiles();

        FilesDirectory GetOrCreateDirectory(string dirname);

        void SetCurrentDirector();
        void Trace();
    }


    /// <summary>
    /// 
    /// </summary>
    public class FilesDirectory : FileResource, IFilesDirectory
    {
         

        public static FilesDirectory Get(string arbsPath=null)
        {
            if (arbsPath == null)
                arbsPath = System.IO.Directory.GetCurrentDirectory();
            if (Directories.ContainsKey(arbsPath)==false)
            {
                Directories[arbsPath] = new FilesDirectory(arbsPath);
            }
            return Directories[arbsPath];
        }


        private static IDictionary<string, FilesDirectory> Directories { get; } =
                    new ConcurrentDictionary<string, FilesDirectory>();



        private IDictionary<string, FilesDirectory> SubDirs { get; set; } = new ConcurrentDictionary<string, FilesDirectory>();


        /// <summary>
        /// 
        /// </summary>
        public FilesDirectory() : this(System.IO.Directory.GetCurrentDirectory())
        {
        }

        public FilesDirectory(string pathAbs) : base(pathAbs)
        {
            if (Directories.ContainsKey(Path))
            {
                Directories[pathAbs].Error(new Exception(
                    $"Ресурсы директории уже созданы их нужно использовать через " +
                    $"{nameof(FilesDirectory) + "." + nameof(FilesDirectory.Get)}"));                
            }
            else
            {
                Directories[pathAbs] = this;
            }
            
        }

        /// <summary>
        /// Устанавливает собственный путь в качестве рабочей директории
        /// </summary>
        public void SetCurrentDirector()
        {            
            System.IO.Directory.SetCurrentDirectory(this.Path);
        }


        public FileResource[] GetExeFiles()
            => GetFiles().Where(f => f.NameShort.EndsWith(".exe")).ToArray();
        public FileResource[] GetDllFiles()
            => GetFiles().Where(f => f.NameShort.EndsWith(".dll")).ToArray();


        public override void OnInit()
        {
       
            this.GetDirectories().ToList().ForEach(OnInitSubDir);
        }
 

        private void OnInitSubDir(FilesDirectory dir)
        {
            Directories[dir.Path] =
                this.SubDirs[dir.NameShort] = dir;
        }


        public void Trace()
        {
            System.IO.Directory.GetDirectories(this.Path).ToJsonOnScreen().WriteToConsole();
            System.IO.Directory.GetFiles(this.Path).ToJsonOnScreen().WriteToConsole();

        }

        public string GetDirectoryName() => this.Path.Substring(this.Path.LastIndexOf(GetPathSeparator()) + 1);

        public FileController<T> Bind<T>(string filename) where T : class
        {
            string filePath = this.Path + GetPathSeparator() + filename;
            return new FileController<T>(filePath);
        }
        public FileResource CreateFile(string filename)
        {
            string dirpath = this.Path + GetPathSeparator() + filename;
            System.IO.File.WriteAllText(dirpath, "");
            return new FileResource(dirpath);
        }
        public FileResource CreateTextFile(string filename, string text)
        {
            string dirpath = this.Path + GetPathSeparator() + filename;
            System.IO.File.WriteAllText(dirpath, "");
            var ctrl = new FileResource(dirpath);
            ctrl.WriteText(text);
            return ctrl;
        }
        public FilesDirectory GetOrCreateDirectory(string dirname)
        {
            string dirpath = this.Path + GetPathSeparator() + dirname;
            if (System.IO.Directory.Exists(dirpath) == false)
            {
                System.IO.Directory.CreateDirectory(dirpath);
            }
            return FilesDirectory.Get(dirpath);
        }
        public override bool Copy(string directory)
        {
            var ctrl =   FilesDirectory.Get(directory);

            foreach (var file in GetFiles())
            {
                ctrl.CreateTextFile(file.NameShort, file.ReadText());
            }
            foreach (var dir in GetDirectories())
            {
                var subctrl = ctrl.GetOrCreateDirectory(dir.NameShort);
                subctrl.Copy(directory + GetPathSeparator() + dir.NameShort);
            }
            return true;
        }

        public override string ToString()
        {
            return NameShort;
        }

        public bool Exists => System.IO.Directory.Exists(this.Path);
        public Stream CreateReadStream()
        {
            return new FileStream(this.PhysicalPath, FileMode.CreateNew);
        }

    


        public long Length => new System.IO.FileInfo(this.PhysicalPath).Length;

        public string PhysicalPath => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public DateTimeOffset LastModified => throw new NotImplementedException();

       
        public IEnumerator<IFileInfo> GetEnumerator()
        {

            GetFiles();
            return null;
        }
        IFileInfo CreateFileInfo(string path) => new FilesDirectory(path);
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
