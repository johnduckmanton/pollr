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
COPY Pollr.AdminUI/Pollr.AdminUI.csproj Pollr.AdminUI/
RUN dotnet restore Pollr.AdminUI/Pollr.AdminUI.csproj
COPY . .
WORKDIR /src/Pollr.AdminUI
ENV ASPNETCORE__ENVIRONMENT=Production
RUN dotnet build Pollr.AdminUI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Pollr.AdminUI.csproj -c Release -o /app

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Pollr.AdminUI.dll", "--environment=Production"]
