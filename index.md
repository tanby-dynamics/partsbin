---
layout: page
---

{%- assign date_format = site.minima.date_format | default: "%b %-d, %Y" -%}

<aside class="well pull-right" style="width: 15rem">
    <h5>Releases</h5>
    <p>
        {%- for post in site.posts -%}
            <span class="post-meta">{{ post.date | date: date_format }}</span>
            <a href="{{ post.url | relative_url }}">
                {{ post.title | escape }}
            </a><br/>
        {%- endfor -%}   
    </p>
</aside>

Attention all hobbyist hardware hackers and engineers! Are you tired of sifting through piles of disorganised electronic components and struggling to keep track of what you have? Look no further!

This simple to use application, complete with intuitive search features and a simple flat taxonomy model, is designed specifically with you in mind. Say goodbye to the chaos and hello to a streamlined system for keeping track of your parts inventory, making your next electrical experiment smoother than ever.

Whether you're an amateur hobbyist or a seasoned engineer, partsbin is the perfect tool to help you stay organised, productive, and get ahead of the game.

It's free, open source with a liberal license (MIT), and easy to install via a Docker image, which can sit on a home server or on your lab PC. It is web based (using Blazor) and works reasonably well with tablets and mobile devices as well—landscape mode on an iPad works best otherwise some of the wider tables are a bit iffy.

<aside class="well pull-right" style="width:20rem">
    <h5>Install it now</h5>
    <p><small>This is not the <a href="/installation-guide">recommended installation method</a>, but it will get you running fast.</small></p>

    <pre>
docker pull becdetat/partsbin:latest
docker run -v ~/partsbin/data:/data -p 8080:80 -d becdetat/partsbin:latest</pre>

    <p>You should now be able to access partsbin at <code>http://localhost:8080</code>, and the data should be stored at <code>~/partsbin/data</code>.</p>
</aside>


Get it from [Docker](https://hub.docker.com/repository/docker/becdetat/partsbin), but the recommended method is to copy [docker-compose.yaml](https://github.com/becdetat/partsbin/blob/main/docker-compose.yaml) to somewhere safe, edit it to suit your setup, and run `docker compose up -d` to make the magic happen.

See the [installation guide](/installation-guide) for detailed installation and upgrade instructions.

By default, when installed using `docker compose`, partsbin will run on port 8080—access it via [http://localhost:8080](http://localhost:8080).

It's totally self-hosted—I have no plans to develop any kind of security or user management, so you should make sure it's restricted to your home/work network.

The design also means that multiple users and concurrency may be an issue. This has only been tested with a single user at a time. It really is a small scale hobbyist tool.

Follow development at <https://github.com/becdetat/partsbin>.

partsbin is a project by [Rebecca Scott](https://becdetat.com), allegedly a human who codes and plays with Arduinos, and pays for hundreds of dollars of components at ebay and aliexpress by working as an IT consultant at [SixPivot](https://sixpivot.com.au).

Thanks to my good friend ChatGPT for helping out with the above copy.

