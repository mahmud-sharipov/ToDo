Param(
    [string]$v
    )

docker build -f "C:\Repos\ToDo\WebUI\Dockerfile" --force-rm -t todoclient:$v --target final "C:\Repos\Todo" 
docker image tag todoclient:$v mahmudsharipov/todoclient:$v
docker image push mahmudsharipov/todoclient:$v