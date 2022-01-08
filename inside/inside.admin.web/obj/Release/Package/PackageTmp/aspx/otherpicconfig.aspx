<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="otherpicconfig.aspx.cs" Inherits="inside.admin.web.aspx.otherpicconfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <%--page css--%>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />

    <%--page js--%>
    <script type="text/javascript" src="../js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../js/select-ui.min.js"></script>
    <script type="text/javascript" src="../js/helper.js"></script>
    <%-- end-----%>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <%-- end-----%>


    <%--lib------Tabs--%>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <%-- end-----%>


    <%--paginator--%>
    <link rel="stylesheet" href="../js/lib/paginator/bootstrapv3.css" />
    <%-- <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>--%>
    <script src="../js/lib/paginator/bootstrap-paginator.js" type="text/javascript"></script>
    <%-- end-----%>
    <script type="text/javascript">
        $(function (e) {

            $("#usual1 ul").idTabs(function (id) {
                switch (id) {
                    case "#tab1":
                        isPicxw = true;
                        articleBind(1);
                        break;
                    case "#tab2":
                        isPicxw = false;
                        articleBind(1);
                        break;
                    default:
                        break;

                } return true;
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="#">网站配置管理</a></li>
                <li>
                    <img src="/images/main/arrow.gif" /></li>
                <li><a href="#">其它图片设置</a></li>
            </ul>
        </div>
        <div class="formbody">

            <div id="usual1" class="usual">
                <div class="itab">
                    <ul>
                        <li><a href="#tab1" class="selected">新闻图片幻灯片</a></li>
                        <li><a href="#tab2">产品与技术图片</a></li>
                    </ul>
                </div>
                <div id="tab1" class="tabson"></div>
                <div id="tab2" class="tabson"></div>

            </div>
            <table class="tablelist" id="article-table-list">
                <thead>
                    <tr>
                        <th style="width: 160px;">操作</th>
                        <th style="width: 50px;">发布</th>
                        <th style="display: none">编号<i class="sort"><img src="/images/px.gif" /></i></th>
                        <th>标题</th>
                        <th style="width: 200px;">关键词</th>
                        <th style="width: 110px;">作者</th>
                        <th style="width: 160px;">发布部门</th>
                        <th style="width: 180px;">发布时间</th>
                        <th style="display: none">编辑</th>
                        <th style="display: none">编辑部门</th>
                        <th style="width: 180px;">更新时间</th>
                        <th style="width: 90px;">图片新闻</th>
                        <th style="width: 50px;">附件</th>
                        <th style="width: 80px;">点击</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
            <ul id='Pageinator'></ul>
        </div>
        <div id="layerOpt" style="display: none">
            <div class="formtitle"><span>图片设置</span></div>
            <%--start--%>
            <ul class="imglist">
            </ul>
            <%--end--%>
        </div>

        <script type="text/javascript">

            var isPicxw = true;
            var Pageinator = $('#Pageinator');
            /*刷新重新加载*/
            function Refresh(pageindex) {

                articleBind(pageindex);
            }

            function articleBind(PageIndex) {

                layer.load();
                var JsonString = (isPicxw == true) ? "{'isPicxw':'1'}" : "{'subjectID':'017'}";
                var btnHTML = isPicxw == true ? "<a href='#' onclick='setHomePic({0},398,290,this,\"新闻\")' class='tablelink' > 设为图片新闻轮播</a>" : "<a href='#' onclick='setHomePic({0},136,98,this,\"产品\")' class='tablelink' > 设为首页产品图</a>";

                var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 15, "PageIndex": PageIndex });
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
                        $(data.list).each(function (i) {

                            data.list[i].isIncludePic = data.list[i].isPicxw == true ? "是" : "否";
                            var optHtml = btnHTML;
                            data.list[i].isIncludeAcc = data.list[i].isIncludeAcc == true ? "是" : "否";
                            var firstCtl = data.list[i].isPassed == "1" ? "<img src='../images/ysh.png' />" : "<img src='../images/dsh.png' />";

                            var optHtmlStringformat = optHtml.format(data.list[i].articleID);
                            var tr_html = "<tr {0}><td>{12}</td><td>{13}</td><td style='display:none'>{1}</td><td style='max-width:370px;'><a href='preview.aspx?opt=preview&&articleID={1}' id='btnEdit' target='_blank'>{2}</a></td><td style='max-width:280px;'>{3}</td><td style='max-width:100px;'>{4}</td><td style='max-width:100px;'>{5}</td><td>{6}</td><td style='display:none'>{7}</td><td style='display:none'>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{14}</td></tr>";
                            var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                            tr_html = tr_html.format(tr_classs, data.list[i].articleID, data.list[i].title, data.list[i].keywords, data.list[i].author, data.list[i].releaseDep, data.list[i].releaseTime, data.list[i].editor, data.list[i].editorDep, data.list[i].updateTime, data.list[i].isIncludePic, data.list[i].isIncludeAcc, optHtmlStringformat, firstCtl, data.list[i].hits);
                            row_index++;
                            html = html + tr_html;
                        });


                        $("#article-table-list tbody").html(html);

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

                        layer.closeAll('loading');
                    }
                });
            }

            //图片新闻设置首页图片
            function setHomePic(articleID, width, height, obj, t) {

                $("#layerOpt").attr("data-id", articleID);
                var title = $(obj).parent().parent().find("td").eq(3).text();
                $("#layerOpt").attr("data-name", title);
                $("#layerOpt").attr("data-type", t);


                $.ajax({

                    type: "post",
                    url: "../ashx/article.ashx?method=xwConfigPic",
                    data: {
                        articleID: articleID
                    },
                    dataType: "json",
                    traditional: true,

                    success: function (data) {

                        var HTMLSection = "";
                        for (var i = 0; i < data.length; i++) {

                            var btnHTML = data[i].isDefault == true ? '' : '<input type="button" class="scbtn" value="图片确认" id="btnConfigOk" onclick="ConfigOkClick(this)">';
                            HTMLSection += '<li class="selected" ><span><img src="' + data[i].picUrl + '" style="width:' + width + 'px;height:' + height + 'px"></span><h2><a href="#"></a></h2><p>' + btnHTML + '</p></li>';
                        }

                        $("#layerOpt .imglist").html(HTMLSection);
                    }
                });

                layerOpen();
            }

            function layerOpen() {
                layer.open({
                    type: 1,
                    title: false,
                    skin: 'layui-layer-rim', //加上边框
                    closeBtn: 1,
                    area: ['920px', '740px'],
                    shadeClose: true,
                    content: $('#layerOpt')
                });
            }

            function ConfigOkClick(obj) {

                layer.confirm('当前操作[首页新闻图片设置],是否继续?', function (index) {

                    var $img = $(obj).parent().parent().find("img");
                    var defaultPicUrl = $img.attr("src");
                    var articleID = $("#layerOpt").attr("data-id");
                    var articleTitle = $("#layerOpt").attr("data-name");
                    var type = $("#layerOpt").attr("data-type");

                    $.ajax({

                        type: "post",
                        url: "../ashx/article.ashx?method=articleXwNesDefaultPic",
                        data: {
                            defaultPicUrl: defaultPicUrl,
                            articleID: articleID,
                            articleTitle: articleTitle,
                            type: type
                        },
                        dataType: "json",
                        traditional: true,

                        success: function (data) {

                            if (data.status == "ok") {
                                layer.msg('设置成功!', { icon: 1, time: 1500 });
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
    </form>
</body>
</html>
