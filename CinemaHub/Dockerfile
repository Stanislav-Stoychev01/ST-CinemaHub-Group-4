#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
RUN apk update && apk add fontconfig
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
EXPOSE 443
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/runtime:2.2
RUN apt-get update
RUN apt-get install -y libfreetype6
RUN apt-get install -y libfontconfig1

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["CinemaHub.csproj", "."]
RUN dotnet restore "./CinemaHub.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CinemaHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CinemaHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CinemaHub.dll"]