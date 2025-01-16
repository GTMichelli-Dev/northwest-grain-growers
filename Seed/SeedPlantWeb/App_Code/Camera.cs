using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using File_Xfr;


using System.Data;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


using System.IO;
using System.Drawing.Text;
using System.Drawing.Printing;
using System.Threading.Tasks;

/// <summary>
/// Summary description for Camera
/// </summary>
public class Camera
{

  

    const string PicType = ".jpg";
    const int TotalPictureCount = 1000;

    public Camera()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public class PicData
    {
        public string FName { get; set; }

        public string ImageURL { get; set; }


    }


    public static List<PicData> GetPicturesForTicket(string Ticket)
    {
        List<PicData> dataList = new List<PicData>();
        try
        {


         //   using (NetworkShareAccesser.Access(PicturePC , PictureDomain, UserName, Password))
            {

                string Filter = $"{Ticket.ToString().Trim()}-{GlobalVars.Location.ToString().Trim()}";
                string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Pictures"));
                foreach (var file in filePaths)
                {

                    FileInfo fi = new FileInfo(file);
                    var URL = $"~/Pictures/{fi.Name}";
                    if (fi.Name.StartsWith(Filter))
                    {
                        dataList.Add(
                            new PicData
                            {
                                FName = fi.Name,
                                ImageURL = URL
                            }
                            );
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Auditing.LogMessage("Camera.GetPicturesForTicket", ex.Message);
            
        }
        return dataList;
    }

    public static void PurgePictures(string fPath)
    {

     //   using (NetworkShareAccesser.Access(PicturePC, PictureDomain, UserName, Password))
        {
            var files = Directory.GetFiles(fPath, "*").OrderByDescending(d => new FileInfo(d).CreationTime);
            List<string> f = files.ToList<string>();
            if (f.Count > TotalPictureCount)
            {
                for (int i = TotalPictureCount; i < f.Count; i++)
                {
                    try
                    {
                        File.Delete(f[i]);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }

    public class PictureResults
    {

        public bool Success = true;
        public string Message = "OK";
        public string VirtualPath = "";

    }


    private static string PictureFileName(int Ticket, int idx,string CameraLocation)
    {
      

        string Filename = $"{Ticket}-{GlobalVars.Location}_{idx}_{CameraLocation}{PicType}";
        return Filename;
    }

    //private static string PictureFileName(int Ticket)
    //{
    //    string Filename = $"{Ticket}{PicType}";
    //    return Filename;
    //}

    public static string FullPicturePath(int Ticket, int idx, string CameraLocation)
    {
        string fPath =Path.Combine(HttpRuntime.AppDomainAppPath, "Pictures");


        fPath += $@"\{PictureFileName(Ticket, idx, CameraLocation)}";
        return fPath;
    }


    //public static string FullPicturePath(int Ticket)
    //{
    //    string fPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Pictures");


    //    fPath += $@"\{PictureFileName(Ticket)}";
    //    return fPath;
    //}


    public static string VirtualPicturePath(int Ticket, int idx, string CameraLocation)
    {
        string fPath = FullPicturePath(Ticket, idx, CameraLocation);
        return $"~/Pictures/{PictureFileName(Ticket, idx, CameraLocation)}";


    }


    //public static string VirtualPicturePath(int Ticket)
    //{

    //    return $"~/Pictures/{PictureFileName(Ticket)}";


    //}



   public static string TicketData(SeedTicketDataSet seedTicketDataset)
    {
        string Retval = string.Empty;

        //int Total = 0;

        //ReportDataSet reportDataset = SeedTicketInfo.GetAllTicketInfo(SeedTicketInfo.seedTicketDataSet, SeedTicketInfo.CurrentSeedTicketRow.UID);

        //if (reportDataset.Ticket_Varieties.Count > 0)
        //{
        //    using (SeedTicketDataSet.VarietyListDataTable VarietyList = new SeedTicketDataSet.VarietyListDataTable())
        //    {
        //        decimal Total = 0;
        //        foreach (var row in reportDataset.Ticket_Varieties)
        //        {
        //            var vlRow = VarietyList.NewVarietyListRow();
        //            vlRow.Item = row.Variety_ID;
        //            vlRow.Description = row.Description;// (row.Description.Length > 22) ? row.Description.Substring(0, 22) :
        //            vlRow.Percent = (decimal)row.Percent_Of_Load;
        //            Total += vlRow.Percent;
        //            vlRow.Bin = (row.IsBin_NameNull()) ? "" : row.Bin_Name;
        //            vlRow.Total = Convert.ToInt32(row.Total);
        //            VarietyList.AddVarietyListRow(vlRow);
        //            vlRow.UID = row.UID;
        //        }
        //        decimal Remainder = 0;
        //        decimal RoundingError = 0;
        //        if (Total < 100)
        //        {
        //            Remainder = 100 - Total;
        //            if (Remainder <= 1 && reportDataset.Ticket_Varieties.Count > 0)
        //            {
        //                Total = 0;
        //                RoundingError = Remainder / reportDataset.Ticket_Varieties.Count;
        //                foreach (var item in VarietyList)
        //                {
        //                    item.Percent += RoundingError;
        //                    Total += item.Percent;
        //                }
        //            }
        //        }

        //        Retval = (Total == 100);

        //    }
     



        

        if (seedTicketDataset.Seed_Tickets.Count>0 & seedTicketDataset.Seed_Ticket_Weights.Count==1)
        {
            var Sr = seedTicketDataset.Seed_Tickets[0];
            Retval = $"Ticket:{Sr.Ticket}" + System.Environment.NewLine;
            Retval += $"Date:{Sr.Ticket_Date}" + System.Environment.NewLine;
            Retval += $"Grower:{Sr.Grower_Name}-{Sr.Grower_ID}" + System.Environment.NewLine;
            if (!Sr.IsPONull()) Retval += $"PO:{Sr.PO }" + System.Environment.NewLine;
            if (!Sr.IsBOLNull()) Retval += $"BOL:{Sr.BOL }" + System.Environment.NewLine;
            if (!Sr.IsTruck_IDNull()) Retval += $"Truck ID:{Sr.Truck_ID }" + System.Environment.NewLine;
            if (!Sr.IsCommentsNull()) Retval += $"Comments:{Sr.Comments }" + System.Environment.NewLine;
            if (!Sr.IsWeighmasterNull()) Retval += $"Weighed By:{Sr.Weighmaster}" + System.Environment.NewLine;



            var InWeight =(seedTicketDataset.Seed_Ticket_Weights[0].IsStarting_WeightNull())?0: seedTicketDataset.Seed_Ticket_Weights[0].Starting_Weight;
            var OutWeight = (seedTicketDataset.Seed_Ticket_Weights[0].IsEnding_WeightNull()) ? 0 : seedTicketDataset.Seed_Ticket_Weights[0].Ending_Weight;
            Retval += System.Environment.NewLine+ "Truck Weights" + System.Environment.NewLine; ;
            Retval += "---------------------" + System.Environment.NewLine; 

            Retval += string.Format("Weight In :{0:N0}", InWeight).PadLeft(8) + System.Environment.NewLine; 
            Retval += string.Format("Weight Out:{0:N0}", OutWeight).PadLeft(8) + System.Environment.NewLine;
            Retval += "---------------------" + System.Environment.NewLine; ;
            Retval += string.Format("Net Weight:{0:N0}",Math.Abs( InWeight-OutWeight) ).PadLeft(8) + System.Environment.NewLine; ;



            Retval += System.Environment.NewLine+"Varieties:" + System.Environment.NewLine;
            Retval += "Variety-ID                                   %Load " + System.Environment.NewLine; ;
            Retval += "---------------------------------------------------" + System.Environment.NewLine; ;
            foreach (var item in seedTicketDataset.Seed_Ticket_Varieties )
            {
                string Name = $"{item.Custom_Name}-{item.Variety_ID}";
                string PercentOfLoad = ((int)item.Percent_Of_Load).ToString().PadLeft(3) + "%";
                try
                {

                    if (Name.Length > 45) Name = item.Custom_Name;
                    if (Name.Length > 45) Name = Name.Substring(0, 45);
                    Name = Name.PadRight(45, ' ');

                }
                catch
                {

                }
                Retval += $"{Name} {PercentOfLoad}" + System.Environment.NewLine;
            }

            Retval += System.Environment.NewLine + "Treatments:" + System.Environment.NewLine;
            Retval += "Treatment-ID                                  Rate " + System.Environment.NewLine;
            Retval += "---------------------------------------------------" + System.Environment.NewLine;
            foreach (var item in seedTicketDataset.Seed_Ticket_Treatments)
            {
                string Name = $"{item.Custom_Name}-{item.Treatment_ID }";
                string Rate = String.Format("{0:N2}", item.Rate).PadLeft(3);
                try
                {
                    if (Name.Length > 45) Name = item.Custom_Name;
                    if (Name.Length > 45) Name = Name.Substring(0, 45);
                    Name = Name.PadRight(45, ' ');

                }
                catch
                {

                }

                Retval += $"{Name} {Rate}" + System.Environment.NewLine;
            }


        }
        return Retval;
    }


    public static List<PictureResults> TakePicture(Guid UID , int idx=1)
    {
        List<PictureResults> Results = new List<global::Camera.PictureResults>();
        SeedTicketDataSet seedTicketDataSet = SeedTicketInfo.GetSeedTicketDataset(UID);
        if (seedTicketDataSet.Seed_Ticket_Weights.Count > 0 && !seedTicketDataSet.Seed_Ticket_Weights[0].IsOutbound_ScaleNull())
        {
             using (SetupDataSetTableAdapters.Scale_CamerasTableAdapter scale_CamerasTableAdapter = new SetupDataSetTableAdapters.Scale_CamerasTableAdapter())
            {
                using (SetupDataSet.Scale_CamerasDataTable scale_CamerasDataTable = new SetupDataSet.Scale_CamerasDataTable())
                {
                    if (scale_CamerasTableAdapter.Fill(scale_CamerasDataTable, GlobalVars.Location, seedTicketDataSet.Seed_Ticket_Weights[0].Outbound_Scale) > 0)
                    {
                        foreach (var row in scale_CamerasDataTable )
                        {
                            if (row != null && seedTicketDataSet.Seed_Tickets.Count > 0 && !seedTicketDataSet.Seed_Tickets[0].IsTicketNull())
                            {
                              Results.Add(  TakePicture(UID, idx, seedTicketDataSet, row));
                            }
                        }
                }
                }
            }
        }
        return Results;
    }

    private  static PictureResults TakePicture(Guid UID, int idx, SeedTicketDataSet seedTicketDataSet, SetupDataSet.Scale_CamerasRow CameraRow)
    {
        PictureResults pr = new global::Camera.PictureResults();
       // string PicturePath = @"\\waldb001\SeedTicketsImages\"; // Path.Combine(HttpRuntime.AppDomainAppPath, "Pictures");
               try
                {
                    string fPath = FullPicturePath(seedTicketDataSet.Seed_Tickets[0].Ticket, idx, CameraRow.Camera_Location );
                    
                    Camera.Picture pic = new Camera.Picture(TicketData(seedTicketDataSet), CameraRow.TextLeftOffset, CameraRow.TextTopOffset, fPath, CameraRow.rtsp_Address, CameraRow.User_Name, CameraRow.Password, 12);
                    Picture.enumPictureResults Results = pic.TakePicture();

                    if (Results == Picture.enumPictureResults.Fail)
                    {
                        pr.Success = false;
                        pr.Message = pic.ResultMessage;

                    }
                    else
                    {
                        pr.VirtualPath = VirtualPicturePath(seedTicketDataSet.Seed_Tickets[0].Ticket, idx, CameraRow.Camera_Location);
                    }

                }
                catch (Exception ex)
                {
                    Auditing.LogMessage("Camera.TakePicture",CameraRow.ToString()+System.Environment.NewLine+ ex.ToString());
                    pr.Success = false;
                    pr.Message = ex.Message;
                }
           
    
        return pr;
    }


    //public static PictureResults TakePicture(int Ticket, string Text)
    //{
    //    string PicturePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Pictures");
    //    TaskFactory tf = new TaskFactory();
    //    tf.StartNew(() => PurgePictures(PicturePath));

    //    PictureResults pr = new global::Camera.PictureResults();
    //    try
    //    {


    //        if (string.IsNullOrEmpty(Text)) Text = $"Load #{Ticket}";

    //        string fPath = FullPicturePath(Ticket);

    //        Camera.Picture pic = new Camera.Picture(Text, Settings.TextLeftOffset, Settings.TextTopOffset, fPath, Settings.RSTP_URL, Settings.CameraUserName, Settings.CameraPassword);
    //        Picture.enumPictureResults Results = pic.TakePicture();

    //        if (Results == Picture.enumPictureResults.Fail)
    //        {
    //            pr.Success = false;
    //            pr.Message = pic.ResultMessage;

    //        }
    //        else
    //        {
    //            pr.VirtualPath = VirtualPicturePath(Ticket);
    //        }




    //    }
    //    catch (Exception ex)
    //    {
    //        pr.Success = false;
    //        pr.Message = ex.Message;
    //    }
    //    return pr;
    //}


    public class Picture
    {

        public Font TextFont = new Font("Courier New", 28);


        string FilePath;
        public string ResultMessage;
        string RTSP_URL;

        string User;
        string Pass;

        int LeftOffset;
        int TopOffset;



        public string Response
        {
            get
            {
                return ResultMessage;
            }
        }


        string PictureText = "";


        public string CammeraIpAddress
        {
            get
            {
                return RTSP_URL;
            }
        }



        public string PictureFilePath
        {
            get
            {
                return FilePath;

            }
            set
            {
                FilePath = value;
            }
        }


        /// <summary>
        /// Initialize the Picture Class
        /// </summary>
        /// <param name="TextToEmbed">The text that you want to be in the picture. By default the font is mono space</param>
        /// <param name="leftOffSet">% of offset from the left 50 = 50% from the left</param>
        /// <param name="topOffset">% of offset from the left 50 = 50% from the Top </param>
        /// <param name="fileName">Full (.png) File name that you want to save it as like c:\Test\Pic.png</param>
        /// <param name="RstpUrl">Camera Address like 192.168.4.10</param>
        /// <param name="UserName">user name of camera</param>
        /// <param name="Password">password of camera</param>
        /// <param name="fontSize">by default its 28 </param>
        public Picture(string TextToEmbed, int leftOffSet, int topOffset, string fileName, string RstpUrl, string UserName, string Password, float fontSize = 28)
        {
            FilePath = fileName;
            RTSP_URL = RstpUrl;
            User = UserName;
            Pass = Password;
            PictureText = TextToEmbed;
            TextFont = new Font("Courier New", fontSize);
            if (leftOffSet < 0) leftOffSet = 0;
            if (leftOffSet > 100) leftOffSet = 100;

            if (topOffset < 0) topOffset = 0;
            if (topOffset > 100) topOffset = 100;


            LeftOffset = leftOffSet;
            TopOffset = topOffset;

        }






        public enum enumPictureResults { Success, Fail }






        public enumPictureResults TakePicture()
        {
            try
            {
                // Set for a HIK vision Camera
                //string URL = RTSP_URL; // @"http://0.0.0.0/Streaming/channels/1/picture";
                //URL = URL.Replace("0.0.0.0", RTSP_URL);
                GetPictureFromCamera(FilePath, User, Pass);
                ResultMessage = "Ok";
                return enumPictureResults.Success;
            }
            catch (Exception e)
            {
                ResultMessage = "Error Saving Picture..  System returned: " + e.Message;
                return enumPictureResults.Fail;
            }

        }


        private bool IsDomainAlive(string aDomain, int aTimeoutSeconds)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var result = client.BeginConnect(aDomain, 80, null, null);

                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(aTimeoutSeconds));

                    if (!success)
                    {
                        return false;
                    }

                    // we have connected
                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }










        public Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }



        //private byte[] GetPictureFromCamera(string Source, string UserName, string Password)
        //{

        //    string sourceURL = Source;

        //    byte[] buffer = new byte[1000000];
        //    // create HTTP request
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
        //    req.Timeout = 5000;
        //    // set login and password
        //    req.Credentials = new NetworkCredential(UserName, Password);
        //    // get response

        //    WebResponse resp = req.GetResponse();
        //    // get response stream
        //    Stream stream = resp.GetResponseStream();
        //    // get bitmap
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        stream.CopyTo(ms);
        //        byte[] result = ms.ToArray();
        //        return result;
        //    }


        //}


        public string SavePictureByteToPNG(byte[] Image, string PicPath)
        {
            try
            {

           //     using (NetworkShareAccesser.Access(PicturePC, PictureDomain, UserName, Password))
                {
                    MemoryStream stream = new MemoryStream(Image);
                    Bitmap bmp = (Bitmap)Bitmap.FromStream(stream);
                    ImageFormat imgFormat = (PicType == ".jpg") ? imgFormat = ImageFormat.Jpeg : imgFormat = ImageFormat.Png;


                    bmp.Save(PicPath, imgFormat);

                    return (PicPath);
                }

            }
            catch (Exception ex)
            {
                ResultMessage = $"Error Saving Byte To {PicType} PicPath=" + PicPath + "-  " + ex.Message;
                return string.Empty;

            }
        }


        /// <summary>
        /// Saves the picture from a memory stream to a png 
        /// </summary>
        /// <param name="Image">memory stream of the picture</param>
        /// <param name="PicPath">windows path of the picture</param>
        /// <returns>true if sucessfull</returns>
        public bool SavePictureMemoryStreamToPNG(MemoryStream Image, string PicPath)
        {
            try
            {


      //          using (NetworkShareAccesser.Access(PicturePC, PictureDomain, UserName, Password))
                {

                    Bitmap bmp = (Bitmap)Bitmap.FromStream(Image);
                    ImageFormat imgFormat = (PicType == ".jpg") ? imgFormat = ImageFormat.Jpeg : imgFormat = ImageFormat.Png;

                    bmp.Save(PicPath, imgFormat);


                    return (true);
                }

            }
            catch (Exception ex)
            {
                ResultMessage = $"Error Saveing Picture From MemoryStream To {PicType} PicPath=" + PicPath + "-  " + ex.Message;
                return false;

            }
        }







        /// <summary>
        /// Sets the picture on the screen
        /// </summary>
        /// <param name="Source">the url of the camera like http://192.168.1.51/Streaming/channels/1/picture </param>
        /// <param name="Ticket">The name of the file like truckscale.jpg</param>
        /// <param name="PicPath">The name of the Path To Save It To like C:\Pictures</param>
        /// <param name="UserName">THe Username for the Camera</param>
        /// <param name="Password">The Password For The Camera</param>
        /// <param name="ImageSize">THe Size Of the Picture Saved like Size(405, 720) Leave Null for Default size</param>
        private void GetPictureFromCamera(string PicPath, string UserName, string Password)
        {



          //  using (NetworkShareAccesser.Access(PicturePC, PictureDomain, UserName, Password))
            {



                byte[] buffer = new byte[1000000];
            // create HTTP request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(RTSP_URL);
            req.Timeout = 5000;
            // set login and password
            req.Credentials = new NetworkCredential(UserName, Password);
            // get response

            WebResponse resp = req.GetResponse();
            // get response stream
            Stream stream = resp.GetResponseStream();
                // get bitmap
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    Bitmap bmp = (Bitmap)Bitmap.FromStream(ms);



                    //RectangleF rectf = new RectangleF(70, 90, 90, 50);

                    Graphics g = Graphics.FromImage(bmp);

                    //g.SmoothingMode = SmoothingMode.AntiAlias;
                    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //g.DrawString("yourText", new Font("Tahoma", 8), Brushes.Black, rectf);

                    //g.Flush();


                    // Create a rectangle for the entire bitmap
                    RectangleF rectf = new RectangleF(0, 0, bmp.Width, bmp.Height);

                    // Create graphic object that will draw onto the bitmap



                    // ------------------------------------------
                    // Ensure the best possible quality rendering
                    // ------------------------------------------
                    // The smoothing mode specifies whether lines, curves, and the edges of filled areas use smoothing (also called antialiasing). 
                    // One exception is that path gradient brushes do not obey the smoothing mode. 
                    // Areas filled using a PathGradientBrush are rendered the same way (aliased) regardless of the SmoothingMode property.
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // The interpolation mode determines how intermediate values between two endpoints are calculated.
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // Use this property to specify either higher quality, slower rendering, or lower quality, faster rendering of the contents of this Graphics object.
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    // This one is important
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    // Create string formatting options (used for alignment)
                    StringFormat format = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };



                    // Create solid brush.
                    SolidBrush whiteBrush = new SolidBrush(Color.White);

                    //Get the size of the text
                    //Graphics g = Graphics.FromImage(bmp);

                    SizeF RecSize = g.MeasureString(PictureText, TextFont);


                    RecSize.Width += 10;
                    RecSize.Height += 10;

                    int PtWidth = (int)((LeftOffset / 100f) * bmp.Width);
                    int PtHeight = (int)((TopOffset / 100f) * bmp.Height);

                    if (PtWidth + RecSize.Width > bmp.Width) PtWidth = bmp.Width - (int)RecSize.Width;
                    if (PtHeight + RecSize.Height > bmp.Height) PtHeight = bmp.Height - (int)RecSize.Height;


                    Point Location = new Point(PtWidth, PtHeight);

                    // Create rectangle.
                    Rectangle rect = new Rectangle(Location, Size.Round(RecSize));




                    // Fill rectangle to screen.
                    g.FillRectangle(whiteBrush, rect);

                    g.DrawRectangle(new Pen(color: Color.Black, width: 2), rect);
                    // Draw the text onto the image
                    g.DrawString(PictureText, TextFont, Brushes.Black, PtWidth + 5, PtHeight + 5);
                    //g.DrawRectangle()

                    // Flush all graphics changes to the bitmap
                    g.Flush();

                    //// Now save or use the bitmap
                    //image.Image = bmp;

                    ImageFormat imgFormat = (PicType == ".jpg") ? imgFormat = ImageFormat.Jpeg : imgFormat = ImageFormat.Png;

                    bmp.Save(PicPath, imgFormat);



                }



            }


        }




        private SizeF StringSize(PrintPageEventArgs e, string StringToMeasure, Font StringFont)
        {
            SizeF stringSize = new SizeF();
            stringSize = e.Graphics.MeasureString(StringToMeasure, StringFont);
            return stringSize;
        }




        private Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        private void saveJpeg(string path, Bitmap img, long quality)
        {

        //    using (NetworkShareAccesser.Access(PicturePC, PictureDomain, UserName, Password))
            {
                // Encoder parameter for image quality
                EncoderParameter qualityParam =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                // Jpeg image codec
                ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

                if (jpegCodec == null)
                    return;

                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                img.Save(path, jpegCodec, encoderParams);
            }
        }

        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }





    }
}
