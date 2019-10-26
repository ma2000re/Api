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
    public class BestellungModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public BestellungModule(BestellungService bestellungService)
            : base("/bestellungen")
        {
            Get["/"] = p =>
            {
                var bestellungen = bestellungService.Get();
                return new JsonResponse(bestellungen, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var bestellung = bestellungService.Get(p.id);
                if(bestellung == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(bestellung, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                Bestellung post = this.Bind();
                try
                {
                    var result = bestellungService.Add(post);
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
                Bestellung put = this.Bind();
                try
                {
                    var result = bestellungService.Update(put);
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
                    var result = bestellungService.Delete(p.id);
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
