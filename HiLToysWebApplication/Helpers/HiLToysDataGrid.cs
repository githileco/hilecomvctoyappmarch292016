using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using HiLToysWebControls;

namespace HiLToysWebApplication.Helpers
{
    public static class HiLToysDataGrid
    {
        /// <summary>
        /// Generate Paged Data Grid
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString RenderHiLToysDataGrid(this HtmlHelper html, HiLToysWebControls.HiLToysDataGrid dataGrid)
        {

            string control = dataGrid.CreateControl();

            return MvcHtmlString.Create(control);

        }

    }
      
}