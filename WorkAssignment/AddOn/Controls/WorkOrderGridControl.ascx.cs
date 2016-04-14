using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Microsoft.Practices.Unity;
using Logging;
using Plugin_WorkAssignment.View.Presenter;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Unity.Extensions;
using Plugin_WorkAssignment.Controls.Grid.Templates;
using WebControls.Grid.Templates;
using Plugin_WorkAssignment.ModelAPI;

namespace Plugin_WorkAssignment.Controls
{
    public partial class WorkOrderGridControl : System.Web.UI.UserControl, IWorkOrderGridControl
    {
        public IList<ACTIVITY> Activities
        {
            set
            {
                this.WorkAssignmentGrid.DataSource = value;
            }
        }

        /// <summary>
        /// Currently selected shop
        /// </summary>
        public string CurrentShop
        {
            get
            {
                string currentShop = null;
                if (null != ViewState["CurrentShop"])
                {
                    currentShop = (string)ViewState["CurrentShop"];
                }
                return currentShop;
            }
            set
            {
                if (null != value)
                {
                    bool shouldRebind = false;
                    if (null != ViewState["CurrentShop"])
                    {
                        string prevShop = (string)ViewState["CurrentShop"];
                        if (!String.Equals(prevShop, value))
                        {
                            shouldRebind = true;
                        }
                    }
                    else
                    {
                        shouldRebind = true;
                    }

                    ViewState["CurrentShop"] = value;
                    if (shouldRebind) {
                        this.WorkAssignmentGrid.DataBind();
                    }
                }
            
               
               
            }
        }

        /// <summary>
        /// Currently selected work date
        /// </summary>
        public DateTime? CurrentWorkDate
        {
            get
            {
                DateTime? currentShop = null;
                if (null != ViewState["CurrentDate"])
                {
                    currentShop = ViewState["CurrentDate"] as DateTime?;
                }
                return currentShop;
            }
            set
            {
                if (null != value)
                {
                    bool shouldRebind = false;
                    if (null != ViewState["CurrentDate"])
                    {
                        DateTime? prevDate = (DateTime?)ViewState["CurrentDate"];
                        if (!DateTime.Equals(prevDate, value))
                        {
                            shouldRebind = true;
                        }
                    }
                    else
                    {
                        shouldRebind = true;
                    }

                    ViewState["CurrentDate"] = value;
                    if (shouldRebind)
                    {
                        this.WorkAssignmentGrid.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// Current activity ID for check box click
        /// </summary>
        public decimal CurrentActivityID { get; set; }

        /// <summary>
        /// Currently states if overtime requirement is checked or not by current click
        /// </summary>
        public bool CurrentIsOTReq { get; set; }

        /// <summary>
        /// When any field that has a list of display field data, it will 
        /// be stored in here during binding of row data to be processed
        /// </summary>
        public string CurrentListFieldValue { get; set; }

        /// <summary>
        /// When any field that has a list of display field data, this is the processed
        /// display for it
        /// </summary>
        public string CurrentListFieldDisplay { get; set; }

        /// <summary>
        /// Name of current field being processed
        /// </summary>
        public string CurrentProcessedField { get; set; }

        /// <summary>
        /// Currently selected filter of area field
        /// </summary>
        public string CurrentAreaFilter
        {
            get
            {
                string currentAreaFilter = null;
                if (null != ViewState["CurrentArea"])
                {
                    currentAreaFilter = ViewState["CurrentArea"] as string;
                }
                return currentAreaFilter;
            }
            set
            {
                if (null != value)
                {
                    bool shouldRebind = false;
                    if (null != ViewState["CurrentArea"])
                    {
                        string prevAreaFilter = ViewState["CurrentArea"] as string;
                        if (String.Equals(prevAreaFilter, value) == false)
                        {
                            shouldRebind = true;
                        }
                    }
                    else
                    {
                        shouldRebind = true;
                    }

                    ViewState["CurrentArea"] = value;
                    if (shouldRebind)
                    {
                        this.WorkAssignmentGrid.DataBind();
                    }
                }
            }
        }

        #region Public methods
        /// <summary>
        /// Set logger for class
        /// </summary>
        /// <param name="logger">Logger for class</param>
        /// <param name="presenter">Presenter performing business logic for this view</param>
        [InjectionMethod]
        public void Initialize(ILogger<WorkOrderGridControl> logger, IWorkOrderGridControlPresenter presenter)
        {
            _logger = logger;
            _presenter = presenter;
            _presenter.View = this;

            _logger.Debug("WorkOrderGridControl::Initialize method.");
        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            GlobalUnityContainer.Container.BuildUp<WorkOrderGridControl>(this);


            _logger.Debug("WorkOrderGridControl::Page_Init method.");

            this.AddGridTemplateColumns();
            this.WorkAssignmentGrid.DataBinding +=new EventHandler(WorkAssignmentGrid_DataBinding);

        }

        protected void WorkAssignmentGrid_ItemCommand(object source, GridCommandEventArgs e) 
        {
            if (e.CommandName == "Update")
            {
                this.WorkAssignmentGrid.DataBind();
            }
            
        }

        protected void WorkAssignmentGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            _logger.Debug("WorkOrderGridControl::WorkAssignmentGrid_ItemDataBound method.");

            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                ACTIVITY act = (ACTIVITY)e.Item.DataItem;
                DateTime current = new DateTime(CurrentWorkDate.Value.Year, CurrentWorkDate.Value.Month, CurrentWorkDate.Value.Day);
                DateTime actStartDate = new DateTime(act.REMAIN_EARLY_START.Value.Year, act.REMAIN_EARLY_START.Value.Month, act.REMAIN_EARLY_START.Value.Day);
                if (!String.Equals(act.SHOP, this.CurrentShop, StringComparison.CurrentCultureIgnoreCase) 
                    || !DateTime.Equals(current, actStartDate))
                {  
                 
                    if (item.ItemType == GridItemType.Item)
                    {
                        item.CssClass = "rgRow disabled-row";
                    }
                    else if (item.ItemType == GridItemType.AlternatingItem)
                    {
                        item.CssClass = "rgAltRow disabled-row";
                    }

                }

                // Process any fields that value has a corresponding display name
                this.CurrentProcessedField = "AREA";
                this.CurrentListFieldValue = item[CurrentProcessedField].Text;
                _presenter.RetrieveDisplayName();
                item[CurrentProcessedField].Text = this.CurrentListFieldDisplay;
            }
        }


        private void AddGridTemplateColumns()
        {

             

            if (this.WorkAssignmentGrid.MasterTableView.Columns[5] is GridTemplateColumn) {
                ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[5]).ItemTemplate = new PredStatusWorkAssignmentColumnTemplate()
                {
                    DataField = ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[5]).DataField
                };
            }
            

            if (this.WorkAssignmentGrid.MasterTableView.Columns[8] is GridTemplateColumn)
            {
                this._techAssignmentColTemplate = new TechAssignmentColumnTemplate(this)
                {
                    DataField = ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[8]).DataField
                };
                ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[8]).ItemTemplate = this._techAssignmentColTemplate;
            }

