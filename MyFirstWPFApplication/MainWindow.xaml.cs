using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Runtime.Intrinsics.Arm;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Threading;
using System.Net.Security;
using System.Windows.Markup;
using System.Xml.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

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
        // int i = 0;
        // int?[] B1CmdP;
        // int?[] B2CmdP;

        Rootobject JsonOut = new Rootobject();




        async Task listen()
        {
            while (true)
            {
                var dataRecieved = await udpListener.ReceiveAsync();
                string text = Encoding.UTF8.GetString(dataRecieved.Buffer);
                Scroller.Content += "Message from " + dataRecieved.RemoteEndPoint + ": " + text + Environment.NewLine;

                /*
                if (StartMsg = True)
                { // skal rettes så det passer med Mathias 
                    i++;
                    Waiting.Visibility = Visibility.Collapsed;
                    BoardSel.Content = "Choose a board:";
                    if (i == 1)
                    {
                        if (text == "cmd1") // skal rettes så det passer med Mathias
                        {
                            Board1Selector.Content = "Board" + i;
                            B1CmdP[1] = 1;
                        }
                        if (text == "cmd2") // skal rettes så det passer med Mathias
                        {
                            B1CmdP[2] = 1;
                        }
                        if (text == "cmd3") // skal rettes så det passer med Mathias
                        {
                            B1CmdP[3] = 1;
                        }
                        Board1Selector.Visibility = Visibility.Visible;
                    }

                    if (i == 2)
                    {
                        if (text == "cmd1") // skal rettes så det passer med Mathias
                        {
                            Board2Selector.Content = "Board" + i;
                            B2CmdP[1] = 1;
                        }
                        if (text == "cmd2") // skal rettes så det passer med Mathias
                        {
                            B2CmdP[2] = 1;
                        }
                        if (text == "cmd3") // skal rettes så det passer med Mathias
                        {
                            B2CmdP[3] = 1;
                        }
                        Board2Selector.Visibility = Visibility.Visible;
                    }
                    // Board1Selector.Visibility = Visibility.Visible;
                    // Board2Selector.Visibility = Visibility.Visible;
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

        private void Board1Selector_Click(object sender, RoutedEventArgs e)
        {
            BoardSel.Content = "Stepper board";
            UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.123"), 72);
            SendMessage.Visibility = Visibility.Visible;

            Scroller.Content += "Target: " + UDPoutEP + Environment.NewLine;
            Scroller.ScrollToBottom();

            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;

            /*
            LED.Visibility = Visibility.Visible;
            Fork.Visibility = Visibility.Visible;
            ComTest.Visibility = Visibility.Visible;
            */
            FuncSelect.Visibility = Visibility.Visible;
            
            
            /*

            if (B1CmdP[1] == 1)
            {
                LED.Visibility = Visibility.Visible;
            }
            if (B1CmdP[2] == 1)
            {
                Fork.Visibility = Visibility.Visible;
            }
            if (B1CmdP[3] == 1)
            {
                StepM.Visibility = Visibility.Visible;
            }
            */
        }

        private void Board2Selector_Click(object sender, RoutedEventArgs e)
        {
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

            /*
            LED.Visibility = Visibility.Visible;
            Fork.Visibility = Visibility.Visible; 
            */
            FuncSelect.Visibility = Visibility.Visible;

            /*
            if (B2CmdP[1] == 1)
            {
                LED.Visibility = Visibility.Visible;
            }
            if (B2CmdP[2] == 1)
            {
                Fork.Visibility = Visibility.Visible;
            }
            if (B2CmdP[3] == 1)
            {
                StepM.Visibility = Visibility.Visible;
            }
            */
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
            var Start1 = new StartUP()
            {
                hej = "Hej, jeg er serveren. Venligst fortæl hvad du har med dig",
                Server = true
            };
            //Encoding.UTF8.GetString(dataRecieved.Buffer);
            var Start = JsonConvert.SerializeObject(Start1);
            var StartSeqMsg = Encoding.UTF8.GetBytes(Start);
            Scroller.Content += Start;
            Scroller.ScrollToBottom();

            //JsonConvert.SerializeObject(Person2);

            StartSeq.Visibility = Visibility.Collapsed;
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
    }

    public class Rootobject
    {
        public string command { get; set; }
        public int deviceID { get; set; }
        public int GoZero { get; set; }
        public Params _params { get; set; }
    }

    public class Params
    {
        public UInt16 RPM { get; set; }
        public int deg { get; set; }
        public int dir { get; set; }
        public byte toggles { get; set; }
    }

    public class StartUP
    {
        public bool Server { get; set; }

        public string hej { get; set; }
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