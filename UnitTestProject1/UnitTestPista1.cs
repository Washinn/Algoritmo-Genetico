﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoritmosGeneticos.Domain;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestPista1
    {
        [TestMethod]
        public void TestCondicionVerdadera1()
        {
            List<Modelo> modelos = new List<Modelo>();
            modelos.Add(new Modelo("Chevrolet Corsa", "Blanco", 4, "tostadora"));
            modelos.Add(new Modelo("Fiat Uno", "Negro", 3, "jaula"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 4, "pelota de futbol"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 4, "pelota de futbol"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 5, "pelota de futbol"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 5, "palo de golf"));

            Pista1 pista = new Pista1();
            Assert.AreEqual(1, pista.validar(modelos));
        }

        [TestMethod]
        public void TestCondicionFalsa1()
        {
            List<Modelo> modelos = new List<Modelo>();
            modelos.Add(new Modelo("Chevrolet Corsa", "Blanco", 2, "jaula"));
            modelos.Add(new Modelo("Fiat Uno", "Negro", 3, "notebook"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 5, "palo de golf"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 1, "pelota de futbol"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 2, "palo de golf"));
            modelos.Add(new Modelo("Toyota Corolla", "Rojo", 1, "pelota de futbol"));

            Pista1 pista = new Pista1();
            Assert.AreEqual(-1, pista.validar(modelos));
        }
    }
}
