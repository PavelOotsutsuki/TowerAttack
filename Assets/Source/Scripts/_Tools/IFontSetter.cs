using System.Collections.Generic;
using TMPro;

namespace Tools
{
    public interface IFontSetter 
    {
        public void SetFont(IEnumerable<TMP_Text> texts);
    }
}