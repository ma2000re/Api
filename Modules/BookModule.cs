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
    public class BookModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public BookModule(BookService bookService)
            : base("/books")
        {
            Get["/"] = p =>
            {
                var books = bookService.Get();
                return new JsonResponse(books, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var book = bookService.Get(p.id);
                if(book == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(book, new JsonNetSerializer());
            };

            Get["/byUserId/{id}"] = p =>
            {
                var books = bookService.GetByUser(p.id);
                return new JsonResponse(books, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                Book post = this.Bind();
                try
                {
                    var result = bookService.Add(post);
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
                Book put = this.Bind();
                try
                {
                    var result = bookService.Update(put);
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
                    var result = bookService.Delete(p.id);
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
