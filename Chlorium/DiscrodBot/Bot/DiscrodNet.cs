
using Discord;
using Discord.WebSocket;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace DiscrodBot
{
    public static class DiscordNet
    {
        static DiscordSocketClient client = new DiscordSocketClient();

        public static void Start()
        {
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


            Console.WriteLine("Connected");
        }




    }
}
