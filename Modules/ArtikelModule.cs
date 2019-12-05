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
    public class ArtikelModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public ArtikelModule(ArtikelService artikelService, LieferantenService lieferantenService)
            : base("/artikel")
        {
            Get["/"] = p =>
            {
                var artikel = artikelService.Get();
                return new JsonResponse(artikel, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var artikel = artikelService.Get(p.id);
                if (artikel == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(artikel, new JsonNetSerializer());
            };

            //Get["/{aktiv}"] = p =>
            //{
            //    var artikel = artikelService.Get(p.aktiv);
            //    if (artikel == null)
            //    {
            //        return HttpStatusCode.NotFound;
            //    }
            //    return new JsonResponse(artikel, new JsonNetSerializer());
            //};


            Post["/"] = p =>
            {
                Artikel post = this.Bind();
                try
                {
                    var result = artikelService.Add(post);
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
                Artikel put = this.Bind();
                try
                {
                    var result = artikelService.Update(put);
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
                    var result = artikelService.Delete(p.id);
                    return new JsonResponse(result, new DefaultJsonSerializer());
                }
                catch (Exception ex)
                {
                    log.errorLog(ex.Message);
                    return HttpStatusCode.BadRequest;
                }
            };

            Post["/lieferanten/{LieferantenID}"] = p =>
            {
                Lieferanten l = lieferantenService.Get(p.LieferantenID);

                Artikel post = this.Bind();
                post.Lieferant = l;

                l.Artikel.Add(post);

                try
                {
                    var result = artikelService.Add(post);
                }
                catch (Exception ex)
                {
                    log.errorLog(ex.Message);
                    return HttpStatusCode.BadRequest;
                }
                return HttpStatusCode.Created;

            };
        }
    }
}
