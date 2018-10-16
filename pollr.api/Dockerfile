FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 53421
EXPOSE 44372

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["pollr.api/Pollr.Api.csproj", "pollr.api/"]
RUN dotnet restore "pollr.api/Pollr.Api.csproj"
COPY . .
WORKDIR "/src/pollr.api"
RUN dotnet build "Pollr.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pollr.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pollr.Api.dll"]