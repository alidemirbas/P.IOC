```csharp
string containerName = "TestContainer";

IOCContainer container = IOCManager.Create(containerName)
  .Set<ISubFoo, SubFoo>();

IOCContainer sameContainer = IOCManager.Get(containerName);

ISubFoo instance = sameContainer.Get<ISubFoo>();

int s = instance.Number;
```

It's that much easy 😏 Check the IOCTest.cs file for more examples
https://github.com/alidemirbas/P.IOC/blob/master/IOCTest.cs
