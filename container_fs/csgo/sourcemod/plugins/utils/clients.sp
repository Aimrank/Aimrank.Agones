//
// Track connected and disconnected players.
//

public void ClientConnected(int client)
{
    char steamId[CLIENT_ID_LENGTH + 1];
    GetClientSteamId(client, steamId);

    g_clientsConnected.SetValue(steamId, true);

    if (g_gamePaused)
    {
        if (GetConnectedClientsCount() == g_maxClients)
        {
            PrintToChatAll("All players connected. Unpausing game.");
            Unpause();
        }
    }
}

public void ClientDisconnected(int client)
{
    char steamId[CLIENT_ID_LENGTH + 1];
    GetClientSteamId(client, steamId);

    g_clientsConnected.SetValue(steamId, false);

    if (!g_gamePaused)
    {
        Pause();
    }
}

int GetConnectedClientsCount()
{
    int clients = 0;

    for (int i = 1; i <= MaxClients; i++)
    {
        if (IsClientOnList(i))
        {
            clients++;
        }
    }

    return clients;
}

public bool IsClientOnList(int client)
{
    char steamId[CLIENT_ID_LENGTH + 1];
    GetClientSteamId(client, steamId);

    bool isClientOnList;

    g_clientsConnected.GetValue(steamId, isClientOnList);

    return isClientOnList;
}

public bool IsClientWhitelisted(int client)
{
    char steamId[CLIENT_ID_LENGTH + 1];
    char whitelist[WHITELIST_LENGTH];

    GetClientSteamId(client, steamId);
    GetConVarString(g_whitelist, whitelist, WHITELIST_LENGTH);

    return StrContains(whitelist, steamId) != -1;
}

/**
 * Returns the count of connected players
 * 
 * @param clients   Steam ids of connected players.
 */
public int GetConnectedClients(char[][] clients)
{
    int count = 0;

    char whitelist[WHITELIST_LENGTH];

    GetConVarString(g_whitelist, whitelist, WHITELIST_LENGTH);

    for (int i = 0; i < WHITELIST_LENGTH - 1; i += 20)
    {
        char steamId[CLIENT_ID_LENGTH + 1];
        strcopy(steamId, CLIENT_ID_LENGTH + 1, whitelist[i]);

        bool connected;

        g_clientsConnected.GetValue(steamId, connected)

        if (connected)
        {
            strcopy(clients[count++], CLIENT_ID_LENGTH + 1, steamId);
        }
    }

    return count;
}

/**
 * Returns the count of not connected players
 * 
 * @param clients   Steam ids of not connected players.
 */
public int GetDisconnectedClients(char[][] clients)
{
    int count = 0;

    char[] whitelist = new char[WHITELIST_LENGTH];

    GetConVarString(g_whitelist, whitelist, WHITELIST_LENGTH);

    for (int i = 0; i < WHITELIST_LENGTH - 1; i += 20)
    {
        char steamId[CLIENT_ID_LENGTH + 1];
        strcopy(steamId, CLIENT_ID_LENGTH + 1, whitelist[i]);

        bool connected;

        g_clientsConnected.GetValue(steamId, connected);

        if (!connected)
        {
            strcopy(clients[count++], CLIENT_ID_LENGTH + 1, steamId);
        }
    }

    return count;
}