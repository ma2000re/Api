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
    public class BestellungArtikelModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public BestellungArtikelModule(BestellungArtikelService bestellungArtikelService)
            : base("/bestellungartikel")
        {
            Get["/"] = p =>
            {
                var bestellungArtikel = bestellungArtikelService.Get();
                return new JsonResponse(bestellungArtikel, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var bestellungArtikel = bestellungArtikelService.Get(p.id);
                if(bestellungArtikel == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(bestellungArtikel, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                BestellungArtikel post = this.Bind();
                try
                {
                    var result = bestellungArtikelService.Add(post);
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
                BestellungArtikel put = this.Bind();
                try
                {
                    var result = bestellungArtikelService.Update(put);
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
                    var result = bestellungArtikelService.Delete(p.id);
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
