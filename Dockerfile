# Use the official .NET 7.0 ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the csproj and restore dependencies
COPY AIhealthchatbot.csproj ./
RUN dotnet restore

# Copy the rest of the app and build it
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AIhealthchatbot.dll"]
