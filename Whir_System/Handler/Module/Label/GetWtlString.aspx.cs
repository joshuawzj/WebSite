using System;
using Whir.Framework;
using Whir.Repository;
using Whir.ezEIP.Web;
using Whir.Domain;
using Whir.Service;
using System.Data;



public partial class Whir_System_Handler_Module_Label_GetWtlString : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
         var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action); 
    }

    public HandlerResult GetWtl()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var labelName = RequestUtil.Instance.GetFormString("labelName");
        var siteId = RequestUtil.Instance.GetFormString("Site").ToInt(0);
        var siteIColumnId = RequestUtil.Instance.GetFormString("Column").ToInt(0);
        var strLabel = string.Empty;

        string field;
        string length;
        string sql;
        switch (labelName)
        {
            case "seo":
                if (siteId != 0)
                {
                    var path = DbHelper.CurrentDb.ExecuteScalar<string>(
                        "SELECT path FROM whir_sit_siteinfo WHERE siteid=@0", siteId);
                    strLabel += " SitePath=\"" + path + "\"";
                }
                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                strLabel =
                    "<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>".FormatWith(labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "category":
                var parentId = RequestUtil.Instance.GetFormString("parentId");
                sql = RequestUtil.Instance.GetFormString("SQL");
                var isParentColumnId = RequestUtil.Instance.GetFormString("IsParentColumnId");
                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                if (parentId.Trim() != string.Empty)
                {
                    strLabel += " ParentId=\"" + parentId + "\"";
                }
                if (sql.Trim() != string.Empty)
                {
                    strLabel += " Sql=\"" + sql + "\"";
                }
                if (isParentColumnId != string.Empty)
                {
                    strLabel += " IsParentColumnId=\"" + isParentColumnId + "\"";
                }
                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "Column":
                field = RequestUtil.Instance.GetFormString("Field");
                length = RequestUtil.Instance.GetFormString("Length");
                var lstParent = RequestUtil.Instance.GetFormString("lstParent");
                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                if (!field.Trim().IsEmpty())
                {
                    strLabel += " Field=\"" + field + "\"";
                }
                if (!length.Trim().IsEmpty())
                {
                    strLabel += " Length=\"" + length + "\"";
                }
                if (!lstParent.IsEmpty())
                {
                    strLabel += " Parent=\"" + lstParent + "\"";
                }
                strLabel =
                    string.Format("<wtl:{0} Id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "content":
                field = RequestUtil.Instance.GetFormString("Field");
                var type = RequestUtil.Instance.GetFormString("Type");
                var lstAutoLink = RequestUtil.Instance.GetFormString("lstAutoLink");
                var leftText = RequestUtil.Instance.GetFormString("LeftText");
                var rightText = RequestUtil.Instance.GetFormString("RightText");
                length = RequestUtil.Instance.GetFormString("Length");
                sql = RequestUtil.Instance.GetFormString("Sql");
                var where = RequestUtil.Instance.GetFormString("Where");

                if (!field.Trim().IsEmpty())
                {
                    strLabel += " field=\"" + field + "\"";
                }
                if (type != "")
                {
                    strLabel += " type=\"" + type + "\"";
                }
                if (!lstAutoLink.IsEmpty())
                {
                    strLabel += " isautolink=\"" + lstAutoLink + "\"";
                }
                if (!leftText.Trim().IsEmpty() && type == "prepage")
                {
                    strLabel += " lefttext=\"" + leftText + "\"";
                }
                if (!rightText.Trim().IsEmpty() && type == "nextpage")
                {
                    strLabel += " righttext=\"" + rightText + "\"";
                }
                if (!length.Trim().IsEmpty())
                {
                    strLabel += " length=\"" + length + "\"";
                }

                if (!sql.Trim().IsEmpty())
                {
                    strLabel += " sql=\"" + sql + "\"";
                }
                if (!where.Trim().IsEmpty())
                {
                    strLabel += " where=\"" + where + "\"";
                }

                //如果没有选中需要生成的项，则不生成
                if (strLabel == "")
                {
                    return new HandlerResult {Status = true, Message = ""};
                }
                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2).ToStr(),
                        strLabel).ToLower();
                break;
            case "system":
                var site = RequestUtil.Instance.GetFormString("Site");

                if (site != string.Empty)
                {
                    string path =
                        DbHelper.CurrentDb.ExecuteScalar<string>("SELECT path FROM whir_sit_siteinfo WHERE siteid=@0",
                            site);
                    strLabel += " SitePath=\"" + path + "\"";
                }
                strLabel += " Type=\"CopyRight\"";

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "hits":
                var itemId = RequestUtil.Instance.GetFormString("ItemID");
                var fieldName = RequestUtil.Instance.GetFormString("FieldName");

                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                if (itemId.Trim() != string.Empty)
                {
                    strLabel += " ItemID=\"" + itemId + "\"";
                }
                if (fieldName.Trim() != string.Empty)
                {
                    strLabel += " FieldName=\"" + fieldName + "\"";
                }

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();

                break;
            case "Image":
                var fileName = RequestUtil.Instance.GetFormString("FileName");
                var errorFile = RequestUtil.Instance.GetFormString("ErrorFile");
                var lstIsShowError = RequestUtil.Instance.GetFormString("lstIsShowError");
                var height = RequestUtil.Instance.GetFormString("Height");
                var width = RequestUtil.Instance.GetFormString("Width");

                if (!fileName.IsEmpty())
                {
                    strLabel += " FileName=\"" + fileName + "\"";
                }
                if (!errorFile.IsEmpty())
                {
                    strLabel += " ErrorFile=\"" + errorFile + "\"";
                }
                if (!lstIsShowError.IsEmpty())
                {
                    strLabel += " IsShowError=\"" + lstIsShowError + "\"";
                }
                if (!height.IsEmpty())
                {
                    strLabel += " Height=\"" + height + "\"";
                }
                if (!width.IsEmpty())
                {
                    strLabel += " Width=\"" + width + "\"";
                }

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2),
                        strLabel).ToLower();


                break;
            case "include":
                site = RequestUtil.Instance.GetFormString("Site");
                fileName = RequestUtil.Instance.GetFormString("FileName");
                var Params = RequestUtil.Instance.GetFormString("Params");

                if (!site.IsEmpty())
                {
                    SiteInfo siteinfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(site.ToInt());
                    strLabel += " sitepath=\"" + siteinfo.Path + "\"";
                }
                if (!fileName.IsEmpty())
                {
                    strLabel += " filename=\"" + fileName + "\"";
                }
                if (!Params.IsEmpty())
                {
                    strLabel += " params=\"" + Params + "\"";
                }

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();

                break;
            case "infor":
                field = RequestUtil.Instance.GetFormString("Field");
                length = RequestUtil.Instance.GetFormString("Length");
                itemId = RequestUtil.Instance.GetFormString("ItemID");
                string format = RequestUtil.Instance.GetFormString("Format");

                if (siteIColumnId != 0)
                {
                    strLabel += " columnid=\"" + siteIColumnId + "\"";
                }
                if (!field.Trim().IsEmpty())
                {
                    strLabel += " field=\"" + field + "\"";
                }
                if (!length.Trim().IsEmpty())
                {
                    strLabel += " length=\"" + length + "\"";
                }
                if (!itemId.Trim().IsEmpty())
                {
                    strLabel += " itemid=\"" + itemId + "\"";
                }
                if (!format.Trim().IsEmpty())
                {
                    strLabel += " format=\"" + format + "\"";
                }

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2).ToStr(),
                        strLabel).ToLower();

                break;
            case "inforarea":
                field = RequestUtil.Instance.GetFormString("Field");
                itemId = RequestUtil.Instance.GetFormString("ItemID");
                sql = RequestUtil.Instance.GetFormString("SQL");

                if (siteIColumnId != 0)
                {
                    strLabel += " columnid=\"" + siteIColumnId + "\"";
                }
                if (!field.Trim().IsEmpty())
                {
                    strLabel += " field=\"" + field + "\"";
                }
                if (!itemId.Trim().IsEmpty())
                {
                    strLabel += " itemid=\"" + itemId + "\"";
                }
                if (!sql.Trim().IsEmpty())
                {
                    strLabel += " sql=\"" + sql + "\"";
                }
                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "List":
                var categoryId = RequestUtil.Instance.GetFormString("CategoryID");
                where = RequestUtil.Instance.GetFormString("Where");
                var count = RequestUtil.Instance.GetFormString("Count");
                var order = RequestUtil.Instance.GetFormString("Order");
                var nullTip = RequestUtil.Instance.GetFormString("NullTip");
                sql = RequestUtil.Instance.GetFormString("Sql");
                var enabledTop = RequestUtil.Instance.GetFormString("EnabledTop");
                var topCount = RequestUtil.Instance.GetFormString("TopCount");
                var topWhere = RequestUtil.Instance.GetFormString("TopWhere");
                var topalwayshow = RequestUtil.Instance.GetFormString("Topalwayshow");
                var topOrder = RequestUtil.Instance.GetFormString("TopOrder");
                var needPage = RequestUtil.Instance.GetFormString("NeedPage");
                var pageSize = RequestUtil.Instance.GetFormString("PageSize");
                var footer = RequestUtil.Instance.GetFormString("Footer");

                string keyNum = labelName + Rand.Instance.Number(2); //定义一个List置标的主键值，为分页中进行Target属性赋值
                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                if (!categoryId.Trim().IsEmpty())
                {
                    strLabel += " CategoryID=\"" + categoryId + "\"";
                }
                if (!where.Trim().IsEmpty())
                {
                    strLabel += " Where=\"" + where + "\"";
                }
                if (!count.Trim().IsEmpty())
                {
                    strLabel += " Count=\"" + count + "\"";
                }
                if (!order.Trim().IsEmpty())
                {
                    strLabel += " Order=\"" + order + "\"";
                }
                if (!nullTip.Trim().IsEmpty())
                {
                    strLabel += " NullTip=\"" + nullTip + "\"";
                }
                if (!sql.Trim().IsEmpty())
                {
                    strLabel += " Sql=\"" + sql + "\"";
                }
                if (enabledTop == "True")
                {
                    strLabel += " TopAlwayShow=\"" + enabledTop + "\"";

                    if (!topCount.Trim().IsEmpty())
                    {
                        strLabel += " TopCount=\"" + topCount + "\"";
                    }
                    if (!topWhere.Trim().IsEmpty())
                    {
                        strLabel += " TopWhere=\"" + topWhere + "\"";
                    }
                    if (!topalwayshow.IsEmpty())
                    {
                        strLabel += " TopAlwayShow=\"" + topalwayshow + "\"";
                    }
                    if (!topOrder.Trim().IsEmpty())
                    {
                        strLabel += " TopOrder=\"" + topOrder + "\"";
                    }
                }
                //启用分页后才执行
                if (needPage == "True")
                {
                    string pagelabel = "Pager";
                    string page_label = string.Empty;
                    strLabel += " NeedPage=\"" + needPage + "\"";

                    page_label += " TargetID=\"" + keyNum + "\"";

                    if (!pageSize.Trim().IsEmpty())
                    {
                        page_label += " PageSize=\"" + pageSize + "\"";
                    }
                    if (!footer.Trim().IsEmpty())
                    {
                        page_label += " Footer=\"" + footer + "\"";
                    }
                    strLabel = string.Format("<wtl:{0} id=\"{1}\"{2}></wtl:{0}>", labelName, keyNum, strLabel).ToLower();

                    strLabel += "\n\n" +
                                string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", pagelabel,
                                    Rand.Instance.Number(2).ToStr(), page_label).ToLower();
                }
                else
                {
                    strLabel = string.Format("<wtl:{0} id=\"{1}\"{2}></wtl:{0}>", labelName, keyNum, strLabel).ToLower();
                }
                strLabel += "|@|" + GetFieldInfo(siteIColumnId);
                break;
            case "location":
                var separator = RequestUtil.Instance.GetFormString("Separator");
                var lstIsDefault = RequestUtil.Instance.GetFormString("lstIsDefault");
                var lstIsAutoLink = RequestUtil.Instance.GetFormString("lstIsAutoLink");
                var defaultValue = RequestUtil.Instance.GetFormString("DefaultValue");
                nullTip = RequestUtil.Instance.GetFormString("NullTip");
                var lstIsFullPath = RequestUtil.Instance.GetFormString("lstIsFullPath");
                var lstIsSubColumn = RequestUtil.Instance.GetFormString("lstIsSubColumn");
                var subId = RequestUtil.Instance.GetFormString("SubID");
                var urlParam = RequestUtil.Instance.GetFormString("UrlParam");
                var lstIsCategory = RequestUtil.Instance.GetFormString("lstIsCategory");
                var categoryParam = RequestUtil.Instance.GetFormString("CategoryParam");
                field = RequestUtil.Instance.GetFormString("Field");

                if (siteId != 0)
                {
                    SiteInfo siteinfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(siteId);
                    if (!siteinfo.IsDefault)
                    {
                        strLabel += " path=\"" + siteinfo.Path + "\"";
                    }
                }
                if (siteIColumnId != 0)
                {
                    strLabel += " columnid=\"" + siteIColumnId + "\"";
                }
                if (!separator.IsEmpty())
                {
                    strLabel += " separator=\"" + separator + "\"";
                }
                if (!lstIsDefault.IsEmpty())
                {
                    strLabel += " isdefault=\"" + lstIsDefault + "\"";
                }
                if (!nullTip.IsEmpty())
                {
                    strLabel += " nulltip=\"" + nullTip + "\"";
                }
                if (!lstIsAutoLink.IsEmpty())
                {
                    strLabel += " isautolink=\"" + lstIsAutoLink + "\"";
                }
                if (!defaultValue.IsEmpty())
                {
                    strLabel += " defaultvalue=\"" + defaultValue + "\"";
                }
                if (!lstIsFullPath.IsEmpty())
                {
                    strLabel += " isfullpath=\"" + lstIsFullPath + "\"";
                }
                if (!lstIsSubColumn.IsEmpty())
                {
                    strLabel += " issubcolumn=\"" + lstIsSubColumn + "\"";
                }
                if (!subId.IsEmpty())
                {
                    strLabel += " subid=\"" + subId + "\"";
                }
                if (!urlParam.IsEmpty())
                {
                    strLabel += " urlparam=\"" + urlParam + "\"";
                }
                if (!lstIsCategory.IsEmpty())
                {
                    strLabel += " iscategory=\"" + lstIsCategory + "\"";
                }
                if (!categoryParam.IsEmpty())
                {
                    strLabel += " categoryname=\"" + categoryParam + "\"";
                }
                if (!field.IsEmpty())
                {
                    strLabel += " field=\"" + field + "\"";
                }

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "many":
                var splitStr = RequestUtil.Instance.GetFormString("SplitStr");
                var split = RequestUtil.Instance.GetFormString("Split");
                count = RequestUtil.Instance.GetFormString("Count");
                if (splitStr.Trim() != string.Empty)
                {
                    strLabel += " SplitStr=\"" + splitStr + "\"";
                }
                if (split.Trim() != string.Empty)
                {
                    strLabel += " Split=\"" + split + "\"";
                }
                if (count.Trim() != string.Empty)
                {
                    strLabel += " Count=\"" + count + "\"";
                }
                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "Metadesc":
                labelName = "system";
                var paramName = RequestUtil.Instance.GetFormString("ParamName");
                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                if (paramName.Trim() != string.Empty)
                {
                    strLabel += " paramname=\"" + paramName + "\"";
                }
                strLabel += " Type=\"MetaDesc\"";

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();

                break;
            case "Metakey":
                labelName = "system";
                paramName = RequestUtil.Instance.GetFormString("ParamName");
                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                if (paramName.Trim() != string.Empty)
                {
                    strLabel += " paramname=\"" + paramName + "\"";
                }
                strLabel += " Type=\"MetaKey\"";

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();

                break;
            case "MetaTitle":
                labelName = "system";
                paramName = RequestUtil.Instance.GetFormString("ParamName");
                if (siteIColumnId != 0)
                {
                    strLabel += " ColumnId=\"" + siteIColumnId + "\"";
                }
                if (paramName.Trim() != string.Empty)
                {
                    strLabel += " paramname=\"" + paramName + "\"";
                }
                strLabel += " Type=\"MetaTitle\"";

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();

                break;
            case "play":
                var playFile = RequestUtil.Instance.GetFormString("PlayFile");
                var autoList = RequestUtil.Instance.GetFormString("AutoList");
                width = RequestUtil.Instance.GetFormString("Width");
                height = RequestUtil.Instance.GetFormString("Height");
                var lstShowErrorFile = RequestUtil.Instance.GetFormString("lstShowErrorFile");
                var errorFileName = RequestUtil.Instance.GetFormString("ErrorFileName");
                if (playFile.Trim() != string.Empty)
                {
                    strLabel += " IsAutoPlay=\"" + playFile + "\"";
                }
                if (autoList != string.Empty)
                {
                    strLabel += " FileName=\"" + autoList + "\"";
                }

                if (width.Trim() != string.Empty)
                {
                    strLabel += " Width=\"" + width + "\"";
                }
                if (height.Trim() != string.Empty)
                {
                    strLabel += " Height=\"" + height + "\"";
                }
                if (lstShowErrorFile != string.Empty)
                {
                    strLabel += " IsShowError=\"" + lstShowErrorFile + "\"";

                    if (lstShowErrorFile.ToBoolean())
                    {
                        if (errorFileName.Trim() != string.Empty)
                        {
                            strLabel += " ErrorFile=\"" + errorFileName + "\"";
                        }
                    }
                }
                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "Record":
                site = RequestUtil.Instance.GetFormString("Site");
                labelName = "system";

                if (site != string.Empty)
                {
                    strLabel += " SitePath=\"" + site + "\"";
                }
                strLabel += " Type=\"Record\"";

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "SiteName":
                site = RequestUtil.Instance.GetFormString("Site");
                labelName = "system";
                if (site != string.Empty)
                {
                    strLabel += " SitePath=\"" + site + "\"";
                }

                strLabel += " Type=\"SiteName\"";

                strLabel =
                    string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelName, Rand.Instance.Number(2), strLabel)
                        .ToLower();
                break;
            case "survey":
                if (siteIColumnId != 0)
                {
                    strLabel += " columnid=\"" + siteIColumnId + "\"";
                }
                strLabel = string.Format("<wtl:{0} id=\"{0}{1}\" {2}></wtl:{0}>", labelName, Rand.Instance.Number(2),
                    strLabel);
                break;
            case "vote":
                var successfulTips = RequestUtil.Instance.GetFormString("SuccessfulTips");
                var failedTips = RequestUtil.Instance.GetFormString("FailedTips");
                var submitText = RequestUtil.Instance.GetFormString("SubmitText");
                var ipRepeatTips = RequestUtil.Instance.GetFormString("IpRepeatTips");
                var unSelectAllTips = RequestUtil.Instance.GetFormString("UnSelectAllTips");
                var successUrl = RequestUtil.Instance.GetFormString("SuccessUrl");
                if (siteIColumnId != 0)
                {
                    strLabel += " columnid=\"" + siteIColumnId + "\"";
                }
                if (!successfulTips.IsEmpty())
                {
                    strLabel += " successfultips=\"" + successfulTips + "\"";
                }
                if (!failedTips.IsEmpty())
                {
                    strLabel += " failedtips=\"" + failedTips + "\"";
                }
                if (!submitText.IsEmpty())
                {
                    strLabel += " submittext=\"" + submitText + "\"";
                }
                if (!ipRepeatTips.IsEmpty())
                {
                    strLabel += " iprepeattips=\"" + ipRepeatTips + "\"";
                }
                if (!unSelectAllTips.IsEmpty())
                {
                    strLabel += " unselectalltips=\"" + unSelectAllTips + "\"";
                }
                if (!successUrl.IsEmpty())
                {
                    strLabel += " successurl=\"" + successUrl + "\"";
                }

                strLabel = string.Format("<wtl:{0} id=\"{0}{1}\" {2}></wtl:{0}>", labelName, Rand.Instance.Number(2),
                    strLabel);
                break;
            case "webform":
                var model = RequestUtil.Instance.GetFormString("Model");
                if (model != "")
                {
                    strLabel = "formid=\"" + model + "\"";
                }
                strLabel = string.Format("<wtl:{0} id=\"{0}{1}\" {2}></wtl:{0}>", labelName, Rand.Instance.Number(2),
                    strLabel);
                break;
        }


        return new HandlerResult { Status = true, Message = strLabel };

    }

    /// <summary>
    /// 获取当前栏目的字段
    /// </summary>
    private string GetFieldInfo(int columnId)
    {
        //清空
        string result = "";
        Model model = ServiceFactory.ModelService.GetModelByColumnId(columnId);
        if (model == null)
        {
            result = "没有任何字段";
            return result;
        }

        DataSet ds = DbHelper.CurrentDb.Query("SELECT FieldName,FieldAlias FROM Whir_Dev_Field WHERE IsDel=0 AND ModelId={0}".FormatWith(model.ModelId));
        if (ds.Tables.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                result += row["FieldName"].ToStr() + "：" + row["FieldAlias"].ToStr() + "&nbsp;&nbsp;，&nbsp;";
            }
            result = result.Substring(0, result.Length - 8);
        }
        return result;
    }
}