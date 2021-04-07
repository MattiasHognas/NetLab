# Base image used by Visual Studio at development time
# (See https://docs.microsoft.com/en-us/visualstudio/containers/container-msbuild-properties)
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
# # Open Container Initiative (OCI) labels (See https://github.com/opencontainers/image-spec/blob/master/annotations.md).
LABEL org.opencontainers.image.title="Content Service" \
    org.opencontainers.image.description="Content Service" \
    org.opencontainers.image.documentation="https://github.com/MattiasHognas/NetLab" \
    org.opencontainers.image.source="https://github.com/MattiasHognas/NetLab.git" \
    org.opencontainers.image.url="https://github.com/MattiasHognas/NetLab" \
    org.opencontainers.image.vendor="Mattias Högnäs"
# Disable the culture invariant mode which defaults to true in the base alpine image
# (See https://github.com/dotnet/corefx/blob/8245ee1e8f6063ccc7a3a60cafe821d29e85b02f/Documentation/architecture/globalization-invariant-mode.md)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache \
    # Install cultures to enable use of System.CultureInfo
    icu-libs \
    # Install time zone database to enable use of System.TimeZoneInfo
    tzdata
# Set the default locale and language.
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8
WORKDIR /app
EXPOSE 80
# To use the debug build configuration pass --build-arg Configuration=Debug
ARG Configuration=Release

RUN if [ "${Configuration}" = "Debug" ]; then \
        apt-get update && \
        apt-get install -y --no-install-recommends apt-utils && \
        apt-get install curl unzip procps mongodb -y && \
        curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /publish/vsdbg; \
     else \
        echo "*Whistling*"; \
    fi

# SDK image used to build and publish the application
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS sdk
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
WORKDIR /src
COPY "Service/Content.Service/*.csproj" "./Service/Content.Service/"
COPY "Test/Content.Service.IntegrationTest/*.csproj" "./Test/Content.Service.IntegrationTest/"
RUN dotnet restore "Service/Content.Service"
RUN dotnet restore "Test/Content.Service.IntegrationTest"
COPY . .
# To view the files that have been copied into the container file system for debugging purposes uncomment this line
# RUN apk add --no-cache tree && tree
RUN dotnet build --configuration $Configuration --no-restore "Service/Content.Service"
RUN dotnet build --configuration $Configuration --no-restore "Test/Content.Service.IntegrationTest"
RUN dotnet test --configuration $Configuration --no-build "Test/Content.Service.IntegrationTest"
RUN dotnet publish --configuration $Configuration --no-build "Service/Content.Service" --output /app

# Runtime image used to run the application
FROM base AS runtime
WORKDIR /app
COPY --from=sdk /app .
ENTRYPOINT ["dotnet", "Content.Service.dll"]
