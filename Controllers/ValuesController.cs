using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dating.api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Dating.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _datacontext;

        public ValuesController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }
        // GET api/values 
        [HttpGet]
        public IActionResult GetValues()
        {
            var values = _datacontext.Values.ToList();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetValue(int id)
        {
            var value = _datacontext.Values.FirstOrDefault(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
