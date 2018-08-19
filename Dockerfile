FROM microsoft/dotnet:2.1-sdk AS build-env

COPY ./MicroserviceWorkshop /app/src/

WORKDIR /app/src
RUN dotnet publish -c Release -o ../out


FROM microsoft/dotnet:2.1-aspnetcore-runtime

WORKDIR /app
EXPOSE 80

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MicroserviceWorkshop.dll"]