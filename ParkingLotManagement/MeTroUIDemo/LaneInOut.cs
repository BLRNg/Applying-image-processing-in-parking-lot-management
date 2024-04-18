using MeTroUIDemo.Helper;
using MeTroUIDemo.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectShowLib;


namespace MeTroUIDemo
{
    public partial class LaneInOut : MetroFramework.Forms.MetroForm
    {
        private IFilterGraph2 filterGraph;
        private ICaptureGraphBuilder2 captureGraphBuilder;
        private IVideoWindow videoWindow;
        private IMediaControl mediaControl;
        private ISampleGrabber sampleGrabber;
        WebClient client;
        SerialPort serialPort1;
        Boolean isIn = true;

        ObjectParameter CustomerName;
        ObjectParameter Message;
        ObjectParameter PlateNum;
        ObjectParameter total_Amount;
        ObjectParameter Time;
        ObjectParameter LanceTye;
        ObjectParameter PhotoCustomerIn;
        ObjectParameter PhotoLicensePlateNumberIN;
        ObjectParameter isMatch;

        private UserRepository userRepository;
        public LaneInOut()
        {
            InitializeComponent();
            serialPort1 = new SerialPort();
            serialPort1.PortName = "COM5"; // Change to match your Arduino's COM port
            serialPort1.BaudRate = 9600; // Must match Arduino's baud rate
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            userRepository = new UserRepository();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            //MessageBox.Show(data);
            
            if (isIn)
            {
                data = "r0 03 40 3a fe";
                isIn = false;
            }
            else
            {
                data = "r1 03 40 3a fe";
                isIn = true;
            }
           
           
            //if (data.Contains("x92"))
            //    MessageBox.Show(data);

                //if( data.Contains("_") && 2==data.Count(c => c == '_'))
                //MessageBox.Show(data +" |"+data.Substring(8,19 -5 - 3)+"| "+ data.IndexOf("_") + " " + data.LastIndexOf("_"));
                if (data.Contains("r"))
            {
                string[] parts = data.Split(' ');

                // Join the parts starting from the second element
                string resultString = string.Join(" ", parts, 1, parts.Length - 1);
                //MessageBox.Show(resultString);
                UpdateTextBox(data);
            }

            //MessageBox.Show("Dau doc 1" + data);
            //if (data.Contains("Reader1") || data.Contains("Reader0"))
            //{
            //    //string textAfterReader1 = data.Substring("Reader1".Length).Trim();
            //    UpdateTextBox(data);
            //    //MessageBox.Show("Dau doc 1"+textAfterReader1);
            //}



        }

        delegate void UpdateTextBoxDelegate(string text);

