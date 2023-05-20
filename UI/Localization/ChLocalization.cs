using ModelCalculater.Definitions;
using UI.Data;

namespace UI.Localization
{
    public class ChLocalization : ILocalization
    {
        #region Common

        public string Common_Ok => "好的";
        public string Common_Cancel => "取消";

        #endregion

        #region Task result display

        public string TaskResultDisplay_Result_ModalTitle => "结果";
        public string TaskResultDisplay_TaskIsCorrect => "任务（I，T）是正确的";
        public string TaskResultDisplay_TaskIsIncorrect => "任务（I，T）必须调整";
        public string TaskResultDisplay_LinksTakesPlace => "信息链接发生";
        public string TaskResultDisplay_NoLinks => "没有信息链接";
        public string TaskResultDisplay_ModelInformation_FormattedText => "<p><b>赤字：</b>{0}<br><b>用于计算赤字的行：</b>{1}</p>";

        #endregion

        #region Info dialog

        public string InfoDialog_Title => "参考资料";
        public string InfoDialog_DescriptionText => "该程序是用于确定任务状态的解决方案";
        public string InfoDialog_DesignationsLabel => "指定名称:";
        public string InfoDialog_FrameColorDescription_Blue => "名称以这种颜色突出显示的列被视为变量列";
        public string InfoDialog_FrameColorDescription_Red => "名称以这种颜色突出显示的列被视为这些值的列";
        public string InfoDialog_FrameColorDescription_Green => "名称以这种颜色突出显示的列被视为所需值的列";

        #endregion

        #region ProcedureTypeSelectorDialog

        public string ProcedureTypeSelectorDialog_Title => "任务类型";
        public string ProcedureTypeSelectorDialog_Message => "选择任务类型";

        #endregion

        #region Settings dialog

        public string SettingsDialog_Title => "设置";
        public string SettingsDialog_SaveButton => "储蓄";
        public string SettingsDialog_PlayResultCheckbox => "重现结果";

        #endregion

        #region Matrix page

        public string MatrixPage_EnterColumnName_ModalTitle => "输入列名";
        public string MatrixPage_IncorrectColumnName_ModalTitle => "列名不正确";
        public string MatrixPage_IncorrectColumnName_ModalText => "列名不正确. 也许具有此名称的列已经存在";
        public string MatrixPage_Error_ModalTitle => "错误";
        public string MatrixPage_Error_ModalText => "你必须指定矩阵";

        #endregion

        #region Nav menu

        public string NavMenu_Title => "Matrix calculater";
        public string NavMenu_Calculate => "计算方法";
        public string NavMenu_Redact => "编辑";
        public string NavMenu_Clear => "清楚";
        public string NavMenu_About => "参考资料";
        public string NavMenu_Settings => "设置";

        #endregion

        #region Extensions

        public string GetName(TaskType taskType)
        {
            return taskType switch
            {
                TaskType.NoSolution => "问题没有解决方案",
                TaskType.Estimated => "任务计算",
                TaskType.Optimization => "优化任务",
                _ => throw new NotImplementedException(),
            };
        }

        public string GetName(FormationProcedureType procedureType)
        {
            return procedureType switch
            {
                FormationProcedureType.Status => "状态确定",
                FormationProcedureType.ComputationalModel => "计算模型的形成",
                FormationProcedureType.InformationLinks => "信息链接的可用性",
                FormationProcedureType.InformationAboutModel => "模型信息",
                _ => throw new NotImplementedException(),
            };
        }


        #endregion
    }
}