FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Pollr.Api.csproj", "./"]
RUN dotnet restore "./Pollr.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Pollr.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pollr.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pollr.Api.dll"]
