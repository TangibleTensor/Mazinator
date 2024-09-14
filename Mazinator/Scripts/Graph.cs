using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mazinator.Scripts
{
    internal class Graph
    {
        public Node[] nodes { get; private set; }
        public int graphWidth { get; private set; }
        public int graphHeight { get; private set; }

        Random random = new Random();

        // The connector holds the current formation of the maze (so all the nodes that are in the maze tree)
        List<Node> connector = new List<Node>();

        public Graph(int graphWidth, int graphHeight) 
        {
            this.graphWidth = graphWidth;
            this.graphHeight = graphHeight;
            this.nodes = new Node[graphWidth * graphHeight];
            Reset();
        }

        /// <summary>
        /// Given a coordinate, will find the node object in the nodes list. 
        /// Returns null if the coordinate aren't in the list.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public Node? FindNode(Coordinate coord)
        {
            if (coord.X < 0 || coord.X >= graphWidth || coord.Y < 0 || coord.Y >= graphHeight)
            {
                return null;
            }

            return nodes[coord.X * graphHeight + coord.Y];
        }

        /// <summary>
        /// Resets the maze structure by resetting nodes to empty nodes without any connections
        /// </summary>
        public void Reset()
        {
            for (int x = 0, i = 0; x < graphWidth; x++)
            {
                for (int y = 0; y < graphHeight; y++)
                {
                    this.nodes[i] = new Node(x, y);
                    i++;
                }
            }
        }

        /// <summary>
        /// Gives the free neighbours (i.e. the neighbouring nodes to a node which aren't in the connector) to a node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Node[] GetFreeNeighbours(Node node)
        {
            Node?[] nodesTo = { FindNode(node.position + new Coordinate(1, 0)), FindNode(node.position + new Coordinate(-1, 0)),
                               FindNode(node.position + new Coordinate(0, 1)), FindNode(node.position+ new Coordinate(0, -1)) };
            return nodesTo.Where(n => n != null && !connector.Contains(n)).ToArray();
        }

        /// <summary>
        /// Randomly connect the given node to one of its neighbours that isn't in the connector.
        /// Returns the node that just got connected if possible, else null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Node CreateRandomConnection(Node node)
        {
            Node[] nodesTo = GetFreeNeighbours(node);
            if (nodesTo.Length > 0) 
            {
                Node nodeTo = nodesTo[random.Next(0, nodesTo.Length)];
                Node.CreateConnection(node, nodeTo);
                connector.Add(nodeTo); 
                return nodeTo;
            }
            return null;
        }

        /// <summary>
        /// Picks a node from the connector which we can use to branch out the maze
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        Node GetNodeToBranchFrom(List<Node> connector)
        {
            foreach(Node node in connector)
            {
                Node[] freeNeighbours = GetFreeNeighbours(node);
                if (freeNeighbours.Length > 0)
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a maze using Depth First Search. Maze structure will be stored in the nodes field
        /// </summary>
        /// <param name="start"></param>
        public void CreateDfsMazeStatic(Node start)
        {
            connector = new List<Node>() { start };
            Node from = start;
            while (connector.Count != nodes.Length)
            {
                from = CreateRandomConnection(from);
                from ??= GetNodeToBranchFrom(connector);
            }
        }

        /// <summary>
        /// Using an IAsyncEnumerable to send nodes data every millisecond, so it looks like its animating
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<Node[]> CreateDfsMazeAnimation(Node start)
        {
            connector = new List<Node>() { start };
            Node from = start;
            while (connector.Count != nodes.Length)
            {
                from = CreateRandomConnection(from);
                await Task.Delay(1);
                yield return nodes;
                from ??= GetNodeToBranchFrom(connector);
            }
        }

        
    }
}
