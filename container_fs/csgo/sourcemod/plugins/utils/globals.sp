#define SERVER_MAX_CLIENTS 10

#define CLIENT_ID_LENGTH 17
#define CLIENT_WHITELIST_ENTRY_LENGTH 19  // {steam_id}:{team}

#define WHITELIST_LENGTH CLIENT_WHITELIST_ENTRY_LENGTH * SERVER_MAX_CLIENTS + (SERVER_MAX_CLIENTS - 1) + 1

#define STATS_SIZE 4
#define STATS_INDEX_KILLS 0
#define STATS_INDEX_ASSISTS 1
#define STATS_INDEX_DEATHS 2
#define STATS_INDEX_HS 3

int g_maxClients;

bool g_gamePaused;
bool g_gameStarted;

StringMap g_scoreboard;
StringMap g_clientsTeams;
StringMap g_clientsConnected;

char g_clients[SERVER_MAX_CLIENTS][CLIENT_ID_LENGTH + 1];

ConVar g_whitelist;
ConVar g_maxRounds;

public void InitializeGlobals()
{
    g_whitelist = CreateConVar("aimrank_whitelist", "0", "SteamID list of whitelisted players.");
    g_maxRounds = FindConVar("mp_maxrounds");

    g_scoreboard = new StringMap();

    g_clientsTeams = new StringMap();
    g_clientsConnected = new StringMap();

    LoadClientsFromWhitelist();
}

public void GetClientSteamId(int client, char[] output)
{
    GetClientAuthId(client, AuthId_SteamID64, output, CLIENT_ID_LENGTH + 1);
}

void LoadClientsFromWhitelist()
{
    int count = 0;

    char whitelist[WHITELIST_LENGTH];

    GetConVarString(g_whitelist, whitelist, WHITELIST_LENGTH);

    for (int i = 0; i < WHITELIST_LENGTH - 1; i += 20)
    {
        if (whitelist[i] == 0)
        {
            break;
        }

        char clientTeam[2];

        strcopy(g_clients[count], CLIENT_ID_LENGTH + 1, whitelist[i]);
        strcopy(clientTeam, 2, whitelist[i + CLIENT_ID_LENGTH + 1]);

        g_clientsTeams.SetValue(g_clients[count], clientTeam[0] == '2' ? CS_TEAM_T : CS_TEAM_CT);
        g_clientsConnected.SetValue(g_clients[count], false);

        count++;
    }

    g_maxClients = count;
}