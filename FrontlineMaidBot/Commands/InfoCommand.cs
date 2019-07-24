﻿using FrontlineMaidBot.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace FrontlineMaidBot.Commands
{
    public class InfoCommand : CommandBase<BaseCommandArgs>
    {
        private const string _commandName = "info";
        private const string _default = "I'm sorry. I don't know.";
        private const string _suggestion = "I'm sorry... did you mean someone like:";

        private readonly IStorage _storage;
        private readonly IResponseGenerator _generator;

        public InfoCommand(IStorage storage, IResponseGenerator generator) : base(name: _commandName)
        {
            _storage = storage;
            _generator = generator;
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, BaseCommandArgs args)
        {
            if (update?.Message?.Chat == null)
                return UpdateHandlingResult.Handled;

            var input = base.ParseInput(update);
            var dolls = _storage.GetByName(input.ArgsInput);
            var count = dolls.Count();

            string msg;
            if (count <= 1)
            {
                msg = _generator.CreateInfoMessage(dolls.FirstOrDefault(), _default);
            }
            else
            {
                msg = _generator.CreateSuggestionMessage(dolls, _default, _suggestion);
            }




            await Bot.Client.SendTextMessageAsync
            (
                update.Message.Chat.Id,
                msg,
                replyToMessageId: update.Message.MessageId
            );

            return UpdateHandlingResult.Handled;
        }
    }
}
