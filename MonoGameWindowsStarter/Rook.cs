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
    class Rook : IPiece
    {
        const int FRAME_RATE = 120;

        Texture2D texture;

        ParticleSystem attackParticles;

        ParticleSystem moveParticles;

        ParticleSystem deathParticles;

        SoundEffect attackSound;

        SoundEffect moveSound;

        SoundEffect deathSound;

        Vector2 positionCurrent;

        Vector2 positionDestination;

        bool drawMovement;

        public bool Selected => drawMovement;

        AnimationState state;

        TimeSpan animationTime;

        public Rook(Vector2 position, Texture2D texture, SoundEffect attackSound, SoundEffect moveSound, SoundEffect deathSound)
        {
            //Set positions
            positionCurrent = position;
            positionDestination = position;

            //Set textures
            this.texture = texture;

            //Set sounds
            this.attackSound = attackSound;
            this.moveSound = moveSound;
            this.deathSound = deathSound;

            //Set misc
            drawMovement = false;
            state = AnimationState.Idle0;
            animationTime = new TimeSpan(0);
        }

        public void Select()
        {
            //Turn on drawing movement
            drawMovement = true;
        }

        public void Move(Vector2 positionNew)
        {
            //Turn off drawing movement
            drawMovement = false;

            //Set new position
            positionDestination = positionNew;
        }

        public void Kill()
        {
            state = AnimationState.Dead;
            deathSound.Play();
        }

        public bool CollidesWithPiece(Point location)
        {
            if ((location.X >= positionCurrent.X && location.X <= positionCurrent.X + 64) &&
                (location.Y >= positionCurrent.Y && location.Y <= positionCurrent.Y + 64))
                return true;
            return false;
        }

        public void Update(GameTime gameTime)
        {
            if (state != AnimationState.Dead)
            {
                //Update animationTime
                animationTime += gameTime.ElapsedGameTime;

                //Update animation state
                if (animationTime.TotalMilliseconds >= FRAME_RATE)
                {
                    state++;
                    if (state == AnimationState.Dead) state = AnimationState.Idle0;
                    animationTime = new TimeSpan(0);
                }

                //Check if piece should be moved
                if (positionCurrent != positionDestination && animationTime.TotalMilliseconds == 0)
                {
                    positionCurrent = positionDestination;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (state != AnimationState.Dead)
            {
                //Draw piece
                Rectangle source = new Rectangle((int)state * 64, 0, 64, 64);
                spriteBatch.Draw(texture, positionCurrent, source, Color.White);

                //Draw movement if selected
                if (drawMovement)
                {

                }
            }
        }
    }
}
