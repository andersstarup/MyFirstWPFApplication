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
        //  UdpClient Client = new UdpClient();
        //String Data = "";
        // IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);

        // IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 70);

        UdpClient udpListener;
        IPEndPoint ListenerEP;
        UdpClient UDPout = new UdpClient(70);
        IPEndPoint UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.0"), 70);

        void start()
        {
            //ListenerEP = new IPEndPoint(IPAddress.Any, 71);
            udpListener = new UdpClient(71);
            
            udpListener.BeginReceive(UDPReceiveCallback, null);
        }

        public MainWindow()
        {
            InitializeComponent();
            start();
            //Scroller.Content += "hej\n";
        }

        void UDPReceiveCallback(IAsyncResult result)
        {

            try
            {
                byte[] data = udpListener.EndReceive(result, ref ListenerEP);
                udpListener.BeginReceive(UDPReceiveCallback, null);
                string text1 = Encoding.UTF8.GetString(data);

                this.Dispatcher.Invoke(() =>
                {
                    Scroller.Content += "Message recieved from " + ": " + text1 + Environment.NewLine;
                    Scroller.Content += "Fuck Dis Shit \n";
                });

            }
            catch (Exception exit1)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Scroller.Content += exit1 + Environment.NewLine;
                });
            }
        }

        private void Board1Selector_Click(object sender, RoutedEventArgs e)
        {
            BoardSel.Content = "Stepper board";
            UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.123"), 70);
            SendMessage.Visibility =  Visibility.Visible;

            Scroller.Content += "Target: " + UDPoutEP + Environment.NewLine;
            Scroller.ScrollToBottom();

            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            LED.Visibility = Visibility.Visible;
            Fork.Visibility = Visibility.Visible;
            FuncSelect.Visibility = Visibility.Visible;

            //udpListener.BeginReceive(UDPReceiveCallback, null);

            //Client.BeginReceive(new AsyncCallback(recv), null);
             
        }

        private void Board2Selector_Click(object sender, RoutedEventArgs e)
        {
            BoardSel.Content = "LED Board";
            UDPoutEP = new IPEndPoint(IPAddress.Parse("192.168.1.124"), 70);
            SendMessage.Visibility = Visibility.Visible;

            Scroller.Content += "Target: " + UDPoutEP + Environment.NewLine;
            Scroller.ScrollToBottom();

            Toggles.Visibility = Visibility.Collapsed;
            NumbRot.Visibility = Visibility.Collapsed;
            Direction.Visibility = Visibility.Collapsed;
            Freq.Visibility = Visibility.Collapsed;
            LED.Visibility = Visibility.Visible;
            Fork.Visibility = Visibility.Visible;
            FuncSelect.Visibility = Visibility.Visible;


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

        private void StepM_Click(object sender, RoutedEventArgs e)
        {
            FuncSel.Content = "Stepper motor";
            DirectionStep.Content = "Choose a direction";
            Toggles.Visibility = Visibility.Collapsed;
            
            Direction.Visibility = Visibility.Visible;
            

            UdpOut.addr = 2;
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

        private void  Send_Click(object sender, RoutedEventArgs e)
        {
            //UdpClient Client = new UdpClient(70);
            try
            {

                UDPout.Connect(UDPoutEP);
                byte[] bytesent = getBytes(UdpOut);
                UDPout.Send(bytesent, bytesent.Length);
                UDPout.Close();

                //var dataRecieved = await Client.ReceiveAsync();
                //string text = Encoding.UTF8.GetString(dataRecieved.Buffer);

                // udpListener.Close();

                //Scroller.Content += "Message recieved from " + dataRecieved.RemoteEndPoint + ": " + text + Environment.NewLine;

                //dataRecieved.RemoteEndPoint

                //Scroller.Content += "Message recieved from " + clientEndPoint + ": " + text + Environment.NewLine;
                //Scroller.ScrollToBottom();
                //start();
                
               
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
