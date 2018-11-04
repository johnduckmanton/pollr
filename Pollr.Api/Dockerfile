#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Pollr.API/Pollr.Api.csproj", "Pollr.API/"]
RUN dotnet restore "Pollr.API/Pollr.Api.csproj"
COPY . .
WORKDIR "/src/Pollr.API"
RUN dotnet build "Pollr.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Pollr.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Pollr.Api.dll"]