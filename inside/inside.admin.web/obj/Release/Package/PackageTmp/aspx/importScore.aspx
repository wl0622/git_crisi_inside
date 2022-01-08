<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="importScore.aspx.cs" Inherits="inside.admin.web.aspx.importScore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--page css--%>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />

    <%--page js--%>
    <script type="text/javascript" src="../js/lib/uploadfile/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <script type="text/javascript">

        $(function () {

            $("#btnImport").click(function () {

                var formData = new FormData();
                var filename = $("#file")[0].files[0];
                if (filename == null || filename == "") {

                    return false;
                }


                if (filename != null) {

                    formData.append('file', $("#file")[0].files[0]);
                    $.ajax({
                        type: "post",
                        url: "/ashx/excelUpload.ashx?method=score",
                        async: true,
                        contentType: false,
                        processData: false,
                        data: formData,
                        dataType: 'json', //返回类型
                        beforeSend: function () {


                        },
                        complete: function () {

                        },
                        success: function (data) {

                            if (data.status == "error") {

                            }
                            else if (data.status == "ok") {

                                let innerHtml = "";

                                let $table = $.parseJSON(data.message);

                                for (let i = 0; i < $table.length; i++) {

                                    let $tr = $table[i];

                                    innerHtml += "<tr><td>" + $tr.ksbh + "</td><td>" + $tr.xm + "</td><td>" + $tr.zzll + "</td><td>" + $tr.wgy + "</td><td>" + $tr.ywk1 + "</td><td>" + $tr.ywk2 + "</td><td>" + $tr.zf + "</td></tr>";
                                }
                                layer.msg('已上传成功!', { icon: 1, time: 1500 });
                                $("#score-table-list tbody").html(innerHtml);

                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown, data) {

                        }
                    });
                }

                return false;
            });
        })


    </script>

</head>
<body>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">网站配置管理</a></li>
            <li><a href="#">考分数据导入</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>考分数据导入</span></div>
        <form id="fUpload">
            <div>
                <input type="file" name="file" id="file" style="border: 1px solid #ccc" />
                <img src="/images/f05.png" /><a href="/scroeExcel/scoreTemplate.xlsx" style="color: blue; margin-right: 10px; margin-left: 5px;">上传文件模板下载</a>
                <input type="button" class="scbtn" value="确定上传" id="btnImport" />
            </div>
        </form>
        <table class="tablelist" id="score-table-list" style="margin-top: 10px; width: 820px;">
            <thead>
                <tr>
                    <th style="width: 220px">考试编号</th>
                    <th style="width: 100px">考生姓名</th>
                    <th style="width: 100px">政治理论</th>
                    <th style="width: 100px">外国语</th>
                    <th style="width: 100px">业条课1</th>
                    <th style="width: 100px">业条课2</th>
                    <th style="width: 100px">总分数</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
</body>
</html>
