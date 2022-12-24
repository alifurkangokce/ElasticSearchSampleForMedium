using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace ElasticSearchSampleForMedium
{
    public class ElasticConnector
    {
        public ElasticClient EsClient()
        {
            Uri uri = new Uri("http://localhost:9200");
            var connectionSettings=new ConnectionSettings(uri).DefaultIndex("medium_blog");
            var elasticClient=new ElasticClient(connectionSettings);
            return elasticClient;
        }
    }
}
