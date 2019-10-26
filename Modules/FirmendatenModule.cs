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
    public class FirmendatenModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public FirmendatenModule(FirmendatenService firmendatenService)
            : base("/firmendaten")
        {
            Get["/"] = p =>
            {
                var daten = firmendatenService.Get();
                return new JsonResponse(daten, new JsonNetSerializer());
            };

            //Get["/{id}"] = p =>
            //{
            //    var daten = firmendatenService.Get(p.id);
            //    if(daten == null)
            //    {
            //        return HttpStatusCode.NotFound;
            //    }
            //    return new JsonResponse(daten, new JsonNetSerializer());
            //};

            //Get["/byUserId/{id}"] = p =>
            //{
            //    var books = bookService.GetByUser(p.id);
            //    return new JsonResponse(books, new JsonNetSerializer());
            //};

            Post["/"] = p =>
            {
                Firmendaten post = this.Bind();
                try
                {
                    var result = firmendatenService.Add(post);
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
                Firmendaten put = this.Bind();
                try
                {
                    var result = firmendatenService.Update(put);
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
                    var result = firmendatenService.Delete(p.id);
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
