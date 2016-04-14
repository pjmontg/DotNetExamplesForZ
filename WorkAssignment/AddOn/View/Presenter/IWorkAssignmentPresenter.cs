using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.View.ViewAbstraction;

namespace Plugin_WorkAssignment.View.Presenter
{
    public interface IWorkAssignmentPresenter
    {
        IWorkAssignment View { get; set; }
        void GetShops();
        void GetAreas();
    }
}
