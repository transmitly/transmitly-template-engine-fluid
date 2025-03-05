// ﻿﻿Copyright (c) Code Impressions, LLC. All Rights Reserved.
//  
//  Licensed under the Apache License, Version 2.0 (the "License")
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

using Fluid;
using Moq;
using Transmitly.Template.Configuration;

namespace Transmitly.TemplateEngine.Fluid.Tests
{
	[TestClass]
	public class TemplateEngineTests
	{
		[TestMethod]
		public async Task CanRender()
		{
			var expected = "Hello World!";
			var templateContent = "Hello {{name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			//var model = new Mock<IContentModel>();
			var x = typeof(IContentModel).Assembly.GetTypes().Where(t => t.Name == "ContentModel");
			var tm = TransactionModel.Create(new { name = "World" });
			var instance = Guard.AgainstNull((IContentModel?)Activator.CreateInstance(x.First(), tm, Array.Empty<IPlatformIdentity>()));

			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(instance);
			var engine = new FluidTemplateEngine(new FluidParserOptions { });

			var result = await engine.RenderAsync(template.Object, context.Object);

			Assert.IsNotNull(result);
			Assert.AreEqual(expected, result);
		}

		class TestPlatformIdentity : IPlatformIdentity
		{
			public string? Id { get; set; }
			public string? Name { get; set; }
			public string? Type { get; set; }
			public IReadOnlyCollection<IIdentityAddress> Addresses { get; set; }
			public IReadOnlyCollection<string> ChannelPreferences { get; set; } = [];
		}

		[TestMethod]
		public async Task CanHandleAudienceLists()
		{
			var expected = "Hello World!";
			var templateContent = "Hello {{aud[0].Name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));

			var tm = TransactionModel.Create(new { name = "Not World" });

			var identity = new TestPlatformIdentity() { Name = "World" };

			var contentModelType = typeof(IContentModel).Assembly.GetTypes().Where(t => t.Name == "ContentModel").First();
			var cm = (IContentModel?)Activator.CreateInstance(contentModelType, tm, new List<IPlatformIdentity> { identity });

			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(cm);
			var engine = new FluidTemplateEngine(new FluidParserOptions { });

			var result = await engine.RenderAsync(template.Object, context.Object);

			Assert.IsNotNull(result);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public async Task CanRenderWithNullModel()
		{

			var expected = "Hello !";
			var templateContent = "Hello {{name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			var model = new Mock<IContentModel>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			model.Setup(s => s.Model).Returns(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(model.Object);
			var engine = new FluidTemplateEngine(new FluidParserOptions() { });

			var result = await engine.RenderAsync(template.Object, context.Object);

			Assert.IsNotNull(result);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public async Task ShouldReturnNullWithInvalidTemplate()
		{
			var expected = "Hello !";
			var templateContent = "Hello {{{name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			var model = new Mock<IContentModel>();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			model.Setup(s => s.Model).Returns(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(model.Object);
			var engine = new FluidTemplateEngine(new FluidParserOptions { });

			var result = await engine.RenderAsync(template.Object, context.Object);

			Assert.IsNull(result);
		}
	}
}