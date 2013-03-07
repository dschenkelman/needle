Needle Dependency Injection Container
======
Needle Dependency Injection Container is a lightweight dependency injection container with a fluent API. The main goal of the project is help developers learn Depencency Injection, how a DI container could be created, and similar stuff.

Samples
=======
Simple type mapping:
```CSharp
INeedleContainer needleContainer = new NeedleContainer(); 
needleContainer.Map<IForceEnlightened>() // when we request an IForceEnlightened
  .To<Jedi>() // provide a Jedi
  .Commit(); // save the mapping
```

Type mappings with specific identifiers:
```CSharp
needleContainer
    .Map<IForceEnlightened>()
    .To<Jedi>()
    .WithId("Jedi")
    .Commit();

needleContainer
    .Map<IForceEnlightened>()
    .To<Sith>()
    .WithId("Sith")
    .Commit();
```

Lifetime management:
```CSharp
needleContainer
    .Map<IForceEnlightened>()
    .To<Jedi>()
    .UsingLifetime(RegistrationLifetime.Singleton)
    .Commit();
```

Putting it all together:
```CSharp
needleContainer
    .Map<IForceEnlightened>()
    .To<Jedi>()
    .WithId("Yoda")
    .UsingLifetime(RegistrationLifetime.Singleton)
    .Commit();
```

NuGet Package
=============
You can now download the alpha (0.1) version of the Needle Container using NuGet. In the Package Manager window type:
```
PM> Install-Package NeedleContainer
```

[Check out the release notes](http://blogs.southworks.net/dschenkelman/2011/02/18/needle-dependency-injection-container-alpha-0-1-version-released/).
