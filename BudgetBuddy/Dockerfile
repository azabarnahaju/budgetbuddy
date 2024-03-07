#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["BudgetBuddy.csproj", "BudgetBuddy/"]
#RUN dotnet restore "/BudgetBuddy.csproj"
#COPY . .
#WORKDIR "/src/BudgetBuddy"
#RUN dotnet build "BudgetBuddy.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "BudgetBuddy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "BudgetBuddy.dll"]

# Dockerfile 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BudgetBuddy.dll"]

ENV ASPNETCORE_URLS=http://+:80



