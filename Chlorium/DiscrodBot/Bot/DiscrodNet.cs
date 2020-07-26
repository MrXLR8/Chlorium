
using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Logging;

using NLog;

using System;
using System.Collections.Generic;
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
            if (!await Logic.DeleteStupidEmojis(message)) 
                Reaction.CheckForReaction(message);
            
        }

        public static async Task<IMessage> LastUserMessage(ISocketMessageChannel channel, SocketUser user,int messagecount=20)
        {
            IEnumerable<IMessage> hismessages = await channel.GetMessagesAsync(messagecount).FlattenAsync();
            IMessage lastmessage = hismessages
                .Where(u => u.Author.Id == user.Id)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(1)
                .FirstOrDefault();
            
            if (lastmessage == null) return null;

            return lastmessage;
        }

        public static async void Auth()
        {
                var token = Environment.GetEnvironmentVariable("DiscordToken");
            if (token == null) throw new Exception("Please fill discord token env. value");

#if DEBUG
            await client.SetGameAsync("новые фичи(баги)", null, ActivityType.Watching);
#endif
#if !DEBUG
          await  client.SetGameAsync("за школьниками",null,ActivityType.Watching);
#endif
            await client.LoginAsync(TokenType.Bot,token);

            await client.StartAsync();

           
            nlog.Info("Discord Logged in");
        }

        public static  async void Stop()
        {
            nlog.Info("Stopping Discord");
            await client.StopAsync();
        }




    }
}
