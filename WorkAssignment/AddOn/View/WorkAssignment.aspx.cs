using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PluginAPI.VPP;
using Microsoft.Practices.Unity;
using Plugin_WorkAssignment.View.Presenter;
using Logging;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Syntempo.Web;
using Telerik.Web.UI;
using WebControls.Toolbar.Navigation;
using Utilities;
using WebControls.Toolbar.Templates;
using Utilities.HttpContext;
using Plugin_WorkAssignment.Controls.Toolbar.Templates;
using Utilities.Web;
using Plugin_WorkAssignment.Controls;
using Telerik.Web.UI.Calendar;
using Utilities.Extensions;

namespace Plugin_WorkAssignment.View
{
    public partial class WorkAssignment : System.Web.UI.Page, IWorkAssignment
    {
        public IDictionary<String, String> ShopList
        {
            set
            {
                
                this._shopDropDown.DataSource = value;
                this._shopDropDown.DataTextField = "Value";
                this._shopDropDown.DataValueField = "Key";
                SiteMaster master = (SiteMaster)Master;
                ((ToolbarButtonSection)master.Toolbar.Sections["Shop"]).DataBind();
            }
        }


        public IDictionary<String, String> AreaList
        {
            set
            {
                this._areaDropDown.DataSource = value;
                this._areaDropDown.DataTextField = "Value";
                this._areaDropDown.DataValueField = "Key";
                SiteMaster master = (SiteMaster)Master;
                ((ToolbarButtonSection)master.Toolbar.Sections["Area"]).DataBind();
            }
        }

        /// <summary>
        /// User date time format for date of work
        /// </summary>
        public string DateTimeFormat
        {
            get
            {
                return Profile.Current.Date.Format;
            }
        }

        public string TechAssignDialogURL
        {
            get
            {
                return "Dialog/TechAssignmentDialog.aspx";
            }
        }

        #region Public methods
        /// <summary>
        /// Set logger for class
        /// </summary>
        /// <param name="logger">Logger for class</param>
        /// <param name="presenter">Presenter performing business logic for this view</param>
        [InjectionMethod]
        public void Initialize(ILogger<WorkAssignment> logger, IWorkAssignmentPresenter presenter)
        {
            _logger = logger;
            _presenter = presenter;
            _presenter.View = this;

            _logger.Debug("WorkAssignment::Initialize method.");
        }
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Page.MasterPageFile = SiteMaster.NAME;
          

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            ((SiteMaster)this.Master).Toolbar.Visible = true;
            RadStyleSheetManager styleSheetManager = ((SiteMaster)this.Master).SiteMasterStyleSheetManager;
            string assemblyName = GetType().Assembly.GetName().Name;
           
            //TODO Move this into plugin initialization
            styleSheetManager.StyleSheets.Add(new StyleSheetReference() { Assembly = "Plugin_WorkAssignment", Name = "Plugin_WorkAssignment.Styles.WorkAssignmentStyles.css" });

            this.PopulateToolbar();
            _presenter.GetShops();
            this._datePicker.SelectedDate = DateTime.Now;

            _presenter.GetAreas();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            _logger.Debug("WorkAssignment::Page_Load method.");

            // Set values to user controls
            this.SetShop(this._shopDropDown.SelectedValue);
            this.SetWorkDate(this._datePicker.SelectedDate);
            this.SetAreaFilter(this._areaDropDown.SelectedValue);

            /*
            if (this.refreshTechUtilGrid.Value == SyntempoConstants.TRUE)
            {
                // Force refresh of technician util grid
                ((TechUtilizationGridControl)this.TechUtGrid).ForceRefresh = true;
                this.refreshTechUtilGrid.Value = SyntempoConstants.FALSE;
            }
             */
        }

        /// <summary>
        /// Handles changing of shop
        /// </summary>
        /// <param name="source">Shop drop down</param>
        /// <param name="e">Selected value</param>
        protected void ShopSelectionChanged(object source, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            _logger.Debug("WorkAssignment::ShopSelectionChanged method.");

            // Set value of selected shop into all grid and chart controls
            this.SetShop(e.Value);
        }

        /// <summary>
        /// Handles area filter updated
        /// </summary>
        /// <param name="source">Area drop down</param>
        /// <param name="e">Selected value</param>
        protected void AreaFilterSelectionChanged(object source, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            _logger.Debug("WorkAssignment::AreaFilterSelectionChanged method.");

            // Set value of selected area filter for main grid
            this.SetAreaFilter(e.Value);
        }

