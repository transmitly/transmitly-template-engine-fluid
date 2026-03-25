// Copyright (c) Code Impressions, LLC. All Rights Reserved.
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
using System;
using Transmitly.Template.Configuration;
using Transmitly.Util;

namespace Transmitly
{
	/// <summary>
	/// Provides extension methods for registering and resolving the Fluid template engine.
	/// </summary>
	public static class FluidTemplateEngineExtensions
	{
		private const string FluidId = "Fluid";

		/// <summary>
		/// Gets the template engine identifier for the Fluid template engine.
		/// </summary>
		/// <param name="templateEngines">The template engine identifier builder.</param>
		/// <param name="providerId">An optional provider identifier to scope the template engine identifier.</param>
		/// <returns>The template engine identifier for the Fluid template engine.</returns>
		public static string Fluid(this TemplateEngines templateEngines, string? providerId = null)
		{
			Guard.AgainstNull(templateEngines);

			return templateEngines.GetId(FluidId, providerId);
		}

		/// <summary>
		/// Registers the Fluid template engine using the provided parser configuration.
		/// </summary>
		/// <param name="templateConfiguration">The template configuration builder to update.</param>
		/// <param name="options">Configures the <see cref="FluidParserOptions" /> used by the Fluid parser.</param>
		/// <param name="templateEngineId">An optional template engine identifier override.</param>
		/// <returns>The communications client builder.</returns>
		public static CommunicationsClientBuilder AddFluidTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, Action<FluidParserOptions> options, string? templateEngineId = null)
		{
			Guard.AgainstNull(templateConfiguration);
			Guard.AgainstNull(options);

			var opts = new FluidParserOptions();
			options(opts);
			return templateConfiguration.Add(new FluidTemplateEngine(opts), Id.TemplateEngines.Fluid(templateEngineId));
		}

		/// <summary>
		/// Registers the Fluid template engine with default parser options.
		/// </summary>
		/// <param name="templateConfiguration">The template configuration builder to update.</param>
		/// <param name="templateEngineId">An optional template engine identifier override.</param>
		/// <returns>The communications client builder.</returns>
		public static CommunicationsClientBuilder AddFluidTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, string? templateEngineId = null)
		{
			return AddFluidTemplateEngine(templateConfiguration, (opts) => { }, templateEngineId);
		}

		/// <summary>
		/// Registers the Fluid template engine on the communications client builder with default parser options.
		/// </summary>
		/// <param name="communicationsClientBuilder">The communications client builder to update.</param>
		/// <returns>The communications client builder.</returns>
		public static CommunicationsClientBuilder AddFluidTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder)
		{
			return AddFluidTemplateEngine(communicationsClientBuilder.TemplateEngine, (opts) => { });
		}

		/// <summary>
		/// Registers the Fluid template engine on the communications client builder using the provided parser configuration.
		/// </summary>
		/// <param name="communicationsClientBuilder">The communications client builder to update.</param>
		/// <param name="options">Configures the <see cref="FluidParserOptions" /> used by the Fluid parser.</param>
		/// <returns>The communications client builder.</returns>
		public static CommunicationsClientBuilder AddFluidTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder, Action<FluidParserOptions> options)
		{
			return AddFluidTemplateEngine(communicationsClientBuilder.TemplateEngine, options, Id.TemplateEngines.Fluid());
		}
	}
}
