using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
namespace MCSharpUpdater
{
    public static class Updater
    {
        class CustomWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest req = (HttpWebRequest)base.GetWebRequest(address);
                req.ServicePoint.BindIPEndPointDelegate = BindIPEndPointCallback;
                req.UserAgent = "MCSharpUpdater";
                return req;
            }
        }
        public static INetListen Listener = new TcpListen();
        static IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEP, int retryCount)
        {
            IPAddress localIP;
            if (Listener.IP != null)
            {
                localIP = Listener.IP;
            }
            else if (!IPAddress.TryParse("0.0.0.0", out localIP))
            {
                return null;
            }
            if (remoteEP.AddressFamily != localIP.AddressFamily) return null;
            return new IPEndPoint(localIP, 0);
        }

        public static WebClient CreateWebClient() { return new CustomWebClient(); }
        public const string BaseURL = "https://github.com/RandomStrangers/MCSharp/raw/master/Uploads/";
        public static string dll = BaseURL + "MCSharp_.dll";
        public static string cli = BaseURL + "MCSharpCLI.exe";
        public static string exe = BaseURL + "MCSharp.exe";

        public static void PerformUpdate()
        {
            try
            {
                try
                {
                    DeleteFiles("MCSharp.update", "MCSharp_.update", "MCSharpCLI.update",
                        "prev_MCSharp.exe", "prev_MCSharp_.dll", "prev_MCSharpCLI.exe");
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error deleting files:");
                    Console.WriteLine(e.ToString());
                    Console.ReadKey(false);
                    return;
                }
                    try
                    {
                        WebClient client = HttpUtil.CreateWebClient();
                        File.Move("MCSharp.exe", "prev_MCSharp.exe");
                        File.Move("MCSharpCLI.exe", "prev_MCSharpCLI.exe");
                        File.Move("MCSharp_.dll", "prev_MCSharp_.dll");
                        client.DownloadFile(dll, "MCSharp_.update");
                        client.DownloadFile(cli, "MCSharpCLI.update");
                        client.DownloadFile(exe, "MCSharp.update");

                }
                catch (Exception x) 
                    {
                        Console.WriteLine("Error downloading update:");
                        Console.WriteLine(x.ToString());
                        Console.ReadKey(false);
                        return;
                    }
                File.Move("MCSharp.update", "MCSharp.exe");
                File.Move("MCSharpCLI.update", "MCSharpCLI.exe");
                File.Move("MCSharp_.update", "MCSharp_.dll");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error performing update:");
                Console.WriteLine(ex.ToString());
                Console.ReadKey(false);
                return;
            }
        }
        static void DeleteFiles(params string[] paths)
        {
            foreach (string path in paths) { AtomicIO.TryDelete(path); }
        }
    }
}
