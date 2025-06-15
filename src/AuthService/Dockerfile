FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev

WORKDIR /workspace

COPY src/AuthService/AuthService.csproj src/AuthService/
COPY lib/EventBusRabbitMQ/EventBusRabbitMQ.csproj lib/EventBusRabbitMQ/
COPY lib/EventBus/EventBus.csproj lib/EventBus/

RUN dotnet restore src/AuthService/AuthService.csproj

COPY src/AuthService src/AuthService
COPY lib/EventBusRabbitMQ lib/EventBusRabbitMQ
COPY lib/EventBus lib/EventBus

EXPOSE 5000
WORKDIR /workspace/src/AuthService

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]

FROM dev AS build

WORKDIR /workspace

RUN dotnet publish src/AuthService/AuthService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
CMD ["dotnet", "AuthService.dll"]
