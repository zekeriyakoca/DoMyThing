using DoMyThing.Functions.Models;
using DoMyThing.Functions.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Functions
{
    public class ProcessorFactory
    {
        public ProcessorFactory()
        { }
        public IProcessor<T> GetProcessor<T>() where T : RequestModelBase
        {
            return default;

        }
    }
}
