FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev

WORKDIR /workspace

COPY src/PageStorage/PageStorage.csproj src/PageStorage/
COPY lib/ComponentData/ComponentData.csproj lib/ComponentData/
COPY lib/Auth/Auth.csproj lib/Auth/

RUN dotnet restore src/PageStorage/PageStorage.csproj

COPY src/PageStorage src/PageStorage
COPY lib/ComponentData lib/ComponentData
COPY lib/Auth lib/Auth

EXPOSE 5000
WORKDIR /workspace/src/PageStorage

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]

FROM dev AS build

WORKDIR /workspace

RUN dotnet publish src/PageStorage/PageStorage.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
CMD ["dotnet", "PageStorage.dll"]
