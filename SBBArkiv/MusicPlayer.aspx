<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MusicPlayer.aspx.cs" Inherits="SBBArkiv.MusicPlayer" Theme="PlasticBlue" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridLookup" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblMessage" CssClass="bold" />
            <dx:ASPxGridView ID="UsersGrid" runat="server" AutoGenerateColumns="False" DataSourceID="MusicPlayerSource" KeyFieldName="Id" OnCustomColumnDisplayText="UsersGrid_CustomColumnDisplayText"
                OnRowUpdated="UsersGrid_RowUpdated" OnRowInserted="UsersGrid_RowInserted" OnRowDeleting="UsersGrid_RowDeleting" Width="100%" OnInit="UsersGrid_Init">
                <Columns>
                    <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="8%">
                        <ClearFilterButton Visible="True" Image-Url="Icons/cancel-16x16.png" />
                        <NewButton Visible="True" Image-Url="Icons/new-16x16.png" Image-SpriteProperties-Left="20" />
                        <EditButton Visible="true" Image-Url="Icons/edit-16x16.png" />
                        <DeleteButton Visible="True" Image-Url="Icons/delete-16x16.png" />
                        <UpdateButton Image-Url="Icons/apply-16x16.png" />
                        <CancelButton Image-Url="Icons/cancel-16x16.png" />
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="false" />
                    <dx:GridViewDataTextColumn FieldName="UserName" VisibleIndex="3" Caption="Brukernavn" Width="5%" />
                    <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2" Caption="Navn">
                        <PropertiesTextEdit>
                            <ValidationSettings>
                                <RequiredField IsRequired="true" ErrorText="Må fylles ut" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="4" Caption="Epost">
                        <PropertiesTextEdit>
                            <ValidationSettings>
                                <RequiredField IsRequired="true" ErrorText="Må fylles ut" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn FieldName="Inactive" VisibleIndex="6" Caption="Inaktiv" Width="5%" />
                    <dx:GridViewDataTextColumn FieldName="MusicParts_Unbound" Caption="Stemmer" UnboundType="String" VisibleIndex="7">
                        <EditItemTemplate>
                            <dx:ASPxGridLookup ID="PartsLookup" runat="server" KeyFieldName="Id" MultiTextSeparator="," TextFormatString="{1}" SelectionMode="Multiple" Width="300px" OnLoad="PartsLookup_Load" OnDataBound="PartsLookup_DataBound">
                                <GridViewProperties Settings-ShowFilterRow="true" SettingsPager-PageSize="20" />
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" ButtonType="Image">
                                        <ClearFilterButton Visible="true" Image-Url="Icons/cancel-16x16.png" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataColumn FieldName="Id" Visible="false" />
                                    <dx:GridViewDataTextColumn FieldName="PartName" />
                                </Columns>
                            </dx:ASPxGridLookup>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="UserGroupId" Caption="Brukergruppe" VisibleIndex="8" Width="10%">
                        <PropertiesComboBox ValueField="Id" ValueType="System.Int32" TextField="Name" />
                    </dx:GridViewDataComboBoxColumn>
                </Columns>
                <SettingsPager PageSize="50" />
                <Settings ShowFilterRow="True" />
                <SettingsCustomizationWindow Enabled="True" />
                <SettingsBehavior ConfirmDelete="true" />
                <SettingsEditing Mode="Inline" />
            </dx:ASPxGridView>
            <asp:EntityDataSource ID="MusicPlayerSource" runat="server" ContextTypeName="SBBArkiv.MusicArchiveContext" EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True"
                EntitySetName="Users" Include="MusicParts" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
