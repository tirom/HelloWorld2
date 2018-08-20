FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 13683
EXPOSE 44326

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/WebApiTest/WebApiTest.csproj src/WebApiTest/
RUN dotnet restore src/WebApiTest/WebApiTest.csproj
COPY . .
WORKDIR /src/src/WebApiTest
RUN dotnet build WebApiTest.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebApiTest.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApiTest.dll"]
