# partsbin

partsbin is a really simple CRUD app with some search capabilities, to
keep track of parts for hobbyist electrical experimenters.

It's not really intended for use at a scale of more than a few simultaneous users - in
fact I'm only planning to test a single user, perhaps with a couple of instances on
different machines pointing at the same data file. The idea is to be able to spin an
instance up on your workbench computer or a home server via a single Docker command.

The data file location will be configurable when the Docker container is spun up, I would
suggest putting it in Dropbox or somewhere it will be backed up automatically.


## Installation
It's possible to build and deploy this on a server from the source, however the easiest
method is probably via Docker.

You can do this using `docker run`:
```
docker run -d -p 8080:80 -v ~/Dropbox/appdata/partsbin/data/:/data --name partsbin partsbin
```

Or to make it a bit easier to manage the options:
1. Copy `docker-compose.yaml` from this repository to somewhere safe (I usually put it at
the top level of where the app volumes will be stored)
3. Edit `docker-compose.yaml`
4. (optional) Update the mapped port to something that won't clash with any other services you have running
5. Change the `/data` volume location to somewhere on your NAS, Dropbox, etc
4. `cd` to where your `docker-compose.yaml` copy is and execute `docker-compose up -d`

If you skip step 4, port 8080 will be used.

If you skip step 5, the database will be written to an internal location which will get
deleted when the container is updated. You _probably_ don't want that.

When the image is pulled down and the container is running, you can access your new partsbin
installation at <http://localhost:8080> (or whatever port you set in step 4).


## Design choices
- Server-side Blazor, with help from [Blazored](https://github.com/Blazored) and 
[Radzen.Blazor](https://blazor.radzen.com/get-started)
- Storage in [LiteDB](https://www.litedb.org/) (JSON store)
- Distributed as a Docker image
- No authentication or access management
- No tests - where we're going we don't need tests


## Todos
### Core features
- Docker image is current arm64, need to get it working on amd64 as well
- LiteDB database in a location that can be configured when creating the Docker container
- Edit parts
- Front page navigation and drill-down to parts
- Auto-suggest for `PartType`, `Range`, `PartName`, `PackageType`, `ValueUnit`, `Location`,
`Manufacturer`, `Supplier.Name`
- If `PartNumber` isn't unique, throw up a warning and recommend editing existing part.
*Note that this it should still be allowed to have multiple parts with the same `PartNumber`*
- Notes
- Links to documentation (data sheets etc)
- Upload documentation
- Links to suppliers
- Upload images - a mini gallery for each part


### Stretch goals and features
- Search engine, possibly via [Sonic](https://github.com/valeriansaliou/sonic) (using
[NSonic](https://github.com/spikensbror-dotnet/nsonic))
- Minimal ChatGPT integration - generate a natural language question like "Please show
me how to use a SN74HC595N shift register with an Arduino Nano to light up some LEDs",
button to copy to clipboard and open ChatGPT

