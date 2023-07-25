using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product entity)
        {
            var prodObj = _db.Products.FirstOrDefault(p => p.Id == entity.Id);
            if (prodObj != null)
            {
                prodObj.Title = entity.Title;
                prodObj.Description = entity.Description;
                prodObj.ISBN = entity.ISBN;
                prodObj.Author = entity.Author;
                prodObj.ListPrice = entity.ListPrice;
                prodObj.Price = entity.Price;
                prodObj.Price50 = entity.Price50;
                prodObj.Price100 = entity.Price100;
                prodObj.ImageUrl = entity.ImageUrl;
                prodObj.CategoryId = entity.CategoryId;
                prodObj.CoverTypeId = entity.CoverTypeId;
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    prodObj.ImageUrl = entity.ImageUrl;
                }
            }
            dbSet.Update(prodObj);
        }
    }
}
