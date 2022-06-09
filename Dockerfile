#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim-arm32v7 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["ReactiveThings.DDNS.csproj", "."]
RUN dotnet restore "./ReactiveThings.DDNS.csproj" -r linux-arm
COPY . .
WORKDIR "/src/."
RUN dotnet publish "ReactiveThings.DDNS.csproj" -c Release -o /app/publish -r linux-arm --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update && apt-get -y install cron
RUN (crontab -l ; echo "* * * * * root echo 'The test cron ran at $(date)' > /proc/1/fd/1 2>/proc/1/fd/2") | crontab

CMD ["cron", "-f"]
#ENTRYPOINT ["dotnet", "ReactiveThings.DDNS.dll"]