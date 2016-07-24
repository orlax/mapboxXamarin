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
using App2;
using App2.Droid;
using Xamarin.Forms.Platform.Android;
using Mapbox.Maps;
using Android.Graphics;
using System.Threading.Tasks;
using Mapbox.Camera;
using Mapbox.Annotations;
using Mapbox.Geometry;

[assembly: ExportRenderer(typeof(main), typeof(mainRenderer))]
namespace App2.Droid
{
    class mainRenderer : PageRenderer, TextureView.ISurfaceTextureListener
    {
        Android.Views.View view;
        static MapboxMap map;
        MapView mapView;
        Activity activity;

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;

            activity = this.Context as Activity;
            view = activity.LayoutInflater.Inflate(Resource.Layout.mapboxmap, this, false);
            mapView = view.FindViewById<MapView>(Resource.Id.mapView);

            setupMap(activity);

            AddView(view);
        }

        private async void setupMap(Activity activity)
        {
            //basic map setup 
            mapView.OnCreate(null);
            mapView.StyleUrl = Mapbox.Constants.Style.Emerald;
            map = await mapView.GetMapAsync();

            // moving point of view with camera object
            var position = new CameraPosition.Builder()
            .Target(new LatLng(41.885, -87.679)) // Sets the new camera position
            .Zoom(11) // Sets the zoom
            .Build(); // Creates a CameraPosition from the builder

            map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(position), 3000);//animates map to new position

            //adding a marker
            map.AddMarker(new MarkerOptions() // new marker option  
            .SetTitle("Test Marker") // market title
            .SetPosition(new LatLng(41.885, -87.679))); // marker position on the map.

            map.SetOnMarkerClickListener(new Listener());
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            view.Measure(msw, msh);
            view.Layout(0, 0, r - l, b - t);
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
           
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            
        }

        public class Listener : Java.Lang.Object, MapboxMap.IOnMarkerClickListener
        {
            public bool OnMarkerClick(Marker marker)
            {
                var newPosition = new CameraPosition.Builder()
                .Target(new LatLng(marker.Position.Latitude,marker.Position.Longitude)) // Sets the new camera position
                .Zoom(18) // Sets the zoom
                .Build(); // Creates a CameraPosition from the builder
                map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(newPosition), 3000);//animates map to new position

                String a = marker.Title;
                MessagingCenter.Send<String>(a, "markerClicked");

                return false;
            }
        }


    }
}