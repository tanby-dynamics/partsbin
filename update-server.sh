#!/bin/zsh
echo Building image
docker build -t partsbin-page .
echo Composing container
docker compose up -d

echo "Complete!"
