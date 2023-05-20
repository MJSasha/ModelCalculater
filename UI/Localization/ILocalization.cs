using ModelCalculater.Definitions;
using UI.Data;

namespace UI.Localization
{
	public interface ILocalization
	{
		#region Common

		string Common_Ok { get; }
		string Common_Cancel { get; }

        #endregion

        #region Task result display

        string TaskResultDisplay_Result_ModalTitle { get; }
        string TaskResultDisplay_TaskIsCorrect { get; }
        string TaskResultDisplay_TaskIsIncorrect { get; }
        string TaskResultDisplay_LinksTakesPlace { get; }
        string TaskResultDisplay_NoLinks { get; }
        string TaskResultDisplay_ModelInformation_FormattedText { get; }

        #endregion

        #region Info dialog

        string InfoDialog_Title { get; }
		string InfoDialog_DescriptionText { get; } 
		string InfoDialog_DesignationsLabel { get; } 
		string InfoDialog_FrameColorDescription_Blue { get; } 
		string InfoDialog_FrameColorDescription_Red { get; } 
		string InfoDialog_FrameColorDescription_Green { get; }

        #endregion

        #region ProcedureTypeSelectorDialog

        string ProcedureTypeSelectorDialog_Title { get; }
        string ProcedureTypeSelectorDialog_Message { get; }

        #endregion

        #region Settings dialog

        string SettingsDialog_Title { get; }
		string SettingsDialog_SaveButton { get; } 
		string SettingsDialog_PlayResultCheckbox { get; } 

        #endregion

        #region Matrix page

        string MatrixPage_EnterColumnName_ModalTitle { get; } 
		string MatrixPage_IncorrectColumnName_ModalTitle { get; } 
		string MatrixPage_IncorrectColumnName_ModalText { get; } 
		string MatrixPage_Error_ModalTitle { get; } 
		string MatrixPage_Error_ModalText { get; }

		#endregion

		#region Nav menu

		string NavMenu_Title { get; } 
		string NavMenu_Calculate { get; } 
		string NavMenu_Redact { get; } 
		string NavMenu_Clear { get; } 
		string NavMenu_About { get; } 
		string NavMenu_Settings { get; } 

		#endregion

		#region Extensions

		string GetName(TaskType taskType);
		string GetName(FormationProcedureType procedureType);

        #endregion
    }
}
