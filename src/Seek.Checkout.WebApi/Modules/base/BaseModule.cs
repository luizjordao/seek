using Nancy;
using Seek.Infrastructure.Exceptions;
using System;

namespace Seek.Checkout.WebApi.Modules.Base
{
    public abstract class BaseModule : NancyModule
    {
        protected BaseModule(string modulePath) : base(modulePath)
        {             
        }

        protected dynamic HandleError(Func<dynamic> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (ResourceNotFoundException)
            {
                return HttpStatusCode.NotFound;
            }           
            catch (Exception exception)
            {
                var model = new[] {  exception.Message, exception.InnerException.ToString() };                

                return Response.AsJson(model, HttpStatusCode.InternalServerError);
            }
        }

    }
}