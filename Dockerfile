FROM ubuntu:latest as builder
RUN apt-get update
RUN apt-get install -y build-essential ruby-full ruby-dev zlib1g-dev liblzma-dev nodejs
RUN gem install bundler jekyll
WORKDIR /app
COPY . .
RUN bundle install
RUN bundle exec jekyll build

FROM nginx:alpine
COPY --from=builder /app/_site /usr/share/nginx/html

