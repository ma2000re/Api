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
    public class LoginModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public LoginModule(LoginService loginService)
            : base("/logins")
        {
            Get["/"] = p =>
            {
                var logins = loginService.Get();
                return new JsonResponse(logins, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var login = loginService.Get(p.id);
                if(login == null)
                {
                    return HttpStatusCode.NotFound;
                }
                return new JsonResponse(login, new JsonNetSerializer());
            };

            //Get["/byUserId/{id}"] = p =>
            //{
            //    var books = loginService.GetByUser(p.id);
            //    return new JsonResponse(login, new JsonNetSerializer());
            //};

            Post["/"] = p =>
            {
                Login post = this.Bind();
                try
                {
                    var result = loginService.Add(post);
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
                Login put = this.Bind();
                try
                {
                    var result = loginService.Update(put);
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
                    var result = loginService.Delete(p.id);
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
