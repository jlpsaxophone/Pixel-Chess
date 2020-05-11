using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace MonoGameWindowsStarter
{
    public class Particle
    {
        /// <summary>
        /// Particle texture
        /// </summary>
        public Texture2D Texture { get; set; }
        /// <summary>
        /// Particles current position 
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Particles speed at the current instance
        /// </summary>
        public Vector2 Velocity { get; set; }
        /// <summary>
        /// Particles angle of rotation 
        /// </summary>
        public float Angle { get; set; }
        /// <summary>
        /// Speed of angle change
        /// </summary>
        public float AngularVelocity { get; set; }
        /// <summary>
        /// Particle color
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Particle size
        /// </summary>
        public float Size { get; set; }
        /// <summary>
        /// particle time to live
        /// </summary>
        public int TTL { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">texture for the particle</param>
        /// <param name="position">position of the particle</param>
        /// <param name="velocity">speed of the particle</param>
        /// <param name="angle">angle of the particle</param>
        /// <param name="angularVelocity">speed at which the angle changes</param>
        /// <param name="color">color of the particle</param>
        /// <param name="size">size fo the particle</param>
        /// <param name="ttl">tme to live fo the particle</param>
        public Particle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
        }

        /// <summary>
        /// Update the particle
        /// </summary>
        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        /// <summary>
        /// draw the particle
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}
   
}
