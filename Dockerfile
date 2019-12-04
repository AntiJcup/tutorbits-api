FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["sites/api/tutorbits-api.csproj", "sites/api/"]
RUN dotnet restore "sites/api/tutorbits-api.csproj"
COPY . .
WORKDIR "/src/sites/api"
RUN dotnet build "tutorbits-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "tutorbits-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tutorbits-api.dll"]
