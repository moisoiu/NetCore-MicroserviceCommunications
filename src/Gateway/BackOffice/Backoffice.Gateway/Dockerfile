#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Backoffice.Gateway.csproj", ""]
COPY ["../../../Shared/DTO/DTO.csproj", "../../../Shared/DTO/"]
COPY ["../../../Shared/Infrastructure/Infrastructure.csproj", "../../../Shared/Infrastructure/"]
COPY ["../../../Shared/CommunicationConfig/CommunicationConfig.csproj", "../../../Shared/CommunicationConfig/"]
RUN dotnet restore "./Backoffice.Gateway.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Backoffice.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backoffice.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backoffice.Gateway.dll"]