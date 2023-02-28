# partsbin

partsbin is a really simple CRUD app with some search capabilities, to keep track of parts for hobbyist electrical experimenters.

It's not really intended for use at a scale of more than a few simultaneous users - in fact I'm only planning to test a single user, perhaps with a couple of instances on different machines pointing at the same data file. The idea is to be able to spin an instance up on your workbench computer or a home server via a single Docker command.

The data file location will be configurable when the Docker container is spun up, I would suggest putting it in Dropbox or somewhere it will be backed up automatically.


## Installation
It's possible to build and deploy this on a server from the source, however the easiest method is probably via Docker.

You can do this using `docker run`:
```
docker run -d -p 8080:80 -v ~/Dropbox/appdata/partsbin/data/:/data --name partsbin partsbin
```

Or to make it a bit easier to manage the options:
1. Copy `docker-compose.yaml` from this repository to somewhere safe (I usually put it at the top level of where the app volumes will be stored)
3. Edit `docker-compose.yaml`
4. (optional) Update the mapped port to something that won't clash with any other services you have running
5. Change the `/data` volume location to somewhere on your NAS, Dropbox, etc
4. `cd` to where your `docker-compose.yaml` copy is and execute `docker-compose up -d`

If you skip step 4, port 8080 will be used.

If you skip step 5, the database will be written to an internal location which will get deleted when the container is updated. You _probably_ don't want that.

When the image is pulled down and the container is running, you can access your new partsbin installation at <http://localhost:8080> (or whatever port you set in step 4).


## Design choices
- Server-side Blazor, with help from [Blazored](https://github.com/Blazored)
- Storage in [LiteDB](https://www.litedb.org/) (JSON store - really fun and fast both as a developer and performance at small scales)
- Rich text editing via [Quill](https://quilljs.com/) (lovely)
- Distributed as a Docker image
- No authentication or access management
- No tests - where we're going we don't need tests
- No external dependencies, everything file based and in process - database is a single file document db, search is via Lucene.NET


## Todos
### Core features
- Docker image is currently arm64, need to get it working on amd64 as well
- If `PartNumber` isn't unique, throw up a warning and recommend editing existing part. *Note that it should still be permitted to have multiple parts with the same `PartNumber`*
- Links to documentation (data sheets etc)
- Upload documentation
- Links to suppliers
- Upload images - a mini gallery for each part (maybe not required, since the notes can include embedded images)


### Stretch goals and possible features
- Minimal ChatGPT integration - generate a natural language question like "Please show me how to use a SN74HC595N shift register with an Arduino Nano to light up some LEDs", button to copy to clipboard and open ChatGPT
- What about building up a BOM? that would be interesting
- landing page could be a dashboard - common items that are running low?


## Technologies and libraries in use
- Blazor (including Bootstrap)
- [Blazored](https://github.com/Blazored) -  modals and toasts
- [LiteDb](https://www.litedb.org/)
- [Lucene.NET](https://lucenenet.apache.org/)

## Contributing
You can contribute just by making bug reports or suggestions, or you can put your valuable time into writing new features or fixing bugs. See [the contributor's guide](CONTRIB.md) for more information. Read through the [Code of Conduct](code-of-conduct.md) too please.

All contributors (code, bug reports, suggestions) wiil be noted in the [Hall of Fame](hall-of-fame.md) (which will probably be included in the app some time).


