FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS publish
COPY . /src
WORKDIR /src
RUN dotnet publish "Server.csproj" -c Release -p:CompileConstants=PRODUCTION -o /app/publish
RUN dotnet tool install dotnet-ef --tool-path /tools
ENV PATH="${PATH}:/tools:/app"
ENV ASPNETCORE_ENVIRONMENT Production
RUN dotnet ef database drop -f
RUN dotnet ef database update

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/Properties/launchSettings.json .
ENTRYPOINT ["dotnet", "run", "Server.dll"]