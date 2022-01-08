<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="inside.inside.admin.weblogin.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>系统登录</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="js/jquery-3.1.1-min.js"></script>
    <script type="text/javascript" src="/js/jsencrypt.min.js"></script>
    <%--lib--%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <script type="text/javascript">

        $(function () {

            $("#yzmRefresh").click(function () {
                var valiCode = $(".yzm-img").attr("src", "/ashx/ValidateCode.ashx?method=validateCode&time=" + (new Date()).getTime());
            });


            $('#send-btn').on('click', function () {


                if ($('#username').val().trim().length == 0 || $('#username').val() == $('#username').attr("data-msg")) {
                    layer.msg($('#username').attr("data-msg"), { icon: 7, time: 2000 });
                    return;
                }
                if ($('#password').val().trim().length == 0 || $('#password').val().trim() == $('#password').attr("data-msg")) {
                    layer.msg($('#password').attr("data-msg"), { icon: 4, time: 2000 });
                    return;
                }
                if ($('#code').val().trim().length == 0 || $('#code').val().trim() == $('#code').attr("data-msg")) {
                    layer.msg($('#code').attr("data-msg"), { icon: 5, time: 2000 });
                    return;
                }

                layer.load();

                $(".error-box").text("正在登录验证...");


                var data = {};
                data.username = $('#username').val();
                data.password = $('#password').val();
                data.code = $('#code').val();
                var result = $.encryptRequest({ data: data });
                $.ajax({
                    url: '/ashx/login.ashx?method=validate',
                    dataType: 'json',
                    type: 'post',
                    data: result,
                    success: function (res) {

                        if (res.status == "ok") {

                            window.location.href = res.message;
                        }
                        else {

                            layer.msg(res.message, { icon: 2, time: 2000 });
                            $("#yzmRefresh").trigger("click")

                        }
                    },
                    complete: function () {

                        $(".error-box").text("");
                        layer.closeAll('loading');
                    },
                    error: function () {
                        alert("异常！");
                    }
                })

                //$.ajax({
                //    type: "POST",
                //    dataType: "json",
                //    url: "/ashx/login.ashx?method=validate",
                //    //$('#loginFrom').serialize(),
                //    data: {
                //        username: $("#username").val(),
                //        password: $("#password").val(),
                //        code: $("#code").val()
                //    },
                //    success: function (data) {
                //        if (data.status == "ok") {
                //            window.location.href = data.message;
                //        }
                //        else {
                //            layer.msg(data.message, { icon: 2, time: 2000 });
                //            $("#yzmRefresh").trigger("click")
                //        }
                //    },
                //    complete: function () {
                //        $(".error-box").text("");
                //        layer.closeAll('loading');
                //    },
                //    error: function () {
                //        alert("异常！");
                //    }
                //});
            })

        });



        function bodyKeydown() {

            if (event.keyCode == 13)
                $('#send-btn').trigger("click");
        }


    </script>
</head>

<body onkeydown="bodyKeydown();">
    <div class="main">
        <div class="login">
            <h1>内网管理系统</h1>
            <div class="inset">
                <!--start-main-->
                <form id="loginFrom" method="post" enctype="multipart/form-data">
                    <div>
                        <h2>管理登录</h2>
                        <span>
                            <label>帐号</label></span>
                        <span>
                            <input type="text" id="username" name="username" class="textbox" data-msg="请输入帐号" data-validate="on" /></span>
                    </div>
                    <div>
                        <span>
                            <label>密码</label></span>
                        <span>
                            <input type="password" id="password" name="password" class="password" data-msg="请输入密码" data-validate="on" />

                        </span>
                    </div>
                    <div>
                        <span>
                            <label>验证码</label></span>
                        <span>
                            <input type="text" id="code" name="password" class="username" data-msg="请输入验证码" data-validate="on" style="width: 100px;" />
                            <img src="/ashx/ValidateCode.ashx?method=validateCode" class="yzm-img" id="yzmRefresh" />
                        </span>
                    </div>
                    <div class="sign">
                        <input type="button" value="登录" class="submit" id="send-btn" />
                    </div>
                </form>
            </div>
        </div>
        <!--//end-main-->
    </div>

</body>



</html>
