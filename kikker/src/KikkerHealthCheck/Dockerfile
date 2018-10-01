FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/KikkerHealthCheck/Kikker.HealthCheck.csproj src/KikkerHealthCheck/
RUN dotnet restore src/KikkerHealthCheck/Kikker.HealthCheck.csproj
COPY . .
WORKDIR /src/src/KikkerHealthCheck
RUN dotnet build Kikker.HealthCheck.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Kikker.HealthCheck.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kikker.HealthCheck.dll"]
