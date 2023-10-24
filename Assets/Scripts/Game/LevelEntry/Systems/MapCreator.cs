using System;
using System.Collections.Generic;
using System.Linq;
using Game.Utils;
using HexLib;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Level.Systems
{
    public class MapCreator : IMapCreator
    {
        private Layout _layout;
        
        private ICustomLogger _logger;

        public MapCreator(Layout layout, ICustomLogger logger)
        {
            _logger = logger;
            _layout = layout;
        }
        
        public Dictionary<string, Hex> SpawnMap(GameObject gameObject, int left, int right, int top, int bottom, Material material)
        {
            // Need Layout offset update for change root position
            gameObject.transform.position = Vector3.zero;

            var meshRender = gameObject.AddComponent<MeshRenderer>();
            meshRender.sharedMaterial = material;

            var mesh = gameObject.AddComponent<MeshFilter>().mesh;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            Dictionary<string, Hex> hexMap = new Dictionary<string, Hex>();

            for (int r = top; r <= bottom; r++)
            {
                int r_offset = Mathf.FloorToInt(r / 2.0f);
                
                for (int q = left - r_offset; q <= right - r_offset; q++)
                {
                    var hex = new Hex(q, r, -q - r);
                    
                    if (hexMap.Values.ToList().Exists(x => x == hex))
                        _logger.LogError($"Value is exist q {hex.Q} r {hex.R} s {hex.S}");
                    
                    hexMap.Add(hex.CoordinateId, hex);
                    AddHex(hex,ref vertices, ref triangles);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;

            return hexMap;
        }
        
        public GameObject SpawnMap(Dictionary<string, Hex> hexMap, Material material)
        {
            var gameObject = new GameObject("HexMap");
            gameObject.transform.position = Vector3.zero;

            var meshRender = gameObject.AddComponent<MeshRenderer>();
            meshRender.sharedMaterial = material;

            var mesh = gameObject.AddComponent<MeshFilter>().mesh;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();


            foreach (var hex in hexMap.Values)
            {
                AddHex(hex,ref vertices, ref triangles);
            }
                
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;

            return gameObject;
        }

        private void AddHex(Hex h, ref List<Vector3> vertices, ref List<int> triangles, float y = 0)
        {
            var corners = _layout.PolygonCorners(h);

            var center = _layout.HexToPixel(h);
            center.y = y;

            for (int i = 0; i < corners.Count - 1; i++)
            {
                AddTriangle(
                    center,
                     new float3((float)corners[i].x, y, (float)corners[i].z),
                     new float3((float)corners[i + 1].x, y, (float)corners[i + 1].z), ref vertices, ref triangles);
            }

            AddTriangle(
                    center,
                     new float3((float)corners[corners.Count - 1].x, y, (float)corners[corners.Count - 1].z),
                     new float3((float)corners[0].x, y, (float)corners[0].z), ref vertices, ref triangles);
        }

        private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, ref List<Vector3> vertices, ref List<int> triangles)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
        }
    }

    public interface IMapCreator
    {
        Dictionary<string, Hex> SpawnMap(GameObject gameObject, int left, int right, int top, int bottom, Material material);
        GameObject SpawnMap(Dictionary<string, Hex> hexMap, Material material);
    }
}