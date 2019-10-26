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
    public class KundenModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public KundenModule(KundenService kundenService)
            : base("/kunden")
        {
            Get["/"] = p =>
            {
                var kunden = kundenService.Get();
                return new JsonResponse(kunden, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var kunden = kundenService.Get(p.id);
                if(kunden == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(kunden, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                Kunden post = this.Bind();
                try
                {
                    var result = kundenService.Add(post);
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
                Kunden put = this.Bind();
                try
                {
                    var result = kundenService.Update(put);
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
                    var result = kundenService.Delete(p.id);
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
