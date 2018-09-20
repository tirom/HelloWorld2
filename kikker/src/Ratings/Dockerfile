FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["src/Ratings/Ratings.csproj", "src/Ratings/"]

RUN dotnet restore "src/Ratings/Ratings.csproj"
COPY . .
WORKDIR "/src/src/Ratings"
RUN dotnet build "Ratings.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ratings.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ratings.dll"]