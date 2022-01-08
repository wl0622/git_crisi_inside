<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="inside.admin.web.top" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //顶部导航切换
            $(".nav li a").click(function () {
                $(".nav li a.selected").removeClass("selected")
                $(this).addClass("selected");
            })
        })
    </script>
</head>

<body style="background: url(images/main/top_bg.gif) repeat-x;">


    <div class="topleft" style="width: 500px; float: left">
        <a href="javascript:void()" target="_parent">
            <img src="images/main/top_left.jpg" title="系统首页" /></a>
    </div>
    <div style="width: 450px; float: right; margin-top: 47px;">
        <span style="background-image: url(images/main/top_right_b1.gif); width: 150px; height: 32px; display: block; float: left"></span>
        <span style="background-image: url(images/main/top_right_b2.gif); width: 220px; height: 32px; line-height: 32px; display: block; float: left">
            <h5 style="color: #fff; vertical-align: middle">登录帐号: <%=loginName %></h5>
        </span>
        <a href="/login/login.aspx?logout=off" target="_parent"><span style="background-image: url(images/main/top_right_b3.gif); width: 50px; height: 32px; display: block; float: left"></span></a>

        <span style="background-image: url(images/main/top_right_b5.gif); width: 30px; height: 32px; display: block; float: left"></span>
    </div>
</body>
</html>
