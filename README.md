# Project

Shared drawing microservice

## Tools

### Migrate data

Migrate Auth changes
```
cd /workspace/src/service/Lab.Service.Auth/Lab.Service.Auth.Api \
dotnet ef migrations add "<NAME>" --project ../Lab.Service.Auth.Data/
```

Migrate Dashboard changes
```
cd /workspace/src/service/Lab.Service.Dashboard/Lab.Service.Dashboard.Api \
dotnet ef migrations add "<NAME>" --project ../Lab.Service.Dashboard.Data/
```

## Todo

### Technical fixes

- Add jwt validation to Dashboard service
- Add e2e tests
- Add Kafka pubsub on changes in services
- Add SignalR hub triggering on Kafka consumer
- Add front end listening to SignalR hub
- Add unit tests
- Add GitHub actions
- Add coverage report

### Features to add

- Add material design
- Change demo service to a real drawing service
- Add ability to create/edit/remove a room
- Add ability to request to join a room
- Add ability to invite to a room
- Add SignalR to real-time view updates
- Add streaming of events storing to logs and db 
- Add local cache
- Add isomorphic GUI
- Add GUI styles
- Add infrastructure as code
- Add staging env
- Add CD
- Add integration test runner to staging env
- Add master merge on CI complete
- Add production env
- Add CI/CD on master

### Features nice to have

- Add server cache
- Add ability to create templates
- Add ability upload images
- Add ability to export as pdf/image/json
- Add ability to import from json
- Add video/audio of users trough WebRTC?
- Add gRPC?
- Add ability to make 3D models?



