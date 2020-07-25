using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscrodBot
{
    public static class  Logic
    {

        public static bool containsEmoji(IMessage message )
        {

            string regex = "(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])";
            var reg = Regex.Match(message.Content, regex);
            if (reg.Success)
            {
                if (reg.Captures.Count == message.Content.Length - 1)
                    return true;
            }
            return  message.Tags.Any(t => t.Type == TagType.Emoji);
        }

        public static async Task<DateTime?> LastMessageTimeAsync(ISocketMessageChannel channel, SocketUser user)
        {
            IEnumerable<IMessage> hismessages =await channel.GetMessagesAsync(20).FlattenAsync();
               var lastmessage = hismessages
                   .Where(u => u.Author.Id == user.Id)
                   .OrderByDescending(m => m.CreatedAt)
                   .Skip(1)
                   .FirstOrDefault(m=>!containsEmoji(m));
               if (lastmessage == null) return null;
            
            return lastmessage.CreatedAt.UtcDateTime;
        }

        public static bool emojiFreeInterval(DateTime? a, DateTime? b,TimeSpan allowedinterval)
        {
            if (a - b < allowedinterval) return true;
            return false;
        }

        public static async Task Validate(SocketMessage message)
        {
            if(containsEmoji(message))
            {
                Console.WriteLine("There is emoji");
                DateTime? currentMessage = message.CreatedAt.UtcDateTime;
                DateTime? previousMessage = await LastMessageTimeAsync(message.Channel, message.Author);
                if(previousMessage==null)
                {
                    Console.WriteLine("Hes first message");
                    await message.DeleteAsync();
                    return;
                }
                if(!emojiFreeInterval(currentMessage,previousMessage,TimeSpan.FromSeconds(5))) 
                {
                    Console.WriteLine("Is in range");
                    await message.DeleteAsync();
                }
            }
        }
        
    }
}
