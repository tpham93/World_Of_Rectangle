using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace sat.Etc
{
    class IntersectData
    {
        private bool intersects;
        private Vector2 normalDirection;
        private float distance;
        //private Vector2 contactPoint;


        public bool Intersects
        {
            get { return intersects; }
        }
        public Vector2 Mtv
        {
            get { return normalDirection; }
            set { normalDirection = value; }
        }
        public float Distance
        {
            get { return distance; }
        }
        //public Vector2 ContactPoint
        //{
        //    get { return contactPoint; }
        //}
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vectorData"></param>
        /// <param name="contactPoint"></param>
        public IntersectData(VectorData vectorData/*, Vector2 contactPoint*/)
        {
            this.intersects = true;
            this.normalDirection = vectorData.direction;
            this.distance = vectorData.length;
            //this.contactPoint = contactPoint;
        }
        /// <summary>
        /// Constructor - no Intersection
        /// </summary>
        public IntersectData()
        {
            this.intersects = false;
            this.normalDirection = Vector2.Zero;
            this.distance = 0f;
        }
    }
}
