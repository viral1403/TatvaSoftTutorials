using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetail>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
        }
        public void Update(OrderDetail entity)
        {
            dbSet.Update(entity);
        }
    }
}
