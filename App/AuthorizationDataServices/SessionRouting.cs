using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog
{
    internal class SessionRouting
    {
        private readonly RequestDelegate _next;
   

        public SessionRouting(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext httpContext, RequestTransient request)
        {
            /*SessionContext context = request.GetSession();
            
            object controller = 
            (from p in context.Context.Keys
             where "." + p.Name == ReplaceAll(httpContext.Request.Path.ToString(), "/", ".")
             select context.Context[p]).FirstOrDefault();            
            if (controller != null && httpContext.Request.Query.ContainsKey("q"))
            {                
                string message = httpContext.Request.Query["q"].ToString();                
                Dictionary<string, object> parameters = 
                    JsonConvert.DeserializeObject<Dictionary<string,object>>(message);
                try
                {
                    if (parameters.ContainsKey("modelId") == false)
                    {
                        throw new Exception("В запросе необходимо передать параметр modelId");
                    }
                    else
                    if (parameters.ContainsKey("eventType") == false)
                    {
                        throw new Exception("В запросе необходимо передать параметр eventType");
                    }
                    else
                    if (parameters.ContainsKey("eventArgs") == false)
                    {
                        throw new Exception("В запросе необходимо передать параметр eventArgs");
                    }
                    else
                    {
                    
                        ViewItem root = (ViewItem)controller;
                        ViewItem target = (root).Find(parameters["modelId"].ToString());
                        if (target==null)
                        {
                            throw new Exception("Не найдена модель с идентификатором: "+ parameters["modelId"].ToString());
                        }

                        InvokeHelper.Do(target,
                            parameters["eventType"].ToString(),
                            parameters["eventArgs"]
                        );

                        var changes = root.GetChanges();
                        await httpContext.Response.WriteAsync(
                            JObject.FromObject(new
                            {
                                Changes = changes
                            }).ToString()
                        );
                                      
                    }
                }
                catch (Exception ex)
                {
                    httpContext.Response.StatusCode = 500;
                    await httpContext.Response.WriteAsync(ex.Message);
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }   */
            await _next.Invoke(httpContext);
        }

        private string ReadRequestBody(HttpContext httpContext)
        {
            var bodyStream = new StreamReader(httpContext.Request.Body);
           
            var bodyText = bodyStream.ReadToEnd();
            return bodyText;
        }

        private string ReplaceAll( string text, string s0, string s)
        {
            string result = text;
            while (result.IndexOf(s0) != -1)
            {
                result = result.Replace(s0, s);
            }
            return result;
        }
    }
}
