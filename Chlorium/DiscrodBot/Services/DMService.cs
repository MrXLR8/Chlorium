using Discord.WebSocket;

using DiscordBot;

using Microsoft.Extensions.Logging;

using System;
using System.Text.RegularExpressions;

namespace DiscrodBot.Services
{
    public class DMService
    {
        private readonly ILogger<DMService> nlog;
        private readonly DatabaseContext db;
        private readonly Guesser guesser;

        public DMService(DiscordClient discord, ILogger<DMService> nlog, DatabaseContext db, Guesser guesser)
        {
            Discord = discord;
            this.nlog = nlog;
            this.db = db;

            this.guesser = guesser;
        }

        public DiscordClient Discord { get; }
        public SocketMessage Message { get; set; }



        public void Process(SocketMessage message)
        {
            Message = message;
            Command cmd = new Command(message);
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


            GuessedWord active = guesser.ActiveWord();

            if (!cmd.isValid)
            {
                Discord.Reply(Message.Channel, "Неизвестная комманда, напиши ;;help");
                return;
            }

            switch (cmd.command)
            {
                case "кто":
                    if (active != null)
                    {
                        Discord.Reply(Message.Channel, $"Слово загадал {active.user.username} в {active.activated}");
                    }
                    else
                    {
                        Discord.Reply(Message.Channel, $"Слово никто не загадал! Будь первым! Напиши ``;;загадать абрикос``");
                    }

                    break;
                case "загадать":
                    if (active != null)
                    {
                        TimeSpan timepassed = DateTime.Now - active.activated;
                        if (timepassed.TotalHours < 24)
                        {
                            Discord.Reply(Message.Channel, $"Слово загадал ``{active.user.username}`` в ``{active.activated}``. \n Реши прежде его слово, чтобы получить возможность загадать своё");
                            return;
                        }
                        nlog.LogInformation("Question expired");
                        guesser.DeActivateWord();
                    }

                    string combinedword = string.Join(" ", cmd.arguments);
                    if (combinedword.Length < 3)
                    {
                        Discord.Reply(Message.Channel, $"Давай что-то по длинее придумай :wink:");
                        return;
                    }
                    if (combinedword.Length > 12)
                    {
                        Discord.Reply(Message.Channel, $"Они же никогда в жизни не угадают, за что ты так с ними? :weary:  \n Давай другое");
                        return;
                    }
                    if (WordValidator(combinedword))
                    {
                        Discord.Reply(Message.Channel, $"Попробуй загадать слово используя лишь буквы");
                        return;
                    }

                    GuessedWord word = new GuessedWord()
                    {
                        user = sender,
                        word = combinedword.ToLower()
                    };

                    guesser.ActivateWord(word);

                    Discord.Reply(Message.Channel, $"Хорошо, пусть попробуют теперь угадать {word.word}");


                    break;
                case "очки":
                    Discord.Reply(Message.Channel, $"Твои баллы: {sender.points}");
                    break;
                case "забудь":
                    if (active == null)
                    {
                        Discord.Reply(Message.Channel, $"Так ничего же загадано!");
                        return;
                    }
                    if (sender.id != active.user.id)
                    {
                        Discord.Reply(Message.Channel, $"Хех, хитрый. Не ты загадывал это слово. Попроси {active.user.username} чтобы он отменил, или дождись 24 часа");
                        return;
                    }
                    guesser.DeActivateWord();
                    Discord.Reply(Message.Channel, $"ОК. Готов к новому слову");
                    break;
                case "help":
                    break;
                default:
                    Discord.Reply(Message.Channel, "Неизвестная комманда, напиши ;;help");
                    break;
            }

        }


        private bool WordValidator(string content)
        {

            return new Regex(@"[^А-Яа-я A-Za-z]").IsMatch(content);
        }


    }
}
