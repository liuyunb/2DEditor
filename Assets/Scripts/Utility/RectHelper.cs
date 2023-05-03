using UnityEngine;

namespace Utility
{
    public static class RectHelper
    {
        public static Rect RectForAnchorCenter(Vector2 centerPos, Vector2 size)
        {
            var weight = size.x;
            var height = size.y;
            var x = centerPos.x - weight * 0.5f;
            var y = centerPos.y - height - 0.5f;

            return new Rect(x, y, weight, height);
        }
        
        public static Rect RectForAnchorCenter(float x, float y, float weight, float height)
        {
            var finalX = x - weight * 0.5f;
            var finalY = y - height * 0.5f;

            return new Rect(finalX, finalY, weight, height);
        }
    }
}