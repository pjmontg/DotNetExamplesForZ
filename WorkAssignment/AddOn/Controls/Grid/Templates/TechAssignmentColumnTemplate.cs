using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ServicesAPI.Templates;
using Utilities.ErrorHandling;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Plugin_WorkAssignment.ModelAPI;

namespace Plugin_WorkAssignment.Controls.Grid.Templates
{
    public class TechAssignmentColumnTemplate : PipelineTemplate
    {
        private WorkOrderGridControl _parentControl;
        public String DataField { get; set; }

        public TechAssignmentColumnTemplate(WorkOrderGridControl parent)
        {
            this._parentControl = parent;
        }

        public override void InstantiateIn(Control container)
        {
            if (container == null)
            {
                return;
            }

            // Check if data field is filled in
            if (string.IsNullOrEmpty(DataField) == true)
            {
                string errorMsg = "TechAssignmentColumnTemplate requires a data field";

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
                    string assignees = Convert.ToString(DataBinder.Eval(item.DataItem, DataField));
                    string activityId = Convert.ToString(DataBinder.Eval(item.DataItem, "ID"));
                    string shop = Convert.ToString(DataBinder.Eval(item.DataItem, "SHOP"));
                    DateTime startDate = Convert.ToDateTime(DataBinder.Eval(item.DataItem, "REMAIN_EARLY_START"));
                    double duration = Convert.ToDouble(DataBinder.Eval(item.DataItem, "ORIG_DURATION"));

                    Panel namesPanel = new Panel();



                    if (String.IsNullOrEmpty(assignees))
                    {
                        namesPanel.Controls.Add(new Label() { Text = "-None-" });
                    }
                    else
                    {
                        namesPanel.Controls.Add(new Label() { Text = assignees });
                    }

                    
                    DateTime current = new DateTime(this._parentControl.CurrentWorkDate.Value.Year, 
                        this._parentControl.CurrentWorkDate.Value.Month, this._parentControl.CurrentWorkDate.Value.Day);
                    DateTime actStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
              
                    bool isEnabled = true;
                   /* if (!String.Equals(this._parentControl.CurrentShop, shop, StringComparison.CurrentCultureIgnoreCase)
                        || !DateTime.Equals(current, actStartDate))
                    {
                        isEnabled = false;
                    }*/
                    Panel buttonPanel = new Panel();
                    buttonPanel.Controls.Add(new Button() { 
                        Text = "Assign...",
                        OnClientClick = "OpenAssignDialog('" + activityId + "', '" + shop + "', '" + startDate + "', " + duration + "); return false;",
                        Enabled = isEnabled,
                        CssClass = "assign-button"
                        
                    });
                    namesPanel.CssClass = "row-assign-names float-left";
                    buttonPanel.CssClass = "row-assign-button float-right";

                    cell.Controls.Add(namesPanel);
                    cell.Controls.Add(buttonPanel);
                    //DateTime date = DateTime.Parse(cellText);
                  //  cell.Text = date.ToString("dddd");

                    //cell.Text = ResourceExtensions.GetGlobalResxObject("WebResources", cellText, String.Empty);
                }
            }
        }
    }
}