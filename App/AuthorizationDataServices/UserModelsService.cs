 

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using TeleReportsDataProvider;

/// <summary>
/// Сервис выполняет регистрацию обьектов в контексте сенса.
/// </summary>
public class UserModelsService : UserSessionService<ConcurrentDictionary<int, object>>, ClientAPI
{
    private readonly RequestTransient _request;
    private readonly APIUsers _users;
    private readonly NotificationsService _notifications;
    private readonly IServiceCollection _services;

    public UserModelsService( APIAuthorization authorization, 
                               APIUsers users,       
                                NotificationsService notifications, 
                                 RequestTransient request) : base(authorization)
    {
        this._request = request;
        this._users = users;
        this._notifications = notifications;
    }





    /// <summary>
    /// 
    /// </summary>

  /*  public ViewContext GetViewContext()
    {
        return _request.GetRoot();
    }

    */
    /// <summary>
    /// 
    /// </summary>
    public object BeforeRender()
    {
        var models = GetModels();
        if( models != null)
        {
            object p = null;
            foreach(var hashcode in new List<int>(models.Keys))
            {
                models.Remove(hashcode, out p);
                if( p !=null && p is ViewItem)
                {
                    ((ViewItem)p).RemoveFromParent();
                }
            }
        }
        return "";
    }


    /// <summary>
    /// 
    /// </summary>
    public string AfterRender() {
        return "";
    }


    /// <summary>
    /// 
    /// </summary>
    public HashSet<int> GetChanges()
    {
        HashSet<int> changes = new HashSet<int>();
        var models = GetModels();
        if(models != null)
        {
            foreach (object val in models.Values) { 
                if( val is ViewItem)
                {
                    ViewItem view = ((ViewItem)val);
                    if (view.ViewInitiallized)
                    {
                        if (view.WasChanged())
                        {
                            changes.Add(val.GetHashCode());
                        }
                    }
                }
            }
        }

        return changes;
    }



    public bool Has(object model)
    {
        return GetModels().ContainsKey(model.GetHashCode());
    }

    public object Remove(int hashcode)
    {
        object p = null;
        GetModels().Remove(hashcode, out p);
        if (p != null && p is ViewItem)
        {
            ((ViewItem)p).RemoveFromParent();
        }
        return p;
    }

    public object FindByHash(int code)
    {    
        try
        {
            var models = GetModels();
            if (models.ContainsKey(code))
            {
                var session = GetModels();
                object result = null;
                session.TryGetValue(code,out result);
                return result;
            }
            else
            {
                return null;
            }
        }
        catch(Exception ex)
        {
            return Formating.Json(new
            {
                Status = "Error",
                Type = "FindModelException",
                Errors = new
                {
                    Source = code,
                    Message = ex.Message,
                }
            });
        }        
    }




    public int RegistrateModel(object model)
    {
        int code = model.GetHashCode();
        
        if(model is ViewItem)
        {
            ((ViewItem)model).Code = code;
        }
        try
        {
            IDictionary<int,object> models = GetModels();
            if(models == null)
            {
                throw new Exception();
            }
            models[code] = model;
            if (model is ViewItem)
            {
                var sessionManager = _request.GetSessionManager();
                string sessionId = _request.Identify();
                ((ViewItem)model).HasRegistered = true;
                ((ViewItem)model).Client = this;
                ((ViewItem)model).GetSession = (d) =>
                {
                    
                    return sessionManager.GetById(sessionId).GetRoot();
                };
            }
        }
        catch(Exception ex)
        {
            if (model is ViewItem)
                ((ViewItem)model).HasRegistered = false;
            throw;
        }
        //if (_page.Find(code) == null)
        //{
        //    _page.Append(model);
        //}
        return code;
    }



    public IDictionary<int, object> GetModels()
    {
        var session = _request.GetSession();
        return session ;
        //return GetModel();
    }


    public override void InitModel(ConcurrentDictionary<int, object> model)
    {

    }

    public bool InfoDialog(string Title, string Text, string Button)
    {
        Enqueue(new ServerTask()
        {
            ToDo = (ctx) => {
                if (ctx is AbstractHub)
                {
                    var hub = (AbstractHub)ctx;                    
                    hub.InfoDialog(Title, Text, Button);                    
                }
                return null;
            }
        });
        return true;
    }

    private void Enqueue(ServerTask serverTask)
    {
        throw new NotImplementedException();
    }

    public bool RemoteDialog(string Title, string Url)
    {
        throw new NotImplementedException();
    }

    public bool ConfirmDialog(string Title, string Text)
    {
        Enqueue(new ServerTask()
        {
            ToDo = (ctx) => {
                if (ctx is AbstractHub)
                {
                    var hub = (AbstractHub)ctx;
                    hub.ConfirmDialog( Title, Text );
                }
                return null;
            }
        });
        return true;
    }

    public bool CreateEntityDialog(string Title, string Entity)
    {
        throw new NotImplementedException();
    }

    public object InputDialog(string Title, object Properties)
    {
        throw new NotImplementedException();
    }

    public string Eval(string js)
    {
        throw new NotImplementedException();
    }

    public string HandleEvalResult(Func<object, object> handle, string js)
    {
        Enqueue(new ServerTask()
        {
            ToDo = (ctx) => {
                if (ctx is AbstractHub)
                {
                    var hub = (AbstractHub)ctx;
                    hub.HandleEvalResult(handle,js);
                }
                return null;
            }
        });
        return "";
    }

    public string Callback(string action, params string[] args)
    {
        Enqueue(new ServerTask()
        {
            ToDo = (ctx) => {
                if (ctx is AbstractHub)
                {
                    var hub = (AbstractHub)ctx;
                    hub.Callback(action, args);
                }
                return null;
            }
        });
        return "";
    }

    public bool AddEventListener(string id, string type, string js)
    {
        throw new NotImplementedException();
    }

    public bool DispatchEvent(string id, string type, object message)
    {
        throw new NotImplementedException();
    }

    public string OnConnected(string token)
    {
        throw new NotImplementedException();
    }

    public void ShowHelp(string Text)
    {
        Enqueue(new ServerTask()
        {
            ToDo = (ctx) => {
                if (ctx is AbstractHub)
                {
                    var hub = (AbstractHub)ctx;
                    hub.ShowHelp(Text);
                }
                return null;
            }
        });        
    }
}
