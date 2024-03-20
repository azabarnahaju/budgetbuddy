FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BudgetBuddy/BudgetBuddy.csproj", "BudgetBuddy/"]
RUN dotnet restore "BudgetBuddy/BudgetBuddy.csproj"
COPY . .
WORKDIR "/src/BudgetBuddy"
RUN dotnet build "BudgetBuddy.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BudgetBuddy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetBuddy.dll"]

ENV ASPNETCORE_URLS=http://+:80



