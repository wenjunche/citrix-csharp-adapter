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
    public class Echo: WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("Received: {0}", e.Data);
            Send(e.Data);
        }
    }

    class Program
    {
        [DllImport("ctxopenfin.dll")]
        static extern byte VD_LoadURL(string url);
        //            [MarshalAs(UnmanagedType.LPStr)] string url);


        static void Main(string[] args)
        {
            var cmd = new RootCommand
            {
                new Option<string>("--ws-url", "WebSocket server url"),
                new Option<string>("--url", "Redirected URL"),
            };

            cmd.Handler = CommandHandler.Create<string, string, IConsole>(HandleGreeting);
            cmd.Invoke(args);


        }

        static void HandleGreeting(string wsURL, string url, IConsole console)
        {

            if (url.IsNullOrEmpty())
            {
                Console.WriteLine("Missing redirect url");
            } 
            else
            {
                var r = VD_LoadURL(url);
            }

            Console.WriteLine("Starting Websocket server a {0}", wsURL);

            WebSocketServer ws = new WebSocketServer(wsURL);
            ws.AddWebSocketService<Echo>("/echo");

            ws.Start();
            Console.ReadKey();
            ws.Stop();

        }

    } // Program

}

