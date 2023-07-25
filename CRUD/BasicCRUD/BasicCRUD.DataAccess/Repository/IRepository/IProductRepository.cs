using BasicCRUD.Models;
namespace BasicCRUD.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product entity);
    }
}
