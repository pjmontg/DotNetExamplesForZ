// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="AbstractWebResourceProviderPlugin.cs">
// COPYRIGHT NOTICE

// SOFTWARE CONTAINING TRADE SECRETS

// Copyright 2014 Pipeline Group, Inc.  (PIPELINE GROUP, INC ). All rights reserved.

// This software and documentation constitute an unpublished work and contain valuable
// trade secrets and proprietary information belonging to the PIPELINE GROUP, INC .
// None of the foregoing material may be copied, duplicated or disclosed without the 
// express written permission of the PIPELINE GROUP, INC .

// PIPELINE GROUP, INC  EXPRESSLY DISCLAIMS ANY AND ALL WARRANTIES CONCERNING THIS 
// SOFTWARE AND DOCUMENTATION, INCLUDING ANY WARRANTIES OF MERCHANTABILITY AND/OR FITNESS
// FOR ANY PARTICULAR PURPOSE, AND WARRANTIES OF PERFORMANCE, AND ANY WARRANTY THAT MIGHT 
// OTHERWISE ARISE FROM COURSE OF DEALING OR USAGE OF TRADE.
// NO WARRANTY IS EITHER EXPRESS OR IMPLIED WITH RESPECT TO THE USE OF THE SOFTWARE OR 
// DOCUMENTATION. 

// Under no circumstances shall PIPELINE GROUP, INC  be liable for incidental, special, indirect, direct 
// or consequential damages or loss of profits, interruption of business, or related expenses 
// which may arise from use of software or documentation, including but not limited to those 
// resulting from defects in software and/or documentation, or loss or inaccuracy of data 
// of any kind.

// </copyright>
// <author>$Author: pmontgomery $</author>
// <remarks>
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/Base/AbstractRootMenuWebResourceProviderPlugin.cs $
//   $LastChangedRevision: 10888 $ 
//   $LastChangedDate: 2014-03-25 09:53:14 -0700 (Tue, 25 Mar 2014) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Abstract example for the inherited web resoureces provider plugins
// </summary> 
// ------------------------------------------------------------- 


using System;
using System.Collections.Generic;
using System.Reflection;
using PluginAPI.Attributes;
using PluginAPI.VPP;
using ServicesAPI.ViewModels;
using ServicesAPI.Plugin;
using PluginAPI.Exception;
using PluginAPI.Command;
using System.Data;
using Logging;
using Unity.Extensions;
using Microsoft.Practices.Unity;

namespace PluginAPI.Base
{
    public abstract class AbstractRootMenuWebResourceProviderPlugin : AbstractWebResourceProviderPlugin
    {
                

        public override WebResourceVirtualPathProvider VirtualPathProvider
        {

            get
            {
                if (null == this._vpp)
                {
                    Assembly asm = Assembly.GetAssembly(this.GetType());
                    WebResourceProviderPluginChildPageAttribute[] pages = (WebResourceProviderPluginChildPageAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginChildPageAttribute));
                    WebResourceProviderPluginGenericResourceAttribute[] resources = (WebResourceProviderPluginGenericResourceAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginGenericResourceAttribute));
                    WebResourceProviderPluginDialogPageAttribute[] dialogs = (WebResourceProviderPluginDialogPageAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginDialogPageAttribute));
                   
                    HashSet<string> resourceSet = new HashSet<string>();

                    foreach (WebResourceProviderPluginChildPageAttribute page in pages)
                    {
                        resourceSet.Add(page.Url);
                    }

                    foreach (WebResourceProviderPluginGenericResourceAttribute resource in resources)
                    {
                        resourceSet.Add(resource.Url);
                    }

                    foreach (WebResourceProviderPluginDialogPageAttribute dialog in dialogs)
                    {
                        resourceSet.Add(dialog.Url);
                    }

                    WebResourceProviderPluginRootMenuAttribute rootMenu = (WebResourceProviderPluginRootMenuAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(WebResourceProviderPluginRootMenuAttribute));

                    this._vpp = new WebResourceVirtualPathProvider(asm, rootMenu.BaseVirtualPath, resourceSet);
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
            WebResourceProviderPluginRootMenuAttribute rootAttribute = (WebResourceProviderPluginRootMenuAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(WebResourceProviderPluginRootMenuAttribute));
            PluginRoleNameAttribute roleAttribute = (PluginRoleNameAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(PluginRoleNameAttribute));

