using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Plugin_WorkAssignment.View.Dialog.ViewAbstraction;
using System.ComponentModel;
using System.Collections;
using Microsoft.Practices.Unity;
using Plugin_WorkAssignment.ServicesAPI;
using Logging;
using Plugin_WorkAssignment.ModelAPI;

namespace Plugin_WorkAssignment.View.Dialog.Presenter
{
    public class TechAssignmentDialogPresenter : ITechAssignmentDialogPresenter
    {
        public ITechAssignmentDialog View { get; set; }

        private Dictionary<string, string> TechNames = null;

        [InjectionMethod]
        public void Initialize(
            ILogger<TechAssignmentDialogPresenter> logger,
            IWorkAssignService workAssignService)
        {
            _logger = logger;
            _workAssignService = workAssignService;

            _logger.Debug("TechUtilizationGridControlPresenter::Initialize method.");
        }

        public string TranslateTechId(string id)
        {
            if (null == TechNames)
            {
                TechNames = new Dictionary<string, string>();
                List<TECHNICIAN> techs = _workAssignService.GetTechnicians(View.Shop);
                foreach (TECHNICIAN tech in techs)
                {
                    TechNames[Convert.ToString(tech.ID)] = tech.NAME;
                }

               
            }

            return TechNames[id];
        }


        public void LoadTechAssignments()
        {
            View.TechAssignments = new BindingList<TECHNICIAN_ASSIGNMENT>(_workAssignService.GetActivityAssignmentsByActIdAndShop(View.ActivityId, View.Shop, View.WorkDate));
        }

        public void UpdateTechAssignmentEntry()
        {
            _workAssignService.UpdateTechAssignments(View.TechAssignments, View.ActivityId, View.Shop, View.WorkDate);

            // TODO: Handle database error situations, above method should have Status returned.  Place below in the status check in future
            string javascriptClose = "CloseWindowAndRefresh();";
            this.View.Refresh = javascriptClose;
            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of this.View.Refresh = {0}", javascriptClose);
            }
        }

        private BindingList<TECHNICIAN_ASSIGNMENT> BuildTechAssignments(string actId)
        {

            BindingList<TECHNICIAN_ASSIGNMENT> assignments = new BindingList<TECHNICIAN_ASSIGNMENT>();

            return assignments;
        }

        public object GetTechnicians()
        {
            return _workAssignService.GetAvailableTechs(View.Shop, View.WorkDate);

        }

        public void AddNewAssignment(Hashtable assignment)
        {
            View.TechAssignments.Add(new TECHNICIAN_ASSIGNMENT()
            {
                TECHNICIAN_ID = Convert.ToDecimal(assignment["TECHNICIAN_ID"]),
                ASSIGNED_HRS = Convert.ToDecimal(assignment["ASSIGNED_HRS"]),
                WORK_DATE = View.WorkDate,
                ACTIVITY_ID = Convert.ToDecimal(View.ActivityId)

            });
        }

        public void DeleteAssignment(Decimal techId)
        {
            TECHNICIAN_ASSIGNMENT deletedAssignment = View.TechAssignments.FirstOrDefault(x=> x.TECHNICIAN_ID == techId);

            if (null != deletedAssignment)
            {
                View.TechAssignments.Remove(deletedAssignment);
            }
        }

        public void UpdateAssignment(decimal techId, decimal hours)
        {
            TECHNICIAN_ASSIGNMENT editedAssignment = View.TechAssignments.FirstOrDefault(x => x.TECHNICIAN_ID == techId);
            if (null != editedAssignment)
            {
                TECHNICIAN_ASSIGNMENT updatedAssignment = new TECHNICIAN_ASSIGNMENT
                {
                    ACTIVITY_ID = editedAssignment.ACTIVITY_ID,
                    TECHNICIAN_ID = editedAssignment.TECHNICIAN_ID,
                    WORK_DATE = editedAssignment.WORK_DATE,
                    ASSIGNED_HRS = hours
                };
                int index = View.TechAssignments.IndexOf(editedAssignment);
                View.TechAssignments.RemoveAt(index);
                View.TechAssignments.Insert(index, updatedAssignment);
            }

        }

        private static ILogger<TechAssignmentDialogPresenter> _logger;

        /// <summary>
        /// Service to retrieve technician information
        /// </summary>
        private static IWorkAssignService _workAssignService;
    }
}