using Discord.WebSocket;

using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscrodBot
{
    public abstract class Reaction
    {
        public static List<Reaction> reactList;
        private static Logger nlog;

        public abstract string[] phrases { get; set; }
        public abstract string[] trigger { get; set; }

        public static void init()
        {
            nlog = LogManager.GetCurrentClassLogger();
        }
        public static async void CheckForReaction(SocketMessage message)
        {
            if (reactList == null) return;
            foreach (var item in reactList)
            {
                if (await item.Execute(message)) return;
            }
            
            
        }

        protected bool checktriggerphrase(string content)
        {
          return trigger.Any(s => content.ToLower().Contains(s));
        }
        protected abstract bool check(SocketMessage message);
        protected abstract string respond(SocketMessage message);

        public async Task<bool>  Execute(SocketMessage message)
        {
            
            if (check(message))
            {
                nlog.Info($"Reacted {message.Author.Username}:{message.Content}");
                await message.Channel.SendMessageAsync(respond(message));
                return true;
            }
            return false;
        }
    }
}
