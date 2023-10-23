using System.Collections.Generic;
using Unity.Mathematics;

namespace HexLib
{
    public struct Layout
    {
        public readonly Orientation Orientation;
        public readonly float3 Size;
        public readonly float3 Origin;
    
        public static Orientation Pointy = new Orientation(math.sqrt(3.0f), math.sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, 
                                                            math.sqrt(3.0f) / 3.0f, - 1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);
    
        public static Orientation Flat = new Orientation(3.0f / 2.0f, 0.0f, math.sqrt(3.0f) / 2.0f, math.sqrt(3.0f),
                                                          2.0f / 3.0f, 0.0f, -1.0f / 3.0f, math.sqrt(3.0f) / 3.0f, 0.0f);
    
        public Layout(Orientation orientation, float3 size, float3 origin)
        {
            Orientation = orientation;
            Size = size;
            Origin = origin;
        }
    
        public float3 HexToPixel(Hex h)
        {
            Orientation m = Orientation;
    
            float x = (m.F0 * h.Q + m.F1 * h.R) * Size.x;
            float z = (m.F2 * h.Q + m.F3 * h.R) * Size.z;
    
            return new float3(x + Origin.x, 0, z + Origin.z);
        }
    
        public FractionalHex PixelToHex(float3 p)
        {
            Orientation m = Orientation;
            float3 pt = new float3((p.x - Origin.x) / Size.x, 0, (p.z - Origin.z) / Size.z);
            double q = m.B0 * pt.x + m.B1 * pt.z;
            double r = m.B2 * pt.x + m.B3 * pt.z;
            return new FractionalHex(q, r, -q - r);
        }
    
        public float3 HexCornerOffset(int corner)
        {
            Orientation m = Orientation;
            float angle = 2.0f * math.PI * (m.StartAngle - corner) / 6.0f;
            return new float3(Size.x * math.cos(angle), 0, Size.z * math.sin(angle));
        }
    
        public List<float3> PolygonCorners(Hex h)
        {
            List<float3> corners = new List<float3>();
            float3 center = HexToPixel(h);
    
            for(int i = 0; i < 6; i++)
            {
                float3 offset = HexCornerOffset(i);
                corners.Add(new float3(center.x + offset.x, 0, center.z + offset.z));
            }
    
            return corners;
        }
    }
}

