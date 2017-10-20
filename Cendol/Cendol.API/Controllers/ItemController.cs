using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cendol.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cendol.API.Controllers
{
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private IBasicRepositoryAsync<InputItem> _repoInputItem;

        public ItemController(IBasicRepositoryAsync<InputItem> repoInputItem)
        {
            _repoInputItem = repoInputItem;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Json(await _repoInputItem.List().ToAsyncEnumerable().ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var item = await _repoInputItem.GetByIdAsync(id);
                return Json(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InputItem item)
        {
            try
            {
                _repoInputItem.Insert(item);
                item.CreatedOn = DateTime.Now;
                await _repoInputItem.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
