# partsbin

partsbin is (intended to be) a really simple CRUD app with some search capabilities, to
keep track of components for hobbyist electrical experimenters.

It's not really intended for use at a scale of more than a few simultaneous users. The
idea is to be able to spin an instance up on your workbench computer or a home server via
Docker.

## Goals

### Implementation
- [ ] Server-side Blazor
- [ ] Off the shelf Blazor supported UI library, nothing fancy (TBD)
	- [ ] Blazored
	- [ ] Radzen.Blazor
- [ ] Storage in SQLite
	- [ ] EF migrations, to keep it simple
- [ ] Distributed as a Docker image
- [ ] No authentication or access management - that's up to you :-)

### Core features
- [ ] SQLite in a location that can be configured when creating the Docker container
- [ ] Upload or search for images
- [ ] Add information sheet links for each component

### Stretch goals and features
- [ ] Desktop application wrapper
- [ ] Search engine via [Sonic](https://github.com/valeriansaliou/sonic) (using
[NSonic](https://github.com/spikensbror-dotnet/nsonic))
- [ ] Auto search for part images
- [ ] Auto search for information sheets

