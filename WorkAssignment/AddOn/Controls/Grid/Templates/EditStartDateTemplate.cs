using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicesAPI.Templates;
using System.Web.UI;
using Utilities.ErrorHandling;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Logging;
using Plugin_WorkAssignment.ServicesAPI;
using Unity.Extensions;

namespace Plugin_WorkAssignment.Controls.Grid.Templates
{
    public class EditStartDateTemplate : PipelineTemplate
    {

        public String DataField { get; set; }

        private WorkOrderGridControl _parentControl;

        [InjectionMethod]
        public void Initialize(
            ILogger<EditStartDateTemplate> logger,
            IWorkAssignService workAssignService)
        {
            _logger = logger;
            _workAssignService = workAssignService;

            _logger.Debug("EditStartDateTemplate::Initialize method.");
        }


        public EditStartDateTemplate(WorkOrderGridControl parent)
        {
            GlobalUnityContainer.Container.BuildUp<EditStartDateTemplate>(this);
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
                string errorMsg = "EditStartDateTemplate requires a data field";

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
           
                    DateTime startDate = Convert.ToDateTime(DataBinder.Eval(item.DataItem, "REMAIN_EARLY_START"));
                  
                    string shop = Convert.ToString(DataBinder.Eval(item.DataItem, "SHOP"));
                  

                    


                    DateTime current = new DateTime(this._parentControl.CurrentWorkDate.Value.Year,
                        this._parentControl.CurrentWorkDate.Value.Month, this._parentControl.CurrentWorkDate.Value.Day);
                    DateTime actStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

                    bool isEnabled = true;
                  /*  if (!String.Equals(this._parentControl.CurrentShop, shop, StringComparison.CurrentCultureIgnoreCase)
                        || !DateTime.Equals(current, actStartDate))
                    {
                        isEnabled = false;
                    }*/
                    Panel buttonPanel = new Panel();
                    RadDatePicker datePicker = new RadDatePicker
                    {
                        Enabled = isEnabled,
                        AutoPostBack = true,


                    };
                    datePicker.SelectedDate = actStartDate;
                    datePicker.DateInput.ReadOnly = true;
                    datePicker.Width = 100;
                    datePicker.SelectedDateChanged += new Telerik.Web.UI.Calendar.SelectedDateChangedEventHandler(datePicker_SelectedDateChanged);
                    buttonPanel.Controls.Add(datePicker);
              
                    cell.Controls.Add(buttonPanel);
                }
            }
        }

        void datePicker_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
        {
            RadDatePicker datePicker = (RadDatePicker)sender;
            GridDataItem item = (GridDataItem)datePicker.Parent.Parent.Parent;
            String actId = Convert.ToString(DataBinder.Eval(item.DataItem, "ID"));
            string shop = Convert.ToString(DataBinder.Eval(item.DataItem, "SHOP"));
            _workAssignService.UpdateActivityRemainingEarlyStartDate(actId, shop, e.NewDate);
            this._parentControl.Rebind();
        }


        private static ILogger<EditStartDateTemplate> _logger;

        /// <summary>
        /// Service to retrieve technician information
        /// </summary>
        private static IWorkAssignService _workAssignService;

    }
}