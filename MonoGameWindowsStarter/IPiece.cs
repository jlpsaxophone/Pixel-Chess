using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameWindowsStarter
{

    enum AnimationState
    {

    }

    /// <summary>
    /// A class representing a texture to render with a SpriteBatch
    /// </summary>
    public class IPiece
    {
        Texture2D[] textures;

        ParticleSystem attackParticles;

        ParticleSystem moveParticles;

        ParticleSystem deathParticles;

        SoundEffect attackSound;

        SoundEffect moveSound;

        SoundEffect deathSound;

        Vector2 positionCurrent;

        Vector2 positionDestination;

        bool drawMovement; 

        /// <summary>
        /// 
        /// </summary>
        public IPiece()
        {
            
        }

        public void Move()
        {

        }

        public void Select()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