            if (this.WorkAssignmentGrid.MasterTableView.Columns[9] is GridTemplateColumn)
            {
                ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[9]).ItemTemplate = new DayFromDateColumnTemplate()
                {
                    DataField = ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[9]).DataField
                };
            }

            if (this.WorkAssignmentGrid.MasterTableView.Columns[10] is GridTemplateColumn)
            {
                ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[10]).ItemTemplate = new EditStartDateTemplate(this)
                {
                    DataField = ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[10]).DataField
                };
            }

            if (this.WorkAssignmentGrid.MasterTableView.Columns[12] is GridTemplateColumn)
            {
                ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[12]).ItemTemplate = new OTReqColumnTemplate(this, ((GridTemplateColumn)this.WorkAssignmentGrid.MasterTableView.Columns[12]).DataField)
                {
                    AutoPostback = true,
                    ToolTip = "Click to specify whether or not overtime is required for this activity",
                    Click = new EventHandler(OTReq_Click)
                };
            }
        }

        /// <summary>
        /// Processes what user is specifying for whether overtime is required or not for specific activity
        /// </summary>
        /// <param name="sender">The row in the table</param>
        /// <param name="e">Arguments</param>
        protected void OTReq_Click(object sender, EventArgs e)
        {
            RadButton cb = sender as RadButton;
            this.CurrentIsOTReq = cb.Checked;
            GridDataItem item = cb.NamingContainer as GridDataItem;
            this.CurrentActivityID = Convert.ToDecimal(item.GetDataKeyValue("ID"));

            _presenter.ProcessOTReq();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void WorkAssignmentGrid_DataBinding(object source, EventArgs e)
        {
            _logger.Debug("WorkOrderGridControl::WorkAssignmentGrid_DataBinding method.");

            _presenter.GetWorkOrders();
        }

        protected void WorkAssignmentGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            _logger.Debug("WorkOrderGridControl::WorkAssignmentGrid_NeedDataSource method.");

            _presenter.GetWorkOrders();
        }

        private IWorkOrderGridControlPresenter _presenter;

        private static ILogger<WorkOrderGridControl> _logger;
        private TechAssignmentColumnTemplate _techAssignmentColTemplate;

        public void Rebind()
        {
            this.WorkAssignmentGrid.Rebind();
        }
    }
}