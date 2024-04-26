using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialog : TutorialBase
{


    public override void Enter()
    {
        Managers.UI.ShowPopupUI<Tutorial_Dialog_Popup_UI>("[Tutorial]_Dialog_Popup_UI");
    }

    public override void Execute(TutorialController controller)
    {
        
    }

    public override void Exit() 
    {
    }
}
