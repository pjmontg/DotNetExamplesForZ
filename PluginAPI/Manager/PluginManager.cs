// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="PluginManager.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/Manager/PluginManager.cs $
//   $LastChangedRevision: 19141 $ 
//   $LastChangedDate: 2016-03-23 08:14:48 -0700 (Wed, 23 Mar 2016) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Class for Plugin Manager
// </summary> 
// -------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using Logging;
using Microsoft.Practices.Unity;
using ServicesAPI.Plugin;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using PluginAPI.Exception;
using PluginAPI.Base;
using System.Reflection;
using System.IO;
using Telerik.Web.UI;
using System.Data;
using Utilities.ErrorHandling;
using Utilities;

namespace PluginAPI.Manager
{
    [Unity.Extensions.Attribute.DependencyInjectionSingleton]
    public class PluginManager : IPluginManager
    {
        private ILogger<IPluginManager> _logger;
        private IPluginService _pluginService;

        private IList<PluginRegistration> _activePlugins = new List<PluginRegistration>();
        private IList<PluginRegistration> _disabledPlugins = new List<PluginRegistration>();
       
        private AggregateCatalog _catalog;

        private bool _activeWebResourceProviderPluginsNeedsRefresh = false;
        private bool _activePageActionPluginsNeedRefresh = false;

        private IList<IWebResourceProviderPlugin> _activeWebResourceProviderPlugins;
        private IList<IPageActionMenuPlugin> _activePageActionMenuPlugins;

        private IList<IPageActionMenuPlugin> ActivePageActionMenuPlugins
        {
            get
            {
                if (_activePageActionMenuPlugins == null || _activePageActionPluginsNeedRefresh)
                {
                    _activePageActionMenuPlugins = new List<IPageActionMenuPlugin>();
                    foreach (Lazy<IPlugin, IPluginMetadata> plugin in _plugins)
                    {
                        PluginRegistration reg = _activePlugins.FirstOrDefault(x => x.Name == plugin.Metadata.UniqueName && x.Version == plugin.Metadata.Version);

                        if (null != reg && plugin.Value.GetType().GetInterfaces().Contains(typeof(IPageActionMenuPlugin)))
                        {
                            _activePageActionMenuPlugins.Add((IPageActionMenuPlugin)plugin.Value);
                        }
                    }
                    _activePageActionPluginsNeedRefresh = false;
                }
                return _activePageActionMenuPlugins;
            }
        }

       

        //TODO need to invalidate this when a plugin change occurs
        private IList<IWebResourceProviderPlugin> ActiveWebResourceProviderPlugins
        {
            get
            {
                if (_activeWebResourceProviderPlugins == null || _activeWebResourceProviderPluginsNeedsRefresh)
                {
                    _activeWebResourceProviderPlugins = new List<IWebResourceProviderPlugin>();
                    foreach (Lazy<IPlugin, IPluginMetadata> webPlugin in _plugins)
                    {
                        PluginRegistration reg = _activePlugins.FirstOrDefault(x => x.Name == webPlugin.Metadata.UniqueName && x.Version == webPlugin.Metadata.Version);

                        if (null != reg && webPlugin.Value.GetType().GetInterfaces().Contains(typeof(IWebResourceProviderPlugin)))
                        {
                            _activeWebResourceProviderPlugins.Add((IWebResourceProviderPlugin)webPlugin.Value);
                        }
                    }
                    _activeWebResourceProviderPluginsNeedsRefresh = false;
                }
                return _activeWebResourceProviderPlugins;
            }
            
        }

        private void MarkPluginListsAsDirty()
        {
            _activeWebResourceProviderPluginsNeedsRefresh = true;
            _activePageActionPluginsNeedRefresh = true;
        }

#pragma warning disable 0649
        [ImportMany]
        private IEnumerable<Lazy<IPlugin, IPluginMetadata>> _plugins;
#pragma warning restore 0649
        private CompositionContainer _container;

        private Dictionary<string, string> _assemblyToFilePath = new Dictionary<string, string>();

        public string PluginPath { get; set; }

        public string ApplicationVersion { get; set; }

        public IList<PluginRegistration> ActivePluginRegistrations
        {
            get
            {
                return _activePlugins;
            }
        }



        public IList<PluginRegistration> DisabledPluginRegistrations
        {
            get
            {
                return _disabledPlugins;
            }
        }

       
      

        [InjectionMethod]
        public void Initialize(ILogger<IPluginManager> logger, IPluginService pluginService)
        {
           
            _pluginService = pluginService;
            _logger = logger;
            _logger.Debug("PluginManager::Initialize method.");
          
        }


