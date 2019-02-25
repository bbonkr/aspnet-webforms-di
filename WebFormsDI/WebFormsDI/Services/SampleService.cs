using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsDI.Services
{
    public class SampleService : ISampleService
    {
        public SampleService(ISampleInnerService sampleInnerService)
        {
            this.sampleInnerService = sampleInnerService;
        }

        public string GetString()
        {
            return $"Hello, World! {sampleInnerService.GetSomeString()}";
        }

        ISampleInnerService sampleInnerService;
    }

    public interface ISampleService
    {
        string GetString();
    }
}