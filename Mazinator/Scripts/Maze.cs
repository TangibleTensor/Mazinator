using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mazinator.Scripts
{
    internal class Maze
    {
        WriteableBitmap bm;
        public Color pathColour;
        public Color bgColour;
        Graph graph;

        int mazeWidth;
        int mazeHeight;
        Random random = new Random();


        public Maze(int mazeWidth, int mazeHeight, WriteableBitmap bm, Color pathColour, Color bgColour)
        {
            this.mazeWidth = mazeWidth;
            this.mazeHeight = mazeHeight;

            // This is so that there is a gap of one in the actual maze when generating.
            graph = new Graph((mazeWidth-2) / 2 + 1, (mazeHeight-2) / 2 + 1);

            this.pathColour = pathColour;
            this.bgColour = bgColour;
            this.bm = bm;

            ResetMaze();
        }
        

        public void ClearCanvas()
        {
            bm.FillRectangle(0, 0, mazeWidth, mazeHeight, bgColour);
        }

        public void ResetMaze()
        {
            graph.Reset();
            ClearCanvas();        
        }

        /// <summary>
        /// Renders the actual bitmap image from the maze structure in the graph object
        /// </summary>
        /// <param name="nodes"></param>
        public void CreateMazeFromNodes(Node[] nodes) 
        {
            ClearCanvas();  
            foreach (Node node in nodes)
            {
                foreach (Node con in node.connections)
                {
                    Coordinate mazeCoord1 = 2 * node.position + new Coordinate(1, 1);
                    Coordinate mazeCoord2 = 2 * con.position + new Coordinate(1, 1);
                    bm.DrawRectangle(mazeCoord1.X, mazeCoord1.Y, mazeCoord2.X, mazeCoord2.Y, pathColour);
                }
            }
        }

        int GetRandomMazeYCoord()
        {
            int[] y_coords = Enumerable.Range(1, (mazeHeight - 2) / 2).ToArray();
            return y_coords[random.Next(y_coords.Length)] * 2 + 1;
        }

        public async Task MakeMaze(bool? animate=false)
        {
            ResetMaze();
            Coordinate graphPosition = new (random.Next(graph.graphWidth), random.Next(graph.graphHeight));

            Node? start = graph.FindNode(graphPosition);
            if (start != null)
            {
                // Have to explicitely compare to true because of the nullability of animate var
                if (animate == true)
                {
                    await foreach (Node[] nodes in graph.CreateDfsMazeAnimation(start))
                    {
                        CreateMazeFromNodes(nodes);
                    }
                }
                else
                {
                    graph.CreateDfsMazeStatic(start);
                    CreateMazeFromNodes(graph.nodes); 
                } 
            }

            // Finish with creating the entrance and exit gaps in the maze
            bm.SetPixel(0, GetRandomMazeYCoord(), pathColour);
            bm.SetPixel(mazeWidth - 1, GetRandomMazeYCoord(), pathColour);
        }
    }
}
