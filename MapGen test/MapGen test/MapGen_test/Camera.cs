using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGen_test
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        private Vector2 centre;
        public Vector2 Centre
        {
            get { return centre; }
        }

        public Camera(Viewport newView)
        {
            view = newView;
        }
        private float zoom;
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }

        }
        public decimal xZoomOffset, yZoomOffSet;
        public void Update(int x, int y,int xOffset,int yOffset)
        {

            
            centre = new Vector2(x, y);
            xZoomOffset = 0; yZoomOffSet = 0;


            if (x > view.Width / 2)
            {
                xZoomOffset = (decimal)(((x- view.Width / 2) ));
            }

                if (y > view.Height / 2)
                    yZoomOffSet = (decimal)(((y - view.Height/2) ));
            

            if (x < view.Width / 2)
                centre.X = view.Width / 2;
            else
                centre.X = x;

            if (y < view.Height / 2)
                centre.Y = view.Height / 2;
            else
                centre.Y = y ;

            if (zoom < 0.1f)
                zoom = 0.1f;
            if ((view.Width/2) / zoom +(float)xZoomOffset> xOffset&&((view.Height/2)/zoom + (float)yZoomOffSet > yOffset))
                zoom+=.01f;

            transform = Matrix.CreateTranslation(new Vector3((-(centre.X) + (view.Width / 2))/zoom, (-(centre.Y) + (view.Height / 2)) / zoom, 0))
                *Matrix.CreateScale(new Vector3(zoom, zoom, 0)) ;

            //transform = Matrix.CreateTranslation(new Vector3(-(int)Math.Round(((decimal)(x / Zoom) + xZoomOffset / (decimal)Zoom)),
            //            -(int)Math.Round(((decimal)(y / Zoom) + (yZoomOffSet / (decimal)Zoom))), 0)) *
            //            Matrix.CreateScale(new Vector3(zoom, zoom, 0))*Matrix.CreateTranslation(new Vector3(view.Width/2,view.Height/2,0));


        }


    }
}
