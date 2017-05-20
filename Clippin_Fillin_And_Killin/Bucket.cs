using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clippin_Fillin_And_Killin
{
    public class Bucket
    {
        public Bucket(Edge edge)
        {
            this.dx = edge.To.X - edge.From.X;
            this.dy = edge.To.Y - edge.From.Y;
            this.yMax = Math.Max(edge.To.Y, edge.From.Y);
            this.yMin = Math.Min(edge.To.Y, edge.From.Y);
            this.startingX = edge.From.X;
            this.currentX = startingX;
           
            

        }
        public double yMax;
        public double yMin;
        public double currentX { get; set; }
        public double startingX { get; set; }
        public double dx;
        public double dy;
      
    }
}
