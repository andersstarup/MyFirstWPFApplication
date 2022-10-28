using MyFirstWPFApplication.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace MyFirstWPFApplication
{
    public partial class MainWindow : Window
    {


        [StructLayout(LayoutKind.Sequential, Pack = 4)] // creates the least padding, due to the program now does the most efficient allignment
        struct UdpPack
        {
            public byte addr;
            public byte op1;
            public int op2;
            public UInt16 Value;
        }
        static byte[] getBytes(UdpPack str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(str, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return arr;
        }

        UdpPack UdpOut = new UdpPack();

        UdpClient udpListener = new UdpClient(73);
        IPEndPoint ListenerEP = new IPEndPoint(IPAddress.Any, 73);
        //UdpClient UDPout = new UdpClient();
        IPEndPoint UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.0"), 72);

        Rootobject JsonOut = new Rootobject()
        {
            command = "",
            deviceID = 0,
            GoZero = 0,
            _params = new Params()
            {
                RPM = new UInt16(),
                deg = 0,
                dir = 0,
                toggles = 0
            },
        };

        int boardNr = 0;
        List<Board> boards = new();
        async Task listen()
        {
            while (true)
            {
                var dataRecieved = await udpListener.ReceiveAsync();
                byte[] array = dataRecieved.Buffer;
                string text = Encoding.UTF8.GetString(dataRecieved.Buffer);
                Scroller.Content += "Message from " + dataRecieved.RemoteEndPoint + ": " + text + Environment.NewLine;
                
                if (array[0] == '0')
                {
                    Waiting.Visibility = Visibility.Collapsed;
                    boardNr ++;
                    Board board = new Board();
                    board.B_ID = boardNr;
                    //boards.First(x => x.B_ID == 2);
                    /*
                    if (boardNr == 1)
                    {
                        Board1Selector.Visibility = Visibility.Visible;
                    }
                    if (boardNr == 2)
                    {
                        Board2Selector.Visibility = Visibility.Visible;
                    }
                    */

                    Scroller.Content += "Found a init message at board nr: " + boardNr + Environment.NewLine;
                    
                    if (array[1] == '1')
                    {
                        Scroller.Content += "This Board has a LED" + Environment.NewLine;
                        board.commands.Add("Led");
                    }

                    if (array[2] == '1')
                    {
                        Scroller.Content += "This Board has Stepper motor" + Environment.NewLine;
                        board.commands.Add("StepM");
                    }

                    if (array[3] == '1')
                    {
                        Scroller.Content += "This Board has Fork sensor" + Environment.NewLine;
                        board.commands.Add("Fork");
                    }
                    boards.Add(board);
                    //boardSelector.Items.Add("Board" + board.B_ID);
                }
                
                /*
                else if (array[0] == '1')
                {
                    Scroller.Content += "Not a init message" + Environment.NewLine;
                    array[0] = 140;
                } 
                */
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            var t = listen();
            //var msgOut = new Rootobject();
        }

        private void LED_Click(object sender, RoutedEventArgs e)
        {
            FuncSel.Content = "LED";
            Direction.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            Toggles.Visibility = Visibility.Visible;

            //var udpMsgO = new Rootobject;
            JsonOut.command = "LED";
            UdpOut.addr = 1;
        }

        private void StepM_Click(object sender, RoutedEventArgs e)
        {
            FuncSel.Content = "Stepper motor";
            DirectionStep.Content = "Choose a direction";
            Toggles.Visibility = Visibility.Collapsed;

            Direction.Visibility = Visibility.Visible;

            JsonOut.command = "StepM";
            UdpOut.addr = 2;
        }

        private void Fork_click(object sender, RoutedEventArgs e)
        {
            FuncSel.Content = "Fork sensor";
            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;

            JsonOut.command = "Fork";
            UdpOut.addr = 3;

            UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.255"), 72);

            Scroller.Content += "Target: " + UDPoutEP + Environment.NewLine;
            Scroller.ScrollToBottom();
        }

        private void ComTest_click(object sender, RoutedEventArgs e)
        {
            JsonOut.command = "ComTest";
            UdpOut.addr = 4;

            FuncSel.Content = "Test com";
            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            BoardSel.Content = "Stepper board";
            UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.123"), 72);
        }
        private void StepB_Click(object sender, RoutedEventArgs e)
        {
            DirectionStep.Content = "Counter Clockwise";
            UdpOut.op1 = 0;
            JsonOut._params.dir = 0;
            Freq.Visibility = Visibility.Visible;
            NumbRot.Visibility = Visibility.Visible;
        }
        private void StepF_Click(object sender, RoutedEventArgs e)
        {
            DirectionStep.Content = "Clockwise";
            UdpOut.op1 = 1;
            JsonOut._params.dir = 1;
            Freq.Visibility = Visibility.Visible;
            NumbRot.Visibility = Visibility.Visible;
        }
        private void GoZero_Click(object sender, RoutedEventArgs e)
        {
            DirectionStep.Content = "Go Zero";
            UdpOut.op1 = 2;
            JsonOut.GoZero = 1;
            Freq.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            using var UDPout = new UdpClient(71);
            try
            {
                var JsonOutSeri = JsonConvert.SerializeObject(JsonOut);
                var JsonOutByte = Encoding.UTF8.GetBytes(JsonOutSeri);


                UDPout.Connect(UDPoutEP);
                byte[] bytesent = getBytes(UdpOut);
                UDPout.Send(JsonOutByte, JsonOutByte.Length);
                UDPout.Close();

                Scroller.Content += JsonOutSeri + Environment.NewLine;
                Scroller.ScrollToBottom();
            }
            catch (Exception err)
            {
                Scroller.Content += (err.ToString());
                Scroller.ScrollToBottom();
            }
        }
        private void slRotations_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UdpOut.op2 = (int)slRotations.Value;
            JsonOut._params.dir = (int)slRotations.Value;
        }
        private void slFreq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UdpOut.Value = (UInt16)SlFreq.Value;
        //    JsonOut._params.RPM = (UInt16)SlFreq.Value;
        }
        private void slToggle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UdpOut.op1 = (byte)slToggle.Value;
            JsonOut._params.toggles = (byte)slToggle.Value;
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Scroller.Content = "";
            Scroller.ScrollToBottom();
        }
        private void StartSeq_Click(object sender, RoutedEventArgs e)
        {
            StartSeq.Visibility = Visibility.Collapsed;
            Waiting.Visibility = Visibility.Visible;
            boardNr = 0;
            var Start1 = new StartUP()
            {
                hej = "Hej, jeg er serveren. Venligst fortæl hvad du har med dig",
                Server = true
            };
            //Encoding.UTF8.GetString(dataRecieved.Buffer);
            var Start = JsonConvert.SerializeObject(Start1);
            var StartSeqMsg = Encoding.UTF8.GetBytes(Start);
            Scroller.Content += Start + Environment.NewLine;
            Scroller.ScrollToBottom();

            //JsonConvert.SerializeObject(Person2);

            //StartSeq.Visibility = Visibility.Collapsed;
            Waiting.Visibility = Visibility.Visible;

            UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.255"), 72);
            using var UDPout = new UdpClient(71);
            try
            {
                UDPout.Connect(UDPoutEP);
                UDPout.Send(StartSeqMsg, StartSeqMsg.Length);
                UDPout.Close();
            }
            catch (Exception err)
            {
                Scroller.Content += (err.ToString());
                Scroller.ScrollToBottom();
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if(boardSelector.Text == "Board1")
            {
                Scroller.Content += "Board 1" + Environment.NewLine ;
            }
            if (boardSelector.Text == "Board2")
            {
                Scroller.Content += "Board 2" + Environment.NewLine;
            }
            */
        }
    }
}


