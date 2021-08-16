using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Runtime.InteropServices;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using WebSocketSharp.Server;


namespace citrix_csharp_demo
{
    public class CTXService: WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Console.WriteLine("WS client connected");
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received: {0}", e.Data);
//            Send(e.Data);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("WS Client closed: {0}", e.Reason);
        }
    }

    class Program
    {
        [DllImport("ctxopenfin.dll")]
        static extern byte VD_LoadURL(string url);
        //            [MarshalAs(UnmanagedType.LPStr)] string url);

        private static String CTX_SERVICE_PATH = "/ctxservice";

        static void Main(string[] args)
        {
            var cmd = new RootCommand
            {
                new Option<string>("--ws-url", "WebSocket server url"),
                new Option<string>("--proxy-url", "Manifest URL for OpenFin proxy app"),
            };

            cmd.Handler = CommandHandler.Create<string, string, IConsole>(HandleGreeting);
            cmd.Invoke(args);
        }

        static void HandleGreeting(string wsURL, string proxyUrl, IConsole console)
        {

            if (proxyUrl.IsNullOrEmpty())
            {
                Console.WriteLine("Missing proxy url");
            } 
            else
            {
                string url = String.Format("{0}/?$$wsurl={1}{2}", proxyUrl, wsURL, CTX_SERVICE_PATH);
                Console.WriteLine("Invoking proxy app at {0}", url);
                var r = VD_LoadURL(url);
            }

            Console.WriteLine("Starting Websocket server a {0}", wsURL);
            WebSocketServer wssv = new WebSocketServer(wsURL);
            Console.WriteLine("Registring CTX service path {0}", CTX_SERVICE_PATH);
            wssv.AddWebSocketService<CTXService>(CTX_SERVICE_PATH);
            wssv.Start();

            Console.WriteLine("Enter JSON string to send to apps at client side");
            String s;
            do {
                s = Console.ReadLine();
                if (s != "exit")
                {
                    wssv.WebSocketServices.Hosts.FirstOrDefault().Sessions.Broadcast(s);
                }
            } while (s != "exit");

            wssv.Stop();

        }

    } // Program

}

