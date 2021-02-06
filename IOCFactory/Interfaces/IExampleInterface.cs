using System;
using System.Collections.Generic;
using System.Text;

namespace IOCFactory.Interfaces
{
    interface IExampleInterface
    {
        String Name { get; }

        String GetConfigurationSetting(String setting_name);
        void PerformWork(String setting, object value);
    }
}
