#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BlackJack.Players.Api/BlackJack.Players.Api.csproj", "BlackJack.Players.Api/"]
RUN dotnet restore "BlackJack.Players.Api/BlackJack.Players.Api.csproj"
COPY . .
WORKDIR "/src/BlackJack.Players.Api"
RUN dotnet build "BlackJack.Players.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlackJack.Players.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlackJack.Players.Api.dll"]