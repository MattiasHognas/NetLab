# Project

Shared drawing microservice

## Tools

### Build tools

Use vscode launch settings or run cake.

#### Build actions

(All following targets also run clean, build, tests and migration)

Start all:
```
dotnet cake --Target=StartAll
```

Build containers:
```
dotnet cake --Target=BuildDocker
```

Build artifacts:
```
dotnet cake --Target=PublishArtifacts
```

## Features

### Features implemented

✔️ Add devcontainer
✔️ Add devcontainer tools
✔️ Add workspace service
✔️ Add workspace integration tests
✔️ Add database to workspace service
✔️ Add workspace dockefile
✔️ Add content service
✔️ Add content integration tests
✔️ Add database to content service
✔️ Add content dockefile
✔️ Add build scripts
✔️ Add database to content service

### Features to implement
 
❤️ Add authentication service
❤️ Add database to authentication service
❤️ Add content dockefile
❤️ Add jwt validation to services
❤️ Add web frontend
❤️ Add SignalR to web frontend
❤️ Add Kafka pubsub on changes in services
❤️ Add SignalR hub triggering on Kafka consumer
❤️ Add ability to create/edit/remove a room
❤️ Add ability to request to join a room
❤️ Add ability to invite to a room
❤️ Add SignalR to real-time view updates
❤️ Add material design
❤️ Add streaming of events storing to logs and db
❤️ Add local cache
❤️ Add GitHub actions
❤️ Add infrastructure as code
❤️ Add staging env
❤️ Add force feature branches
❤️ Add CD staging from feature branch
❤️ Add production env
❤️ Add master merge on post-staging accepted
❤️ Add CD production from master
❤️ Add readable coverage report
❤️ Add domainname to production env

### Features nice to have

💎 Add isomorphic GUI
💎 Add frontend cache
💎 Add services cache
💎 Add ability to create templates
💎 Add ability upload images
💎 Add ability to export as pdf/image/json
💎 Add ability to import from json
💎 Add video/audio of users trough WebRTC?
💎 Add gRPC?
💎 Add ability to make 3D models?
