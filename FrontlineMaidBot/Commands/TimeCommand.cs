﻿using FrontlineMaidBot.Interfaces;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace FrontlineMaidBot.Commands
{
    public class TimeCommand : CommandBase<BaseCommandArgs>
    {
        private const string _commandName = "time";
        private const string _default = "I'm sorry. I can't find anything.";
        private readonly IStorage _storage;
        private readonly IResponseGenerator _generator;

        public TimeCommand(IStorage storage, IResponseGenerator generator) : base(name: _commandName)
        {
            _storage = storage;
            _generator = generator;
        }

        public override async Task<UpdateHandlingResult> HandleCommand(Update update, BaseCommandArgs args)
        {
            if (update == null || update.Message == null || update.Message.Chat == null)
                return UpdateHandlingResult.Handled;

            var input = ParseInput(update);
            var dolls = _storage.GetByTime(input.ArgsInput);
            var message = _generator.CreateTimerMessage(dolls, _default);

            await Bot.Client.SendTextMessageAsync
            (
                update.Message.Chat.Id,
                message,
                Telegram.Bot.Types.Enums.ParseMode.Html,
                replyToMessageId: update.Message.MessageId
            );

            return UpdateHandlingResult.Handled;
        }
    }
}
