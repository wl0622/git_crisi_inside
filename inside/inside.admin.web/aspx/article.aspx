﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="article.aspx.cs" Inherits="admin.web.aspx.article" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript">
        var jQuery_general = jQuery.noConflict();
    </script>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <script type="text/javascript" src="../js/select-ui.min.js"></script>

    <script type="text/javascript">
        jQuery_general(function (e) {

            jQuery_general(".select1").uedSelect({ width: 345 });

            jQuery_general("#usual1 ul").idTabs();

            jQuery_general('.tablelist tbody tr:odd').addClass('odd');

            /*右侧按钮*/
            jQuery_general(window).scroll(function () {

                if ($(window).scrollTop() > 300) {
                    $('#jump li:eq(0)').fadeIn(800);
                } else {
                    $('#jump li:eq(0)').fadeOut(800);
                }
            });

            $("#top").click(function () {
                $('body,html').animate({
                    scrollTop: 0
                },
                    1000);
                return false;
            });

        });
    </script>

    <%//---------------------------------umeditor start-----------------------------------------------//%>
    <link href="../umeditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../umeditor/third-party/jquery.min.js"></script>
    <%--  <script type="text/javascript">
              var jQuery_umeditor_plugin = jQuery.noConflict();
          </script>
    --%>
    <script type="text/javascript" charset="utf-8" src="../umeditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../umeditor/umeditor.min.js"></script>
    <script type="text/javascript" src="../umeditor/lang/zh-cn/zh-cn.js"></script>
    <%//-----------------------------------umeditor end-------------------------------------------------//%>




    <%//---------------------------------ueditor start--------------------------------------------------//%>

    <script type="text/javascript" charset="utf-8" src="../ueditor/1.4.3/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/1.4.3/ueditor.all.min.js"> </script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/1.4.3/lang/zh-cn/zh-cn.js"></script>

    <%//-----------------------------------ueditor end-------------------------------------------------//%>

    <%--lib--%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>


    <script type="text/javascript">

        function checknull() {

            var subject = $("#cmbSubject").val();
            var special = $("#cmbSpecial").val();
            var title = $('#txtTitle').val();

            if (subject == "" || special == "" || title == "") {
                return false;
            }
            else { return true; }
        }


        function save() {

            if (checknull() == false) {

                layer.msg('关键信息不能为空!', { icon: 2, time: 1000 });

            }
            else {

                var strdata = $("#articleform").serializeArray();
                $.ajax({
                    url: "/ashx/article.ashx?method=save",
                    dataType: "json",
                    data: { jsondata: JSON.stringify(strdata) },
                    type: "post",
                    success: function (data) {

                        if (data.status == "ok") {

                            layer.confirm('发布保存成功?是否清空编辑器', function (index) {

                                UM.getEditor('myEditor').setContent('', false);

                                layer.msg('已清空!', { icon: 1, time: 1000 });
                            },
                            function () {

                            });
                        }
                        else {

                            layer.msg(data.message, { icon: 2, time: 3000 });
                        }
                    }
                })
            }
        }

        function ueditorpreview() {


            if (checknull() == false) {

                layer.msg('关键信息不能为空!', { icon: 2, time: 1500 });

            }
            else {

                document.articleform.target = "_blank"
                document.articleform.action = "ueditornews.aspx";
                document.articleform.submit();
            }
        }

        function umeditorpreview() {


            if (checknull() == false) {

                layer.msg('关键内容不能为空!', { icon: 2, time: 1500 });

            }
            else {

                document.articleform.target = "_blank"
                document.articleform.action = "umeditornews.aspx";
                document.articleform.submit();
            }
        }
    </script>

    <link href="../js/lib/rightbtn/css/style.css" rel="stylesheet" type="text/css" />


