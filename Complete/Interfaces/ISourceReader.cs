using System;
using System.Collections.Generic;
using System.Text;

namespace Complete.Interfaces
{
    /// <summary>
    /// Base interface type for a reader
    /// </summary>
    interface ISourceReader
    {
        String Name { get; }
        String Location {get;}
        String ReadContent();
    }

    /// <summary>
    /// Differentiate between IWeb and ISystem Readers by using interaces. This solves the issue
    /// with the multiple ISourceReader types being created in the factory so that we only get the one we
    /// are after. 
    /// </summary>
    interface IWebReader : ISourceReader
    {

    }

    /// <summary>
    /// Differentiate between IWeb and ISystem Readers by using interaces. This solves the issue
    /// with the multiple ISourceReader types being created in the factory so that we only get the one we
    /// are after. 
    /// </summary>
    interface ISystemReader : ISourceReader
    {

    }
}
