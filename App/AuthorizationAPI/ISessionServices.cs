using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISessionServices 
{
    public void AddHistory(string url);
    public TService Get<TService>();
    public int RegistrateModel(object model);
    public ConcurrentDictionary<int, object> GetModels();
    public object Find(int hash);
    public object FindByHash(int id);
}
