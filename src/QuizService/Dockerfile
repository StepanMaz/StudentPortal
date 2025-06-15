FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev

WORKDIR /workspace

COPY src/QuizService/QuizService.csproj src/QuizService/
COPY lib/Auth/Auth.csproj lib/Auth/

RUN dotnet restore src/QuizService/QuizService.csproj

COPY src/QuizService src/QuizService
COPY lib/Auth lib/Auth

EXPOSE 5000
WORKDIR /workspace/src/QuizService

ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]

FROM dev AS build
RUN dotnet publish src/QuizService/QuizService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
CMD ["dotnet", "QuizService.dll"]
