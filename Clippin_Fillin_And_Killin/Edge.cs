using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clippin_Fillin_And_Killin
{
    public class Edge
    {

        public Edge(Point from, Point to)
        {
            this.From = from;
            this.To = to;
            
        }

        public readonly Point From;
        public readonly Point To;
  

    }
}
