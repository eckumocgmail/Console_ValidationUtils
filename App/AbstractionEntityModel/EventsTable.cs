using ApplicationDb.Entities;

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Фиксирует события бизнес процессов /
///     другими словами логируем бизнес - операции
/// </summary>
public class EventsTable : StoredItem
{
    [NotNullNotEmpty("Необходимо указать дату")]
    [InputDateTime()]
    public DateTime Created { get; set; }

    [Label("Календарь")]
    public int CalendarID { get; set; }
    public virtual TimePoint Calendar { get; set; }
}