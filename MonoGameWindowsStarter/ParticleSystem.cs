using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    public enum ParticleType
    {
        Attack, 
        Movement, 
        Death
    }
    public class ParticleSystem
    {
        private Random random;
        /// <summary>
        /// Location of the emitter
        /// </summary>
        public Vector2 EmitterLocation { get; set; }
        /// <summary>
        /// List of particles
        /// </summary>
        private List<Particle> particles;
        /// <summary>
        /// List of textures
        /// </summary>
        private List<Texture2D> textures;

        /// <summary>
        /// Number of particles
        /// </summary>
        private int particleCount;

        /// <summary>
        /// Measuring elapsed time
        /// </summary>
        double elapsedTime;

        /// <summary>
        /// Particle systems TTL 
        /// </summary>
        double systemTTL;

        /// <summary>
        /// Size of the particles
        /// </summary>
        float particleSize;

        float angularVelocity;

        ParticleType particleType;  

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="textures">List of textures</param>
        /// <param name="location"></param>
        public ParticleSystem(List<Texture2D> textures, Vector2 location, ParticleType systemType)
        {
            this.particleType = systemType; 
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();

            systemTTL = 0;
            particleCount = 100;
            particleSize = (float)0;
            EmitterLocation = location;

            switch (this.particleType)
            {

                case ParticleType.Attack:
                    break;
                case ParticleType.Movement:
                    break;
                case ParticleType.Death:
                    systemTTL = 200;
                    particleCount = 150;
                    particleSize = (float)0.60;
                    break;
            }
        }

        /// <summary>
        /// Update 
        /// </summary>
        public void Update(Vector2 location, GameTime time)
        {
            elapsedTime += time.ElapsedGameTime.TotalMilliseconds;

            if(this.particleType == ParticleType.Death)
                this.EmitterLocation = new Vector2(location.X + 30, location.Y + 30);

            if (elapsedTime < systemTTL)
            {
                for (int i = 0; i < this.particleCount; i++)
                {
                    particles.Add(GenerateNewParticle());
                }

                for (int particle = 0; particle < particles.Count; particle++)
                {
                    particles[particle].Update();
                    if (particles[particle].TTL <= 0)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                    }
                }
            }
            else
            {
                particles = new List<Particle>(); 
            }
        }

        /// <summary>
        /// Private method for generating new particles
        /// </summary>
        /// <returns></returns>
        private Particle GenerateNewParticle()
        {
            float angle = (float)0;
            Vector2 velocity = new Vector2(0);
            float angularVelocity = (float)0;
            int particleTTL = 0;
            Texture2D texture = textures[random.Next(textures.Count)];

            switch (this.particleType)
            {

                case ParticleType.Attack:
                    break;
                case ParticleType.Movement:
                    break;
                case ParticleType.Death:
                    angle = 0;
                    particleTTL = 3;
                    angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                    //angularVelocity = 1000;
                    velocity = new Vector2(
                                    1f * (float)((double)random.Next(-15, 15)),
                                    1f * (float)((double)random.Next(-15, 15)));
                    //1f * (float)(random.NextDouble() * 12 - 1));
                    break;
            }
            return new Particle(texture, this.EmitterLocation, velocity, 
                                angle, angularVelocity, Color.White, 
                                this.particleSize, particleTTL);
        }

        /// <summary>
        /// Draw the particles
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}