        /// <summary>
        /// Handles changing of work date
        /// </summary>
        /// <param name="source">Work date control</param>
        /// <param name="e">Selected date value</param>
        protected void WorkDateSelectionChanged(object source, SelectedDateChangedEventArgs e)
        {
            _logger.Debug("WorkAssignment::WorkDateSelectionChanged method.");

            this.SetWorkDate(e.NewDate);
        }

        /// <summary>
        /// Set the shop into all user controls
        /// </summary>
        /// <param name="shop">Current shop selected</param>
        private void SetShop(string shop)
        {
            ((TechUtilizationGridControl)this.TechUtGrid).CurrentShop = shop;
            ((TechUtilizationChartControl)this.TechUtChart).CurrentShop = shop;
            ((WorkOrderGridControl)this.WOGrid).CurrentShop = shop;
        }

        /// <summary>
        /// Set the area filter for the top main grid
        /// </summary>
        /// <param name="areaFilter">Area value to filter on</param>
        private void SetAreaFilter(string areaFilter)
        {
            ((WorkOrderGridControl)this.WOGrid).CurrentAreaFilter = areaFilter;
        }

        /// <summary>
        /// Set the work date into all user controls
        /// </summary>
        /// <param name="shop">Current work date picked</param>
        private void SetWorkDate(DateTime? workDate)
        {
            ((TechUtilizationGridControl)this.TechUtGrid).CurrentWorkDate = workDate;
            ((TechUtilizationChartControl)this.TechUtChart).CurrentWorkDate = workDate;
            ((WorkOrderGridControl)this.WOGrid).CurrentWorkDate = workDate;
        }

