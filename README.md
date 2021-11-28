Have you ever thought about how it would be if you tried to write IOC library or how DI works?  
I guess it's something like that 🙄  

```csharp
string containerName = "FooContainer";

IOCContainer container = IOCManager.Create(containerName)
  .Set<IFoo, Foo>();

IOCContainer sameContainer = IOCManager.Get(containerName);

IFoo instance = sameContainer.Get<Foo>();

int number = instance.NumberProperty;
```

It's that much easy 😏 Check the IOCTest.cs file for more examples.  
https://github.com/alidemirbas/P.IOC/blob/master/IOCTest.cs  
