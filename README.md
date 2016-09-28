[![Build status](https://img.shields.io/appveyor/ci/silverrockinc/lazuli/master.svg?maxAge=300)](https://ci.appveyor.com/project/silverrockinc/lazuli)
[![Test coverage](https://img.shields.io/coveralls/SilverRockInc/Lazuli.svg?maxAge=300)](https://coveralls.io/github/SilverRockInc/Lazuli/)
[![Nuget package](https://img.shields.io/nuget/vpre/SilverRock.Lazuli.svg?maxAge=300)](https://www.nuget.org/packages/SilverRock.Lazuli/)

# Lazuli
Lazuli is a JSON/YAML-based command line toole for configuring Microsoft Azure entities.  (Currently, only Service Bus Topics and App Services are supported.)
It is named after the semi-precious stone Lapis Lazuli from which the word "Azure" is derived.

## Design Philosophy
* Configuration is in source control
* Configuration can be applied as part of the build and/or deployment process
* Code that *applies* the configuration is independent of project

## Quick Start
1. Get Lazuli via NuGet: `Install-Package SilverRock.Lazuli -Pre`
1. Create a config file (config.yml):
```YAML
deployEnvironments:
- name: prod
  topics:
    create:
    - namespace:
        endpoint: sb://some-service-bus.servicebus.windows.net
        accessKeyName: RootManageSharedAccessKey
        accessKey: hunter2
      path: my-topic-path
      authorization:
      - name: default
        primaryKey: XsEK+cAqjfuMZSN1kDGiNsb7p3
        accessRights:
        - Manage
        - Send
        - Listen
      subscriptions:
      - name: audit
        defaultMessageTimeToLive: 7.00:00:00
      - name: filtered
        sqlFilter: CustomProp = 'Some value'
```
3. Run Lazuli: `{path to package}\lazuli.exe -e:prod -c:config.yml`

## File Structure

### Deployment Environments
The config file contains a list property "`deployEnvironments`" which allows for multiple environments to be defined within a single file.  Each environment
has a `name` which is passed as the `e|environment` when running Lazuli. Each environment can contain multiple Entities.

### Entities
Each entity (eg. Topic, AppService) has three optional properties: `create`, `update`, and `remove`.

### Topics
The creation of a Service Bus Topic assumes the prior existance of a Service Bus Namespace.  Each Topic has a `namespace` property which
contains the `endpoint`, `accessKeyName`, and `accessKey` for that namespace. The rest of the shcema for Topic objects
is the same as the `TopicDefinition` class in Microsoft's [SerivceBus package](https://www.nuget.org/packages/WindowsAzure.ServiceBus/).

### Subscriptions
Topics may contain multiple Subscriptions. In general the shcema for Subscription objects
is the same as the `SubscriptionDefinition` class in Microsoft's [SerivceBus package](https://www.nuget.org/packages/WindowsAzure.ServiceBus/).
However, there is also an added `sqlFilter` property which allows for the definition of a `SqlFilter`
