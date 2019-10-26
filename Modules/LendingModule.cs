using Common.Models;
using Common.Services;
using Nancy;
using Nancy.Responses;
using Nancy.Serialization.JsonNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Api.Modules
{
    public class LendingModule : NancyModule
    {
        ErrorLogFile log = Program.log;

        public LendingModule(LendingService lendingService, UserService userService, BookService bookService)
            : base("/lendings")
        {
            //Passwortabsicherung des Moduls
            this.RequiresAuthentication();

            Get["/"] = p =>
            {
                var lendings = lendingService.Get();
                return new JsonResponse(lendings, new JsonNetSerializer());
            };

            Get["/{id}"] = p =>
            {
                var lending = lendingService.Get(p.id);
                return new JsonResponse(lending, new JsonNetSerializer());
            };

            Post["/"] = p =>
            {
                Lending post = this.Bind();
                try
                {
                    var result = lendingService.Add(post);
                }
                catch (Exception ex)
                {
                    log.errorLog(ex.Message);
                    return HttpStatusCode.BadRequest;
                }
                return HttpStatusCode.Created;
            };

            Post["/user/{uid}/book/{bid}"] = p =>
            {
                User u = userService.Get(p.uid);
                Book b = bookService.Get(p.bid);

                Lending post = this.Bind();
                post.Book = b;
                post.User = u;

                u.Lendings.Add(post);
                b.Lendings.Add(post);

                try
                {
                    var result = lendingService.Add(post);
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
                Lending put = this.Bind();
                try
                {
                    var result = lendingService.Update(put);
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
                    var result = lendingService.Delete(p.id);
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
