<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="inside.admin.web.aspx.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

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

    <script type="text/javascript" src="../js/page/index.js"></script>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/select.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">


        $(function () {

            $("#tabUpload .ibtn").bind("click", upload);

            $("input[name='cfgColor']").bind("click", function () {

                setCfgColor($(this));
            });

            $("input[name='cfgTopColor']").bind("click", function () {

                setTopColor($(this));
            });

            $("#tabUpload ul").idTabs();


        });

    </script>

</head>
<body class="pageBody">
    <form id="form1">
        <div class="place">
            <span>位置：</span>
            <ul class="placeul">
                <li><a href="#">系统管理</a></li>
                <li>
                    <img src="/images/main/arrow.gif" /></li>
                <li><a href="#">网站配置</a></li>
            </ul>
        </div>
        <table class="tablelist" id="cfg-table-list" style="width: 1280px; margin: auto; margin-bottom: 20px;">
            <thead>
                <tr>
                    <th style="width: 220px;">
                        <label class="cfglabel">配置项</label>
                    </th>
                    <th>设置选项</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>显示黑白网页</td>
                    <td>
                        <%
                            bool? isBlack = cfgList.FindAll(a => a.cfgName == "isBlack").First().cfgVal;

                            if (isBlack == true)
                            { Response.Write("<input type='checkbox'  checked='checked' onclick='javascript:clickIsBlack($(this)) '/> "); }
                            else { Response.Write("<input type='checkbox' onclick='javascript:clickIsBlack($(this))' /> "); }
                        %></td>
                </tr>
                <tr>
                    <td>显示春节背景</td>
                    <td>
                        <%
                            bool? isNewYearBg = cfgList.FindAll(a => a.cfgName == "isNewYearBg").First().cfgVal;

                            if (isNewYearBg == true)
                            { Response.Write("<input type='checkbox'  checked='checked' onclick='javascript:clickIsNewYearBg($(this))' /> "); }
                            else { Response.Write("<input type='checkbox' onclick='javascript:clickIsNewYearBg($(this))' /> "); }
                        %>
                    </td>
                </tr>
                <tr>
                    <td>新闻头条颜色</td>
                    <td>
                        <%
                            string bColor = "#307ee3";
                            string rColor = "#f40b21";
                            string dColor = "#333333";
                            string selectColor = cfgBaseList.FindAll(a => a.item.Equals("toutiaoColor")).First().itemVal.Trim();
                            string selectBlue = selectColor == bColor ? "checked='checked'" : "";
                            string selectRed = selectColor == rColor ? "checked='checked'" : "";
                            string selectDefault = selectColor == dColor ? "checked='checked'" : "";
                            Response.Write(string.Format("<span style='color:{1}'>蓝色</span> <input type='radio' name='cfgColor' data-c='{1}' {0}/>", selectBlue, bColor));
                            Response.Write(string.Format(" <span style='color:{1}' class='ml-10'>红色</span> <input type='radio' name='cfgColor' data-c='{1}' {0}/>", selectRed, rColor));
                            Response.Write(string.Format(" <span style='color:{1}' class='ml-10'>黑色</span> <input type='radio' name='cfgColor' data-c='{1}' {0}/>", selectDefault, dColor));

                            bool? isOnTopBold = cfgList.FindAll(a => a.cfgName == "isOnTopBold").First().cfgVal;

                            if (isNewYearBg == true)
                            { Response.Write("<b class='ml_20'>粗字体</b> <input type='checkbox'  checked='checked' onclick='javascript:clickOnTopBold($(this))' /> "); }
                            else { Response.Write("<b class='ml_20'>粗字体</b> <input type='checkbox'  onclick='javascript:clickOnTopBold($(this))' />"); }
                        %>
                    </td>
                </tr>
                <tr>
                    <td>栏目置顶颜色</td>
                    <td>
                        <%

                            string lmselectColor = cfgBaseList.FindAll(a => a.item.Equals("topColor")).First().itemVal.Trim();
                            string lmselectBlue = lmselectColor == bColor ? "checked='checked'" : "";
                            string lmselectRed = lmselectColor == rColor ? "checked='checked'" : "";
                            string lmselectDefault = lmselectColor == dColor ? "checked='checked'" : "";
                            Response.Write(string.Format("<span style='color:{1}'>蓝色</span> <input type='radio' name='cfgTopColor' data-c='{1}' {0}/>", selectBlue, bColor));
                            Response.Write(string.Format(" <span style='color:{1}' class='ml-10'>红色</span> <input type='radio' name='cfgTopColor' data-c='{1}' {0}/>", selectRed, rColor));
                            Response.Write(string.Format(" <span style='color:{1}' class='ml-10'>黑色</span> <input type='radio' name='cfgTopColor' data-c='{1}' {0}/>", selectDefault, dColor));

                            bool? isTopBold = cfgList.FindAll(a => a.cfgName == "isTopBold").First().cfgVal;

                            if (isNewYearBg == true)
                            { Response.Write("<b  class='ml_20' >粗字体</b> <input type='checkbox' checked='checked' onclick='javascript:clickTopBold($(this))' /> "); }
                            else { Response.Write("<b class='ml_20'>粗字体</b> <input type='checkbox'  onclick='javascript:clickTopBold($(this))' />"); }
                        %>
                    </td>
                </tr>
            </tbody>
        </table>

        <div id="tabUpload" class="usual" style="width: 1270px; margin: auto; padding: 5px; background-color: #fff">
            <div style="padding: 0px;">
                <div class="itab">
                    <ul>
                        <li><a id="#tab1" href="#tab1" class="selected">首页图片通栏1</a></li>
                        <li><a id="#tab3" href="#tab3">首页图片通栏2</a></li>
                        <li><a id="#tab2" href="#tab2">二级页图片</a></li>
                        <li><a id="#tab4" href="#tab4">右侧悬浮</a></li>
                        <li><a id="#tab5" href="#tab5">栏目图片</a></li>
                    </ul>
                </div>
                <div id="tab1" class="tabson">
                    <table class="tablelist" style="width: inherit; margin: auto">
                        <tbody>
                            <tr>
                                <td rowspan="2" style="width: 220px;">970图片上传(970*110)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgFlashlong1" name="cfgFlashlong1" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> longPic = cfgPicList.FindAll(a => a.isShortPic == false && a.position == 1);
                                List<crsri.cn.Model.t_web_siteCfgPic_model> shortPic = cfgPicList.FindAll(a => a.isShortPic == true && a.position == 1);
                                string picUrl = "/ueditor/1.4.3/net/upload/homePicConfig/";
                            %>
                            <tr>
                                <td>
                                    <div class="longPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in longPic)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width: 875px;' value='{2}'  class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>
                                    ", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2" style="border-bottom: 1px solid #444">480图片上传(480*110)</td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgFlashshort1" name="cfgFlashshort1" style="width: 340px;" />
                                            </li>
                                            <li><a class="ibtn">确认上传</a></li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="shortPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in shortPic)
                                            {
                                                Response.Write(string.Format(@"
                                      <ul>
                                        <li>
                                            <a href='javascript:void()'>
                                                <img src='{0}' /></a>

                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                <span class='ibtnDel_icon'></span>
                                            </span>

                                        </li>
                                        <li>
                                            <input  data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text'/>
                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px' onclick='configlink($(this))' />
                                        </li>
                                      </ul>", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div id="tab3" class="tabson">
                    <table class="tablelist" style="width: inherit; margin: auto">
                        <tbody>
                            <tr>
                                <td rowspan="2" style="width: 220px;">970图片上传(970*110)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgFlashlongP2" name="cfgFlashlongP2" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> longPicP2 = cfgPicList.FindAll(a => a.isShortPic == false && a.position == 2);
                                List<crsri.cn.Model.t_web_siteCfgPic_model> shortPicP2 = cfgPicList.FindAll(a => a.isShortPic == true && a.position == 2);
                            %>
                            <tr>
                                <td>
                                    <div class="longPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in longPicP2)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width: 875px;' value='{2}' class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>
                                    ", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2" style="border-bottom: 1px solid #444">480图片上传(480*110)</td>
                                <td style="background-color: #e3efff">
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgFlashshortP2" name="cfgFlashshortP2" style="width: 340px;" />
                                            </li>
                                            <li><a class="ibtn">确认上传</a></li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="shortPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in shortPicP2)
                                            {
                                                Response.Write(string.Format(@"
                                      <ul>
                                        <li>
                                            <a href='javascript:void()'>
                                                <img src='{0}' /></a>

                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                <span class='ibtnDel_icon'></span>
                                            </span>

                                        </li>
                                        <li>
                                            <input  data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text'/>
                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px' onclick='configlink($(this))' />
                                        </li>
                                      </ul>
                                    ", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div id="tab2" class="tabson">
                    <table class="tablelist" style="width: inherit; margin: auto">
                        <tbody>
                            <tr>
                                <td rowspan="2" style="width: 220px;">970图片上传(970*110)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgCntPagePicL3" name="cfgCntPagePicL3" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> longCfgCntPage = cfgPicList.FindAll(a => a.isShortPic == false && a.position == 3);
                                List<crsri.cn.Model.t_web_siteCfgPic_model> shortCfgCntPage = cfgPicList.FindAll(a => a.isShortPic == true && a.position == 3);
                            %>
                            <tr>
                                <td>
                                    <div class="longPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in longCfgCntPage)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width: 875px;' value='{2}' class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>
                                    ", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="2" style="border-bottom: 1px solid #444">480图片上传(480*110)</td>
                                <td style="background-color: #e3efff">
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgCntPagePicS3" name="cfgCntPagePicS3" style="width: 340px;" />
                                            </li>
                                            <li><a class="ibtn">确认上传</a></li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="shortPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in shortCfgCntPage)
                                            {
                                                Response.Write(string.Format(@"
                                      <ul>
                                        <li>
                                            <a href='javascript:void()'>
                                                <img src='{0}' /></a>

                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                <span class='ibtnDel_icon'></span>
                                            </span>

                                        </li>
                                        <li>
                                            <input  data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text'/>
                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px' onclick='configlink($(this))' />
                                        </li>
                                      </ul>
                                    ", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div id="tab4" class="tabson">
                    <table class="tablelist" style="width: inherit; margin: auto">
                        <tbody>
                            <tr>
                                <td rowspan="2" style="width: 220px; border-bottom: 1px solid #444">右侧悬浮(90*90)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgRightFloat4" name="cfgRightFloat4" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> cfgRightFloat = cfgPicList.FindAll(a => a.position == 4);
                            %>
                            <tr>
                                <td>
                                    <div class="shortPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in cfgRightFloat)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div id="tab5" class="tabson">
                    <table class="tablelist" style="width: inherit; margin: auto">
                        <tbody>
                            <tr>
                                <td rowspan="2" style="width: 220px; border-bottom: 1px solid #444">水利与科技(160*110)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgslkj5" name="cfgslkj5" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> lmSlkj = cfgPicList.FindAll(a => a.position == 5);
                            %>
                            <tr>
                                <td>
                                    <div class="lmPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in lmSlkj)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td rowspan="2" style="width: 220px; border-bottom: 1px solid #444">科技交流(160*110)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgkjjl6" name="cfgkjjl6" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> lmKjjl = cfgPicList.FindAll(a => a.position == 6);
                            %>
                            <tr>
                                <td>
                                    <div class="lmPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in lmKjjl)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>


                            <tr>
                                <td rowspan="2" style="width: 220px; border-bottom: 1px solid #444">项目动态(160*110)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgxmdt7" name="cfgxmdt7" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> lmxmdt = cfgPicList.FindAll(a => a.position == 7);
                            %>
                            <tr>
                                <td>
                                    <div class="lmPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in lmxmdt)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>

                                              <tr>
                                <td rowspan="2" style="width: 220px; border-bottom: 1px solid #444">单位文化(160*110)<br />
                                </td>
                                <td>
                                    <div class="cfgUploadBox">
                                        <ul>
                                            <li>
                                                <input type="file" id="cfgdwwh8" name="cfgdwwh8" style="width: 340px;" /></li>
                                            <li>
                                                <a class="ibtn">确认上传</a>
                                            </li>
                                        </ul>
                                    </div>

                                </td>
                            </tr>
                            <% 
                                List<crsri.cn.Model.t_web_siteCfgPic_model> lmdwwh = cfgPicList.FindAll(a => a.position == 8);
                            %>
                            <tr>
                                <td>
                                    <div class="lmPicList">
                                        <% 
                                            foreach (crsri.cn.Model.t_web_siteCfgPic_model m in lmdwwh)
                                            {
                                                Response.Write(string.Format(@"
                                                     <ul>
                                                        <li>
                                                            <a href='javascript:void()'>
                                                                <img src='{0}' /></a>

                                                            <span class='ibtnDel_icon_box' title='删除' data-id='{1}' onclick='del($(this))'>
                                                                <span class='ibtnDel_icon'></span>
                                                            </span>

                                                        </li>
                                                        <li>
                                                            <input data-id={1} type='text' style='width:385px;' value='{2}' class='upload_link_text' />
                                                            <input type='button' class='scbtn' value='保存链接' style='height: 30px'  onclick='configlink($(this))' />
                                                        </li>
                                                     </ul>", picUrl + m.picName, m.id, m.linkUrl));
                                            }
                                        %>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

