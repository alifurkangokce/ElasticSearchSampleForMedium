using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchSampleForMedium
{
    public class Elastic
    {
        private readonly ElasticConnector _elasticConnector;
        public Elastic()
        {
            _elasticConnector = new ElasticConnector();
        }

        // Index Oluşturma
        public JsonResult AddIndex()
        {
            var response = _elasticConnector.EsClient().Indices.Create("medium_blog",
                index=>index.Map<MediumBlog>(x=>x.AutoMap())
                );
            return new JsonResult(response);
        }
        // Veri Ekleme
        public JsonResult InsertData()
        {
            var mediumPost = new MediumBlog()
            {
                PostDate = DateTime.Now,
                PostText = "ElastiSearch ile ilgili ufak çaplı bilgiler.",
                PostTitle = "ElasticSearch Nedir?"
            };
            var result = _elasticConnector.EsClient().Index(new IndexRequest<MediumBlog>(mediumPost));
            return new JsonResult(result);
        }

        // Veri Güncelleme
        public JsonResult UpdateData()
        {
            var mediumPost = new MediumBlog()
            {
                PostDate = DateTime.Now,
                PostText = "ElastiSearch ile ilgili ufak çaplı bilgiler update .",
                PostTitle = "ElasticSearch Nedir Update ?"
            };
            var result = _elasticConnector.EsClient().Update<MediumBlog>("id", x => x.Index("medium_blog").Doc(mediumPost));
            return new JsonResult(result);
        }


        // İçinde blog geçen alanları bulma
        public JsonResult PerformTermQuery()
        {
            var result = _elasticConnector.EsClient().Search<MediumBlog>(
                s => s.Query(
                    p => p.Term(
                        q => q.PostText, "ElastiSearch ile ilgili ufak çaplı bilgiler."
                        )
                    )
                );
            return new JsonResult(result);
        }


        // Tam cümle ile arama
        public JsonResult PerformMatchPhrase()
        {
            var result = _elasticConnector.EsClient().Search<MediumBlog>(
                s => s.Query(
                    q => q.MatchPhrase(
                        m => m.Field(f => f.PostText)
                            .Query("ufak çaplı"))));
            return new JsonResult(result);
        }

        // Filter kullanım => PostDate'i şuandan küçük olanları getirme
        public JsonResult PerformFilter()
        {
            var result = _elasticConnector.EsClient().Search<MediumBlog>(
                q => q.Query(
                    s => s.Term(
                        p => p.PostText, "ufak"
                        )
                    ).PostFilter(f => f.DateRange(c => c.Field(f => f.PostDate).LessThan(DateMath.Now)))
                );
            return new JsonResult(result);
        }


    }
}