            SiteMapNodeDefinition parentNode = new SiteMapNodeDefinition
            {
                Url = rootAttribute.BaseVirtualPath,
                Title = rootAttribute.Title,
                Description = rootAttribute.Description,
                ResourceKey = rootAttribute.ResourceKey,
                Role = roleAttribute.Role
            };

            this.InstallerCommands.Enqueue(new InstallRootNodeCommand
            {
                ParentNode = parentNode
            });



            WebResourceProviderPluginChildPageAttribute[] childPages = (WebResourceProviderPluginChildPageAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginChildPageAttribute));

            IList<SiteMapNodeDefinition> childNodes = new List<SiteMapNodeDefinition>();
            foreach (WebResourceProviderPluginChildPageAttribute child in childPages)
            {
                childNodes.Add(new SiteMapNodeDefinition
                {
                    Url = parentNode.Url + child.Url,
                    Title = child.Title,
                    Description = child.Description,

                });

            }

            this.InstallerCommands.Enqueue(new InstallChildNodesCommand
            {
                ChildNodes = childNodes,
                ParentNode = parentNode
            });



            WebResourceProviderPluginDialogPageAttribute[] dialogPages = (WebResourceProviderPluginDialogPageAttribute[])Attribute.GetCustomAttributes(this.GetType(), typeof(WebResourceProviderPluginDialogPageAttribute));
            IList<SiteMapNodeDefinition> dialogNodes = new List<SiteMapNodeDefinition>();
            foreach (WebResourceProviderPluginDialogPageAttribute dialog in dialogPages)
            {
                dialogNodes.Add(new SiteMapNodeDefinition
                {
                    Url = parentNode.Url + dialog.Url,
                    Title = dialog.Title,
                    Description = dialog.Description,

                });

            }
            this.InstallerCommands.Enqueue(new InstallDialogNodesCommand
            {
                DialogNodes = dialogNodes,
                ParentNode = parentNode
            });
           
        }

      

        private class WebResourceProviderEnableCommand : IPluginStateCommand
        {
            private AbstractRootMenuWebResourceProviderPlugin plugin;

            public WebResourceProviderEnableCommand(AbstractRootMenuWebResourceProviderPlugin abstractWebResourceProviderPlugin)
            {

                this.plugin = abstractWebResourceProviderPlugin;
            }

            public void Enable(IPluginService PluginService)
            {
                WebResourceProviderPluginRootMenuAttribute rootAttribute = (WebResourceProviderPluginRootMenuAttribute)Attribute.GetCustomAttribute(this.plugin.GetType(), typeof(WebResourceProviderPluginRootMenuAttribute));

                WebResourceProviderPluginDialogPageAttribute[] dialogPages = (WebResourceProviderPluginDialogPageAttribute[])Attribute.GetCustomAttributes(this.plugin.GetType(), typeof(WebResourceProviderPluginDialogPageAttribute));
                IList<SiteMapNodeDefinition> nodes = new List<SiteMapNodeDefinition>();

                foreach (WebResourceProviderPluginDialogPageAttribute dialog in dialogPages)
                {
                    nodes.Add(new SiteMapNodeDefinition
                    {
                        Url = dialog.Url,
                        Title = dialog.Title,
                        Description = dialog.Description,

                    });

                }

                nodes.Add(new SiteMapNodeDefinition
                {
                    Url = rootAttribute.BaseVirtualPath,
                    Title = rootAttribute.Title,
                    Description = rootAttribute.Description,

                });



                PluginService.SetSiteMapNodeVisibility(nodes, "1");
            }

            public void Disable(IPluginService PluginService)
            {
                WebResourceProviderPluginRootMenuAttribute rootAttribute = (WebResourceProviderPluginRootMenuAttribute)Attribute.GetCustomAttribute(this.plugin.GetType(), typeof(WebResourceProviderPluginRootMenuAttribute));

                WebResourceProviderPluginDialogPageAttribute[] dialogPages = (WebResourceProviderPluginDialogPageAttribute[])Attribute.GetCustomAttributes(this.plugin.GetType(), typeof(WebResourceProviderPluginDialogPageAttribute));
                IList<SiteMapNodeDefinition> nodes = new List<SiteMapNodeDefinition>();

                foreach (WebResourceProviderPluginDialogPageAttribute dialog in dialogPages)
                {
                    nodes.Add(new SiteMapNodeDefinition
                    {
                        Url = dialog.Url,
                        Title = dialog.Title,
                        Description = dialog.Description,

                    });

                }

                nodes.Add(new SiteMapNodeDefinition
                {
                    Url = rootAttribute.BaseVirtualPath,
                    Title = rootAttribute.Title,
                    Description = rootAttribute.Description,

                });



                PluginService.SetSiteMapNodeVisibility(nodes, "0");
            }
        }

        private class InstallRootNodeCommand : IPluginInstallerCommand
        {

           
            public SiteMapNodeDefinition ParentNode { get; set; }

            public void CheckPreconditions(IPluginService PluginService)
            {

                bool isExisting = PluginService.IsSiteMapUrlExisting(ParentNode.Url);

                if (isExisting)
                {
                    throw new PluginInstallerException(ParentNode.Url + " already exists.");
                }

            }

            public void Execute(IPluginService PluginService)
            {
                PluginService.RegisterParentSiteMapNode(ParentNode);
            }

            public void Rollback(IPluginService PluginService)
            {   
                IList<SiteMapNodeDefinition> nodes = new List<SiteMapNodeDefinition>();
                nodes.Add(ParentNode);

                try
                {
                    PluginService.RemoveSiteMapNodes(nodes);
                }
                catch (UpdateException ex)
                {
                    ILogger<InstallRootNodeCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallRootNodeCommand>>();
                    logger.Warn(ex);
                }
                catch (System.Exception ex)
                {
                    ILogger<InstallRootNodeCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallRootNodeCommand>>();
                    logger.Fatal(ex);
                    throw;
                }
            }

            public void Validate(IPluginService PluginService)
            {

                bool isExisting = PluginService.IsSiteMapUrlExisting(ParentNode.Url);

                if (!isExisting)
                {
                    throw new PluginInstallerException(ParentNode.Url + " was not installed");
                }
            }

           
        }

        private class InstallChildNodesCommand : IPluginInstallerCommand
        {
           

            public IList<SiteMapNodeDefinition> ChildNodes { get; set; }
            public SiteMapNodeDefinition ParentNode { get; set; }

            public void CheckPreconditions(IPluginService PluginService)
            {

                bool isExisting = PluginService.IsSiteMapUrlExisting(ParentNode.Url);
                if (!isExisting)
                {
                    throw new PluginInstallerException(ParentNode.Url + " was not installed");
                }
            }

            public void Execute(IPluginService PluginService)
            {

                PluginService.RegisterChildSiteMapNodes(ParentNode, ChildNodes);
            }

            public void Rollback(IPluginService PluginService)
            {
                try
                {
                    PluginService.RemoveSiteMapNodes(ChildNodes);
                }
                catch (UpdateException ex)
                {
                    ILogger<InstallChildNodesCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallChildNodesCommand>>();
                    logger.Warn(ex);
                }
                catch (System.Exception ex)
                {
                    ILogger<InstallChildNodesCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallChildNodesCommand>>();
                    logger.Fatal(ex);
                    throw;
                }
            }

            public void Validate(IPluginService PluginService)
            {

                foreach (SiteMapNodeDefinition child in ChildNodes)
                {
                    bool isExisting = PluginService.IsSiteMapUrlExisting(child.Url);

                    if (!isExisting)
                    {
                        throw new PluginInstallerException(child.Url + " was not installed");
                    }

                }
            }
        }

        private class InstallDialogNodesCommand : IPluginInstallerCommand
        {
         
            public IList<SiteMapNodeDefinition> DialogNodes { get; set; }
            public SiteMapNodeDefinition ParentNode { get; set; }

            public void CheckPreconditions(IPluginService PluginService)
            {

            }

            public void Execute(IPluginService PluginService)
            {
                PluginService.RegisterDialogSiteMapNodes(DialogNodes);
            }

            public void Rollback(IPluginService PluginService)
            {
                try
                {
                    PluginService.RemoveSiteMapNodes(DialogNodes);
                }
                catch (UpdateException ex)
                {
                    ILogger<InstallDialogNodesCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallDialogNodesCommand>>();
                    logger.Warn(ex);
                }
                catch (System.Exception ex)
                {
                    ILogger<InstallDialogNodesCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<InstallDialogNodesCommand>>();
                    logger.Fatal(ex);
                    throw;
                }
            }

            public void Validate(IPluginService PluginService)
            {

                foreach (SiteMapNodeDefinition dialog in DialogNodes)
                {
                    bool isExisting = PluginService.IsSiteMapUrlExisting(dialog.Url);

                    if (!isExisting)
                    {
                        throw new PluginInstallerException(dialog.Url + " was not installed");
                    }

                }
            }
        }
    }

   
}
