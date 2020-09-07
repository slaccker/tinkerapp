FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

# Copy everything else and build
COPY ./publish ./

ENTRYPOINT ["dotnet", "Web.dll"]
