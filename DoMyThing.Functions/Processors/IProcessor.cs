using DoMyThing.Functions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMyThing.Functions.Processors
{
    public interface IProcessor<T> where T : RequestModelBase
    {
        public Task ProcessAsync(T request);
    }
}
