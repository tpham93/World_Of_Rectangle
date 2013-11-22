﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using sat.Etc;

namespace sat.Shape
{
    class EdgeShape : Shape2D
    {

        Vector2[] corners;
        Vector2[] currentCorners;
        List<Vector2> normals;
        List<Vector2> currentNormals;

        /// <summary>
        /// gets the ObjectType
        /// </summary>
        public override EShapeType ShapeType
        {
            get { return EShapeType.EdgeShape; }
        }

        /// <summary>
        /// gets & sets the position
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                for (int i = 0; i < corners.Length; ++i)
                {
                    currentCorners[i] = currentCorners[i] - position + value;
                }
                this.position = value;
            }
        }

        /// <summary>
        /// gets & sets the rotation
        /// </summary>
        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                this.rotation = value;
                float rotationSin = (float)Math.Sin(rotation);
                float rotationCos = (float)Math.Cos(rotation);
                for (int i = 0; i < corners.Length; ++i)
                {
                    currentCorners[i] = Helper.rotateVector2(corners[i], rotationCos, rotationSin) + position;
                }
                for (int i = 0; i < normals.Count; ++i)
                {
                    currentNormals[i] = Helper.rotateVector2(normals[i], rotationCos, rotationSin);
                }
            }
        }

        public EdgeShape(Vector2[] corners, Texture2D texture, Color color, Vector2 position, Vector2 centerOfMass, bool moveable = true)
            : base(color, new Vector2(texture.Width / 2, texture.Height / 2).Length(), position, new Vector2(texture.Width / 2, texture.Height / 2), moveable)
        {
            this.corners = new Vector2[corners.Length];
            this.currentCorners = new Vector2[corners.Length];
            this.normals = new List<Vector2>(corners.Length);
            this.currentNormals = new List<Vector2>(corners.Length);

            for (int i = 0; i < corners.Length; ++i)
            {
                this.corners[i] = new Vector2(corners[i].X, corners[i].Y) - centerOfMass;
                this.currentCorners[i] = this.corners[i] + position;
            }
            for (int i = 1; i <= corners.Length; ++i)
            {
                Vector2 edge = this.corners[i - 1] - this.corners[i % corners.Length];
                Vector2 normal = Vector2.Normalize(new Vector2(-edge.Y, edge.X));
                normals.Add(normal);
                currentNormals.Add(normal);
            }
        }

        /// <summary>
        /// calculate the range when projected to a vector
        /// </summary>
        /// <param name="vector">the vector to which the object is beeing projected</param>
        /// <returns>range of the projection</returns>
        public override Range getProjectionRange(Vector2 vector)
        {
            float min = float.PositiveInfinity;
            float max = float.NegativeInfinity;
            for (int i = 0; i < currentCorners.Length; ++i)
            {
                float value = Vector2.Dot(vector, currentCorners[i]);
                if (value < min)
                {
                    min = value;
                }
                else if (value > max)
                {
                    max = value;
                }
            }
            return new Range(min, max);
        }

        /// <summary>
        /// checks if the object is intersecting with another CircleObject
        /// </summary>
        /// <param name="o">EdgeObject which is to be checked for an intersection</param>
        /// <returns>true if it intersects</returns>
        public override IntersectData intersects(EdgeShape o)
        {
            VectorData mtv = new VectorData();
            mtv.length = float.PositiveInfinity;
            Vector2 minimumDistanceCorner = currentCorners[0];
            foreach (Vector2 n in currentNormals)
            {
                Vector2 possibleMtv = n;

                Range r1 = getProjectionRange(possibleMtv);
                Range r2 = o.getProjectionRange(possibleMtv);

                float distance = Range.distance(r1, r2);

                if (distance >= 0)
                {
                    return new IntersectData();
                }
                else if (mtv.length > -distance)
                {
                    mtv.length = -distance;
                    mtv.direction = possibleMtv;
                }

            }
            foreach (Vector2 n in o.currentNormals)
            {
                Vector2 possibleMtv = n;

                Range r1 = getProjectionRange(possibleMtv);
                Range r2 = o.getProjectionRange(possibleMtv);

                float distance = Range.distance(r1, r2);

                if (distance >= 0)
                {
                    return new IntersectData();
                }
                else if (mtv.length > -distance)
                {
                    mtv.length = -distance;
                    mtv.direction = possibleMtv;
                }
            }

            return new IntersectData(mtv/*, minimumDistanceCorner - mtv.length * mtv.direction*/);
        }

        /// <summary>
        /// checks if the object is intersecting with another CircleObject
        /// </summary>
        /// <param name="o">CircleObject which is to be checked for an intersection</param>
        /// <returns>true if it intersects</returns>
        public override IntersectData intersects(CircleShape o)
        {
            VectorData mtv = new VectorData();
            mtv.length = float.PositiveInfinity;

            for (int i = 0; i < currentCorners.Length; ++i)
            {
                Vector2 possibleMtv = Vector2.Normalize(currentCorners[i] - o.Position);

                Range r1 = getProjectionRange(possibleMtv);
                Range r2 = o.getProjectionRange(possibleMtv);

                float distance = Range.distance(r1, r2);

                if (distance > 0)
                {
                    return new IntersectData();
                }
                else if (mtv.length > -distance)
                {
                    mtv.length = -distance;
                    mtv.direction = possibleMtv;
                }
            }
            foreach (Vector2 n in currentNormals)
            {
                Vector2 possibleMtv = n;

                Range r1 = getProjectionRange(possibleMtv);
                Range r2 = o.getProjectionRange(possibleMtv);

                float distance = Range.distance(r1, r2);

                if (distance > 0)
                {
                    return new IntersectData();
                }
                else if (mtv.length > -distance)
                {
                    mtv.length = -distance;
                    mtv.direction = possibleMtv;
                }
            }


            if (Vector2.Dot(mtv.direction, o.Position - Position) < 0)
            {
                mtv.direction *= -1;
            }

            return new IntersectData(mtv/*, -mtv.direction * o.Radius + o.Position*/);
        }

        /// <summary>
        /// checks if a point is inside of the object
        /// </summary>
        /// <param name="point">Point which needs to be checked</param>
        /// <returns>true if point is inside of the object</returns>
        public override bool contains(Vector2 point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// generates a texture based on the input data
        /// </summary>
        /// <param name="corners">corners the shape should have in CW</param>
        /// <param name="fillPoint">the point where the function begins to fill the texture</param>
        /// <param name="width">the texture's width</param>
        /// <param name="height">the texture's height</param>
        /// <param name="color">the tecture's filling color</param>
        /// <returns>a texture for an EdgeObject</returns>
        public static Texture2D genTexture(Vector2[] corners, Vector2 fillPoint, int width, int height, Color color)
        {
            Texture2D pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            RenderTargetBinding[] originalRenderTarget = graphicsDevice.GetRenderTargets();

            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            graphicsDevice.SetRenderTarget(renderTarget);

            graphicsDevice.Clear(Color.Transparent);

            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
            spriteBatch.Begin();
            for (int i = 1; i <= corners.Length; ++i)
            {
                Vector2 startPoint = corners[i - 1];
                Vector2 edge = corners[i % corners.Length] - corners[i - 1];
                spriteBatch.Draw(pixel, new Rectangle((int)startPoint.X, (int)startPoint.Y, (int)Math.Ceiling(edge.Length()), 1), null, color, (float)Helper.getAngleFromVector2(edge), Vector2.Zero, SpriteEffects.None, 0);
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTargets(originalRenderTarget);

            Stack<Point> nextPoints = new Stack<Point>();
            nextPoints.Push(new Point((int)fillPoint.X, (int)fillPoint.Y));

            Color[] pixels = new Color[width * height];

            renderTarget.GetData<Color>(pixels);

            while (nextPoints.Count > 0)
            {
                Point currentPos = nextPoints.Pop();
                int index = currentPos.X + currentPos.Y * width;
                if (pixels[index] == Color.Transparent)
                {
                    pixels[index] = color;
                    if (currentPos.X > 0 && pixels[index - 1] == Color.Transparent)
                    {
                        nextPoints.Push(new Point(currentPos.X - 1, currentPos.Y));
                    }
                    if (currentPos.X < width - 1 && pixels[index + 1] == Color.Transparent)
                    {
                        nextPoints.Push(new Point(currentPos.X + 1, currentPos.Y));
                    }
                    if (currentPos.Y > 0 && pixels[index - width] == Color.Transparent)
                    {
                        nextPoints.Push(new Point(currentPos.X, currentPos.Y - 1));
                    }
                    if (currentPos.Y < height - 1 && pixels[index + width] == Color.Transparent)
                    {
                        nextPoints.Push(new Point(currentPos.X, currentPos.Y + 1));
                    }
                }
            }
            renderTarget.SetData<Color>(pixels);

            return renderTarget;
        }

    }
}
