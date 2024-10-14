FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY NanhiDuniya.Services.AuthAPI.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish NanhiDuniya.Services.AuthAPI.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

COPY ./Templates /app/Templates

ENTRYPOINT [ "dotnet", "NanhiDuniya.Services.AuthAPI.dll" ]