using Microsoft.JSInterop;

namespace TangyWeb_Client.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }
        public static async ValueTask ToastrError(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr",
      "error", message);
        }
        public static async ValueTask Swal(this IJSRuntime jsRuntime, string message, string type)
        {
            if (type == "success")
            {
                await jsRuntime.InvokeVoidAsync("ShowSwal", "success", message);
            }
            else if (type == "error")
            {
                await jsRuntime.InvokeVoidAsync("ShowSwal", "error", message);
            }

        }
        public static async ValueTask Modal(this IJSRuntime jsRuntime, bool isShow)
        {
            if (isShow)
            {
				await jsRuntime.InvokeVoidAsync("ShowDeleteConfirmationModal");
			}
			else
			{
				await jsRuntime.InvokeVoidAsync("HideDeleteConfirmationModal");
			}
        }
    }
}
