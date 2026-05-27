using System.Collections.Generic;
using Tools.InputSettings;
using Tools.UI;
using Tools.UI.Extendeds;

namespace StartMenues.LogInButtonsPanels
{
    internal class LogInButtonsPanelSelectHandler: SignSelectHandler
    {
        public LogInButtonsPanelSelectHandler(ExtendedTMP_InputField loginIF, ExtendedTMP_InputField passwordIF, ConfirmableFocusableButton logInButton,
            ConfirmableFocusableButton registraitionButton, ConfirmableFocusableButton exitButton) : base(new Dictionary<object, NextFocusData>
            {
                { loginIF, null} ,
                { passwordIF, null} ,
                { logInButton, new NextFocusData(registraitionButton, loginIF, registraitionButton, exitButton) } ,
                { registraitionButton, new NextFocusData(loginIF, logInButton, null, exitButton) } ,
                { exitButton, new NextFocusData(loginIF, logInButton, registraitionButton, null) }
            })
        { }
    }
}