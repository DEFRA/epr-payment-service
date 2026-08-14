FROM defradigital/dotnetcore:dotnet10.0 AS base

USER root
ARG PORT=8080
ENV ASPNETCORE_URLS=http://*:${PORT}
EXPOSE ${PORT}

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

FROM defradigital/dotnetcore-development:dotnet10.0 AS build
USER root

# Install dotnet-ef tool
RUN dotnet tool install dotnet-ef --version 10.0.0 --tool-path "/root/.dotnet/tools"

# Add the tools folder to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /src
COPY ["EPR.Payment.Service/EPR.Payment.Service.csproj", "EPR.Payment.Service/"]
COPY ["EPR.Payment.Service.Common/EPR.Payment.Service.Common.csproj", "EPR.Payment.Service.Common/"]
COPY ["EPR.Payment.Service.Common.Data/EPR.Payment.Service.Common.Data.csproj", "EPR.Payment.Service.Common.Data/"]
COPY ["EPR.Payment.Service.Messaging/EPR.Payment.Service.Messaging.csproj", "EPR.Payment.Service.Messaging/"]
RUN dotnet restore "EPR.Payment.Service/EPR.Payment.Service.csproj"

COPY EPR.Payment.Service/. ./EPR.Payment.Service/.
COPY EPR.Payment.Service.Common/. ./EPR.Payment.Service.Common/.
COPY EPR.Payment.Service.Common.Data/. ./EPR.Payment.Service.Common.Data/.
COPY EPR.Payment.Service.Messaging/. ./EPR.Payment.Service.Messaging/.

WORKDIR "/src/EPR.Payment.Service"
RUN dotnet publish "EPR.Payment.Service.csproj" -c Release /p:AzureBuild=false --no-restore -o /app/publish

FROM base AS final

# Switch to the non-root user
USER dotnet

WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EPR.Payment.Service.dll"]
