﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using sat.Etc;

namespace sat.Shape
{
    abstract class Shape2D
    {

        /*
         * protected static Member
         */
        protected static GraphicsDevice graphicsDevice;


        /*
         * Xna-specific attributes for drawing and handling calculations
         */
        protected Vector2 middlePoint;

        public abstract EShapeType ShapeType
        {
            get;
        }

        /*
         * Physic-spcefic attributes for Calculations
         */
        protected float radius;

        protected Vector2 position;
        public abstract Vector2 Position
        {
            get;
            set;
        }

        protected float rotation;
        public abstract float Rotation
        {
            get;
            set;
        }

        protected bool moveable;

        /// <summary>
        /// Constructor
        /// </summary>
        protected Shape2D()
        {
            this.radius = 0f;
            this.position = Vector2.Zero;
            this.middlePoint = Vector2.Zero;
            this.moveable = true;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        protected Shape2D(float radius, Vector2 position, Vector2 middlePoint, bool moveable)
            : this()
        {
            this.radius = radius;
            this.position = position;
            this.middlePoint = middlePoint;
            this.moveable = moveable;
        }

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            Shape2D.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// checks if the object is intersecting with another object
        /// </summary>
        /// <param name="o">Object which is to be checked for an intersection</param>
        /// <returns>true if it intersects</returns>
        public IntersectData intersects(Shape2D o)
        {
            if (moveable)
            {
                float minDistance = this.radius + o.radius;
                if (minDistance * minDistance >= (o.Position - Position).LengthSquared())
                {
                    IntersectData intersectData = null;
                    switch (o.ShapeType)
                    {
                        case EShapeType.EdgeShape:
                            intersectData = intersects((EdgeShape)o);
                            break;
                        case EShapeType.CircleShape:
                            intersectData = intersects((CircleShape)o);
                            break;
                        default:
                            return null;
                    }

                    if (Vector2.Dot(intersectData.Mtv, -o.Position + Position) < 0)
                    {
                        intersectData.Mtv *= -1;
                    }
                    return intersectData;
                }
            }
            return new IntersectData();
        }

        public abstract Range getProjectionRange(Vector2 v);


        public abstract IntersectData intersects(EdgeShape o);
        public abstract IntersectData intersects(CircleShape o);

        /// <summary>
        /// checks if a point is inside of the object
        /// </summary>
        /// <param name="point">Point which needs to be checked</param>
        /// <returns>true if point is inside of the object</returns>
        public abstract bool contains(Vector2 point);

        public static void handleCollision(IntersectData data, Shape2D o1, Shape2D o2)
        {
            if (o1.moveable)
            {
                if (o2.moveable)
                {
                    o1.Position += (data.Mtv * data.Distance / 2);
                    o2.Position -= (data.Mtv * data.Distance / 2);

                }
                else
                    o1.Position += (data.Mtv * data.Distance);
            }
        }
    }
}
