using System;
using Complete.Interfaces;

namespace Complete.Classes
{
    /// <summary>
    /// Concrete implmentation of the IWebReader
    /// </summary>
    class WebReader: IWebReader
    {
        public string Name { get; private set; }
        public string Location { get; private set; }

        public WebReader(String location)
        {
            this.Name = "WebReader";
            this.Location = location;
        }


        public string ReadContent()
        {
            return $"{this.Name} read from {this.Location}";
        }
    }
}
