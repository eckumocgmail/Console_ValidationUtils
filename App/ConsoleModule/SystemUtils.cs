using System;
using System.Diagnostics;
using System.IO;

using static System.Console;
public class SystemUtils
{

    /// <summary>
    /// Выполнение инструкции через командную строку
    /// </summary>
    public static string CmdExec(string command)
    {
        WriteLine(command);
        string result = "";
        command = command.ReplaceAll("\n", "").ReplaceAll("\r", "").ReplaceAll(@"\\", @"\").ReplaceAll(@"//", @"/");

        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + command);

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        string response = process.StandardOutput.ReadToEnd();
        result = response.ReplaceAll("\r", "\n");
        result = result.ReplaceAll("\n\n", "\n");
        while (result.EndsWith("\n"))
        {
            result = result.Substring(0, result.Length - 1);
        }
        process.WaitForExit();
        return result;
    }




    public static void OnError(object sender, ErrorEventArgs e) =>
        PrintException(e.GetException());

    public static void Copy(string fullPath, string newpath)
    {
        string path = "D:";
        foreach (var name in newpath.Substring(2).Split("\\"))
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                path += "\\" + name;
                if (System.IO.Directory.Exists(path) == false)
                    if (name.IndexOf(".") == -1)

                        System.IO.Directory.CreateDirectory(path);
            }
        }

        var data = fullPath.ReadBytes((readed, length) => {
            //Console.WriteLine($"readed {readed} bytes of {length}");
        });
        newpath.WriteBytes(data, (readed, length) => {
            //Console.WriteLine($"writed {readed} bytes of {length}");
        });
        //System.IO.File.Copy(fullPath, newpath,true);
    }
    public static void PrintException(Exception? ex)
    {
        if (ex != null)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Stacktrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }

}