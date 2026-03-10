# Transmitly.TemplateEngine.Fluid

`Transmitly.TemplateEngine.Fluid` adds [Fluid](https://github.com/sebastienros/fluid) templating support to the core [Transmitly](https://github.com/transmitly/transmitly) library.

This package is typically used alongside:

- `Transmitly`
- A Transmitly channel provider such as `Transmitly.ChannelProvider.Smtp`, `Transmitly.ChannelProvider.SendGrid`, or `Transmitly.ChannelProvider.Infobip`

## Installation

Install the core library, this template engine, and a channel provider:

```shell
dotnet add package Transmitly
dotnet add package Transmitly.TemplateEngine.Fluid
dotnet add package Transmitly.ChannelProvider.Smtp
```

## Quick Start

```csharp
using Transmitly;
using Transmitly.ChannelProvider.Smtp.Configuration;

ICommunicationsClient client = new CommunicationsClientBuilder()
	.AddSmtpSupport(options =>
	{
		options.Host = "smtp.example.com";
		options.Port = 587;
		options.UserName = "smtp-user";
		options.Password = "smtp-password";
	})
	.AddFluidTemplateEngine()
	.AddPipeline("order-shipped", pipeline =>
	{
		pipeline.AddEmail("orders@my.app".AsIdentityAddress("Orders"), email =>
		{
			email.Subject.AddStringTemplate("Order {{orderNumber}} has shipped");
			email.HtmlBody.AddStringTemplate(
				"""
				<p>Hello {{firstName}},</p>
				{% if trackingUrl %}
				<p>
					Your order has shipped.
					Track it here:
					<a href="{{trackingUrl}}">{{trackingNumber}}</a>
				</p>
				{% else %}
				<p>Your order has shipped.</p>
				{% endif %}
				"""
			);
			email.TextBody.AddStringTemplate("Hello {{firstName}}, your order {{orderNumber}} has shipped.");
		});
	})
	.BuildClient();

var result = await client.DispatchAsync(
	"order-shipped",
	"customer@example.com".AsIdentityAddress("Ava Example"),
	new
	{
		firstName = "Ava",
		orderNumber = "A10023",
		trackingNumber = "1Z999",
		trackingUrl = "https://carrier.example/track/1Z999"
	});
```

## How It Fits Into Transmitly

Transmitly templates are attached to channel content such as:

- `email.Subject`
- `email.HtmlBody`
- `email.TextBody`
- `sms.Message`
- `push.Title`
- `push.Body`

When you call `DispatchAsync(...)`, Transmitly passes the model you supply into the registered template engine. With Fluid registered, placeholders like `{{firstName}}` are resolved from `context.ContentModel.Model`.

## Registering The Engine

Use either of these registration styles:

```csharp
new CommunicationsClientBuilder()
	.AddFluidTemplateEngine();
```

```csharp
new CommunicationsClientBuilder()
	.TemplateEngine.AddFluidTemplateEngine();
```

If you need to customize Fluid parsing, use the overload that accepts `FluidParserOptions`:

```csharp
using Fluid;

new CommunicationsClientBuilder()
	.AddFluidTemplateEngine(options =>
	{
		// Configure FluidParserOptions here.
	});
```

## Template Sources

The engine works with the normal Transmitly template registration APIs, including:

- `AddStringTemplate(...)`
- `AddEmbeddedResourceTemplate(...)`
- `AddTemplateResolver(...)`

That means you can keep templates inline, embed them in assemblies, or resolve them dynamically at dispatch time.

## Behavior Notes

- Invalid Fluid templates currently return `null` rather than throwing from this package.
- A `null` content model is allowed; missing values render as empty output.
- The current Transmitly core supports only one template engine registration per `CommunicationsClientBuilder`.

## Related Packages

- [Transmitly](https://github.com/transmitly/transmitly)
- [Transmitly.TemplateEngine.Scriban](https://github.com/transmitly/transmitly-template-engine-scriban)
- [Transmitly.ChannelProvider.Smtp](https://github.com/transmitly/transmitly-channel-provider-smtp)

---
_Copyright (c) Code Impressions, LLC. This open-source project is sponsored and maintained by Code Impressions and is licensed under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
