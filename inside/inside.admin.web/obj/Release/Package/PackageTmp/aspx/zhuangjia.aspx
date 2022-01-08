<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="zhuangjia.aspx.cs" Inherits="inside.admin.web.aspx.zhuangjia" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <%--page css--%>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />


    <%--page js--%>
    <script type="text/javascript" src="../js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../js/select-ui.min.js"></script>
    <script type="text/javascript" src="../js/helper.js"></script>
    <script type="text/javascript" src="../js/jquery.form.js"></script>
    <%-- end-----%>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <%-- end-----%>

    <%--paginator--%>
    <link rel="stylesheet" href="../js/lib/paginator/bootstrapv3.css" />
    <%-- <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>--%>
    <script src="../js/lib/paginator/bootstrap-paginator.js" type="text/javascript"></script>
    <%-- end-----%>


    <link href="../umeditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" charset="utf-8" src="../umeditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../umeditor/umeditor.min.js"></script>
    <script type="text/javascript" src="../umeditor/lang/zh-cn/zh-cn.js"></script>

    <%--lib------uploadfile--%>
    <script type="text/javascript" src="../js/lib/uploadfile/ajaxfileupload.js"></script>
    <%-- end-----%>

    <style type="text/css">
        #zhuanjiaform {
        }

            #zhuanjiaform ul {
                margin: 0 auto;
                height: 35px;
                margin-top: 12px;
            }

                #zhuanjiaform ul li {
                    float: left;
                    margin-left: 8px;
                    list-style: none;
                }

            #zhuanjiaform .left {
                float: left;
                width: 50%;
            }

            #zhuanjiaform .right {
                float: left;
                width: 50%;
            }
    </style>
