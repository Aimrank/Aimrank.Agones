# Aimrank.Agones

![Docker build status](https://github.com/Aimrank/Aimrank.Agones/workflows/Master%20build/badge.svg)

Single CS:GO server instance wrapped with web API project.

## Environment variables

|Name                 |Default value|
|---------------------|-------------|
|SERVER_HOSTNAME      |Counter-Strike: Global Offensive Dedicated Server|
|SERVER_PASSWORD      ||
|RCON_PASSWORD        |changeme|

## What it does

1. Install CS:GO server on Linux
2. Install Metamod
3. Install Sourcemod
4. Copy server configuration files
5. Start web server that is used to manage CS:GO servers
   
## Important

When starting CS:GO server for the first time it has to download all necessary data (~28GB). This might take a while depending on
your connection. It's saved under /home/steam/csgo and it's persisted inside `container_data` directory.

## References

Some links that might come handy for developers working on project.

1. CS:GO Server

    - [Metamod](https://wiki.alliedmods.net/Category:Metamod:Source_Documentation)
    - [Sourcemod](https://wiki.alliedmods.net/Category:SourceMod_Documentation)
        - [Events](https://wiki.alliedmods.net/Counter-Strike:_Global_Offensive_Events)

2. Some repositories used as reference:

    - [csgobash](https://github.com/jpcanoso/csgobash)

3. Sourcemod

   ```bash
   sm_dump_datamaps datamaps.txt
   sm_dump_netprops netprops.txt
   ```

   Plugins used:

    - system2
    - sm-json
