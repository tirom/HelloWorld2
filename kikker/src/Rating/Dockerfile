FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY src/Rating/Rating.csproj src/Rating/
RUN dotnet restore src/Rating/Rating.csproj
COPY . .
WORKDIR /src/src/Rating
RUN dotnet build Rating.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Rating.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Rating.dll"]
