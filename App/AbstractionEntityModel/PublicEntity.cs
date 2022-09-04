using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



/// <summary>
/// Разделяемыек между пользователелями данные
/// Пример: 
///   Товар на полках магазина может буть продан только одному покапателю
/// </summary>
public class PublicEntity : BaseEntity
{

    /// <summary>
    /// Занят/свободен
    /// </summary>
    public int Status { get; set; }

}