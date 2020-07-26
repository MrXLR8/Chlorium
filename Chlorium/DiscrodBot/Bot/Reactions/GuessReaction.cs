using Discord.WebSocket;

using System;

namespace DiscrodBot.Bot.Reactions
{
    //by Asan
    public class GuessReaction : Reaction
    {
        public override string[] phrases { get; set; } = new string[]
        {
            "Анно калл",
            "в ск не играл",
            "качай тундру"
        };
        public override string[] trigger { get; set; } = new string[]
        {
            "asan",
            "асан"
        };

        protected override bool check(SocketMessage message)
        {
            return checktriggerphrase(message.Content) && !message.Author.IsBot
            #if !DEBUG
            &&!message.Author.Username.Contains("XLR8");
            #endif
            #if DEBUG
                ;
            #endif
        }

        protected override string respond(SocketMessage message)
        {
            return phrases[new Random().Next(0, phrases.Length)];
        }
    }
}
