<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="inside.admin.web.main" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>网站后台管理系统</title>
</head>
<frameset rows="79,*" cols="*" frameborder="no" border="0" framespacing="0">
  <frame src="top.aspx" name="topFrame" scrolling="No" noresize="noresize" id="topFrame" title="topFrame" />
  <frameset cols="175,*" frameborder="no" border="0" framespacing="0">
    <frame src="left.html" name="leftFrame" scrolling="No" noresize="noresize" id="leftFrame" title="leftFrame" />
    <frame src="aspx/index.aspx" name="rightFrame" id="rightFrame" title="rightFrame"/>
  </frameset>
</frameset>
<noframes><body>
</body></noframes>
</html>
