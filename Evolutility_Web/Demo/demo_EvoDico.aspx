<%@ Page validateRequest="false"  Language="C#" MasterPageFile="zmDemo.master" AutoEventWireup="true" 
CodeFile="demo.aspx.cs" Inherits="demo" 
Title="Evolutility :: Metadata for Evolutility Demos" 
CodeFileBaseClass="BasePage"  
Menus="demo_md"
SubMenuID="210"
  %>
<%@ Register Assembly="Evolutility.UIServer" Namespace="Evolutility" TagPrefix="EVOL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<H1><IMG hspace="5" src="../PixEvo/cube.gif" border="0">&nbsp;Metadata for Evolutility Demos</H1>
		
			<EVOL:UIServer ID="evo1" runat="server" BackColor="#EDEDED" BackColorRowMouseOver="Beige"
				CssClass="main1" DBAllowExport="true" DBReadOnly="true" DesignDisplayMode="View"
				DesignWebPath="C:\Inetpub\wwwroot\Evolutility\" DisplayModeStart="List" RowsPerPage="50"
				SecurityModel="Single_User" ToolbarPosition="Top" VirtualPathPictures="../PixEvo/"
				VirtualPathToolbar="../PixEvo/" Width="100%" XMLfile="xml/evodico_form.xml" /> 

			<p><a href="EvoDemo/evodico_form.xml" target="db"><img src="../PixEvo/tag.png" class="icon" alt="XML"/> XML definition of this application</a>.</p>


</asp:Content>

