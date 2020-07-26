using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscrodBot.Services
{
    public class DiscordClient
    {
        public DiscordSocketClient client = new DiscordSocketClient();
        private readonly ILogger<DiscordClient> nlog;
        string token;

        public IServiceProvider provider { get; }

        public DiscordClient(ILogger<DiscordClient> nlog,IServiceProvider provider)
        {
            this.nlog = nlog;
            this.provider = provider;
            token = Environment.GetEnvironmentVariable("DiscordToken");
            if (token==null) 
                throw new Exception("Please fill discord token env. value");
            Auth();
            RegisterEvent(Client_MessageReceived);
        }

        public async void Auth()
        {
            #if DEBUG
            await client.SetGameAsync("новые фичи(баги)", null, ActivityType.Watching);
            #endif
            #if !DEBUG
            await  client.SetGameAsync("за школьниками",null,ActivityType.Watching);
            #endif
            await client.LoginAsync(TokenType.Bot, token);

            await client.StartAsync();

          
            nlog.LogInformation("Discord Logged in");
        }

        public   void RegisterEvent(Func<SocketMessage,Task> @event)
        {

            client.MessageReceived += @event;
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            if (arg.Author.IsBot) return;

            if (arg.Channel is SocketDMChannel)
            {
                ActivatorUtilities.CreateInstance<DMService>(provider.CreateScope().ServiceProvider).Process(arg);
            }
            else
            {
                ActivatorUtilities.CreateInstance<ChannelMonitor>(provider.CreateScope().ServiceProvider).Process(arg);
            }
            return;
        }

        public void DeregisterEvent(Func<SocketMessage, Task> @event)
        {

            client.MessageReceived -= @event;
        }
        
        public async void Reply(ISocketMessageChannel channel, string text)
        {
            try
            {
                await channel.SendMessageAsync(text);
            }
            catch(Exception e)
            {
                nlog.LogError(e.ToString());
            }
        }

        public async void DM(SocketUser target,string text)
        {
            try
            {
                await target.SendMessageAsync(text, true);
            }
            catch (Exception e)
            {
                nlog.LogError(e.ToString());
            }
        }


        public   async Task<IMessage> LastUserMessage(ISocketMessageChannel channel, SocketUser user, int messagecount = 20)
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

       

        public   async void Logoff()
        {
            nlog.LogInformation("Stopping Discord");
            await client.StopAsync();
        }



    }
}
