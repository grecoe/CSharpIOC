using Complete.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.ClassMapper;

namespace Complete.Classes
{
    /// <summary>
    /// The base wrapper for any ISourceReader interface
    /// </summary>
    /// <typeparam name="TReader">An class deriving from ISourceReader this 
    /// can be IWebReader, ISystemReader or ANY other class that derives ISourceReader</typeparam>
    /// <typeparam name="TScribe">A class that derives IScribe</typeparam>
    class WrappedSourceReader<TReader, TScribe> : ISourceReader
        where TReader : ISourceReader
        where TScribe : IScribe
    {
        #region Supporting Internal Objects for all wrapped ISource Readers
        protected TReader Reader { get; set; }

        protected Mapper<TReader, TScribe> Mapper { get; set; }
        #endregion

        #region ISourceReader - All interface definitions to be routed to base ISourceReader and wrapped.
        public string Location { get { return this.Reader.Location; } }

        public string Name { get { return this.Reader.Name; } }

        public string ReadContent()
        {
            string return_value = String.Empty;
            if(this.Mapper != null)
            {
                return_value = (String)this.Mapper.ExecuteMethod("ReadContent");
            }
            return return_value;
        }
        #endregion
    }
}