        public IList<Lazy<IPlugin, IPluginMetadata>> GetActivePlugins()
        {
           
            IList<Lazy<IPlugin, IPluginMetadata>> plugins = new List<Lazy<IPlugin, IPluginMetadata>>();

            foreach (Lazy<IPlugin, IPluginMetadata> plugin in _plugins)
            {
                PluginRegistration reg = _activePlugins.FirstOrDefault(x => x.Name == plugin.Metadata.UniqueName && x.Version == plugin.Metadata.Version);

                

                if (null != reg)
                {
                    plugins.Add(plugin);
                }
            }

            return plugins;
        }

        public Lazy<IPlugin, IPluginMetadata> GetActivePlugin(string pluginName)
        {
            Lazy<IPlugin, IPluginMetadata> returnPlugin = null;

            foreach (Lazy<IPlugin, IPluginMetadata> plugin in _plugins)
            {
                PluginRegistration reg = _activePlugins.FirstOrDefault(x => x.Name == pluginName);

                if (null != reg)
                {
                    returnPlugin = plugin;
                }
            }

            return returnPlugin;
        }

        public void ShutdownGlobal()
        {
            _catalog.Dispose();
            _container.Dispose();
            
        }

        public void StartupGlobal()
        {
            _assemblyToFilePath.Clear();
            _catalog = new AggregateCatalog();
            _catalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));

            string[] pluginFilePaths = Directory.GetFiles(PluginPath, "*.dll",
                                         SearchOption.TopDirectoryOnly);

            //Loads each plugin manually and checks to make sure all dependencies are met.  
            foreach (string pluginFilePath in pluginFilePaths)
            {
                Assembly pluginAsm = Assembly.LoadFrom(pluginFilePath);
                AssemblyCatalog asmCatalog = new AssemblyCatalog(pluginAsm);
                
                try
                {
                    asmCatalog.Parts.ToArray();
                    _catalog.Catalogs.Add(asmCatalog);
                    _assemblyToFilePath.Add(pluginAsm.GetName().Name, pluginFilePath);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    _logger.Error("Error loading plugin: " + pluginFilePath);
                    _logger.Error(ex);
                    foreach (System.Exception loaderEx in ex.LoaderExceptions)
                    {
                        _logger.Error(loaderEx);
                    }
                    

                }
               
            }
           
            _container = new CompositionContainer(_catalog);

            _container.ComposeParts(this);

            foreach (Lazy<IPlugin, IPluginMetadata> plugin in _plugins)
            {
                plugin.Value.Initialize(_pluginService);
            }

            this.LoadPluginRegistrations();

