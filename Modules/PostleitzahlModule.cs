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
    public class PostleitzahlModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public PostleitzahlModule(PostleitzahlService postleitzahlService)
            : base("/postleitzahlen")
        {
            Get["/"] = p =>
            {
                var postleitzahlen = postleitzahlService.Get();
                return new JsonResponse(postleitzahlen, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var postleitzahlen = postleitzahlService.Get(p.id);
                if(postleitzahlen == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(postleitzahlen, new JsonNetSerializer());
            };

        }
    }
}
