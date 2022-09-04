using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Модель параметра вызова метода или процедуры или функции.
/// ПО этой модели приложение-клиент создаёт поле для ввода информации
/// на форме выполнения операции.
/// </summary>
public class MyParameterDeclarationModel
{
    private ParameterInfo par;

    public MyParameterDeclarationModel() { }
    public MyParameterDeclarationModel(ParameterInfo par)
    {
        this.par = par;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsOptional { get; set; }
    public int Position { get; set; }
    public object DefValue { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
}
