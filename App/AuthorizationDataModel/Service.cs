using ApplicationDb.Entities;

 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Обьект модели пользователя сеансов
/// </summary>
[EntityLabel("Микрослужба")]
[EntityIcon("build")]
public class Service : ActiveObject
{    
    public int ProcessID { get; set; }
    public BusinessFunction Process { get; set; }
    public byte[] PublicKey { get; set; } = new byte[0];

    public Service()
    {         
        Name = "[user]";
    } 
         
}