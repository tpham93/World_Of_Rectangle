using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sat.Etc
{
    class Helper
    {

        static GraphicsDevice graphicsDevice;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            Helper.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// rotates a vector
        /// </summary>
        /// <param name="vector">vector which needs to be rotated</param>
        /// <param name="rotation">angle in rad</param>
        /// <returns>rotated vector</returns>
        public static Vector2 rotateVector2(Vector2 vector, float rotation)
        {
            // multplicate vector with rotationmatrix
            // (cos(a) -sin(a))
            // (sin(a)  cos(a))
            return rotateVector2(vector, (float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        /// <summary>
        /// rotates a vector
        /// </summary>
        /// <param name="vector">vector which needs to be rotated</param>
        /// <param name="rotationCos">cos of the angle</param>
        /// <param name="rotationSin">sin of the angle</param>
        /// <returns>rotated vector</returns>
        public static Vector2 rotateVector2(Vector2 vector, float rotationCos, float rotationSin)
        {
            // multplicate vector with rotationmatrix
            // (cos(a) -sin(a))
            // (sin(a)  cos(a))
            Vector2 tmp = Vector2.Normalize(new Vector2((vector.X * rotationCos + vector.Y * -rotationSin),(vector.X * rotationSin + vector.Y * rotationCos)));
            return tmp*vector.Length();
        }

        // calculates the angle of an Vector relatively to the y axis
        /// <summary>
        /// extracts the angle of a vector relatively to the y-axis
        /// </summary>
        /// <param name="v">the vector, from which the angle needs to be extracted</param>
        /// <returns>angle of the vector relatively to the y axis</returns>
        public static double getAngleFromVector2(Vector2 v)
        {
            return Math.Atan2(v.Y, v.X);
        }

        /// <summary>
        /// calculates the crossproduct of 2 dimensional vectors
        /// </summary>
        /// <param name="a">1st vector</param>
        /// <param name="b">2nd vector</param>
        /// <returns>scalar of the crossproduct</returns>
        public static float crossProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Texture2D genRectangle(int width, int height,Color color, Color outline)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, width, height);

            Color[] pixel = new Color[width * height];

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        pixel[x + y * width] = outline;
                    }
                    else
                    {
                        pixel[x + y * width] = color;
                    }
                }
            }
            renderTarget.SetData<Color>(pixel);
            return renderTarget;
        }

        public static Texture2D loadImage(String filename, Rectangle rect = new Rectangle(), Color color = new Color())
        {
            //Might throw FileNotFoundException
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Close();
            // returns original car if the rectangle has no size
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return texture;
            }
            else
            {
                // rendertarget to save the resized image
                RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, rect.Width, rect.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
                // create  spriteBatch to draw the resized image
                SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
                // save the earlier rendertargets
                RenderTargetBinding[] tmpRenderTargets = graphicsDevice.GetRenderTargets();
                graphicsDevice.SetRenderTarget(renderTarget);
                // draw the resized image on the rendertarget
                graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin();
                if (color.Equals(new Color()))
                {
                    spriteBatch.Draw(texture, rect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(texture, rect, color);
                }
                spriteBatch.End();
                // set back the saved rendertargets
                graphicsDevice.SetRenderTargets(tmpRenderTargets);
                // return the resized drawn Iimage
                return renderTarget;
            }
        }


    }
}
