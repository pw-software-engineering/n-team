FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS publish
COPY . /src
WORKDIR /src
RUN dotnet publish "Hotel.csproj" -c Release -p:CompileConstants=PRODUCTION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotel.dll"]
