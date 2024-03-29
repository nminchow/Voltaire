﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Views
{
    public static class Message
    {

        public static Tuple<string, Embed> Response(string username, string message)
        {
            EmbedBuilder embed = BuildEmbed(message);

            if (!string.IsNullOrEmpty(username))
            {
                embed.Author = new EmbedAuthorBuilder
                {
                    Name = username
                };
            }

            return new Tuple<string, Embed>("", embed.Build());
        }

        private static EmbedBuilder BuildEmbed(string message)
        {
            return new EmbedBuilder
            {
                Description = message,
                Color = new Color(111, 111, 111)
            };
        }
    }
}
