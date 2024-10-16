FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY NanhiDuniya.Services.EmailApi.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish NanhiDuniya.Services.EmailApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

COPY Assets/Logo.png /app/Assets/Logo.png

ENTRYPOINT [ "dotnet", "NanhiDuniya.Services.EmailApi.dll" ]