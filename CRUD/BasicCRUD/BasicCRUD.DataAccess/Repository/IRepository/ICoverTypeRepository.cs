using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType entity);
    }
}