        private void PopulateToolbar()
        {
            _logger.Debug("WorkAssignment::PopulateToolBar method.");
            SiteMaster master = (SiteMaster)Master;



            _shopDropDown = new ToolbarDropDownTemplate();
            _shopDropDown.DropDownLabelText = "Shop: ";
            _shopDropDown.SelectedIndexChangedHandler = ShopSelectionChanged;
            master.Toolbar.Sections.Add(new ToolbarButtonSection
            {
                ID = "Shop",
                EnableEmbeddedSkins = false,
                Skin = "Mystique",
                Direction = WebControls.Toolbar.DisplayDirection.LEFT,
                CssClass = "ShopDropDown",
                Items = 
				{ 
					new RadToolBarButton
					{
						Text = "",
						Value = "SHOP_DROP_DOWN",
						ItemTemplate = _shopDropDown
					}
                },
            });


            master.Toolbar.Sections.Add(new ToolbarButtonSection
            {
                ID = "PreviousDayButton",
                EnableEmbeddedSkins = false,
                Skin = "Mystique",
                Direction = WebControls.Toolbar.DisplayDirection.LEFT,
                CssClass = "DatePickerButton PrevButton",
                Items = 
				{ 
					new RadToolBarButton
					{
						Text = "",
						Value = "PREV_DAY",
                        ToolTip = "Previous Day",
						EnableImageSprite=true
					}
                },
            });

            ((ToolbarButtonSection)master.Toolbar.Sections["PreviousDayButton"]).ButtonClicked += new RadToolBarEventHandler(OnPreviousDayButtonCommand);

            _datePicker = new Plugin_WorkAssignment.Controls.Toolbar.Templates.ToolbarDatePickerTemplate() { SelectedDateChanged = WorkDateSelectionChanged };
            master.Toolbar.Sections.Add(new ToolbarButtonSection
            {
                ID = "DatePicker",
                EnableEmbeddedSkins = false,
                Skin = "Mystique",
                Direction = WebControls.Toolbar.DisplayDirection.LEFT,
                CssClass = "ScheduleDatePicker",
                Items = 
				{ 
					new RadToolBarButton
					{
						Text = "",
						Value = "SCHEDULE_DATE",
						ItemTemplate = _datePicker
					}
                },
            });


            master.Toolbar.Sections.Add(new ToolbarButtonSection
            {
                ID = "NextDayButton",
                EnableEmbeddedSkins = false,
                Skin = "Mystique",
                Direction = WebControls.Toolbar.DisplayDirection.LEFT,
                CssClass = "DatePickerButton NextButton",
                Items = 
				{ 
					new RadToolBarButton
					{
						Text = "",
						Value = "NEXT_DAY",
                         ToolTip = "Next Day",
						EnableImageSprite=true
					}
                },
            });

            ((ToolbarButtonSection)master.Toolbar.Sections["NextDayButton"]).ButtonClicked += new RadToolBarEventHandler(OnNextDayButtonCommand);

            _areaDropDown = new ToolbarDropDownTemplate();
            _areaDropDown.DropDownLabelText = "Area Filter: ";
            _areaDropDown.SelectedIndexChangedHandler = AreaFilterSelectionChanged;
            _areaDropDown.DataBound = AreaDropDownDataBound;
            master.Toolbar.Sections.Add(new ToolbarButtonSection
            {
                ID = "Area",
                EnableEmbeddedSkins = false,
                Skin = "Mystique",
                Direction = WebControls.Toolbar.DisplayDirection.LEFT,
                CssClass = "ShopDropDown",
                Items = 
				{ 
					new RadToolBarButton
					{
						Text = "",
						Value = "AREA_DROP_DOWN",
						ItemTemplate = _areaDropDown
					}
                },
            });

          
            //2) Actions
            ToolbarMenuSection oMenuSection = new ToolbarMenuSection
            {
                ID = "PageActions",
                CssClass = "Zindex ActionsRoot",
                Direction = WebControls.Toolbar.DisplayDirection.RIGHT,
                Skin = "Mystique",
                EnableEmbeddedSkins = false,

                Items =
				{
					new RadMenuItem 
					{ 
						ToolTip = "Page Actions",
                        Value = "PAGE_ACTIONS",
						PostBack = false,
                        
						Items =
						{
							new RadMenuItem { Text = "Export To Excel...", Value = SyntempoConstants.EXCEL },
							new RadMenuItem { Text = "Export To CSV...", Value = SyntempoConstants.CSV },
							new RadMenuItem { Text = "Export To PDF...", Value = SyntempoConstants.PDF }
						}
					}
				}
            };



            // Only allow hiding and showing of filter if filtering is allowed


            //Add menu section to the toolbar

            master.Toolbar.Sections.Add(oMenuSection);
            
               this._layoutMenu = this.BuildLayoutMenu();
                this._layoutDisplay = this.BuildLayoutDisplay();

                master.Toolbar.Sections.Add(this._layoutDisplay);
                master.Toolbar.Sections.Add(this._layoutMenu);

               // this._layoutMenu.DataBinding += new EventHandler(Toolbar_LayoutMenuDataBinding);
               // ((ToolbarMenuSection)this._layoutMenu).ItemClicked += new RadMenuEventHandler(LayoutMenu_ItemClicked);

                ((ToolbarMenuSection)this._layoutMenu).OnClientLoad = "OnMenuLoaded";
            
            {
                

               // this._filterMenu.DataBinding += new EventHandler(Toolbar_FilterMenuDataBinding);
               // ((ToolbarMenuSection)this._filterMenu).ItemClicked += new RadMenuEventHandler(Toolbar_PageActionItemClicked);
            }

            //Attach ItemClick event to the section that will do something on postback.
            //If "ViewConfig" section need to do something on postback, uncomment the second event and write codes to handle it.
            //((ToolbarMenuSection)master.Toolbar.Sections["PageActions"]).ItemClicked += new RadMenuEventHandler(Toolbar_PageActionItemClicked);

            
        }

        private ToolbarSection BuildFilterDisplay()
        {
            ToolbarReadOnlyTextBoxTemplate filterTextBox = new ToolbarReadOnlyTextBoxTemplate();
            filterTextBox.DefaultDisplay = ResourceExtensions.GetGlobalResxObject("WebResources", "NoFilterParen", "(No Filter)");
            filterTextBox.ToolTip = "Current Filter";
            //filterTextBox.TextBoxDataBindingHandler = new EventHandler(FilterDisplayNameDataBinding);
            ToolbarButtonSection filterDisplay = new ToolbarButtonSection
            {
                ID = "CurrentFilter",
                EnableEmbeddedSkins = false,
                Skin = "Mystique",
                Direction = WebControls.Toolbar.DisplayDirection.RIGHT,
                CssClass = "current-filter-textbox",
                Items = 
				    { 
					    new RadToolBarButton
					    {
						    Text = "",
						    Value = "CURRENT_FILTER_TEXTBOX",
						    ItemTemplate = filterTextBox
					    },
				    },
            };

            return filterDisplay;
        }



