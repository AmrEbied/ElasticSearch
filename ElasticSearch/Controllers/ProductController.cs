using ElasticSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IElasticClient _elasticClient;

        public ProductController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet(Name = "GetProduct")]
        public async Task<IActionResult> Get(string keyWord)
        {
            var result = await _elasticClient.SearchAsync<Product>(
                s => s.Query(
                    q => q.QueryString(
                        d => d.Query('*' + keyWord + '*')
                        )
                    ).Size(1000)
                );
            return Ok(result.Documents.ToList());
        }
        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> Post(Product product)
        {
            await _elasticClient.IndexDocumentAsync(product);
            return Ok();
        }
    }
}