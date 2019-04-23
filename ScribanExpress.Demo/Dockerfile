FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ScribanExpress.Demo/ScribanExpress.Demo.csproj", "ScribanExpress.Demo/"]
RUN dotnet restore "ScribanExpress.Demo/ScribanExpress.Demo.csproj"
COPY . .
WORKDIR "/src/ScribanExpress.Demo"
RUN dotnet build "ScribanExpress.Demo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ScribanExpress.Demo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ScribanExpress.Demo.dll"]