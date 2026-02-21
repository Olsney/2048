using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class VictoryWindowView : WindowBase, IVictoryWindowView
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetMessage(string message) =>
            _text.text = message;
    }
}
