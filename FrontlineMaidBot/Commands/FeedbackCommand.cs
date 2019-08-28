﻿using FrontlineMaidBot.Extensions;
using FrontlineMaidBot.Interfaces;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace FrontlineMaidBot.Commands
{
    public class FeedbackCommand : ICommand
    {
        private readonly IDefaultMessages _defaultMessages;
        private readonly IStorage _storage;

        public string CommandName => "/feedback";
        public IEnumerable<string> Aliases => new List<string> { };

        public FeedbackCommand(IStorage storage, IDefaultMessages defaultMessages)
        {
            _defaultMessages = defaultMessages;
            _storage = storage;
        }

        public string CreateResponse(Message message)
        {
            if (message?.Chat == null)
                return null;

            var input = message.GetCommandArgs();
            if (string.IsNullOrEmpty(input))
            {
                return _defaultMessages.EmptyParams;
            }

            _storage.SaveFeedback(message?.Chat?.Username, message?.Chat?.Type.ToString(), input);

            return _defaultMessages.EverythingWentGood;
        }

    }
}
