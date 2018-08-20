FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY src/SimpleService_Docker/SimpleService_Docker.csproj src/SimpleService_Docker/
RUN dotnet restore src/SimpleService_Docker/SimpleService_Docker.csproj
COPY . .
WORKDIR /src/src/SimpleService_Docker
RUN dotnet build SimpleService_Docker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SimpleService_Docker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SimpleService_Docker.dll"]
