#!/bin/zsh
docker build -t partsbin-home .
docker compose up -d

echo "Complete!"