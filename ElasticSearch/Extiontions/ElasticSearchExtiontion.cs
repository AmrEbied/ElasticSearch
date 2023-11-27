using ElasticSearch.Models;
using Nest;

namespace ElasticSearch.Extiontions
{
    public static class ElasticSearchExtiontion
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            // Configration from appSetting
            var url = configuration["ELKConfiguration:Uri"];
            var defultIndex = configuration["ELKConfiguration:index"];
            var setting = new ConnectionSettings(new Uri(url)).PrettyJson().DefaultIndex(defultIndex);
            // Choose table that  mapping 
            AddDefultMapping(setting);
            var client = new ElasticClient(setting);
            services.AddSingleton<IElasticClient>(client);
            //Create New Index dependon Configration
            CreateIndex(client, defultIndex);
        }
        private static void AddDefultMapping(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Product>(p =>
            p.Ignore(x => x.Price)
             .Ignore(x => x.Qty)
             .Ignore(x => x.Id));
        }
        private static void CreateIndex(IElasticClient client,string indexName)
        {
            client.Indices.Create(indexName,i=>i.Map<Product>(x=>x.AutoMap()));
        }
    }
}
