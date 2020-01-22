using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenStreetMap_CV_Toolkit
{
    public class RootObject
    {
        public float carid { get; set; }
        public int seq { get; set; }
        public string timestamp { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public float speed { get; set; }
    }
}
