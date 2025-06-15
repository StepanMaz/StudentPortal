FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev

WORKDIR /workspace

COPY src/PageView/PageView.csproj src/PageView/
COPY lib/CommonComponents/CommonComponents.csproj lib/CommonComponents/
COPY lib/ComponentData/ComponentData.csproj lib/ComponentData/
COPY lib/Services/Services.csproj lib/Services/
COPY lib/Auth/Auth.csproj lib/Auth/

RUN dotnet restore src/PageView/PageView.csproj

COPY src/PageView src/PageView
COPY lib/CommonComponents lib/CommonComponents
COPY lib/ComponentData lib/ComponentData
COPY lib/Services lib/Services
COPY lib/Auth lib/Auth

EXPOSE 5000

WORKDIR /workspace/src/PageView

ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]

FROM dev AS build

RUN dotnet publish src/PageView/PageView.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000

ENV ASPNETCORE_URLS=http://0.0.0.0:5000

CMD ["dotnet", "PageView.dll"]