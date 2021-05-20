#!/bin/bash

docker build -t ghcr.io/aimrank/aimrank-agones:$1 -f Dockerfile .
