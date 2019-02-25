using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebFormsDI.Services;

namespace WebFormsDI
{
    public class PageBase : Page
    {
        public ISampleService sampleService { get; set; }
    }
}