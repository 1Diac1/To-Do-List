FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/To-Do-List.API/To-Do-List.API.csproj", "src/To-Do-List.API/"]
COPY ["src/To-Do-List.Application/To-Do-List.Application.csproj", "src/To-Do-List.Application/"]
COPY ["src/To-Do-List.Domain/To-Do-List.Domain.csproj", "src/To-Do-List.Domain/"]
COPY ["src/To-Do-List.Infrastructure/To-Do-List.Infrastructure.csproj", "src/To-Do-List.Infrastructure/"]
RUN dotnet restore "src/To-Do-List.API/To-Do-List.API.csproj"
COPY . .
WORKDIR "/src/src/To-Do-List.API"
RUN dotnet build "To-Do-List.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "To-Do-List.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /src/To-Do-List.API
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "To-Do-List.API.dll"]
