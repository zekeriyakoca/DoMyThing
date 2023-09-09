#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated7.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

RUN apt-get update && apt-get install -y xorg openbox libnss3 libasound2
RUN apt-get clean
RUN rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DoMyThing.Functions/DoMyThing.Functions.csproj", "DoMyThing.Functions/"]
COPY ["DoMyThing.Common/DoMyThing.Common.csproj", "DoMyThing.Common/"]
RUN dotnet restore "DoMyThing.Functions/DoMyThing.Functions.csproj"
COPY . .
WORKDIR "/src/DoMyThing.Functions"
RUN dotnet build "DoMyThing.Functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DoMyThing.Functions.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true