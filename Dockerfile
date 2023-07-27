FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MassTransitPoc.csproj", "MassTransitPoc/"]
RUN dotnet restore "MassTransitPoc/MassTransitPoc.csproj"

COPY . MassTransitPoc/.
RUN dotnet publish -c Release -o /app MassTransitPoc/MassTransitPoc.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MassTransitPoc.dll"]