using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Resources.Locallizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonsHandBook.Services
{
    public class ValidationActionFilterAttribute : IActionFilter
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ValidationActionFilterAttribute(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.SingleOrDefault();

            if(param.Value == null)
            {
                context.Result = new BadRequestObjectResult(Constants.ObjectNull);
                return;
            }

            if (context.ModelState.IsValid == false)
                context.Result = new BadRequestObjectResult(context.ModelState);

        }
    }


    public class ModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = (from modelState in context.ModelState.Values from error in modelState.Errors select error.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(errors);
            }

            base.OnActionExecuting(context);
        }
    }

}
