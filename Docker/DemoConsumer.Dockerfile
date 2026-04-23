FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Demo.Consumer/Demo.Consumer.csproj", "Demo.Consumer/"]
COPY ["../Demo.Contracts/Demo.Contracts.csproj", "Demo.Contracts/"]
RUN dotnet restore "Demo.Consumer/Demo.Consumer.csproj"

COPY . .
WORKDIR "/src/Demo.Consumer"
RUN dotnet build -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Consumer.dll"]