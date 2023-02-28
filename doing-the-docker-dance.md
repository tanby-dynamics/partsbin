# Do the dance...do the docker dance

## Building and deploying the volume
https://hub.docker.com/repository/docker/becdetat/partsbin/general

1. In the root directory, `docker build ./src`. This builds the volume from `./src/Dockerfile`.


## Running up an instance
Copy `docker-compose.yml` somewhere, then edit it to suit your needs.

By default it maps port 80 (which is what `partsbin` exposes) to port 8080, so you should be able to access it via `http://127.0.0.1:8080`.

It also maps `/data` to a folder in `~/Dropbox`, so the data files are stored somewhere distributed and backed up. You might want to keep it local, or put it on a NAS or file share.

Then create the container:

```
docker compose up -d
```




