using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWPFApplication.Classes;

public class Board
{
    public IPEndPoint b_IP { get; set; }
    public int b_id { get; set; } = 0;
    public string b_name { get; set; } = "";
    public Devices devices { get; set; }
}

public class Devices
{
    public List<Led> leds { get; set; }
    public List<StepM> stepMs { get; set; }
}

public class Led
{
    public int dev_nr { get; set; } = 0;
    public List<int> min_max { get; set; }
    public string name { get; set; } = "";
}

public class StepM
{
    public int dev_nr { get; set; } = 0;
    public List<int> min_max { get; set; }
    public string name { get; set; } = "";
}
