using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RequestifyTF2.API;
using RequestifyTF2.Managers;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;


namespace APIPlugin
{
    public class APIPlugin : IRequestifyPlugin
    {

        public string Author => "Weespin";
        public string Name => "API";
        public string Desc => "Mini API Server for controlling";
        private int Port => 7233;

        public void OnLoad()
        {

            Console.WriteLine("APIPlugin Loaded");
            new Thread(StartServer).Start();
            Console.WriteLine("OK");
            Logger.Write(Logger.Status.STATUS, "OK");

        }

        public void StartServer()
        {
            // The routing strategy is Wildcard by default, but you can change it to Regex as follows:
            var server = new WebServer($"http://localhost:{Port}/", RoutingStrategy.Simple);

            server.RegisterModule(new WebApiModule());
            server.Module<WebApiModule>().RegisterController<InstanceController>();
            server.RunAsync();



        }

        public class InstanceController : WebApiController
        {
            [WebApiHandler(HttpVerbs.Get, "/api/plugins/get")]
            public bool GetPlugins(WebServer server, HttpListenerContext context)
            {
                try
                {

                    context.JsonResponse(JsonConvert.SerializeObject(Instance.Plugins.GetPlugins()));
                    return true;

                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/queue/get")]
            public bool GetBackGroundQueue(WebServer server, HttpListenerContext context)
            {
                try
                {

                    context.JsonResponse(JsonConvert.SerializeObject(Instance.BackGroundQueue.PlayList));
                    return true;

                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/getlanguage")]
            public bool GetLanguage(WebServer server, HttpListenerContext context)
            {
                try
                {

                    context.JsonResponse(JsonConvert.SerializeObject(Instance.Language.ToString()));
                    return true;

                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/getcommands")]
            public bool GetCommands(WebServer server, HttpListenerContext context)
            {
                try
                {

                    context.JsonResponse(JsonConvert.SerializeObject(Instance.Commands.GetCommands()));
                    return true;

                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/getadminname")]
            public bool GetAdminName(WebServer server, HttpListenerContext context)
            {
                try
                {

                    context.JsonResponse(JsonConvert.SerializeObject(Instance.Config.Admin));
                    return true;

                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/ignorelist/get")]
            public bool GetIgnoreList(WebServer server, HttpListenerContext context)
            {
                try
                {

                    context.JsonResponse(JsonConvert.SerializeObject(Instance.Config.Ignored));
                    return true;

                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/ignorelist/getrevese")]
            public bool GetReveseIgnore(WebServer server, HttpListenerContext context)
            {
                try
                {

                    context.JsonResponse(JsonConvert.SerializeObject(Instance.Config.IgnoredReversed));
                    return true;

                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/queue/addsong/{type}/{link}/{requester}/{title}")]
            public bool AddSong(WebServer server, HttpListenerContext context, string type, string link, string requester, string title)
            {
                type = context.Request.Url.Segments[4];
                link = context.Request.Url.Segments[5];
                requester = context.Request.Url.Segments[6];
                title = context.Request.Url.Segments[7];
                try
                {
                    switch (type)
                    {
                        case "AAC":
                            return Instance.AddEqueue(Instance.SongType.AAC, link, requester, title);
                        case "MP3":
                            return Instance.AddEqueue(Instance.SongType.MP3, link, requester, title);
                        default:
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/queue/removesong/{requester}/{title}")]
            public bool RemoveSong(WebServer server, HttpListenerContext context, string requester, string title)
            {
               requester= context.Request.Url.Segments[4];
                title = context.Request.Url.Segments[5];
                try
                {
                    var a = Instance.BackGroundQueue.PlayList
                        .FirstOrDefault(n => n.Title == title && n.RequestedBy.Name == requester);
                    if (a != null)
                    {
                        a.Dequeued = true;
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/ignorelist/add/{name}")]
            public bool AddToIgnore(WebServer server, HttpListenerContext context, string name)
            {
                try
                {

                    name = context.Request.Url.Segments[4];
                    Instance.Config.Ignored.Add(name);
                    return true;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/ignorelist/delete/{name}")]
            public bool RemoveFromIgnore(WebServer server, HttpListenerContext context, string name)
            {
                name = context.Request.Url.Segments[4];
                try
                {
                    
                    if (Instance.Config.Ignored.Contains(name))
                    {
                        Instance.Config.Ignored.Remove(name);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/ignorelist/reversed/{value}")]
            public bool IgnoreReversed(WebServer server, HttpListenerContext context, bool value)
            {
                value = Convert.ToBoolean(context.Request.Url.Segments[4]);
                try
                {
                    Instance.Config.IgnoredReversed = value;
                    return true;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/plugins/enable/{name}")]
            public bool EnablePlugin(WebServer server, HttpListenerContext context, string value)
            {
                value = context.Request.Url.Segments[4];
                try
                {
                    var a = Instance.Plugins.GetPlugin(value);
                    if (a != null)
                    {
                        if (a.Status == PluginManager.Status.Disabled)
                        {
                            Instance.Plugins.EnablePlugin(a);
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/plugins/disable/{value}")]
            public bool DisablePlugin(WebServer server, HttpListenerContext context, string value)
            {
                value = context.Request.Url.Segments[4];
                try
                {
                    var a = Instance.Plugins.GetPlugin(value);
                    if (a != null)
                    {
                        if (a.Status == PluginManager.Status.Enabled)
                        {
                            Instance.Plugins.DisablePlugin(a);
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/commands/disable/{value}")]
            public bool DisableCommand(WebServer server, HttpListenerContext context, string value)
            {
                value = context.Request.Url.Segments[4];
                try
                {
                    var a = Instance.Commands.GetCommand(value);
                    if (a != null)
                    {
                        if (a.Status == CommandManager.Status.Enabled)
                        {
                            Instance.Commands.DisableCommand(a);
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }
            [WebApiHandler(HttpVerbs.Get, "/api/commands/enable/{value}")]
            public bool EnableCommand(WebServer server, HttpListenerContext context, string value)
            {
                value = context.Request.Url.Segments[4];
                try
                {
                    var a = Instance.Commands.GetCommand(value);
                    if (a != null)
                    {
                        if (a.Status == CommandManager.Status.Disabled)
                        {
                            Instance.Commands.EnableCommand(a);
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return context.JsonExceptionResponse(ex);
                }
            }


        }


    }
   


}
