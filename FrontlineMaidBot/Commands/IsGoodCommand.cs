﻿using FrontlineMaidBot.Extensions;
using FrontlineMaidBot.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace FrontlineMaidBot.Commands
{
    public class IsGoodCommand : ICommand
    {
        private readonly IDefaultMessages _defaultMessages;
        private readonly IResponseGenerator _generator;
        private readonly IStorage _storage;

        public string CommandName => "/isgood";
        public IEnumerable<string> Aliases => new List<string> { };

        public IsGoodCommand(IStorage storage, IResponseGenerator generator, IDefaultMessages defaultMessages) 
        {
            _storage = storage;
            _generator = generator;
            _defaultMessages = defaultMessages;
        }

        public string CreateResponse(Message message)
        {
            if (message?.Chat == null)
                return null;

            var input = message.GetCommandArgs();

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var dolls = _storage.GetByName(input);
            var count = dolls.Count();

            string msg;
            if (count <= 1)
            {
                return _generator.CreateSummaryMessage(dolls.FirstOrDefault(), _defaultMessages.DontKnow);
            }
            else
            {
                return _generator.CreateSuggestionMessage(dolls, _defaultMessages.DontKnow, _defaultMessages.Suggestion);
            }
        }

    }
}