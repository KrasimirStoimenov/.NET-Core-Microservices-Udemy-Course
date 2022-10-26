#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Mango.Services.OrderAPI/Mango.Services.OrderAPI.csproj", "Services/Mango.Services.OrderAPI/"]
RUN dotnet restore "Services/Mango.Services.OrderAPI/Mango.Services.OrderAPI.csproj"
COPY . .
WORKDIR "/src/Services/Mango.Services.OrderAPI"
RUN dotnet build "Mango.Services.OrderAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mango.Services.OrderAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mango.Services.OrderAPI.dll"]