FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY LegacyVault.csproj ./
RUN dotnet restore LegacyVault.csproj
COPY . ./
RUN dotnet publish LegacyVault.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=10000
COPY --from=build /app/publish ./
EXPOSE 10000
ENTRYPOINT ["dotnet", "LegacyVault.dll"]