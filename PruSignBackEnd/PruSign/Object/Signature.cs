using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruSignBackEnd.PruSign.Object
{
	public class Signature
	{

		public string image;
		public Points[] points;
		public string datetime;
		public string customerName;
		public string customerId;
		public string documentId;
		public string applicationId;
		public string hash;

	}

	public class Points
	{
		public Point point;
		public long when;
	}

	public class Point
	{
		public bool isEmpty;
		public float X;
		public float Y;
	}

}
