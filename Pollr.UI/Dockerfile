#FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
FROM microsoft/dotnet:2.1-sdk AS base
WORKDIR /app

# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs
# End setup

EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build

ENV ASPNETCORE_ENVIRONMENT=Production

# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_10.x | bash - && \
    apt-get install -y build-essential nodejs
# End setup

WORKDIR /src
COPY Pollr.UI/Pollr.UI.csproj Pollr.UI/
RUN dotnet restore Pollr.UI/Pollr.UI.csproj
COPY . .
WORKDIR /src/Pollr.UI
ENV ASPNETCORE__ENVIRONMENT=Production
RUN dotnet build Pollr.UI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Pollr.UI.csproj -c Release -o /app

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Pollr.UI.dll", "--environment=Production"]
