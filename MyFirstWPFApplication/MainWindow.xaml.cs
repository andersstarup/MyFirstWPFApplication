using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;

using System.Runtime.Intrinsics.Arm;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Threading;

namespace MyFirstWPFApplication
{
    public partial class MainWindow : Window
    {
        int result;
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

        UdpClient Client = new UdpClient();
        UdpClient Client1 = new UdpClient();
        UdpClient Client2 = new UdpClient();
        
        IPEndPoint localEp;
        IPEndPoint localEpSTEP;
        IPEndPoint localEpLED;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Board1Selector_Click(object sender, RoutedEventArgs e)
        {
            BoardSel.Content = "Stepper board";
            localEp = new IPEndPoint(IPAddress.Parse("192.168.1.123"), 70);
            SendMessage.Visibility =  Visibility.Visible;

            Scroller.Content += "Target: " + localEp + Environment.NewLine;
            Scroller.ScrollToBottom();

            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            ComTest.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            LED.Visibility = Visibility.Visible;
            Fork.Visibility = Visibility.Visible;
            StepM.Visibility = Visibility.Visible;
            FuncSelect.Visibility = Visibility.Visible;
        }

        private void Board2Selector_Click(object sender, RoutedEventArgs e)
        {
            BoardSel.Content = "LED Board";
            localEp = new IPEndPoint(IPAddress.Parse("192.168.1.124"), 69);
            SendMessage.Visibility = Visibility.Visible;

            Scroller.Content += "Target: " + localEp + Environment.NewLine;
            Scroller.ScrollToBottom();

            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            ComTest.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            LED.Visibility = Visibility.Visible;
            Fork.Visibility = Visibility.Visible;
            StepM.Visibility = Visibility.Visible;
            FuncSelect.Visibility = Visibility.Visible;
        }

        private void BothBoards_click(object sender, RoutedEventArgs e)
        {
            ComTest.Visibility = Visibility.Visible;
            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            LED.Visibility = Visibility.Collapsed;
            Fork.Visibility = Visibility.Collapsed;
            StepM.Visibility = Visibility.Collapsed;
            FuncSelect.Visibility = Visibility.Visible;
        }

        private void StepM_Click(object sender, RoutedEventArgs e)
        {
            FuncSel.Content = "Stepper motor";
            DirectionStep.Content = "Choose a direction";
            Toggles.Visibility = Visibility.Collapsed;
            
            Direction.Visibility = Visibility.Visible;
            

            UdpOut.addr = 2;
        }

        private void LED_Click(object sender, RoutedEventArgs e)
        {
            FuncSel.Content = "LED";
            Direction.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            Toggles.Visibility = Visibility.Visible;


            UdpOut.addr = 1;
        }

        private void Fork_click(object sender, RoutedEventArgs e)
        {
            FuncSel.Content = "Fork sensor";
            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;

            UdpOut.addr = 3;
        }

        private void Com_test_click(object sender, RoutedEventArgs e)
        {
            
            FuncSel.Content = "Com Test"; 
           
            localEpSTEP  = new IPEndPoint(IPAddress.Parse("192.168.1.123"), 70);
            localEpLED = new IPEndPoint(IPAddress.Parse("192.168.1.124"), 69);

            string text;
            //fvar dataRecieved = localEpLED;
            byte[] bytesent;

            /////////////////////////////////////////////////////////// SEND AND RECIEVE TIL STEP BOARD
            UdpOut.addr = 2;
            UdpOut.op1 = 2;

            UdpClient Client = new UdpClient();
            try
            {
                
                Client.Connect(localEpSTEP);
                bytesent = getBytes(UdpOut);
                Client.Send(bytesent, bytesent.Length);

                var dataRecieved = Client.Receive(ref localEpSTEP);
                text = Encoding.UTF8.GetString(dataRecieved);

                Client.Close();

                Scroller.Content += text + Environment.NewLine;

                Scroller.ScrollToBottom();
            }
            catch (Exception err)
            {
                Scroller.Content += (err.ToString());
                Scroller.ScrollToBottom();
            }
            
            ///////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////// SEND AND RECIEVE TIL LED BOARD'

            // while (text != "Go To Zero") ;
            UdpClient Client1 = new UdpClient();
            try
            {
                UdpOut.addr = 4;
                Client1.Connect(localEpLED);
                bytesent = getBytes(UdpOut);
                Client1.Send(bytesent, bytesent.Length);

                var dataRecieved = Client1.Receive(ref localEpLED);
                result = Environment.TickCount;
                text = Encoding.UTF8.GetString(dataRecieved);

                Client1.Close();


                Scroller.Content += text + Environment.NewLine;

                Scroller.ScrollToBottom();
            }
            catch (Exception err)
            {
                Scroller.Content += (err.ToString());
                Scroller.ScrollToBottom();
            }

            ///////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////// SEND AND RECIEVE TIL STEP BOARD
            ///
          UdpClient Client2 = new UdpClient();
            try
            {
               // while (text != "Har fået int") ;
                Client2.Connect(localEpSTEP);
                bytesent = getBytes(UdpOut);
                Client2.Send(bytesent, bytesent.Length);
                result = Environment.TickCount & Int32.MaxValue;

                var dataRecieved = Client2.Receive(ref localEpSTEP);
                text = Encoding.UTF8.GetString(dataRecieved);

                Client2.Close();


                Scroller.Content += text + Environment.NewLine;

                Scroller.ScrollToBottom();
            }
            catch (Exception err)
            {
                Scroller.Content += (err.ToString());
                Scroller.ScrollToBottom();
            }

            ///////////////////////////////////////////////////////////
        }

        private void StepB_Click(object sender, RoutedEventArgs e)
        {
            DirectionStep.Content = "Counter Clockwise";
            UdpOut.op1 = 0;
            Freq.Visibility = Visibility.Visible;
            NumbRot.Visibility = Visibility.Visible;
        }

        private void StepF_Click(object sender, RoutedEventArgs e)
        {
            DirectionStep.Content = "Clockwise";
            UdpOut.op1 = 1;
            Freq.Visibility = Visibility.Visible;
            NumbRot.Visibility = Visibility.Visible;

        }

        private void GoZero_Click(object sender, RoutedEventArgs e)
        {
            DirectionStep.Content = "Go Zero"; 
            UdpOut.op1 = 2;
            Freq.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
        }

        private async void  Send_Click(object sender, RoutedEventArgs e)
        {

            UdpClient Client = new UdpClient();
            try
            {
                
                Client.Connect(localEp);
                byte[] bytesent = getBytes(UdpOut);
                Client.Send(bytesent, bytesent.Length);

                var dataRecieved = await Client.ReceiveAsync();
                string text = Encoding.UTF8.GetString(dataRecieved.Buffer);

                Client.Close();


                Scroller.Content += "Message recieved from " + dataRecieved.RemoteEndPoint + ": " + text + Environment.NewLine;

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
           // Debug.WriteLine(hej);
        }

        private void slFreq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UdpOut.Value = (UInt16)SlFreq.Value;
        }

        private void slToggle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UdpOut.op1 = (byte)slToggle.Value;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Scroller.Content = "";
            Scroller.ScrollToBottom();
        }
    }
}
