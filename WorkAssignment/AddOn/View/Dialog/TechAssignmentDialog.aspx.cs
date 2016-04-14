using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PluginAPI.VPP;
using Plugin_WorkAssignment.View.Dialog.ViewAbstraction;
using Microsoft.Practices.Unity;
using Logging;
using Plugin_WorkAssignment.View.Dialog.Presenter;
using Utilities;
using System.ComponentModel;
using System.Collections;
using Plugin_WorkAssignment.ModelAPI;
using Syntempo.Web;

namespace Plugin_WorkAssignment.View.Dialog
{
    public partial class TechAssignmentDialog : System.Web.UI.Page, ITechAssignmentDialog
    {
        public BindingList<TECHNICIAN_ASSIGNMENT> TechAssignments
        {
            set
            {
                Session["TechAssignmentDialog.TechAssignments"] = value;
            }
            get
            {
                if (null == Session["TechAssignmentDialog.TechAssignments"])
                {
                    BindingList<TECHNICIAN_ASSIGNMENT> bl = new BindingList<TECHNICIAN_ASSIGNMENT>();
                    bl.AllowNew = true;
                    Session["TechAssignmentDialog.TechAssignments"] = bl;
                }
                return Session["TechAssignmentDialog.TechAssignments"] as BindingList<TECHNICIAN_ASSIGNMENT>;
            }
        }

        public string ActivityId
        {
            get
            {
                return Request.Params["aId"];
            }
        }

        public string Shop
        {
            get
            {
                return Request.Params["sId"];
            }
        }


        public DateTime WorkDate
        {
            get
            {
                return DateTime.Parse(Request.Params["dId"]);
            }
        }

        public string Duration
        {
            get
            {
                return Request.Params["hId"];
            }
        }

        /// <summary>
        /// Save and refresh the full page via javascript
        /// </summary>
        public string Refresh
        {
            set
            {
                this.ClientScript.RegisterStartupScript(GetType(), "key", value, true);
            }
        }

        [InjectionMethod]
        public void Initialize(ILogger<TechAssignmentDialog> logger, ITechAssignmentDialogPresenter presenter)
        {
            _logger = logger;
            _presenter = presenter;
            _presenter.View = this;

            _logger.Debug("TechAssignmentDialog::Initialize method.");
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Page.MasterPageFile = Popup.NAME;
            
            
        }

        public string TranslateTechId(object dataItem) {
            Decimal id = (Decimal)DataBinder.Eval(dataItem, "TECHNICIAN_ID");
            return _presenter.TranslateTechId(Convert.ToString(id));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _presenter.LoadTechAssignments();
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            _logger.Debug("TechAssignmentDialog::Save_Click method.");

            // Only save data if session has not expired and if server side validation for any date/time fields is good
            if ((this.Session[SyntempoConstants.USER_SESSION_EXPIRED] == null) && (Page.IsValid == true))
            {
                _presenter.UpdateTechAssignmentEntry();
            }
        }

        protected void TechAssignmentGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            _logger.Debug("TechAssignmentDialog::TechAssignmentGrid_NeedDataSource method.");
            
            this.TechAssignmentGrid.DataSource = this.TechAssignments;
        }

        protected void TechAssignmentGrid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            _logger.Debug("TechAssignmentDialog::TechAssignmentGrid_ItemDataBound method.");

            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                GridEditManager editMan = editedItem.EditManager;
                RadTextBox assignedHours = editedItem.FindControl("AssignHours") as RadTextBox;
                if (String.IsNullOrWhiteSpace(assignedHours.Text))
                {
                    assignedHours.Text = this.Duration;
                }
            }
        }

        protected void TechAssignmentGrid_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
           
            if (e.CommandName == "PerformInsert")
            {
                this._presenter.AddNewAssignment(GetItemValues(e.Item as GridDataInsertItem));
            }
            else if (e.CommandName == "Delete")
            {
                GridDataItem gdi = e.Item as GridDataItem;
                this._presenter.DeleteAssignment(Convert.ToDecimal(gdi.GetDataKeyValue("TECHNICIAN_ID")));
            }
            else if (e.CommandName == "Update")
            {
                GridDataItem editedItem = e.Item as GridDataItem;
                Hashtable table = GetItemValues(editedItem);
                Decimal techId = Convert.ToDecimal(table["TECHNICIAN_ID"]);
                Decimal hours = Convert.ToDecimal(table["ASSIGNED_HRS"]);
                this._presenter.UpdateAssignment(techId, hours);
                this.TechAssignmentGrid.DataBind();
            }
            
        }

        private Hashtable GetItemValues(GridDataItem editedItem)
        {
            Hashtable values = new Hashtable();
            editedItem.ExtractValues(values);
            return values;
        }

        

        protected void TechAssignmentGrid_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

            if (e.Item is GridEditableItem && (e.Item as GridEditableItem).IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
              

                RadComboBox techCombo = (RadComboBox)editedItem.FindControl("ComboTechAssign");

                techCombo.DataSource = _presenter.GetTechnicians();
                techCombo.DataTextField = "TECH_NAME";
                techCombo.DataValueField = "TECH_ID";

              
            }
        }

        private ITechAssignmentDialogPresenter _presenter;

        private static ILogger<TechAssignmentDialog> _logger;
    }
}