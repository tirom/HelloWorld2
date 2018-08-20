FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 7543
EXPOSE 44395

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/WebTest/WebTest.csproj src/WebTest/
RUN dotnet restore src/WebTest/WebTest.csproj
COPY . .
WORKDIR /src/src/WebTest
RUN dotnet build WebTest.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebTest.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebTest.dll"]
