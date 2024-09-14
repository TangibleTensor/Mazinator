using System;
using System.Collections.Generic;

namespace Mazinator.Scripts
{
    /// <summary>
    /// Simple node structure (non-directional connections)
    /// </summary>
    internal class Node
    {
        public Coordinate position { get; private set; }
        public List<Node> connections { get; private set; }


        public Node(int x, int y)
        {
            this.position = new Coordinate(x, y);
            connections = new List<Node>();
        }


        bool AddConnection(Node node)
        {
            if (!connections.Contains(node))
            {
                connections.Add(node);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Connect two nodes to each other
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static bool CreateConnection(Node n1, Node n2)
        {
            return n1.AddConnection(n2) && n2.AddConnection(n1);
        }
    }
}
