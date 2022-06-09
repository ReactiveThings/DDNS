#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0-bullseye-slim-arm32v7
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReactiveThings.DDNS.csproj", "."]
RUN dotnet restore "./ReactiveThings.DDNS.csproj" -r linux-arm
COPY . .
WORKDIR "/src/."
RUN dotnet build "ReactiveThings.DDNS.csproj" -c Release -o /app/build -r linux-arm --no-restore

FROM build AS publish
RUN dotnet publish "ReactiveThings.DDNS.csproj" -c Release -o /app/publish -r linux-arm --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReactiveThings.DDNS.dll"]