using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWPFApplication.Classes;

public class OutJsonObj
{
    public string command { get; set; } = "";
    public int deviceID { get; set; }
    public int GoZero { get; set; }
    public Params _params { get; set; }
}
public class Params
{
    public ushort RPM { get; set; }
    public int deg { get; set; }
    public int dir { get; set; }
    public byte toggles { get; set; }
}
