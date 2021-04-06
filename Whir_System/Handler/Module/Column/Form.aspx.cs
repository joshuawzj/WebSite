using System;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;



public partial class Whir_System_Handler_Module_Column_Form : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }


    /// <summary>
    /// 批量添加已有字段到表单
    /// </summary>
    public HandlerResult AddFieldToForm()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            var selected = RequestUtil.Instance.GetFormString("selected").Trim(',');
            int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
            int maxLength = RequestUtil.Instance.GetFormString("MaxLength").ToInt(0);
            string[] arrSeleted = selected.Split(',');
            foreach (string fieldId in arrSeleted)
            {
                SaveAddToField(fieldId.ToInt(), columnId, maxLength);
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int formId = RequestUtil.Instance.GetFormString("FormId").ToInt(0);
        int fieldId = RequestUtil.Instance.GetFormString("FieldId").ToInt(0);
        int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
        HandlerResult result = new HandlerResult();

        try
        {
            if (formId != 0)
            {
                //编辑表单输入项
                result = SaveEditForm(formId, fieldId, columnId);
            }
            else if (fieldId != 0)
            {
                //添加已有字段,传入要添加的字段ID
                result = SaveAddField(fieldId, columnId);
            }
            else if (formId == 0 && fieldId == 0)
            {
                //新建表单输入字段
                result = SaveNewForm(columnId);
            }

            return result;
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }

    }

    //新建表单输入字段
    private HandlerResult SaveNewForm(int columnId)
    {
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId);
        if (column == null)
            return new HandlerResult { Status = false, Message = "ColumnId为{0}的栏目不存在".FormatWith(columnId) };

        Field field = ModelFactory<Field>.Insten();
        Form form = ModelFactory<Form>.Insten();

        //反射获取表单字段数据
        var type = typeof(Form);
        form = GetPostObject(type, form) as Form;

        //反射获取表单字段数据
        type = typeof(Field);
        field = GetPostObject(type, field) as Field;

        field.ModelId = column.ModelId;
        field.IsEnableListShow = 1;

        form.ColumnId = columnId;
        form.ModelId = field.ModelId;
        //默认值
        string dateDefaultValue = RequestUtil.Instance.GetFormString("DateDefaultValue");
        string txtDateDefaultValue = RequestUtil.Instance.GetFormString("txtDateDefaultValue");
        string boolDefaultValue = RequestUtil.Instance.GetFormString("BoolDefaultValue");
        if (field.FieldType == 7)
        {
            form.DefaultValue = dateDefaultValue == "3" ? txtDateDefaultValue.ToStr().Trim() : dateDefaultValue;
        }
        else if (field.FieldType == 9)//是/否
        {
            form.DefaultValue = boolDefaultValue;
        }
 
        if (ServiceFactory.SensitiveWordService.IsWord(form.FieldAlias))
        {
            return new HandlerResult { Status = false, Message = "表单别名中包含敏感词".ToLang() };
        }

        int formId = ServiceFactory.FormService.AddForm(form, field);

        if (form.IsBold || form.IsColor)
            ServiceFactory.FormService.AddBoldAndColorColumn(form, field);//颜色/加粗列

        SaveAttach(formId, field.FieldType.ToStr());
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    //批量添加已有字段
    private void SaveAddToField(int fieldId, int columnId, int maxLength)
    {

        var field = ServiceFactory.FieldService.SingleOrDefault<Field>(fieldId);
        if (field == null)
            throw new Exception("未找到FieldID为{0}的字段".FormatWith(fieldId));
        var form = ServiceFactory.FormService.GetFormByColumnIdAndFieldId(columnId, fieldId) ?? ModelFactory<Form>.Insten();
        if (form.FormId > 0)
            return;

        form.FieldId = field.FieldId;
        form.ColumnId = columnId;
        form.ModelId = field.ModelId;
        form.FieldAlias = field.FieldAlias;
        form.IsAllowNull = true;
        form.IsOnly = false;
        form.IsReadOnly = false;
        form.DefaultValue = field.DefaultValue;
        form.MaxLength = maxLength.ToInt();
        form.ValidateType = field.ValidateType;
        form.ValidateExpression = field.ValidateExpression;

        int formId = ServiceFactory.FormService.Insert(form).ToInt();
        //补上默认的附属表单
        ServiceFactory.FormService.AddDefaultAttachForm(formId, field);
    }

    //添加已有字段
    private HandlerResult SaveAddField(int fieldId, int columnId)
    {

        string fieldType = RequestUtil.Instance.GetFormString("FieldType");
        Form form = ModelFactory<Form>.Insten();

        Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(fieldId);
        if (field == null)
            return new HandlerResult { Status = false, Message = "未找到FieldID为{0}的字段".FormatWith(fieldId) };

        //判断form表是否存在要添加的字段
        if (ServiceFactory.FormService.IsExist(fieldId, columnId))
        {
            return new HandlerResult { Status = false, Message = "表单已经存在此字段".ToLang() };
        }

        //反射获取表单字段数据
        var type = typeof(Form);
        form = GetPostObject(type, form) as Form;

        form.ModelId = field.ModelId;
        switch (field.FieldType.ToStr())
        {
            case "1":

                #region 单行文本

                //string[] widthStrs = {"title", "productname", "magazinename"}; //特殊字段，创建表单时文本的宽度设置为500
                //if (widthStrs.Contains(field.FieldName.ToLower()))
                //{
                //    form.Width = 500;
                //}
                //else
                //{
                //    form.Width = 300;
                //}

                #endregion 单行文本

                break;
            case "2":

                #region 多行文本

                //form.Width = form.Width == 0 ? 500 : form.Width;
                //form.Height = form.Height == 0 ? 130 : form.Height;

                #endregion 多行文本

                break;
            case "3":

                #region HTML

                form.Width = form.Width == 0 ? 640 : form.Width;
                form.Height = form.Height == 0 ? 350 : form.Height;

                #endregion HTML

                break;
            case "5":

                #region 数字

                form.Width = form.Width ;

                #endregion 数字

                break;
            case "6":

                #region 货币

                form.Width = form.Width ;

                #endregion 货币

                break;
            case "8":

                #region 超链接

                form.Width = form.Width == 0 ? 200 : form.Width;

                #endregion 超链接

                break;

            case "14":

                #region 密码型字段

                form.Width = form.Width == 0 ? 200 : form.Width;

                #endregion 密码型字段

                break;
        }

        if (ServiceFactory.SensitiveWordService.IsWord(form.FieldAlias))
        {
            return new HandlerResult { Status = false, Message = "表单别名中包含敏感词".ToLang() };
        }

        var formId = ServiceFactory.FormService.Insert(form);

        if (form.IsBold || form.IsColor)
            ServiceFactory.FormService.AddBoldAndColorColumn(form, field); //颜色/加粗列

        SaveAttach(formId.ToInt(), fieldType);

        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    //编辑表单输入项
    private HandlerResult SaveEditForm(int formId, int fieldId, int columnId)
    {
        Form form = ServiceFactory.FormService.SingleOrDefault<Form>(formId);
        int fId = form.FieldId;
        Field field = ServiceFactory.FieldService.GetFieldByFormId(formId);
        string fieldType = RequestUtil.Instance.GetFormString("FieldType");
        if (form == null)
            return new HandlerResult { Status = false, Message = "未找到FormID为{0}的表单输入项".FormatWith(formId) };
        if (field == null)
            return new HandlerResult { Status = false, Message = "未找到FieldID为{0}的字段".FormatWith(fieldId) };
        if (field.FieldType.ToStr() != fieldType)
            return new HandlerResult
            {
                Status = false,
                Message = "字段类型“{0}”与表现形式“{1}”不匹配".FormatWith(field.FieldType, fieldType)
            };

        //反射获取表单字段数据
        var type = typeof(Form);
        form = GetPostObject(type, form) as Form;

        if (form.FieldId == 0)
        {
            form.FieldId = fId;
        }
        //判断form表是否存在要添加的字段
        if (ServiceFactory.FormService.IsExist(fieldId, columnId))
        {
            return new HandlerResult { Status = false, Message = "表单已经存在此字段".ToLang() };
        }

        switch (fieldType)
        {
            case "1":

                #region 单行文本
                //string[] widthStrs = {"title", "productname", "magazinename"}; //特殊字段，创建表单时文本的宽度设置为500
                //if (widthStrs.Contains(field.FieldName.ToLower()))
                //{
                //    form.Width = 500;
                //}
                //else
                //{
                //    form.Width = 300;
                //}

                #endregion 单行文本

                break;
            case "2":

                #region 多行文本

                //form.Width = form.Width == 0 ? 500 : form.Width;
                //form.Height = form.Height == 0 ? 130 : form.Height;

                #endregion 多行文本

                break;
            case "3":

                #region HTML

                form.Width = form.Width == 0 ? 640 : form.Width;
                form.Height = form.Height == 0 ? 350 : form.Height;

                #endregion HTML

                break;
            case "4":

                #region 选项

                #endregion 选项

                break;
            case "5":

                #region 数字

                form.Width = form.Width ;

                #endregion 数字

                break;
            case "6":

                #region 货币

                form.Width = form.Width ;

                #endregion 货币

                break;
            case "7":
                #region 日期
                string dateDefaultValue = RequestUtil.Instance.GetFormString("DateDefaultValue");
                string txtDateDefaultValue = RequestUtil.Instance.GetFormString("txtDateDefaultValue");
                if (dateDefaultValue == "1" || dateDefaultValue == "2")
                {
                    form.DefaultValue = dateDefaultValue;
                }
                else
                {
                    form.DefaultValue = txtDateDefaultValue;
                }
                #endregion
                break;
            case "8":

                #region 超链接

                form.Width = form.Width == 0 ? 200 : form.Width;

                #endregion 超链接

                break;
            case "9":

                #region 是/否
                string boolDefaultValue = RequestUtil.Instance.GetFormString("BoolDefaultValue");
                form.DefaultValue = boolDefaultValue;
                #endregion 是/否

                break;
            case "10":

                #region 图片

                #endregion 图片

                break;
            case "11":

                #region 文件
                #endregion 文件
                break;
            case "13":

                #region 地区

                #endregion 地区

                break;
            case "14":

                #region 密码型字段

                form.Width = form.Width == 0 ? 200 : form.Width;

                #endregion 密码型字段

                break;

        }

        if (ServiceFactory.SensitiveWordService.IsWord(form.FieldAlias))
        {
            return new HandlerResult { Status = false, Message = "表单别名中包含敏感词".ToLang() };
        }

        ServiceFactory.FormService.Update(form);

        if (form.IsBold || form.IsColor)
            ServiceFactory.FormService.AddBoldAndColorColumn(form, field); //颜色/加粗列

        SaveAttach(formId, fieldType);
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }


    //保存附加表单
    private void SaveAttach(int formId, string fieldType)
    {
        switch (fieldType)
        {
            case "4":
                #region 保存FormOption

                FormOption formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(formId) ??
                                        ModelFactory<FormOption>.Insten();

                //反射获取表单字段数据
                var type = typeof(FormOption);
                formOption = GetPostObject(type, formOption) as FormOption;
                if (formOption.FormOptionId != 0)
                    ServiceFactory.FormOptionService.Update(formOption);
                else
                    formOption.FormId = formId;
                ServiceFactory.FormOptionService.Save(formOption);

                #endregion 保存FormOption
                break;
            case "7":
                #region 保存FormDate

                FormDate formDate = ServiceFactory.FormDateService.GetFormDateByFormID(formId) ?? ModelFactory<FormDate>.Insten();

                //反射获取表单字段数据
                type = typeof(FormDate);
                formDate = GetPostObject(type, formDate) as FormDate;
                formDate.FormId = formId;
                if (formDate.FormDateId != 0)
                    ServiceFactory.FormDateService.Update(formDate);
                else
                    ServiceFactory.FormDateService.Save(formDate);

                #endregion 保存FormDate
                break;
            case "10":
                #region 保存FormImageUpload
                FormUpload formImageUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(formId) ?? ModelFactory<FormUpload>.Insten();

                //反射获取表单字段数据
                type = typeof(FormUpload);
                formImageUpload = GetPostObject(type, formImageUpload) as FormUpload;
                formImageUpload.FormId = formId;
                formImageUpload.UploadMode = RequestUtil.Instance.GetFormString("ImageUploadMode").ToInt();
                if (formImageUpload.FormUploadId != 0)
                    ServiceFactory.FormUploadService.Update(formImageUpload);
                else
                    ServiceFactory.FormUploadService.Save(formImageUpload);


                #endregion
                break;
            case "11":
                #region 保存FormFileUpload

                FormUpload formFileUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(formId) ??
                                            ModelFactory<FormUpload>.Insten();

                //反射获取表单字段数据
                type = typeof(FormUpload);
                formFileUpload = GetPostObject(type, formFileUpload) as FormUpload;
                formFileUpload.UploadMode = RequestUtil.Instance.GetFormString("FileUploadMode").ToInt();
                formFileUpload.FormId = formId;
                if (formFileUpload.FormUploadId != 0)
                    ServiceFactory.FormUploadService.Update(formFileUpload);
                else
                    ServiceFactory.FormUploadService.Save(formFileUpload);

                #endregion
                break;
            case "13":
                #region 保存FormArea
                FormArea formArea = ServiceFactory.FormAreaService.GetFormAreaByFormID(formId) ?? ModelFactory<FormArea>.Insten();
                //反射获取表单字段数据
                type = typeof(FormArea);
                formArea = GetPostObject(type, formArea) as FormArea;
                formArea.FormId = formId;
                if (formArea.FormAreaId != 0)
                    ServiceFactory.FormAreaService.Update(formArea);
                else
                    ServiceFactory.FormAreaService.Save(formArea);

                #endregion 保存FormArea
                break;
        }
    }

    /// <summary>
    /// 调用属性
    /// </summary>
    /// <returns></returns>
    public HandlerResult LinkAttr()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int linkFormId = RequestUtil.Instance.GetFormString("LinkFormId").ToInt();
        Field field = ServiceFactory.FieldService.SingleOrDefault<Field>(linkFormId);
        if (field != null)
            return new HandlerResult { Status = true, Message = field.ToJson() };
        else
        {
            var form = ServiceFactory.FormService.SingleOrDefault<object>(@"select f1.*,f2.FieldType,f2.IsSystemField,f2.FieldName from Whir_Dev_Form f1,Whir_Dev_Field f2 
                                                                            where f1.FieldId=f2.FieldId and f1.FormId=@0", linkFormId) ?? ModelFactory<object>.Insten();

            return new HandlerResult { Status = true, Message = form.ToJson() };
        }

    }

    /// <summary>
    /// 列表显示/掩藏
    /// </summary>
    /// <returns></returns>
    public HandlerResult ListShow()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int formid = RequestUtil.Instance.GetFormString("FormId").ToInt(0);
            ServiceFactory.FormService.ChangeListShow(formid);
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 搜索显示/掩藏
    /// </summary>
    /// <returns></returns>
    public HandlerResult SearchShow()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int formid = RequestUtil.Instance.GetFormString("FormId").ToInt(0);
            ServiceFactory.FormService.ChangeSearchShow(formid);
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int formid = RequestUtil.Instance.GetFormString("FormId").ToInt(0);
            ServiceFactory.FormService.RemoveFormByFormId(formid);
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 批量排序
    /// </summary>
    /// <returns></returns>
    public HandlerResult Sort()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string strSort = RequestUtil.Instance.GetFormString("strSort").Trim(',');
            string[] arrSort = strSort.Split(',');
            foreach (string str in arrSort)
            {
                int formId = str.Split('|')[0].ToInt();
                long sort = str.Split('|')[1].ToLong(0);
                ServiceFactory.FormService.ModifyFormSort(formId, sort);
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelAll()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string selected = RequestUtil.Instance.GetFormString("selected").Trim(',');
            string[] arrSelected = selected.Split(',');
            foreach (string formId in arrSelected)
            {
                ServiceFactory.FormService.RemoveFormByFormId(formId.ToInt());
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Entity()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            int columnId = RequestUtil.Instance.GetFormString("ColumnId").ToInt(0);
            bool result = ServiceFactory.ModelService.CreateColumnEntity(columnId);
            if (result)
                return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
            else
                return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }
    /// <summary>
    /// 表单推送
    /// </summary>
    /// <returns></returns>
    public HandlerResult PushForm()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string selectedOpen = RequestUtil.Instance.GetFormString("selectedOpen");
            string selected = RequestUtil.Instance.GetFormString("selected");
            string[] columnIds = GetStringArrayByStringSplit(selectedOpen);//获取选择要批量推送的栏目ID号
            int[] formIds = GetIntArrayByStringSplit(selected);//选中的字段ID号
            foreach (string strColumnId in columnIds)
            {
                int columnId = strColumnId.Split('|')[0].ToInt();
                if (columnId == 0)
                    continue;

                foreach (int formId in formIds)
                {
                    var sourceForm = ServiceFactory.FormService.SingleOrDefault<Form>(formId);
                    if (sourceForm == null)
                        continue;

                    //检测目标栏目是否已经存在此form
                    var exsit = ServiceFactory.FormService.IsExist(sourceForm.FieldId, columnId);
                    if (exsit)
                        continue;

                    //复制form
                    var newForm = ModelFactory<Form>.Clone(sourceForm);
                    newForm.ColumnId = columnId;
                    int newFormId = ServiceFactory.FormService.Insert(newForm).ToInt();

                    //复制附属form
                    var field = ServiceFactory.FieldService.SingleOrDefault<Field>(newForm.FieldId);
                    switch (field.FieldType)
                    {
                        case 4:
                            //保存FormOption
                            FormOption formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(sourceForm.FormId);
                            var newformOption = ModelFactory<FormOption>.Clone(formOption);
                            newformOption.FormId = newFormId;
                            ServiceFactory.FormOptionService.Insert(newformOption);
                            break;
                        case 7:
                            //保存FormDate
                            FormDate formDate = ServiceFactory.FormDateService.GetFormDateByFormID(sourceForm.FormId);
                            var newformDate = ModelFactory<FormDate>.Clone(formDate);
                            newformDate.FormId = newFormId;
                            ServiceFactory.FormDateService.Insert(newformDate);
                            break;
                        case 10:
                            //保存FormUpload
                            FormUpload formImageUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(sourceForm.FormId);
                            var newformImageUpload = ModelFactory<FormUpload>.Clone(formImageUpload);
                            newformImageUpload.FormId = newFormId;
                            ServiceFactory.FormUploadService.Insert(newformImageUpload);
                            break;
                        case 11:
                            //保存FormUpload
                            FormUpload formFileUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(sourceForm.FormId);
                            var newformFileUpload = ModelFactory<FormUpload>.Clone(formFileUpload);
                            newformFileUpload.FormId = newFormId;
                            ServiceFactory.FormUploadService.Insert(newformFileUpload);
                            break;
                        case 13:
                            //保存FormArea
                            FormArea formArea = ServiceFactory.FormAreaService.GetFormAreaByFormID(sourceForm.FormId);
                            var newformArea = ModelFactory<FormArea>.Clone(formArea);
                            newformArea.FormId = newFormId;
                            ServiceFactory.FormAreaService.Insert(newformArea);
                            break;
                    }
                }
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败：".ToLang() + ex.Message };
        }
    }

    /// <summary>
    /// 推送字段 即推送未添加到form里的字段
    /// </summary>
    /// <returns></returns>
    public HandlerResult PushField()
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsDevUser);
            if (handlerResult.Status)
            {
                return new HandlerResult { Status = false, Message = handlerResult.Message };
            }
            string selectedOpen = RequestUtil.Instance.GetFormString("selectedOpen");
            string selected = RequestUtil.Instance.GetFormString("selected");
            string[] columnIds = GetStringArrayByStringSplit(selectedOpen);//获取选择要批量推送的栏目ID号
            int[] fieldIds = GetIntArrayByStringSplit(selected);//选中的字段ID号

            foreach (string strColumnId in columnIds)
            {
                int columnId = strColumnId.Split('|')[0].ToInt();
                if (columnId == 0)
                    continue;

                foreach (int fieldId in fieldIds)
                {
                    //检测目标栏目是否已经存在此field
                    var exsit = ServiceFactory.FormService.IsExist(fieldId, columnId);
                    if (exsit)
                        continue;

                    var form = ModelFactory<Form>.Insten();
                    var field = ServiceFactory.FieldService.SingleOrDefault<Field>(fieldId);
                    if (field == null)
                        continue;

                    form.FieldId = field.FieldId;
                    form.ColumnId = columnId;
                    form.ModelId = field.ModelId;
                    form.FieldAlias = field.FieldAlias;
                    form.IsAllowNull = true;
                    form.IsOnly = false;
                    form.IsReadOnly = false;
                    form.DefaultValue = field.DefaultValue;
                    form.MaxLength = 250;
                    form.ValidateType = field.ValidateType;
                    form.ValidateExpression = field.ValidateExpression;
                    int formId = ServiceFactory.FormService.Insert(form).ToInt();

                    //附属表单
                    ServiceFactory.FormService.AddDefaultAttachForm(formId, field);
                }
            }
            return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "操作失败".ToLang() };
        }
    }

    //以英文逗号分开的字符串, 分割成int类型数组
    private int[] GetIntArrayByStringSplit(string stringSplit)
    {
        string[] arrStrings = stringSplit.Split(',');
        int[] reslut = new int[arrStrings.Length];
        if (arrStrings.Length > 0)
        {
            for (int i = 0; i < arrStrings.Length; i++)
            {
                reslut[i] = arrStrings[i].ToInt(0);
            }

        }
        return reslut;
    }
    //以英文逗号分开的字符串, 分割成string类型数组
    private string[] GetStringArrayByStringSplit(string stringSplit)
    {
        return stringSplit.Split(',');
    }

}