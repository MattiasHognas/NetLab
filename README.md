# What is this?

This is a shared whiteboard app. It's built in a strictly separated microservice architecture.

The goals of the project are:<br/>
🚀 high speed output.<br/>
💾 low memory usage.<br/>
🌈 use only bleeding edge tools and frameworks.<br/>
🤖 automate development as much as possible.<br/>

## Tools

### Build tools

Run docker-compose (or vscode launch settings).

#### Build actions

Build containers:
```
dotnet-compose build
```

Start containers:
```
dotnet-compose up
```

## Features

### Features implemented

✔️ Add devcontainer<br/>
✔️ Add devcontainer tools<br/>
✔️ Add workspace service<br/>
✔️ Add workspace integration tests<br/>
✔️ Add database to workspace service<br/>
✔️ Add workspace dockefile<br/>
✔️ Add content service<br/>
✔️ Add content integration tests<br/>
✔️ Add database to content service<br/>
✔️ Add content dockefile<br/>
✔️ Add build scripts<br/>
✔️ Add database to content service<br/>
✔️ Add migrations
✔️ Add authentication service<br/>
✔️ Add database to authentication service<br/>
✔️ Add content dockefile<br/>

### Features to implement

❤️ Add jwt validation to services<br/>
❤️ Add web frontend<br/>
❤️ Add SignalR to web frontend<br/>
❤️ Add Kafka pubsub on changes in services<br/>
❤️ Add SignalR hub triggering on Kafka consumer<br/>
❤️ Add ability to create/edit/remove a room<br/>
❤️ Add ability to request to join a room<br/>
❤️ Add ability to invite to a room<br/>
❤️ Add SignalR to real-time view updates<br/>
❤️ Add material design<br/>
❤️ Add streaming of events storing to logs and db<br/>
❤️ Add local cache<br/>
❤️ Add GitHub actions<br/>
❤️ Add infrastructure as code<br/>
❤️ Add staging env<br/>
❤️ Add force feature branches<br/>
❤️ Add CD staging from feature branch<br/>
❤️ Add production env<br/>
❤️ Add master merge on post-staging accepted<br/>
❤️ Add CD production from master<br/>
❤️ Add readable coverage report<br/>
❤️ Add domainname to production env

### Features nice to have

💎 Add isomorphic GUI<br/>
💎 Add frontend cache<br/>
💎 Add services cache<br/>
💎 Add ability to create templates<br/>
💎 Add ability upload images<br/>
💎 Add ability to export as pdf/image/json<br/>
💎 Add ability to import from json<br/>
💎 Add video/audio of users trough WebRTC?<br/>
💎 Add gRPC?<br/>
💎 Add ability to make 3D models?
