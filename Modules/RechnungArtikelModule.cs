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
    public class RechnungArtikelModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public RechnungArtikelModule(RechnungArtikelService rechnungArtikelService)
            : base("/rechnungartikel")
        {
            Get["/"] = p =>
            {
                var rechnungenArtikel = rechnungArtikelService.Get();
                return new JsonResponse(rechnungenArtikel, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var rechnungArtikel = rechnungArtikelService.Get(p.id);
                if(rechnungArtikel == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(rechnungArtikel, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                RechnungArtikel post = this.Bind();
                try
                {
                    var result = rechnungArtikelService.Add(post);
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
                RechnungArtikel put = this.Bind();
                try
                {
                    var result = rechnungArtikelService.Update(put);
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
                    var result = rechnungArtikelService.Delete(p.id);
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
