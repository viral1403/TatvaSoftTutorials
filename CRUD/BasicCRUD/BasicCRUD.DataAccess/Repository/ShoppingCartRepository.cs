﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicCRUD.DataAccess.Repository.IRepository;
using BasicCRUD.Models;

namespace BasicCRUD.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>,IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int IncrementCount(ShoppingCart cart, int count)
        {
            cart.Count += count;
            return cart.Count;
        }
        public int DecrementCount(ShoppingCart cart, int count)
        {
            cart.Count -= count;
            return cart.Count;
        }
    }
}
