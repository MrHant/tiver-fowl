#!/bin/bash
set -e

echo "Installing Ubuntu packages..."

sudo apt-get update

echo " = Packages required for chrome and chromedriver"
sudo apt-get install -y libglib2.0-dev libgbm1 libnspr4 libnss3 libdbus-1-3 libatk1.0 \
    libatk-bridge2.0 libcups2 libxkbcommon0 libxcomposite1 libxdamage1 libxrandr2 libpango-1.0 libasound2t64

echo "Package installation complete!"
