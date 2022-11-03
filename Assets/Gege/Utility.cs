using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gege
{
    public static class Utility
    {
        #region DrawWorldText as TextMesh
        public static TextMesh DrawWorldText(string text, GameObject parent, Vector3 position, int sortingOrder = 1, int fontSize = 12, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            GameObject gameObject = new("World_Text");
            gameObject.transform.SetParent(parent.transform);
            TextMesh textMesh = gameObject.AddComponent<TextMesh>();
            textMesh.text = text;
            textMesh.transform.position = position;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            textMesh.fontSize = fontSize;
            textMesh.anchor = anchor;
            textMesh.color = Color.white;
            return textMesh;
        }
        public static TextMesh DrawWorldText(string text, int sortingOrder = 1, int fontSize = 12, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            return DrawWorldText(text, new GameObject(), Vector3.zero, sortingOrder, fontSize, anchor);
        }
        #endregion
    }
}