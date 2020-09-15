FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["UserLocation.Api/UserLocation.Api.csproj", "UserLocation.Api/"]
RUN dotnet restore "UserLocation.Api/UserLocation.Api.csproj"
COPY . .
WORKDIR "/src/UserLocation.Api"
RUN dotnet build "UserLocation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserLocation.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserLocation.Api.dll"]