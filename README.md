# Transmitly.TemplateEngine.Fluid

A [Transmitly™](https://github.com/transmitly/transmitly) template engine that enables rendering templates with the [Fluid](https://github.com/sebastienros/fluid) template engine.

### Getting started

To use the Fluid template engine, first install the [NuGet package](https://nuget.org/packages/transmitly.templateengine.fluid):

```shell
dotnet add package Transmitly.TemplateEngine.Fluid
```

Then add the template engine using `AddFluidTemplateEngine()`:

```csharp
using Transmitly;
...
var communicationClient = new CommunicationsClientBuilder()
	.AddFluidTemplateEngine();
```

* Check out the [Transmitly](https://github.com/transmitly/transmitly) project for more details on what a template engine is and how it can be used to improve how you manage your customer communications.


### Copyright and Trademark 

Copyright © 2024–2025 Code Impressions, LLC.

Transmitly™ is a trademark of Code Impressions, LLC.

This open-source project is sponsored and maintained by Code Impressions
and is licensed under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).

The Apache License applies to the software code only and does not grant
permission to use the Transmitly name or logo, except as required to
describe the origin of the software.
