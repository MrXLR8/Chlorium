using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscrodBot.Services
{




    public class Command
    {
        private readonly SocketMessage message;
        static string commandTrigger = ";;";

        public string command;
        public List<string> arguments = new List<string>();
        public bool isValid = false;
        public Command(SocketMessage message)
        {
            
            this.message = message;

            isValid = Fill();
        }

        public bool isAllowed(IEnumerable<string> whitelist)
        {
            if (command == null) return false;
            return whitelist.Contains(command);
        }

        public bool Fill()
        {
            string content = message.Content;
            string result = content;
            if (!content.Contains(commandTrigger)) return false;
            string[] splitedSymbol = content.Split(commandTrigger);
            if (content.Split(commandTrigger).Count() != 2) return false;
            string[] splitedCommands = splitedSymbol[1].Split(" ");
            command = splitedCommands.First();
            arguments.AddRange(splitedCommands.Skip(1));
            return true;
        }
         
    }
}
