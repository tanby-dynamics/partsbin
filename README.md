# partsbin

partsbin is (intended to be) a really simple CRUD app with some search capabilities, to
keep track of components for hobbyist electrical experimenters.

It's not really intended for use at a scale of more than a few simultaneous users. The
idea is to be able to spin an instance up on your workbench computer or a home server via
Docker.

## Goals

### Implementation goals
- [-] Server-side Blazor
- [-] Off the shelf Blazor supported UI library, nothing fancy (TBD)
	- [-] Blazored
	- [-] Radzen.Blazor
- [ ] Storage in [LiteDB](https://www.litedb.org/) (JSON store)
- [ ] Distributed as a Docker image
- [-] No authentication or access management - that's up to you :-)
- [-] No tests - where we're going we don't need tests
- [-] Default dependency injection
- [-] Single project (if possible)

### Core features
- [ ] LiteDb database in a location that can be configured when creating the Docker container
- [ ] Upload or search for images
- [ ] Add information sheet links for each component

### Stretch goals and features
- [ ] Desktop application wrapper
- [ ] Search engine, possibly via [Sonic](https://github.com/valeriansaliou/sonic) (using
[NSonic](https://github.com/spikensbror-dotnet/nsonic))
- [ ] Adding and searching for part images
- [ ] Adding and searching for information sheets
- [ ] Minimal ChatGPT integration - generate a natural language question like "Please show
me how to use a SN74HC595N shift register with an Arduino Nano to light up some LEDs"

