using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexLib
{
    public struct Hex : IPathNode
    {
        public string CoordinateId { get; }
    
        public readonly int Q;
        public readonly int R;
        public readonly int S;
    
        public Hex(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
    
            CoordinateId = $"{Q};{R};{S}";
    
            if (q + r + s != 0)
                throw new ArgumentException("q + r + s must be 0");
        }
    
        public Hex Add(Hex b) =>
             new Hex(Q + b.Q, R + b.R, S + b.S);
    
        public Hex Subtract(Hex b) =>
            new Hex(Q - b.Q, R - b.R, S - b.S);
    
        public Hex Scale(int k) =>
            new Hex(Q * k, R * k, S * k);
    
        public Hex RotateLeft() =>
            new Hex(-S, -Q, -R);
    
        public Hex RotateRight() =>
            new Hex(-R, -S, -Q);
    
        public static List<Hex> Dirrections = new List<Hex>
        {
            new Hex(1, 0, -1),
            new Hex(1, -1, 0),
            new Hex(0, -1, 1),
            new Hex(-1, 0, 1),
            new Hex(-1, 1, 0),
            new Hex(0, 1, -1)
        };
    
        public static Hex Dirrection(int dirrection) => Hex.Dirrections[dirrection];
    
        public Hex Neighbour(int dirrection) => Add(Hex.Dirrection(dirrection));
    
        public static List<Hex> Diagonals = new List<Hex> { new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2) };
    
        public Hex DiagonalNeighbour(int direction) => Add(Hex.Dirrection(direction));
    
        public int Length() => (int)((Mathf.Abs(Q) + Mathf.Abs(R) + Mathf.Abs(S)) / 2);
    
        public int Distance(Hex b) => Subtract(b).Length();
    
        public bool Equals(Hex x, Hex y) => x == y;
    
        public static bool operator == (Hex first, Hex second) => (first.Q == second.Q && first.R == second.R && first.S == second.S);
        public static bool operator != (Hex first, Hex second) => !(first == second);
        public static Hex Add(Hex first, Hex second) => new Hex(first.Q + second.Q, first.R + second.R, first.S + second.S);

        public static Hex StringToHex(string coordinateId)
        {
            if (string.IsNullOrEmpty(coordinateId))
                throw new Exception("string is null or empty");

            var coordinates = coordinateId.Split(";");
            
            if (coordinates.Length < 2)
                throw new Exception("coordinate must be 3");

            return new Hex(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));
        }
    
        public override string ToString() => CoordinateId;
    }
}

