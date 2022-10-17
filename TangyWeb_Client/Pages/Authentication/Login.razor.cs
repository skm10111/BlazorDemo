using Microsoft.AspNetCore.Components;
using System.Web;
using TangyModels;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Login
    {
		private SignInRequestDTO SignInRequest = new();
		public bool IsProcessing { get; set; } = false;
		public bool ShowSignInErrors { get; set; }
		public string Errors { get; set; }
		[Inject]
		public IAuthenticationService _authenticationService { get; set; }
		[Inject]
		public NavigationManager _navigationManger { get; set; }

		public string ReturnUrl { get; set; }
		private async Task LoginUser()
		{
			ShowSignInErrors = false;
			IsProcessing = true;
			var result = await _authenticationService.Login(SignInRequest);
			if (result.IsAuthSuccessful)
			{
				var absoluteUri = new Uri(_navigationManger.Uri);
				var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
				ReturnUrl = queryParam["returnUrl"];
                if (string.IsNullOrEmpty(ReturnUrl))
                {
					_navigationManger.NavigateTo("/");
                }
                else
                {
					_navigationManger.NavigateTo("/" + ReturnUrl);
				}
				
			}
			else
			{
				Errors = result.ErrorMessage;
				ShowSignInErrors = true;
			}
			IsProcessing = false;
		}
	}
}
