using Android.Views;
using Android.Graphics;
using Android.Content;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PruSign.Android
{
	public class DrawView : View
	{

		private static bool doubleTapped;
		private static long lastTouched = -1;
		private static long oneSecond = 10000000;
		private static long halfSecond = 5000000;

		public DrawView(Context context) : base(context)
		{

			//GestureDetector gestureDetector = new GestureDetector(context, new GestureListener());
			//gestureDetector.DoubleTap += (object sender, GestureDetector.DoubleTapEventArgs e) => {
			//	string aux = "";
			//};
			//apply touch to your view
			//this.Touch += (object sender, View.TouchEventArgs e) => {
			//       gestureDetector.OnTouchEvent (e.Event);
			//};


			Start();
		}

		public Color CurrentLineColor { get; set; }
		public float PenWidth { get; set; }
		private Path DrawPath;
		private Paint DrawPaint;
		private Paint DrawPaintReset;
		private Paint CanvasPaint;
		private Canvas DrawCanvas;
		private Bitmap CanvasBitmap;
		public List<Droid.PointWhen> points;



		private void Start()
		{

			CurrentLineColor = Color.Black;
			PenWidth = 3.0f;
			points = new List<Droid.PointWhen>();

			DrawPath = new Path();
			DrawPaint = new Paint
			{
				Color = CurrentLineColor,
				AntiAlias = true,
				StrokeWidth = PenWidth
			};
			DrawPaintReset = new Paint
			{
				Color = Color.White,
				AntiAlias = true,
				StrokeWidth = PenWidth
			};


			DrawPaint.SetStyle(Paint.Style.Stroke);
			DrawPaint.StrokeJoin = Paint.Join.Round;
			DrawPaint.StrokeCap = Paint.Cap.Round;

			CanvasPaint = new Paint
			{
				Dither = true
			};

		}


		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			CanvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
			DrawCanvas = new Canvas(CanvasBitmap);
		}


		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			DrawPaint.Color = CurrentLineColor;
			canvas.DrawBitmap(CanvasBitmap, 0, 0, CanvasPaint);
			canvas.DrawPath(DrawPath, DrawPaint);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{




			var touchX = e.GetX();
			var touchY = e.GetY();

			var newPoint = new PointF(touchX, touchY);
			Droid.PointWhen customPoint = new Droid.PointWhen
			{
				point = newPoint,
				when = System.DateTime.Now.Ticks
			};
			points.Add(customPoint);

			switch (e.Action)
			{
				case MotionEventActions.Down:
					// doubletap counter
					long nowTouched = System.DateTime.Now.Ticks;
					if (lastTouched != -1 && (nowTouched - lastTouched<halfSecond))
					{
						DrawCanvas.DrawPaint(DrawPaintReset);
						lastTouched = nowTouched;
					}
					else
					{
						lastTouched = nowTouched;
					}
					DrawPath.MoveTo(touchX, touchY);
					break;
				case MotionEventActions.Move:
					DrawPath.LineTo(touchX, touchY);
					break;
				case MotionEventActions.Up:
					DrawCanvas.DrawPath(DrawPath, DrawPaint);
					DrawPath.Reset();
					//Aqui podemos crear una carpeta para firmas temporales
					var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
					var directoryname = System.IO.Path.Combine(documents, "temporalSignatures");
					System.IO.Directory.CreateDirectory(directoryname);

					byte[] imageByteArray = CapturePNG(this);
					long ticks = System.DateTime.Now.Ticks;
					string pngFilename = System.IO.Path.Combine(directoryname, "signature.png");
					System.IO.File.WriteAllBytes(pngFilename, imageByteArray);


					// Salvamos tambien el fichero de puntos
					string pointsFilename = System.IO.Path.Combine(directoryname, "points.json");
					string pointsString = JsonConvert.SerializeObject(points);
					System.IO.File.WriteAllText(pointsFilename, pointsString);

					break;
				default:
					return false;
			}

			Invalidate();

			return true;
		}



		public byte[] CapturePNG(View view, bool autoScale = true)
		{
			view.BuildDrawingCache();
			var wasDrawingCacheEnabled = view.DrawingCacheEnabled;
			view.DrawingCacheEnabled = false;
			view.BuildDrawingCache(autoScale);
			var bitmap = view.GetDrawingCache(autoScale);
			view.DrawingCacheEnabled = wasDrawingCacheEnabled;
			byte[] bitmapData;
			using (var stream = new System.IO.MemoryStream())
			{
			    bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
			    bitmapData = stream.ToArray();
			}
			view.DestroyDrawingCache();
			return bitmapData;
		}

	}






}