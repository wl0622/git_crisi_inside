<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userlist.aspx.cs" Inherits="inside.admin.web.aspx.userlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />

    <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/select-ui.min.js"></script>

    <%--paginator--%>
    <link rel="stylesheet" href="../js/lib/paginator/bootstrapv3.css" />
    <script src="../js/lib/paginator/bootstrap-paginator.js" type="text/javascript"></script>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>

    <%--lib------ComboTree--%>
    <link rel="stylesheet" type="text/css" href="../js/easyui/easyui.css" />
    <script type="text/javascript" src="../js/easyui/jquery.easyui.min.js"></script>
    <%-- end-----%>
</head>
<body class="pageBody">
    <form>
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="/index.html">系统管理</a></li>
                <li><img src="/images/main/arrow.gif" /></li>
                <li><a href="articlelist.aspx">帐号列表</a></li>
            </ul>
        </div>
        <div class="rightinfo">
            <div class="tools">
                <ul class="toolbar">
                    <li class="click" onclick="create();">
                        <div class="btnPublic">
                            <a href="#" target="rightFrame">
                                <span>
                                    <img src="/images/t01.png" />
                                </span>新增用户
                            </a>
                        </div>
                    </li>
                </ul>
                <ul class="seachform">
                    <li>
                        <label>登录名</label><input id="userName" type="text" class="scinput" /></li>
                    <li>
                        <label>姓名</label><input id="userCnName" type="text" class="scinput" /></li>
                    <li style="display: none">
                        <label>所属部门</label><input id="deptID" type="text" class="scinput" style="width: 80px" /></li>
                    <li style="display: none">
                        <label>用户组</label><input id="userGroupID" type="text" class="scinput" style="width: 80px" /></li>

                    <li>
                        <label>&nbsp;</label><input type="button" class="scbtn" value="查询" id="btnquery" /></li>
                </ul>
            </div>

            <table class="tablelist" id="article-table-list">
                <thead>
                    <tr>
                        <th style="display: none">
                            <input name="" type="checkbox" value="" /></th>
                        <th style="display: none">用户编号<i class="sort"><img src="/images/px.gif" /></i></th>
                        <th style="display: none">用户组编号</th>
                        <th style="display: none">部门编号</th>
                        <th style="width: 100px;">用户组</th>
                        <th style="width: 150px;">部门</th>
                        <th style="width: 100px;">登录名</th>
                        <th style="width: 100px;">姓名</th>
                        <th style="width: 200px;">Email</th>
                        <th style="width: 150px;">添加日期</th>
                        <th style="width: 150px;">最后登录日期</th>
                        <th style="width: 120px;">最后登录IP</th>
                        <th style="width: 100px;">操作</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <ul id='Pageinator'></ul>
        </div>

        <div id="layerOpt" style="display: none">
            <div class="formtitle"><span>帐号编辑</span></div>
            <ul class="forminfo">
                <li>
                    <label style="width: 60px;">用户组</label>
                    <div style="margin-left: 60px;">
                        <select class="select1" name="ckUserGroups" id="ckUserGroups">
                            <% 
                                StringBuilder selectHtml = new StringBuilder("<option value='-1'>--请选择用户组--</option>");
                                foreach (inside.admin.web.entityframework.tableEntity.UserGroups_model m in userGroupsList)
                                {
                                    selectHtml.Append(string.Format("<option value='{0}'>{1}</option>", m.userGroupID, m.name));
                                }
                                Response.Write(selectHtml.ToString());
                            %>
                        </select>
                    </div>
                </li>
                <li>
                    <label style="width: 60px;">部门</label><input id="lay_DeptID" type="text" class="dfinput" /></li>
                <li>
                    <label style="width: 60px;">登录名</label><input id="lay_userName" type="text" class="dfinput" /></li>
                <li>
                    <label style="width: 60px;">姓名</label><input id="lay_userCnName" type="text" class="dfinput" /></li>
                <li>
                    <label style="width: 60px;">邮箱</label><input id="lay_userEmail" type="text" class="dfinput" /></li>
            </ul>

            <div style="margin: 0px auto; text-align: center; width: 300px;">

                <a class="ibtn_max" style="margin: 0 5px;" onclick="save();"><span class="glyphicon glyphicon-floppy-saves">保存</span></a>
                <a class="ibtn_max" onclick="reset();" id="btnReset"><span class="glyphicon glyphicon-floppy-saves">密码重置</span></a>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        var Pageinator = $('#Pageinator');

        /*刷新重新加载*/
        function Refresh(pageindex) {

            GridBind(pageindex);
        }

        String.prototype.format = function () {
            var values = arguments;
            return this.replace(/\{(\d+)\}/g, function (match, index) {
                if (values.length > index) {
                    return values[index];
                } else {
                    return "";
                }
            });
        };

        function GridBind(PageIndex) {

            var userName = $("#userName").val();
            var userCnName = $("#userCnName").val();
            var deptID = $("#deptID").val();
            var userGroupID = $("#userGroupID").val();


            var JsonString = "{'userName':'" + userName + "'," + "'userCnName':'" + userCnName + "'," + "'deptID':'" + deptID + "'," + "'userGroupID':'" + userGroupID + "'}";
            var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 15, "PageIndex": PageIndex });

            $.ajax({

                type: "post",
                url: "../ashx/user.ashx?method=userlistbind",
                contentType: "application/json; charset=utf-8",
                data: dataSend,
                dataType: "json",
                traditional: true,

                success: function (data) {

                    var row_index = 1;
                    var html = "";

                    $(data.list).each(function (i) {

                        //if (title != "") { data.list[i].title = data.list[i].title.replace(title, "<span class='txthighlight'>" + title + "</span>"); }
                        //data.list[i].isIncludePic = data.list[i].isIncludePic == true ? "是" : "否";
                        //data.list[i].isIncludeAcc = data.list[i].isIncludeAcc == true ? "是" : "否";

                        var tr_html = "<tr {0}><td style='display:none'><input  type='checkbox' /></td><td style='display: none'>{1}</td><td style='display: none'>{2}</td><td style='display: none'>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td><a href='#' id='btnEdit' class='tablelink' onclick='edit({1});'>编辑</a>     <a href='# class='tablelink' id='btnDel' onclick='del({1})'> 删除</a></td><td></td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(tr_classs, data.list[i].userID, data.list[i].userGroupID, data.list[i].deptID, data.list[i].userGroupName, data.list[i].deptName, data.list[i].userName, data.list[i].userCnName, data.list[i].userEmail, data.list[i].addDate, data.list[i].lastlogin, data.list[i].userLastIP);
                        row_index++;
                        html = html + tr_html;
                    });


                    $("#article-table-list tbody").html(html);

                    var pageCount = data.TotalPage; //取到pageCount的值
                    var currentPage = data.PageIndex; //得到currentPage

                    if (pageCount > 1) {

                        var options = {

                            bootstrapMajorVersion: 3, //版本
                            currentPage: currentPage, //当前页数
                            totalPages: pageCount, //总页数
                            numberOfPages: 10,

                            itemTexts: function (type, page, current) {

                                switch (type) {

                                    case "first":
                                        return "首页";
                                    case "prev":
                                        return "上一页";
                                    case "next":
                                        return "下一页";
                                    case "last":
                                        return "末页";
                                    case "page":
                                        return page;
                                }
                            }
                            //点击事件，用于通过Ajax来刷新整个list列表
                            , onPageClicked: function (event, originalEvent, type, page) {

                                Refresh(page);
                            }
                        };

                        Pageinator.bootstrapPaginator(options);
                    }
                }

            });
        }

        function layerOpen() {

            layer.open({
                type: 1,
                title: false,
                skin: 'layui-layer-rim', //加上边框
                closeBtn: 1,
                area: ['520px', '420px'],
                shadeClose: true,
                content: $('#layerOpt'),
                zIndex: 100 //层优先级
            });
        }

        function edit(uid) {

            $("#btnReset").css("display", "block");
            $.ajax({

                type: "post",
                url: "../ashx/user.ashx?method=usersInfoByuserID",
                dataType: "json",
                data: {
                    userID: uid
                },
                dataType: "json",
                traditional: true,
                success: function (data) {

                    $(".uew-select-text").text($(".select1 option[value=" + data.userGroupID + "]").text());
                    $("#ckUserGroups").val(data.userGroupID);


                    if (data.deptID != null) {

                        $('#lay_DeptID').combotree('setValue', data.deptID);
                        //赋值后显示文本有时不能回显所以做此处理
                        $('#lay_DeptID').combotree('setText', $("#lay_DeptID").combotree('getText'));
                        //$('#lay_DeptID').combotree('setValue', { id: data.deptID, text: "信息中心" });
                    }
                    $("#lay_userName").val(data.userName);
                    $("#lay_userCnName").val(data.userCnName);
                    $("#lay_userEmail").val(data.userEmail);
                    $("#layerOpt").attr("data-id", data.userID);

                    layerOpen();
                }
            });
        }

        function create() {
            $("#btnReset").css("display", "none");
            $("#layerOpt").attr("data-id", 0);
            $("#layerOpt").find("input[type=text]").each(function () { $(this).val(""); });
            $(".uew-select-text").text($(".select1 option[value=-1]").text());
            $("#ckUserGroups").val("-1");
            layerOpen();
        }


        function save() {

            layer.load();
            var userID = $("#layerOpt").attr("data-id");
            var deptID = $('#lay_DeptID').combotree('getValue');
            var userGroupID = $("#ckUserGroups").val();
            var userName = $("#lay_userName").val();
            var userCnName = $("#lay_userCnName").val();
            var userEmail = $("#lay_userEmail").val();

            $.ajax({
                url: '../ashx/user.ashx?method=saveUserInfo',
                type: 'post',
                dataType: "json",
                data: {
                    userID: userID,
                    deptID: deptID,
                    userGroupID: userGroupID,
                    userName: userName,
                    userCnName: userCnName,
                    userEmail: userEmail
                },
                success: function (data) {

                    if (data.status == "ok") {
                        layer.msg('帐号保存成功!', { icon: 1, time: 1500 });
                        setTimeout(function () { window.location.reload(); }, 2000);
                    }
                    else {
                        layer.msg(data.message, { icon: 2, time: 2000 });
                    }
                },
                complete: function () {
                    layer.closeAll('loading');
                }

            });

        }

        function reset() {

            layer.confirm('当前操作[密码重置],是否继续?', function (index) {

                var userID = $("#layerOpt").attr("data-id");
                var userName = $("#lay_userName").val();

                $.ajax({
                    url: '../ashx/user.ashx?method=resetPassword',
                    type: 'post',
                    dataType: "json",
                    data: {
                        userID: userID,
                        userName: userName
                    },
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('密码已重置!', { icon: 1, time: 1500 });
                            setTimeout(function () { layer.closeAll(); }, 1500);

                        }
                        else {
                            layer.msg(data.message, { icon: 2, time: 2000 });
                        }
                    },
                    complete: function () {
                        layer.closeAll('loading');
                        $("#layerOpt").attr("data-id", "0")
                    }

                });
            },
            function () {

            });

        }

        function del(userID) {

            var userName = $("#lay_userName").val();

            layer.confirm('当前操作[删除帐号],是否继续?', function (index) {


                $.ajax({
                    url: '../ashx/user.ashx?method=delUserByID',
                    type: 'post',
                    dataType: "json",
                    data: {
                        userID: userID,
                        userName: userName
                    },
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('帐号已删除!', { icon: 1, time: 1500 });
                            setTimeout(function () { window.location.reload(); }, 1500);

                        }
                        else {
                            layer.msg(data.message, { icon: 2, time: 2000 });
                        }
                    },
                    complete: function () {

                        $("#layerOpt").attr("data-id", "0")
                    }

                });
            });

        }

        $(document).ready(function () {

            GridBind(1);
            $("#btnquery").bind("click", query);
            //栏目下拉框设置
            $(".select1").uedSelect({ width: 345 });
        });

        function query() {
            GridBind(1);
        }

        //部门
        $('#lay_DeptID').combotree({
            url: '../ashx/department.ashx?method=getDepartmentComboTree&valueColumn=deptID',
            method: 'get',
            required: false,
            onBeforeSelect: function (node) {
                //禁止父目录选中
                if (typeof (node.children) != "undefined" && node.children != null) {
                    return false;
                }
            },
            onSelect: function (node) {
                var selectedId = node.id;
            },
            onLoadSuccess: function (node, data) {
            }
        });

    </script>
</body>
</html>
