FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PaySlipCalculator.Web/PaySlipCalculator.Web.csproj", "PaySlipCalculator.Web/"]
RUN dotnet restore "PaySlipCalculator.Web/PaySlipCalculator.Web.csproj"
COPY . .
WORKDIR "/src/PaySlipCalculator.Web"
RUN dotnet build "PaySlipCalculator.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PaySlipCalculator.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaySlipCalculator.Web.dll"]