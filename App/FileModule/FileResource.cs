using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using RootLaunch;

public class FileResource:  ConsoleProgram<System.IO.FileSystemWatcher>
{
    private DateTime AccessTime;
    private DateTime WriteTime;

   
    public string Path { get; }
    public bool IsDirectory { get; }
    public bool IsFile { get; }

    public string NameShort {
        get => GetNameShort(); 
    }
    public string GetNameShort()
    {
        return this.Path.Substring(GetParentPath(this.Path).Length + 1);
    }
    public static string PATH_SEPARATOR = null;

    /**
     * возвращает файловый разделитель 
     */
    public string GetPathSeparator()
    {
        if (PATH_SEPARATOR == null)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            int i0 = path.LastIndexOf("/");
            int i1 = path.LastIndexOf("\\");
            if (i0 > i1)
            {
                PATH_SEPARATOR = "/";
            }
            else if (i1 > i0)
            {
                PATH_SEPARATOR = "\\";
            }
            else
            {
                throw new Exception("ResourceManager: can not compute file path separator");
            }
        }
        return PATH_SEPARATOR;
    }
    public string GetParentPath(string path)
    {
        string sep = GetPathSeparator();
        int ind = path.LastIndexOf(sep);
        if (ind == -1)
        {
            throw new Exception("Не возможно получить путь к родительской директории для " + path);
        }
        else
        {
            return path.Substring(0, ind);
        }
    }

    public string GetName()
    {
        int i=NameShort.IndexOf(".");
        return i==-1? NameShort: NameShort.Substring(0, i);
    }
    public string GetExt()
    {        
        var shName = NameShort;
        var i = shName.IndexOf(".");

        return shName.Substring(i + 1);        
    }
    
    private byte[] Data { get; set; }
    public bool IsInner { get; }

    public byte[] ReadBytes()
    {
        if (Data == null)
        {
            this.Data = System.IO.File.ReadAllBytes(this.Path);
        }
        return this.Data;
    }

    public FileResource()
    {
    }

    public FileResource( string pathAbs )
    {                        
        this.Path = pathAbs;
        //this.ID = this.NameShort;
        this.IsDirectory = System.IO.Directory.Exists(this.Path);
        this.IsFile = System.IO.File.Exists(this.Path);
        if (this.IsDirectory==false && this.IsFile == false)
        {
            this.IsInner = this.Path.StartsWith(System.IO.Directory.GetCurrentDirectory());
            System.IO.File.Create(this.Path);
            if (this.IsDirectory == false && this.IsFile == false )
            {
                throw new Exception($"[404][" + pathAbs + $"] => Путь=[{pathAbs}] не существует такого файла, кстати говоря я проверил директории что такой нету.. .");
            }
        }
        
    }

    internal object Modify(string v)
    {
        throw new NotImplementedException();
    }

    public virtual void OnInit()
    {         
        this.AccessTime = System.IO.File.GetLastAccessTimeUtc(this.Path);
        this.WriteTime = System.IO.File.GetLastWriteTime(this.Path);                    
    }


    public byte[] GetBInaryData() => this.Data;



    public virtual bool Copy(string directory)
    {
        var ctrl = FilesDirectory.Get(directory);
        ctrl.CreateFile(NameShort).WriteText(ReadText());
        return true;
    }

    public string ReadText()
    {
        this.Info($"ReadText({this.NameShort})");
        return System.IO.File.ReadAllText(this.Path);
    }
    public void WriteText(string context)
    {
        //this.Info($"WriteText({this.Path})");
        System.IO.File.WriteAllText(this.Path, context);
    }
    public async Task<string> ReadTextAsync()
    {
        return await System.IO.File.ReadAllTextAsync(this.Path);
    }
    public async Task WriteTextAsync(string context)
    {
        await System.IO.File.WriteAllTextAsync(this.Path, context);
    }

    public virtual FilesDirectory[] GetDirectories()
    {
        if (IsFile)
        {
            throw new Exception("Не возможно получить список директорий для файла " + this.Path);
        }
        return System.IO.Directory.GetDirectories(this.Path).Select(path => FilesDirectory.Get(path)).ToArray();
    }

    public virtual FileResource[] GetFiles()
    {
        if (IsFile)
        {
            throw new Exception("Не возможно получить список директорий для файла " + this.Path);
        }
        return System.IO.Directory.GetFiles(this.Path).Select(path => new FileResource(path)).ToArray();
    }

    public virtual FileResource[] GetAllFiles()
    {
        var resources = new List<FileResource>();
        if (IsFile)
        {
            throw new Exception("Не возможно получить список директорий для файла " + this.Path);
        }
        resources.AddRange(System.IO.Directory.GetFiles(this.Path).Select(path => new FileResource(path) ).ToArray());
        GetDirectories().ToList().ForEach(dir=> resources.AddRange(dir.GetAllFiles()));
        return resources.ToArray();
    }

    private FileResource GetFiles(string path)
    {
        return new FileResource(GetChildPath(this.Path, path));
    }

    public virtual FileResource GetParent(   )
    {
        return new FileResource(GetParentPath(this.Path));
    }
    public string GetChildPath(string path1, string path2)
    {
        return $"{path1}{GetPathSeparator()}{path2}";
    }


    public override string ToString()
    {
        return Path;
    }
}
