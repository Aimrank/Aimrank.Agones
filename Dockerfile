# -- Step 1 -- Build web API

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /app

COPY *.sln .
COPY src/Aimrank.Agones.Api/*.csproj ./src/Aimrank.Agones.Api/
COPY src/Aimrank.Agones.Core/*.csproj ./src/Aimrank.Agones.Core/
COPY src/Aimrank.Agones.Infrastructure/*.csproj ./src/Aimrank.Agones.Infrastructure/

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o /app/out

# -- Step 2 -- Create image with web API and CS:GO server

FROM mcr.microsoft.com/dotnet/aspnet:5.0

RUN mkdir -p /home/app

COPY --from=build /app/out/ /home/app/.

ENV STEAM_DIR /home/steam
ENV STEAM_CMD_DIR /home/steam/steamcmd
ENV CSGO_APP_ID 740
ENV CSGO_DIR /home/steam/csgo

ARG STEAM_CMD_URL=https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz

RUN DEBIAN_FRONTEND=noninteractive && apt-get update \
  && apt-get install --no-install-recommends --no-install-suggests -y \
      lib32gcc1 \
      lib32stdc++6 \
      ca-certificates \
      net-tools \
      locales \
      curl \
      wget \
      unzip \
      screen \
      libc6-dev \
  && locale-gen en_US.UTF-8 \
  && mkdir -p ${STEAM_CMD_DIR} \
  && cd ${STEAM_CMD_DIR} \
  && curl -sSL ${STEAM_CMD_URL} | tar -zx -C ${STEAM_CMD_DIR} \
  && mkdir -p ${STEAM_DIR}/.steam/sdk32 \
  && ln -s ${STEAM_CMD_DIR}/linux32/steamclient.so ${STEAM_DIR}/.steam/sdk32/steamclient.so \
  && { \
    echo '@ShutdownOnFailedCommand 1'; \
    echo '@NoPromptForPassword 1'; \
    echo 'login anonymous'; \
    echo 'force_install_dir ${CSGO_DIR}'; \
    echo 'app_update ${CSGO_APP_ID}'; \
    echo 'quit'; \
  } > ${STEAM_DIR}/autoupdate_script.txt \
  && mkdir -p ${CSGO_DIR} \
  && rm -rf /var/lib/apt/lists/*
  
COPY container_fs/csgo/ ${STEAM_DIR}/

# -- Step 3 -- Compile sourcemod plugins

WORKDIR ${STEAM_DIR}/sourcemod/plugins

RUN tar -xzf build.tar.gz \
  && chmod +x ./build/sourcemod/scripting/spcomp \
  && ./build/sourcemod/scripting/spcomp aimrank.sp
  
# -- Step 4 -- Startup

VOLUME ${CSGO_DIR}

WORKDIR /home/app

EXPOSE 27016/udp
EXPOSE 27016/tcp

HEALTHCHECK --interval=30s --timeout=30s --start-period=30s --retries=5 \
  CMD curl -f http://localhost/ || exit 1
  
ENV ASPNETCORE_ENVIRONMENT=Production
  
CMD ["dotnet", "Aimrank.Agones.Api.dll"]
