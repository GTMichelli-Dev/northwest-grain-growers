namespace GrainManagement.Reporting
{
#nullable disable

    using System;
    using System.ComponentModel;
    using DevExpress.DataAccess.ObjectBinding;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class LotLabelReport
    {
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private BottomMarginBand bottomMarginBand1;
        private DetailBand detailBand1;
        private ObjectDataSource objectDataSource1;
        private XRLabel xrLotId;
        private XRLabel xrSgValue;
        private XRLabel xrCropValue;
        private XRLabel xrAccountValue;
        private XRLabel xrCreatedValue;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LotLabelReport));
            DevExpress.XtraPrinting.BarCode.QRCodeGS1Generator qrCodeGS1Generator2 = new DevExpress.XtraPrinting.BarCode.QRCodeGS1Generator();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLotId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSgValue = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCropValue = new DevExpress.XtraReports.UI.XRLabel();
            this.xrAccountValue = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCreatedValue = new DevExpress.XtraReports.UI.XRLabel();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 5F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 5F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrPictureBox1,
            this.xrBarCode1,
            this.xrLabel1,
            this.xrLotId,
            this.xrSgValue,
            this.xrCropValue,
            this.xrAccountValue,
            this.xrCreatedValue});
            this.detailBand1.HeightF = 313.88F;
            this.detailBand1.Name = "detailBand1";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Angle = 90F;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(96.93056F, 0F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(11.07F, 313.88F);
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Northwest Grain Growers. Walla Walla Wa";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(9.999954F, 271.1806F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(41.00006F, 38.19443F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.BarCodeOrientation = DevExpress.XtraPrinting.BarCode.BarCodeOrientation.RotateLeft;
            this.xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LotId]")});
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(1.999954F, 0F);
            this.xrBarCode1.Module = 1.5F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 10F, 0F, 0F, 100F);
            this.xrBarCode1.ShowText = false;
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(57.00005F, 36.34415F);
            this.xrBarCode1.Symbology = qrCodeGS1Generator2;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Angle = 90F;
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "trim([LocationDescription]) + \' - \' + [LocationId]")});
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(1.249949F, 36.34415F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(18F, 233.1702F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLotId
            // 
            this.xrLotId.Angle = 90F;
            this.xrLotId.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLotId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'LOT:\'+[As400Id]")});
            this.xrLotId.Font = new DevExpress.Drawing.DXFont("Arial", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLotId.LocationFloat = new DevExpress.Utils.PointFloat(19.75F, 42.84425F);
            this.xrLotId.Name = "xrLotId";
            this.xrLotId.Padding = new DevExpress.XtraPrinting.PaddingInfo(1F, 1F, 0F, 0F, 100F);
            this.xrLotId.SizeF = new System.Drawing.SizeF(22.5F, 220.17F);
            this.xrLotId.StylePriority.UseBorders = false;
            this.xrLotId.StylePriority.UseFont = false;
            this.xrLotId.StylePriority.UseTextAlignment = false;
            this.xrLotId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrSgValue
            // 
            this.xrSgValue.Angle = 90F;
            this.xrSgValue.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Split: \'+[SplitGroupId] + \' \' + [SplitGroupDescription]")});
            this.xrSgValue.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrSgValue.LocationFloat = new DevExpress.Utils.PointFloat(75F, 0F);
            this.xrSgValue.Name = "xrSgValue";
            this.xrSgValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(1F, 1F, 0F, 0F, 100F);
            this.xrSgValue.SizeF = new System.Drawing.SizeF(13F, 313.88F);
            this.xrSgValue.StylePriority.UseFont = false;
            this.xrSgValue.StylePriority.UseTextAlignment = false;
            this.xrSgValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrCropValue
            // 
            this.xrCropValue.Angle = 90F;
            this.xrCropValue.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CropName]")});
            this.xrCropValue.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrCropValue.LocationFloat = new DevExpress.Utils.PointFloat(42.25001F, 36.34415F);
            this.xrCropValue.Name = "xrCropValue";
            this.xrCropValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(1F, 1F, 0F, 0F, 100F);
            this.xrCropValue.SizeF = new System.Drawing.SizeF(16F, 233.1669F);
            this.xrCropValue.StylePriority.UseFont = false;
            this.xrCropValue.StylePriority.UseTextAlignment = false;
            this.xrCropValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrAccountValue
            // 
            this.xrAccountValue.Angle = 90F;
            this.xrAccountValue.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PrimaryAccountName] + \' (\' + [PrimaryAccountId] + \')\'")});
            this.xrAccountValue.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrAccountValue.LocationFloat = new DevExpress.Utils.PointFloat(59F, 0F);
            this.xrAccountValue.Name = "xrAccountValue";
            this.xrAccountValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(1F, 1F, 0F, 0F, 100F);
            this.xrAccountValue.SizeF = new System.Drawing.SizeF(16F, 313.88F);
            this.xrAccountValue.StylePriority.UseFont = false;
            this.xrAccountValue.StylePriority.UseTextAlignment = false;
            this.xrAccountValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrCreatedValue
            // 
            this.xrCreatedValue.Angle = 90F;
            this.xrCreatedValue.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Created:\'+[CreatedDate]")});
            this.xrCreatedValue.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrCreatedValue.LocationFloat = new DevExpress.Utils.PointFloat(88F, 0F);
            this.xrCreatedValue.Name = "xrCreatedValue";
            this.xrCreatedValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(1F, 1F, 0F, 0F, 100F);
            this.xrCreatedValue.SizeF = new System.Drawing.SizeF(8F, 313.88F);
            this.xrCreatedValue.StylePriority.UseFont = false;
            this.xrCreatedValue.StylePriority.UseTextAlignment = false;
            this.xrCreatedValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.LotLabelDto);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // LotLabelReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.DefaultPrinterSettingsUsing.UseLandscape = true;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.Margins = new DevExpress.Drawing.DXMargins(2F, 2F, 5F, 5F);
            this.PageHeightF = 350F;
            this.PageWidthF = 150F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        private XRLabel xrLabel1;
        private XRBarCode xrBarCode1;
        private XRPictureBox xrPictureBox1;
        private XRLabel xrLabel2;
    }
}
