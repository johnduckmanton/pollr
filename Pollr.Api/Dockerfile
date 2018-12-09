FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

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
ENV ASPNETCORE_SignalR__UseAzureSignalRManagedHub=false
ENV PollrDatabase='Server=tcp:demodb-jrd.database.windows.net,1433;Initial Catalog=pollr;Persist Security Info=False;User ID=duckmanj;Password=z9uWLk3$R6sqFW;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
ENV ASPNETCORE_ENVIRONMENT=Docker
COPY --from=publish /app .

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "Pollr.Api.dll"]