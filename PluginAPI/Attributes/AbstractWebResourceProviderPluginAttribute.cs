using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginAPI.Attributes
{
   
    public abstract class AbstractWebResourceProviderPluginAttribute : Attribute
    {
        private string _baseVirtualPath;

        public AbstractWebResourceProviderPluginAttribute(string baseVirtualPath)
        {
            this._baseVirtualPath = baseVirtualPath;
        }

        public string BaseVirtualPath
        {
            get
            {
                return _baseVirtualPath;
            }
        }
    }

    
}
