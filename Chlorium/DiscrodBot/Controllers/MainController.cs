using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DiscordBot;

using DiscrodBot.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscrodBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly DatabaseContext db;

        public MainController(DatabaseContext db,Guesser guesser)
        {
            this.db = db;
            Guesser = guesser;
        }

        public DiscordClient Disc { get; }
        public Guesser Guesser { get; }

        [HttpGet("GetWord")]
        public ActionResult GetWord()
        {
            return Ok(db.GuessedWord.FirstOrDefault(w => w.active));
        }

        [HttpGet("CancelWord")]
        public ActionResult CancelWord()
        {
            Guesser.DeActivateWord();
            return Ok();
        }

        [HttpGet("GetUsers")]
        public ActionResult GetUsers()
        {
            return Ok(db.Users.ToList());
        }

        [HttpGet("GetWords")]
        public ActionResult GetWords()
        {
            return Ok(db.GuessedWord.ToList());
        }
    }
}
