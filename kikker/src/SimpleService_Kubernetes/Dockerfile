FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY src/SimpleService_Kubernetes/SimpleService_Kubernetes.csproj src/SimpleService_Kubernetes/
RUN dotnet restore src/SimpleService_Kubernetes/SimpleService_Kubernetes.csproj
COPY . .
WORKDIR /src/src/SimpleService_Kubernetes
RUN dotnet build SimpleService_Kubernetes.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SimpleService_Kubernetes.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SimpleService_Kubernetes.dll"]
