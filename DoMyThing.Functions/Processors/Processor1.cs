using DoMyThing.Functions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Functions.Processors
{
    public class Processor1 : IProcessor<Model1>
    {
        public void Process(Model1 request)
        {
            throw new NotImplementedException();
        }
    }
}
