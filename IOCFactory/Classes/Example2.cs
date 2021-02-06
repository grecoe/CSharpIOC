using System;
using System.Collections.Generic;
using System.Text;
using IOCFactory.Interfaces;

namespace IOCFactory.Classes
{
    class Example2 : IExampleInterface
    {
        public String Name { get; set; }

        public Example2()
        {
            this.Name = "Example2";
        }

        public string GetConfigurationSetting(string setting_name)
        {
            return $"{this.Name} -> Setting[{setting_name}]";
        }

        public void PerformWork(string setting, object value)
        {
            Console.Write($"{this.Name} -> Working on {setting}, {value.ToString()}");
        }

    }
}
