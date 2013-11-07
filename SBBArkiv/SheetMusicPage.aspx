<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SheetMusicPage.aspx.cs" Inherits="SBBArkiv.SheetMusicPage" Theme="PlasticBlue" %>

<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridLookup" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v13.1, Version=13.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" EnableViewState="true" ID="lblMessages" />
    <div style="text-align: right">
        <asp:ImageButton ID="ibtExcelExport" runat="server" ImageUrl="Icons/export-xls-32x32.png" OnClick="ibtExcelExport_Click" />
        <asp:ImageButton ID="ibtPdfExport" runat="server" ImageUrl="Icons/export-pdf-32x32.png" OnClick="ibtPdfExport_Click" />
    </div>
    <dx:ASPxGridView ID="SheetMusicGrid" ClientInstanceName="SheetMusicGrid" runat="server" DataSourceID="SheetMusicSource" AutoGenerateColumns="False" KeyFieldName="Id" OnRowDeleting="SheetMusicGrid_RowDeleting"
        OnCustomButtonCallback="SheetMusicGrid_CustomButtonCallback" Width="100%" OnInit="SheetMusicGrid_Init" OnCellEditorInitialize="Grid_CellEditorInitialize" OnStartRowEditing="SheetMusicGrid_StartRowEditing"
        OnInitNewRow="SheetMusicGrid_InitNewRow" OnRowValidating="SheetMusicGrid_RowValidating">
        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="6%">
                <ClearFilterButton Visible="True" Image-Url="Icons/cancel-16x16.png" />
                <NewButton Visible="True" Image-Url="Icons/new-16x16.png" Image-SpriteProperties-Left="20" />
                <EditButton Visible="true" Image-Url="Icons/edit-16x16.png" />
                <DeleteButton Visible="True" Image-Url="Icons/delete-16x16.png" />
                <UpdateButton Image-Url="Icons/apply-16x16.png" />
                <CancelButton Image-Url="Icons/cancel-16x16.png" />
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="Id" Caption="Nummer" ReadOnly="True" VisibleIndex="1" Width="5%" />
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Tittel" VisibleIndex="2" ExportWidth="250" Settings-AutoFilterCondition="Contains" />
            <dx:GridViewDataTextColumn FieldName="Composer" Caption="Komponist" VisibleIndex="3" ExportWidth="120" Settings-AutoFilterCondition="Contains" />
            <dx:GridViewDataTextColumn FieldName="Arranger" Caption="Arrangør" VisibleIndex="4" ExportWidth="120" Settings-AutoFilterCondition="Contains" />
            <%--<dx:GridViewDataTextColumn FieldName="SoleSellingAgent" Caption="Forlag" VisibleIndex="5" Settings-AutoFilterCondition="Contains" />--%>
            <dx:GridViewDataTextColumn FieldName="MissingParts" Caption="Mangler" VisibleIndex="6" Settings-AutoFilterCondition="Contains" />
            <dx:GridViewDataComboBoxColumn FieldName="SheetMusicCategoryId" Caption="Kategori" VisibleIndex="7">
                <PropertiesComboBox ValueField="Id" ValueType="System.Int32" TextField="Name" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataCheckColumn FieldName="HasBeenScanned" Caption="Skannet" VisibleIndex="8" Width="5%" />
            <dx:GridViewCommandColumn Name="SendToAll" Caption="Send ut" ButtonType="Image" VisibleIndex="9">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnSendToAll" Text="Send ut sett til alle per e-post" Image-Url="Icons/mail-16x16.png" Visibility="BrowsableRow" />
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn Name="Download" Caption="Hent" ButtonType="Image" VisibleIndex="10">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnDownloadAll" Text="Last ned .zip-arkiv med alle stemmer" Image-Url="Icons/next-16x16.png" Visibility="BrowsableRow" />
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="PartsGrid" runat="server" DataSourceID="SheetMusic_StemmerDetailSource" AutoGenerateColumns="False" KeyFieldName="Id" OnBeforePerformDataSelect="PartsGrid_BeforePerformDataSelect"
                    OnRowDeleting="PartsGrid_RowDeleting" OnCellEditorInitialize="Grid_CellEditorInitialize" OnInit="PartsGrid_Init" OnRowInserting="PartsGrid_RowInserting" OnCustomColumnDisplayText="PartsGrid_CustomColumnDisplayText"
                    OnCustomButtonCallback="PartsGrid_CustomButtonCallback" Width="50%">
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="70px">
                            <ClearFilterButton Visible="True" Image-Url="Icons/cancel-16x16.png" />
                            <DeleteButton Visible="True" Image-Url="Icons/delete-16x16.png" />
                            <UpdateButton Image-Url="Icons/apply-16x16.png" />
                            <CancelButton Image-Url="Icons/cancel-16x16.png" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" Visible="false" VisibleIndex="1" />
                        <dx:GridViewDataComboBoxColumn FieldName="MusicPartId" Caption="Stemme" VisibleIndex="2">
                            <PropertiesComboBox ValueField="Id" ValueType="System.Int32" TextField="PartName" />
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn FieldName="Users_Unbound" Caption="Musikanter" UnboundType="String" VisibleIndex="3" ReadOnly="true">
                            <Settings AllowAutoFilter="False" AllowSort="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Hent">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="btnDown" Text="Last ned denne stemmen" Image-Url="Icons/next-16x16.png" Visibility="BrowsableRow" />
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn Name="SendPart" ButtonType="Image" Caption="Send">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="btnSend" Text="Send denne stemmen på e-post" Image-Url="Icons/mail-16x16.png" Visibility="BrowsableRow" />
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn Name="SendPartToAddress" ButtonType="Image" Caption="Send til adresse(r)">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="btnSendToAddress" Text="Send denne stemmen til angitt(e) e-postadresse(r)" Image-Url="Icons/person-16x16.png" Visibility="BrowsableRow" />
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                    </Columns>
                    <ClientSideEvents CustomButtonClick="
                                function(s, e) {
                                    if (e.buttonID == 'btnSend')
                                    {
                                        e.processOnServer = confirm('Vil du sende stemme per e-post?');
                                    }
                                    else if (e.buttonID == 'btnSendToAddress')
                                    {
                                        var addresses = prompt('Epostadresser (skill med komma)', '');

                                        if (addresses.length != 0)
                                        {
                                            var input = $get('EmailAddressInput');
                                            input.value = addresses;
                                            e.processOnServer = true;
                                        }
                                        else
                                        {
                                            e.processOnServer = false;
                                        }
                                    }
                                    else
                                    {
                                        clientCallback.PerformCallback('DownloadSinglePdf,' + s.GetRowKey(e.visibleIndex));
                                        e.processOnServer = false;
                                    }
                                }" />
                    <SettingsPager Visible="False" Mode="ShowAllRecords" />
                    <SettingsBehavior ConfirmDelete="true" />
                    <SettingsEditing Mode="Inline" />
                </dx:ASPxGridView>
                <br />
                <br />
                <dx:ASPxLabel ID="FileUploaderLabel" runat="server" Text="Last opp nye stemmer:" OnLoad="Control_Load" />
                <br />
                <dx:ASPxUploadControl ID="FileUploader" ClientInstanceName="uploader_AllParts" runat="server" Width="300px" ShowProgressPanel="True" UploadMode="Advanced" ShowUploadButton="true" OnFilesUploadComplete="FileUploader_FilesUploadComplete"
                    OnLoad="Control_Load">
                    <ValidationSettings AllowedFileExtensions=".pdf" />
                    <AdvancedModeSettings EnableMultiSelect="true" />
                    <BrowseButton Text="Velg filer..." />
                    <ClientSideEvents FilesUploadComplete="
                        function(s, e) { 
                            if (!e.IsValid && e.errorText != '')
                            {
                                alert(e.errorText);
                            }

                            SheetMusicGrid.Refresh(); 
                        }" />
                </dx:ASPxUploadControl>
            </DetailRow>
        </Templates>
        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" ShowFilterRowMenuLikeItem="true" />
        <SettingsDetail ShowDetailRow="true" ShowDetailButtons="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsBehavior ConfirmDelete="true" />
        <SettingsPager PageSize="30" />
        <ClientSideEvents CustomButtonClick="
                    function(s, e) {
                        if (e.buttonID == 'btnSendToAll') 
                        {
                            e.processOnServer = confirm('Vil du sende ut stemmer til alle musikanter?');
                        }
                        else  //get zip, no confirm
                        {    
                            clientCallback.PerformCallback('SendSheetAsZip,' + s.GetRowKey(e.visibleIndex));
                            e.processOnServer = false;
                        }
           }" />
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gridExporter" GridViewID="SheetMusicGrid" runat="server" ExportedRowType="All" Landscape="true" TopMargin="30" RightMargin="30" BottomMargin="30" LeftMargin="30" />
    <asp:HiddenField ID="EmailAddressInput" runat="server" ClientIDMode="Static" />
    <asp:EntityDataSource ID="SheetMusicSource" runat="server" EnableFlattening="False" ContextTypeName="SBBArkiv.MusicArchiveContext" EntitySetName="SheetMusic" EnableDelete="True" EnableInsert="True"
        EnableUpdate="True" OnContextCreating="EntityDatasource_ContextCreating" OnContextDisposing="EntityDatasource_ContextDisposing">
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="SheetMusic_StemmerDetailSource" runat="server" ContextTypeName="SBBArkiv.MusicArchiveContext" EnableDelete="True" EntitySetName="SheetMusicParts" EntityTypeFilter="SheetMusicPart"
        OnContextCreating="EntityDatasource_ContextCreating" OnContextDisposing="EntityDatasource_ContextDisposing" Where="it.[SheetMusicId] = @SheetMusicId" Include="MusicPart.Users" OrderBy="it.[MusicPartId]">
        <WhereParameters>
            <asp:SessionParameter DbType="Int32" Name="SheetMusicId" SessionField="SheetMusicId" />
        </WhereParameters>
    </asp:EntityDataSource>
    <dx:ASPxCallback ID="SheetMusicCallback" runat="server" ClientInstanceName="clientCallback" OnCallback="SheetMusicCallback_Callback" />
</asp:Content>
