# use alpine for smaller footprint
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base

# needed to get alpine to work
RUN apk add --no-cache tzdata
RUN apk add icu-libs
RUN apk add openssl
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# required for keytool to add KC certs
RUN apk add openjdk11
# required for healthcheck
RUN apk add curl

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
# install step to allow SSL cert registration
# add community repo
RUN echo "http://dl-cdn.alpinelinux.org/alpine/edge/community" >> /etc/apk/repositories
RUN apk update

WORKDIR /App
ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
FROM build AS publish
RUN dotnet publish -c Release -o out

# Build runtime image
FROM base AS final
WORKDIR /App
COPY --from=publish /App/out .
ENTRYPOINT ["dotnet", "aspnet_app.dll"]
