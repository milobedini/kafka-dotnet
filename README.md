# README

A simple kafka enrichment pattern written using .Net Core (6).
Includes:

- Generator - Generates 10 messages, one every 10 seconds.
- Enrichment service - Reads messages from incoming, enriches them and sends to outgoing.
- Kafka - One node cluster.
- Zookeeper (used by kafka).

### How do I get set up?

Currently:

```

$ docker-compose up -d
$ cd generator && dotnet run generator.cs
$ cd enrichment && dotnet run enrichment.cs


```

This will be fully containerised in the near future, and a UI to view the topics will also be implemented.

### Who do I talk to?

- Repo owner milo.bedini@dae.mn
- Any of the Daemon Frameworks team!
