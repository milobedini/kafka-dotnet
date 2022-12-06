# README

A simple kafka enrichment pattern written using .Net Core (6).
Includes:

- Generator - Generates 10 messages, one every 10 seconds.
- Enrichment service - Reads messages from incoming, enriches them and sends to outgoing.
- Kafka - One node cluster.
- Zookeeper (used by kafka).
- Kafkdrop - a simple web UI to look at topics and messages.

### How do I get set up?

```

$ docker-compose up -d
$ docker-compose logs -f

Open http://127.0.0.1:9000 to look at the topics.

```

