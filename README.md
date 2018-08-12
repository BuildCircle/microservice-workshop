# microservice-workshop

Goal: To build and host .NET Core API from scratch with supporting tests.

# 1. Install dotnet

Chocolatey
```
choco install dotnetcore-sdk
```

or

[Download installer](https://www.microsoft.com/net/download)

# 2. Create web project

To see available templates
```
dotnet new
```

Create a new empty web project
```
dotnet new web
```

Run it
```
dotnet run
```

Test it
```
http://localhost:5000
```