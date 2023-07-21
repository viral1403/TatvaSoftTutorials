using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository
{
    public class CoverTypeRepository :Repository<CoverType>, ICoverTypeRepository
    {
        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
