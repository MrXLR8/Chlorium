using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using System;
using System.Threading.Tasks;

namespace DiscrodBot
{
    public static class DSharpPlus
    {
       static DiscordClient client;

        public static void Start()
        {
            Auth();
            client.MessageCreated += MessageRecieved;
        }

        public static void Auth()
        {
            client = new DiscordClient(new DiscordConfiguration
            {
                Token = Environment.GetEnvironmentVariable("DiscordToken"),
                TokenType = TokenType.Bot,
                
            });
            client.ConnectAsync().Wait();
            Console.WriteLine("Connected");
        }

        static async Task MessageRecieved(MessageCreateEventArgs ea)
        {
       
            Console.WriteLine("Triggered");
            string author = ea.Message.Author.Username;
            if (author.Contains("XLR8")) await ea.Message.DeleteAsync();
        }


    }
}
