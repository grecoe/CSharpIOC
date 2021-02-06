using System;
using Complete.Interfaces;

namespace Complete.Classes
{
    /// <summary>
    /// Concrete Implementation of the ISystemReader
    /// </summary>
    class SystemReader : ISystemReader
    {
        public string Name { get; private set; }
        public string Location { get; private set; }

        public SystemReader(String location)
        {
            this.Name = "SystemReader";
            this.Location = location;
        }

        public string ReadContent()
        {
            return $"{this.Name} read from {this.Location}";
        }
    }

}
