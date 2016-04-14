using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ServicesAPI.Templates;
using Utilities.ErrorHandling;
using Telerik.Web.UI;

namespace Plugin_WorkAssignment.Controls.Grid.Templates
{
    public class DayFromDateColumnTemplate : PipelineTemplate
    {
        public String DataField { get; set; }
        public override void InstantiateIn(Control container)
        {
            if (container == null)
            {
                return;
            }

            // Check if data field is filled in
            if (string.IsNullOrEmpty(DataField) == true)
            {
                string errorMsg = "DayFromDateColumnTemplate requires a data field";

                throw new GridDefinitionException(errorMsg);
            }

            container.DataBinding += new EventHandler(container_DataBinding);
        }

        /// <summary>
        /// Localizes field value
        /// </summary>
        /// <param name="sender">Field cell</param>
        /// <param name="e">Arguments</param>
        void container_DataBinding(object sender, EventArgs e)
        {
            GridTableCell cell = (GridTableCell)sender;
            if (cell != null)
            {
                GridDataItem item = (GridDataItem)cell.NamingContainer;
                if (item != null)
                {
                    string cellText = Convert.ToString(DataBinder.Eval(item.DataItem, DataField));
                    if (!String.IsNullOrEmpty(cellText))
                    {
                        DateTime date = Convert.ToDateTime(cellText);
                        cell.Text = date.ToString("ddd");
                    }

                    //cell.Text = ResourceExtensions.GetGlobalResxObject("WebResources", cellText, String.Empty);
                }
            }
        }
   

    }
}