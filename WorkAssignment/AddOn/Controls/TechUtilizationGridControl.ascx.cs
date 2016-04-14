using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Logging;
using Plugin_WorkAssignment.View.Presenter;
using Microsoft.Practices.Unity;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Unity.Extensions;
using System.Data;
using System.Drawing;

namespace Plugin_WorkAssignment.Controls
{
    public partial class TechUtilizationGridControl : System.Web.UI.UserControl, ITechUtilizationGridControl
    {
        #region Public methods
        /// <summary>
        /// Set logger for class
        /// </summary>
        /// <param name="logger">Logger for class</param>
        /// <param name="presenter">Presenter performing business logic for this view</param>
        [InjectionMethod]
        public void Initialize(ILogger<TechUtilizationGridControl> logger, ITechUtilizationGridControlPresenter presenter)
        {
            _logger = logger;
            _presenter = presenter;
            _presenter.View = this;

            _logger.Debug("TechUtilizationGridControl::Initialize method.");
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Daily tech utilization list
        /// </summary>
        public object DailyTechUtilization
        {
            set
            {
                this.TechUtilizationGridChart.DataSource = value;
                this.TechUtilizationGridChart.DataBind();
            }
        }

        /// <summary>
        /// Currently selected shop
        /// </summary>
        public string CurrentShop { get; set; }

        /// <summary>
        /// Currently selected work date
        /// </summary>
        public DateTime? CurrentWorkDate { get; set; }

        /*
        /// <summary>
        /// True means to refresh data
        /// </summary>
        public bool ForceRefresh
        {
            get
            {
                return this._forceRefresh;
            }

            set
            {
                this._forceRefresh = value;
                if (value == true)
                {
                    this.TechUtilizationGrid.DataSource = null;
                    this.TechUtilizationGrid.Rebind();
                }
            }
        }
        private bool _forceRefresh;

        /// <summary>
        /// Shop value before postback processing
        /// </summary>
        public string PreviousShop
        {
            get
            {
                return this.ViewState[PREVIOUS_SHOP] as string;
            }

            set
            {
                this.ViewState[PREVIOUS_SHOP] = value;
            }
        }

        /// <summary>
        /// Work date value before postback processing
        /// </summary>
        public DateTime? PreviousWorkDate
        {
            get
            {
                return (DateTime?)(this.ViewState[PREVIOUS_WORK_DATE]);
            }

            set
            {
                this.ViewState[PREVIOUS_WORK_DATE] = value;
            }
        }

        /// <summary>
        /// True if page is initially loaded (no postbacks)
        /// </summary>
        public bool IsInitialLoad
        {
            get
            {
                return !(this.IsPostBack);
            }
        }

        /// <summary>
        /// The current data being bound to the grid
        /// </summary>
        public GridDataItem CurrentData { get; set; }
         */
        #endregion

        #region Protected methods
        /// <summary>
        /// Injects concrete classes
        /// </summary>
        /// <param name="sender">Caller</param>
        /// <param name="e">Arguments</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            GlobalUnityContainer.Container.BuildUp<TechUtilizationGridControl>(this);

            _logger.Debug("TechUtilizationGridControl::Page_Init method.");

        }

        /*
        /// <summary>
        /// Binds data to grid
        /// </summary>
        /// <param name="source">Caller</param>
        /// <param name="e">Arguments</param>
        protected void TechUtilizationGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            _logger.Debug("TechUtilizationGridControl::TechUtilizationGrid_NeedDataSource method.");

            if (this.ForceRefresh == true)
            {
                _presenter.GetDailyTechUtilizations();
            }
        }
         */

        /// <summary>
        /// Force rebind of grid if shop or work date is changed
        /// </summary>
        /// <param name="source">Caller</param>
        /// <param name="e">Arguments</param>
        protected void Page_PreRender(object source, EventArgs e)
        {
            _logger.Debug("TechUtilizationGridControl::Page_PreRender method.");

            //_presenter.Display();
            _presenter.GetDailyTechUtilizations();
        }

        /*
        /// <summary>
        /// Processes the look of utilization percentage of technicians by coloring the value
        /// based on if the value is less than, equal to, or greater than 100%
        /// </summary>
        protected void TechUtilizationGrid_ItemDatabound(object sender, GridItemEventArgs e)
        {
            _logger.Debug("TechUtilizationGridControl::TechUtilizationGrid_ItemDatabound method.");

            if (e.Item is GridDataItem)
            {
                this.CurrentData = e.Item as GridDataItem;
                _presenter.ProcessUtilizationPercentage();
            }
        }
         */
        #endregion

        #region Private variables
        /// <summary>
        /// Presenter that performs all business logic for this page
        /// </summary>
        private ITechUtilizationGridControlPresenter _presenter;

        /*
        #region Private const variables
        /// <summary>
        /// Index into view state for shop before postback processing
        /// </summary>
        private const string PREVIOUS_SHOP = "TechUTGrid_PREV_SHOP";

        /// <summary>
        /// Index into view state for work date before postback processing
        /// </summary>
        private const string PREVIOUS_WORK_DATE = "TechUTGrid_PREV_WORK_DATE";
        #endregion
         */

        #region Private static variables
        /// <summary>
        /// Logger for class
        /// </summary>
        private static ILogger<TechUtilizationGridControl> _logger;
        #endregion
        #endregion
    }
}