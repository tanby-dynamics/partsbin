# partsbin

Attention all hobbyist hardware hackers and engineers! Are you tired of sifting through piles of disorganised electronic components and struggling to keep track of what you have? Look no further!

This simple to use application, complete with intuitive search features and a simple flat taxonomy model, is designed specifically with you in mind. Say goodbye to the chaos and hello to a streamlined system for keeping track of your parts inventory, making your next electrical experiment smoother than ever.

Whether you’re an amateur hobbyist or a seasoned engineer, partsbin is the perfect tool to help you stay organised, productive, and get ahead of the game.

It’s free, open source with a liberal license (MIT), and easy to install via a Docker image, which can sit on a home server or on your lab PC. It is web based (using Blazor) and works reasonably well with tablets and mobile devices as well—landscape mode on an iPad works best otherwise some of the wider tables are a bit iffy.

It's not really intended for use at a scale of more than a few simultaneous users - in fact I'm only planning to test a single user, perhaps with a couple of instances on different machines pointing at the same data file. The idea is to be able to spin an instance up on your workbench computer or a home server via a single Docker command.

The data file location is configurable, I suggest using `docker compose` per the [installation guide](https://partsbin.page/installation-guide) and putting it in Dropbox, BackBlaze, or a NAS where it will be backed up automatically.


## Installation
Please see the [installation guide](https://partsbin.page/installation-guide) on [partsbin.page](https://partsbin.page).


## Release plans
Note that this is subject to change.

### Release 0.2
- 'enter' to search
- Give 'part number' a little link to open a Google search for it
- "Add to home screen" option in Admin for mobile devices
- Supplier management for parts
- List supplier name/site/contact details separately from parts, and allow selecting them when editing a part (for some data normalisation)
- Documentation management for parts
- Perhaps uploading arbitrary files (docs, images, etc)
- Adding arbitrary links

### Release 0.3
- Some kind of project management, like 1. create a project 2. add parts to it 3. add docs 4. generate a BOM (spreadsheet with supplier links)
- ChatGPT prompt generation, like "Given an Arduino Uno and a shift register how do I make the Knight Rider animation?"
- Look into auth/access management via an additional Docker image

### ???
Please submit your ideas and suggestions [here](https://github.com/becdetat/partsbin/issues).


## Design choices
- Server-side Blazor, with help from [Blazored](https://github.com/Blazored)
- Storage in [LiteDB](https://www.litedb.org/) (JSON store - really fun and fast both as a developer and performance at small scales)
- Rich text editing via [Quill](https://quilljs.com/) (lovely, does image embeds as base64 encoded values)
- Distributed as a Docker image
- No authentication or access management (althought it might be possible to do this via an additional Docker image...)
- No tests - where we're going we don't need tests
- No external dependencies, everything file based and in process - database is a single file document db, search is via Lucene.NET


## Technologies and libraries in use
- Blazor (including Bootstrap)
- [Blazored](https://github.com/Blazored) -  modals and toasts
- [LiteDb](https://www.litedb.org/)
- [Lucene.NET](https://lucenenet.apache.org/)
- [Quill](https://quilljs.com/), using a tutorial on [puresourcecode.com](https://www.puresourcecode.com/dotnet/blazor/create-a-blazor-component-for-quill/) to get it working

## Contributing
You can contribute just by making bug reports or suggestions, or you can put your valuable time into writing new features or fixing bugs. See the [contribution guide](CONTRIB.md) for more information. Read through the [Code of Conduct](code-of-conduct.md) too please.

All contributors (code, bug reports, suggestions) wiil be noted in the [Hall of Fame](hall-of-fame.md) (which will probably be included in the app some time).


