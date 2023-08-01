using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail entity);
    }
}
