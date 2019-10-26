using Common.Services;
using Nancy;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ModelBinding;
using Common.Models;
using Nancy.Serialization.JsonNet;
using Nancy.Security;

namespace Api.Modules
{
    public class NewsModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public NewsModule(NewsService newsService)
            : base("/news")
        {
            Get["/"] = p =>
            {
                var news = newsService.Get();
                return new JsonResponse(news, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var news = newsService.Get(p.id);
                if(news == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(news, new JsonNetSerializer());
            };

            //Get["/byUserId/{id}"] = p =>
            //{
            //    var books = bookService.GetByUser(p.id);
            //    return new JsonResponse(books, new JsonNetSerializer());
            //};

            Post["/"] = p =>
            {
                News post = this.Bind();
                try
                {
                    var result = newsService.Add(post);
                }
                catch (Exception ex)
                {
                    log.errorLog(ex.Message);
                    return HttpStatusCode.BadRequest;
                }
                return HttpStatusCode.Created;
            };

            Put["/"] = p =>
            {
                News put = this.Bind();
                try
                {
                    var result = newsService.Update(put);
                }
                catch (Exception ex)
                {
                    log.errorLog(ex.Message);
                    return HttpStatusCode.BadRequest;
                }
                return HttpStatusCode.OK;
            };

            Delete["/{id}"] = p =>
            {
                try
                {
                    var result = newsService.Delete(p.id);
                    return new JsonResponse(result, new DefaultJsonSerializer());
                }
                catch (Exception ex)
                {
                    log.errorLog(ex.Message);
                    return HttpStatusCode.BadRequest;
                }
            };
        }
    }
}
