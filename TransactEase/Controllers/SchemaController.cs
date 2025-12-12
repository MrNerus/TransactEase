using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/schema")]
    public class SchemaController : ControllerBase
    {
        private readonly SchemaService _schemaService;

        public SchemaController(SchemaService schemaService)
        {
            _schemaService = schemaService;
        }

        [HttpGet("table/{name}")]
        public IActionResult GetTableSchema(string name)
        {
            var schema = _schemaService.GetTableSchema(name);
            if (schema == null)
            {
                return NotFound(new { message = $"Table schema '{name}' not found." });
            }
            return Ok(schema);
        }

        [HttpGet("form/{name}")]
        public IActionResult GetFormSchema(string name)
        {
            var schema = _schemaService.GetFormSchema(name);
            if (schema == null)
            {
                return NotFound(new { message = $"Form schema '{name}' not found." });
            }
            return Ok(schema);
        }
    }
}
