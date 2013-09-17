<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MusicParts.aspx.cs" Inherits="SBBArkiv.MusicParts" Theme="PlasticBlue" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblMessage" CssClass="bold" />
            <dx:ASPxGridView runat="server" ID="MusicPartsGrid" KeyFieldName="Id" DataSourceID="MusicPartsSource" Width="50%">
                <Columns>
                    <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                        <ClearFilterButton Visible="True" Image-Url="Icons/cancel-16x16.png" />
                        <NewButton Visible="True" Image-Url="Icons/new-16x16.png" Image-SpriteProperties-Left="20" />
                        <EditButton Visible="true" Image-Url="Icons/edit-16x16.png" />
                        <DeleteButton Visible="True" Image-Url="Icons/delete-16x16.png" />
                        <UpdateButton Image-Url="Icons/apply-16x16.png" />
                        <CancelButton Image-Url="Icons/cancel-16x16.png" />
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn FieldName="PartName" Caption="Stemme">
                        <PropertiesTextEdit>
                            <ValidationSettings>
                                <RequiredField IsRequired="true" ErrorText="Må fylles ut" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Aliases" Caption="Aliaser (brukes ved opplasting)" />
                </Columns>
                <Settings ShowFilterRow="True" />
                <SettingsPager PageSize="50" />
                <SettingsBehavior ConfirmDelete="true" />
                <SettingsEditing Mode="Inline" />
            </dx:ASPxGridView>
            <asp:EntityDataSource ID="MusicPartsSource" runat="server" ContextTypeName="SBBArkiv.MusicArchiveContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntitySetName="MusicParts"
                OnContextCreating="EntityDatasource_ContextCreating" OnContextDisposing="EntityDatasource_ContextDisposing" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
