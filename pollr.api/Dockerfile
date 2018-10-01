FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 53420
EXPOSE 44371

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["pollr.api/pollr.api.csproj", "pollr.api/"]
RUN dotnet restore "pollr.api/pollr.api.csproj"
COPY . .
WORKDIR "/src/pollr.api"
RUN dotnet build "pollr.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "pollr.api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "pollr.api.dll"]