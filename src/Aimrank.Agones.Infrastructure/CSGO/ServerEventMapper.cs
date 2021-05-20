using Aimrank.Agones.Core.Commands.CSGO.CancelMatch;
using Aimrank.Agones.Core.Commands.CSGO.FinishMatch;
using Aimrank.Agones.Core.Commands.CSGO.PlayerDisconnected;
using Aimrank.Agones.Core.Commands.CSGO.ServerStarted;
using MediatR;
using System.Collections.Generic;
using System.Text.Json;
using System;

namespace Aimrank.Agones.Infrastructure.CSGO
{
    internal class ServerEventMapper : IServerEventMapper
    {
        private readonly Dictionary<string, Type> _commands = new()
        {
            ["map_start"] = typeof(ServerStartedCommand),
            ["match_end"] = typeof(FinishMatchCommand),
            ["match_cancel"] = typeof(CancelMatchCommand),
            ["player_disconnect"] = typeof(PlayerDisconnectCommand)
        };

        public IRequest Map(string name, dynamic data)
        {
            var settings = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            var commandType = _commands.GetValueOrDefault(name);
            if (commandType is null)
            {
                return null;
            }

            var content = data is null ? "{}" : JsonSerializer.Serialize<dynamic>(data, settings) as string;
            var command = (IRequest) JsonSerializer.Deserialize(content, commandType, settings);

            return command;
        }
    }
}