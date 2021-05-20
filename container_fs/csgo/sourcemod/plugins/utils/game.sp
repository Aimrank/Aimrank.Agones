public void Pause()
{
    g_gamePaused = true;
    ServerCommand("mp_pause_match");
}

public void Unpause()
{
    g_gamePaused = false;
    ServerCommand("mp_unpause_match");
}

public bool IsWarmup()
{
    return GameRules_GetProp("m_bWarmupPeriod") == 1;
}

public void CancelGame()
{
    PrintToChatAll("Match canceled. Some players failed to connect.");
    PublishEvent(CreateIntegrationEvent("match_cancel", null));
}

public void AbandonGame(int client)
{
    int countT = GetTeamClientCount(CS_TEAM_T);
    int countCT = GetTeamClientCount(CS_TEAM_CT);

    if (countT > 0 && countCT > 0)
    {
        PrintToChatAll("Player failed to reconnect within the specified time. He will be issued a cooldown.");

        char steamId[CLIENT_ID_LENGTH + 1];
        GetClientSteamId(client, steamId);

        JSON_Object data = new JSON_Object();
        data.SetString("steamId", steamId);

        PublishEvent(CreateIntegrationEvent("player_disconnect", data));

        Unpause();
    }
    else
    {
        int loser = GetClientTeamFromMap(client);
        int winner = loser == CS_TEAM_T ? CS_TEAM_CT : CS_TEAM_T;

        PrintToServer("[Aimrank] All enemies disconnected and failed to reconnect. The game is abandoned.");

        PublishEvent(CreateIntegrationEvent("match_end", GetScoreboard(winner)));
    }
}