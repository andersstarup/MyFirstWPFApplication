using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWPFApplication.Classes;

public class Board
{
    public int B_ID { get; set; }
    public string IP_Addr { get; set; }
    public int type { get; set; }
    // public commands _commands { get; set; }
    public List<string> commands { get; set; } = new();
    public string B_Name
    {
        get
        {
            return $"Board {B_ID}";
        }
    }
}
