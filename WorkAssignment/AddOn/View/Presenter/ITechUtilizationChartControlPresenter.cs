using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.View.ViewAbstraction;

namespace Plugin_WorkAssignment.View.Presenter
{
    public interface ITechUtilizationChartControlPresenter
    {
        #region Methods
        /// <summary>
        /// Retrieve total assigned hours and available hours for technician's for a shop for a given week
        /// </summary>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        void GetWeeklyUtilizations();
        #endregion

        #region Properties
        /// <summary>
        /// Assigned hours and technicians week view presenter performs business logic for
        /// </summary>
        ITechUtilizationChartControl View { get; set; }
        #endregion);
    }
}
