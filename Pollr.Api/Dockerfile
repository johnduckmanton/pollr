FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 53421
EXPOSE 44372

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore

COPY . ./
WORKDIR "/src"
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
ENV SignalR__UseAzureSignalRManagedHub="false"
ENV SQLAZURECONNSTR__PollrDatabase=Server="tcp:demodb-jrd.database.windows.net,1433;Initial Catalog=pollr;Persist Security Info=False;User ID=duckmanj;Password=z9uWLk3$R6sqFW;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pollr.Api.dll"]