using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaCaelum.Models
{
    internal class ModelStatus
    {
        public byte ready { get; set; }
        public byte takeOff { get; set; }
        public byte ascent { get; set; }
        public byte firstDeploy { get; set; }
        public byte drag { get; set; }
        public byte secondDeploy { get; set; }
        public byte descent { get; set; }
        public byte landed { get; set; }
    }
}
