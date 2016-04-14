// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="IPluginManager.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/Manager/IPluginManager.cs $
//   $LastChangedRevision: 10888 $ 
//   $LastChangedDate: 2014-03-25 09:53:14 -0700 (Tue, 25 Mar 2014) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Interface for Plugin Manager
// </summary> 
// -------------------------------------------------------------

using System;
using System.Collections.Generic;
using ServicesAPI.Plugin;
using PluginAPI.Base;
using Telerik.Web.UI;

namespace PluginAPI.Manager
{
    public interface IPluginManager
    {


        void StartupGlobal();
        void StartupSiteMasterView();
        void StartupViewMasterView();

        void ShutdownGlobal();
       

        IList<PluginRegistration> ActivePluginRegistrations { get; }
        IList<PluginRegistration> DisabledPluginRegistrations { get; }

        UploadedPluginValidator GetNewPluginValidator(byte[] bytes);

        string PluginPath { get; set; }
        string ApplicationVersion { get; set; }

        void InstallPlugin(string filename, bool isEnabled);

        void DisablePlugin(string name, string version);

        void EnablePlugin(string name, string version);

        void DeletePlugin(string name, string version);

        Lazy<IPlugin, IPluginMetadata> GetActivePlugin(string pluginName);
        IList<Lazy<IPlugin, IPluginMetadata>> GetActivePlugins();


        Func<IList<IWebResourceProviderPlugin>, string> InitializeWebResourceProviderPlugins { get; set; }
        Func<IList<IFilterMenuPlugin>, string> InitializeFilterMenuPlugins { get; set; }
        Func<IList<ILayoutMenuPlugin>, string> InitializeLayoutMenuPlugins { get; set; }
        Func<IList<IPageActionMenuPlugin>, string> InitializePageActionMenuPlugins { get; set; }
        Func<IList<IRowActionDialogPlugin>, string> InitializeRowActionMenuPlugins { get; set; }
        Func<IList<ITabPlugin>, string> InitializeTabPlugins { get; set; }
        Func<IList<IToolbarPlugin>, string> InitializeToolbarPlugins { get; set; }

        bool HandlePageMenuAction(object sender, RadMenuEventArgs e);
    }
}
