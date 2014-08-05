/**
 * @Author: Kheir Eddine FARFAR
 * @Author Github: https://github.com/Reddine
 * @Description: Capture Images from WebCam
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Net;
using System.IO;

namespace Inscription
{
    public partial class Form1 : Form
    {
        FilterInfoCollection videoDevices;

        public Form1()
        {
            InitializeComponent();

            // show device list
            try
            {
                // enumerate video devices
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                // add all devices to combo
                foreach (FilterInfo device in videoDevices)
                {
                    devicesCombo.Items.Add(device.Name);
                }
            }
            catch (ApplicationException)
            {
                devicesCombo.Items.Add("No local capture devices");
                devicesCombo.Enabled = false;
                takePictureBtn.Enabled = false;
            }

            devicesCombo.SelectedIndex = 0;

            VideoCaptureDevice videoCaptureSource = new VideoCaptureDevice(videoDevices[devicesCombo.SelectedIndex].MonikerString);
            videoSourcePlayer.VideoSource = videoCaptureSource;
            videoSourcePlayer.Start();
        }

        private void takePictureBtn_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;              // Use current time
	        string format = "MMM ddd d HH mm yyyy";    // Use this format
            String strFilename = "Capture-" + time.ToString(format) + ".jpg";
            if (videoSourcePlayer.IsRunning)
            {
                Bitmap picture = videoSourcePlayer.GetCurrentVideoFrame();
                picture.Save(strFilename, ImageFormat.Jpeg);
                labelSaved.Text = "Capture Saved : " + strFilename;                
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
            videoDevices = null;
            videoDevices = null;
        }

        private void devicesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
            VideoCaptureDevice videoCaptureSource = new VideoCaptureDevice(videoDevices[devicesCombo.SelectedIndex].MonikerString);
            videoSourcePlayer.VideoSource = videoCaptureSource;
            videoSourcePlayer.Start();
        }
    }
}
