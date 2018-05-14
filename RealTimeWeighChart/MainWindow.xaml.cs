using RealTimeWeighChart.VoltageViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
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
using System.Windows.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.Drawing;
using System.Threading;
using Microsoft.Win32;

namespace RealTimeWeighChart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker stoper;
        private List<string> textlist = new List<string>();
        int weigh_count = 0;
        private List<String> _property;
        public List<String> Property
        {
            get
            {
                return _property;
            }
            set
            {
                _property = value;
            }
        }
        public bool PortIsOpened
        {
            get
            {
                return serialPort.IsOpen;
            }
        }
        System.Threading.Semaphore SemaphoreDataReceived = new System.Threading.Semaphore(1, 1);
        
        List<byte> result;
        
        public long mesuValue = 0;
        public static SerialPort serialPort;
        public static int SelectedRoutDestination = 1;
        public static string PortName_Serial = "COM1";
        public static int DataBits_Serial = 8;
        public static int SerialBaudRate_Serial = 38400;
        //public static int SerialBaudRate_Serial = 38400;
        public static int ReadTimeout_Serial = 500;
        public static int WriteTimeout_Serial = 500;
        public static int sizeOfReceivedTransPacket = 8;
        public Dictionary<int, DateTime> mesuredValue;
        public int sayac = 0;
        char sending = 'A';
        public enum CommandType
        {
            StartingProtocol = 0x41, // 'A'
            NoMove_NoTare = 0x41,
            NoMove_Tare = 0x42,
            Move_NoTare = 0x43,
            Move_Tare = 0x44,
            Exceed = 0x45,
            Negative = 0x2D,
            CarriageReturn = 0x0D,
            EndOfProtocol = (byte)'X',
            GainOffsetStart = (byte)'/',
            GainOffsetEnd = (byte)'_',
        }
        private int _maxVoltage;
        public int MaxVoltage
        {
            get { return _maxVoltage; }
            set { _maxVoltage = value; this.OnPropertyChanged("MaxVoltage"); }
        }

        private int _minVoltage;
        public int MinVoltage
        {
            get { return _minVoltage; }
            set { _minVoltage = value; this.OnPropertyChanged("MinVoltage"); }
        }



        public VoltagePointCollection voltagePointCollection;
        public VoltagePointCollection voltagepoint1;
        public VoltagePointCollection voltagepoint2;
        public VoltagePointCollection voltagepoint3;
        DispatcherTimer updateCollectionTimer;
        private int i = 0;

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        void updateCollectionTimer_Tick(object sender, EventArgs e)
        {
            i++;
            voltagePointCollection.Add(new VoltagePoint(Math.Sin(i * 0.1), DateTime.Now));/*DateTime.Now*/
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion
        public MainWindow()
        {
            mesuredValue = new Dictionary<int, DateTime>();
            SerialPortService.PortsChanged += SerialPortService_PortsChanged;

            serialPort = new SerialPort();

            serialPort.Disposed += SerialPort_Disposed;
            serialPort.PortName = PortName_Serial;
            serialPort.BaudRate = SerialBaudRate_Serial;
            serialPort.Handshake = Handshake.None;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = DataBits_Serial;
            serialPort.StopBits = StopBits.One;
            serialPort.RtsEnable = true;
            serialPort.DtrEnable = true;
            serialPort.DataReceived += serial_DataReceived;

            serialPort.PortName = "COM3";
            serialPort.ReadTimeout = ReadTimeout_Serial;
            serialPort.WriteTimeout = WriteTimeout_Serial;

            InitializeComponent();
            this.Closed += MainWindow_Closed;

            (closePort_BTN).Background = System.Windows.Media.Brushes.Red;

            this.DataContext = this;

            voltagePointCollection = new VoltagePointCollection();
           
            updateCollectionTimer = new DispatcherTimer();
            updateCollectionTimer.Interval = TimeSpan.FromMilliseconds(100);
            updateCollectionTimer.Tick += new EventHandler(updateCollectionTimer_Tick);
            //updateCollectionTimer.Start();

            var ds = new EnumerableDataSource<VoltagePoint>(voltagePointCollection);
            ds.SetXMapping(x => dateAxis.ConvertToDouble(x.Date));
            ds.SetYMapping(y => y.Voltage);
            plotter.AddLineGraph(ds, Colors.Green, 2, "Volts"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
            MaxVoltage = 1;
            MinVoltage = -1;
        }
        private void SerialPort_Disposed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void SerialPortService_PortsChanged(object sender, PortsChangedArgs e)
        {
            try
            {
                string[] allPortNames = System.IO.Ports.SerialPort.GetPortNames();
                _property = allPortNames.ToList();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    comSelect_CmBx.ItemsSource = _property;
                    if (!serialPort.IsOpen)
                    {
                        (openPort_BTN).Background = System.Windows.Media.Brushes.LightGray;
                        (closePort_BTN).Background = System.Windows.Media.Brushes.Red;
                    }
                }));



            }
            catch (Exception ex)
            {

                MessageBox.Show("Error In COM Port : " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Close();
                    serialPort.Dispose();
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SemaphoreDataReceived.WaitOne();

                
                SerialPort mySerialP = (SerialPort)sender;
                result = new List<byte>();
                byte[] firstReadData = new byte[mySerialP.BytesToRead];
                List<byte> metaData = new List<byte>();
                List<byte> lastReceivedTranMsg = new List<byte>();

                List<char> myMetaData = new List<char>();


                mySerialP.Read(firstReadData, 0, firstReadData.Length);
                metaData.AddRange(firstReadData);


                string datas = System.Text.Encoding.UTF8.GetString(firstReadData);
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                {
                    textboxSP.Text += (datas)+"\n";
                    textboxSP.ScrollToEnd();
                    //hercul.listboxSP.SelectedIndex = hercul.listboxSP.Items.Count - 1;
                     ListBoxAutomationPeer svAutomation = (ListBoxAutomationPeer)ScrollViewerAutomationPeer.CreatePeerForElement(listboxSP);

                     IScrollProvider scrollInterface = (IScrollProvider)svAutomation.GetPattern(PatternInterface.Scroll);
                     System.Windows.Automation.ScrollAmount scrollVertical = System.Windows.Automation.ScrollAmount.LargeIncrement;
                     System.Windows.Automation.ScrollAmount scrollHorizontal = System.Windows.Automation.ScrollAmount.NoAmount;
                     //If the vertical scroller is not available, the operation cannot be performed, which will raise an exception. 
                     if (scrollInterface.VerticallyScrollable)
                         scrollInterface.Scroll(scrollHorizontal, scrollVertical);
                });
                int indexCounter = 0;
                for (int i = 0; i < metaData.Count; i++)
                {

                    lastReceivedTranMsg = new List<byte>();
                    if (metaData.Contains((byte)CommandType.GainOffsetStart)) 
                    {
                        if (metaData.Contains((byte)CommandType.GainOffsetEnd)) 
                        {
                            int startIndex = metaData.IndexOf((byte)CommandType.StartingProtocol);
                            int endIndex = metaData.IndexOf((byte)CommandType.EndOfProtocol);
                            for (int j = startIndex+1; j < metaData.Count; j++)
                            {
                                if(metaData[j]==(byte)CommandType.GainOffsetStart)
                                {
                                    lastReceivedTranMsg.Clear();
                                    continue;
                                }
                                else if (metaData[j] == (byte)CommandType.GainOffsetEnd) 
                                {
                                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
                                    {
                                        if(!Harf.Content.ToString().Contains(ByteArrayToString(lastReceivedTranMsg.ToArray())))
                                    Harf.Content = Harf.Content + " " + ByteArrayToString(lastReceivedTranMsg.ToArray());
                                    lastReceivedTranMsg.Clear();
                                    });
                                    
                                }
                                lastReceivedTranMsg.Add(metaData[j]);
                            }
                        }
                    }
                    if (metaData.Contains((byte)CommandType.StartingProtocol))
                    {
                        if (metaData.Contains((byte)CommandType.EndOfProtocol))
                        {
                            int startIndex = metaData.IndexOf((byte)CommandType.StartingProtocol);
                            int endIndex = metaData.IndexOf((byte)CommandType.EndOfProtocol);
                            if (startIndex < endIndex)
                            {
                                for (int j = startIndex; j < metaData.Count; j++)
                                {
                                    indexCounter++;
                                    if (metaData[j] == (byte)CommandType.EndOfProtocol)
                                    {
                                        lastReceivedTranMsg.Add(metaData[j]);
                                        metaData.RemoveRange(0, startIndex + endIndex);
                                        break;
                                    }
                                    lastReceivedTranMsg.Add(metaData[j]);

                                }
                                myMetaData = System.Text.ASCIIEncoding.ASCII.GetChars(lastReceivedTranMsg.ToArray()).ToList();

                                string myStringValue = "";
                                for (int j = 1; j < myMetaData.Count - 1; j++)
                                {
                                    myStringValue += myMetaData[j];
                                }
                                mesuValue = (Convert.ToInt64(myStringValue));
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    textlist.Add("" + mesuValue);
                                    listboxSP.Items.Add(textlist[textlist.Count - 1]);
                                    voltagePointCollection.Add(new VoltagePoint(mesuValue, DateTime.Now));


                                }));
                                i += indexCounter;
                            }
                            else
                            {
                                metaData.RemoveRange(0, startIndex);

                                byte[] secondReadData;
                                do
                                {
                                    secondReadData = new byte[mySerialP.BytesToRead];

                                    mySerialP.Read(secondReadData, 0, secondReadData.Length);
                                } while (!secondReadData.Contains((byte)CommandType.EndOfProtocol));

                                metaData.AddRange(secondReadData);

                                for (int j = startIndex; j < metaData.Count; j++)
                                {
                                    if (metaData[j] == (byte)CommandType.EndOfProtocol)
                                    {
                                        lastReceivedTranMsg.Add(metaData[j]);
                                        endIndex = metaData.IndexOf((byte)CommandType.EndOfProtocol);
                                        metaData.RemoveRange(0, startIndex + endIndex);
                                        break;
                                    }
                                    lastReceivedTranMsg.Add(metaData[j]);

                                }
                                myMetaData = System.Text.ASCIIEncoding.ASCII.GetChars(lastReceivedTranMsg.ToArray()).ToList();

                                string myStringValue = "";
                                for (int j = 0; j < myMetaData.Count - 1; j++)
                                {
                                    myStringValue += myMetaData[j];
                                }
                                mesuValue = (Convert.ToInt64(myStringValue));

                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    textlist.Add("" + mesuValue);
                                    listboxSP.Items.Add(textlist[textlist.Count - 1]);
                                    voltagePointCollection.Add(new VoltagePoint(mesuValue, DateTime.Now));
                                    
                                    
                                }));
                                i += indexCounter;


                            }

                        }
                }
            }
            SemaphoreDataReceived.Release();
            }
            catch (Exception ex)
            {
                try
                {
                    SemaphoreDataReceived.Release();
                }
                catch (Exception)
                {

                    //throw;
                }
                

                //throw;
            }
        }
        private void comSelect_LB_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                string[] allPortNames = System.IO.Ports.SerialPort.GetPortNames();
                _property = allPortNames.ToList();
                comSelect_CmBx.ItemsSource = _property;
                //comSelect_CmBx.Items.Clear();
                //foreach (var portItem in allPortNames)
                //{
                //    comSelect_CmBx.Items.Add(portItem);

                //}
                //if(comSelect_CmBx.Items.Count > 0)
                //{
                //    comSelect_CmBx.SelectedIndex = 0;
                //}

            }
            catch (Exception)
            {

                MessageBox.Show("Error In COM Port", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void openPort_Click(object sender, RoutedEventArgs e)
        {
            
                
                if (serialPort.IsOpen)
                {
                    MessageBox.Show("Your Port is opened !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
                    {
                        //stoper = new BackgroundWorker();
                        //stoper.DoWork += stoper_DoWork;
                        //stoper.RunWorkerAsync();
                        //Thread ctrl = new Thread(new ThreadStart(control));
                        //ctrl.Start();
                        serialPort.PortName = comSelect_CmBx.SelectedValue.ToString();
                        //textlist.Add("" + weigh_name());
                        //listboxSP.Items.Add(textlist[textlist.Count - 1]);
                        serialPort.Open();
                        //serialPort.Write("" + sending);
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                        (openPort_BTN).Background = System.Windows.Media.Brushes.Green;
                        (closePort_BTN).Background = System.Windows.Media.Brushes.LightGray;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error in opening COM Port", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            



        }

        void stoper_DoWork(object sender, DoWorkEventArgs e)
        {
            control();
        }
        

        private void closePort_Click(object sender, RoutedEventArgs e)
        {

            close();
            //screenshot();

            //Harf.Content = sending; 
            
            voltagePointCollection.Clear(); 
        }
        void close()
        {
            // hercul.Hide();
            // sayac = 0; counter.Content = "" + sayac;
            // sending = 'A'; Harf.Content = "" + sending;

            if (!serialPort.IsOpen)
            {
                MessageBox.Show("Your Port is already closed !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {

                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    Thread xyz = new Thread(new ThreadStart(closer));
                    xyz.Start();
                    (openPort_BTN).Background = System.Windows.Media.Brushes.LightGray;
                    (closePort_BTN).Background = System.Windows.Media.Brushes.Red;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error in closing COM Port", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }
        void closer() {
            try 
            {
                serialPort.Close();
            }
            catch { }
        }
        private void startCount_BTN_Click(object sender, RoutedEventArgs e)
        {
            mesuredValue = new Dictionary<int, DateTime>();
        }

        private void endCount_BTN_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, DateTime> checkedValue = new Dictionary<int, DateTime>();

            foreach (var meauseredItem in mesuredValue)
            {
                
            }
        }
        private void screenshot()
        {

            double screenLeft = Application.Current.MainWindow.Left;
            double screenTop = Application.Current.MainWindow.Top;
            double screenWidth = Application.Current.MainWindow.Width;
            double screenHeight = Application.Current.MainWindow.Height;

            using (Bitmap bmp = new Bitmap((int)screenWidth, (int)screenHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    String filename = "" + sending + "-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                    Opacity = .0;
                    g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                    bmp.Save("ScreenShots\\" + filename);
                    Opacity = 1;
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Kapatmak istediğinize emin misiniz?", "Kapanıyor", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        void sendfixed()
        {
            if (sending == 'Z')
            {
                sending = 'a';
            }
            else if (sending == 'z')
            {
                weigh_count++;
                sending = 'A';
                textlist.Add("" + weigh_name());
                listboxSP.Items.Add(textlist);

                stoper = new BackgroundWorker();
                stoper.DoWork += stoper_DoWork;
                stoper.RunWorkerCompleted += stoper_RunWorkerCompleted;
                stoper.RunWorkerAsync();
            }
            else sending++;
            Harf.Content = "";
        }

        void stoper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(!weigh_name().Equals("0kg"))
                openport();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
        string weigh_name()
        {
            if (weigh_count == 1) return "5kg";
            else if (weigh_count == 2) return "25kg";
            else return "0kg";
        }
        System.Threading.Semaphore SemaphoreDataControl = new System.Threading.Semaphore(1, 1);
        public void control()
        {

           

                { 
                    

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                     saveFileDialog1.ShowDialog();
                string filepath = saveFileDialog1.FileName;
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(filepath + ".txt"))
                {
                    foreach (string line in textlist)
                    {


                        file.WriteLine(line);

                    }
                }
                textlist.Clear();
                listboxSP.Items.Clear();
                MessageBox.Show("İşlem Tamamlandı.");
                     
                    /*break;*/ }


        }
        void openport()
        {
            
                if (serialPort.IsOpen)
                {
                    MessageBox.Show("Your Port is opened !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    try
                    {

                        serialPort.PortName = comSelect_CmBx.SelectedValue.ToString();

                        serialPort.Open();
                        
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                        (openPort_BTN).Background = System.Windows.Media.Brushes.Green;
                        (closePort_BTN).Background = System.Windows.Media.Brushes.LightGray;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error in opening COM Port", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
            
        }

        private void ugur_Click(object sender, RoutedEventArgs e)
        {
            
            BackgroundWorker x = new BackgroundWorker();
            x.DoWork += x_DoWork;
            x.RunWorkerCompleted += x_RunWorkerCompleted;
            x.RunWorkerAsync();
        }

        void x_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            voltagePointCollection.Clear();
            voltagepoint1 = new VoltagePointCollection();
            voltagepoint2 = new VoltagePointCollection();
            voltagepoint3 = new VoltagePointCollection();
            var ds1 = new EnumerableDataSource<VoltagePoint>(voltagepoint1);
            ds1.SetXMapping(x => dateAxis.ConvertToDouble(x.Date));
            ds1.SetYMapping(y => y.Voltage);
            plotter.AddLineGraph(ds1, Colors.Green, 2, "0kg"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
            MaxVoltage = 1;
            MinVoltage = -1;
            var ds2 = new EnumerableDataSource<VoltagePoint>(voltagepoint2);
            ds2.SetXMapping(x => dateAxis.ConvertToDouble(x.Date));
            ds2.SetYMapping(y => y.Voltage);
            plotter.AddLineGraph(ds2, Colors.Red, 2, "5kg"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
            MaxVoltage = 1;
            MinVoltage = -1;
            var ds3 = new EnumerableDataSource<VoltagePoint>(voltagepoint3);
            ds3.SetXMapping(x => dateAxis.ConvertToDouble(x.Date));
            ds3.SetYMapping(y => y.Voltage);
            plotter.AddLineGraph(ds3, Colors.Black, 2, "25kg"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
            MaxVoltage = 1;
            MinVoltage = -1;
            for (int i = 0; i < values[0].Count; i++) 
            {
                if (i % 50 == 0) { voltagepoint1.Clear(); voltagepoint2.Clear(); voltagepoint3.Clear(); screenshot(); }

                DateTime now = DateTime.Now;
                voltagepoint1.Add(new VoltagePoint(values[0][i], now));
                voltagepoint2.Add(new VoltagePoint(values[1][i], now));
                voltagepoint3.Add(new VoltagePoint(values[2][i], now));
            }
            MessageBox.Show("Tebrikler başardınız.");
        }
        List<List<int>> values = new List<List<int>>();
        void x_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = listboxSP.Items.Count;
            values.Clear();
            List<int> values_weigh=new List<int>();
            for (int i = 0; i < count;i++ )
            {
                if (i!=0 && listboxSP.Items[i].ToString().Contains("kg")) 
                {
                    values.Add(values_weigh);
                    values_weigh = new List<int>();
                }
                try
                {

                    int val = Convert.ToInt32(listboxSP.Items[i].ToString());
                    values_weigh.Add(val);
                }
                catch { }
                
            }
            values.Add(values_weigh);
            values_weigh.Clear();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            control();
        }
    }
}
