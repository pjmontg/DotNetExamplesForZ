using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginAPI.VPP;
using System.Reflection;
using ServicesAPI.Plugin;

namespace PluginAPI.Base
{
    public abstract class AbstractWebResourceProviderPlugin : AbstractPlugin, IWebResourceProviderPlugin
    {
        protected WebResourceVirtualPathProvider _vpp;

        public abstract WebResourceVirtualPathProvider VirtualPathProvider { get; }


        
    }
}
