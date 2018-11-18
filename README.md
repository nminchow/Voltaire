# Voltaire

**Voltaire** is a discord bot that allows you to send messages anonymously. It supports sending messages to server channels as well as sending DMs to members of the server it has been added to. Voltaire has several admin settings (which can be viewed with `!volt admin`) to allow admins to best fit the bot to their use case. When Voltaire is added to your server, type `!volt help` to get a list of commands.

[Official Voltaire Discord](https://discord.gg/xyzMyJH)

## Built With

* [Discord.net](https://github.com/RogueException/Discord.Net) - Bot Framework

## Contributing

Pull requests are welcome!

#### Development setup

To get running locally:
1. Create a [discord bot user](https://discordapp.com/developers/applications/)
2. Set up a sql database
3. Create a appsettings.json file within the project's "Voltaire" directory (see example below)
4. Run migrations
5. Be excellent to eachother

```
// appsettings.json
{
  "discordAppToken": "F5OCongcjYOMXmEgrTmGDFy1Te5CUZy5ignm2DLoUUwJ1QsbfqEeOpyWBhe",
  // the emoji the bot will use when a message is sent
  "sent_emoji": "<:message_sent:491776018970050570>",
  // a 256 bit key used to generate response codes and usernames
  "encryptionKey": "PSVJQRk9QTEpNVU1DWUZCRVFGV1VVT0ZOV1RRU1NaWQ=",
  "ConnectionStrings": {
    "sql": "Server=(localdb)\\mssqllocaldb;Database=Artifact;Trusted_Connection=True;"
  }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
