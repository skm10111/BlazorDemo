using Microsoft.AspNetCore.Components;
using TangyModels;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication
{
    public partial class Register
    {
		//For adding this write filename and after . and then razor.cs (ex: Register.razor.cs)
		private SignUpRequestDTO SignUpRequest = new();
		public bool IsProcessing { get; set; } = false;
		public bool ShowRegistrationErrors { get; set; }
		public IEnumerable<string> Errors { get; set; }
		[Inject]
		public IAuthenticationService _authenticationService { get; set; }
		[Inject]
		public NavigationManager _navigationManger { get; set; }
		private async Task RegisterUser()
		{
			ShowRegistrationErrors = false;
			IsProcessing = true;
			var result = await _authenticationService.RegisterUser(SignUpRequest);
			if (result.IsRegisterationSuccessful)
			{
				_navigationManger.NavigateTo("/login");
			}
			else
			{
				Errors = result.Errors;
				ShowRegistrationErrors = true;
			}
			IsProcessing = false;
		}
	}
}