            if (null != InitializeWebResourceProviderPlugins)
            {
                InitializeWebResourceProviderPlugins(this.ActiveWebResourceProviderPlugins);
            }
        }

        private void LoadPluginRegistrations()
        {
            this._activePlugins.Clear();
            this._disabledPlugins.Clear();

            IList<PluginRegistration> pluginRegistrations = null;
            IList<PluginRegistration> missingPlugins = null;
            try
            {
                pluginRegistrations = _pluginService.GetAllRegistrations();
                missingPlugins = _pluginService.GetAllRegistrations();
            }
            catch (System.Exception ex)
            {
                _logger.Fatal(ex);
                throw;
            }


            foreach (Lazy<IPlugin, IPluginMetadata> plugin in _plugins)
            {
                //Find registration
                PluginRegistration reg = pluginRegistrations.FirstOrDefault(x => x.Name == plugin.Metadata.UniqueName && x.Version == plugin.Metadata.Version);

                //If plugin is not installed, delete the file
                if (null == reg)
                {
                    
                    string assemblyName = plugin.Value.GetType().Assembly.GetName().Name;
                    string filename = _assemblyToFilePath[assemblyName];
                   

                    if (File.Exists(filename))
                    {
                        _logger.Debug("Deleting uninstalled plugin " + filename);
                        try
                        {
                            File.Delete(filename);
                        }
                        catch (System.Exception ex)
                        {
                            // Most likely means file had been deleted already
                            _logger.Warn(ex);
                        }
                    }
                    continue;
                }

                //Check for version mismatch
/*
                if (!String.Equals(plugin.Metadata.RequiresVersion, this.ApplicationVersion, StringComparison.CurrentCultureIgnoreCase))
                {
                    PluginRegistration mismatchedPlugin = new PluginRegistration
                    {
                        Name = plugin.Metadata.UniqueName,
                        Version = plugin.Metadata.Version,
                        Description = plugin.Metadata.Description,
                        RequiresVersion = plugin.Metadata.RequiresVersion,
                        Status = PluginStatus.SYSTEM_DISABLED_VERSIONMISMATCH
                        
                    };

                    try
                    {
                        _pluginService.UpdatePluginRegistration(mismatchedPlugin);
                    }
                    catch (DataRepositoryException ex)
                    {
                        _logger.Fatal(ex);
                        throw;
                    }

                    _disabledPlugins.Add(mismatchedPlugin);
                }
*/              

                //if Licensed
                if (!_pluginService.IsPluginLicensed(plugin.Metadata.UniqueName, plugin.Metadata.Version))
                {

                    try
                    {
                        plugin.Value.StateCommand.Disable(_pluginService);
                    }
                    catch (DataRepositoryException ex)
                    {
                        _logger.Fatal(ex);
                        throw;
                    }

                    PluginRegistration unlicensedPlugin = new PluginRegistration
                    {
                        Name = plugin.Metadata.UniqueName,
                        Version = plugin.Metadata.Version,
                        Description = plugin.Metadata.Description,
                        RequiresVersion = plugin.Metadata.RequiresVersion,
                        Status = PluginStatus.SYSTEM_DISABLED_UNLICENSED
                    };

                    try
                    {
                        _pluginService.UpdatePluginRegistration(unlicensedPlugin);
                    }
                    catch (DataRepositoryException ex)
                    {
                        _logger.Fatal(ex);
                        throw;
                    }

                    _disabledPlugins.Add(unlicensedPlugin);

                }
                else
                {
                    if (reg.Status != PluginStatus.USER_DISABLED)
                    {
                        PluginRegistration enabledPlugin = new PluginRegistration
                        {
                            Name = plugin.Metadata.UniqueName,
                            Version = plugin.Metadata.Version,
                            Description = plugin.Metadata.Description,
                            Status = PluginStatus.ENABLED
                        };
                        this._activePlugins.Add(enabledPlugin);

                        //If not disabled by the user, the turn it back on if it's not already enabled
                        if (reg.Status != PluginStatus.ENABLED)
                        {
                            try
                            {
                                plugin.Value.StateCommand.Enable(_pluginService);
                            }
                            catch (DataRepositoryException ex)
                            {
                                _logger.Fatal(ex);
                                throw;
                            }
                        }
                    }
                    else
                    {
                        this._disabledPlugins.Add(new PluginRegistration
                        {
                            Name = plugin.Metadata.UniqueName,
                            Version = plugin.Metadata.Version,
                            Description = plugin.Metadata.Description,
                            RequiresVersion = plugin.Metadata.RequiresVersion,
                            Status = PluginStatus.USER_DISABLED
                        });
                    }
                }



                PluginRegistration activePlugin = missingPlugins.FirstOrDefault(x => (x.Name == plugin.Metadata.UniqueName) && (x.Version == plugin.Metadata.Version));
                missingPlugins.Remove(activePlugin);
            }

            foreach (PluginRegistration missingPlugin in missingPlugins)
            {
                missingPlugin.Status = PluginStatus.SYSTEM_DISABLED_MISSING;
                missingPlugin.RequiresVersion = SyntempoConstants.N_A;
                missingPlugin.Description = SyntempoConstants.N_A;
                _disabledPlugins.Add(missingPlugin);
                _pluginService.UpdatePluginRegistration(missingPlugin);
            }
        }




        public void DisablePlugin(string name, string version)
        {
            Lazy<IPlugin, IPluginMetadata> plugin = _plugins.FirstOrDefault(x => x.Metadata.UniqueName == name && x.Metadata.Version == version);

            if (null == plugin)
            {
                throw new PluginNotFoundException("The plugin " + name + ", " + version + ", could not be found in the Active plugins collection.");
            }

            _pluginService.UpdatePluginRegistration(new PluginRegistration
            {
                Name = name,
                Version = version,
                Status = PluginStatus.USER_DISABLED
            });
            plugin.Value.StateCommand.Disable(_pluginService);
            this.LoadPluginRegistrations();
            MarkPluginListsAsDirty();
        }

        public void EnablePlugin(string name, string version)
        {
            Lazy<IPlugin, IPluginMetadata> plugin = _plugins.FirstOrDefault(x => x.Metadata.UniqueName == name && x.Metadata.Version == version);

            if (null == plugin)
            {
                throw new PluginNotFoundException("The plugin " + name + ", " + version + ", could not be found in the disabled plugins collection.");
            }

            _pluginService.UpdatePluginRegistration(new PluginRegistration
            {
                Name = name,
                Version = version,
                Status = PluginStatus.ENABLED
            });

            plugin.Value.StateCommand.Enable(_pluginService);
            this.LoadPluginRegistrations();
            MarkPluginListsAsDirty();
        }

        public void DeletePlugin(string name, string version)
        {
            Lazy<IPlugin, IPluginMetadata> plugin = _plugins.FirstOrDefault(x => x.Metadata.UniqueName == name && x.Metadata.Version == version);

            if (null != plugin)
            {
                PluginUninstaller uninstaller = new PluginUninstaller(plugin, _pluginService, _logger);
                uninstaller.Uninstall();
                
            }
            else
            {
                try
                {
                    _pluginService.DeletePluginRegistration(new PluginRegistration
                    {
                        Name = name,
                        Version = version

                    });
                }
                catch (UpdateException ex)
                {
                    _logger.Warn(ex);
                }
                catch (System.Exception ex)
                {
                    _logger.Fatal(ex);
                    throw;
                }
            }

            

            this.ShutdownGlobal();

            if (null != plugin)
            {
                string assemblyName = plugin.Value.GetType().Assembly.GetName().Name;
                string filename = _assemblyToFilePath[assemblyName];

                if (File.Exists(filename))
                {
                    _logger.Debug("Uninstalling plugin " + filename);
                    try
                    {
                        File.Delete(filename);
                    }
                    catch (System.Exception ex)
                    {
                        // Most likely means file had been deleted already
                        _logger.Warn(ex);
                    }
                }
            }

            this.StartupGlobal();
        }
     

     

        public void InstallPlugin(string filename, bool isEnabled)
        {
            this.ShutdownGlobal();

            //Copy the plugin to the main plugins directory and reinitialize
            string sourceFile = PluginPath + @"\Temp\" + filename;
            string destinationFile = PluginPath + @"\" + filename;

            if (File.Exists(destinationFile))
            {
                _logger.Debug("Replacing existing file with newly uploaded file: " + destinationFile);
                try
                {
                    File.Delete(destinationFile);
                }
                catch (System.Exception ex)
                {
                    // Most likely means file had been deleted already
                    _logger.Warn(ex);
                }
            }
            File.Move(sourceFile, destinationFile);
            try
            {
                PluginInstaller installer = new PluginInstaller(filename, _pluginService, PluginPath, this.ApplicationVersion, _logger);
                installer.Install(isEnabled);
            }
            catch (PluginInstallerException)
            {

                if (File.Exists(destinationFile))
                {
                    _logger.Debug("Deleting plugin file for failed installation: " + destinationFile);
                    try
                    {
                        File.Delete(destinationFile);
                    }
                    catch (System.Exception ex)
                    {
                        // Most likely means file had been deleted already
                        _logger.Warn(ex);
                    }
                }
                throw;
            }
            
            this.StartupGlobal();
        }

      /*  private void SetFullPermissionsOnFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            FileSecurity fSec = fileInfo.GetAccessControl();
            string username = System.Security.Principal.WindowsIdentity.GetCurrent(false).Name;
            fSec.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, AccessControlType.Allow));
            fileInfo.SetAccessControl(fSec);
        }*/

        public UploadedPluginValidator GetNewPluginValidator(byte[] bytes)
        {
            return new UploadedPluginValidator(bytes);
        }

        public void RemoveUnreferencedPlugins()
        {

        }

        public Func<IList<IWebResourceProviderPlugin>, string> InitializeWebResourceProviderPlugins { get; set; }
        public Func<IList<IFilterMenuPlugin>, string> InitializeFilterMenuPlugins { get; set; }
        public Func<IList<ILayoutMenuPlugin>, string> InitializeLayoutMenuPlugins { get; set; }
        public Func<IList<IPageActionMenuPlugin>, string> InitializePageActionMenuPlugins { get; set; }
        public Func<IList<IRowActionDialogPlugin>, string> InitializeRowActionMenuPlugins { get; set; }
        public Func<IList<ITabPlugin>, string> InitializeTabPlugins { get; set; }
        public Func<IList<IToolbarPlugin>, string> InitializeToolbarPlugins { get; set; }

       

        public void StartupSiteMasterView()
        {

        }

        public void StartupViewMasterView()
        {
            if (null != this.InitializePageActionMenuPlugins) 
            {
                this.InitializePageActionMenuPlugins(this.ActivePageActionMenuPlugins);
            }
        }

        public bool HandlePageMenuAction(object sender, RadMenuEventArgs e)
        {
            bool isHandled = false;
            foreach (IPageActionMenuPlugin plugin in this.ActivePageActionMenuPlugins)
            {
                isHandled = isHandled || plugin.HandleMenuAction(sender, e);
            }

            return isHandled;
        }
    }
}
