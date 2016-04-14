
<%@ Assembly Name="Plugin_WorkAssignment" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkAssignment.aspx.cs" ViewStateEncryptionMode="Always" Inherits="Plugin_WorkAssignment.View.WorkAssignment" %>

<%@ Register src="~/Work/Controls/TechUtilizationChartControl.ascx" tagname="TechUtilChart" tagprefix="workassign" %>
<%@ Register src="~/Work/Controls/TechUtilizationGridControl.ascx" tagname="TechUtilGrid" tagprefix="workassign" %>
<%@ Register src="~/Work/Controls/WorkOrderGridControl.ascx" tagname="WorkOrderGrid" tagprefix="workassign" %>


<asp:Content ID="Content2" ContentPlaceHolderID="AdditionalCSSRef" runat="server">

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            // Determine if activity details pane is availableTechAssignmentsToString
            function pageLoad() {
              
             



            }

            function OTReqCheckChanged(sender, args) {
                trace("OTReqCheckChanged", sender, args);
            }

            function BottomRadPaneBeforeCollapse(sender, args) {
                //trace("BottomRadPaneBeforeCollapse", sender, args);
                var menu = $find($(".ActionsRoot").attr("id"));
                var menuItem = menu.findItemByText("Hide Details Pane");
                if (menuItem) {
                    menu.trackChanges();
                    menuItem.set_value('SHOW_DETAILS_PANE');
                    menuItem.set_text('Show Details Pane');
                    menu.commitChanges();
                }
            }

            function BottomRadPaneBeforeExpand(sender, args) {
                //trace("BottomRadPaneBeforeExpand", sender, args);
                var menu = $find($(".ActionsRoot").attr("id"));
                var menuItem = menu.findItemByText("Show Details Pane");
                if (menuItem) {
                    menu.trackChanges();
                    menuItem.set_value('HIDE_DETAILS_PANE');
                    menuItem.set_text('Hide Details Pane');
                    menu.commitChanges();
                }


            }

            function BottomRadPaneExpanded(sender, args) {
                trace("BottomRadPaneExpanded", sender, args);
                $(window).trigger('resize');
            }

            function OpenAssignDialog(id, shop, date, duration) {
                var oWnd = window.radopen("<%= this.TechAssignDialogURL %>?aId=" + id  + "&sId=" + shop + "&dId=" + date + "&hId=" + duration, 'AssignDialog', 600, 400);

            }

            function AssignDialogClose(oWnd, args) {
                trace("AssignDialogClose", oWnd, args);
                if ((args.get_argument() != null) && (args.get_argument().refresh)) {
                    // Hidden variable specifies to also refresh technician utilization grid via code behind
                    $('[id$=refreshTechUtilGrid]').val("true");
                    refreshWorkOrderGrid();
                }
            }

            function BottomSplitterLoaded(splitter) {

                var leftPane = splitter.GetStartPane();
                resizeLeftChart(leftPane);

                var rightPane = splitter.GetEndPane();
                resizeRightChart(rightPane);
            }

            function RightPaneResized(pane) {
                resizeRightChart(pane);
            }

            function LeftPaneResized(pane) {
                resizeLeftChart(pane);
            }
        </script>
    </telerik:RadCodeBlock>

   <asp:HiddenField ID="refreshTechUtilGrid" runat="server" Value="false" />
   <telerik:RadSplitter ID="WorkAssignmentSplitter" VisibleDuringInit="false" runat="server" Width="100%" Orientation="Horizontal" Skin="Default" OnClientLoad="SplitterLoaded" CssClass="page-splitter">
	    <%-- Top level grid --%>
        <telerik:RadPane ID="TopPane" runat="server" ClientIDMode="Static" Scrolling="none" EnableEmbeddedSkins="false" Skin="Mystique" OnClientResized="TopPaneResize" MinHeight="100" >
		 <workassign:WorkOrderGrid runat="server" ID="WOGrid" />
	    </telerik:RadPane>

        <%-- Splitter between top level grid and bottom activity detail tabs--%>
		<telerik:RadSplitBar ID="RadTopSplitBar" runat="server" CollapseMode="Backward" EnableResize="true" />

	    <%-- Activity detail tabs section --%>
	    <telerik:RadPane ID="BottomPane" runat="server" Scrolling="none" OnClientResized="BottomPaneResize" Height="250" OnClientBeforeCollapse="BottomRadPaneBeforeCollapse" OnClientBeforeExpand="BottomRadPaneBeforeExpand" OnClientExpanded="BottomRadPaneExpanded">
	      <telerik:RadSplitter ID="BottomPaneSplitter" VisibleDuringInit="false" runat="server" Width="100%" Orientation="Vertical" Skin="Default"  CssClass="page-splitter" OnClientLoaded="BottomSplitterLoaded">
           <telerik:RadPane ID="BottomLeftPane" runat="server" ClientIDMode="Static" Scrolling="none" EnableEmbeddedSkins="false" Skin="Mystique" OnClientResized="LeftPaneResized" Width="50%" >
		     <workassign:TechUtilGrid runat="server" ID="TechUtGrid" />
	    </telerik:RadPane>

        <%-- Splitter between top level grid and bottom activity detail tabs--%>
		<telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Backward" EnableResize="true" />
          <telerik:RadPane ID="BottomRightPane" runat="server" ClientIDMode="Static" Scrolling="none" EnableEmbeddedSkins="false" Skin="Mystique" OnClientResized="RightPaneResized" Width="50%" >
		   <workassign:TechUtilChart runat="server" ID="TechUtChart" />
	    </telerik:RadPane>
          </telerik:RadSplitter>
	    </telerik:RadPane>
    </telerik:RadSplitter>

     <telerik:RadWindowManager ID="RadWindowManager_WorkAssign" runat="server" EnableEmbeddedSkins="false" Skin="Mystique" VisibleStatusbar="false" VisibleOnPageLoad="false" DestroyOnClose="false" ReloadOnShow="false" ShowContentDuringLoad="false">
                    <Windows>
                        <telerik:RadWindow runat="server" AutoSize="false"  EnableShadow="false"  Modal="true"  Overlay="true" 
                            ID="AssignDialog"  Width="600" Height="400" CssClass="ActionDialog" Style="display: none;" Behaviors="Move, Close" InitialBehaviors="None" OnClientClose="AssignDialogClose" >
                              
                            
                        </telerik:RadWindow>
                        

                      
                    </Windows>
                </telerik:RadWindowManager>
</asp:Content>