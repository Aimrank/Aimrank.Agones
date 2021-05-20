#!/bin/bash

echo $CR_PAT | docker login ghcr.io -u $CR_USER --password-stdin

docker push ghcr.io/aimrank/aimrank-agones:$1
