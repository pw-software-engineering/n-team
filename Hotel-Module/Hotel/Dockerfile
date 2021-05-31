FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:5000;https://+:5001
ENV Kestrel__Certificates__Default__Path /app/aspnet.pfx
ENV Kestrel__Certificates__Default__Password=ioioioio1234
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS publish
COPY . /src
WORKDIR /src
RUN dotnet publish "Hotel.csproj" -c Release -p:CompileConstants=PRODUCTION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotel.dll"]