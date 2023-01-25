FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Client/ToDo.Client.csproj", "Client/"]
COPY ["Shared/ToDo.Shared.csproj", "Shared/"]
RUN dotnet restore "Client/ToDo.Client.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet build "ToDo.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDo.Client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
