FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100 AS build
WORKDIR /src
EXPOSE 80

# Install deps
COPY ChallengePad.csproj .
RUN dotnet restore "ChallengePad.csproj"

# Build ChallengePad
COPY . .
RUN dotnet publish "ChallengePad.csproj" -c Release -o /app

# Create runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet AS publish
WORKDIR /app
COPY --from=build /app .

# go!
ENTRYPOINT ["dotnet", "ChallengePad.dll"]
#ENTRYPOINT ["sleep", "100000"]
