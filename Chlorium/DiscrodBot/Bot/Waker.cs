using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiscrodBot.Bot
{
    public class Waker
    {
        HttpClient client = new HttpClient();
        static Logger nlog;
        string url;
        int ms;
        public Waker()
        {
            nlog = LogManager.GetCurrentClassLogger();
            url = Environment.GetEnvironmentVariable("SwaggerUrl");
            
        }
        public void Start(int ms)
        {
            if(string.IsNullOrEmpty(url))
            {
                nlog.Warn("Empty swaggerurl. no waker will be started");
                return;
            }
            this.ms = ms;
            new Thread(ThreadTask).Start();
            nlog.Info($"Waker started");
        }

        async void ThreadTask()
        {
            while(true)
            {

                try
                {
                    
                    var resp = await client.GetAsync(url);
                    resp.EnsureSuccessStatusCode();
                    nlog.Info($"Waked Status Code:{resp.StatusCode}");
                }
                catch (Exception e)
                {
                    nlog.Error(e);
                }
                Thread.Sleep(ms);
                   
            }
        }

    }
}
