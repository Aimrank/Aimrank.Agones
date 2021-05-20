//
// Track players stats
//

public void GetClientStats(char[] steamId, int[] stats)
{
    if (!g_scoreboard.GetArray(steamId, stats, STATS_SIZE))
    {
        ClearClientStats(steamId);
        GetClientStats(steamId, stats);
    }
}

public void SetClientStats(char[] steamId, int[] stats)
{
    g_scoreboard.SetArray(steamId, stats, STATS_SIZE, true);
}

public void ClearClientStats(char[] steamId)
{
    int stats[STATS_SIZE];
    g_scoreboard.SetArray(steamId, stats, STATS_SIZE)
}

// 0 - get winner from scores
// 1 - draw
// 2 - t
// 3 - ct
public JSON_Object GetScoreboard(int winner)
{
    JSON_Array clientsT = new JSON_Array();
    JSON_Array clientsCt = new JSON_Array();

    for (int i = 0; i < g_maxClients; i++)
    {
        JSON_Object data = new JSON_Object();

        int stats[STATS_SIZE];

        GetClientStats(g_clients[i], stats);

        data.SetString("steamId", g_clients[i]);
        data.SetInt("kills", stats[STATS_INDEX_KILLS]);
        data.SetInt("assists", stats[STATS_INDEX_ASSISTS]);
        data.SetInt("deaths", stats[STATS_INDEX_DEATHS]);
        data.SetInt("hs", stats[STATS_INDEX_HS]);

        int team;

        g_clientsTeams.GetValue(g_clients[i], team);

        if (team == CS_TEAM_T)
        {
            clientsT.PushObject(data);
        }
        else if (team == CS_TEAM_CT)
        {
            clientsCt.PushObject(data);
        }
    }

    JSON_Object teamT = new JSON_Object();
    JSON_Object teamCt = new JSON_Object();

    int maxRounds = GetConVarInt(g_maxRounds);

    int scoreT = GetTeamScore(CS_TEAM_T);
    int scoreCT = GetTeamScore(CS_TEAM_CT);

    bool isSecondHalf = scoreT + scoreCT >= maxRounds / 2;

    if (isSecondHalf)
    {
        int temp = scoreT;
        scoreT = scoreCT;
        scoreCT = temp;
    }

    teamT.SetInt("score", scoreT);
    teamT.SetObject("clients", clientsT);

    teamCt.SetInt("score", scoreCT);
    teamCt.SetObject("clients", clientsCt);

    JSON_Object scores = new JSON_Object();
    scores.SetObject("teamTerrorists", teamT);
    scores.SetObject("teamCounterTerrorists", teamCt);

    if (winner > 0)
    {
        scores.SetInt("winner", winner);
    }
    else
    {
        scores.SetInt("winner", scoreT > scoreCT ? CS_TEAM_T : (scoreCT > scoreT ? CS_TEAM_CT : 1));
    }
    
    return scores;
}