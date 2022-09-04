using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace TeleReportsDataProvider
{
    public class AbstractHub : Hub, ClientAPI
    {
        protected readonly UserModelsService _models;



        public AbstractHub( UserModelsService models): base()
        {
            _models = models;
        }



        public bool AddEventListener(string id, string type, string js)
        {
            throw new NotImplementedException();
        }

        public string Callback(string action, params string[] args)
        {
            string paramsstr = "";
            foreach(string arg in args)
            {
                paramsstr += "'" + arg + "',";
            }
            if (paramsstr.EndsWith(",")) paramsstr = paramsstr.Substring(0, paramsstr.Length - 1);
            return Eval($"{action}({paramsstr})");
        }

        public bool ConfirmDialog(string Title, string Text)
        {
          
            CallbackMessage("confirmDialog", (resp)=> {
                Writing.ToConsole(resp);
                return null;
            },  "", "test");
            return true;
        }

        private string CallbackMessage(string action, Func<string, Action> handler, params string[] args)
        {

            /*new RequestMessage()
            {
                ActionName = action,
                
            };*/
            string paramsstr = "";
            foreach (string arg in args)
            {
                paramsstr += "'" + arg + "',";
            }
            if (paramsstr.EndsWith(",")) paramsstr = paramsstr.Substring(0, paramsstr.Length - 1);
            
            var task = Clients.Client(Context.ConnectionId).SendAsync("eval", $"{action}({paramsstr})");
            task.ConfigureAwait(true);
            task.Wait();            
            return Formating.ToJson(new
            {
                Status = "Success"
            });
        }

        public bool DispatchEvent(string id, string type, object message)
        {
            throw new NotImplementedException();
        }

        public string Eval(string js)
        {
            return EvalConnection(Context.ConnectionId,js);
        }

        public string EvalConnection(string ConnectionId,string js)
        {
            var task = Clients.Client(ConnectionId).SendAsync("eval", js);
            task.ConfigureAwait(true);
            return Formating.ToJson(new
            {
                Status = "Success"
            });
        }

        public bool CreateEntityDialog(string Title, string Entity )
        {
            Callback("createEntityDialog", Title, Entity);
            return true;
        }

        public object InputDialog(string Title, object Properties)
        {
            Callback("infoDialog", "test", "test");
            return true;
        }

        public string OnConnected(string token)
        {
            throw new NotImplementedException();
        }

        //
        public bool RemoteDialog(string Title, string Url )
        {
            Callback("remoteDialog", Title, Url);
            return true;
        }

        public bool InfoDialog(string Title, string Text, string Button)
        {
            Callback("infoDialog",Title, Text, Button);
            return true;
        }

        public string HandleEvalResult(Func<object, object> handle, string js)
        {
            string serialkey = PushCallback((json) =>
            {
                return handle(json);
            });
            Callback("returnEvalResult", serialkey, js, "");
            return null;
        }

        private string PushCallback(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        public string ReturnEvalResult(string message)
        {
            Writing.ToConsole(message);
            ClientResponse resp = JsonConvert.DeserializeObject<ClientResponse>(message);
            Func<object,object> callback = TakeCallback(resp.serialkey);
            var res = callback(resp.response);
            return "FromHub => "+res;
        }

        private Func<object, object> TakeCallback(string serialkey)
        {
            throw new NotImplementedException();
        }

        public void ShowHelp(string Text)
        {
            Callback("$app.$help.$show",Text);
        }

    }






    class ClientResponse
    {
        public string serialkey { get; set; }
        public object response { get; set; }
    }
}
