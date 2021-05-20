#!/bin/bash

VERSION_METAMOD=1.10.7-git971-linux

mkdir -p $CSGO_DIR/csgo/addons

function install_metamod {
  if [[ ! -d "$CSGO_DIR/csgo/addons/metamod" ]]
  then
    wget https://mms.alliedmods.net/mmsdrop/1.10/mmsource-$VERSION_METAMOD.tar.gz
    tar -xzf mmsource-$VERSION_METAMOD.tar.gz -C $CSGO_DIR/csgo
    rm mmsource-$VERSION_METAMOD.tar.gz
  fi
}

install_metamod

mkdir -p $CSGO_DIR/csgo/addons/metamod

cp $STEAM_DIR/metamod/metaplugins.ini $CSGO_DIR/csgo/addons/metamod/metaplugins.ini
