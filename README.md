# microservice-workshop

Goal: To build and host .NET Core API with edge to edge unit tests using .net core TestServer and host in docker.


# Prerequisites

## Install dotnet

- Chocolatey - `choco install dotnetcore-sdk`

- [Download installer](https://www.microsoft.com/net/download)


## Install docker

- [Download installer](https://www.docker.com/products/docker-desktop)


# Hello world
Run the API
```
dotnet run --project MicroserviceWorkshop
```
The API should start and show you the uris to access it from. Verify the API is running by navigating there in your browser.


# Run with Docker

## Create Dockerfile

To build a docker container to host our app we must create a [Dockerfile](https://docs.docker.com/engine/reference/builder/).

We are going to create a Dockerfile that builds and publishes the artifacts of our app from the dotnet sdk container. We will then use those artifacts in our dotnet core runtime container as our entrypoint.

### Build steps

1. Create a file named `Dockerfile` in the root of the project. Add the following line to load the dotnet sdk container.
```
FROM microsoft/dotnet:2.1-sdk AS build-env
```

2. Copy the source code from our project to the container.
```
COPY ./MicroserviceWorkshop /app/src/
```

3. Build and publish the artefacts.
```
WORKDIR /app/src
RUN dotnet publish -c Release -o ../out
```

### Run steps

1. In the same Dockerfile, load the dotnet runtime container.
```
FROM microsoft/dotnet:2.1-aspnetcore-runtime
```

2. Copy the artefacts from the dotnet sdk container into the `app` directory of the dotnet runtime container.
```
WORKDIR /app
EXPOSE 80

COPY --from=build-env /app/out .
```

3. Run
```
ENTRYPOINT ["dotnet", "MicroserviceWorkshop.dll"]
```

## Build and run the container

Now that we have a Dockerfile we can build and run it.
```
docker build .
docker run -p 5000:80 --name=microservice-workshop <container-id>
```

Test it by navigating to [http://localhost:5000]

Stop the running container
```
docker stop microservice-workshop
```

Remove the container
```
docker rm microservice-workshop
```

# Docker compose

Docker commands can quickly become unweildy and very often we want to launch a bunch of containers that interact with each other together. This is where the `docker-compose` command comes in.

Notice the `docker-compose.yml` file; this file is picked up by the `docker-compose` command. The file contains information about the port number mappings in this case but there are lots more things you can do with it. As ours is a simple case, this is all we need.

Run `docker-compose up` to launch the container.