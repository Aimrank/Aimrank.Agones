#include <sourcemod>
#include <sdktools>
#include <cstrike>
#include <system2>
#include <json>

#include "utils/globals.sp"
#include "utils/stats.sp"
#include "utils/events.sp"
#include "utils/game.sp"
#include "utils/teams.sp"
#include "utils/clients.sp"

public Plugin myinfo =
{
    name = "Aimrank",
    author = "Aimrank",
    description = "Aimrank",
    version = "1.0.0",
    url = ""
};

public void OnPluginStart()
{
    InitializeGlobals();
    InitializeEvents();
}

public void InitializeEvents()
{
    AddCommandListener(Command_JoinTeam, "jointeam");

    HookEvent("cs_win_panel_match", Event_MatchEnd);
    HookEvent("round_start", Event_RoundStart);
    HookEvent("player_death", Event_PlayerDeath);
    HookEvent("player_disconnect", Event_PlayerDisconnect);

    HookUserMessage(GetUserMessageId("VGUIMenu"), OnTeamMenu, true);
}

/* Forwards */

public void OnMapStart()
{
    PublishEvent(CreateIntegrationEvent("map_start", null));
}

public void OnClientPutInServer(int client)
{
    if (IsFakeClient(client))
    {
        return;
    }

    if (!IsClientWhitelisted(client))
    {
        KickClient(client, "You are not whitelisted");
    }

    ClientConnected(client);
}

/* Commands */

public Action Command_JoinTeam(int client, const char[] command, int argc)
{
    if (!client || !IsClientInGame(client) || IsFakeClient(client))
    {
        return Plugin_Continue;
    }

    return Plugin_Handled;
}

/* Events */

public Action Event_MatchEnd(Event event, const char[] name, bool dontBroadcast)
{
    PublishEvent(CreateIntegrationEvent("match_end", GetScoreboard(0)));
}

public Action Event_RoundStart(Event event, const char[] name, bool dontBroadcast)
{
    if (!IsWarmup() && !g_gameStarted)
    {
        if (GetClientCount() != g_maxClients)
        {
            CancelGame();
        }
        else
        {
            g_gameStarted = true;
        }
    }
}

public Action Event_PlayerDeath(Event event, const char[] name, bool dontBroadcast)
{
    if (IsWarmup())
    {
        return Plugin_Continue;
    }

    int attacker = event.GetInt("attacker");
    bool headshot = event.GetBool("headshot");
    int clientId = GetClientOfUserId(attacker);

    for (int i = 1; i <= MaxClients; i++)
    {
        if (IsClientConnected(i) && IsClientInGame(i))
        {
            char steamId[CLIENT_ID_LENGTH + 1];
            int stats[STATS_SIZE];

            GetClientSteamId(i, steamId);
            GetClientStats(steamId, stats)

            stats[STATS_INDEX_KILLS] = GetEntProp(i, Prop_Data, "m_iFrags");
            stats[STATS_INDEX_ASSISTS] = CS_GetClientAssists(i);
            stats[STATS_INDEX_DEATHS] = GetEntProp(i, Prop_Data, "m_iDeaths");
            stats[STATS_INDEX_HS] = stats[STATS_INDEX_HS] + (i == clientId && headshot ? 1 : 0);

            SetClientStats(steamId, stats);
        }
    }

    return Plugin_Continue;
}

public Action Event_PlayerDisconnect(Event event, const char[] name, bool dontBroadcast)
{
    int client = GetClientOfUserId(event.GetInt("userid"));

    if (!IsWarmup() && IsClientOnList(client))
    {
        PrintToChatAll("Match will be paused at the beginning of next round for 1 minute.");
        PrintToChatAll("Waiting for all players to reconnect.");

        ClientDisconnected(client);

        CreateTimer(120.0, OnPauseTimer, client);
    }
}

Action OnPauseTimer(Handle timer, int client)
{
    if (g_gamePaused)
    {
        AbandonGame(client);
    }
}
