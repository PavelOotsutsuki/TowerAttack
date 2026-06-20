using System.Collections.Generic;
using Tools.InputSettings;
using Tools.UI;
using Tools.UI.Extendeds;

namespace StartMenues.RegistrationButtonsPanels
{
    internal class RegistrationButtonsPanelSelectHandler: SignSelectHandler
    {
        public RegistrationButtonsPanelSelectHandler(ExtendedTMP_InputField loginIF, ExtendedTMP_InputField passwordIF, ExtendedTMP_InputField passwordAgainIF,
            ConfirmableFocusableButton registraitionButton, ConfirmableFocusableButton backButton, ConfirmableFocusableButton exitButton) :
            base(new Dictionary<object, NextFocusData>
            {
                { loginIF, null} ,
                { passwordIF, null} ,
                { passwordAgainIF, null} ,
                { registraitionButton, new NextFocusData(backButton, loginIF, backButton, exitButton) } ,
                { backButton, new NextFocusData(loginIF, registraitionButton, null, exitButton) } ,
                { exitButton, new NextFocusData(loginIF, registraitionButton, backButton, null) }
            })
        { }
    }
}