#!/bin/bash

VERSION_SOURCEMOD=1.10.0-git6478-linux

mkdir -p $CSGO_DIR/csgo/addons

function install_sourcemod {
  if [[ ! -d "$CSGO_DIR/csgo/addons/sourcemod" ]]
  then
    wget https://sm.alliedmods.net/smdrop/1.10/sourcemod-$VERSION_SOURCEMOD.tar.gz
    tar -xzf sourcemod-$VERSION_SOURCEMOD.tar.gz -C $CSGO_DIR/csgo
    rm sourcemod-$VERSION_SOURCEMOD.tar.gz
  fi
}

install_sourcemod

cp -a $STEAM_DIR/sourcemod/plugins/*.smx $CSGO_DIR/csgo/addons/sourcemod/plugins/
cp -a $STEAM_DIR/sourcemod/configs/. $CSGO_DIR/csgo/addons/sourcemod/configs/
cp -a $STEAM_DIR/sourcemod/extensions/. $CSGO_DIR/csgo/addons/sourcemod/extensions/
