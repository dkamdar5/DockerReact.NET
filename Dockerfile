FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

COPY ["react-app/react-app.csproj", "react-app/"]
RUN dotnet restore "react-app/react-app.csproj"
COPY . .
WORKDIR "/src/react-app"
RUN dotnet build "react-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "react-app.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "react-app.dll"]