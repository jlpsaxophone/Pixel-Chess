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

        string team;

        Vector2 positionCurrent;
        Vector2 positionDestination;
        double travelDistance; 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="textures">List of textures</param>
        /// <param name="locationCurrent"></param>
        public ParticleSystem(List<Texture2D> textures, Vector2 locationCurrent, Vector2 locationDestination, ParticleType systemType, string side)
        {
            this.team = side; 
            this.particleType = systemType;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();

            systemTTL = 0;
            particleCount = 100;
            particleSize = (float)0;
            positionCurrent = locationCurrent;
            positionDestination = locationDestination;
            EmitterLocation = positionCurrent;
            travelDistance = FindDistance(positionCurrent, positionDestination);

            switch (this.particleType)
            {

                case ParticleType.Attack:
                    systemTTL = 200;
                    particleCount = 150;
                    particleSize = (float)0.60;
                    break;
                case ParticleType.Movement:
                    systemTTL = 200;
                    particleCount = 600;
                    particleSize = (float)0.01;
                    break;
                case ParticleType.Death:
                    systemTTL = 300;
                    particleCount = 150;
                    particleSize = (float)0.60;
                    break;
            }
        }

        /// <summary>
        /// Update 
        /// </summary>
        public void Update(Vector2 locationCurrent, GameTime time)
        {
            elapsedTime += time.ElapsedGameTime.TotalMilliseconds;

            if(this.particleType == ParticleType.Death)
                this.EmitterLocation = new Vector2(locationCurrent.X + 30, locationCurrent.Y + 30);
            if (this.particleType == ParticleType.Movement)
                this.EmitterLocation = new Vector2(locationCurrent.X, locationCurrent.Y + 50);

            if (elapsedTime < systemTTL)
            {
                for (int i = 0; i < this.particleCount; i++)
                {
                    //if (this.particleType == ParticleType.Movement && i % 400 == 0)
                        //this.EmitterLocation = new Vector2(locationCurrent.X + 5, locationCurrent.Y);
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
                    //EmitterLocation = positionDestination; 
                    angle = 0;
                    particleTTL = 3;
                    angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                    velocity = new Vector2(
                                        1f * (float)(random.NextDouble() * 2000 - 1),
                                        1f * (float)(random.NextDouble() * 2 - 1)); ;
                    break;
                case ParticleType.Movement:
                    angle = 0;
                    particleTTL = 10;
                    angularVelocity = 0;// .1f * (float)(random.NextDouble() * 2 - 1);
                    velocity = new Vector2(
                        1f * (float)(random.NextDouble() * (travelDistance * 50) - 1),
                        1f * (float)((double)random.Next(-15, 15)));
                    break;
                case ParticleType.Death:
                    angle = 0;
                    particleTTL = 3;
                    angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                    velocity = new Vector2(
                                    1f * (float)((double)random.Next(-15, 15)),
                                    1f * (float)((double)random.Next(-15, 15)));
                    break;
            }

            return new Particle(texture, this.EmitterLocation, velocity, 
                                angle, angularVelocity, Color.White, 
                                this.particleSize, particleTTL);
        }

        public Vector2 angleOf(Vector2 p1, Vector2 p2)
        {
            return new Vector2(p2.X - p1.X, p1.Y - p2.Y);
        }

        public double FindDistance(Vector2 p1, Vector2 p2)
        {
            return Math.Sqrt(((p2.X - p1.Y) * (p2.X - p1.Y)) + ((p2.Y - p1.Y) * (p2.Y - p1.Y)));
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
