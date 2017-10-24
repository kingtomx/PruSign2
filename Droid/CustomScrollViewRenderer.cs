using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using PruSign;
using PruSign.Droid;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomScrollView), typeof(CustomScrollViewRenderer))]

namespace PruSign.Droid
{
    class CustomScrollViewRenderer : ScrollViewRenderer
    {
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return false; 
        }
    }
}