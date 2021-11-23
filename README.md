```csharp
string containerName = "TestContainer";

IOCContainer container = IOCManager.Create(containerName)
  .Set<ISubFoo, SubFoo>();

IOCContainer sameContainer = IOCManager.Get(containerName);

ISubFoo instance = sameContainer.Get<ISubFoo>();

int s = instance.Number;
```
