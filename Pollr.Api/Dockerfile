FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY . ./
WORKDIR /app

RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app

ENV ASPNETCORE_SignalR__UseAzureSignalRManagedHub=false
# ENV PollrDatabase='Server=tcp:demodb-jrd.database.windows.net,1433;Initial Catalog=pollr;Persist Security Info=False;User ID=duckmanj;Password=z9uWLk3$R6sqFW;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
ENV PollrDatabase='Server=tcp:jrd-sql-server.database.windows.net,1433;Initial Catalog=jrd-sqlsb;Persist Security Info=False;User ID=sqladmin;Password=N%c84c7yjAw8m8;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Pollr.Api.dll"]