<%@ Assembly Name="Plugin_WorkAssignment" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderGridControl.ascx.cs"
    Inherits="Plugin_WorkAssignment.Controls.WorkOrderGridControl" %>
<telerik:RadScriptBlock runat="server">
    <script type="text/javascript">
        function refreshWorkOrderGrid() {
            var masterTable = $find("<%= WorkAssignmentGrid.ClientID %>").get_masterTableView();
            masterTable.rebind();
        }

    </script>
</telerik:RadScriptBlock>
<telerik:RadGrid runat="server" ID="WorkAssignmentGrid" OnNeedDataSource="WorkAssignmentGrid_NeedDataSource"
    OnItemDataBound="WorkAssignmentGrid_ItemDataBound" AllowPaging="true" EnableEmbeddedSkins="false"
    Skin="Mystique" PageSize="25" AllowSorting="true" AllowFilteringByColumn="false"
    CssClass="UserListingGrid" EnableViewState="false" EnableLinqExpressions="false"
    AutoGenerateColumns="false">
    <ClientSettings Scrolling-UseStaticHeaders="true" Scrolling-AllowScroll="true" AllowGroupExpandCollapse="true">
        <ClientMessages DragToGroupOrReorder="" />
    </ClientSettings>
    <MasterTableView DataKeyNames="ID, SHOP, START_DATE, TechAssignmentsToString"
        EnableColumnsViewState="false" GroupLoadMode="Client">
        <GroupByExpressions>
            <telerik:GridGroupByExpression>
                <SelectFields>
                    <telerik:GridGroupByField FieldAlias="CUSTOM_ATTR_STRING_21" FieldName="CUSTOM_ATTR_STRING_21"
                        HeaderText="WO" HeaderValueSeparator=": "></telerik:GridGroupByField>
                    <telerik:GridGroupByField FieldAlias="CUSTOM_ATTR_STRING_22" FieldName="CUSTOM_ATTR_STRING_22"
                        HeaderText="Desc" HeaderValueSeparator=": "></telerik:GridGroupByField>
                </SelectFields>
                <GroupByFields>
                    <telerik:GridGroupByField FieldName="CUSTOM_ATTR_STRING_21" SortOrder="Descending">
                    </telerik:GridGroupByField>
                </GroupByFields>
            </telerik:GridGroupByExpression>
        </GroupByExpressions>
        <PagerStyle AlwaysVisible="true" NextPageToolTip="<%$ Resources:WebResources, NextPage %>"
            NextPagesToolTip="<%$ Resources:WebResources, LastPage %>" PrevPageToolTip="<%$ Resources:WebResources, PreviousPage %>"
            PrevPagesToolTip="<%$ Resources:WebResources, FirstPage %>" PagerTextFormat="<%$ Resources:WebResources, RadGridPagerInfo %>" />
        <Columns>
            <telerik:GridBoundColumn DataField="ACT_NUM" HeaderText="WO#" UniqueName="ACT_NUM"
                HeaderTooltip="Work Order #" />
            <telerik:GridBoundColumn DataField="DESCRIPTION" HeaderText="Maintenance Activities for "
                HeaderStyle-Width="15%" UniqueName="DESCRIPTION" HeaderTooltip="Maintenance Activities for " />
            <telerik:GridBoundColumn DataField="AREA" HeaderText="Area" UniqueName="AREA"
                HeaderTooltip="Area" />
            <telerik:GridBoundColumn DataField="CUSTOM_ATTR_STRING_23" HeaderText="Equipment Number"
                UniqueName="CUSTOM_ATTR_STRING_23" HeaderTooltip="Equipment Number" />
            <telerik:GridBoundColumn DataField="CUSTOM_ATTR_STRING_24" HeaderText="Equipment Description"
                HeaderStyle-Width="15%" UniqueName="CUSTOM_ATTR_STRING_24" HeaderTooltip="Equipment Description" />
            <telerik:GridTemplateColumn DataField="PRED_STATUS_CODE" HeaderText="Pred Status"
                UniqueName="PRED_STATUS_CODE" HeaderTooltip="Predecessor Status" />
            <telerik:GridBoundColumn DataField="ORIG_DURATION" HeaderText="Task Duration" UniqueName="ORIG_DURATION"
                HeaderStyle-Width="4%" ItemStyle-HorizontalAlign="Center" HeaderTooltip="Task Duration" />
            <telerik:GridBoundColumn DataField="CUSTOM_ATTR_INT_5" HeaderText="# of Resources" UniqueName="CUSTOM_ATTR_INT_5"
                HeaderStyle-Width="4%" ItemStyle-HorizontalAlign="Center" HeaderTooltip="Number of people required" />
            <telerik:GridTemplateColumn DataField="TechAssignmentsToString" HeaderText="Resources"
                UniqueName="TechAssignmentsToString" HeaderStyle-Width="250" HeaderTooltip="Resources" />
            <telerik:GridTemplateColumn DataField="REMAIN_EARLY_START" HeaderText="Day" UniqueName="StartDay"
                HeaderStyle-Width="4%" HeaderTooltip="Day of the week" />
            <telerik:GridTemplateColumn DataField="REMAIN_EARLY_START" HeaderText="Start Date" UniqueName="REMAIN_EARLY_START"
                HeaderTooltip="Date"  HeaderStyle-Width="120"/>
            <telerik:GridBoundColumn DataField="CUSTOM_ATTR_STRING_25" HeaderText="Compliance Code"
                ItemStyle-HorizontalAlign="Center" UniqueName="CUSTOM_ATTR_STRING_25" HeaderTooltip="Compliance Code" />
            <telerik:GridTemplateColumn DataField="CUSTOM_ATTR_BOOL_5" HeaderText="OT Req'd"
                UniqueName="CUSTOM_ATTR_BOOL_5" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                HeaderTooltip="Overtime Required" />
 
        </Columns>
    </MasterTableView>
</telerik:RadGrid>
