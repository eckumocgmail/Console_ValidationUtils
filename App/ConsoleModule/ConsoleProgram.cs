using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.Console;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class ConsoleProgram<T> : ConsoleProgram 
{
    public override int Run(params string[] args)
        => RunInteractive<T>( args);

}

public abstract class ConsoleProgram: SystemUtils
{
    private static ILogger<ConsoleProgram> Logger = Factory.GetLogger<ConsoleProgram>();

    internal static IConfiguration Run(object target)
    {
        throw new NotImplementedException();
    }

    private static object ProgramData;
    private static MethodInfo ProgramAction;

    public static Dictionary<string, object> ProgramArguments { get; private set; }
    public static object ActionResult { get; private set; }


    public abstract int Run(params string[] args);

    public static int RunInteractive<TypeOfPogram>(params string[] args)
    {
        Type ProgramType = typeof(TypeOfPogram);
        ProgramData = ProgramType.New();
        while (true)
        {
            PrintProgramData();
            SelectNextAction();
            InputActionParameters();
            ShowExecuteActionResults();
        }
    }


    /// <summary>
    /// вывод результатов процедуры
    /// </summary>
    private static void ShowExecuteActionResults()
    {
        WriteLine(ActionResult.ToJsonOnScreen());
        WriteLine("Нажмите клавишу для продолжения ... ");
        ReadKey();
    }


    /// <summary>
    /// ввод параметров процедуры
    /// </summary>
    private static void InputActionParameters()
    {      
        Clear();
        PrintProgramData();
        WriteLine($"\n {ProgramAction.Name}");
        int n = 1;
        var args = new List<object>();
        ProgramArguments = new Dictionary<string, object>();
        foreach(ParameterInfo par in ProgramAction.GetParameters())
        {
            WriteLine($"{n++}) {par.Name}: {par.ParameterType.Name}>");            
            object result = TextDataSetter.ToType(ReadLine(), par.ParameterType);
            ProgramArguments[par.Name] = result;
            args.Add(result);
        }
        ActionResult = ProgramAction.Invoke(ProgramData, args.ToArray());
    }


    public static void SelectNextAction()
    {
        try
        {
            int i = 1;
            var methods = ProgramData.GetOwnMethodNames();
            foreach (var next in methods)
            {
                WriteLine(i + ")" + next); i++;
            }
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            } while (key.KeyChar < '0' || key.KeyChar > methods.Count().ToString().ToCharArray()[0]);
            int index = int.Parse(key.KeyChar.ToString()) - 1;
            ProgramAction = ProgramData.GetType().GetMethods().Where(m => m.Name == methods.ToArray()[index]).FirstOrDefault();
            WriteLine(ProgramAction.Name);
        }catch (Exception ex)
        {
            WriteLine(ex);
        }
    }

    private static void PrintProgramData()
    {
        Type ProgramType = ProgramData.GetType();     
      
        WriteLine("Свойства: ");
        ProgramType.GetProperties().ToList().ForEach(prop => {
            WriteLine($"\t {prop.Name}=\"{ProgramData.GetProperty(prop.Name)}\";");
        });

  
    }


    /// <summary>
    /// 
    /// </summary>    
    public TResult Invoke<TService,TResult>(string action, params object[] args)
    {
        object injected = typeof(TResult).New();
        if (injected == null)
            throw new Exception("Не удалось найти сервис " + typeof(TService).Name);
        TService instance = (TService)injected;
        var methodInfo = typeof(TService).GetMethod(action);
        if (methodInfo == null)
            throw new Exception($"Не удалось найти метод {action} у сервиса " + typeof(TService).Name);
        try
        {
            object result = methodInfo.Invoke(injected, args);
            return (TResult)result;
        }
        catch (Exception ex)
        {
            string text = "";
            if (args != null)
            {
                foreach (var arg in args)
                {
                    text += arg + ",";
                }
                if (args.Length > 0)
                {
                    text = text.Substring(0, text.Length - 1);
                }
            }
            throw new Exception($"Не удалось выполнить метод {action} у сервиса " + typeof(TService).Name +
                " с аргументами: " + text, ex);
        }
    }


    /// <summary>
    /// 
    /// </summary>    
    public static void RunConsoleProgram(params string[] args)
    {
        while (true)
        {
            try
            {
                Console.Clear();
                Logger.LogInformation("Укажите путь к сборке dll или exe файлу чтобы подключить консоль управления.");
                string path = Console.ReadLine();
                var assembly = Assembly.LoadFile(path);
                //TODO
            }
            catch(Exception ex)
            {
                ex.ToString().WriteToConsole();
            }
        }
    }


    
}