<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userlog.aspx.cs" Inherits="inside.admin.web.aspx.userlog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />
    <link href="../js/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/helper.js"></script>

    <script type="text/javascript">
        var jQuery_general = jQuery.noConflict();
    </script>
    <script type="text/javascript" src="../js/select-ui.min.js"></script>
    <script type="text/javascript">
        jQuery_general(function () {
            jQuery_general(".select3").uedSelect({ width: 130 });
        });
    </script>

    <%--paginator--%>
    <link rel="stylesheet" href="../js/lib/paginator/bootstrapv3.css" />
    <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../js/lib/paginator/bootstrap-paginator.js" type="text/javascript"></script>

    <%--lib------Tabs--%>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <%-- end-----%>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
</head>
<body class="pageBody">
    <form>
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="/index.html">系统管理</a></li>
                <li>
                    <img src="/images/main/arrow.gif" /></li>
                <li><a href="articlelist.aspx">帐号日志</a></li>
            </ul>
        </div>

        <div class="rightinfo">
            <div class="tools">
                <ul class="seachform">
                    <li>
                        <label>登录名</label><input id="loginName" type="text" class="scinput" /></li>
                    <li style="display: none">
                        <label>登录时间</label><input id="loginTime" type="text" class="scinput" /></li>
                    <li style="display: none">
                        <label>登录IP</label><input id="loginIp" type="text" class="scinput" style="display: none" /></li>
                    <li>
                        <label>&nbsp;</label><input type="button" class="scbtn" value="查询" id="btnquery" /></li>
                </ul>
            </div>

            <div id="usual1" class="usual">
                <div class="itab">
                    <ul>
                        <li><a id="#tab1" href="#tab1" class="selected">帐号登录日志</a></li>
                        <li><a id="#tab2" href="#tab2">帐号异常日志</a></li>
                        <li><a id="#tab3" href="#tab3">帐号操作日志</a></li>
                    </ul>
                </div>
                <div id="tab1" class="tabson" style="width: 920px">
                    <table class="tablelist" id="article-table-list">
                        <thead>
                            <tr>
                                <th style="width: 150px;">登录名</th>
                                <th style="width: 180px;">登录时间</th>
                                <th style="width: 180px;">登录IP</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <ul id='Pageinator'></ul>
                </div>
                <div id="tab2" class="tabson" style="width: 920px">
                    <table class="tablelist" id="errlog-table-list">
                        <thead>
                            <tr>
                                <th style="width: 100px;">登录名</th>
                                <th style="width: 160px;">登录IP</th>
                                <th style="width: 160px;">第一次登录时间</th>
                                <th style="width: 160px;">最后一次登录时间</th>
                                <th style="width: 80px;">错误次数</th>
                                <th style="width: 150px;">操作</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <div id="tab3" class="tabson" style="width: 920px">
                    <table class="tablelist" id="optlog-table-list">
                        <thead>
                            <tr>
                                <th style="display: none;">id</th>
                                <th style="width: 80px;">帐号</th>
                                <th style="width: 160px;">操作时间</th>
                                <th>操作</th>

                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <ul id='PageinatorOpt'></ul>
                </div>
            </div>
        </div>

    </form>
    <script type="text/javascript">
        var Pageinator = $('#Pageinator');
        var PageinatorOpt = $('#PageinatorOpt');

        /*刷新重新加载*/
        function Refresh(pageindex) {

            GridBind(pageindex);
        }

        function GridBind(PageIndex) {

            var loginName = $("#loginName").val();
            var loginTime = $("#loginTime").val();
            var loginIp = $("#loginIp").val();

            var JsonString = "{'loginName':'" + loginName + "'," + "'loginTime':'" + loginTime + "'," + "'loginIp':'" + loginIp + "'}";
            var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 15, "PageIndex": PageIndex });

            $.ajax({

                type: "post",
                url: "../ashx/user.ashx?method=userlogbind",
                contentType: "application/json; charset=utf-8",
                data: dataSend,
                dataType: "json",
                traditional: true,

                success: function (data) {

                    var row_index = 1;
                    var html = "";

                    $(data.list).each(function (i) {

                        var tr_html = "<tr {0}><td style='display:none'></td><td>{1}</td><td>{2}</td><td>{3}</td><td></td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(tr_classs, data.list[i].loginName, data.list[i].loginTime, data.list[i].loginIp);
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

        function unlock(obj) {

            layer.confirm('当前操作[帐号解锁],是否继续?', function (index) {

                $parent = $(obj).parent().parent();
                var id = $parent.children().first().text();
                var username = $parent.children().eq(1).text();

                $.ajax({

                    type: "post",
                    url: "../ashx/user.ashx?method=unlock",
                    dataType: "json",
                    data: {
                        id: id,
                        username: username
                    },
                    dataType: "json",
                    traditional: true,
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('解锁成功!', { icon: 1, time: 1500 });
                            $(obj).parent().parent().css("display", "none");
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

        function errorGridBind() {

            var loginName = $("#loginName").val();
            var loginTime = $("#loginTime").val();
            var loginIp = $("#loginIp").val();

            var JsonString = "{'loginName':'" + loginName + "'," + "'loginTime':'" + loginTime + "'," + "'loginIp':'" + loginIp + "'}";
            var dataSend = JSON.stringify({ "data": JsonString });


            $.ajax({
                type: "post",
                url: "../ashx/user.ashx?method=userErrorlogbind",
                contentType: "application/json; charset=utf-8",
                data: dataSend,
                dataType: "json",
                traditional: true,

                success: function (data) {

                    var row_index = 1;
                    var html = "";

                    $(data).each(function (i) {

                        var tr_html = "<tr {0}><td style='display:none'>{6}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td><a class='ibtn' onclick='unlock(this)' style='width:70px;padding:0px;margin-left:5px;'><span class='glyphicon glyphicon-lock'>解锁</a></td><td></td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(tr_classs, data[i].login, data[i].ipAddress, data[i].firstTime, data[i].lastTime, data[i].errorCount, data[i].id);
                        row_index++;
                        html = html + tr_html;
                    });

                    $("#errlog-table-list tbody").html(html);
                },
                complete: function () {

                }
            });
        }

        function RefreshOpt(pageindex) {

            OptLogGridBind(pageindex);
        }

        function OptLogGridBind(PageIndex) {

            var loginName = $("#loginName").val();
            var loginTime = $("#loginTime").val();
            var loginIp = $("#loginIp").val();

            var JsonString = "{'loginName':'" + loginName + "'," + "'loginTime':'" + loginTime + "'," + "'loginIp':'" + loginIp + "'}";
            var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 15, "PageIndex": PageIndex });

            $.ajax({

                type: "post",
                url: "../ashx/user.ashx?method=optlogbind",
                contentType: "application/json; charset=utf-8",
                data: dataSend,
                dataType: "json",
                traditional: true,

                success: function (data) {

                    var row_index = 1;
                    var html = "";


                    $(data.list).each(function (i) {

                        var tr_html = "<tr {0}><td style='display:none'>{1}</td><td>{2}</td><td>{4}</td><td>{3}</td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(tr_classs, data.list[i].id, data.list[i].uname, data.list[i].opt, data.list[i].operatorTime);
                        row_index++;
                        html = html + tr_html;
                    });


                    $("#optlog-table-list tbody").html(html);

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

                           RefreshOpt(page);
                       }
                        };

                        //alert(options.pageCount);
                        //alert(options.currentPage);


                        PageinatorOpt.bootstrapPaginator(options);
                    }
                }

            });
        }

        $(document).ready(function () {

            $("#usual1 ul").idTabs(function (id) {

                $("#loginName").val("");

                switch (id) {
                    case "#tab1":
                        GridBind(1);
                        break;
                    case "#tab2":
                        errorGridBind();
                        break;
                    case "#tab3":
                        OptLogGridBind(1);
                        break;
                } return true;
            });

            $("#btnquery").bind("click", query);

        });

        function query() {

            var selectedTab = "";
            var $tabitem = $("#usual1 ul").find("li");
            $tabitem.each(function () {
                var a = $(this).find("a");
                if (a.attr("class") == "selected") {
                    selectedTab = a.attr("id");
                }

            });
            if (selectedTab == "#tab1") {
                GridBind(1);
            }
            else if (selectedTab == "#tab2") {
                errorGridBind();
            }
            else if (selectedTab == "#tab3") {
                OptLogGridBind(1);
            }
        }


    </script>
</body>
</html>
