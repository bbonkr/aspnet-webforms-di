using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsDI.Services
{
    public class SampleInnerService : ISampleInnerService
    {
        public string GetSomeString()
        {
            return "Inner Service";
        }
    }

    public interface ISampleInnerService
    {
        string GetSomeString();
    }
}