using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsDI.Services;

namespace WebFormsDI
{
    public partial class _Default : PageBase
    {
        public INotImplService notImplService;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = sampleService.GetString();
        }
    }
}