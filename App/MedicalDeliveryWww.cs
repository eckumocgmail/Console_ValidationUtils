using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace pickpoint_delivery_service.Module.Backend
{
    public class MedicalDeliveryWww : BackgroundService
    {
        private ConcurrentQueue<Action<IHostedService>> Queue;
        private Process _process;
        private int _port { get; set; } = 8080;
        public MedicalDeliveryWww()
        {
            Queue = new ConcurrentQueue<Action<IHostedService>>();
        }

        

        public void Stop() => _process.Kill();
        public void Start()
        {
            Console.WriteLine("перенаправление порта " + _port + " в интернет ");
            _process = Process.Start("powershell", @"/C ngrok.exe http " + _port);
    
        }





        public void Run()
        {
            StartInBackground();
        }
        public void StartInBackground()
        {
            Console.WriteLine("перенаправление порта " + _port + " в интернет ");
            Thread work = new Thread(new ThreadStart(() => {
                ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " + "ngrok http " + _port);

                info.RedirectStandardError = false;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                _process = System.Diagnostics.Process.Start(info);
                string line = null;
                while (((line = _process.StandardOutput.ReadLine())) != null)
                {
                    Console.WriteLine(line);
                    Console.WriteLine(line);
                    Console.WriteLine(line);
                    Console.WriteLine(line);
                    Console.WriteLine(line);
                    Console.WriteLine(line);
                    Console.WriteLine(line);

                    if (line.IndexOf("Forwarding") != -1)
                    {
                        Console.WriteLine(line);
                    }

                }

            }));
            work.IsBackground = true;
            work.Start();

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            return Task.Run(() => {
                
                Start();
                Thread.Sleep(Timeout.Infinite);
            });
        }
    }
}
