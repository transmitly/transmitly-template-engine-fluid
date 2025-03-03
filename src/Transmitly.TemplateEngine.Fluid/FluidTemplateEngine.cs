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
using System.Threading.Tasks;
using Transmitly.Template.Configuration;

namespace Transmitly
{
	internal class FluidTemplateEngine : ITemplateEngine
	{
		private readonly FluidParser _parser;
		public FluidTemplateEngine(FluidParserOptions fluidParserOptions)
		{
			Guard.AgainstNull(fluidParserOptions);
			_parser = new FluidParser(fluidParserOptions);
		}

		public async Task<string?> RenderAsync(IContentTemplateRegistration? registration, IDispatchCommunicationContext? context)
		{
			if (registration == null || context == null)
				return null;
			var source = await registration.GetContentAsync(context);
			if (_parser.TryParse(source, out var template, out var error))
			{
				var templateContext = new TemplateContext(context.ContentModel?.Model ?? new { });
				return await template.RenderAsync(templateContext);
			}
			System.Diagnostics.Debug.WriteLine($"{nameof(FluidTemplateEngine)} {error}");
			return null;
		}
	}
}
