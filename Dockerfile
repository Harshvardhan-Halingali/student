# Multi-stage Dockerfile for .NET 8 ASP.NET Core MVC
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore project dependencies
COPY ["StudentAttendancePerformanceSystem/StudentAttendancePerformanceSystem.csproj", "StudentAttendancePerformanceSystem/"]
RUN dotnet restore "StudentAttendancePerformanceSystem/StudentAttendancePerformanceSystem.csproj"

# Copy remaining code and publish
COPY . .
WORKDIR "/src/StudentAttendancePerformanceSystem"
RUN dotnet publish "StudentAttendancePerformanceSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StudentAttendancePerformanceSystem.dll"]
