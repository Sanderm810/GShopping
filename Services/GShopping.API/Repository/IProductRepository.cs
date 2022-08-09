using GShopping.API.Data.ValueObjects;

namespace GShopping.API.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductVO>> findAll();
        Task<ProductVO> findById(long id);
        Task<ProductVO> Create(ProductVO vo);
        Task<ProductVO> Update(ProductVO vo);
        Task<bool> Delete(long id);
    }
}
