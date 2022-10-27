using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWPFApplication.Classes;

public class Rootobject
{
    public string command { get; set; } = "";
    public int deviceID { get; set; }
    public int GoZero { get; set; }
    public Params _params { get; set; }
}
