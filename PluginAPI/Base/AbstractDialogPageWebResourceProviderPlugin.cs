using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginAPI.VPP;
using System.Reflection;
using PluginAPI.Attributes;
using ServicesAPI.Plugin;
using PluginAPI.Command;
using PluginAPI.Exception;
using Unity.Extensions;
using Logging;
using System.Data;
using ServicesAPI.ViewModels;
using Microsoft.Practices.Unity;

namespace PluginAPI.Base
{
    public class AbstractDialogPageWebResourceProviderPlugin : AbstractWebResourceProviderPlugin
    {
        public override WebResourceVirtualPathProvider VirtualPathProvider
        {

            get
            {
                if (null == this._vpp)
                {
                    Assembly asm = Assembly.GetAssembly(this.GetType());
                    WebResourceProviderPluginGenericResourceAttribute[] resources = (WebResourceProviderPluginGenericResourceAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginGenericResourceAttribute));
                    WebResourceProviderPluginDialogPageAttribute[] dialogs = (WebResourceProviderPluginDialogPageAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginDialogPageAttribute));
                    IList<SiteMapNodeDefinition> nodes = new List<SiteMapNodeDefinition>();

                    HashSet<string> resourceSet = new HashSet<string>();

                   
                    foreach (WebResourceProviderPluginGenericResourceAttribute resource in resources)
                    {
                        resourceSet.Add(resource.Url);
                    }

                    foreach (WebResourceProviderPluginDialogPageAttribute dialog in dialogs)
                    {
                        resourceSet.Add(dialog.Url);
                    }

                    



                    this._vpp = new WebResourceVirtualPathProvider(asm, dialogs[0].BaseVirtualPath, resourceSet);
                }
                return this._vpp;
            }
        }

        protected override void RegisterStateCommand(IPluginService pluginService)
        {
            this.StateCommand = new WebResourceProviderEnableCommand(this);
        }

        protected override void RegisterInstallerCommands()
        {
            WebResourceProviderPluginDialogPageAttribute[] dialogAttributes = (WebResourceProviderPluginDialogPageAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginDialogPageAttribute));
            PluginRoleNameAttribute roleAttribute = (PluginRoleNameAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(PluginRoleNameAttribute));

            foreach (WebResourceProviderPluginDialogPageAttribute dialogAttribute in dialogAttributes) {
                SiteMapNodeDefinition dialogNode = new SiteMapNodeDefinition
                {
                    Url = dialogAttribute.BaseVirtualPath + dialogAttribute.Url,
                    Title = dialogAttribute.Title,
                    Description = dialogAttribute.Description,
                    Role = roleAttribute.Role
                };

                this.InstallerCommands.Enqueue(new InstallDialogNodeCommand
                {
                    DialogNode = dialogNode
                });
            }
       
        }

         private class WebResourceProviderEnableCommand : IPluginStateCommand
        {
             private AbstractWebResourceProviderPlugin plugin;

             public WebResourceProviderEnableCommand(AbstractWebResourceProviderPlugin abstractWebResourceProviderPlugin)
            {

                this.plugin = abstractWebResourceProviderPlugin;
            }

            public void Enable(IPluginService PluginService)
            {

                WebResourceProviderPluginDialogPageAttribute[] dialogs = (WebResourceProviderPluginDialogPageAttribute[])Attribute.GetCustomAttributes(this.plugin.GetType(), typeof(WebResourceProviderPluginDialogPageAttribute));
                IList<SiteMapNodeDefinition> nodes = new List<SiteMapNodeDefinition>();

                foreach (WebResourceProviderPluginDialogPageAttribute dialog in dialogs)
                {
                    nodes.Add(new SiteMapNodeDefinition
                    {
                        Url = dialog.Url,
                        Title = dialog.Title,
                        Description = dialog.Description,

                    });


                }
               



                PluginService.SetSiteMapNodeVisibility(nodes, "0");
            }

            public void Disable(IPluginService PluginService)
            {
                //Dialogs don't really need to be disabled
                
            }
        }

         private class InstallDialogNodeCommand : IPluginInstallerCommand
        {

           
            public SiteMapNodeDefinition DialogNode { get; set; }

            public void CheckPreconditions(IPluginService PluginService)
            {

                bool isExisting = PluginService.IsSiteMapUrlExisting(DialogNode.Url);

                if (isExisting)
                {
                    throw new PluginInstallerException(DialogNode.Url + " already exists.");
                }

            }

            public void Execute(IPluginService PluginService)
            {
                PluginService.RegisterDialogSiteMapNode(DialogNode);
            }

            public void Rollback(IPluginService PluginService)
            {   
                IList<SiteMapNodeDefinition> nodes = new List<SiteMapNodeDefinition>();
                nodes.Add(DialogNode);

                try
                {
                    PluginService.RemoveSiteMapNodes(nodes);
                }
                catch (UpdateException ex)
                {
                    ILogger<InstallDialogNodeCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallDialogNodeCommand>>();
                    logger.Warn(ex);
                }
                catch (System.Exception ex)
                {
                    ILogger<InstallDialogNodeCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallDialogNodeCommand>>();
                    logger.Fatal(ex);
                    throw;
                }
            }

            public void Validate(IPluginService PluginService)
            {

                bool isExisting = PluginService.IsSiteMapUrlExisting(DialogNode.Url);

                if (!isExisting)
                {
                    throw new PluginInstallerException(DialogNode.Url + " was not installed");
                }
            }
        }
    }
}
