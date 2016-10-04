[![Build status](https://img.shields.io/appveyor/ci/silverrockinc/lazuli/master.svg?maxAge=300)](https://ci.appveyor.com/project/silverrockinc/lazuli)
[![Test coverage](https://img.shields.io/coveralls/SilverRockInc/Lazuli.svg?maxAge=300)](https://coveralls.io/github/SilverRockInc/Lazuli/)
[![Nuget package](https://img.shields.io/nuget/vpre/SilverRock.Lazuli.svg?maxAge=300)](https://www.nuget.org/packages/SilverRock.Lazuli/)

# Lazuli
Lazuli is a JSON/YAML-based command line tool for configuring Microsoft Azure entities.  (Currently, only Service Bus Topics and App Services are supported.)
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
All examples are in YAML, anthough Lazuli also supports JSON configs.

### Deployment Environments
The config file contains a list property "`deployEnvironments`" which allows for multiple environments to be defined within a single file.  Each environment
has a `name` which is passed as the `e|environment` when running Lazuli. Each environment can contain multiple Entities.

### Entities
Each entity (eg. Topic, AppService) has three optional properties: `create`, `update`, and `remove`.

### Topics
The creation of a Service Bus Topic assumes the prior existance of a Service Bus Namespace.  Each Topic has a required `namespace` property which
contains the `endpoint`, `accessKeyName`, and `accessKey` for that namespace. The rest of the shcema for Topic objects
is the same as the [`TopicDefinition`](https://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.topicdescription.aspx) class in Microsoft's [SerivceBus package](https://www.nuget.org/packages/WindowsAzure.ServiceBus/).

Example:
```YAML
# Required properties
namespace:
  endpoint: sb://some-service-bus.servicebus.windows.net
  accessKeyName: RootManageSharedAccessKey
  accessKey: hunter2
path: my-topic-path

# Optional properties
authorization:
- name: default
  primaryKey: XsEK+cAqjfuMZSN1kDGiNsb7p3
  accessRights:
  - Manage
  - Send
  - Listen
autoDeleteOnIdle: 00:30:00
defaultMessageTimeToLive: 1.00:00:00
duplicateDetectionHistoryTimeWindow: 1.00:00:00
enableBatchedOperations: false
enableExpress: false
enableFilteringMessagesBeforePublishing: true
enablePartitioning: true
maxSizeInMegabytes: 10240
requiresDuplicateDetection: true
subscriptions: # Array of subscriptions; see below
supportOrdering: true
userMetadata: 
```

### Subscriptions
Topics may contain multiple Subscriptions. In general the shcema for Subscription objects is the same as the
[`SubscriptionDefinition`](https://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.subscriptiondescription.aspx)
class in Microsoft's [SerivceBus package](https://www.nuget.org/packages/WindowsAzure.ServiceBus/).
However, there is also an added `sqlFilter` property which allows for the definition of a `SqlFilter` constructed from the
supplied string.

Example:
```YAML
# Required properties
name: my-topic-name

# Optional properties
autoDeleteOnIdle: 00:30:00
defaultMessageTimeToLive: 1.00:00:00
enableBatchedOperations: false
enableDeadLetteringOnFilterEvaluationExceptions: true
enableDeadLetteringOnMessageExpiration: true
forwardDeadLetteredMessagesTo:
forwardTo:
lockDuration: 00:01:00
maxDeliveryCount: 10
requiresSession: false
sqlFilter: CustomProp = 'Some value'
userMetadata:
```

### App Services
Currently, only application settings are supported for App Services.  Application settings
can be updated for multiple sites by adding multiple `account` entries.  The
`serviceName` property corresponds to the name of the App Service in Azure. The
`username` and `password` properties correspond to the deployment credentials for the App.

Example:
``` YAML
appServices:
  update:
  - accounts:
    - serviceName: some-auzre-app-service-west
      username: AzureDiamond
      password: hunter2
    - serviceName: some-auzre-app-service-central
      username: deployaccount
      password: 123456
    settings:
      SomeSetting: Setting value
      AnotherSetting: Another setting value
  - accounts:
    - serviceName: another-app-service
      username: rmunroe
      password: correcthorsebatterystaple
    settings:
      my-password: Y29ycmVjdGhvcnNlYmF0dGVyeXN0YXBsZQ==
```