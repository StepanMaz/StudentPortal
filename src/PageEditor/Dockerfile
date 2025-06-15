FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev

WORKDIR /workspace

COPY src/PageEditor/PageEditor.csproj src/PageEditor/
COPY lib/CommonComponents/CommonComponents.csproj lib/CommonComponents/
COPY lib/ComponentData/ComponentData.csproj lib/ComponentData/
COPY lib/Services/Services.csproj lib/Services/

RUN dotnet restore src/PageEditor/PageEditor.csproj

COPY src/PageEditor src/PageEditor
COPY lib/CommonComponents lib/CommonComponents
COPY lib/ComponentData lib/ComponentData
COPY lib/Services lib/Services

EXPOSE 5000

WORKDIR /workspace/src/PageEditor

ENV DOTNET_WATCH_RESTART_ON_EXIT=1

CMD ["dotnet", "watch", "run", "--no-hot-reload", "--urls", "http://0.0.0.0:5000"]


FROM dev AS build
RUN dotnet publish src/PageEditor/PageEditor.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
CMD ["dotnet", "PageEditor.dll"]
