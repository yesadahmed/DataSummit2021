using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace V3DurableCore3CrmTemplate.DI
{
	public class DelegationHandler : DelegatingHandler
	{
		private readonly ICrmTokenService _tokenService;

		public DelegationHandler(ICrmTokenService tokenService)
		{
			_tokenService = tokenService;
		}
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _tokenService.GetCRMTokenAsync();//once for session
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			return await base.SendAsync(request, cancellationToken);
		}

		public DelegationHandler()
			: base(new HttpClientHandler()) { }
		public DelegationHandler(HttpMessageHandler innerHandler)
			: base(innerHandler)
		{ }
	}
}
