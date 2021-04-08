<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="Test.WebForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
</head>
<body>
    <form id="form1" runat="server">
         <div>  
    <table align="center">  
            <tr>  
                <td colspan="2">  
                    <h3>Registration using Role</h3>  
                </td>  
            </tr>  
            <tr>  
                <td>  
                    <asp:Label ID="Label1" runat="server" Text="UserName:" Font-Bold="True" Width="100px" BackColor="#FFFF66" ForeColor="#FF3300"></asp:Label>  
                </td>  
                <td>  
                    <asp:TextBox ID="txt_UserName" runat="server" Width="150px"></asp:TextBox>  
                </td>  
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter UserName" ControlToValidate="txt_UserName"></asp:RequiredFieldValidator>
                </td>  
            </tr>  
            <tr>  
                <td>  
                    <asp:Label ID="Label2" runat="server" Text="Password:" Font-Bold="True" Width="100px" BackColor="#FFFF66" ForeColor="#FF3300"></asp:Label>  
                </td>  
                <td>  
                    <asp:TextBox ID="txt_Password"  TextMode="Password" runat="server" Width="150px"></asp:TextBox>  
                </td>  
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Password" ControlToValidate="txt_Password"></asp:RequiredFieldValidator> 
                </td>  
            </tr>  
            <tr>  
                <td>  
                    <asp:Label ID="Label3" runat="server" Text="Role:" Font-Bold="True" Width="100px"  Height="100px" BackColor="#FFFF66" ForeColor="#FF3300"></asp:Label>  
                </td>  
                <td>  
                    <asp:RadioButtonList ID="rbtRole" runat="server" RepeatDirection="Vertical">  
                        <asp:ListItem>Admin</asp:ListItem>  
                        <asp:ListItem>FreeUser</asp:ListItem>  
                        <asp:ListItem>PaidUser</asp:ListItem>  
                    </asp:RadioButtonList>  
                </td>  
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Select role" ControlToValidate="rbtRole"></asp:RequiredFieldValidator>
                </td>  
            </tr>  
            <tr>  
            <td></td>  
                <td>  
                    <asp:Button ID="btn_register" runat="server" BackColor="#CCFF99" Text="Register" />  
                </td>  
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx" ForeColor="#663300">Click here to Login</asp:HyperLink>
                </td>  
            </tr>  
            <tr>  
                <td align="center"  colspan="2">  
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>  
                </td>  
            </tr>  
        </table>  
    </div>  
    </form>
</body>
</html>
