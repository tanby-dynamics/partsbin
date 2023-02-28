# Do the dance...do the docker dance

This is the manual process.

The package gets published to the [Docker Hub](https://hub.docker.com/repository/docker/becdetat/partsbin/general).

## Building and deploying the image
In the root directory:

```
docker build -t becdetat/partsbin ./src
```

This builds the image from `./src/Dockerfile`, calling it `becdetat/partsbin`.

Now give it a tag. If it's a test build you can tag it anything you want. Don't tag it as `latest` because that's done automatically when the `main` branch is updated:

```
docker tag becdetat/partsbin becdetat/partsbin:202302281336
```

Now the image needs to deployed (pushed) to hub.docker.com:

```
docker push becdetat/partsbin:202302281336
```

## Running up an instance
Copy `docker-compose.yml` somewhere, then edit it to suit your needs.

By default it maps port 80 (which is what `partsbin` exposes) to port 8080, so you should be able to access it via `http://127.0.0.1:8080`.

It also maps `/data` to a folder in `~/Dropbox`, so the data files are stored somewhere distributed and backed up. You might want to keep it local, or put it on a NAS or file share.

Then create the container byu running this in the directory `docker-compose.yml` lives:

```
docker compose up -d
```




