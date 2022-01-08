<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="homepiconfig.aspx.cs" Inherits="inside.admin.web.aspx.homepiconfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--page css--%>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />

    <%--page js--%>
    <script type="text/javascript" src="../js/lib/uploadfile/jquery-1.7.1.js"></script>

    <script type="text/javascript" src="../js/select-ui.min.js"></script>
    <script type="text/javascript" src="../js/helper.js"></script>
    <%-- end-----%>

    <%--lib-----layer-----%>
    <script type="text/javascript" src="/js/lib/layer/2.4/layer.js"></script>
    <%-- end-----%>
    <%--lib------Tabs--%>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <%-- end-----%>

    <%--lib------uploadfile--%>
    <script type="text/javascript" src="../js/lib/uploadfile/ajaxfileupload.js"></script>
    <%-- end-----%>


    <script type="text/javascript">

        function groupPic(groupName) {
            $.ajax({
                url: "/ashx/picconfig.ashx?method=getGroupPic",
                dataType: "json",
                data: { groupName: groupName },
                type: "post",
                success: function (data) {

                    if (data.status == "ok") {
                        var list = JSON.parse(data.message);
                        //在文本框显示图片配置的URL
                        if ($("#url").val() == "") {
                            if (list.length > 0) {
                                $("#url").val(list[0].url);
                            }
                        }
                        //幻灯片张数可以为动态的所以单独处理
                        if (groupName == "slide") {
                            WriteSlideHTML(list);
                        }
                        else {
                            //其它固定张数的图片
                            var $files = $("#" + groupName).find("input[type='file']");

                            //当前组的上传控件。根据上传控件查找对应img
                            $files.each(function () {

                                //获取的组图片遍历
                                for (var i = 0; i < list.length; i++) {

                                    //对应控件的图片赋值
                                    if ($(this).attr("id") == list[i].item) {

                                        //查找图片控件
                                        var $img = $(this).parent().parent().find("img");
                                        $img.attr("src", list[i].url + list[i].picName);
                                        //查找文本控件
                                        var $linkBox = $img.parent().prev().find("input[type='text']");
                                        $linkBox.val(list[i].linkurl);
                                    }
                                }
                            });
                        }
                    }
                    else {

                        layer.msg(data.message, { icon: 2, time: 3000 });
                    }
                }
            })
        }

        //幻灯片根据张数动态的输出
        function WriteSlideHTML(list) {

            var HTML = "";

            var HTMLOutput = '<div style="margin-bottom: 20px;float:left;margin-left:15px;border:1px solid #d3dbde;">';
            HTMLOutput += '  <ul style="margin:5px">';
            HTMLOutput += '        <li style="display: -webkit-flex; display: flex; margin-bottom: 5px;"><a class="ibtn_max">配置链接</a></li>';
            HTMLOutput += '        <li>';
            HTMLOutput += '           <img src="{1}" class="home_config_img_container" style="width: 398px; height: 290px;" /></li>';
            HTMLOutput += '            <li style="display: -webkit-flex; display: flex;">';
            HTMLOutput += '             <input type="file" id="{0}" name="{0}"  /><a class="ibtn">上传更新</a></li>';
            HTMLOutput += '   </ul>'
            HTMLOutput += ' </div>';

            for (var i = 0; i < list.length; i++) {

                HTML = HTML + HTMLOutput.format(list[i].item, list[i].url + list[i].picName);
            };


            $("#slideoutput").html(HTML);

            $("#slideoutput .ibtn").bind("click", upload);

        }

        $(function (e) {

            $("#usual1 ul").idTabs(function (id) {
                switch (id) {
                    case "#tonglan1left":
                        groupPic("tonglan1left");
                        break;
                    case "#tonglan1right":
                        groupPic("tonglan1right");
                        break;
                        //case "#slide":
                        //    groupPic("slide");
                        //    break;
                    case "#tonglan2":
                        groupPic("tonglan2");
                        break;
                    case "#tonglan3":
                        groupPic("tonglan3");
                        break;
                    case "#002004":
                        groupPic("002004");
                        break;
                    case "#002005":
                        groupPic("002005");
                        break;
                    case "#002006":
                        groupPic("002006");
                        break;
                } return true;
            });

            var $btn = $("#usual1 .ibtn");

            $btn.each(function () {

                $(this).bind("click", upload);
            });

            var $btnDel = $("#usual1 .ibtnDel");

            $btnDel.each(function () {

                $(this).bind("click", del);
            });
        });


        function del() {
            var $img = $(this).parent().parent().find("img");
            var url = $img.attr("src");
            if (url != "../images/nopic.png") {

                var $elementName = $(this).prev().prev();
                var eName = $elementName.attr("name");
                //layer.msg(eName, { icon: 5, time: 2000 });

                layer.confirm('确认移除图片?', function (index) {

                    $.ajax({
                        url: "/ashx/picconfig.ashx?method=delUrlConfig",
                        dataType: "json",
                        data: {
                            item: eName
                        },
                        type: "post",
                        success: function (data) {

                            if (data.status == "ok") {
                                layer.msg('移除图片成功!', { icon: 1, time: 1500 });
                                $img.attr("src", "../ueditor/1.4.3/net/upload/homePicConfig/nopic.png");
                            }
                        }
                    });
                },
                function () {

                })
            }
            else {
                layer.msg("当前无移除的图像对象", { icon: 5, time: 2000 });
                return false;
            }


        }


        function upload() {

            var elementName = $(this).prev().attr("name");
            if ($("#" + elementName).val() != "") {
                layer.load();
                $.ajaxFileUpload(
                   {
                       url: 'upload.aspx', //用于文件上传的服务器端请求地址
                       secureuri: false, //一般设置为false
                       fileElementId: elementName, //文件上传空间的id属性  <input type="file" id="file" name="file" />
                       dataType: 'json', //返回值类型 一般设置为json
                       success: function (data, status)  //服务器成功响应处理函数
                       {
                           layer.closeAll('loading');
                           var norights = false;
                           if (typeof (data.error) != 'undefined') {
                               if (data.error != '') {
                                   norights = true;
                                   layer.msg(data.error, { icon: 2, time: 1500 });
                               }
                           }

                           if (norights == false) {
                               var $img = $("#" + elementName).parent().parent().find("img");
                               $img.attr("src", $("#url").val() + data.imgurl)
                               layer.msg("上传成功", { icon: 1, time: 2000 });
                           }
                       },
                       error: function (data, status, e)//服务器响应失败处理函数
                       {
                           layer.closeAll('loading');
                           layer.msg(e, { icon: 2, time: 2000 });
                       }
                   }
               )
                return false;
            }
            else {
                layer.msg("未选择上传文件", { icon: 5, time: 2000 });
                return false;
            }

        }


        //点击复选框后文本框的切换
        function setDisabled(obj) {

            var $parent = $(obj).parent().parent();
            var $input = $parent.find("input[type=text]");

            $input.each(function () {

                if ($(this).attr("class") == "dfinput") {
                    $(this).attr("class", "dfinput_un");
                    $(this).attr("disabled", "disabled");
                }
                else if ($(this).attr("class") == "dfinput_un") {
                    $(this).attr("class", "dfinput");
                    $(this).removeAttr("disabled");
                }
            });
        }

        function updateURL() {

            layer.confirm('确认修改图片地址配置?', function (index) {

                var url = $("#url").val();

                $.ajax({
                    url: "/ashx/picconfig.ashx?method=updateUrlConfig",
                    dataType: "json",
                    data: { url: url },
                    type: "post",
                    success: function (data) {

                        if (data.status == "ok") {
                            layer.msg('新配置保存成功!', { icon: 1, time: 1500 });
                            $("#ckConfig").trigger("click");
                        }
                    }
                });
            },
          function () {

          })
        }

        function configlink(obj) {
            var item = $(obj).parent().parent().find("li:eq(2)").find("input").attr("name");
            var link = $(obj).prev().val();
            if (link != "") {
                layer.confirm('确认修改链接地址?', function (index) {


                    $.ajax({
                        url: "/ashx/picconfig.ashx?method=updateLinkConfig",
                        dataType: "json",
                        data: {
                            item: item,
                            link: link
                        },
                        type: "post",
                        success: function (data) {

                            if (data.status == "ok") {
                                layer.msg('新配置保存成功!', { icon: 1, time: 1500 });
                                $("#ckConfig").trigger("click");
                            }
                        }
                    });
                },
               function () {

               })
            }
            else {
                layer.msg('链接地址不能为空!', { icon: 1, time: 1500 });
            }
        }

    </script>
