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
using System;
using Transmitly.Template.Configuration;
using Transmitly.Util;

namespace Transmitly
{
	public static class FluidTemplateEngineExtensions
	{
		private const string FluidId = "Fluid";

		public static string Fluid(this TemplateEngines templateEngines, string? providerId = null)
		{
			Guard.AgainstNull(templateEngines);

			return templateEngines.GetId(FluidId, providerId);
		}

		public static CommunicationsClientBuilder AddFluidTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, Action<FluidParserOptions> options, string? templateEngineId = null)
		{
			Guard.AgainstNull(templateConfiguration);
			Guard.AgainstNull(options);

			var opts = new FluidParserOptions();
			options(opts);
			return templateConfiguration.Add(new FluidTemplateEngine(opts), Id.TemplateEngines.Fluid(templateEngineId));
		}

		public static CommunicationsClientBuilder AddFluidTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, string? templateEngineId = null)
		{
			return AddFluidTemplateEngine(templateConfiguration, (opts) => { }, templateEngineId);
		}

		public static CommunicationsClientBuilder AddFluidTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder)
		{
			return AddFluidTemplateEngine(communicationsClientBuilder.TemplateEngine, (opts) => { });
		}

		public static CommunicationsClientBuilder AddFluidTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder, Action<FluidParserOptions> options)
		{
			return AddFluidTemplateEngine(communicationsClientBuilder.TemplateEngine, options, Id.TemplateEngines.Fluid());
		}
	}
}