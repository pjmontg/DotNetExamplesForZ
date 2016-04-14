using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Extensions;
using Microsoft.Practices.Unity;
using Logging;
using Plugin_WorkAssignment.View.Presenter;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Plugin_WorkAssignment.ModelAPI;
using Telerik.Web.UI;

namespace Plugin_WorkAssignment.Controls
{
    public partial class TechUtilizationChartControl : System.Web.UI.UserControl, ITechUtilizationChartControl
    {
        #region Public methods
        /// <summary>
        /// Set logger for class
        /// </summary>
        /// <param name="logger">Logger for class</param>
        /// <param name="presenter">Presenter performing business logic for this view</param>
        [InjectionMethod]
        public void Initialize(ILogger<TechUtilizationChartControl> logger, ITechUtilizationChartControlPresenter presenter)
        {
            _logger = logger;
            _presenter = presenter;
            _presenter.View = this;

            _logger.Debug("TechUtilizationGridControl::Initialize method.");
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Weeks worth utilizations totals of a shop
        /// </summary>
        public List<UtilizationChartData> WeeklyTechUtilization
        {
            set
            {
                this.TechUtilizationChart.DataSource = value;
                this.TechUtilizationChart.DataBind();
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
        #endregion

        #region Protected methods
        /// <summary>
        /// Setups injection for control
        /// </summary>
        /// <param name="sender">Caller</param>
        /// <param name="e">Arguments</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            GlobalUnityContainer.Container.BuildUp<TechUtilizationChartControl>(this);

            _logger.Debug("TechUtilizationGridControl::Page_Init method.");
        }

        /// <summary>
        /// Display grid based on date and shop values
        /// </summary>
        /// <param name="sender">Caller</param>
        /// <param name="e">Arguments</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            _logger.Debug("TechUtilizationGridControl::Page_PreRender method.");

            

            _presenter.GetWeeklyUtilizations();

            //_presenter.Display();
        }
        #endregion

        #region Private variables
        /// <summary>
        /// Presenter that performs all business logic for this page
        /// </summary>
        private ITechUtilizationChartControlPresenter _presenter;

        #region Private static variables
        /// <summary>
        /// Logger for class
        /// </summary>
        private static ILogger<TechUtilizationChartControl> _logger;
        #endregion
        #endregion
    }
}