<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="navmenuconfig.aspx.cs" Inherits="inside.admin.web.aspx.navmenuconfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--page css--%>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../js/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/table.css" rel="stylesheet" type="text/css" />

    <%--page js--%>
    <script type="text/javascript" src="../js/lib/uploadfile/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../js/lib/treeTable/jquery.treeTable.js"></script>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>

    <script type="text/javascript">
        $(function () {
            var option = {
                theme: 'default',
                expandLevel: 2,
                beforeExpand: function ($treeTable, id) {
                    //判断id是否已经有了孩子节点，如果有了就不再加载，这样就可以起到缓存的作用
                    if ($('.' + id, $treeTable).length) { return; }
                    //这里的html可以是ajax请求
                    //var html = '<tr id="8" pId="6"><td>5.1</td><td>可以是ajax请求来的内容</td></tr>'
                    //         + '<tr id="9" pId="6"><td>5.2</td><td>动态的内容</td></tr>';

                    $treeTable.addChilds(html);
                },
                onSelect: function ($treeTable, id) {
                    window.console && console.log('onSelect:' + id);

                }
            };

            $('#treeTable1').treeTable(option);
            $("#layerSave").on("click", function () { btnSave(); })
        });

        function addChildren(obj) {

            var $parent = $(obj).parent().parent();
            var $layer = $("#layerOpt");

            $("#layerMenuName").val("");
            $("#layerLink").val("");
            $layer.attr("data-id", "0");
            $layer.attr("data-parent", $parent.attr("id"));
            layerOpen();
        }

        function layerOpen() {
            layer.open({
                type: 1,
                title: false,
                skin: 'layui-layer-rim', //加上边框
                closeBtn: 1,
                area: ['620px', '240px'],
                shadeClose: true,
                content: $('#layerOpt')
            });
        }

        function delChildren(nodeId, nodeName) {


            layer.confirm('确认要删除当前菜单？', {
                btn: ['继续', '取消']
            }, function () {

                $.ajax({

                    type: "post",
                    url: "../ashx/navmenuconfig.ashx?method=delNavMenu",
                    data: { nodeId: nodeId, nodeName: nodeName },
                    dataType: "json",
                    traditional: true,
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('删除成功!', { icon: 1, time: 1500 });
                            setTimeout(function () { window.location.reload(); }, 1500);
                        }
                        else {
                            layer.msg(data.message, { icon: 2, time: 2000 });
                        }
                    }
                });

            }, function () {

            });

        }

        function updateNodeName(obj) {

            var $parent = $(obj).parent().parent();
            var $layer = $("#layerOpt");
            $layer.attr("data-parent", "0");
            $layer.attr("data-id", $parent.attr("id"));

            var menuname = $parent.find("td").first().children().eq(2).text();
            var link = $parent.find("td:eq(1)").find("a").first().text();
            $("#layerMenuName").val(menuname);
            $("#layerLink").val(link);
            layerOpen();

        }

        function btnSave() {

            var $layer = $("#layerOpt");
            var id = $layer.attr("data-id");
            var parent = $layer.attr("data-parent");

            var menuname = $("#layerMenuName").val();
            var link = $("#layerLink").val();


            layer.confirm('当前操作[数据保存],是否继续?', function (index) {

                $.ajax({

                    type: "post",
                    url: "../ashx/navmenuconfig.ashx?method=navmenusave",
                    data: {
                        id: id,
                        parent: parent,
                        menuname: menuname,
                        link: link
                    },
                    dataType: "json",
                    traditional: true,

                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('保存成功!', { icon: 1, time: 1500 });
                            setTimeout(function () { window.location.reload(); }, 1500);
                        }
                        else {
                            layer.msg(data.message, { icon: 2, time: 2000 });
                        }
                    }
                });
            },
            function () {

            });

        }

    </script>
</head>
<body>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">网站配置管理</a></li>
            <li><a href="#">导航菜单</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>导航菜单</span></div>
        <table id="treeTable1">
            <tr>
                <th style="width: 200px;">导航菜单名称</th>
                <th style="width: 400px;">链接跳转地址</th>
                <th style="width: 180px;">添加子菜单</th>
                <th style="width: 120px;">删除</th>
                <th style="width: 120px;">修改</th>
            </tr>
            <%

                sortlist.ForEach(delegate(inside.admin.web.entityframework.mapping.t_navmenu_list_model m)
                {
                    Response.Write(string.Format(@"<tr id='{0}' pid='{1}'>
                <td><span controller='true'>{2}</span></td>
                <td><a target='_blank' href='{3}'>{3}</a></td><td>{4}</td><td>{5}</td><td>{6}</td>
            </tr>", m.nodeId, m.parentNodeId, m.nodeName, m.linkurl, m.nodeId.Length == 3 ? "</span><a class='ibtn' onclick='addChildren(this)'><span class='glyphicon glyphicon-plus'>添加子导航</a>" : "",
                                                                      m.nodeId.Length > 3 ? "<a class='ibtn' onclick='delChildren(\"" + m.nodeId + "\",\"" + m.nodeName + "\")'><span class='glyphicon glyphicon-trash'>删除</a>" : "",
                                                                                            "<a class='ibtn' onclick='updateNodeName(this)'><span class='glyphicon glyphicon-pencil'>修改</a>"));
                });
                  
            %>
        </table>


        <div id="layerOpt" style="display: none">
            <div class="formtitle"><span>子导航菜单</span></div>
            <ul class="forminfo">
                <li>
                    <label style="width: 80px;">菜单名称</label><input id="layerMenuName" type="text" class="dfinput" /></li>
                <li>
                    <label style="width: 80px;">链接地址</label><input id="layerLink" type="text" class="dfinput" /></li>

            </ul>
            <div style="margin: 0px auto; text-align: center; width: 120px;" id="layerSave">
                <a class="ibtn_max"><span class="glyphicon glyphicon-floppy-saves">保存</span></a>
            </div>
        </div>
    </div>
</body>
</html>
