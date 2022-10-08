FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY Blog.Application/*.csproj ./Blog.Application/
COPY Blog.Core/*.csproj ./Blog.Core/
COPY Blog.Infrastructure/*.csproj ./Blog.Infrastructure/
COPY Blog.MVC/*.csproj ./Blog.MVC/
RUN dotnet restore Blog.MVC

COPY . .
RUN dotnet publish Blog.MVC -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR Blog.MVC
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Blog.MVC.dll"]