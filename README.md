# .Net-Microservices

This Repository Contains Two Microservices, 
1. Platform Service
   Maintains List Of Platforms Like dotnet, docker etc
2. Commands Service
   Maintains commands used by various platforms.

Project is developed using .Net 5 and both services make use of EntityFramework InMemory, Sql Server Database, AutoMappers etc.
Also, For deployment purpose docker and kubernetes is used.
You can find docker images at 
https://hub.docker.com/repository/docker/9689599462/platformservice
https://hub.docker.com/repository/docker/9689599462/commandservice
