#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReactiveThings.DDNS.csproj", "."]
RUN dotnet restore "./ReactiveThings.DDNS.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ReactiveThings.DDNS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReactiveThings.DDNS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReactiveThings.DDNS.dll"]