#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Mango.Services.PaymentAPI/Mango.Services.PaymentAPI.csproj", "Services/Mango.Services.PaymentAPI/"]
COPY ["PaymentProcessor/PaymentProcessor.csproj", "PaymentProcessor/"]
RUN dotnet restore "Services/Mango.Services.PaymentAPI/Mango.Services.PaymentAPI.csproj"
COPY . .
WORKDIR "/src/Services/Mango.Services.PaymentAPI"
RUN dotnet build "Mango.Services.PaymentAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mango.Services.PaymentAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mango.Services.PaymentAPI.dll"]