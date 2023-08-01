using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderHeader entity)
        {
            dbSet.Update(entity);
        }
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(u=>u.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id,string sessionId,string PaymentIntentId)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(u=>u.Id==id);
            orderFromDb.SessionId = sessionId;
            orderFromDb.PaymentIntentId = PaymentIntentId;
            orderFromDb.PaymentDate = DateTime.Now;

            dbSet.Update(orderFromDb);
        }
    }
}
