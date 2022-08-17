using GShopping.CartAPI.Data.ValueObjects;

namespace GShopping.CartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartVO> FindCartByUserId(string id);
        Task<CartVO> SaveOrUpdateCart (CartVO vo);
        Task<bool> RemoveFromCart (long cartDetailsId);
        Task<bool> ApplyCoupon (string userId, string couponCode);
        Task<bool> RemoveCoupon (string userId);
        Task<bool> ClearCart (string userId);
    }
}
