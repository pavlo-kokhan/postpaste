FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Post/Post.Api/Post.Api.csproj", "Services/Post/Post.Api/"]
COPY ["Post.Domain/Post.Domain.csproj", "Post.Domain/"]
COPY ["Shared/Shared.Result/Shared.Result.csproj", "Shared/Shared.Result/"]
COPY ["Post.Infrastructure/Post.Infrastructure.csproj", "Post.Infrastructure/"]
RUN dotnet restore "Services/Post/Post.Api/Post.Api.csproj"
COPY . .
WORKDIR "/src/Services/Post/Post.Api"
RUN dotnet build "./Post.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Post.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Post.Api.dll"]
