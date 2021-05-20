#!/bin/bash

cd container_fs/csgo/sourcemod/plugins
rm -rf build
tar -xzf build.tar.gz
chmod +x ./build/sourcemod/scripting/spcomp

./build/sourcemod/scripting/spcomp aimrank.sp
