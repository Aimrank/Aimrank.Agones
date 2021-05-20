#!/bin/bash

SERVER_STEAM_TOKEN="$1"

export SERVER_HOSTNAME="${SERVER_HOSTNAME:-Counter-Strike: Global Offensive Dedicated Server}"
export SERVER_PASSWORD="${SERVER_PASSWORD:-}"
export SERVER_ADMIN_STEAMID="${SERVER_ADMIN_STEAMID:-}"
export RCON_PASSWORD="${RCON_PASSWORD:-changeme}"

# Attempt to update CSGO before starting server

[[ -z ${CI+x} ]] && "$STEAM_CMD_DIR/steamcmd.sh" +login anonymous +force_install_dir "$CSGO_DIR" +app_update "$CSGO_APP_ID" +quit

# Create autoexec config

cat << AUTOEXECCFG > "$CSGO_DIR/csgo/cfg/autoexec.cfg"
log on
hostname "$SERVER_HOSTNAME"
rcon_password "$RCON_PASSWORD"
sv_password "$SERVER_PASSWORD"
sv_cheats 0
sv_lan 0
sv_allowupload 1
sv_allowdownload 1
exec banned_user.cfg
exec banned_ip.cfg
sv_hibernate_when_empty 0
sv_hibernate_postgame_delay 0
sv_hibernate_ms 0
sv_hibernate_ms_vgui 0
AUTOEXECCFG

# Create server config

cat << SERVERCFG > "$CSGO_DIR/csgo/cfg/server.cfg"
writeid
writeip
SERVERCFG

# Copy predefined config files

cp -a $STEAM_DIR/cfg/. $CSGO_DIR/csgo/cfg/
cp -a $STEAM_DIR/maps/. $CSGO_DIR/csgo/maps/

cp $STEAM_DIR/pure_server_whitelist.txt $CSGO_DIR/csgo/pure_server_whitelist.txt

"$BASH" "$STEAM_DIR/configure_metamod.sh"
"$BASH" "$STEAM_DIR/configure_sourcemod.sh"

SRCDS_ARGUMENTS=(
  "-console"
  "-usercon"
  "-game csgo"
  "-autoupdate"
  "-steam_dir $STEAM_CMD_DIR"
  "-steamcmd_script $STEAM_DIR/autoupdate_script.txt"
  "-tickrate 128"
  "-port 27016"
  "-net_port_try 1"
  "-ip 0.0.0.0"
  "-nohltv"
  "+game_type 0"
  "+game_mode 1"
  "+map aim_map"
  "+sv_setsteamaccount" "$SERVER_STEAM_TOKEN"
  "+sv_lan 0"
)

SRCDS_RUN="$CSGO_DIR/srcds_run"

# Patch srcds_run to fix autoupdates

if grep -q 'steam.sh' "$SRCDS_RUN"; then
  sed -i 's/steam.sh/steamcmd.sh/' "$SRCDS_RUN"
  echo "Applied patch to srcds_run to fix autoupdates"
fi

# Start the server

screen -dmS csgo $SRCDS_RUN ${SRCDS_ARGUMENTS[@]}
