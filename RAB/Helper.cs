using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RAB
{
    public class Helper
    {
        /*
         * diperoleh dari youtube :
         * tanggal 31 Okt 2020 01:34
         */
        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContex = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                    );
                viewResult.View.RenderAsync(viewContex);
                return sw.GetStringBuilder().ToString();
            }
        }

        //untuk menghindari aksess langsung dari alamat link
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class TidakBolehAksesLangsungAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                base.OnActionExecuting(context);
                if(context.HttpContext.Request.GetTypedHeaders().Referer == null ||
                    context.HttpContext.Request.GetTypedHeaders().Host.ToString() != 
                    context.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString()
                    )
                {
                    context.HttpContext.Response.Redirect("/");
                }
            }
        }
    }
}
