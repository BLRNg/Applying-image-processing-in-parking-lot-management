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
using RawInput_dll;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;

namespace MeTroUIDemo
{
    public partial class LaneInOut : MetroFramework.Forms.MetroForm
    {
        private readonly RawInput _rawinput;
        private string text;
        private bool secondTime;
        const bool CaptureOnlyInForeground = false;
        private IFilterGraph2 filterGraph;
        private ICaptureGraphBuilder2 captureGraphBuilder;
        private IVideoWindow videoWindow;
        private IMediaControl mediaControl;
        private ISampleGrabber sampleGrabber;
        WebClient client;
       
       

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
        private SettingParamRepo settingParamRepo;
        private string NameReaderIn;
        private string NameReaderOut;
        public LaneInOut()
        {
            InitializeComponent();
            userRepository = new UserRepository();
            settingParamRepo = new SettingParamRepo();

            NameReaderIn = settingParamRepo.sp_SelectSetting("NameReaderIn");
            NameReaderOut = settingParamRepo.sp_SelectSetting("NameReaderOut");

            text = string.Empty;

            secondTime = false;
            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);
            //_rawinput.AddMessageFilter();   // Adding a message filter will cause keypresses to be handled
            //Win32.DeviceAudit();            // Writes a file DeviceAudit.txt to the current directory

            _rawinput.KeyPressed += OnKeyPressed;
        }
        private void OnKeyPressed(object sender, RawInputEventArg e)
        {
            if (richTextBoxNameReader.Focused)
            {
                richTextBoxNameReader.Text = e.KeyPressEvent.DeviceName;
                Clipboard.Clear();
                Clipboard.SetText(e.KeyPressEvent.DeviceName);
                return;
            }
          
            if (IsAnyControlFocused(this))
            {
                //customerNameTile.Text = e.KeyPressEvent.DeviceName;
                if (!secondTime)
                {
                    if(text.Length < 1)
                    {
                        customerNameTile.Text = "";
                        messageValueTile.Text = "";
                        licenseValueTile.Text = "";
                        totalAAmountValueTile.Text = "";
                        timeValueTile.Text = "";
                        laneValueTile.Text = "";
                       
                    }

                  
                    if (e.KeyPressEvent.VKeyName.Equals("ENTER") )
                    {
                        string urlIn = urlImg();
                        string licenseNumber = GetLicenseNumber.GetLicenseNumberByUrl(urlIn, client);
                        if(licenseNumber.Equals("No License"))
                        {
                            customerNameTile.Text = "";
                            messageValueTile.Text = "Không phát hiện biển số";
                            licenseValueTile.Text = "";
                            totalAAmountValueTile.Text = "";
                            timeValueTile.Text = "";
                            laneValueTile.Text = "";
                            text = "";
                            return;
                        }
                        PhotoCustomerIn.Value = urlIn;
                        PhotoLicensePlateNumberIN.Value = urlIn;

                        if (NameReaderIn.Equals(e.KeyPressEvent.DeviceName))
                        {
                            var result = userRepository.sp_laneIn(1, licenseNumber, text, DateTime.Now, CustomerName, Message, PlateNum, total_Amount, Time,
                       LanceTye, PhotoCustomerIn, PhotoLicensePlateNumberIN);

                            picturePlateInBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            picturePlateOutBox.SizeMode = PictureBoxSizeMode.StretchImage;

                            picturePlateInBox.Image = Image.FromFile(Path.GetFullPath(PhotoLicensePlateNumberIN.Value.ToString()));
                            picturePlateOutBox.Image = null;

                        }
                        else if (NameReaderOut.Equals(e.KeyPressEvent.DeviceName))
                        {
                            var result = userRepository.sp_laneOut(1, licenseNumber, text, DateTime.Now, CustomerName, Message, PlateNum, total_Amount, Time,
                             LanceTye, PhotoCustomerIn, PhotoLicensePlateNumberIN, urlIn, urlIn, isMatch);
                            
                            picturePlateInBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            picturePlateOutBox.SizeMode = PictureBoxSizeMode.StretchImage;

                            picturePlateInBox.Image = Image.FromFile(Path.GetFullPath(PhotoLicensePlateNumberIN.Value.ToString()));
                            picturePlateOutBox.Image = Image.FromFile(Path.GetFullPath(urlIn));

                        }



                        customerNameTile.Text = CustomerName.Value.ToString();
                        messageValueTile.Text = Message.Value.ToString();
                        licenseValueTile.Text = PlateNum.Value.ToString();
                        totalAAmountValueTile.Text = total_Amount.Value.ToString();
                        timeValueTile.Text = Time.Value.ToString();
                        laneValueTile.Text = LanceTye.Value.ToString();
                        cardNoValueTile.Text = text;

                       
                        text = "";
                    }
                    else
                    {
                       

                        if (e.KeyPressEvent.VKeyName.Equals("BACK") && (text.Length - 1) >=0 )
                        {
                            text = text.Remove(text.Length - 1);

                        }
                        else if (!e.KeyPressEvent.VKeyName.Equals("BACK"))
                        {
                            text += e.KeyPressEvent.VKeyName;
                            text = Regex.Replace(text, @"D", "");

                        }


                        cardNoValueTile.Text = text;
                    }

                    secondTime = true;
                }
                else
                {
                    secondTime = false;
                }
            }
           
           
            

            //switch (e.KeyPressEvent.Message)
            //{
            //    case Win32.WM_KEYDOWN:
            //        Debug.WriteLine(e.KeyPressEvent.KeyPressState);
            //        break;
            //     case Win32.WM_KEYUP:
            //        Debug.WriteLine(e.KeyPressEvent.KeyPressState);
            //        break;
            //}
        }

        private bool IsAnyControlFocused(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Focused || IsAnyControlFocused(ctrl))
                {
                    return true;
                }
            }
            return false;
        }


        private string urlImg()
        {
            string fileName = @"Image\" + ImageNameGenerator.GenerateUniqueImageName(); ;
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
