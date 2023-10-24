using System.Collections.Generic;
using UnityEngine;

public static class IsOnPanel
{
    public static bool Check(RectTransform rect, Vector2 inputPos)
    {
        Vector2 inversePos = rect.InverseTransformPoint(inputPos);

        return rect.rect.Contains(inversePos);
    }

    public static bool CheckAll(List<RectTransform> rects, Vector2 inputPos)
    {
        bool contains = false;

        foreach (var rect in rects)
        {
            if (rect == null)
            {
                Debug.LogError($"{nameof(IsOnPanel)} Rect is null");
                continue;
            }
            
            Vector2 inversePos = rect.InverseTransformPoint(inputPos);

            if (rect.rect.Contains(inversePos))
            {
                contains = true;
            }
        }
        
        return contains;
    }
}
