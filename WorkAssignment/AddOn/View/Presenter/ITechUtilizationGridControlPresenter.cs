using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.View.ViewAbstraction;

namespace Plugin_WorkAssignment.View.Presenter
{
    public interface ITechUtilizationGridControlPresenter
    {
        #region Methods
        /// <summary>
        /// Retrieve daily technician utilization
        /// </summary>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        void GetDailyTechUtilizations();

        /*
        /// <summary>
        /// Setup display of data
        /// </summary>
        void Display();

        /// <summary>
        /// Processes the look of utilization percentage of technicians
        /// </summary>
        void ProcessUtilizationPercentage();
         */
        #endregion

        #region Properties
        /// <summary>
        /// Tech utilization view presenter performs business logic for
        /// </summary>
        ITechUtilizationGridControl View { get; set; }
        #endregion
    }
}
