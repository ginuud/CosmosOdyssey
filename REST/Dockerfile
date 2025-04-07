FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20 AS build
WORKDIR /src

COPY ["REST.csproj", "./"]
RUN dotnet restore "REST.csproj"

COPY . .

RUN dotnet publish "REST.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.20
WORKDIR /app

COPY --from=build /publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080
ENTRYPOINT ["dotnet", "REST.dll"]