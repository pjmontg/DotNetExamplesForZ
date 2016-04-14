// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="OTReqColumnTemplate.cs">
// COPYRIGHT NOTICE

// SOFTWARE CONTAINING TRADE SECRETS

// Copyright 2012 Pipeline Group, Inc.  (PIPELINE GROUP, INC ). All rights reserved.

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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/branches/Release/81/WebApp/Plugin_WorkAssignment/Controls/Grid/Templates/OTReqColumnTemplate.cs $
//   $LastChangedRevision: 8256 $ 
//   $LastChangedDate: 2013-08-07 10:56:09 -0700 (Wed, 07 Aug 2013) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Represents a column that specifies if overtime is required or not for activities
// </summary> 
// -------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServicesAPI.Templates;
using System.Web.UI;
using Utilities.ErrorHandling;
using Telerik.Web.UI;
using WebControls.Grid.Templates.Private;

namespace Plugin_WorkAssignment.Controls.Grid.Templates
{
    public class OTReqColumnTemplate : CheckBoxColumnTemplate
    {
        #region Public methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">True parent control</param>
        /// <param name="dataField">Field data for the cell</param>
        public OTReqColumnTemplate(WorkOrderGridControl parent, string dataField) : base(dataField)
        {
            this._parentControl = parent;
            this.ClassName = "OTReqColumnTemplate";
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Bind check box and enable/disable it based on shop and date matching
        /// </summary>
        /// <param name="sender">Caller</param>
        /// <param name="e">Argument</param>
        protected override void OnChecboxDataBinding(object sender, EventArgs e)
        {
            base.OnChecboxDataBinding(sender, e);

            // Enable/disable depending on shop and date matchin
            RadButton cb = sender as RadButton;
            if (null != cb)
            {
                GridDataItem item = (GridDataItem)cb.NamingContainer;
                if (null != item)
                {
                    string shop = Convert.ToString(DataBinder.Eval(item.DataItem, "SHOP"));
                    DateTime startDate = Convert.ToDateTime(DataBinder.Eval(item.DataItem, "REMAIN_EARLY_START"));

                    DateTime current = new DateTime(
                        this._parentControl.CurrentWorkDate.Value.Year,
                        this._parentControl.CurrentWorkDate.Value.Month,
                        this._parentControl.CurrentWorkDate.Value.Day);
                    DateTime actStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

                    bool isEnabled = true;
                    if (!String.Equals(this._parentControl.CurrentShop, shop, StringComparison.CurrentCultureIgnoreCase)
                        || !DateTime.Equals(current, actStartDate))
                    {
                        isEnabled = false;
                    }
                    cb.Enabled = isEnabled;

                    // Determine if field is checked or not
                    cb.Checked = Convert.ToBoolean(DataBinder.Eval(item.DataItem, DataField));
                }
            }
        }
        #endregion

        #region Private variables
        /// <summary>
        /// True parent control
        /// </summary>
        private WorkOrderGridControl _parentControl;
        #endregion
    }
}