using System;
using System.Collections.Generic;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;
using Newtonsoft.Json;


namespace PruSign.iOS
{
	public class DrawView : UIView
	{


		private PointF PreviousPoint;
		private CGPath DrawPath;
		private byte IndexCount;
		private UIBezierPath CurrentPath;
		private List<VESLine> Lines;
		public UIColor CurrentLineColor { get; set; }
		public float PenWidth { get; set; }
		public List<PointWhen> points;





		public DrawView(RectangleF frame) : base(frame)
		{
			DrawPath = new CGPath();
			CurrentLineColor = UIColor.Black;
			PenWidth = 2.0f;
			Lines = new List<VESLine>();
			points = new List<PointWhen>();
			UITapGestureRecognizer doubletap = new UITapGestureRecognizer(OnDoubleTap)
			{
				NumberOfTapsRequired=2
			};
			this.AddGestureRecognizer(doubletap);
		}

		private void OnDoubleTap(UIGestureRecognizer gesture)
		{
			Lines.Clear();
		}

		public void Clear()
		{
			DrawPath.Dispose();
			DrawPath = new CGPath();
			SetNeedsDisplay();
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			IndexCount++;

			var path = new UIBezierPath
			{
				LineWidth = PenWidth
			};

			var touch = (UITouch)touches.AnyObject;
			PreviousPoint = (System.Drawing.PointF)touch.PreviousLocationInView(this);

			var newPoint = touch.LocationInView(this);
			path.MoveTo(newPoint);

			InvokeOnMainThread(SetNeedsDisplay);

			CurrentPath = path;

			var line = new VESLine
			{
				Path = CurrentPath,
				Color = CurrentLineColor,
				Index = IndexCount
			};

			Lines.Add(line);
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			var touch = (UITouch)touches.AnyObject;
			var currentPoint = touch.LocationInView(this);

			if (Math.Abs(currentPoint.X - PreviousPoint.X) >= 4 ||
				Math.Abs(currentPoint.Y - PreviousPoint.Y) >= 4)
			{

				var newPoint = new PointF(((float)currentPoint.X + (float)PreviousPoint.X) / 2, ((float)currentPoint.Y + (float)PreviousPoint.Y) / 2);
				PointWhen customPoint = new PointWhen
				{
					point = newPoint,
					when = System.DateTime.Now.Ticks
				};
				points.Add(customPoint);

				CurrentPath.AddQuadCurveToPoint(newPoint, PreviousPoint);
				PreviousPoint = (System.Drawing.PointF)currentPoint;
			}
			else {
				CurrentPath.AddLineTo(currentPoint);
			}

			InvokeOnMainThread(SetNeedsDisplay);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{

			//Aqui podemos crear una carpeta para firmas temporales
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var directoryname = System.IO.Path.Combine(documents, "temporalSignatures");
			System.IO.Directory.CreateDirectory(directoryname);

			byte[] imageByteArray = CapturePNG(1.0, this);
			NSData imgData = ToUIImage(imageByteArray).AsPNG();
			long ticks = System.DateTime.Now.Ticks;
			string pngFilename = System.IO.Path.Combine(directoryname, "signature.png");
			NSError err = null;
			imgData.Save(pngFilename, false, out err);

			// Salvamos tambien el fichero de puntos
			string pointsFilename = System.IO.Path.Combine(directoryname, "points.json");
			String pointsString = JsonConvert.SerializeObject(points);
			System.IO.File.WriteAllText(pointsFilename, pointsString);

			InvokeOnMainThread(SetNeedsDisplay);
		}


		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			InvokeOnMainThread(SetNeedsDisplay);
		}


		public override void Draw(CGRect rect)
		{
			foreach (var line in Lines)
			{
				line.Color.SetStroke();
				line.Path.Stroke();
			}
		}


		public byte[] CapturePNG(double scale, UIView area)
		{
			
			//UIImage image = UIViewExtensions.TakeScreenshot();
			UIImage image = UIViewExtensions.AsImage(area);
			UIImage scaled = image;
			if (scale != 1.0)
			{
				scaled = image.Scale(new SizeF((float)(image.Size.Width * scale), (float)(image.Size.Height * scale)));
			}
			NSData png = scaled.AsPNG();
			byte[] dataBytes = new byte[png.Length];

			System.Runtime.InteropServices.Marshal.Copy(png.Bytes, dataBytes, 0, Convert.ToInt32(png.Length));
			return dataBytes;
		}


		public UIImage ToUIImage(byte[] data)
		{
			if (data == null)
			{
				return null;
			}
			UIImage image = null;
			try
			{
				image = new UIImage(NSData.FromArray(data));
				data = null;
			}
			catch (Exception)
			{
				return null;
			}
			return image;
		}


		public override void MotionEnded(UIEventSubtype motion, UIEvent evt)
		{
			if (motion == UIEventSubtype.MotionShake)
			{

				Clear();
			}

		}


	}
}