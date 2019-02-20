using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Repositories;
using Framework;
using Framework.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/items")]
    public class ItemsController : BaseController
    {
        IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet()]
        public IActionResult GetItems()
        {
            var itemId = Request.Query["itemId"].ToNullableInt();

            var predicate = PredicateBuilder.True<Item>();

            if (itemId.HasValue)
            {
                predicate = predicate.And(x => x.Id == itemId.Value);
            }

            var items = this._itemRepository.GetBy(predicate);

            var results = Mapper.Map<IEnumerable<ItemWithoutItemStepsDto>>(items);

            return Ok(results);
        }
    }
}
