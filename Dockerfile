FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

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
ENV SignalR__UseAzureSignalRManagedHub="false"
ENV PollrDatabase="Server=tcp:demodb-jrd.database.windows.net,1433;Initial Catalog=pollr;Persist Security Info=False;User ID=duckmanj;Password=z9uWLk3$R6sqFW;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
ENTRYPOINT ["dotnet", "Pollr.Api.dll"]
