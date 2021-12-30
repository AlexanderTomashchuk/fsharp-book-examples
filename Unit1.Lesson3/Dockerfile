FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["demo1/demo1.fsproj", "demo1/"]
RUN dotnet restore "demo1/demo1.fsproj"
COPY . .
WORKDIR "/src/demo1"
RUN dotnet build "demo1.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "demo1.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "demo1.dll"]
