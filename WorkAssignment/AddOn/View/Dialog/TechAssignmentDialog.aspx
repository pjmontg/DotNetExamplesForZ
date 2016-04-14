<%@ Assembly Name="Plugin_WorkAssignment" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TechAssignmentDialog.aspx.cs"
    Inherits="Plugin_WorkAssignment.View.Dialog.TechAssignmentDialog" %>

<asp:content contentplaceholderid="PopupContentPlaceHolder" runat="server" id="PopupContent">
     <telerik:RadGrid EnableEmbeddedSkins="false" Width="100%" Height="310"
                                Skin="Mystique" ID="TechAssignmentGrid" ShowGroupPanel="false"
                                ShowStatusBar="false" AllowPaging="True" AllowMultiRowEdit="false" OnNeedDataSource="TechAssignmentGrid_NeedDataSource"
                                 OnItemCommand="TechAssignmentGrid_ItemCommand" OnItemCreated="TechAssignmentGrid_ItemCreated" EnableViewState="false"
                                 CssClass="TechAssignmentGrid" OnItemDataBound="TechAssignmentGrid_ItemDataBound"
                                runat="server" BorderStyle="None" >
                                 <ClientSettings Scrolling-UseStaticHeaders="true" Scrolling-AllowScroll="true">
        <ClientMessages DragToGroupOrReorder="" />
    </ClientSettings>
                                <MasterTableView  EditMode="InPlace" CommandItemDisplay="Top" DataKeyNames="TECHNICIAN_ID, ASSIGNED_HRS" EnableColumnsViewState="false"
                                     AutoGenerateColumns="False">
                                    <Columns>
                                             <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" 
                                             CancelImageUrl="~/Images/Cancel.gif"
                                             InsertImageUrl="~/Images/Update.gif"
                                             UpdateImageUrl="~/Images/Update.gif"
                                             EditImageUrl="~/Images/Edit.gif"
                                             
                                            HeaderTooltip="<%$ Resources:WebResources, EditRecord %>">
                                            <ItemStyle CssClass="MyImageButton" />
                                        </telerik:GridEditCommandColumn>
                                        <telerik:GridTemplateColumn HeaderText="Resource"
                                            DataField="Name" HeaderTooltip="Resource" DataType="System.String"
                                            UniqueName="Name" >
                                                <ItemTemplate>
                                                <asp:Label Text='<%# TranslateTechId(Container.DataItem) %>' ID="LabelName" runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadComboBox ID="ComboTechAssign" runat="server" SelectedValue='<%#Bind("TECHNICIAN_ID") %>'
                                                    EnableEmbeddedSkins="false" Skin="Mystique" CssClass="EditMode" />
                                            </EditItemTemplate>
                                            
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn HeaderText="Hours"
                                            DataField="Hours" HeaderTooltip="Hours" DataType="System.String"
                                            UniqueName="Hours" >
                                               <ItemTemplate>
                                                <asp:Label Text='<%# Eval("ASSIGNED_HRS") %>' ID="LabelAssignHours" runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="AssignHours" runat="server" Text='<%#Bind("ASSIGNED_HRS") %>'/>
                                            </EditItemTemplate>
                                            
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn ConfirmText="Unassign this resource?" ConfirmDialogType="RadWindow"
                                            ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Unassign" HeaderText="Unassign"
                                            HeaderTooltip="Unassign" UniqueName="DeleteColumn" ImageUrl="~/Images/Delete.gif">
                                            <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                    <CommandItemSettings ShowAddNewRecordButton="true" ShowRefreshButton="true" AddNewRecordText="Add new assignment" />
                                </MasterTableView>
                                
                                <PagerStyle AlwaysVisible="true" NextPageToolTip="<%$ Resources:WebResources, NextPage %>"
            NextPagesToolTip="<%$ Resources:WebResources, LastPage %>" PrevPageToolTip="<%$ Resources:WebResources, PreviousPage %>"
            PrevPagesToolTip="<%$ Resources:WebResources, FirstPage %>" PagerTextFormat="<%$ Resources:WebResources, RadGridPagerInfo %>" />
                            </telerik:RadGrid>

                            
        <div style=" text-align:right; float: right; position:fixed; bottom: 6px; right: 6px;">
            <asp:Button CssClass="standard-button" ID="Save" runat="server" Text="Save" OnClick="Save_Click" />
            <asp:Button CssClass="standard-button" ID="Cancel" runat="server" Text="Cancel" CausesValidation="false"
                UseSubmitBehavior="false" OnClientClick="CloseWindow(); return false;" />
        </div>
        </asp:content>
        <asp:Content ContentPlaceHolderID="AdditionalScriptContentPlaceHolder" runat="server"
    ID="ScriptContent">
    <script type="text/javascript">
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz az well)

            return oWindow;
        }

        function CloseWindow(button, args) {
            var oWnd = GetRadWindow();

            oWnd.argument = args;
            oWnd.close();


        }

        function CloseWindowAndRefresh() {
            var oWnd = GetRadWindow();

            var obj = new Object();
            obj.refresh = true;

            oWnd.close(obj);
        }
    </script>
    </asp:content>
