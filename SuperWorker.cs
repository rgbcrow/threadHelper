using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker
{
    public class SuperWorker : BackgroundWorker
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}
