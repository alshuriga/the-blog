FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY ./Blog.Application/*.csproj ./Blog.Application/
COPY ./Blog.Core/*.csproj ./Blog.Core/
COPY ./Blog.Infrastructure/*.csproj ./Blog.Infrastructure/
COPY ./Blog.WebAPI/*.csproj ./Blog.WebAPI/
RUN dotnet restore Blog.WebAPI

COPY . .
RUN dotnet publish Blog.WebAPI -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR Blog.WebAPI
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS=http://+:8000 
ENV ASPNETCORE_ENVIRONMENT=PRODUCTION
ENTRYPOINT ["dotnet", "Blog.WebAPI.dll"]
