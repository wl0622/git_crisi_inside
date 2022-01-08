function clickIsBlack(obj) {

    let isBlack = obj.is(':checked');

    $.ajax({
        url: "/ashx/sitecfg.ashx?method=setIsBlackCfg",
        dataType: "json",
        data: {
            isBlack: isBlack,
        },
        type: "post",
        success: function (data) {

            if (data.status == "error") {

            }
        }
    });
}

function clickTopBold(obj) {
    let isBold = obj.is(':checked');
    $.ajax({
        url: "/ashx/sitecfg.ashx?method=setIsTopBoldCfg",
        dataType: "json",
        data: {
            isTopBold: isBold,
        },
        type: "post",
        success: function (data) {

            if (data.status == "error") {

            }
        }
    });
}

function clickOnTopBold(obj) {
    let isBold = obj.is(':checked');
    $.ajax({
        url: "/ashx/sitecfg.ashx?method=setIsOnTopBoldCfg",
        dataType: "json",
        data: {
            isOnTopBold: isBold,
        },
        type: "post",
        success: function (data) {

            if (data.status == "error") {

            }
        }
    });
}

function clickIsNewYearBg(obj) {

    let isNewYearBg = obj.is(':checked');

    $.ajax({
        url: "/ashx/sitecfg.ashx?method=setIsNewYearBgCfg",
        dataType: "json",
        data: {
            isNewYearBg: isNewYearBg,
        },
        type: "post",
        success: function (data) {

            if (data.status == "error") {

            }
        }
    });
}

function upload() {


    let elInsert = $(this).parents("tr").next().find("div");
    var elementName = $(this).parent().parent().find("li:eq(0)").find("input[type='file']").attr("name");

    if ($("#" + elementName).val() != "") {
        layer.load();
        $.ajaxFileUpload(
           {
               //用于文件上传的服务器端请求地址
               url: 'upload.aspx',
               //一般设置为false
               secureuri: false,
               //文件上传空间的id属性  <input type="file" id="file" name="file" />
               fileElementId: elementName,
               //返回值类型 一般设置为json
               dataType: 'json',
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

                       //setTimeout(function () { window.location.reload(true); }, 2500);
                       layer.msg("上传成功", { icon: 1, time: 2000 });

                       let elCss = elInsert.attr("class");
                       let linkInputWidth = elCss == ("shortPicList" || "longPicList") ? (elCss == "shortPicList" ? "385" : "875") : "160";

                       let html = "<ul><li><a href='javascript:void()'><img src='/ueditor/1.4.3/net/upload/homePicConfig/" + data.imgurl + "'></a><span class='ibtnDel_icon_box' title='删除' data-id='" + data.id + "' onclick='del($(this))'> <span class='ibtnDel_icon'></span></span></li>";
                       html = html + "<li><input data-id='" + data.id + "' type='text' style='width:" + linkInputWidth + "px' value='' class='upload_link_text'><input type='button' class='scbtn' value='保存链接' style='height: 30px' onclick='configlink($(this))'></li></ul>";

                       if (elInsert.find("ul").length > 0) {

                           elInsert.append(html)
                       }
                       else {
                           elInsert.html(html);
                       }
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

function setTopColor(obj) {
    let cString = $(obj).attr("data-c");
    $.ajax({
        url: "/ashx/sitecfg.ashx?method=setTopColor",
        dataType: "json",
        data: {
            item: cString,
        },
        type: "post",
        success: function (data) {

            if (data.status == "error") {

                layer.msg(data.msg);
            }
        }
    });
}


function setCfgColor(obj) {

    let cString = $(obj).attr("data-c");

    $.ajax({
        url: "/ashx/sitecfg.ashx?method=setTitleColor",
        dataType: "json",
        data: {
            item: cString,
        },
        type: "post",
        success: function (data) {

            if (data.status == "error") {

                layer.msg(data.msg);
            }
        }
    });
}

function configlink(obj) {

    let $tlink = $(obj).prev();
    let item = $tlink.attr("data-id");
    let link = $tlink.val();


    if (link != "") {

        layer.confirm('确认修改链接地址?', function (index) {

            $.ajax({
                url: "/ashx/sitecfg.ashx?method=updateLinkConfig",
                dataType: "json",
                data: {
                    item: item,
                    link: link
                },
                type: "post",
                success: function (data) {

                    if (data.status == "ok") {
                        layer.msg('图片URL链接设置成功!', { icon: 1, time: 1500 });

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

function del(obj) {

    layer.confirm('确认移除图片?', function (index) {

        $.ajax({
            url: "/ashx/sitecfg.ashx?method=delCfgPic",
            dataType: "json",
            data: {
                item: $(obj).attr("data-id")
            },
            type: "post",
            success: function (data) {

                if (data.status == "ok") {
                    //setTimeout(function () { window.location.reload(true); }, 2000);
                    layer.msg('移除图片成功!', { icon: 1, time: 1500 });

                    let elUl = $(obj).parents("ul");
                    $(elUl).remove();

                }
            }
        });
    },
    function () {

    });
}