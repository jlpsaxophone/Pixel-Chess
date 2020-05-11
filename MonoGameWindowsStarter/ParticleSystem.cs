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
        /// Constructor
        /// </summary>
        /// <param name="textures">List of textures</param>
        /// <param name="location"></param>
        public ParticleSystem(List<Texture2D> textures, Vector2 location)
        {
            particleCount = 10; 
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }

        /// <summary>
        /// Update 
        /// </summary>
        public void Update()
        {
            int total = particleCount; 

            for (int i = 0; i < total; i++)
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

        /// <summary>
        /// Private method for generating new particles
        /// </summary>
        /// <returns></returns>
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1),
                                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 20 + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        /// <summary>
        /// Draw the particles
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