</head>
<body>
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">网站配置管理</a></li>
            <li>
                <img src="/images/main/arrow.gif" /></li>
            <li><a href="#">通栏与栏目图片</a></li>
        </ul>
    </div>
    <div class="formbody">
        <div class="formtitle"><span>图片配置</span></div>
        <ul class="forminfo">
            <li>
                <label style="width: 25px;">
                    <input type="checkbox" onclick="setDisabled($(this))" id="ckConfig" style="width: 20px; height: 20px; margin-top: 6px;" /></label>
                <label>设置图片地址</label>

                <input id="url" type="text" disabled="disabled" class="dfinput_un" style="width: 560px; margin-left: 5px;" /><i></i><input type="button" class="standardbtn" value="配置更新" onclick="updateURL();" />
            </li>
        </ul>
        <div id="usual1" class="usual">
            <div class="itab">
                <ul>
                    <li><a id="#tonglan1left" href="#tonglan1left" class="selected">通栏一(左)</a></li>
                    <li><a id="#tonglan1right" href="#tonglan1right">通栏一(右)</a></li>
                    <%--<li><a id="#slide" href="#slide" style="display: none">幻灯片</a></li>--%>
                    <li><a id="#tonglan2" href="#tonglan2">通栏二</a></li>
                    <li><a id="#tonglan3" href="#tonglan3">通栏三</a></li>
                    <li><a id="#002004" href="#002004"><%=list.Find(a=>a.subjectID=="002004").subjectName %></a></li>
                    <li><a id="#002005" href="#002005"><%=list.Find(a=>a.subjectID=="002005").subjectName %></a></li>
                    <li><a id="#002006" href="#002006"><%=list.Find(a=>a.subjectID=="002006").subjectName %></a></li>
                </ul>
            </div>
            <div id="tonglan1left" class="tabson" style="width: 920px">
                <div class="explain"><b>图片尺存:(宽) 620px * (高) 110px </b></div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">
                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this);">保存链接</a>

                        </li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">1</i>
                        </li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1left_1" name="tonglan1left_1" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">
                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this)">保存链接</a></li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">2</i>
                        </li>

                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1left_2" name="tonglan1left_2" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">
                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this)">保存链接</a></li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">3</i>
                        </li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1left_3" name="tonglan1left_3" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">
                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this)">保存链接</a></li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">4</i>
                        </li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1left_4" name="tonglan1left_4" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="tonglan1right" class="tabson" style="width: 920px">
                <div class="explain"><b>图片尺存:(宽) 620px * (高) 110px </b></div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">

                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this);">保存链接</a>

                        </li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">1</i>
                        </li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1right_1" name="tonglan1right_1" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">
                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this)">保存链接</a></li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">2</i>
                        </li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1right_2" name="tonglan1right_2" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>

                </div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">
                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this)">保存链接</a></li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">3</i>
                        </li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1right_3" name="tonglan1right_3" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
                <div style="margin-bottom: 20px; border: 1px solid #d3dbde;">
                    <ul style="margin: 5px;">
                        <li style="display: -webkit-flex; margin-bottom: 5px;">
                            <input type="text" class="dfinput" style="margin-right: 5px;" />
                            <a class="ibtn_max" onclick="configlink(this)">保存链接</a></li>
                        <li>
                            <img class="home_config_img_container" style="width: 620px; height: 110px;" />
                            <i style="float: right; font-size: 50px; margin-right: 35px; margin-top: 25px; color: #ced9df">4</i>
                        </li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan1right_4" name="tonglan1right_4" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
            </div>

            <%--    
            <div id="slide" class="tabson" style="width: 1300px">
                <div class="explain"><b>图片尺存:(宽) 398px * (高) 290px </b></div>
                <div id="slideoutput"></div>
            </div>
            --%>
            <div id="tonglan2" class="tabson" style="width: 920px">
                <div class="explain"><b>图片尺存:(宽) 1250px * (高) 110px </b></div>
                <div>
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 1250px; height: 110px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan21" name="tonglan21" style="width: 540px;" /><a class="ibtn">上传更新</a> <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>

                </div>
            </div>
            <div id="tonglan3" class="tabson" style="width: 920px">
                <div class="explain"><b>图片尺存:(宽) 1250px * (高) 110px </b></div>
                <div>
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 1250px; height: 110px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="tonglan31" name="tonglan31" style="width: 540px;" /><a class="ibtn">上传更新</a>  <a class="ibtnDel">移除图片</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div id="002004" class="tabson" style="width: 920px">
                <div class="explain"><b>图片尺存:(宽) 395px * (高) 117px </b></div>
                <div style="margin-bottom: 20px">
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 395px; height: 117px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="002004_1" name="002004_1" /><a class="ibtn">上传更新</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 395px; height: 117px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="002004_2" name="002004_2" /><a class="ibtn">上传更新</a></li>
                    </ul>
                </div>
            </div>
            <div id="002005" class="tabson" style="width: 920px">
                <div class="explain"><b>图片尺存:(宽) 395px * (高) 117px </b></div>
                <div style="margin-bottom: 20px">
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 395px; height: 117px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="002005_1" name="002005_1" /><a class="ibtn">上传更新</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 395px; height: 117px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="002005_2" name="002005_2" /><a class="ibtn">上传更新</a></li>
                    </ul>
                </div>
            </div>
            <div id="002006" class="tabson" style="width: 920px">
                <div class="explain"><b>图片尺存:(宽) 395px * (高) 117px </b></div>
                <div style="margin-bottom: 20px">
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 395px; height: 117px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="002006_1" name="002006_1" /><a class="ibtn">上传更新</a></li>
                    </ul>
                </div>
                <div>
                    <ul>
                        <li>
                            <img class="home_config_img_container" style="width: 395px; height: 117px;" /></li>
                        <li style="display: -webkit-flex;">
                            <input type="file" id="002006_2" name="002006_2" /><a class="ibtn">上传更新</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
