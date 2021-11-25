#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/hsa-lab7/hsa-lab7.csproj", "src/hsa-lab7/"]
RUN dotnet restore "src/hsa-lab7/hsa-lab7.csproj"
COPY . .
WORKDIR "/src/src/hsa-lab7"
RUN dotnet build "hsa-lab7.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "hsa-lab7.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish/ .
ENTRYPOINT ["dotnet", "hsa-lab7.dll"]