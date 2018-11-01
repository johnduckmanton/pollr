FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 63604
EXPOSE 44399

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Pollr.AdminUI/Pollr.AdminUI.csproj", "Pollr.AdminUI/"]
RUN dotnet restore "Pollr.AdminUI/Pollr.AdminUI.csproj"
COPY . .
WORKDIR "/src/Pollr.AdminUI"
RUN dotnet build "Pollr.AdminUI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pollr.AdminUI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pollr.AdminUI.dll"]