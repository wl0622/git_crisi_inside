<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="releasearticle.aspx.cs" Inherits="inside.admin.web.aspx.releasearticle" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="renderer" content="webkit|ie-comp|ie-stand" />
    <meta name="renderer" content="webkit" />
    <title></title>


    <%--page css--%>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />
    <link href="../css/table.css" rel="stylesheet" type="text/css" />

    <%--page js--%>
    <script type="text/javascript" src="../js/easyui/jquery.min.js"></script>
    <script type="text/javascript" src="../js/select-ui.min.js"></script>
    <script type="text/javascript" src="../js/helper.js"></script>
    <%-- end--%>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <%--    <link href="../js/lib/rightbtn/css/style.css" rel="stylesheet" type="text/css" />--%>
    <%-- end-----%>

    <%--lib------ComboTree--%>
    <link rel="stylesheet" type="text/css" href="../js/easyui/easyui.css" />
    <script type="text/javascript" src="../js/easyui/jquery.easyui.min.js"></script>
    <%-- end-----%>

    <%--lib-----ueditor start------%>
    <script type="text/javascript" charset="utf-8" src="../ueditor/1.4.3/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/1.4.3/ueditor.all.min.js"> </script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/1.4.3/lang/zh-cn/zh-cn.js"></script>
    <%-- end-----%>


    <%--lib------datepicker--%>
    <script type="text/javascript" src="/js/bootstrap/bootstrap.min.js"></script>
    <link href="/js/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/bootstrap/bootstrap-datepicker/js/bootstrap-datetimepicker.min.js"></script>
    <script type="text/javascript" src="/js/bootstrap/bootstrap-datepicker/js/bootstrap-datetimepicker.zh-CN.js"></script>
    <link href="/js/bootstrap/bootstrap-datepicker/css/bootstrap-datetimepicker.min.css" rel="stylesheet" type="text/css" />
    <%-- end-----%>


    <%--lib------Tabs--%>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <%-- end-----%>


    <%--paginator--%>
    <link rel="stylesheet" href="../js/lib/paginator/bootstrapv3.css" />
    <%-- <script src="../js/lib/paginator/jquery-1.9.1.min.js" type="text/javascript"></script>--%>
    <script src="../js/lib/paginator/bootstrap-paginator.js" type="text/javascript"></script>
    <%-- end-----%>

    <%--tabletree--%>
    <script type="text/javascript" src="../js/lib/treeTable/jquery.treeTable.js"></script>

    <script type="text/javascript">

        var model = null;

        $(function (e) {

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

            $("#usual1 ul").idTabs(function (id) {
                switch (id) {
                    case "#tab3":
                        todayReleaseTableBind(1);
                        break;
                    default:
                        $("#jump").css("display", "block");
                        break;
                } return true;
            });

            //栏目下拉框设置
            $(".select1").uedSelect({ width: 260 });

            //发布日期插件
            $('#releaseTime').datetimepicker({

                format: 'yyyy-mm-dd hh:mm:ss',
                language: 'zh-CN',
                autoclose: true,
                clearBtn: true,
                minView: "month"
            });

            $("#releaseTime").datetimepicker("setDate", new Date());


            //专题下拉
            $('#specialID').combotree({
                url: '../ashx/special.ashx?method=getSpecialComboTree',
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
                    if (selectedId == "021") {

                        $(".gridtable tr:eq(1)").removeAttr("style");
                    }
                    else {
                        $(".gridtable tr:eq(1)").attr("style", "display:none");
                    }
                },
                onLoadSuccess: function (node, data) {

                    //院内稿件来源
                    $('#releaseDep').combotree({
                        url: '../ashx/department.ashx?method=getDepartmentComboTree&valueColumn=briefName',
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

                            var opt = getQueryVariable("opt");
                            if (opt == false) {
                                setToolbarBtnShow("0");//发布新内容显示的按钮
                            }
                            else if (opt == "update") {

                                setToolbarBtnShow("1");//更新内容显示的按钮

                                //获取url参数
                                var articleID = getQueryVariable("articleID");
                                //根据ID获取内容
                                var JsonString = "{'articleID':'" + articleID + "'}";
                                var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 1, "PageIndex": 1 });

                                $.ajax({
                                    type: "post",
                                    url: "../ashx/article.ashx?method=articlelistbind",
                                    contentType: "application/json; charset=utf-8",
                                    data: dataSend,
                                    dataType: "json",
                                    traditional: true,
                                    success: function (data) {

                                        $(data.list).each(function (i) {

                                            model = data.list[i];
                                            //subjectID
                                            $("#subjectID").val(model.subjectID);//val
                                            $(".gridtable tr:eq(0) .uew-select-text").text($(".gridtable tr:eq(0) .select1 option[value=" + model.subjectID + "]").text());//text

                                            //specialID
                                            if (model.specialID != null) {

                                                $('#specialID').combotree('setValue', model.specialID);

                                                if (model.specialID == "021") {

                                                    $(".gridtable tr:eq(1)").removeAttr("style");

                                                    $("#JHXYid").val(model.JHXYid);//val
                                                    $(".gridtable tr:eq(1) .uew-select-text").text($(".gridtable tr:eq(1) .select1 option[value=" + model.JHXYid + "]").text());//text

                                                }
                                                else {

                                                    $(".gridtable tr:eq(1)").attr("style", "display:none");
                                                }
                                            }
                                            //title
                                            $("#title").val(model.title);
                                            //keywords
                                            $("#keywords").val(model.keywords);
                                            //isOnTop
                                            if (model.isOnTop) {
                                                $("#isOnTop").attr("checked", true);
                                                var $parent = $("#isOnTop").parent().parent();
                                                var $input = $parent.find("input[type=text]");
                                                $input.each(function () {
                                                    $(this).attr("class", "dfinput");
                                                    $(this).removeAttr("disabled");
                                                });

                                                $("#titletoutiao").val(model.titletoutiao);
                                            }
                                            //isPicxw
                                            if (model.isPicxw) {
                                                $("#isPicxw").attr("checked", true);
                                                //var $parent = $("#isPicxw").parent().parent();
                                                //var $input = $parent.find("input[type=text]");
                                                //$input.each(function () {
                                                //    $(this).attr("class", "dfinput");
                                                //    $(this).removeAttr("disabled");
                                                //});
                                            }
                                            //issync
                                            if (model.issync) {
                                                $("#issync").attr("checked", true);
                                            }
                                            //isTop
                                            if (model.isTop) {
                                                $("#isTop").attr("checked", true);
                                            }
                                            //author
                                            $("#author").val(model.author);
                                            //releaseDep
                                            if (model.reprint == null || model.reprint == "") {

                                                $("#releaseDep").combotree('setValue', model.releaseDep);
                                            }
                                            else {
                                                $("#ckIsreprint").attr("checked", true);
                                                var $parent = $("#ckIsreprint").parent().parent();
                                                var $input = $parent.find("input[type=text]");
                                                $input.each(function () {
                                                    $(this).attr("class", "dfinput");
                                                    $(this).removeAttr("disabled");
                                                });

                                                $("#releaseDep_reprint").val(model.releaseDep);
                                                $("#reprint").val(model.reprint);
                                            }
                                            //editor
                                            //editorDep
                                            var s = new Date(model.releaseTime.replace(/\-/g, "\/"));
                                            $("#releaseTime").datetimepicker("setDate", s);
                                            //content
                                            ue.setContent(model.content);

                                        });
                                    }
                                })
                            }
                        }
                    });
                }
            });
        });

        //点击复选框后文本框的切换
        function setDisabled(obj) {

            var $parent = $(obj).parent().parent();
            var $input = $parent.find("input[type=text]");

            $input.each(function () {

                if ($(this).attr("class") == "dfinput") {
                    $(this).attr("class", "dfinput_un");
                    $(this).attr("disabled", "disabled");
                    $(this).val("");
                }
                else if ($(this).attr("class") == "dfinput_un") {
                    $(this).attr("class", "dfinput");
                    $(this).removeAttr("disabled");
                }
            });
        }

        function releaseArticle() {

            layer.load();

            var specialID = $("#specialID").combotree("getValue");

            if ($("#subjectID").val() == "-1" && specialID == "") {
                layer.msg('请选择所属栏目或者专题至少选择一项!', { icon: 5, time: 1500 });
                layer.closeAll('loading');
                return false;
            }

            if ($("#title").val() == "") {
                layer.msg('请填写标题!', { icon: 5, time: 1500 });
                layer.closeAll('loading');
                return false;
            }


            if ($("#author").val() == "") {
                layer.msg('请填写作者!', { icon: 5, time: 1500 });
                layer.closeAll('loading');
                return false;
            }

            var releaseDep = $("#releaseDep").combotree('getValue');
            var releaseDep_reprint = $("#releaseDep_reprint").val();




            if (releaseDep == "" && releaseDep_reprint == "" && specialID != "021") {
                layer.msg('请选择稿件来源,院内和转载须二选一', { icon: 5, time: 1500 });
                layer.closeAll('loading');
                return false;
            }

            if (releaseDep != "" && releaseDep_reprint != "") {
                layer.msg('院内来源和转载来源只能二选一', { icon: 5, time: 1500 });
                layer.closeAll('loading');
                return false;
            }

            var getContent = ue.getContentTxt();
            if (getContent == "") {
                layer.msg('文章内容不能为空!', { icon: 5, time: 1500 });
                layer.closeAll('loading');
                return false;
            }

            var formJsonString = $("#articleform").serializeArray();

            $.ajax({
                url: "/ashx/article.ashx?method=release",
                dataType: "json",
                data: {
                    jsondata: JSON.stringify(formJsonString),
                    modeldata: model == null ? null : JSON.stringify(model)
                },
                type: "post",
                success: function (data) {

                    if (data.status == "ok") {

                        layer.msg('保存成功!', { icon: 1, time: 1500 });
                        setTimeout(function () { window.close(); }, 1500);


                        //layer.confirm('发布保存成功!是否清空?', function (index) { }, function () { });
                    }
                    else {

                        layer.msg(data.message, { icon: 2, time: 3000 });
                    }
                },
                complete: function () {
                    layer.closeAll('loading');
                }
            })
        }

        function setToolbarBtnShow(multipleFalg) {

            var falg = multipleFalg.toString().split(",");

            var $toolbar = $("#jump");

            for (var i = 0; i < falg.length; i++) {


                $toolbar.find("li:eq(" + falg[i] + ")").css("display", "block");
            }
        }

        function clearSpecialComboTree() {

            $('#specialID').combotree('setValue', "");
        }

        function clearReleaseDepComboTree() {

            $('#releaseDep').combotree('setValue', "");
        }


        function buildJHXY() {

            $.ajax({
                url: "/ashx/article.ashx?method=buildJHXY",
                dataType: "json",
                data: {},
                type: "post",
                success: function (data) {

                    if (data.status == "ok") {

                        var item = $.parseJSON(data.message);
                        var itemHTML = "<option value=" + item.id + ">" + item.volume_name + "</option>";
                        $("#JHXYid option[value='-1']").after(itemHTML);
                        layer.msg('新期次已生成在下拉选项框中!', { icon: 1, time: 1500 });

                        //setTimeout(function () { window.close(); }, 1500);
                        //layer.confirm('发布保存成功!是否清空?', function (index) { }, function () { });
                    }
                    else {

                        layer.msg(data.message, { icon: 2, time: 3000 });
                    }
                },
                complete: function () {
                    layer.closeAll('loading');
                }
            })
        }


    </script>

    <style type="text/css">
        table.gridtable {
            font-family: "SimSun";
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #d0dee5;
            border-collapse: collapse;
        }

            table.gridtable td {
                border-width: 1px;
                padding: 8px;
                border-style: solid;
                border-color: #d0dee5;
                background-color: #ffffff;
                font-size: 16px;
            }

        table .tdLabel {
            border-width: 1px;
            padding: 8px;
            border-style: solid;
            border-color: #d0dee5;
            background-color: #dedede;
        }

        .blueText {
            color: #428bca;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <form name="articleform" id="articleform" method="post">
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="#">内容发布管理</a></li>
                <li>
                    <img src="/images/main/arrow.gif" /></li>
                <li><a href="#">发布新文章</a></li>

            </ul>
        </div>
        <div class="formbody">
            <div id="usual1" class="usual">
                <div class="itab">
                    <ul>
                        <li><a id="#tab1" href="#tab1" class="selected">内容编辑</a></li>
                        <li><a id="#tab3" href="#tab3">我发布的</a></li>
                    </ul>
                </div>

                <div id="tab1" class="tabson" style="width: 1520px">
                    <table class="gridtable">
                        <tr>
                            <td class="blueText">所属栏目<i style="font-size: 12px; color: #ea2020">(2选1)</i></td>
                            <td>
                                <select class="select1" name="subjectID" id="subjectID">
                                    <% 
                                        StringBuilder selectHtml = new StringBuilder("<option value='-1'>--请选择栏目--</option>");
                                        foreach (inside.admin.web.entityframework.tableEntity.t_subject_list_model m in subjectlist)
                                        {
                                            selectHtml.Append(string.Format("<option value='{0}'>{1}</option>", m.subjectID, m.subjectName));
                                        }
                                        selectHtml.Append(string.Format("<option value='018003'>EnglishNews</option>"));
                                        Response.Write(selectHtml.ToString());
                                    %>
                                </select>
                            </td>
                            <td class="blueText">所属专题<i style="font-size: 12px; color: #ea2020">(2选1)</i></td>
                            <td colspan="3">
                                <input name="specialID" id="specialID" type="text" class="dfinput" style="min-width: 518px;" />
                                <input type="button" class="scbtn" value="清空" style="margin-left: 5px; width: 50px;" onclick="clearSpecialComboTree();" />
                                <input type="button" class="scbtn" value="专题栏目维护" style="margin-left: 5px; width: 90px;" onclick="setSpecialEdit();" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="blueText">江花心语<i style="font-size: 12px; color: #ea2020"></i></td>
                            <td>
                                <select class="select1" name="JHXYid" id="JHXYid">
                                    <% 
                                        StringBuilder JHXYItems = new StringBuilder("<option value='-1'>--请选择期次--</option>");
                                        foreach (crsri.cn.Model.t_web_jhxy_model m in jhxyItems)
                                        {
                                            JHXYItems.Append(string.Format("<option value='{0}'>{1}</option>", m.id, m.volume_name));
                                        }
                                        Response.Write(JHXYItems.ToString());
                                    %>
                                </select>
                            </td>
                            <td class="blueText">
                                <input type="button" class="scbtn" value="创建新期次" style="margin-left: 5px; width: 90px;" onclick="buildJHXY();" /><i style="font-size: 12px; color: #ea2020"></i></td>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td class="blueText">是否图片新闻</td>
                            <td>
                                <input name="isPicxw" id="isPicxw" type="checkbox" style="width: 20px; height: 20px;" /></td>
                            <td class="blueText">新闻标题<i style="font-size: 12px; color: #ea2020">(*必填)</i></td>
                            <td colspan="3">
                                <input name="title" id="title" type="text" class="dfinput" style="width: 560px;" /></td>
                        </tr>
                        <tr>
                            <td class="blueText">关键字</td>
                            <td colspan="2">
                                <input name="keywords" id="keywords" type="text" class="dfinput" style="width: 418px;" /></td>
                            <td colspan="2">
                                <input name="issync" id="issync" type="checkbox" style="width: 20px; height: 20px;" />
                                同步到外网 </td>
                            <td>
                                <input name="isTop" id="isTop" type="checkbox" style="width: 20px; height: 20px;" />
                                设为置顶
                            </td>
                        </tr>
                        <tr>
                            <td class="blueText">是否新闻头条</td>
                            <td>
                                <input name="isOnTop" id="isOnTop" type="checkbox" style="width: 20px; height: 20px;" onclick="setDisabled($(this));" />
                            </td>
                            <td class="blueText">头条新闻标题</td>
                            <td colspan="3">
                                <input name="titletoutiao" id="titletoutiao" type="text" disabled="disabled" class="dfinput_un" style="width: 450px;" /><i style="font-size: 12px; color: #ea2020">不超过24字</i></td>
                        </tr>
                        <tr>
                            <td class="blueText">作者<i style="font-size: 12px; color: #ea2020">(*必填)</i></td>
                            <td>
                                <input name="author" id="author" type="text" class="dfinput" style="width: 88px;" /></td>
                            <td class="blueText">院内稿件来源<i style="font-size: 12px; color: #ea2020">(*院内和转载二选一)</i></td>
                            <td colspan="3">
                                <input name="releaseDep" id="releaseDep" type="text" class="dfinput" style="min-width: 480px;" /><input type="button" class="scbtn" value="清空" style="margin-left: 5px; width: 50px;" onclick="clearReleaseDepComboTree();" /></td>
                        </tr>
                        <tr>
                            <td class="blueText">是否转载新闻</td>
                            <td>
                                <input name="ckIsreprint" id="ckIsreprint" type="checkbox" style="width: 20px; height: 20px;" onclick="setDisabled($(this));" /></td>
                            <td class="blueText">转载稿件来源<i style="font-size: 12px; color: #ea2020">(*转载和院内二选一)</i></td>
                            <td>
                                <input name="releaseDep_reprint" id="releaseDep_reprint" type="text" disabled="disabled" class="dfinput_un" style="width: 160px;" /></td>
                            <td class="blueText">转载稿件链接</td>
                            <td>
                                <input name="reprint" id="reprint" type="text" disabled="disabled" class="dfinput_un" style="width: 318px;" /></td>
                        </tr>
                        <tr>
                            <td class="blueText">编辑</td>
                            <td>
                                <input name="editor" id="editor" type="text" class="dfinput" style="width: 118px;" value="<%=curUserCnName %>" readonly="readonly" /></td>
                            <td class="blueText">编辑单位</td>
                            <td>
                                <input name="editorDep" id="editorDep" type="text" class="dfinput" style="width: 218px;" value="<%=curUserbriefName %>" readonly="readonly" /></td>
                            <td class="blueText">发布时间</td>
                            <td>
                                <input name="releaseTime" id="releaseTime" type="text" class="dfinput" style="width: 198px;" readonly="readonly" /></td>
                        </tr>

                    </table>
                    <ul style="margin: 20px 0px">
                        <li>
                            <script id="ueditor" name="ueditorValue" type="text/plain" style="max-width: 920px;"></script>
                            <script type="text/javascript">

                                var ue = UE.getEditor('ueditor', {
                                    initialFrameHeight: 400,
                                    initialFrameWidth: 920,
                                    scaleEnabled: true
                                });
                            </script>
                        </li>

                    </ul>

                    <div class="bottomBtn">
                        <div class="tools" style="width: 105px; border-bottom: 1px solid #a4bed4; background-color: #fff; vertical-align: middle; height: 35px; line-height: 35px; margin: auto; margin-top: 4px; cursor: pointer">
                            <ul class="toolbar" id="jump">
                                <li onclick="releaseArticle();" style="display: none;"><span>
                                    <img src="/images/t01.png" /></span> 提交保存</li>
                                <li onclick="releaseArticle();" style="display: none"><span>
                                    <img src="/images/t02.png" /></span>更新保存</li>

                            </ul>
                        </div>
                        <img src="../images/content_03.png" style="cursor: pointer; position: fixed; top: 5px; right: 1px;" onclick="javascript:window.close()" />
                    </div>

                </div>

                <div id="tab3" class="tabson">
                    <table class="tablelist" id="article-table-list">
                        <thead>
                            <tr>
                                <th style="width: 50px;">发布</th>
                                <th style="display: none">编号<i class="sort"><img src="/images/px.gif" /></i></th>
                                <th>标题</th>
                                <th style="width: 220px;">关键词</th>
                                <th style="width: 110px;">作者</th>
                                <th style="width: 150px;">发布部门</th>
                                <th style="width: 180px;">发布时间</th>
                                <th style="display: none">编辑</th>
                                <th style="display: none">编辑部门</th>
                                <th style="width: 180px;">更新时间</th>
                                <th style="width: 50px;">图片</th>
                                <th style="width: 50px;">附件</th>
                                <th style="width: 80px;">点击</th>
                                <th style="width: 260px;">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <ul id='Pageinator'></ul>
                </div>
            </div>

        </div>
        <div id="layerOpt" style="display: none">
            <div class="formtitle"><span>设置图片-图片新闻</span></div>
            <%--start--%>
            <ul class="imglist">
            </ul>
            <%--end--%>
        </div>
        <div id="layerSpecial" style="display: none">
            <div class="formtitle"><span>专题栏目编辑</span></div>
            <table id="treeTable1">
                <tr>
                    <th style="width: 600px;">专题&栏目名称</th>
                    <th style="width: 160px;">添加子菜单</th>
                    <th style="width: 100px;">删除</th>
                    <th style="width: 100px;">修改</th>
                </tr>
                <%

                    speciallist.ForEach(delegate(inside.admin.web.entityframework.tableEntity.t_special_list_model m)
                    {
                        Response.Write(string.Format(@"<tr id='{0}' pid='{1}'>
                <td><span controller='true'>{2}</span></td>
                <td>{3}</td><td>{4}</td><td>{5}</td>
            </tr>", m.specialID, (m.specialID.Length == 3) ? "0" : m.specialID.Substring(0, 3), m.specialName, m.specialID.Length == 3 ? "</span><a class='ibtn' onclick='addSpecialChildren(\"" + m.specialID + "\")'><span class='glyphicon glyphicon-plus'>添加子栏目</a>" : "",
                                                                          m.specialID.Length > 3 ? "<a class='ibtn' onclick='delChildren(\"" + m.specialID + "\",\"" + m.specialName + "\",)'><span class='glyphicon glyphicon-trash'>删除</a>" : "",
                                                                                                "<a class='ibtn' onclick='updateNodeName(\"" + m.specialID + "\",\"" + m.specialName + "\",)'><span class='glyphicon glyphicon-pencil'>修改</a>"));
                    });
                  
                %>
            </table>
        </div>
        <div id="layerOptDialog" style="display: none">
            <div class="formtitle"><span>编辑</span></div>
            <ul class="forminfo">
                <li>
                    <label style="width: 80px;">栏目标题</label><input id="layerName" type="text" class="dfinput" style="width: 500px;" /></li>
            </ul>
            <div style="margin: 0px auto; text-align: center; width: 120px;" id="layerSave" onclick="layerSave();">
                <a class="ibtn_max"><span class="glyphicon glyphicon-floppy-saves">保存</span></a>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        var Pageinator = $('#Pageinator');
        /*刷新重新加载*/
        function Refresh(pageindex) {

            todayReleaseTableBind(pageindex);
        }

        function todayReleaseTableBind(PageIndex) {

            $("#jump").css("display", "none");

            var editor = "<%=curUserCnName %>";
            var JsonString = "{'editor':'" + editor + "'}";
            var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 10, "PageIndex": PageIndex });
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
                        var optHtml = null;
                        if (data.list[i].isPicxw == true) {

                            optHtml = "<a href='releasearticle.aspx?opt=update&&articleID={0}' class='tablelink' target='_blank'> 更新</a> <a href='#' class='tablelink' onclick='del(this)'> 删除</a><a href='#' onclick='setHomePic({0},this)' class='tablelink' > 设为图片新闻轮播</a>";
                        }
                        else {
                            optHtml = "<a href='releasearticle.aspx?opt=update&&articleID={0}' class='tablelink' target='_blank'> 更新</a> <a href='#' class='tablelink' onclick='del(this)'> 删除</a>";
                        }

                        data.list[i].isIncludeAcc = data.list[i].isIncludeAcc == true ? "是" : "否";
                        var firstCtl = data.list[i].isPassed == "1" ? "<img src='../images/ysh.png' />" : "<img src='../images/dsh.png' />";
                        var optHtmlStringformat = optHtml.format(data.list[i].articleID);
                        var tr_html = "<tr {0}><td>{13}</td><td style='display:none'>{1}</td><td style='max-width:370px;'><a href='preview.aspx?opt=preview&&articleID={1}' id='btnEdit' target='_blank'>{2}</a></td><td style='max-width:280px;'>{3}</td><td style='max-width:100px;'>{4}</td><td style='max-width:100px;'>{5}</td><td>{6}</td><td style='display:none'>{7}</td><td style='display:none'>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{14}</td><td>{12}</td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(tr_classs, data.list[i].articleID, data.list[i].title, data.list[i].keywords, data.list[i].author, data.list[i].releaseDep, data.list[i].releaseTime, data.list[i].editor, data.list[i].editorDep, data.list[i].updateTime, data.list[i].isIncludePic, data.list[i].isIncludeAcc, optHtmlStringformat, firstCtl, data.list[i].hits);
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

        function setSpecialEdit() {

            layerOpen($('#layerSpecial'), '1024px', '740px');
        }

        function addSpecialChildren(pid) {

            var $layer = $("#layerOptDialog");
            $layer.attr("data-parent", pid);
            $layer.attr("data-id", "0");
            $("#layerName").val("");
            layerOpen($('#layerOptDialog'), '620px', '240px');

        }

        function delChildren(id, itemName) {


            layer.confirm('确认要删除当子栏目？', {
                btn: ['继续', '取消']
            }, function () {

                $.ajax({

                    type: "post",
                    url: "../ashx/special.ashx?method=delSpecialItem",
                    data: { id: id, itemName: itemName },
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

        function layerSave() {

            var $layer = $("#layerOptDialog");
            var parent = $layer.attr("data-parent");
            var id = $layer.attr("data-id");
            var title = $("#layerName").val();



            layer.confirm('当前操作[数据保存],是否继续?', function (index) {

                $.ajax({

                    type: "post",
                    url: "../ashx/special.ashx?method=specialConfigTitle",
                    data: {
                        id: id,
                        parent: parent,
                        title: title
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

            //if (pId == "0") {

            //    $("#treeTable1> tbody> tr").each(function (i) {

            //        if ($(this).attr("id") == Id) {

            //            var $span = $(this).find("td:eq(0)").find("span:eq(2)");
            //            console.log($span.text(title));
            //        }
            //    });
            //}

        }

        function updateNodeName(pid, title) {

            var $layer = $("#layerOptDialog");
            $layer.attr("data-parent", "0");
            $layer.attr("data-id", pid);
            $("#layerName").val(title);
            layerOpen($('#layerOptDialog'), '620px', '240px');

        }

        //图片新闻设置首页图片
        function setHomePic(articleID, obj) {

            $("#layerOpt").attr("data-id", articleID);
            var title = $(obj).parent().parent().find("td").eq(2).text();
            $("#layerOpt").attr("data-name", title);

            $.ajax({

                type: "post",
                url: "../ashx/article.ashx?method=xwConfigPic",
                data: {
                    articleID: articleID,
                },
                dataType: "json",
                traditional: true,

                success: function (data) {

                    var HTMLSection = "";
                    for (var i = 0; i < data.length; i++) {

                        var btnHTML = data[i].isDefault == true ? '' : '<input type="button" class="scbtn" value="图片确认" id="btnConfigOk">';
                        HTMLSection += '<li class="selected" onclick="btnConfigOkClick(this)"><span><img src="' + data[i].picUrl + '" style="width:398px;height:290px;"></span><h2><a href="#"></a></h2><p>' + btnHTML + '</p></li>';
                    }

                    $("#layerOpt .imglist").html(HTMLSection);
                }
            });

            layerOpen($('#layerOpt'), '920px', '740px');
        }

        function layerOpen(eObj, width, height) {
            layer.open({
                type: 1,
                title: false,
                skin: 'layui-layer-rim', //加上边框
                closeBtn: 1,
                area: [width, height],
                shadeClose: true,
                content: eObj
            });
        }

        function btnConfigOkClick(obj) {

            layer.confirm('当前操作[首页新闻图片设置],是否继续?', function (index) {

                var $img = $(obj).parent().parent().find("img");
                var defaultPicUrl = $img.attr("src");
                var articleID = $("#layerOpt").attr("data-id");
                var articleTitle = $("#layerOpt").attr("data-name");

                $.ajax({

                    type: "post",
                    url: "../ashx/article.ashx?method=articleXwNesDefaultPic",
                    data: {
                        defaultPicUrl: defaultPicUrl,
                        articleID: articleID,
                        articleTitle: articleTitle
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
    </script>
</body>
</html>
