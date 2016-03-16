using System;
using System.Collections.Generic;
using Math_Implementation;

namespace GameApplication {
    class ParticleSystem {
        protected List<Particle> particleList;
        protected int maxParticles;
        protected int numParticles;
        protected Vector3 systemOrigin;
        protected float accumulatedTime;
        protected Random random;

        public ParticleSystem(int maxParticles,Vector3 origin) {
            particleList = new List<Particle>();
            this.maxParticles = maxParticles;
            systemOrigin = new Vector3(origin.X, origin.Y, origin.Z);
            numParticles = 0;
            accumulatedTime = 0f;
            random = new Random();

            for (int i = 0; i < maxParticles; i++) {
                particleList.Add(new Particle());
            }
        }
        public virtual void Update(float dTime) {

        }
        public virtual void Render() {

        }
        public virtual void Shutdown() {

        }
        public virtual void InitParticle(int index) {
            Console.WriteLine("ParticleSystem.initParticle should never be called directly");
            Console.WriteLine("only subclasses should call this function!");
        }
        public int Emit(int request) {
            //if we can make particles, create them
            while (request > 0 && (numParticles < maxParticles)) {
                //initialize a particle, and increase the count
                InitParticle(numParticles++);
                request--;
            }
            //return how many particles were not created
            return request;
        }
    }
}
