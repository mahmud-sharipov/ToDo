#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Web.Server/ToDo.Web.Server.csproj", "Web.Server/"]
COPY ["src/Web.Client/ToDo.Web.Client.csproj", "Web.Client/"]
COPY ["src/Shared/ToDo.Shared.csproj", "Shared/"]
RUN dotnet restore "Web.Server/ToDo.Web.Server.csproj"
COPY . .
WORKDIR "/src/src/Web.Server"
RUN dotnet build "ToDo.Web.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDo.Web.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo.Web.Server.dll"]