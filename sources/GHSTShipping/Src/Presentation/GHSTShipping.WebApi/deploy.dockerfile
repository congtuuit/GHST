# Use the official .NET Core runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory in the container
WORKDIR /app

# Copy the published files from the host to the container
COPY . .

# Expose the port that the app runs on
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "GHSTShipping.WebApi.dll"]
