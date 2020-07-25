using Discord.WebSocket;

using System;

namespace DiscrodBot.Bot.Reactions
{
    public class AnnoReaction : Reaction
    {
        public override string[] phrases { get; set; } = new string[]
        {
            "А ты купил анношечку?",
            "Почему тебя не было в каточке в анно?",
            "Почему у тебя так мало часов в анно?",
            "Ты уже попробовал этот шедевр анно?",
            "Проверь EGS и Uplay на скидку Анно",
            "Ты же понимаешь что каждый раз когда ты незапускаешь анно, где то грустит хлор",
            "https://www.epicgames.com/store/ru/product/anno-1800",
            "https://store.ubi.com/ie/anno-1800/"
        };
        public override string[] trigger { get; set; } = new string[]
        {
            "анно",
            "anno"
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
