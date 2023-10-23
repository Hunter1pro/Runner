using System.Collections.Generic;
using System.Linq;

namespace HexLib
{
    public class HexPathfinding<TNode> where TNode : IPathNode
    {
        private List<PathNode> _openList;
        private List<TNode> _reachList;

        private Dictionary<string,PathNode> _pathNodes;

        private IGetNeigbours<TNode> _getNeighbours;

        public class PathNode
        {
            public TNode Node { get; set; }
            public int GCost { get; set; }
            public int HCost { get; set; }
            public int FCost { get; private set; }

            public PathNode ComeFrom { get; set; }

            public void CalculateFCost()
            {
                FCost = GCost + HCost;
            }
        }

        public HexPathfinding(IGetNeigbours<TNode> getNeigbours)
        {
            _getNeighbours = getNeigbours;
        }

        public List<TNode> FindPath(TNode start, TNode goal, List<TNode> nodes)
        {
            _pathNodes = new Dictionary<string, PathNode>();
            _pathNodes = nodes.Select(x => new KeyValuePair<string, PathNode>(x.CoordinateId, new PathNode { Node = x })).ToDictionary(x => x.Key, x => x.Value);

            foreach (var node in _pathNodes)
            {
                node.Value.GCost = int.MaxValue;
                node.Value.CalculateFCost();
            }

            var startNode = new PathNode { Node = start, HCost = int.MaxValue };
            startNode.CalculateFCost();

            _openList = new List<PathNode>() { startNode };

            _reachList = new List<TNode>() { start };

            while(_openList.Count > 0)
            {
                var current = GetLowestFCostNode(_openList);

                if (current.Node.CoordinateId == goal.CoordinateId)
                {
                    return CalculatePath(start, current);
                }

                _openList.Remove(current);
                _reachList.Add(current.Node);

                foreach (var neighbour in _getNeighbours.GetNeighbours(current.Node))
                {
                    if (_reachList.Contains(neighbour)) 
                        continue;

                    _reachList.Add(neighbour);

                    int tentativeCost = current.GCost + _getNeighbours.CalculateDistance(current.Node, neighbour);

                    if(_pathNodes.TryGetValue(neighbour.CoordinateId, out var neigbourValue))
                    {
                        if (tentativeCost < neigbourValue.GCost)
                        {
                            neigbourValue.ComeFrom = current;
                            neigbourValue.GCost = tentativeCost;
                            neigbourValue.HCost = _getNeighbours.CalculateDistance(current.Node, neighbour);
                            neigbourValue.CalculateFCost();

                            if (!_openList.Contains(neigbourValue))
                                _openList.Add(neigbourValue);
                        }
                    }
                }
            }

            return null;
        }

        private List<TNode> CalculatePath(TNode start, PathNode goal)
        {
            List<TNode> path = new List<TNode>();

            var current = goal;
            while (current.Node.CoordinateId != start.CoordinateId)
            {
                path.Add(current.Node);
                current = current.ComeFrom;
            }

            path.Reverse();

            return path;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];

            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }

            return lowestFCostNode;
        }
    }

    public interface IGetNeigbours<TNode>
    {
        List<TNode> GetNeighbours(TNode point);

        int CalculateDistance(TNode from, TNode To);
    }

    public interface IPathNode
    {
        public string CoordinateId { get; }
    }
}

