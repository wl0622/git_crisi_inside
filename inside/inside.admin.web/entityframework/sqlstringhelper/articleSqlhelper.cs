using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.sqlstringhelper
{
    public class articleSqlhelper
    {
        public static string articleAuditSqlStriing(int articleID, string userCnName)
        {
            return string.Format("update Article set isPassed=1,EditorInCharge='{1}' where articleID={0}", articleID, userCnName);
        }

        public static string articleDeleteByID()
        {
            return string.Format("delete Article where articleID=@articleID");
        }

        public static string articleXwPicDefault()
        {
            return string.Format("update Article set defaultPicUrl=@defaultPicUrl where articleID=@articleID");
        }

        public static string syncOutSide()
        {
            return string.Format(@"if exists (select articleID from cjslkjw.dbo.Article where insideArticle=@articleID)
                                begin
                                  update a set a.subjectID=b.subjectId,
                                               a.title=b.title,
                                               a.keywords=b.keywords,
                                               a.content=b.content,
                                               a.author=b.author,
                                               a.releaseDep=b.releaseDep,
                                               a.releaseTime=b.releaseTime,
                                               a.editor=b.editor,
                                               a.editorDep=b.editorDep,
                                               a.EditorInCharge=b.EditorInCharge,
                                               a.updateTime=b.updateTime,
                                               a.isIncludePic=b.isIncludePic,
                                               a.defaultPicID=b.defaultPicID,
                                               a.defaultPicUrl=b.defaultPicUrl,
                                               a.isIncludeAcc=b.isIncludeAcc,
                                               hits=0,
                                               isDeleted=0,
                                               a.isHot=CASE WHEN b.isHot=NULL THEN 0 ELSE b.isHot END,
                                               a.isOnTop=b.isOnTop,
                                               a.isPicxw=b.isPicxw,
                                               a.isElite=b.isElite,
                                               a.isComment=b.isComment,
                                               a.isPassed=0,
                                               a.isPost=b.isPost,
                                               a.isChecked=b.isChecked,
                                               a.isexarticle=0,
                                               a.titlejiancheng=b.Titlejiancheng,
                                               a.linkurl=b.linkurl,
                                               a.titletoutiao=b.titletoutiao,
                                               a.isEnglishpic=CASE WHEN b.isEnglishpic=NULL THEN 0 ELSE b.isEnglishpic END,
                                               a.isEngpic=a.isEngpic,
                                               a.isTop=b.IsTop1,
                                               a.isspecialpic=b.isspecialpic,
                                               a.isspecialtop=b.isspecialtop,
                                               a.isspecialpicxw=b.isspecialPicxw,
                                               a.picID = b.picID,
                                               a.isNewsTop=b.IsNewsTop,
                                               a.reprint=b.reprint,
                                               a.topExpireTime=b.topExpireTime,
                                               a.repostSubjectID=b.repostSubjectID,
                                               a.insideArticle=b.articleID
                                               from cjslkjw.dbo.Article  as a inner join CRSRIWEB.dbo.Article as b on a.articleID=b.articleID and a.insideArticle=@articleID
                                end
                                else
                                begin
                                  insert into cjslkjw.dbo.Article(subjectID,title,keywords,content,author,releaseDep,releaseTime,editor,editorDep,EditorInCharge,updateTime,isIncludePic,
                                  defaultPicID,defaultPicUrl,isIncludeAcc,hits,isDeleted,isHot,isOnTop,isPicxw,isElite,isComment,isPassed,isPost,isChecked,isexarticle,titlejiancheng,
                                  linkurl,titletoutiao,isEnglishpic,isEngpic,isTop,isspecialpic,isspecialtop,isspecialpicxw,picID,isNewsTop,reprint,topExpireTime,repostSubjectID,insideArticle)
  
                                  select subjectID,title,keywords,content,author,releaseDep,releaseTime,editor,editorDep,EditorInCharge,updateTime,isIncludePic,
                                  defaultPicID,defaultPicUrl,isIncludeAcc,0,0,isHot,isOnTop,isPicxw,isElite,isComment,0,isPost,isChecked,0,titlejiancheng,
                                  linkurl,titletoutiao,isEnglishpic,isEngpic,isTop,isspecialpic,isspecialtop,isspecialpicxw,picID,isNewsTop,reprint,topExpireTime,repostSubjectID,articleID 
                                  from CRSRIWEB.dbo.Article where articleID=@articleID
                                end");
        }
    }
}