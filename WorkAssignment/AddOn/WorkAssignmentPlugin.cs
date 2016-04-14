using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PluginAPI.Base;
using PluginAPI;
using System.ComponentModel.Composition;
using PluginAPI.Attributes;
using PluginAPI.VPP;
using Unity.Extensions;
using Microsoft.Practices.Unity;
using Plugin_WorkAssignment.View.Presenter;
using Logging;
using System.Web.UI;
using Plugin_WorkAssignment.View.Dialog.Presenter;
using Plugin_WorkAssignment.ModelAPI;
using Plugin_WorkAssignment.Model;
using Plugin_WorkAssignment.ServicesAPI;
using Plugin_WorkAssignment.Services;
using System.Web.Configuration;
using System.Reflection;
using System.Configuration;
using ServicesAPI.Plugin;
using ServicesAPI.Configuration;

[assembly: WebResource("Plugin_WorkAssignment.Styles.WorkAssignmentStyles.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Plugin_WorkAssignment.Images.CalendarNextPrev.png", "image/png")]

namespace Plugin_WorkAssignment
{
    [Export(typeof(IPlugin)),
        ExportMetadata("UniqueName", "WorkAssignment"),
        ExportMetadata("Description", "Work Assignment"),
        ExportMetadata("Version", "1.0"),
        ExportMetadata("RequiresVersion", "2013 B1")]

    [PluginRoleName("Group")]
    [WebResourceProviderPluginRootMenu("~/Work/", "Work Assignment", "Work Assignment", "WORKASSIGNMENT_MENU_RK")]
    [WebResourceProviderPluginDialogPage("~/Work/", "View/Dialog/TechAssignmentDialog.aspx", "TechAssignment", "Tech Assignment")]
    [WebResourceProviderPluginChildPage("~/Work/", "View/WorkAssignment.aspx", "Work Assignment", "Work Assignment")]

    [WebResourceProviderPluginGenericResource("~/Work/", "View/Dialog/TechAssignmentDialog.ascx")]
    [WebResourceProviderPluginGenericResource("~/Work/", "Controls/TechUtilizationChartControl.ascx")]
    [WebResourceProviderPluginGenericResource("~/Work/", "Controls/TechUtilizationGridControl.ascx")]
    [WebResourceProviderPluginGenericResource("~/Work/", "Controls/WorkOrderGridControl.ascx")]

    public class WorkAssignmentPlugin : AbstractRootMenuWebResourceProviderPlugin
    {
        public override void Initialize(IPluginService pluginService)
        {
            base.Initialize(pluginService);

            ILogger<WorkAssignmentPlugin> logger = GlobalUnityContainer.Container.Resolve<ILogger<WorkAssignmentPlugin>>();

            logger.Debug("WorkAssignmentPlugin::Initialize");
            GlobalUnityContainer.Container.RegisterType<IWorkAssignmentPresenter, WorkAssignmentPresenter>();
            GlobalUnityContainer.Container.RegisterType<IWorkOrderGridControlPresenter, WorkOrderGridControlPresenter>();
            GlobalUnityContainer.Container.RegisterType<ITechUtilizationChartControlPresenter, TechUtilizationChartControlPresenter>();
            GlobalUnityContainer.Container.RegisterType<ITechUtilizationGridControlPresenter, TechUtilizationGridControlPresenter>();
            GlobalUnityContainer.Container.RegisterType<ITechAssignmentDialogPresenter, TechAssignmentDialogPresenter>();
            GlobalUnityContainer.Container.RegisterType<IVW_TECHNICIAN_UTILIZATIONRepository, VW_TECHNICIAN_UTILIZATIONRepository>();
            GlobalUnityContainer.Container.RegisterType<IVW_TECHNICIAN_AVAILABILITYRepository, VW_TECHNICIAN_AVAILABILITYRepository>();
            GlobalUnityContainer.Container.RegisterType<IVW_TECHNICIAN_ASSIGN_AVAILRepository, VW_TECHNICIAN_ASSIGN_AVAILRepository>();
            GlobalUnityContainer.Container.RegisterType<IACTIVITYRepository, ACTIVITYRepository>();
            GlobalUnityContainer.Container.RegisterType<ITECHNICIAN_ASSIGNMENTRepository, TECHNICIAN_ASSIGNMENTRepository>();
            GlobalUnityContainer.Container.RegisterType<ITECHNICIANRepository, TECHNICIANRepository>();
            GlobalUnityContainer.Container.RegisterType<IUnitOfWork, EFUnitOfWork>();
            GlobalUnityContainer.Container.RegisterType<IObjectContext, DBObjectContext>();
            GlobalUnityContainer.Container.RegisterType<IWorkAssignService, WorkAssignService>();
        }
    }
}