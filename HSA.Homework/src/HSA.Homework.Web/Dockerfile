FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/HSA.Homework.Web/HSA.Homework.Web.csproj", "src/HSA.Homework.Web/"]
RUN dotnet restore "src/HSA.Homework.Web/HSA.Homework.Web.csproj"
COPY . .
WORKDIR "/src/src/HSA.Homework.Web"
RUN dotnet build "HSA.Homework.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HSA.Homework.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HSA.Homework.Web.dll"]