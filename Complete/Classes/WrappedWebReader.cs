using Complete.Interfaces;
using Utils.ClassMapper;

namespace Complete.Classes
{
    /// <summary>
    /// The outer wrapper for an IWebReader object. Derives from WrappedSourceReader 
    /// which can take any ISourceReader and will wrap each function call to the interface
    /// with logging and timing.
    /// </summary>
    /// <typeparam name="TReader">An instance of a class deriving from IWebReader</typeparam>
    /// <typeparam name="TScribe">An instance of a class deriving from IScribe</typeparam>
    class WrappedWebReader<TReader, TScribe> : WrappedSourceReader<TReader, TScribe>, ISourceReader
        where TReader: IWebReader
        where TScribe : IScribe
    {
        public WrappedWebReader(TReader reader, TScribe scribe)
        {
            // Set base class variables so that all interface calls get routed and logged.
            this.Reader = reader;
            this.Mapper = new Mapper<TReader, TScribe>(reader, scribe);
        }
    }
}
