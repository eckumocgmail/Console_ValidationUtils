using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;


/// <summary>
/// Модуль тестирования наследует данный класс.
/// </summary>
public abstract class TestingElement: Microsoft.AspNetCore.Mvc.Controller
{
    protected ILogger logger;

    private Dictionary<string, TestingElement> Children = new Dictionary<string, TestingElement>();

    /// <summary>
    /// Отчёт о проведении тестирования
    /// </summary>
    protected TestingReport Report;

    /// <summary>
    /// Список сообщений, полученныйв результате тестирования
    /// 
    /// </summary>
    protected List<string> Messages = new List<string>();


    protected TestingElement()
    {
        this.logger = LoggerFactory.Create((builder) => { builder.AddConsole(); }).CreateLogger(GetType().GetTypeName());
        Report = new TestingReport(this.GetType().GetTypeName());
    }

    /// <summary>
    /// Реализация метода тестирования
    /// </summary>
    public abstract void OnTest();

 

    /// <summary>
    /// Добавление метода тестирования
    /// </summary>
    /// <param name="unit"> метод тестирования </param>
    protected void Push(TestingElement unit)
    {
        this.Children[unit.GetType().Name] = unit;
    }


    /// <summary>
    /// Выполнения теста и составления отчета о тестировании
    /// </summary>
    /// <returns> отчет о тестировании </returns>
    public virtual TestingReport DoTest()
    {
        logger.LogInformation( $"Выполняем тест {this.GetType().Name}" );
   
        try
        {
            Report.Started = DateTime.Now;
            Report.Failed = false;
            this.OnTest();                
        }
        catch (Exception ex)
        {
            logger.LogError("Исключение при выполнении теста: "+ ex.Message);
            Report.Failed = true;
            Report.Messages.Add( ex.ToString() );
            throw;
        }
        finally
        {
            
           
            if(this.GetType().IsExtendsFrom(typeof(TestingUnit)) == false)
            {
                if (Report.Messages.Count == 0)
                    throw new Exception(GetType().GetTypeName()+
                        " Тест выполнен некорректно так как отчёт не содержит ни одного утверждения.");
                Report.ToJsonOnScreen().WriteToConsole();
                Report.ToDocument().WriteToConsole();
                ConfirmDialog("Тест правильно выполнен тест?");
            }


            Report.Ended = DateTime.Now;
            foreach (var p in Children)
            {
                Report.Subreports[p.Key] = p.Value.DoTest();
                if(Report.Subreports[p.Key].Failed)
                {
                    Report.Failed = true;
                }
            }                    
        }
        return Report;
    }

    private bool ConfirmDialog(string messages)
    {

        Console.WriteLine(messages);
        Console.WriteLine("Введите y/n");
        return (Console.ReadLine() == "y"); 
    }
}
