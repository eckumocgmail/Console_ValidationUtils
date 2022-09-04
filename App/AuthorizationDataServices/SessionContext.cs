using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;


public class SessionContext: IDisposable, IDictionary<int, object>
{
    internal ConcurrentDictionary<Type, object> Context = new ConcurrentDictionary<Type, object>();
    //internal ViewContext ViewContext = new ViewContext();
    internal IDictionary<int, object> data = new ConcurrentDictionary<int, object>();
    internal Queue<object> Events { get; set; } = new Queue<object>();
    internal Func<string, object[], int> Invoke;
    internal long Timestamp;
    internal string ip;
    internal bool IsLocked = false;
    
    public SessionContext(long timestamp)
    {
        Writing.ToConsole("Created "+nameof(SessionContext));
        this.Timestamp = timestamp;
    }


    /*public ViewContext GetModels()
    {
        return (ViewContext)Get(typeof(ViewContext));
    }


    public ViewContext GetRoot()
    {
        return ((ViewContext)Get(typeof(ViewContext)));
    }
    */

    public object Get(Type type)
    {
        if(Context.ContainsKey(type) == false)
        {
            Context[type]=CreateWithDefaultConstructor(type);
        }
        return Context[type];
    }


    private object GetById(Type type)
    {
        if (Context.ContainsKey(type))
        {
            return Context[type];
        }
        else
        {
            Context[type] = CreateWithDefaultConstructor(type);                
            return Context[type];
        }
    }


    private object CreateWithDefaultConstructor(Type type)
    {
        ConstructorInfo constructor = (from c in new List<ConstructorInfo>(type.GetConstructors()) where c.GetParameters().Length == 0 select c).FirstOrDefault();
        return constructor.Invoke(new object[0]);
    }

    public void Dispose()
    {
  

    }

    public void Add(int key, object value)
    {
        data.Add(key, value);
    }

    public bool ContainsKey(int key)
    {
        return data.ContainsKey(key);
    }

    public bool Remove(int key)
    {
        return data.Remove(key);
    }

    public bool TryGetValue(int key, [MaybeNullWhen(false)] out object value)
    {
        return data.TryGetValue(key, out value);
    }

    public object this[int key] { get => data[key]; set => data[key] = value; }

    public ICollection<int> Keys => data.Keys;

    public ICollection<object> Values => data.Values;

    public void Add(KeyValuePair<int, object> item)
    {
        data.Add(item);
    }

    public void Clear()
    {
        data.Clear();
    }

    public bool Contains(KeyValuePair<int, object> item)
    {
        return data.Contains(item);
    }

    public void CopyTo(KeyValuePair<int, object>[] array, int arrayIndex)
    {
        data.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<int, object> item)
    {
        return data.Remove(item);
    }

    public int Count => data.Count;

    public bool IsReadOnly => data.IsReadOnly;

    public IEnumerator<KeyValuePair<int, object>> GetEnumerator()
    {
        return data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)data).GetEnumerator();
    }

    internal object GetRoot()
    {
        throw new NotImplementedException();
    }

    /*
    public void ForEach(Action<ViewNode> action)
    {
        lock (Context)
        {
            foreach(var p in Context)
            {
                if(Context[p.Key]  is ViewNode)
                {
                    ((ViewNode)(Context[p.Key])).Do(action);
                }
            }
        }
    }


    public void Dispose()
    {            
        foreach (var p in Context)
        {     
            if(p.Value is ViewItem)
            {
                ((ViewItem)p.Value).Destroy();
            }
                    
        }
          
    }*/
}