</head>
<body>

    <form name="articleform" id="articleform" method="post">

        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="/mail.aspx">首页</a></li>
                <li><a href="articlelist.aspx">列表</a></li>
                <li><a href="#">发布文章</a></li>
            </ul>
        </div>
        <div class="formbody">
            <div id="usual1" class="usual">
                <div class="itab">
                    <ul>
                        <li><a href="#tab1" class="selected">基本信息</a></li>
                        <li><a href="#tab2">正文编辑</a></li>
                        <%--  <li><a href="#tab3">正文编辑(第2款编辑器)</a></li>--%>
                    </ul>
                </div>

                <div id="tab1" class="tabson">
                    <%--<div class="formtext">Hi，<b>admin</b>，欢迎您试用信息发布功能！</div>--%>
                    <ul class="forminfo">
                        <li>
                            <div class="gridlab">
                                <span>所属栏目<b>*</b></span>
                            </div>
                            <div class="gridCtrls">
                                <div class="vocation">
                                    <select class="select1" name="cmbSubject" id="cmbSubject">
                                        <option>测试</option>
                                        <option>其他</option>
                                    </select>
                                </div>
                            </div>
                            <div class="gridtip" style="width: 220px;">
                                注意：不能指定为含有子栏目的栏目
                            </div>
                        </li>
                        <li>
                            <div class="gridlab">
                                <span>所属专题<b>*</b></span>
                            </div>
                            <div class="gridCtrls">
                                <div class="vocation">
                                    <select class="select1" name="cmbSpecial" id="cmbSpecial">
                                        <option>测试</option>
                                        <option>其他</option>
                                    </select>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="gridlab">
                                <span>新闻标题<b>*</b></span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtTitle" id="txtTitle" type="text" class="dfinput" style="width: 518px;" value="这是一个测试的标题" />
                            </div>
                        </li>
                        <li>
                            <div class="gridlab">
                                <span>图片新闻标题</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtPicTitle" type="text" class="dfinput" style="width: 518px;" />
                            </div>
                            <div class="gridtip" style="width: 220px;">
                                注意：不能超过20个汉字
                            </div>
                        </li>
                        <li>
                            <div class="gridlab">
                                <span>头条新闻标题</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtTopNewsTitle" type="text" class="dfinput" style="width: 518px;" />
                            </div>
                        </li>
                        <li>
                            <div class="gridlab">
                                <span>关键字</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtKeyword" type="text" class="dfinput" style="width: 518px;" />
                            </div>
                            <div class="gridtip" style="width: 220px;">
                                中间用 “|” 符号分开
                            </div>
                        </li>
                        <li>
                            <div class="gridlab">
                                <span>作者</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtAuthor" type="text" class="dfinput" style="width: 88px;" />
                            </div>
                            <div class="gridlab">
                                <span>院内稿件来源</span>
                            </div>
                            <div class="gridCtrls">
                                <select class="select1" name="cmbArticleOrigin">
                                    <option>测试</option>
                                    <option>其他</option>
                                </select>
                            </div>
                            <div class="gridtip" style="width: 220px;">
                                注意：不能指定为含有下级单位的单位
                            </div>
                        </li>
                        <li>
                            <div class="gridlab">
                                <span>是否转载新闻</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="ckIsTransferNews" id="ckIsTransferNews" type="checkbox" checked="" style="margin-top: 10px" />
                            </div>
                            <div class="gridlab">
                                <span>转载新闻稿件来源</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtTransferNewsOrigin" type="text" class="dfinput" style="width: 518px;" />
                            </div>

                        </li>
                        <li>
                            <div class="gridlab">
                                <span>转载新闻稿件链接</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtTransferNewsLink" type="text" class="dfinput" style="width: 518px;" />
                            </div>
                            <div class="gridlab">
                                <span>转载新闻发布时间</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtTransferNewsDate" type="text" class="dfinput" style="width: 118px;" />
                            </div>
                        </li>
                        <li>

                            <div class="gridlab">
                                <span>编辑</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtEdit" type="text" class="dfinput" style="width: 118px;" />
                            </div>
                            <div class="gridlab">
                                <span>编辑单位</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtEditCompany" type="text" class="dfinput" style="width: 218px;" />
                            </div>
                            <div class="gridlab">
                                <span>图片排序ID</span>
                            </div>
                            <div class="gridCtrls">
                                <input name="txtPicSort" type="text" class="dfinput" style="width: 111px;" />
                            </div>
                        </li>
                    </ul>
                </div>
                <div id="tab2" class="tabson">
                    <ul>
                        <li>
                            <%--<label>文稿内容<b>*</b></label>--%>
                            <script type="text/plain" id="myEditor" style="width: 1024px; min-height: 1540px;"></script>
                            <script type="text/javascript">
                                var um = UM.getEditor('myEditor');
                            </script>
                        </li>

                    </ul>
                </div>
                <%--        <div id="tab3" class="tabson">
                    <ul>
                        <li>
                            <script id="ueditor" name="ueditorValue" type="text/plain" style="width: 1024px; height: 500px;"></script>
                            <script type="text/javascript">
                                var ue = UE.getEditor('ueditor');
                            </script>
                        </li>
                    </ul>
                </div>--%>
            </div>
        </div>
        <ul id="jump">
            <li style="display: none;"><a id="top" href="#top"></a></li>
            <li><a id="umeditorbtn" href="#" onclick="umeditorpreview();"></a></li>
            <li><a id="ueditorbtn" href="#" onclick="ueditorpreview();"></a></li>
            <li><a id="savetempbtn" href="#" onclick="tempsave();"></a></li>
            <li><a id="savebtn" href="#" onclick="save();"></a></li>
        </ul>
    </form>
</body>
</html>

