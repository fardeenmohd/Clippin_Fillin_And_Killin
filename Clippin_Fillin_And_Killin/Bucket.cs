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
            this.slopeInverse = (edge.To.X - edge.From.X) / (edge.To.Y - edge.From.Y);
            this.yMax = Math.Max(edge.To.Y, edge.From.Y);
            this.xMin = Math.Min(edge.To.X, edge.From.X);
            this.yMin = Math.Min(edge.To.Y, edge.From.Y);
            this.dx = edge.To.X - edge.From.X;
            this.dy = edge.To.Y - edge.From.Y;
            this.edge = edge;

        }
        public double yMax;
        public double xMin;
        public double yMin;
        public double sign;
        public double dx;
        public double dy;
        public double slopeInverse;
        public Edge edge;
    }
}
