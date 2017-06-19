# (C#) PorEjemplo

PorEjemplo is a small framework, which offers you a simple way to generate objects filled with example values.

<br/>

## Installation

NuGet package manager console

```
> Install-Package PorEjemplo
```

<br/>

## Simple generation

Add the following reference

```csharp
using PorEjemplo.Generator;
```

Create a new instance of the Generator class and add the type of the object which should be generated as a generic type parameter
```csharp
var generator = new Generator<[your type]>();
```

Call the Generate() method
```csharp
var generated = generator.Generate();
```

Currently the generator is able to set random values for all primitive types automated.
To get the random values the Random class of .NET and the ExtendedRandom class, supplied by this framework, are used.

<br/>

## Setup

As said, the default generator will fill all properties, which are of a primitive type, with random generated example values.

The GeneratorSetup is used to configure a generator:

```csharp
var generatorSetup = Generator<[your type]>.Setup();
```

As you see, the type must be provided here as well.

To get the generator from the setup, just write:

```csharp
var generator = generatorSetup.GetGenerator();
```

The configurations must be applied to the setup in a builder pattern like way.
