using System;
using UnityEngine;

namespace CubeGame.Scroll
{
    [Serializable]
    public sealed class ScrollElementData
    {
        [SerializeField] private string elementId;
        [SerializeField] private Sprite elementView;

        public string ElementId => elementId;
        public Sprite ElementView => elementView;

        public ScrollElementData(string elementId, Sprite elementView)
        {
            this.elementId = elementId;
            this.elementView = elementView;
        }
    }
}
