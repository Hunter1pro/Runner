using System;
using System.Collections.Generic;

namespace HexLib
{
    public struct FractionalHex 
    {
        public readonly double Q;
        public readonly double R;
        public readonly double S;
    
        public FractionalHex(double q, double r, double s)
        {
            Q = q;
            R = r;
            S = s;
    
            if (Math.Abs(Q + R + S) != 0) throw new ArgumentException("q + r + s");
        }
    
        public Hex HexRound()
        {
            int qi = (int)(Math.Round(Q));
            int ri = (int)(Math.Round(R));
            int si = (int)(Math.Round(S));
    
            double q_diff = Math.Abs(qi - Q);
            double r_diff = Math.Abs(ri - R);
            double s_diff = Math.Abs(si - S);
    
            if (q_diff > r_diff && q_diff > s_diff)
            {
                qi = -ri - si;
            }
            else
            {
                if (r_diff > s_diff)
                {
                    ri = -qi - si;
                }
                else
                {
                    si = -qi - ri;
                }
            }
    
            return new Hex(qi, ri, si);
        }
    
        public FractionalHex HexLerp(FractionalHex b, double t) =>
            new FractionalHex(Q * (1.0 - t) + b.Q * t, R * (1.0 - t) + b.R * t, S * (1.0 - t) + b.S * t);
    
        public static List<Hex> HexLinedraw(Hex a, Hex b)
        {
            int n = a.Distance(b);
            FractionalHex a_nudge = new FractionalHex(a.Q + 1e-06, a.R + 1e-06, a.S - 2e-06);
            FractionalHex b_nudge = new FractionalHex(b.Q + 1e-06, b.R + 1e-06, b.S - 2e-06);
    
            List<Hex> results = new List<Hex>();
            double step = 1.0 / Math.Max(n, 1);
    
            for (int i = 0; i <= n; i++)
            {
                results.Add(a_nudge.HexLerp(b_nudge, step * i).HexRound());
            }
    
            return results;
        }
    }
}

