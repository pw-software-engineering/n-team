FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS publish
COPY . /src
WORKDIR /src
RUN dotnet publish "Client-Module.csproj" -c Release -p:CompileConstants=PRODUCTION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/Properties/launchSettings.json .
ENTRYPOINT ["dotnet", "Client-Module.dll"]