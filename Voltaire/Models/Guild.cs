using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Models
{
    public class Guild
    {
        public int ID { get; set; }
        public string DiscordId { get; set; }
        public bool AllowDirectMessage { get; set; } = true;
    }
}
