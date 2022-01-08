<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="articlelist.aspx.cs" Inherits="inside.admin.web.aspx.articlelist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript">
        var jQuery_general = jQuery.noConflict();
    </script>
    <script type="text/javascript" src="../js/select-ui.min.js"></script>
    <script type="text/javascript" src="../js/helper.js"></script>
    <script type="text/javascript">
        jQuery_general(function () {
            jQuery_general(".select3").uedSelect({ width: 130 });
        });
    </script>


    <%--paginator--%>
    <link rel="stylesheet" href="../js/lib/paginator/bootstrapv3.css?10" />
    <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../js/lib/paginator/bootstrap-paginator.js" type="text/javascript"></script>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>

</head>
<body class="pageBody">
    <form>
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="/index.html">内容发布管理</a></li>
                <li>
                    <img src="/images/main/arrow.gif" /></li>
                <li><a href="articlelist.aspx">文章搜索</a></li>
            </ul>
        </div>

        <div class="rightinfo">
            <div class="tools">
                <ul>
                    <li>
                        <div class="btnPublic">
                            <a href="releasearticle.aspx" target="_blank"><span>
                                <img src="/images/t01.png" /></span> 发布新内容</a>
                        </div>

                    </li>
                </ul>
                <ul class="seachform">

                    <li>
                        <label>标题包含</label><input id="qtitle" type="text" class="scinput" /></li>
                    <li>
                        <label>关键字</label><input id="qkeyword" type="text" class="scinput" /></li>
                    <li>
                        <label>首页栏目</label>
                        <div class="vocation">
                            <div class="uew-select" style="top: 8px;">
                                <select class="select3" style="left: 0px">
                                    <% 
                                        StringBuilder selectHtml = new StringBuilder("<option value='-1'>全部</option>");
                                        foreach (inside.admin.web.entityframework.tableEntity.t_subject_list_model m in subjectlist)
                                        {
                                            selectHtml.Append(string.Format("<option value='{0}'>{1}</option>", m.subjectID, m.subjectName));
                                        }

                                        Response.Write(selectHtml.ToString());
                                    %>
                                </select>
                            </div>
                        </div>
                    </li>
                    <li>
                        <label>作者</label><input id="qauthor" type="text" class="scinput" style="width: 80px" /></li>
                    <li>
                        <label>编辑</label><input id="qeditor" type="text" class="scinput" style="width: 80px" /></li>

                    <li>
                        <label>&nbsp;</label><input type="button" class="scbtn" value="查询" id="btnquery" /></li>


                    <li>
                        <label>&nbsp;</label><input type="button" class="scbtn" value="列表刷新" id="btnrefresh" /></li>
                </ul>
                <%-- <ul class="toolbar1">
                    <li><span>
                        <img src="/images/t05.png" /></span>回收站</li>
                </ul>--%>
            </div>

            <table class="tablelist" id="article-table-list">

                <thead>
                    <tr>
                        <th style="width: 50px;">发布</th>
                        <th style="width: 70px;">编号<i class="sort"><img src="/images/px.gif" /></i></th>
                        <th style="width: 385px;">标题</th>
                        <th>关键词</th>
                        <th>作者</th>
                        <th>发布部门</th>
                        <th style="width: 170px;">发布时间</th>
                        <th style="width: 70px;">编辑</th>
                        <th>编辑部门</th>
                        <th style="width: 170px;">更新时间</th>
                        <th style="width: 50px;">图片</th>
                        <th style="width: 50px;">附件</th>
                        <th style="width: 85px;">点击</th>
                        <th style="width: 160px;">操作</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <ul id='Pageinator'></ul>
        </div>
    </form>
    <script type="text/javascript">
        var Pageinator = $('#Pageinator');

        /*刷新重新加载*/
        function Refresh(pageindex) {

            GridBind(pageindex);
        }

        function del(obj) {

            layer.confirm('当前操作[删除文章],是否继续?', function (index) {

                var $parent = $(obj).parent().parent();
                var articleID = $parent.find("td:eq(1)").text();
                var articleTitle = $parent.find("td:eq(2)").text();

                $.ajax({

                    type: "post",
                    url: "../ashx/article.ashx?method=delByArticle",
                    dataType: "json",
                    data: {
                        articleID: articleID,
                        articleTitle: articleTitle
                    },
                    dataType: "json",
                    traditional: true,
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('已删除成功!', { icon: 1, time: 1500 });
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

        function GridBind(PageIndex) {

            layer.load();

            var reqUrl_isPassed = getQueryVariable("passed");
            var isPassed = reqUrl_isPassed.toString() == "0" ? reqUrl_isPassed : "";

            var title = $("#qtitle").val();
            var keyword = $("#qkeyword").val();
            var author = $("#qauthor").val();
            var editor = $("#qeditor").val();
            var subjectID = $('.select3 option:selected').val() == -1 ? "" : $('.select3 option:selected').val();

            var JsonString = "{'isPassed':'" + isPassed + "','title':'" + title + "'," + "'keyword':'" + keyword + "'," + "'author':'" + author + "'," + "'editor':'" + editor + "'," + "'subjectID':'" + subjectID + "'," + "'articleID':''}";
            var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 20, "PageIndex": PageIndex });

            $.ajax({

                type: "post",
                url: "../ashx/article.ashx?method=articlelistbind",
                contentType: "application/json; charset=utf-8",
                data: dataSend,
                dataType: "json",
                traditional: true,

                success: function (data) {

                    var row_index = 1;
                    var html = "";

                    var optHtml = "", firstCtl = "";

                    $(data.list).each(function (i) {

                        //搜索匹配的文字高亮显示
                        if (title != "") { data.list[i].title = data.list[i].title.replace(title, "<span class='txthighlight'>" + title + "</span>"); }
                        data.list[i].isIncludePic = data.list[i].isIncludePic == true ? "是" : "否";
                        data.list[i].isIncludeAcc = data.list[i].isIncludeAcc == true ? "是" : "否";

                        //未审核与已审核分别显示不同的图标
                        firstCtl = data.list[i].isPassed == 1 ? "<img src='../images/ysh.png' />" : "<img src='../images/dsh.png' />";

                        //已审核的文章显示对应按钮
                        var audit_ok = "<a href='releasearticle.aspx?opt=update&&articleID={0}' class='tablelink' target='_blank'> 修改</a><a href='#' class='tablelink' onclick='del(this)'> 删除</a>";
                        //仅从审核页面跳转过来待审核行才显示审核铵钮
                        var audit_only = "<a href='preview.aspx?opt=audit&&articleID={0}' target='_blank' id='btnEdit' target='_blank'>审核</a> <a href='releasearticle.aspx?opt=update&&articleID={0}' class='tablelink' target='_blank'> 修改</a> <a href='#' class='tablelink' onclick='del(this)'> 删除</a>";
                        //非审核页面跳转过来的待审核行显示的按钮
                        var audit_no = "<a href='releasearticle.aspx?opt=update&&articleID={0}' class='tablelink' target='_blank'> 修改</a> <a href='#' class='tablelink' onclick='del(this)'> 删除</a>";
                        //确定要显示的按钮 isPassed=="0"指仅从审核页面跳转过来的
                        optHtml = data.list[i].isPassed == 1 ? audit_ok : (isPassed == "0") ? audit_only : audit_no;
                        //格式化按钮html
                        var optHtmlStringformat = optHtml.format(data.list[i].articleID);
                        //每行要显示的数据
                        var tr_html = "<tr {0}><td>{13}</td><td>{1}</td><td><a href='preview.aspx?opt=preview&&articleID={1}'  target='_blank'>{2}</a></td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{14}</td><td>{12}</td></tr>";
                        //隔行样式变换
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        //格式化行数据
                        tr_html = tr_html.format(tr_classs, data.list[i].articleID, data.list[i].title, data.list[i].keywords, data.list[i].author, data.list[i].releaseDep, data.list[i].releaseTime, data.list[i].editor, data.list[i].editorDep, data.list[i].updateTime, data.list[i].isIncludePic, data.list[i].isIncludeAcc, optHtmlStringformat, firstCtl, data.list[i].hits);
                        row_index++;
                        html = html + tr_html;
                    });


                    $("#article-table-list tbody").html(html);


                    if (data.TotalPage > 0) {
                        var pageCount = data.TotalPage; //取到pageCount的值
                        var currentPage = data.PageIndex; //得到currentPage

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
                },
                complete: function () {
                    layer.closeAll('loading');
                }
            });
        }

        $(document).ready(function () {

            GridBind(1);
            $("#btnquery").bind("click", query);

            $("#btnrefresh").bind("click", refresh);
        });

        function refresh() {
            var $pagerli = $("#Pageinator").find("li");
            $pagerli.each(function () {
                if ($(this).attr("class") == "active") {

                    var pIndex = $(this).find("a").text();
                    GridBind(pIndex);
                }
            });
        }

        function query() {
            GridBind(1);
        }


    </script>
</body>
</html>
