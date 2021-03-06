﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAF;
using GAF.Operators;
using GAF.Extensions;
using GAF.Threading;
using System.Windows.Forms;
using System.Drawing;
using AlgoritmosGeneticos.Domain;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using AlgoritmosGeneticos.CustomOperators;

namespace AlgoritmosGeneticos
{
    class GAFRunner 
    {
        //GAFRunner es un Singleton
        private static GAFRunner instance;
        private Logger logger;
        private long fitnessRequired;
        public ProgressBar progress;
        internal int cantIteraciones;
        private double bestFitness = -1;
        private List<DataPoint> puntos = new List<DataPoint>();
        private Stopwatch reloj = new Stopwatch();
     
        private GAFRunner() 
        {
            this.logger = Logger.Instance;
        }

        public static GAFRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GAFRunner();
                }
                return instance;
            }
        }

        //En este metodo se configura los operadores y sus parametros para la corrida
        public void correrAlgoritmoGenetico(ProgressBar progressBar, int cantPoblacion, int cantIteraciones)
        {
            reloj.Start();
            this.progress = progressBar;
            this.cantIteraciones = cantIteraciones;
            this.fitnessRequired = 630;

            var population = new Population(populationSize: cantPoblacion,
              chromosomeLength: 63,
              reEvaluateAll: true,
              useLinearlyNormalisedFitness: true,
              selectionMethod: ParentSelectionMethod.FitnessProportionateSelection);


            var crossover = new Crossover(0.4)
            {
                AllowDuplicates = true,
                CrossoverType = CrossoverType.SinglePoint
            };

            var binaryMutate = new BinaryMutate(mutationProbability: 0.05D, allowDuplicates: true);
            var randomReplace = new RandomReplace(numberToReplace: 9, allowDuplicates: true);
            var elite = new Elite(1);
            var tempMutate = new MutacionPorTemperatura(0.01D, 0.3D, cantIteraciones, true);
            var ga = new GeneticAlgorithm(population, CalculateFitness)
            {
                UseMemory = false
            };

            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;

            //aca se agregan los operadores para el algoritmo genetico

            ga.Operators.Add(crossover);
            //ga.Operators.Add(randomReplace);
            ga.Operators.Add(tempMutate);
            //ga.Operators.Add(binaryMutate);
            ga.Operators.Add(elite);
            ga.Run(Terminate);
        }

        void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            puntos.Add(new DataPoint(e.Generation,e.Population.MaximumFitness));
            asignarMejorPoblacion(e);
            this.progress.PerformStep();
        }

        void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            reloj.Stop();
            this.logger.appendText(Color.Tomato, "TERMINO LA CORRIDA ("+ reloj.ElapsedMilliseconds + "ms)", true);
            this.logger.loguearResultados(e, cantIteraciones);
            Grafica grafica = new Grafica(puntos);
            grafica.Show();
        }

        private bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return population.MaximumFitness >= fitnessRequired || currentGeneration == this.cantIteraciones;
        }

        private void asignarMejorPoblacion(GaEventArgs e)
        {
            Chromosome cromo = e.Population.Solutions.Find(x => x.Fitness == e.Population.MaximumFitness);
            if (cromo.Fitness > bestFitness)
            {
                bestFitness = cromo.Fitness;
                this.logger.asignarMejorSolucion(cromo, e.Generation, e.Evaluations);
            }
        }

        private double CalculateFitness(Chromosome chromosome)
        {
            double fitnessValue = -1;
            
            if (chromosome != null)
            {
                if (chromosome.Count == 63)
                {
                    fitnessValue = Acertijo.Instance.FitnessFunction(chromosome);
                }
                else
                {
                    throw new ApplicationException("The Chromosome length is incorrect.");
                }
            }
            else
            {
                //chromosome is null
                throw new ArgumentNullException("chromosome", "The specified Chromosome is null.");
            }
            
            return fitnessValue;
        }
    }
}
