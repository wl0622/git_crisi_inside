<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="preview.aspx.cs" Inherits="inside.admin.web.aspx.preview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8" />
    <title>发布预览－预览审核</title>
    <meta content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0" name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link rel="stylesheet" href="../css/preview/index.css" />
    <link rel="stylesheet" href="../css/preview/children.css" />
    <%--page js--%>
    <script type="text/javascript" src="../js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../js/helper.js"></script>
    <%-- end-----%>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="../js/lib/layer/2.4/layer.js"></script>
    <link href="../js/lib/rightbtn/css/style.css" rel="stylesheet" type="text/css" />
    <%-- end-----%>
    <script type="text/javascript">

        $(function () {

            var $img = $("#btn").find("img");
            $img.bind("click", release);

        });

        function release() {
            layer.confirm('确认审核并发布?', function (index) {

                //获取url参数
                var articleID = getQueryVariable("articleID");

                $.ajax({
                    url: "/ashx/article.ashx?method=audit",
                    dataType: "json",
                    data: {
                        articleID: articleID,
                        userCnName: "<%=currentUserCnName%>",
                        title: "<%=model.title%>"
                    },
                    type: "post",
                    success: function (data) {

                        if (data.status == "ok") {

                            layer.confirm('审核并发布成功!是否关闭预览页?', function (index) {

                                window.close();
                            },
                            function () {

                            });

                        }
                        else {

                            layer.msg(data.message, { icon: 2, time: 3000 });
                        }
                    }
                })
            },
            function () {

            });
        }

    </script>
</head>
<body>



    <div class="contentbox wid_main bora_5">
        <div>
            <ul>
                <li class="l ml-10">
                    <%=previewAuditHtml %>
                </li>
                <li id="btn" class="r mr-10">
                    <%=auditButtonHtml %>
                    <img src="../images/content_03.jpg" style="cursor: pointer" onclick="javascript:window.close()" />
                </li>
            </ul>

        </div>
        <div class="content" style="padding: 0px;">
            <!-- 子页内容框 -->
            <div class="zy_contentbox">
                <!-- zy_content -->
                <div class="zy_content fix">

                    <div class="zy_conbox">
                        <div class="zy_newsxq">
                            <div class="xq_tit bdb_d">
                                <h1><%=model.title %></h1>
                                <p>
                                    <span>文章来源：<%=model.releaseDep %> </span>
                                    <span>作者：<%=model.author %> </span>
                                    <span>发布时间：<%=model.releaseTime!=null? model.releaseTime.ToString():"" %></span>
                                </p>
                            </div>
                            <div class="txt">
                                <%=model.content %>
                            </div>
                        </div>
                    </div>

                </div>
                <!-- zy_content end -->
            </div>
            <!-- 内容框end -->
            <!-- 友情链接 -->
        </div>
    </div>

</body>
</html>
