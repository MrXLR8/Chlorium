using DiscordBot;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscrodBot.Services
{
    public class Guesser
    {
        private readonly DatabaseContext db;
     
        public ILogger<Guesser> Nlog { get; }

        public static GuessedWord Active;


        public Guesser(ILogger<Guesser> nlog,DatabaseContext db)
        {
            Nlog = nlog;
            this.db = db;
        }

        public GuessedWord ActiveWord()
        {
            return  Active??db.GuessedWord.FirstOrDefault(w => w.active);
        }


        public void DeActivateWord()
        {
            GuessedWord word = db.GuessedWord.FirstOrDefault(w => w.active);
            if (word == null) return;
            word.deactivated = DateTime.Now;
            db.Update(word);
            word.active = false;
            db.SaveChanges();
            Active = null;
        }
        public void ActivateWord(GuessedWord word)
        {
            word.active = true;
            word.activated = DateTime.Now;
            db.Add(word);
            db.SaveChanges();
            Active = word;
        }

    }
}