        private void UpdateTextBox(string text)
        {
            if (cardNoValueTile.InvokeRequired)
            {
                cardNoValueTile.Invoke(new UpdateTextBoxDelegate(UpdateTextBox), new object[] { text });
            }
            else
            {
                string[] parts = text.Split(' ');
                string cardNo = string.Join(" ", parts, 1, parts.Length - 1).TrimEnd('\r', '\n');
                if (text.Contains("r1"))
                {
                    string urlOut = urlImg();
                    string licenseNumber = GetLicenseNumber.GetLicenseNumberByUrl(urlOut, client);

                    var result = userRepository.sp_laneOut(1, licenseNumber, cardNo, DateTime.Now, CustomerName, Message, PlateNum, total_Amount, Time,
                LanceTye, PhotoCustomerIn, PhotoLicensePlateNumberIN, urlOut, urlOut, isMatch);

                    customerNameTile.Text = CustomerName.Value.ToString();
                    messageValueTile.Text = Message.Value.ToString();
                    licenseValueTile.Text = PlateNum.Value.ToString();
                    totalAAmountValueTile.Text = total_Amount.Value.ToString();
                    timeValueTile.Text = Time.Value.ToString();
                    laneValueTile.Text = LanceTye.Value.ToString();
                    cardNoValueTile.Text = cardNo;

                    if (Message.Value.ToString().Contains("Xin mời qua"))
                    {
                        try
                        {
                            serialPort1.Write("x");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }

                }

                if (text.Contains("r0"))
                {
                    string urlIn = urlImg();
                    string licenseNumber = GetLicenseNumber.GetLicenseNumberByUrl(urlIn, client);
                    PhotoCustomerIn.Value = urlIn;
                    PhotoLicensePlateNumberIN.Value = urlIn;
                    var result = userRepository.sp_laneIn(1, licenseNumber, cardNo, DateTime.Now, CustomerName, Message, PlateNum, total_Amount, Time,
                    LanceTye, PhotoCustomerIn, PhotoLicensePlateNumberIN);

                    customerNameTile.Text = CustomerName.Value.ToString();
                    messageValueTile.Text = Message.Value.ToString();
                    licenseValueTile.Text = PlateNum.Value.ToString();
                    totalAAmountValueTile.Text = total_Amount.Value.ToString();
                    timeValueTile.Text = Time.Value.ToString();
                    laneValueTile.Text = LanceTye.Value.ToString();
                    cardNoValueTile.Text = cardNo;

                    if (Message.Value.ToString().Contains("Xin mời qua"))
                    {
                        try
                        {
                            serialPort1.Write("z");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }

                }




            }
        }

        private string urlImg()
        {
            string fileName = @"C:\Users\trant\Downloads\Image\" + ImageNameGenerator.GenerateUniqueImageName(); ;
            try
            {
                if (sampleGrabber != null)
                {
                    var sampGrabber = sampleGrabber as ISampleGrabber;
                    int bufferSize = 0;
                    sampGrabber.GetCurrentBuffer(ref bufferSize, IntPtr.Zero);

                    if (bufferSize > 0)
                    {
                        var buffer = Marshal.AllocCoTaskMem(bufferSize);
                        sampGrabber.GetCurrentBuffer(ref bufferSize, buffer);

                        int width = 1280; // Change the width as needed
                        int height = 720; // Define the desired height
                        int stride = width * 3; // Calculate stride based on width and pixel format

                        var bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, buffer);
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                        Marshal.FreeCoTaskMem(buffer);

                        return fileName;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("An error occurred while capturing and saving image: " + ex.Message);
            }

            return "";
        }

        private void LaneInOut_Load(object sender, EventArgs e)
        {
            CustomerName = new ObjectParameter("CustomerName", typeof(string));
            Message = new ObjectParameter("Message", typeof(string));
            PlateNum = new ObjectParameter("PlateNum", typeof(string));
            total_Amount = new ObjectParameter("total_Amount", typeof(string));
            Time = new ObjectParameter("Time", typeof(string));
            LanceTye = new ObjectParameter("LanceTye", typeof(string));
            PhotoCustomerIn = new ObjectParameter("PhotoCustomerIn", typeof(string));
            PhotoLicensePlateNumberIN = new ObjectParameter("PhotoLicensePlateNumberIN", typeof(string));
            isMatch = new ObjectParameter("isMatch", typeof(bool));

            client = new WebClient();
            try
            {
                serialPort1.Open(); // Open the serial port
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            filterGraph = new FilterGraph() as IFilterGraph2;
            captureGraphBuilder = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;

            // Create the media control for controlling the graph
            mediaControl = filterGraph as IMediaControl;

            try
            {
                // Get the first video input device
                var device = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice)[0];

                // Attach the camera filter to the graph
                IBaseFilter sourceFilter;
                var hr = captureGraphBuilder.SetFiltergraph(filterGraph);
                hr = filterGraph.AddSourceFilterForMoniker(device.Mon, null, "Video Capture", out sourceFilter);
                Marshal.ThrowExceptionForHR(hr);

                // Create the renderer filter
                IBaseFilter rendererFilter = new VideoRenderer() as IBaseFilter;

                // Add both filters to the filter graph
                hr = filterGraph.AddFilter(sourceFilter, "Source Filter");
                Marshal.ThrowExceptionForHR(hr);
                hr = filterGraph.AddFilter(rendererFilter, "Renderer Filter");
                Marshal.ThrowExceptionForHR(hr);

                // Connect the camera filter to the renderer filter
                hr = captureGraphBuilder.RenderStream(PinCategory.Capture, MediaType.Video, sourceFilter, null, rendererFilter);
                Marshal.ThrowExceptionForHR(hr);

                // Create SampleGrabber
                sampleGrabber = new SampleGrabber() as ISampleGrabber;
                var sampGrabber = sampleGrabber as ISampleGrabber;
                var mt = new AMMediaType();
                mt.majorType = MediaType.Video;
                mt.subType = MediaSubType.RGB24;
                sampGrabber.SetMediaType(mt);
                hr = filterGraph.AddFilter(sampleGrabber as IBaseFilter, "SampleGrabber");
                Marshal.ThrowExceptionForHR(hr);

                // Connect SampleGrabber
                hr = captureGraphBuilder.RenderStream(null, null, sourceFilter, null, sampleGrabber as IBaseFilter);
                Marshal.ThrowExceptionForHR(hr);

                hr = sampGrabber.SetBufferSamples(true);
                Marshal.ThrowExceptionForHR(hr);
                hr = sampGrabber.SetOneShot(false);
                Marshal.ThrowExceptionForHR(hr);

                hr = sampGrabber.GetConnectedMediaType(mt);
                Marshal.ThrowExceptionForHR(hr);

                var header = (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));
                Marshal.FreeCoTaskMem(mt.formatPtr);



                // Get the video window interface
                videoWindow = filterGraph as IVideoWindow;
                videoWindow.put_Owner(camera1.Handle);
                videoWindow.put_MessageDrain(this.Handle);
                videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
                videoWindow.SetWindowPosition(camera1.ClientRectangle.Left, camera1.ClientRectangle.Top, camera1.ClientRectangle.Width, camera1.ClientRectangle.Height);
                videoWindow.put_Visible(OABool.True);



                // Start the preview
                hr = mediaControl.Run();
                Marshal.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void LaneInOut_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close(); // Close the serial port when the form is closing

            // Stop the preview and release resources
            if (mediaControl != null)
                mediaControl.Stop();
            if (videoWindow != null)
                videoWindow.put_Visible(OABool.False);

            // Release COM objects
            if (mediaControl != null) Marshal.ReleaseComObject(mediaControl);
            if (videoWindow != null) Marshal.ReleaseComObject(videoWindow);
            if (captureGraphBuilder != null) Marshal.ReleaseComObject(captureGraphBuilder);
            if (filterGraph != null) Marshal.ReleaseComObject(filterGraph);
        }
    }
}
