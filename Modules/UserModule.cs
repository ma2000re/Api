using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Services;
using Nancy.Responses;
using Nancy.Serialization.JsonNet;
using Nancy.ModelBinding;
using Common.Models;
using Nancy.Security;

namespace Api.Modules
{
    public class UserModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public UserModule(UserService userService)
            : base("/users")
        {

            this.RequiresAuthentication();

            Get["/"] = p =>
            {
                var users = userService.Get();
                return new JsonResponse(users, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var user = userService.Get(p.id);
                return new JsonResponse(user, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                User post = this.Bind();
                try
                {
                    var result = userService.Add(post);
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
                User put = this.Bind();
                try
                {
                    var result = userService.Update(put);
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
                    var result = userService.Delete(p.id);
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
