<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userright.aspx.cs" Inherits="inside.admin.web.aspx.userright" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />
    <link href="../js/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/helper.js"></script>
    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <%--lib-----zTree-----%>

    <link rel="stylesheet" href="../js/lib/zTree/zTreeStyle/zTreeStyle.css" type="text/css" />
    <script type="text/javascript" src="../js/lib/zTree/jquery.ztree.core.js"></script>
    <script type="text/javascript" src="../js/lib/zTree/jquery.ztree.excheck.js"></script>

    <script type="text/javascript">
		<!--
    var setting = {
        check: {
            enable: true
        },
        data: {
            simpleData: {
                enable: true
            }
        }
    };

    var code;

    function setCheck() {
        var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
        py = $("#py").attr("checked") ? "p" : "",
        sy = $("#sy").attr("checked") ? "s" : "",
        pn = $("#pn").attr("checked") ? "p" : "",
        sn = $("#sn").attr("checked") ? "s" : "",
        type = { "Y": py + sy, "N": pn + sn };
        zTree.setting.check.chkboxType = type;
        showCode('setting.check.chkboxType = { "Y" : "' + type.Y + '", "N" : "' + type.N + '" };');
    }
    function showCode(str) {
        if (!code) code = $("#code");
        code.empty();
        code.append("<li>" + str + "</li>");
    }

    </script>
</head>
<body>
    <form>
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="/index.html">系统管理</a></li>
                <li>
                    <img src="/images/main/arrow.gif" /></li>
                <li><a href="articlelist.aspx">权限配置</a></li>
            </ul>
        </div>

        <div class="rightinfo">
            <table class="tablelist" id="userGroups-table-list" style="width: 420px;">
                <thead>
                    <tr>
                        <th style="display: none">组编号</th>
                        <th style="width: 150px;">用户组</th>
                        <th style="width: 150px;">操作</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
        <div id="layerOpt" style="display: none">
            <div class="formtitle"><span>权限分配</span></div>
            <div class="zTreeDemoBackground left">
                <ul id="treeDemo" class="ztree"></ul>
            </div>
            <div style="margin-left: 130px;">

                <a class="ibtn_max" style="margin: 0 5px;" onclick="save();"><span class="glyphicon glyphicon-floppy-saves">保存</span></a>
            </div>

        </div>
    </form>
    <script type="text/javascript">

        function GridBind() {

            $.ajax({

                type: "post",
                url: "../ashx/user.ashx?method=userGroups",
                dataType: "json",
                traditional: true,

                success: function (data) {

                    var row_index = 1;
                    var html = "";

                    $(data).each(function (i) {

                        var tr_html = "<tr {0}><td style='display:none'>{1}</td><td>{2}</td><td onclick='disRight({1},\"{2}\")'><a href='#'>{3}</a></td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(tr_classs, data[i].userGroupID, data[i].name, "分配");
                        row_index++;
                        html = html + tr_html;
                    });

                    $("#userGroups-table-list tbody").html(html);

                }

            });
        }

        $(document).ready(function () {

            GridBind(1);
        });

        function layerOpen() {

            getGroupsRightsByID();

            layer.open({
                type: 1,
                title: false,
                skin: 'layui-layer-rim', //加上边框
                closeBtn: 1,
                area: ['420px', '660px'],
                shadeClose: true,
                content: $('#layerOpt'),
                zIndex: 100 //层优先级
            });
        }

        function disRight(userGroupID, GroupName) {

            $("#layerOpt").attr("data-id", userGroupID);
            $("#layerOpt").attr("data-name", GroupName);
            layerOpen();
        }


        function getGroupsRightsByID() {

            //$("input[name='ckRights']").prop("checked", false);

            $.ajax({

                type: "post",
                url: "../ashx/user.ashx?method=getGroupsRights",
                data: {
                    userGroupID: $("#layerOpt").attr("data-id")
                },
                dataType: "json",
                traditional: true,

                success: function (data) {

                    console.log(data);
                    $.fn.zTree.init($("#treeDemo"), setting, data);

                    setCheck();
                    $("#py").bind("change", setCheck);
                    $("#sy").bind("change", setCheck);
                    $("#pn").bind("change", setCheck);
                    $("#sn").bind("change", setCheck);
                }

            });

        }

        function save() {

            var chk_value = [];
            var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
            var nodes = treeObj.getCheckedNodes(true);

            $(nodes).each(function (i) {

                chk_value.push(nodes[i].id);
            });

            var repuserGroupID = $("#layerOpt").attr("data-id");
            var repuserGroupName = $("#layerOpt").attr("data-name");

            layer.confirm('确认权限操作,是否继续?', function (index) {

                layer.load();

                $.ajax({
                    url: '../ashx/user.ashx?method=setUserRights',
                    type: 'post',
                    dataType: "json",
                    data: {
                        userGroupID: repuserGroupID,
                        userGroupName: repuserGroupName,
                        rightsID: chk_value.join(",")
                    },
                    success: function (data) {

                        if (data.status == "ok") {

                            layer.msg('权限已分配!', { icon: 1, time: 1500 });
                            setTimeout(function () { window.location.reload(); }, 1600);
                        }
                        else {
                            layer.msg(data.message, { icon: 2, time: 2000 });
                        }
                    },
                    complete: function () {
                        layer.closeAll('loading');
                        $("#layerOpt").removeAttr("id");
                    }

                });
            },
 function () {

 });

        }

    </script>
</body>
</html>
