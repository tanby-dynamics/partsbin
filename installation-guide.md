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
Next, create a file somewhere safe, named `docker-compose.yml`. You should put it somewhere where it will be backed up as the data file will be written to `./data` by default. Copy the following to your `docker-compose.yml` file:

```
services:
  partsbin:
    image: becdetat/partsbin:latest
    ports:
      - 8035:8080
    volumes:
      - ./data:/data
```

Then run the following:

```
docker compose up -d
```

Go to <http://localhost:8035> and you should be greeted with an empty partsbin home screen.

By default the `docker-compose.yaml` file will write its data files to `./data`, which is a directory relative to the `docker-compose.yaml` file.

### Upgrading

When a new version of partsbin is released, you can upgrade simply by going to where you put `docker-compose.yaml` and running:

```
docker compose pull
docker compose up -d
```

If you have any issues, please report them as [GitHub Issues](https://github.com/becdetat/partsbin/issues).
