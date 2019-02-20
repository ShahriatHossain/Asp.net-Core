using Core.Entities;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Persistence.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(TailoryfyDbContext context) : base(context)
        {

        }

        public override IEnumerable<Item> GetBy(Expression<Func<Item, bool>> predicate)
        {
            return SafeCall(() => this.Context.Items.Where(predicate));
        }
    }
}
