using Blazored.LocalStorage;
using Microsoft.JSInterop;
using Tangy_Common;
using TangyWeb_Client.Service.IService;
using TangyWeb_Client.ViewModels;


namespace TangyWeb_Client.Service
{
    public class CardService : ICardService
    {
        private readonly ILocalStorageService _localStorageService;
        public event Action OnChange;

        public CardService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;

        }
        public async Task DecrementCart(ShoppingCart shoppingCart)
        {
            Console.WriteLine(shoppingCart.Count);
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);

            for (int i = 0; i < cart.Count; i++)
            {

                if (cart[i].ProductId == shoppingCart.ProductId && cart[i].ProductPriceId == shoppingCart.ProductPriceId)
                {

                    if (shoppingCart.Count == 0 || cart[i].Count == 1)
                    {
                        cart.Remove(cart[i]);

                    }
                    else
                    {
                        cart[i].Count -= shoppingCart.Count;
                    }
                }
            }
            await _localStorageService.SetItemAsync(SD.ShoppingCart, cart);
            OnChange.Invoke();
        }
        public async Task IncrementCart(ShoppingCart shoppingCart)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemInCart = false;
            if (cart == null)
            {
                cart = new List<ShoppingCart>();
            }
            foreach (var obj in cart)
            {
                if (obj.ProductId == shoppingCart.ProductId && obj.ProductPriceId == shoppingCart.ProductPriceId)
                {
                    itemInCart = true;
                    obj.Count += shoppingCart.Count;
                }
            }
            if (!itemInCart)
            {
                cart.Add(new ShoppingCart()
                {
                    ProductId = shoppingCart.ProductId,
                    ProductPriceId = shoppingCart.ProductPriceId,
                    Count = shoppingCart.Count
                });
            }
            await _localStorageService.SetItemAsync(SD.ShoppingCart, cart);
            OnChange.Invoke();
        }
        public  async Task UpdateCart()
        {
            OnChange.Invoke();
        }
    }
}
