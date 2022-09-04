using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


/// <summary>
/// Отчет о тестировании
/// </summary>
public class TestingReport 
{
    public string Name { get; set; }
    public bool Failed { get; set; }
    public DateTime Started { get; set; }
    public DateTime Ended { get; set; }
    public List<string> Messages { get; set; }

    public Dictionary<string, TestingReport> Subreports { get; set; }

    public TestingReport(string Name) : this() { 
        this.Name = Name;
    }
    public TestingReport()
    {        
        this.Subreports = new Dictionary<string, TestingReport>();
        this.Messages = new List<string>();
        this.Failed = false;        
    }

    /// <summary>
    /// Фактический номер версии, показывает отношение коль-ва тестов к выполненым
    /// </summary>
    /// <returns></returns>
    public int GetVersion()
        => (from r in this.Subreports.Values where r.Failed == false select r).Count();


    /// <summary>
    /// Фактический номер версии, показывает отношение коль-ва тестов к выполненым
    /// </summary>
    /// <returns></returns>
    public Version GetRealisticVersion()
    {
        if(this.Subreports.Count == 0)
        {
            return new Version(1, this.Failed ? 0 : 1);
        }                
        return new Version(
            (from r in this.Subreports.Values where r.Failed==false select r).Count(), 
            this.Subreports.Count);
    }

    /// <summary>
    /// Количественный номер версии, показывает кол-во выполненых проверок
    /// </summary>
    /// <returns></returns>
    public Version GetMaximalisticVersion()
    {
        if (this.Subreports.Count == 0)
        {
            return new Version(1, this.Failed ? 0 : 1);
        }
        return new Version((from r in this.Subreports.Values where r.Failed == false select r).Count(), this.Subreports.Count);
    }



    public class Version
    {
        int _count;
        int _completed;
        public Version(int completed, int count)
        {
            _count = count;
            _completed = completed;
        }
        public override string ToString()
        {
            return $"{_completed}/{_count}";
        }
    }



    /// <summary>
    /// Метод получчения числовой информации о результатх тестирования 
    /// </summary>
    /// <returns> числовая информация о результатах тестирования </returns>
    public string GetStat()
    {            
        if( this.Subreports.Count() == 0)
        {
            return this.Failed ? "0" : "1";
        }
        else
        {
            int inc = 0;
            foreach (var p in this.Subreports)
            {
                if (p.Value.Failed)
                {
                    break;
                }
                else
                {
                    inc++;
                }
            }
            return $"{this.Subreports.Count}-{inc}";
        }            
    }


    /// <summary>
    /// Составление текстового документа, содержащего информацию о результатах тестирования
    /// </summary>
    /// <param name="isTopReport"> true, если отчет составлен на верхнем уровне </param>
    /// <returns> теккстовый документ </returns>
    public string ToDocument(int level=0)
    {
        string document = "";
        foreach (string message in Messages)
        {
            for(int i=0; i<=level; i++)
            {
                document += "    ";
            }
            document += message + "\n";
        }
        int number = 1;
        foreach( var pair in this.Subreports)
        {
            for (int i = 0; i <= level; i++)
            {
                document += "    ";
            }
                
            document += //$"{GetRealisticVersion().ToString()}: "+pair.Key + "\n";
                $"{number}/{this.Subreports.Count()}: " + pair.Key + "\n";
            document += pair.Value.ToDocument(level+1);
            number++;
        }
        return document;
    }


    /// <summary>
    /// Преобразование в текстовый формат
    /// </summary>
    /// <returns> текстовые данные </returns>
    public override string ToString()
    {
        return JObject.FromObject(this).ToString();
    }
}
