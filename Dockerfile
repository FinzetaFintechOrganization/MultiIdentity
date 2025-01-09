FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . /src/
RUN dotnet restore "src/AsyncIdentity.csproj"
COPY . .
WORKDIR "/src/AsyncIdentity"
RUN dotnet publish "AsyncIdentity.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5000
ENTRYPOINT [ "dotnet", "AsyncIdentity.dll" ]
