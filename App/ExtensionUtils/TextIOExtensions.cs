using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TextIOExtensions
{

    public static string GetFileExt(this string path){
        int i = path.LastIndexOf(".");
        return i==-1? "": path.Substring(i+1).ToLower();
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static bool Exists(this string path)
    {
        return System.IO.Directory.Exists(path) || System.IO.File.Exists(path);
    }

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static bool IsImage(this string path)
    {

        var ImageFileExts = new List<string>(){ "jpg","png","ico" };
        return (System.IO.Directory.Exists(path) || System.IO.File.Exists(path)) && ImageFileExts.Contains(path.GetFileExt());
    }

/// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static List<string> GetFiles(this string path)
    {
        return new List<string>(System.IO.Directory.GetFiles(path));
    }

    /// <summary>
    /// Вывод в консоль
    /// </summary>    
    public static string WriteToConsole(this string path)
    {
        Writing.ToConsole(path);
        return path;
    }

    /// <summary>
    /// Вывод в консоль
    /// </summary>    
    public static string WriteOrangle(this string path)
    {
        Console.ForegroundColor= ConsoleColor.Yellow;
        Writing.ToConsole(path);
        Console.ResetColor();
        return path;
    }


    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static string WriteToFile(this string text, string path)
    {
        System.IO.File.WriteAllText(path,text);
        return text;
    }
    public static string AppendToFile(this string text, string path)
    {
        System.IO.File.AppendAllText(path, text);
        return text;
    }

    

    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static string ReadText(this string path)
    {
        return System.IO.File.ReadAllText(path);
    }

    public static bool FileExists(this string path)
        => System.IO.File.Exists(path);


    public static long WriteBytes(this string path, List<byte> data, Action<long, long> onprogress)
    {    
        long len = data.Count();
        long pos = 0;

        using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            foreach (var next in data)
            {
                stream.WriteByte(next);
                onprogress((long)Math.Floor((decimal)++pos)/(len/100),100);
            }
            stream.Flush();
        }
        return pos;      
    }


    /// <summary>
    /// Получение списка файлов
    /// </summary>    
    public static List<byte> Read(this string path ){
        return path.ReadBytes((r,l)=>{
          
        });
    }
    public static List<byte> ReadBytes(this string path, Action<long,long> onprogress)
    {
        var bytes = new List<byte>();
        if (path.FileExists())
        {
            int bufferSize = 1024 * 1024;
            var burred = new byte[bufferSize];
            
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                long l = stream.Length;
                long v = 0;
                while (v < l)
                {
                    int readed = stream.Read(burred);
                    l-= readed;
                    v += readed;
                    for(int i=0; i<readed; i++)
                    {
                        bytes.Add(burred[i]);

                        
                    }
                }
            }
        }
        return bytes;
    }
}