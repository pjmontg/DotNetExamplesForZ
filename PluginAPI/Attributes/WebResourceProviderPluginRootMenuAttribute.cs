// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="WebResourceProviderPluginRootPageAttribute.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/Attributes/WebResourceProviderPluginRootMenuAttribute.cs $
//   $LastChangedRevision: 10888 $ 
//   $LastChangedDate: 2014-03-25 09:53:14 -0700 (Tue, 25 Mar 2014) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Attribute for Web Resource Provider Plugin Root Page
// </summary> 
// ------------------------------------------------------------- 


using System;

namespace PluginAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class WebResourceProviderPluginRootMenuAttribute : AbstractWebResourceProviderPluginAttribute
    {
      //  private string _basePageUrl;
        private string _baseTitle;
        private string _baseDescription;
        private string _resourceKey;

        public WebResourceProviderPluginRootMenuAttribute(string baseVirtualPath, string baseTitle, string baseDescription, string resourceKey) : base(baseVirtualPath)
        {
            
            this._baseTitle = baseTitle;
            this._baseDescription = baseDescription;
            this._resourceKey = resourceKey;
        }

        public string ResourceKey

        {
            get
            {
                return _resourceKey;
            }
        }

       

        public string Title
        {
            get
            {
                return _baseTitle;
            }
        }

        public string Description
        {
            get
            {
                return _baseDescription;
            }
        }


       
    }
}
