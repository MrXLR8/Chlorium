using Discord.WebSocket;

using DiscordBot;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;

namespace DiscrodBot.Services
{
    public class ChannelMonitor
    {
        private readonly DatabaseContext db;
        private readonly Guesser guesser;

        public ChannelMonitor(DiscordClient discord, ILogger<DMService> nlog, DatabaseContext db,Guesser guesser)
        {
            Discord = discord;
            this.db = db;

            this.guesser = guesser;
        }

        public DiscordClient Discord { get; }
        public SocketMessage Message { get; set; }
       
        public bool checkForWord(string content )
        {
            string triggerword = guesser.ActiveWord()?.word;
            if(string.IsNullOrEmpty(triggerword)) return false;

            if (triggerword == content) return true;
            if (content.StartsWith(triggerword + " ")) return true;
            if (content.EndsWith(" " + triggerword)) return true ;
            if (content.Contains(" " + triggerword + " ")) return true;
            return false;
        }

        public void Process(SocketMessage message)
        {
            if(checkForWord(message.Content))
            {

                User sender = db.Find<User>(message.Author.Id.ToString());
                if (sender == null)
                {
                    sender = new User()
                    {
                        id = message.Author.Id.ToString(),
                        username = message.Author.Username
                    };
                    db.Add(sender);
                }
                #if DEBUG
                if (sender.id == guesser.ActiveWord().user.id) return; //ignore owner
                #endif
                sender.points++;
                Discord.Reply(message.Channel, $"{sender.username} угадал загаданное слово ``{guesser.ActiveWord().word}`` \n Успей загадать готовое слово в личку боту. Например ``;;загадать пельмень``");
                Discord.DM(message.Author, $"Ты угадал загаданное слово ``{guesser.ActiveWord().word}`` \n Успей загадать готовое слово в личку боту. Например ``;;загадать мама`` \n\n Твое количество очков: ``{sender.points}``");
                guesser.DeActivateWord();
              
            }
        }



    }
}
