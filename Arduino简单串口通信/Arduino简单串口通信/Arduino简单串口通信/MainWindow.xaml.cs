using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Arduino简单串口通信
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          
        }

        SerialPort Sp = new SerialPort();
        private void ReceiveData()
        {
            Sp.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);//触发事件，一旦有串口数据就激活函数
            
        }
            string s0 = "";
        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string s = "";
            int count = Sp.BytesToRead;//缓冲数据区数据的字节数

            byte[] data = new byte[count];//用于保存缓冲数据区的数据
            Sp.Read(data, 0, count);


            foreach (byte item in data)
            {
                s = Convert.ToString(item);
            }
                this.Dispatcher.Invoke(new Action(() =>
                {//委托操作GUI控件的部分

                    tbReceiveData.Text = s;   //textbox文字加上字符串


                }));


        }
 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sp.WriteLine("1");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sp.WriteLine("0");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Sp.BaudRate = 9600;
            Sp.PortName = "COM4";
            Sp.Open();
            ThreadStart threadStart = new ThreadStart(ReceiveData);//ThreadStart是一个委托，创建一个线程来在后台接收数据
            Thread thread = new Thread(threadStart);
            thread.Start();
        }
    }
}
