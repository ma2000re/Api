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
    public class LieferantenModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public LieferantenModule(LieferantenService lieferantenService)
            : base("/lieferanten")
        {
            Get["/"] = p =>
            {
                var lieferanten = lieferantenService.Get();
                return new JsonResponse(lieferanten, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var lieferanten = lieferantenService.Get(p.id);
                if (lieferanten == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(lieferanten, new JsonNetSerializer());
            };

            //Get["/{Firma}"] = p =>
            //{
            //    var lieferanten = lieferantenService.GetByFirma(p.Firma);
            //    if(lieferanten==null)
            //    {
            //        return HttpStatusCode.NotFound;
            //    }
            //    return new JsonResponse(lieferanten, new JsonNetSerializer());

            //};

            Post["/"] = p =>
            {
                Lieferanten post = this.Bind();
                try
                {
                    var result = lieferantenService.Add(post);
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
                Lieferanten put = this.Bind();
                try
                {
                    var result = lieferantenService.Update(put);
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
                    var result = lieferantenService.Delete(p.id);
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
