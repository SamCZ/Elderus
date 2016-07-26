using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world.gen.noise {
    public class SimplexNoise {

        private SimplexNoiseOctave[] octaves;
	    private double[] frequencys;
	    private double[] amplitudes;

	    private int largestFeature;
	    private double persistence;
	
	    //TODO: Seed
	    private int seed;

        public SimplexNoise(int largestFeature, double persistence, int seed) {
            this.largestFeature = largestFeature;
            this.persistence = persistence;
            this.seed = seed;

            // recieves a number (eg 128) and calculates what power of 2 it is (eg 2^7)
            int numberOfOctaves = (int)Math.Ceiling(Math.Log10(this.largestFeature) / Math.Log10(2));

            this.octaves = new SimplexNoiseOctave[numberOfOctaves];
            this.frequencys = new double[numberOfOctaves];
            this.amplitudes = new double[numberOfOctaves];

            System.Random rnd = new System.Random(seed);

            for (int i = 0; i < numberOfOctaves; i++) {
                this.octaves[i] = new SimplexNoiseOctave(rnd.Next());
                this.frequencys[i] = Math.Pow(2, i);
                this.amplitudes[i] = Math.Pow(this.persistence, this.octaves.Length - i);
            }

        }

        public double getNoise(int x, int y) {
            double result = 0;

            for (int i = 0; i < this.octaves.Length; i++) {
                // double frequency = Math.pow(2,i);
                // double amplitude = Math.pow(persistence,octaves.length-i);

                result = result + this.octaves[i].noise(x / this.frequencys[i], y / this.frequencys[i]) * this.amplitudes[i];
            }

            return result;
        }

        public double getNoise(int x, int y, int z) {

            double result = 0;

            for (int i = 0; i < this.octaves.Length; i++) {
                double frequency = Math.Pow(2, i);
                double amplitude = Math.Pow(this.persistence, this.octaves.Length - i);

                result = result + this.octaves[i].noise(x / frequency, y / frequency, z / frequency) * amplitude;
            }

            return result;
        }

    }
}
