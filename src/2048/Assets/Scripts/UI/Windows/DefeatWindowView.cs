using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class DefeatWindowView : WindowBase, IDefeatWindowView
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetMessage(string message) =>
            _text.text = message;
    }
}
