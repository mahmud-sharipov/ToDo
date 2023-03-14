Param(
    [string]$v
    )

docker build -f "C:\Repos\ToDo\src\API\Dockerfile" --force-rm -t todoserver:$v --target final "C:\Repos\Todo\src" 
docker image tag todoserver:$v mahmudsharipov/todoserver:$v
docker image push mahmudsharipov/todoserver:$v