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
    public class FormularModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public FormularModule(FormularService formularService)
            : base("/formulare")
        {
            Get["/"] = p =>
            {
                var formulare = formularService.Get();
                return new JsonResponse(formulare, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var formular = formularService.Get(p.id);
                if(formular == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(formular, new JsonNetSerializer());
            };

            //Get["/byUserId/{id}"] = p =>
            //{
            //    var books = bookService.GetByUser(p.id);
            //    return new JsonResponse(books, new JsonNetSerializer());
            //};

            Post["/"] = p =>
            {
                Formular post = this.Bind();
                try
                {
                    var result = formularService.Add(post);
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
                Formular put = this.Bind();
                try
                {
                    var result = formularService.Update(put);
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
                    var result = formularService.Delete(p.id);
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