/* 
var Person = new Personer()
{
    Name = "Anders",
    age = 25
};

var Person2 = new Personer();

var Perso1 = JsonConvert.SerializeObject(Person);

Person2.age = 25;

var Perso2 = JsonConvert.SerializeObject(Person2);


Scroller.Content += Perso2 + Environment.NewLine;
Scroller.ScrollToBottom();*/









/*
private void Board1Selector_Click(object sender, RoutedEventArgs e)
{
    var board1 = boards.First(x => x.B_ID == 1);
    if (board1.commands.Contains("Led"))
    {
        LED.Visibility = Visibility.Visible;
    }

    if (board1.commands.Contains("StepM"))
    {
        StepM.Visibility = Visibility.Visible;
    }

    if (board1.commands.Contains("Fork"))
    {
        Fork.Visibility = Visibility.Visible;
    }

    BoardSel.Content = "Stepper board";
    UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.123"), 72);
    SendMessage.Visibility = Visibility.Visible;

    Scroller.Content += "Target: " + UDPoutEP + Environment.NewLine;
    Scroller.ScrollToBottom();

    Toggles.Visibility = Visibility.Collapsed;
    NumbRot.Visibility = Visibility.Collapsed;
    Direction.Visibility = Visibility.Collapsed;
    Freq.Visibility = Visibility.Collapsed;
}

private void Board2Selector_Click(object sender, RoutedEventArgs e)
{
    var board2 = boards.First(x => x.B_ID == 2);
    if (board2.commands.Contains("Led"))
    {
        LED.Visibility = Visibility.Visible;
    } else LED.Visibility = Visibility.Collapsed;

    if (board2.commands.Contains("StepM"))
    {
        StepM.Visibility = Visibility.Visible;
    } else StepM.Visibility = Visibility.Collapsed;

    if (board2.commands.Contains("Fork"))
    {
        Fork.Visibility = Visibility.Visible;
    } else Fork.Visibility = Visibility.Collapsed;


    BoardSel.Content = "LED Board";
    UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.124"), 72);
    SendMessage.Visibility = Visibility.Visible;

    Scroller.Content += "Target: " + UDPoutEP + Environment.NewLine;
    Scroller.ScrollToBottom();

    Toggles.Visibility = Visibility.Collapsed;
    NumbRot.Visibility = Visibility.Collapsed;
    Direction.Visibility = Visibility.Collapsed;
    Freq.Visibility = Visibility.Collapsed;
    ComTest.Visibility = Visibility.Collapsed;


    // LED.Visibility = Visibility.Visible;
    // Fork.Visibility = Visibility.Visible; 

    FuncSelect.Visibility = Visibility.Visible
}
*/
