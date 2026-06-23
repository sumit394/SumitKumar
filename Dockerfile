# Use the official .NET SDK image for build
FROM https://chainguard.artifactory.danskenet.net/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/SumitKumar.Web/*.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . ./

# Publish the application
RUN dotnet publish src/SumitKumar.Web -c Release -o out

# Build runtime image
FROM https://chainguard.artifactory.danskenet.net/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose ports
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS="http://+:80;https://+:443"
ENV ASPNETCORE_ENVIRONMENT=Development

# Start the application
ENTRYPOINT ["dotnet", "SumitKumar.Web.dll"]
