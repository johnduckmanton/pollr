FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Pollr.Api/Pollr.Api.csproj", "Pollr.Api/"]
RUN dotnet restore "Pollr.Api/Pollr.Api.csproj"
COPY . .
WORKDIR "/src/Pollr.Api"
RUN dotnet build "Pollr.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pollr.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Pollr.Api.dll"]
