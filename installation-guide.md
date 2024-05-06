---
layout: page
title: Installation guide
permalink: /installation-guide
---

## Have you any Docker?

First you'll need to have [Docker](https://www.docker.com/) installed. Probably the easiest method is just to download an installer for your system. There are other hacker-friendly methods like using homebrew but I won't cover them here.

Ok, so you now have a Docker. Make sure you've got command line tooling working as well. You can test that by opening a terminal and executing:

```
docker -v
```

You'll get a short response showing the installed version of Docker.

## Set up a fresh partsbin installation
Next, download or copy [`docker-compose.yaml`](https://github.com/becdetat/partsbin/blob/main/docker-compose.yaml). If you're on MacOS, Linux, or Windows/WSL with cURL installed you could also just do this:

```
curl https://raw.githubusercontent.com/becdetat/partsbin/main/docker-compose.yaml > docker-compose.yaml
```

Now copy `docker-compose.yaml` to somewhere safe, like on a NAS or in DropBox. You'll end up storing partsbin's data files there too.

By default the `docker-compose.yaml` file will write its data files to `./data`, which is a directory relative to the `docker-compose.yaml` file when it is brought up by Docker.

Given that, I personally copy `docker-compose.yaml` to `/Volumes/NAS/docker-apps/partsbin`, which is a location on my NAS where I store other Docker application's data. The default `docker-compose.yaml` would then create a `./data` directory within `/Volumes/NAS/docker-apps/partsbin` and use that to store data.

partsbin will run mapped to port 8080, which is configured in `docker-compose.yaml` as well—feel free to change that to anything that isn't already being used.

Once `docker-compose.yaml` is in the correct location and you're happy with the configuration, run the following to get it up and running:

```
docker compose up -d
```

This will pull down the image and then start it running in detached mode (so it doeesn't tie up the terminal).

Go to <http://localhost:8080> and you should be greeted with an empty partsbin home screen.

If there are issues, you can either check the container in the Docker dashboard, or try running it from the terminal without detached mode enabled (which will let you see any errors). Just use `docker compose up` (without the `-d`) to run the container attached to the terminal.

### Upgrading

When a new version of partsbin is released, you can upgrade simply by travelling to where you put `docker-compose.yaml` and running the following:

```
docker compose pull
docker compose up -d
```

And if you have any issues, please report them as [GitHub Issues](https://github.com/becdetat/partsbin/issues).
