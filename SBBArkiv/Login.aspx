<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SBBArkiv.Login" Theme="PlasticBlue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Table runat="server" Width="60%" Height="100%" HorizontalAlign="Center">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Login ID="SbbLogin" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana"
                            Font-Size="Smaller" ForeColor="#333333" Height="200px" Width="250px" DestinationPageUrl="~/SheetMusicPage.aspx" DisplayRememberMe="False" FailureText="Feil brukernavn/passord. Prøv igjen."
                            InstructionText="Brukernavn = fornavn" LoginButtonText="Logg inn" MembershipProvider="SbbMembershipProvider" PasswordLabelText="Passord:" PasswordRequiredErrorMessage="Passord må fylles ut"
                            RememberMeText="Husk meg" TitleText="Logg inn" UserNameLabelText="Brukernavn:" UserNameRequiredErrorMessage="Brukernavn må fylles ut" OnLoggedIn="SbbLogin_LoggedIn">
                            <TitleTextStyle BackColor="#4b6c9e" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                            <HyperLinkStyle Font-Size="Small" HorizontalAlign="Center" />
                            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                            <TextBoxStyle Font-Size="Small" />
                            <LabelStyle Font-Size="Small" />
                            <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" ForeColor="#284775"
                                Width="100px" />
                        </asp:Login>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" Height="200px" Width="250px" Font-Names="Verdana" UserNameRequiredErrorMessage="Brukernavn må fylles ut"
                            UserNameFailureText="Finner ikke ditt brukernavn." UserNameInstructionText="Tast inn brukernavn for å få tilsendt nytt passord per e-post." TextLayout="TextOnLeft"
                            GeneralFailureText="Feil under genering av nytt passord." BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1" ForeColor="#333333"
                            BackColor="#F7F6F3" UserNameLabelText="Brukernavn:" UserNameTitleText="Glemt passord?" SubmitButtonText="Send passord" SuccessText="Nytt passord sendt">
                            <InstructionTextStyle Font-Italic="True" ForeColor="Black" Font-Size="Smaller" />
                            <TitleTextStyle BackColor="#4b6c9e" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
                            <TextBoxStyle Font-Size="Small" />
                            <LabelStyle Font-Size="Small" />
                            <SubmitButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" ForeColor="#284775"
                                Width="100px" />
                        </asp:PasswordRecovery>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
