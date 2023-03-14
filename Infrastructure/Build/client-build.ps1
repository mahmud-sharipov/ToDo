Param(
    [string]$v
    )

docker build -f "C:\Repos\ToDo\src\Web.Server\Dockerfile" --force-rm -t todoclient:$v --target final "C:\Repos\Todo\src" 
docker image tag todoclient:$v mahmudsharipov/todoclient:$v
docker image push mahmudsharipov/todoclient:$v