        private ToolbarButtonSection BuildLayoutDisplay()
        {
            ToolbarReadOnlyTextBoxTemplate layoutDisplayName = new ToolbarReadOnlyTextBoxTemplate();
            layoutDisplayName.ID = "UserLayoutDisplayName";
           // layoutDisplayName.TextBoxDataBindingHandler = new EventHandler(this.LayoutDisplayNameDataBinding);
            layoutDisplayName.DefaultDisplay = ResourceExtensions.GetGlobalResxObject("WebResources", "Base", "Base");
            layoutDisplayName.ToolTip = "Current Layout";
            ToolbarButtonSection layoutDisplay = new ToolbarButtonSection
            {
                ID = "CurrentLayout",
                EnableEmbeddedSkins = false,
                Skin = "Mystique",
                Direction = WebControls.Toolbar.DisplayDirection.RIGHT,
                CssClass = "current-layout-textbox",
                Items = 
				    { 
					    new RadToolBarButton
					    {
						    Text = "",
						    Value = "CURRENT_LAYOUT_TEXTBOX",
						    ItemTemplate = layoutDisplayName
					    },
				    },
            };

            return layoutDisplay;

        }

        private ToolbarSection BuildFilterMenu()
        {
            _logger.Debug("View::BuildFilterMenu method.");

            ToolbarMenuSection filterMenuSection = new ToolbarMenuSection
            {
                ID = "Filters",
                CssClass = "Zindex FilterRoot",
                Direction = WebControls.Toolbar.DisplayDirection.RIGHT,
                Skin = "Mystique",
                EnableEmbeddedSkins = false,
                Items =
				{
					new RadMenuItem 
					{ 
						ToolTip = "Filters",
                        Value = "FILTERS",
						PostBack = false
                        

                    }
                }
            };

            filterMenuSection.OnClientItemClicking = "OpenFilterDialog";

            return filterMenuSection;
        }

        private ToolbarSection BuildLayoutMenu()
        {
            _logger.Debug("View::BuildLayoutMenu method.");

            ToolbarMenuSection layoutMenuSection = new ToolbarMenuSection
            {
                ID = "Layouts",
                CssClass = "Zindex LayoutRoot",
                Direction = WebControls.Toolbar.DisplayDirection.RIGHT,
                Skin = "Mystique",
                EnableEmbeddedSkins = false,
                Items =
				{
					new RadMenuItem 
					{ 
						ToolTip = "Layouts",
                        Value = "LAYOUTS",
						PostBack = false
                        

                    }
                }
            };





            //ToolTip = ResourceExtensions.GetGlobalResxObject("WebResources", "SaveConfiguration", "Save Configuration"), Value = "SAVE_CONFIG", CausesValidation = false, CssClass="SaveButton", EnableImageSprite=true }

            layoutMenuSection.OnClientItemClicking = "OnLayoutItemClicked";
            return layoutMenuSection;
        }

        protected void OnPreviousDayButtonCommand(Object sender, EventArgs e)
        {
            _logger.Debug("View::OnPreviousDayButtonCommand method.");
            if (this._datePicker.SelectedDate.HasValue)
            {
                this._datePicker.SelectedDate = this._datePicker.SelectedDate.Value.AddWorkDays(-1);
                this.SetWorkDate(this._datePicker.SelectedDate);
            }
        }

        protected void OnNextDayButtonCommand(Object sender, EventArgs e)
        {
            _logger.Debug("View::OnNextDayButtonCommand method.");
            if (this._datePicker.SelectedDate.HasValue)
            {
                this._datePicker.SelectedDate = this._datePicker.SelectedDate.Value.AddWorkDays(1);
                this.SetWorkDate(this._datePicker.SelectedDate);
            }
        }
        
        /// <summary>
        /// Add default no filter field
        /// </summary>
        /// <param name="sender">Drop down control</param>
        /// <param name="e">Arguments</param>
        protected void AreaDropDownDataBound(object sender, EventArgs e)
        {
            RadComboBox areaDropDown = (RadComboBox)sender;
            areaDropDown.Items.Insert(0, new RadComboBoxItem("-- No Area Filter --", ""));
        }

        #region Private variables
        /// <summary>
        /// Presenter that performs all business logic for this page
        /// </summary>
        private IWorkAssignmentPresenter _presenter;
        private ToolbarDropDownTemplate _shopDropDown;
        private ToolbarDropDownTemplate _areaDropDown;
        private Plugin_WorkAssignment.Controls.Toolbar.Templates.ToolbarDatePickerTemplate _datePicker;
        private ToolbarSection _filterMenu;
        private ToolbarSection _layoutMenu;
        private ToolbarSection _layoutDisplay;
        private ToolbarSection _filterDisplay;

        #region Private static variables
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static ILogger<WorkAssignment> _logger;

 
        #endregion
        #endregion
    }
}