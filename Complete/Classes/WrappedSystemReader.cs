using System;
using System.Collections.Generic;
using System.Text;
using Complete.Interfaces;
using Utils.ClassMapper;

namespace Complete.Classes
{
    /// <summary>
    /// The outer wrapper for an ISystemReader object. Derives from WrappedSourceReader 
    /// which can take any ISourceReader and will wrap each function call to the interface
    /// with logging and timing.
    /// </summary>
    /// <typeparam name="TReader">An instance of a class deriving from ISystemReader</typeparam>
    /// <typeparam name="TScribe">An instance of a class deriving from IScribe</typeparam>
    class WrappedSystemReader<TReader, TScribe> : WrappedSourceReader<TReader, TScribe>, ISourceReader
        where TReader : ISystemReader
        where TScribe : IScribe
    {
        public WrappedSystemReader(TReader reader, TScribe scribe)
        {
            // Set base class variables so that all interface calls get routed and logged.
            this.Reader = reader;
            this.Mapper = new Mapper<TReader, TScribe>(reader, scribe);
        }
    }
}
