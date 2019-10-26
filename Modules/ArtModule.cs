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
    public class ArtModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public ArtModule(ArtService artService)
            : base("/arts")
        {
            Get["/"] = p =>
            {
                var art = artService.Get();
                return new JsonResponse(art, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var art = artService.Get(p.id);
                if(art == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(art, new JsonNetSerializer());
            };

            //Get["/byUserId/{id}"] = p =>
            //{
            //    var books = artService.GetByUser(p.id);
            //    return new JsonResponse(arts, new JsonNetSerializer());
            //};

            Post["/"] = p =>
            {
                Art post = this.Bind();
                try
                {
                    var result = artService.Add(post);
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
                Art put = this.Bind();
                try
                {
                    var result = artService.Update(put);
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
                    var result = artService.Delete(p.id);
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
