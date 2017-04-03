using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using samStore.Models;

namespace samStore
{
    internal class CartItemCalculatorAttribute : FilterAttribute, IActionFilter
    {
        public CartItemCalculatorAttribute()
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            List<ProductModel> cart = filterContext.HttpContext.Session["Cart"] as List<ProductModel>;
            if (cart == null)
            {
                cart = new List<ProductModel>();
            }
            filterContext.Controller.ViewBag.ItemsInCart = cart.Count;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}