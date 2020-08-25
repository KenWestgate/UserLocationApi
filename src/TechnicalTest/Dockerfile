FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TechnicalTest.Api/TechnicalTest.Api.csproj", "TechnicalTest.Api/"]
RUN dotnet restore "TechnicalTest.Api/TechnicalTest.Api.csproj"
COPY . .
WORKDIR "/src/TechnicalTest.Api"
RUN dotnet build "TechnicalTest.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechnicalTest.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechnicalTest.Api.dll"]