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
        public IProcessor<T, TResult> GetProcessor<T, TResult>() where T : RequestModelBase
                                                               where TResult : ResponseModelBase
        {
            throw new NotImplementedException();
            //(IProcessor<T>) new DownloadSubtitleProcessor();

        }
    }
}
