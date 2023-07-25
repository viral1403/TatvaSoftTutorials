using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Add(Category entity);

        void Remove(Category entity);

        void Update(Category entity);

    }
}
