//
// Automatically join players to correct teams and prevent team switching.
//

public Action OnTeamMenu(UserMsg msg_id, Protobuf msg, const int[] players, int playersNum, bool reliable, bool init)
{
    char buffer[64];
    PbReadString(msg, "name", buffer, sizeof(buffer));

    if (StrEqual(buffer, "team", true))
    {
        int client = players[0];

        CreateTimer(0.1, OnTeamMenuTimer, client);
    }

    return Plugin_Continue;
}

Action OnTeamMenuTimer(Handle timer, any client)
{
    CS_SwitchTeam(client, GetClientTeamFromMap(client));
}

int GetClientTeamFromMap(int client)
{
    char steamId[CLIENT_ID_LENGTH + 1];
    GetClientSteamId(client, steamId);

    int maxRounds = GetConVarInt(g_maxRounds);

    int team;

    if (g_clientsTeams.GetValue(steamId, team))
    {
        int scoreT = CS_GetTeamScore(CS_TEAM_T);
        int scoreCT = CS_GetTeamScore(CS_TEAM_CT);

        if (scoreT + scoreCT >= maxRounds / 2)
        {
            team = team == CS_TEAM_T ? CS_TEAM_CT : CS_TEAM_T;
        }

        return team;
    }

    return CS_TEAM_T;
}
