using Discord;
using Discord.WebSocket;

using NLog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscrodBot
{
    public static class Logic
    {
        static Logger nlog;
        public static void init()
        {
            nlog = LogManager.GetCurrentClassLogger();
        }
        public static bool containsEmoji(IMessage message)
        {

            string regex = "(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])";
            var reg = Regex.Match(message.Content, regex);
            if (reg.Success)
            {
                if (reg.Captures.Count == message.Content.Length - 1)
                    return true;
            }
            return message.Tags.Any(t => t.Type == TagType.Emoji);
        }

        public static async Task<IMessage> LastMessage(ISocketMessageChannel channel, SocketUser user)
        {
            IEnumerable<IMessage> hismessages = await channel.GetMessagesAsync(20).FlattenAsync();
            var lastmessage = hismessages
                .Where(u => u.Author.Id == user.Id)
                .OrderByDescending(m => m.CreatedAt)
                .Skip(1)
                .FirstOrDefault(m => !containsEmoji(m));
            if (lastmessage == null) return null;

            return lastmessage;
        }

        public static bool emojiFreeInterval(DateTime? a, DateTime? b, TimeSpan allowedinterval)
        {
            if (a - b < allowedinterval) return true;
            return false;
        }

        public static async Task<bool> DeleteStupidEmojis(SocketMessage message)
        {
            if (containsEmoji(message))
            {
                nlog.Info($"{message.Author} : {message.Content} ");
                IMessage lastmessage = await LastMessage(message.Channel, message.Author);
                DateTime? currentMessage = message.CreatedAt.UtcDateTime;
                DateTime? previousMessage = lastmessage.CreatedAt.UtcDateTime;
                if (previousMessage == null)
                {
                    nlog.Info($"{message.Author} no previous messages. Deleting");

                    await message.DeleteAsync();
                    return true;
                }
                if (!emojiFreeInterval(currentMessage, previousMessage, TimeSpan.FromSeconds(5)))
                {
                    nlog.Info($"{message.Author} posted with no message.");
                    await message.DeleteAsync();
                    await CreateReactions(lastmessage);
                   
                    return true;
                }
            }
            return false;
        }

        static List<Emoji> emojis = new List<Emoji>()
        {
               new Emoji("🇮"),
               new Emoji("◻️"),
               new Emoji("🅰️"),
               new Emoji("🇲"),
               new Emoji("⬜"),
               new Emoji("🇬"),
               new Emoji("🇦"),
               new Emoji("🇾")
        };

        public static async Task CreateReactions(IMessage message)
        {

            foreach (var item in emojis)
            {
              await  message.AddReactionAsync(item);
            }
        }
        
    }
}
