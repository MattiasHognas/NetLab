# Project tools
dotnet tool install --global dotnet-ef --version 5.0.4 \
    && echo 'export PATH="$PATH:/root/.dotnet/tools"' >> ~/.bashrc

dotnet tool install --global dotnet-outdated-tool --version 3.2.0 \
    && echo 'export PATH="$PATH:/root/.dotnet/tools"' >> ~/.bashrc

# Git settings
git config --global user.email "mattias.hognas@gmail.com"
git config --global user.name "Mattias Högnäs"

# # run npm
# npm --global install npm
# npm install --global npm-check-updates
