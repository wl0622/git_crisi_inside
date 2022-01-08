<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chengguo.aspx.cs" Inherits="inside.admin.web.aspx.chengguo" %>

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
    <script type="text/javascript" src="../js/jquery.form.js"></script>
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
</head>
<body>

    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">获奖成果管理</a></li>
            <li><a href="#">获奖成果编辑</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>获奖成果编辑</span></div>
        <div class="tools">
            <ul class="toolbar">
                <li class="click">
                    <a href="#" onclick="create();" target="rightFrame"><span>
                        <img src="/images/t01.png" /></span> 添加新成果</a></li>
            </ul>
        </div>

        <table class="tablelist" id="article-table-list">
            <thead>
                <tr>
                    <th style="width: 100px">操作</th>
                    <th style="display: none">系统编号</th>
                    <th style="width: 270px">获奖名称</th>
                    <th>项目名称</th>
                    <th style="width: 90px">获奖等级</th>
                    <th style="width: 50px">类别</th>
                    <th style="width: 200px">部门</th>
                    <th>参于部门</th>
                    <th style="width: 60px">年份</th>
                    <th style="display: none">获奖人员</th>
                    <th style="display: none">简介</th>
                    <th style="width: 60px">已审核</th>

                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <ul id='Pageinator'></ul>
    </div>
    <div id="layerOpt" style="display: none">
        <form id="chengguoform" runat="server">
            <div class="formtitle"><span>获奖成果编辑</span></div>
            <ul class="forminfo">
                <li>
                    <label style="width: 80px;">获奖名称</label>
                    <input id="lay_txt_huojiangname" type="text" class="dfinput" name="huojiangname" />
                </li>
                <li>
                    <label style="width: 80px;">项目名称</label>
                    <input id="lay_txt_xiangmuname" type="text" class="dfinput" name="xiangmuname" />

                </li>
                <li>
                    <label style="width: 80px;">获奖等级</label>
                    <input id="lay_txt_huojiangdengji" type="text" class="dfinput" name="huojiangdengji" />

                </li>
                <li>
                    <label style="width: 80px;">类别</label>
                    <input id="lay_txt_leibie" type="text" class="dfinput" name="leibie" /></li>
                <li>
                    <label style="width: 80px;">部门</label>
                    <input id="lay_txt_dept" type="text" class="dfinput" name="dept" />

                </li>
                <li>
                    <label style="width: 80px;">参于部门</label>
                    <input id="lay_txt_canyudept" type="text" class="dfinput" name="canyudept" />

                </li>
                <li>
                    <label style="width: 80px;">获取年份</label>
                    <input id="lay_txt_huojiangniandai" type="text" class="dfinput" name="huojiangniandai" />

                </li>
                <li>
                    <label style="width: 80px;">获奖人员</label>
                    <input id="lay_txt_huojiangrenyuan" type="text" class="dfinput" name="huojiangrenyuan" />

                </li>
                <li>
                    <label style="width: 80px;">成果简介</label>
                    <textarea id="lay_txt_chengguojj" name="chengguojj" style="overflow: hidden; width: 345px; height: 100px; line-height: normal; text-indent: inherit" class="dfinput"></textarea>
                </li>
                <li>
                    <input type="hidden" name="chengguoID" id="lay_txt_chengguoID" /></li>
            </ul>
            <div style="margin: 0px auto; text-align: center; width: 300px;">

                <a class="ibtn_max" style="margin: 0 5px;" onclick="save();"><span class="glyphicon glyphicon-floppy-saves">保存</span></a>
            </div>
        </form>
    </div>

    <script type="text/javascript">

        $(function () {

            chengguoBind(1);

        });

        var Pageinator = $('#Pageinator');
        /*刷新重新加载*/
        function Refresh(pageindex) {

            chengguoBind(pageindex);
        }

        function chengguoBind(PageIndex) {

            layer.load();

            var JsonString = "{'huojiangniandai ':''}";
            var dataSend = JSON.stringify({ "data": JsonString, "PageSize": 15, "PageIndex": PageIndex });
            $.ajax({

                type: "post",
                url: "../ashx/chengguo.ashx?method=chengguobind",
                contentType: "application/json; charset=utf-8",
                data: dataSend,
                dataType: "json",
                traditional: true,
                success: function (data) {

                    var row_index = 1;
                    var html = "";
                    $(data.list).each(function (i) {

                        data.list[i].isPassed = true ? "是" : "否";
                        var tr_html = "<tr {0}><td><a href='#' onclick='eidt({1});' class='tablelink' > 编辑</a> <a href='#' onclick='del({1},this);' class='tablelink' > 删除</a></td><td style='display:none'>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td style='display: none'>{9}</td><td style='display: none'>{10}</td><td>{11}</td></tr>";
                        var tr_classs = row_index % 2 == 0 ? "class='odd'" : "";
                        tr_html = tr_html.format(
                            tr_classs,
                            data.list[i].chengguoID,
                            data.list[i].huojiangname,
                            data.list[i].xiangmuname,
                            data.list[i].huojiangdengji,
                            data.list[i].leibie == null ? "" : data.list[i].leibie,
                            data.list[i].dept == null ? "" : data.list[i].dept,
                            data.list[i].canyudept == null ? "" : data.list[i].canyudept,
                            data.list[i].huojiangniandai,
                            data.list[i].huojiangrenyuan == null ? "" : data.list[i].huojiangrenyuan,
                            data.list[i].chengguojj == null ? "" : data.list[i].chengguojj, data.list[i].isPassed
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


            $('#chengguoform').ajaxSubmit({
                url: 'chengguo.aspx',
                type: 'post',
                dataType: 'text',
                beforeSubmit: function () {

                    if ($("#lay_txt_huojiangname").val() == "") { layer.msg('获奖名称不能为空!', { icon: 5, time: 1500 }); return false; }
                    if ($("#lay_txt_xiangmuname").val() == "") { layer.msg('项目名称不能为空!', { icon: 5, time: 1500 }); return false; }
                    if ($("#lay_txt_huojiangdengji").val() == "") { layer.msg('获奖等级不能为空!', { icon: 5, time: 1500 }); return false; }
                },
                success: function (responseText, statusText, xhr, $form) {

                    var jsonstr = responseText.match(/\{[^\}]+\}/)[0];
                    var reg = /{"status":"(ok|error)","message":([\s\S]*)}/;

                    if (reg.test(jsonstr)) {

                        var msg = $.parseJSON(jsonstr);

                        if (msg.status == "ok") {

                            layer.msg('保存成功!', { icon: 1, time: 1500 });
                            setTimeout(function () { window.location.reload(); }, 1500);
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

            $("#lay_txt_chengguoID").val(0);

            $(".forminfo input[type=text]").each(function () {
                $(this).val("");
            });

            layerOpen();
        }

        function del(chengguoID, obj) {

            layer.confirm('当前操作[删除数据],是否继续?', function (index) {

                layer.load();

                $.ajax({
                    url: '../ashx/chengguo.ashx?method=delByID',
                    type: 'post',
                    dataType: "json",
                    data: {
                        chengguoID: chengguoID,
                        xiangmuname: $(obj).parent().parent().children().eq(3).text()
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
                        $("#lay_txt_chengguoID").val("0");
                    }

                });
            },
             function () {

             });
        }


        function eidt(chengguoID) {

            $.ajax({

                type: "post",
                url: "../ashx/chengguo.ashx?method=chengguoInfoByID",
                dataType: "json",
                data: {
                    chengguoID: chengguoID
                },
                dataType: "json",
                traditional: true,
                success: function (data) {

                    $("#lay_txt_chengguoID").val(chengguoID);
                    $("#lay_txt_huojiangname").val(data.huojiangname);
                    $("#lay_txt_xiangmuname").val(data.xiangmuname);
                    $("#lay_txt_huojiangdengji").val(data.huojiangdengji);
                    $("#lay_txt_leibie").val(data.leibie);

                    $("#lay_txt_dept").val(data.dept);
                    $("#lay_txt_canyudept").val(data.canyudept);
                    $("#lay_txt_huojiangniandai").val(data.huojiangniandai);
                    $("#lay_txt_huojiangrenyuan").val(data.huojiangrenyuan);
                    $("#lay_txt_chengguojj").val(data.chengguojj);

                    layerOpen();
                }
            });

        }

        function layerOpen() {
            layer.open({
                type: 1,
                title: false,
                skin: 'layui-layer-rim', //加上边框
                closeBtn: 1,
                area: ['620px', '700px'],
                shadeClose: true,
                content: $('#layerOpt')
            });
        }



        function btnConfigOk(obj) {

            layer.confirm('当前操作[首页新闻图片设置],是否继续?', function (index) {

                var $img = $(obj).parent().parent().find("img");
                var defaultPicUrl = $img.attr("src");
                var articleID = $("#layerOpt").attr("data-id");

                $.ajax({

                    type: "post",
                    url: "../ashx/article.ashx?method=articleXwNesDefaultPic",
                    data: {
                        defaultPicUrl: defaultPicUrl,
                        articleID: articleID
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

</body>
</html>
