using Discord.WebSocket;

using System;

namespace DiscrodBot.Bot.Reactions
{
    public class YandexReaction : Reaction
    {
        public override string[] phrases { get; set; } = new string[]
        {
            "Юзай гугл",
            "Яндекс параша",
            "Хуяндекс",
            "Алиса нихуя не умеет",
            "Нахуй ты юзаешь сервисы яндекса?",
            "xepnЯ - перевернутый яндекс"
        };
        public override string[] trigger { get; set; } = new string[]
        {
            "yandex",
            "яндекс"
        };
        protected override bool check(SocketMessage message)
        {

            //&& 
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
