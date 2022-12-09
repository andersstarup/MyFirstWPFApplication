using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWPFApplication.Classes;



public class Message
{
    public int msgType { get; set; }
    public dynamic payload { get; set; }
    // Prøv dynamic hvis object ikke virker
    // Først decerilize message, og tjek type derefter decerialize payload ind i board, som rå tekst.
}

public class Board
{
    public IPEndPoint b_IP { get; set; }
    public int b_id { get; set; } = 0;
    public string b_name { get; set; } = "";
    //public List<IDevice> devices { get; set; }
    public Device devices { get; set; }
}

public enum DeviceTypes
{
    Led,
    StepMotor,
}

public interface IDevice
{
    public string Name { get; }

    public DeviceTypes Type { get; }
}


public class Device
{
    public List<Led> leds { get; set; }
    public List<StepM> stepMs { get; set; }

    public IEnumerable<IDevice> GetAllDevices() => leds.Cast<IDevice>().Concat(stepMs);
}

public class Led : IDevice
{
    public int dev_nr { get; set; } = 0;
    public List<int> min_max { get; set; }
    public string Name { get; set; } = "";
    public DeviceTypes Type { get; } = DeviceTypes.Led;
}

public class StepM : IDevice
{
    public int dev_nr { get; set; } = 0;
    public List<int> min_max { get; set; }
    public string Name { get; set; } = "";
    public DeviceTypes Type { get; } = DeviceTypes.StepMotor;
}
