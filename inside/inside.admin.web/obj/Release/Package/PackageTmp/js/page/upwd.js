$(function () {

    $(".scbtn").bind("click", function () {

        var oldPwd = $("#oldPwd").val();
        var newPwd = $("#newPwd").val();
        var again = $("#again").val();

        if (oldPwd == '') {
            layer.msg('请输入旧密码!', { icon: 2, time: 1000 });
            $("#oldPwd").focus();
            return false;
        }
        if (newPwd == '') {
            layer.msg('请输入新密码!', { icon: 2, time: 1000 });
            $("#newPwd").focus();
            return false;
        }


        var re = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^]{8,16}$/;
        var result = re.test(newPwd);
        if (!result) {

            layer.msg("密码至少包含大写字母，小写字母，数字，且不少于8位", { icon: 2, time: 1500 });
            return false;
        }

        if (again == '') {
            layer.msg('请确认新密码!', { icon: 2, time: 1000 });
            $("#again").focus();
            return false;
        }

        if (newPwd != again) {
            layer.msg('两次输入的新密码不一致!', { icon: 2, time: 1000 });
            $("#again").focus();
            return false;
        }

        var data = {};
        data.oldPwd = oldPwd;
        data.newPwd = newPwd;
        data.again = again;
        var result = $.encryptRequest({ data: data });
        $.ajax({
            url: '../../ashx/user.ashx?method=modify',
            dataType: 'json',
            type: 'post',
            data: result,
            success: function (res) {

                if (res.status == "ok") {

                    window.location.href = "../../html/succeed.html";
                }
                else {

                    layer.msg(res.message, { icon: 2, time: 2000 });

                }
            },
            complete: function () {


            },
            error: function () {
                alert("异常！");
            }
        })
    });
});