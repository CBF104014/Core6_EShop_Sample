using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;
using Core6_EShop.ViewModel;
using System.Data;

namespace Core6_EShop.Service.Implement
{
    public class CartService : BaseService<Cart>
    {
        private CartRepository cartRepository { get; set; }
        public CartService(CartRepository cartRepository) : base(cartRepository)
        {
            this.cartRepository = cartRepository;
        }
        public Cart GetSampleData(int memberId = 0, int gId = 0, int sizeId = 0, int quantity = 0)
        {
            return this.cartRepository.GetSampleData(memberId, gId, sizeId, quantity);
        }
        public Task<IEnumerable<CartViewModel>> SelToViewModelById(int memberId)
        {
            return this.cartRepository.SelToViewModelById(memberId);
        }
        public Task<IEnumerable<CartViewModel>> SelPreOrderById(int memberId)
        {
            return this.cartRepository.SelPreOrderById(memberId);
        }
        public async Task<int> AddToCart(Cart cartData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await this.cartRepository.AddToCart(cartData, conn, tran);
        }
        public async Task<int> UpdCart(Cart cartData, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await this.cartRepository.UpdCart(cartData, conn, tran);
        }
        public async Task<int> UpdCartChecked(int memberId, bool isChecked, IDbConnection conn = null, IDbTransaction tran = null) 
        {
            return await this.cartRepository.UpdCartChecked(memberId, isChecked, conn, tran);
        }
        public async Task<int> DelCart(long rankey, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return await this.cartRepository.DelByRankey(rankey, conn, tran);
        }
    }
}
