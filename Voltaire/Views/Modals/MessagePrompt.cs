using Discord.Interactions;
using Discord;

namespace Voltaire.Views.Modals
{
    public class MessagePrompt : IModal
    {
      // note, this title is currently always overwritten
      public string Title => "Sending anonymous message";
      // Strings with the ModalTextInput attribute will automatically become components.
      [InputLabel("Message")]
      [ModalTextInput("message", style: TextInputStyle.Paragraph)]
      public string message { get; set; }
    }
}
