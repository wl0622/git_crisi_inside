<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userpwd.aspx.cs" Inherits="inside.admin.web.aspx.userpwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />

    <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/jsencrypt.min.js"></script>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <%--page-----%>
    <script type="text/javascript" src="/js/page/upwd.js"></script>
</head>
<body>
    <form id="form1">
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="#">系统管理</a></li>
                <li>
                    <img src="/images/main/arrow.gif" /></li>
                <li><a href="#">密码修改</a></li>
            </ul>
        </div>
        <div class="formbody">
            <ul class="forminfo">
                <li>
                    <label>原密码</label><input name="oldPwd" id="oldPwd" type="password" class="dfinput" /></li>
                <li>
                    <label>新密码</label><input name="newPwd" id="newPwd" type="password" class="dfinput" /></li>
                <li>
                    <label>确认新密码</label><input name="again" id="again" type="password" class="dfinput" /></li>
                <li>
                    <label>&nbsp;</label><input type="button" class="scbtn" value="确认修改" style="height: 35px;" /></li>
            </ul>
        </div>
    </form>
</body>
</html>
