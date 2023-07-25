using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> ,ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db =db;
        }
        public void Update(Category entity)
        {
            dbSet.Update(entity);
        }
    }
}
