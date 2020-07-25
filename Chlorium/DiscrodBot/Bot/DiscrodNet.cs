
using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Logging;

using NLog;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace DiscrodBot
{
    public static class DiscordNet
    {
        static DiscordSocketClient client = new DiscordSocketClient();
        static Logger nlog;
        public static void Start()
        {
            nlog = LogManager.GetCurrentClassLogger();
            Auth();
            client.MessageReceived += MessageReceived;
        }

        private static async Task MessageReceived(SocketMessage message)
        {
           await Logic.Validate(message);
           
        }

        public static async void Auth()
        {
                var token = Environment.GetEnvironmentVariable("DiscordToken");
            if (token == null) throw new Exception("Please fill discord token env. value");
            await client.LoginAsync(TokenType.Bot,token);
            await client.StartAsync();


            nlog.Info("Discord Logged in");
        }




    }
}
