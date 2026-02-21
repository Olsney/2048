using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Elements
{
    public class CubeValueView : MonoBehaviour, ICubeValueView
    {
        [SerializeField] private List<TextMeshPro> _texts = new();

        public void SetValue(int value)
        {
            foreach (TextMeshPro text in _texts)
                text.SetText(value.ToString());
        }
    }
}
