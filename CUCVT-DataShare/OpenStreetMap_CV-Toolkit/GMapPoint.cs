using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using System.Drawing;

namespace OpenStreetMap_CV_Toolkit
{
    class GMapPoint : GMap.NET.WindowsForms.GMapMarker
    {
        private PointLatLng point_;
        private float size_;
        public PointLatLng Point
        {
            get
            {
                return point_;
            }
            set
            {
                point_ = value;
            }
        }
        public GMapPoint(PointLatLng p, int size)
            : base(p)
        {
            point_ = p;
            size_ = size;
        }
        public override void OnRender(Graphics g)
        {
           // base.OnRender(g);
            //g.FillRectangle(Brushes.Black, LocalPosition.X, LocalPosition.Y, size_, size_);
            //OR 
            g.DrawEllipse(Pens.Red, LocalPosition.X, LocalPosition.Y, size_, size_);
        }
        
    }
}
