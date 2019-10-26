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
    public class RechnungModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public RechnungModule(RechnungService rechnungService)
            : base("/rechnungen")
        {
            Get["/"] = p =>
            {
                var rechnungen = rechnungService.Get();
                return new JsonResponse(rechnungen, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var rechnung = rechnungService.Get(p.id);
                if(rechnung == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(rechnung, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                Rechnung post = this.Bind();
                try
                {
                    var result = rechnungService.Add(post);
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
                Rechnung put = this.Bind();
                try
                {
                    var result = rechnungService.Update(put);
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
                    var result = rechnungService.Delete(p.id);
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