</head>
<body>

    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">专家管理</a></li>
            <li><a href="#">专家编辑</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>专家编辑</span></div>
        <div class="tools">
            <ul class="toolbar">
                <li class="click">
                    <a href="#" onclick="create();" target="rightFrame"><span>
                        <img src="/images/t01.png" /></span> 新增专家</a></li>
            </ul>
        </div>

        <table class="tablelist" id="article-table-list">
            <thead>
                <tr>
                    <th style="width: 150px">操作</th>
                    <th style="width: 80px">专家编号</th>
                    <th style="width: 100px">专家姓名</th>
                    <th style="width: 200px">专业</th>
                    <th style="width: 260px">职称</th>
                    <th style="width: 150px">学位</th>
                    <th>是否审核通过</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <ul id='Pageinator'></ul>
    </div>
    <div id="layerOpt" style="display: none">
        <form id="zhuanjiaform" runat="server">
            <%--<div class="formtitle"><span>专家编辑</span></div>--%>
            <div class="left">
                <ul>
                    <li>
                        <label style="width: 80px;">专家姓名</label>
                        <input id="lay_txt_name" type="text" class="dfinput" name="name" />

                    </li>
                </ul>
                <ul>
                    <li>
                        <label style="width: 80px;">专业</label>
                        <input id="lay_txt_zhuanye" type="text" class="dfinput" name="zhuanye" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <label style="width: 80px;">职称</label>
                        <input id="lay_txt_zhicheng" type="text" class="dfinput" name="zhicheng" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <label style="width: 80px;">学位</label>
                        <input id="lay_txt_xuewei" type="text" class="dfinput" name="xuewei" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <!--style给定宽度可以影响编辑器的最终宽度-->
                        <script type="text/plain" id="brief" style="width: 860px; height: 300px;"></script>
                    </li>
                </ul>
                <ul>
                    <li>
                        <input id="lay_txt_zhuanjiaID" type="hidden" class="dfinput" name="zhuanjiaID" value="0" />
                    </li>
                </ul>
            </div>
            <div class="right">
                <ul>
                    <li>
                        <a class="ibtnDel" style="margin-right: 3px;">清空照片</a>
                        <input type="file" id="photo" name="photo" />
                    </li>
                </ul>
                <ul>
                    <li>
                        <img style="width: 90px; height: 114px; border: 1px solid #efefef" id="imgPhoto" />
                    </li>
                </ul>
            </div>
        </form>
        <div style="clear: both; float: right; margin-top: 15px; margin-right: 30px">
            <a class="ibtn_max" style="margin: 0 5px;" onclick="save();"><span class="glyphicon glyphicon-floppy-saves">保存</span></a>
        </div>
    </div>

    <script type="text/javascript">

        //实例化编辑器
        var um = UM.getEditor('brief');


        $(function () {

            zhuangjiaBind(1);
        });

        var Pageinator = $('#Pageinator');
        /*刷新重新加载*/
        function Refresh(pageindex) {

            zhuangjiaBind(pageindex);
        }

        function zhuangjiaBind(PageIndex) {

            layer.load();

            var JsonString = "{'name ':''}";
            var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 15, "PageIndex": PageIndex });
            $.ajax({

                type: "post",
                url: "../ashx/zhuanjia.ashx?method=zhuanjiabind",
                contentType: "application/json; charset=utf-8",
                data: dataSend,
                dataType: "json",
                traditional: true,
                success: function (data) {

                    var row_index = 1;
                    var html = "";
                    $(data.list).each(function (i) {

                        var tr_html = "<tr {0}><td><a href='#' onclick='edit({1});' class='tablelink' > 编辑</a> <a href='#' onclick='del({1},this);' class='tablelink' > 删除</a><a href='#' onclick='chk({1},this);' class='tablelink' > 审核</a></td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(
                            tr_classs,
                            data.list[i].zhuanjiaID,
                            data.list[i].name,
                            data.list[i].zhuanye,
                            data.list[i].zhicheng,
                            data.list[i].xuewei,
                            data.list[i].isPassed == true ? "是" : "否"
                            );
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
                },
                complete: function () {
                    layer.closeAll('loading');
                }
            });
        }

        function save() {

            $('#zhuanjiaform').ajaxSubmit({
                url: 'zhuangjia.aspx',
                type: 'post',
                dataType: 'text',
                beforeSubmit: function () {

                    if ($("#lay_txt_name").val() == "") { layer.msg('专家姓名不能为空!', { icon: 5, time: 1500 }); return false; }
                    if ($("#lay_txt_zhuanye").val() == "") { layer.msg('专业不能为空!', { icon: 5, time: 1500 }); return false; }
                    var html = UM.getEditor('brief').getContent();
                    if (html == "") { layer.msg('专家介绍不能为空!', { icon: 5, time: 1500 }); return false; }
                },
                success: function (responseText, statusText, xhr, $form) {

                    var jsonstr = responseText.match(/\{[^\}]+\}/)[0];
                    var reg = /{"status":"(ok|error)","message":([\s\S]*)}/;

                    if (reg.test(jsonstr)) {
                        var msg = $.parseJSON(jsonstr);

                        if (msg.status == "ok") {

                            if ($("#photo").val() != "") {
                                layer.load();
                                $.ajaxFileUpload(
                                   {
                                       url: 'zhuanjiaupload.aspx?zhuanjiaid=' + msg.message, //用于文件上传的服务器端请求地址
                                       secureuri: false, //一般设置为false
                                       fileElementId: "photo", //文件上传空间的id属性  <input type="file" id="file" name="file" />
                                       dataType: 'json', //返回值类型 一般设置为json
                                       success: function (data, status)  //服务器成功响应处理函数
                                       {
                                           $("#imgPhoto").attr("src", data.imgurl);
                                           layer.msg('保存成功!', { icon: 1, time: 1500 });
                                           setTimeout(function () { window.location.reload(); }, 1500);
                                       },
                                       error: function (data, status, e)//服务器响应失败处理函数
                                       {
                                           layer.closeAll('loading');
                                           layer.msg(e, { icon: 2, time: 2000 });
                                       }
                                   }
                               )

                            }
                            else {
                                layer.msg('保存成功!', { icon: 1, time: 1500 });
                                setTimeout(function () { window.location.reload(); }, 1500);
                            }


                        }
                        else {
                            layer.msg('无权限操作,请联系管理员!', { icon: 2, time: 1500 });
                        }
                    }
                },
                clearForm: false,//禁止清楚表单
                resetForm: false //禁止重置表单
            });
        }

        function create() {

            $("#lay_txt_zhuanjiaID").val("0");
            UM.getEditor('brief').setContent("", false);

            $("#zhuanjiaform input[type=text]").each(function () {
                $(this).val("");
            });

            layerOpen();
        }

        function del(zhuanjiaID, obj) {


            layer.confirm('当前操作[删除数据],是否继续?', function (index) {

                layer.load();

                $.ajax({
                    url: '../ashx/zhuanjia.ashx?method=delByID',
                    type: 'post',
                    dataType: "json",
                    data: {
                        zhuanjiaID: zhuanjiaID,
                        name: $(obj).parent().parent().children().eq(2).text()
                    },
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('数据已删除!', { icon: 1, time: 1500 });
                            setTimeout(function () { window.location.reload(); }, 1500);

                        }
                        else {
                            layer.msg(data.message, { icon: 2, time: 2000 });
                        }
                    },
                    complete: function () {
                        layer.closeAll('loading');
                        $("#lay_txt_zhuanjiaID").val("0")
                    }

                });
            },
             function () {

             });
        }


        function chk(zhuanjiaID, obj) {

            layer.confirm('当前操作[审核],是否继续?', function (index) {

                layer.load();

                $.ajax({
                    url: '../ashx/zhuanjia.ashx?method=chkByID',
                    type: 'post',
                    dataType: "json",
                    data: {
                        zhuanjiaID: zhuanjiaID,
                        name: $(obj).parent().parent().children().eq(2).text()
                    },
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('审核完成!', { icon: 1, time: 1500 });
                            setTimeout(function () { window.location.reload(); }, 1500);

                        }
                        else {
                            layer.msg(data.message, { icon: 2, time: 2000 });
                        }
                    },
                    complete: function () {
                        layer.closeAll('loading');
                        $("#lay_txt_zhuanjiaID").val("0")
                    }

                });
            },
             function () {

             });
        }


        function edit(zhuanjiaID) {

            $.ajax({

                type: "post",
                url: "../ashx/zhuanjia.ashx?method=zhuanjiaInfoByID",
                dataType: "json",
                data: {
                    zhuanjiaID: zhuanjiaID
                },
                dataType: "json",
                traditional: true,
                success: function (data) {

                    $("#lay_txt_name").val(data.name);
                    $("#lay_txt_zhuanye").val(data.zhuanye);
                    $("#lay_txt_zhicheng").val(data.zhicheng);
                    $("#lay_txt_xuewei").val(data.xuewei);
                    $("#lay_txt_zhuanjiaID").val(data.zhuanjiaID);
                    $("#imgPhoto").attr("src", data.photoname);

                    layerOpen();
                    UM.getEditor('brief').setContent(data.brief, false);

                }
            });

        }

        function layerOpen() {
            layer.open({
                type: 1,
                title: false,
                skin: 'layui-layer-rim', //加上边框
                closeBtn: 1,
                area: ['920px', '720px'],
                shadeClose: true,
                content: $('#layerOpt')
            });
        }

    </script>

</body>
</html>
