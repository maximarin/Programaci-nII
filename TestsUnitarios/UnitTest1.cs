﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntidadesJuego;
using System.Collections.Generic;

namespace TestsUnitarios
{
    [TestClass]
    public class PartidasTest
    {
        [TestMethod]
        public void DeberíaAgregarLaPartidaCreadaALaListaPartidasDelJuego()
        {
            var juego = new Juego();
            var Partida = new Partida();
            Partida.Nombre = "Partida";
            juego.AgregarPartida(Partida);

            Assert.AreEqual(1, juego.Partidas.Count);
        }

        [TestMethod]
        public void DeberiaGanarJugadroDosPorAtributoConValorMasAlto()
        {
            var PartidaPrueba = new Partida();
            var Jugador1 = new Jugador().IdConexion("Id1").Nombre("Maxi").Numero(NumJugador.uno);
            var Jugador2 = new Jugador().IdConexion("Id2").Nombre("Lautaro").Numero(NumJugador.dos);
            PartidaPrueba.jugadores.Add(Jugador1);
            PartidaPrueba.jugadores.Add(Jugador2);

            var Atributo1 = new Atributo() { Nombre = "Fuerza", Valor = 20 };
            var Atributo2 = new Atributo() { Nombre = "Velocidad", Valor = 30 };
            var Atributo3 = new Atributo() { Nombre = "Fuerza", Valor = 24 };
            var Atributo4 = new Atributo() { Nombre = "Resistencia", Valor = 40 };

            List<Atributo> Lista1 = new List<Atributo>();
            Lista1.Add(Atributo1); Lista1.Add(Atributo3);
            List<Atributo> Lista2 = new List<Atributo>();
            Lista2.Add(Atributo3); Lista2.Add(Atributo4);

            var Carta1 = new Carta() { IdCarta = "1", Atributos = Lista1 };
            var Carta2 = new Carta() { IdCarta = "2", Atributos = Lista2 };
            var Carta3 = new Carta() { IdCarta = "3", Atributos = Lista1 };
            var Carta4 = new Carta() { IdCarta = "4", Atributos = Lista2 };

            Jugador1.Cartas.Add(Carta1); Jugador1.Cartas.Add(Carta2);
            Jugador2.Cartas.Add(Carta3); Jugador2.Cartas.Add(Carta4);

            Assert.AreEqual(PartidaPrueba.ResolverCartasNormales("Fuerza", "1", "4"), "Id2");
            Assert.AreEqual(PartidaPrueba.jugadores[0].Cartas.Count, 1);
            Assert.AreEqual(PartidaPrueba.jugadores[1].Cartas.Count, 3);

        }      

        [TestMethod]
        public void DeberiaPoderCrearPartida()
        {
            var nuevapartida = new Partida();
            var jugador = new Jugador();
            nuevapartida.Turno = jugador;
            nuevapartida.EstaCompleto = true;

            Assert.AreEqual(true, nuevapartida.EstaCompleto);
            Assert.AreEqual(jugador, nuevapartida.Turno);
            Assert.AreEqual(0, nuevapartida.jugadores.Count);
            Assert.AreEqual(null, nuevapartida.Mazo);
        }

        [TestMethod]
        public void DeberiaPoderAgregarJugadoresALaPartida()
        {
            var nuevapartida = new Partida();
            var jugador1 = new Jugador();
            var jugador2 = new Jugador();

            nuevapartida.jugadores.Add(jugador1);
            nuevapartida.jugadores.Add(jugador2);

            Assert.AreEqual(2, nuevapartida.jugadores.Count);
        }

        [TestMethod]
        public void DeberiaPoderAsignarUnMazoAUnaPartida()
        {
            var nuevapartida = new Partida();
            var mazoxmen = new Mazo();

            nuevapartida.Mazo = mazoxmen;

            Assert.AreEqual(mazoxmen, nuevapartida.Mazo);
        }

        [TestMethod]
        public void DeberiaPoderControlarCanrtidadDeJugadores()
        {
            var nuevapartida = new Partida();
            var jugador1 = new Jugador();
            var jugador2 = new Jugador();
            var mazo = new Mazo();

            nuevapartida.Mazo = mazo;
            nuevapartida.jugadores.Add(jugador1);
            nuevapartida.jugadores.Add(jugador2);

            nuevapartida.EstaCompleto = false;
            nuevapartida.RepartirCartas();

            Assert.AreEqual(true, nuevapartida.EstaCompleto);

        }



        [TestMethod]
        public void SeDeberiaRepartirTodasLasCartas()
        {
            var nuevapartida = new Partida();
            var carta1 = new Carta(); carta1.IdCarta = "1";
            var carta2 = new Carta(); carta2.IdCarta = "2";
            var carta3 = new Carta(); carta3.IdCarta = "3";
            var carta4 = new Carta(); carta4.IdCarta = "4";

            var mazzo = new Mazo();
            mazzo.Cartas.Add(carta1);
            mazzo.Cartas.Add(carta2);
            mazzo.Cartas.Add(carta3);
            mazzo.Cartas.Add(carta4);

            var mazzo2 = new Mazo();
            mazzo2.Cartas.Add(carta1);
            mazzo2.Cartas.Add(carta2);
            mazzo2.Cartas.Add(carta3);
            mazzo2.Cartas.Add(carta4);


            nuevapartida.Mazo = mazzo;

            var jugador1 = new Jugador(); jugador1.NumeroJugador = NumJugador.uno;
            var jugador2 = new Jugador(); jugador1.NumeroJugador = NumJugador.dos;

            nuevapartida.jugadores.Add(jugador1);
            nuevapartida.jugadores.Add(jugador2);

            nuevapartida.RepartirCartas();

            bool ok = false;
            foreach (var item in nuevapartida.jugadores[0].Cartas)
            {
                foreach (var item2 in nuevapartida.jugadores[1].Cartas)
                {
                    if (item == item2)
                    {
                        ok = true;
                    }
                }
            }

            Assert.AreEqual(2, jugador1.Cartas.Count);
            Assert.AreNotEqual(mazzo2, nuevapartida.Mazo);
            Assert.AreEqual(false, ok);

           // Si se juega revancha
            nuevapartida.Revancha();
            Assert.AreEqual(2, nuevapartida.jugadores[0].Cartas.Count);
            Assert.AreEqual(2, nuevapartida.jugadores[1].Cartas.Count);
        }       

        [TestMethod]
        public void NoDeberiaRepartirConMazoVacio()
        {
            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");
            Partida nuevaPartida = new Partida();
            var mazo = new Mazo();

            nuevaPartida.Jugador(jugador1).Jugador(jugador2).SetMazo(mazo).EstaCompleto = true;

            nuevaPartida.RepartirCartas();

            Assert.AreEqual(0, nuevaPartida.jugadores[0].Cartas.Count);


        }

        [TestMethod]
        public void DeberiaSacarDosCartasAlJugadorQueEnfrentaAUnaCartaRoja()
        {
            List<Atributo> atributos = new List<Atributo>();
            atributos.Add(new Atributo { Nombre = "Velocidad", Valor = 25 });
            Carta carta1 = new Carta { IdCarta = "1", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta2 = new Carta { IdCarta = "2", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta3 = new Carta { IdCarta = "3", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta4 = new Carta { IdCarta = "4", TipoCarta = TipoDeCarta.Roja, Atributos = null };
            Carta carta5 = new Carta { IdCarta = "5", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };

            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");

            jugador1.Cartas.Add(carta1); jugador1.Cartas.Add(carta2); jugador1.Cartas.Add(carta3); jugador1.Cartas.Add(carta5);
            jugador2.Cartas.Add(carta4);


            Partida nuevaPartida = new Partida();
            nuevaPartida.Jugador(jugador1).Jugador(jugador2);

            nuevaPartida.AgregarCartasGanadas(carta3, jugador1, 2, carta4, jugador2);

            Assert.AreEqual(2, nuevaPartida.jugadores[1].Cartas.Count);

            Assert.AreEqual(2, nuevaPartida.jugadores[0].Cartas.Count);


            Assert.AreEqual(2, nuevaPartida.jugadores[0].Cartas.Count);
        }

        [TestMethod]
        public void DeberiaSacarUnaCartaAlJugadorQueEnfrentaAUnaCartaAmarillaConUnaNormal()
        {
            List<Atributo> atributos = new List<Atributo>();
            atributos.Add(new Atributo { Nombre = "Velocidad", Valor = 25 });
            Carta carta1 = new Carta { IdCarta = "1", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta2 = new Carta { IdCarta = "2", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta3 = new Carta { IdCarta = "3", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta4 = new Carta { IdCarta = "4", TipoCarta = TipoDeCarta.Amarilla, Atributos = null };
            Carta carta5 = new Carta { IdCarta = "5", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };

            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");

            jugador1.Cartas.Add(carta1); jugador1.Cartas.Add(carta2); jugador1.Cartas.Add(carta3); jugador1.Cartas.Add(carta5);
            jugador2.Cartas.Add(carta4);


            Partida nuevaPartida = new Partida();
            nuevaPartida.Jugador(jugador1).Jugador(jugador2);

            nuevaPartida.AgregarCartasGanadas(carta3, jugador1, 1, carta4, jugador2);
            Assert.AreEqual(3, nuevaPartida.jugadores[0].Cartas.Count);
            Assert.AreEqual(1, nuevaPartida.jugadores[1].Cartas.Count);
        }

        [TestMethod]
        public void DeberiaSacarUnaCartaAlJugadorQueEnfrentaAUnaCartaRojaConUnaAmarilla()
        {

            List<Atributo> atributos = new List<Atributo>();
            atributos.Add(new Atributo { Nombre = "Velocidad", Valor = 25 });
            Carta carta1 = new Carta { IdCarta = "1", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta2 = new Carta { IdCarta = "2", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta4 = new Carta { IdCarta = "3", TipoCarta = TipoDeCarta.Roja, Atributos = null };
            Carta carta3 = new Carta { IdCarta = "4", TipoCarta = TipoDeCarta.Amarilla, Atributos = null };
            Carta carta5 = new Carta { IdCarta = "5", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta6 = new Carta { IdCarta = "6", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };

            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");

            jugador1.Cartas.Add(carta1); jugador1.Cartas.Add(carta2); jugador1.Cartas.Add(carta3); jugador1.Cartas.Add(carta5);
            jugador2.Cartas.Add(carta4); jugador2.Cartas.Add(carta6);

            Partida nuevaPartida = new Partida();
            nuevaPartida.Jugador(jugador1).Jugador(jugador2);

            nuevaPartida.AgregarCartasGanadas(carta3, jugador1, 1, carta4, jugador2);
            Assert.AreEqual(3, nuevaPartida.jugadores[0].Cartas.Count);
            Assert.AreEqual(2, nuevaPartida.jugadores[1].Cartas.Count);

        }

        [TestMethod]
        public void DeberiaSacarLaPrimeraCartaAljugadorQueSeEnfrentaAUnaRojaConUnaAmarillaDeUltimaCarta()
        {

            List<Atributo> atributos = new List<Atributo>();
            atributos.Add(new Atributo { Nombre = "Velocidad", Valor = 25 });
            Carta carta1 = new Carta { IdCarta = "1", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta2 = new Carta { IdCarta = "2", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta4 = new Carta { IdCarta = "3", TipoCarta = TipoDeCarta.Roja, Atributos = null };
            Carta carta3 = new Carta { IdCarta = "4", TipoCarta = TipoDeCarta.Amarilla, Atributos = null };
            Carta carta5 = new Carta { IdCarta = "5", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta6 = new Carta { IdCarta = "6", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };

            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");

            jugador1.Cartas.Add(carta1); jugador1.Cartas.Add(carta2); jugador1.Cartas.Add(carta3);
            jugador2.Cartas.Add(carta4); jugador2.Cartas.Add(carta6);

            Partida nuevaPartida = new Partida();
            nuevaPartida.Jugador(jugador1).Jugador(jugador2);

            nuevaPartida.AgregarCartasGanadas(carta3, jugador1, 1, carta4, jugador2);
            Assert.AreEqual(2, nuevaPartida.jugadores[0].Cartas.Count);
            Assert.AreEqual(2, nuevaPartida.jugadores[1].Cartas.Count);
            Assert.AreEqual(carta1, nuevaPartida.jugadores[0].Cartas[0]);

        }

        [TestMethod]
        public void DeberiaSacarLaUltimaYPrimerCartaAlJugadorQuePierdeConUnaCartaNormalVsUnaRoja()
        {

            List<Atributo> atributos = new List<Atributo>();
            atributos.Add(new Atributo { Nombre = "Velocidad", Valor = 25 });
            Carta carta1 = new Carta { IdCarta = "1", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta2 = new Carta { IdCarta = "2", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta4 = new Carta { IdCarta = "3", TipoCarta = TipoDeCarta.Roja, Atributos = null };
            Carta carta3 = new Carta { IdCarta = "4", TipoCarta = TipoDeCarta.Amarilla, Atributos = null };
            Carta carta5 = new Carta { IdCarta = "5", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta6 = new Carta { IdCarta = "6", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };

            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");

            jugador1.Cartas.Add(carta1); jugador1.Cartas.Add(carta2); jugador1.Cartas.Add(carta3); jugador1.Cartas.Add(carta5);
            jugador2.Cartas.Add(carta4); jugador2.Cartas.Add(carta6);

            Partida nuevaPartida = new Partida();
            nuevaPartida.Jugador(jugador1).Jugador(jugador2);

            nuevaPartida.AgregarCartasGanadas(carta5, jugador1, 2, carta4, jugador2);
            Assert.AreEqual(2, nuevaPartida.jugadores[0].Cartas.Count);
            Assert.AreEqual(3, nuevaPartida.jugadores[1].Cartas.Count);
            Assert.AreEqual(carta1, nuevaPartida.jugadores[0].Cartas[0]);

        }

        [TestMethod]
        public void DeberiaSacarUnaCartaAlJugadorQuePierdeContraUnaRojaTeniendoUnaAmarilla()
        {

            List<Atributo> atributos = new List<Atributo>();
            atributos.Add(new Atributo { Nombre = "Velocidad", Valor = 25 });
            Carta carta1 = new Carta { IdCarta = "1", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta2 = new Carta { IdCarta = "2", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta4 = new Carta { IdCarta = "3", TipoCarta = TipoDeCarta.Roja, Atributos = null };
            Carta carta3 = new Carta { IdCarta = "4", TipoCarta = TipoDeCarta.Amarilla, Atributos = null };
            Carta carta5 = new Carta { IdCarta = "5", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            Carta carta6 = new Carta { IdCarta = "6", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };

            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");

            jugador1.Cartas.Add(carta1); jugador1.Cartas.Add(carta2); jugador1.Cartas.Add(carta3); jugador1.Cartas.Add(carta5);
            jugador2.Cartas.Add(carta4); jugador2.Cartas.Add(carta6);

            Partida nuevaPartida = new Partida();
            nuevaPartida.Jugador(jugador1).Jugador(jugador2);

            nuevaPartida.AgregarCartasGanadas(carta3, jugador1, 2, carta4, jugador2);
            Assert.AreEqual(2, nuevaPartida.jugadores[0].Cartas.Count);
            Assert.AreEqual(3, nuevaPartida.jugadores[1].Cartas.Count);


        }

        [TestMethod]
        public void DeberiaActualizarRanking()
        {
            var nuevojuego = new Juego();

            Carta carta1 = new Carta();

            var nuevapartida1 = new Partida();
            Jugador jugador1 = new Jugador().Nombre("Riquelme").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Palermo").Numero(NumJugador.dos).IdConexion("1"); jugador2.Cartas.Add(carta1);
            nuevapartida1.jugadores.Add(jugador1);
            nuevapartida1.jugadores.Add(jugador2);

            nuevojuego.Partidas.Add(nuevapartida1);
            nuevojuego.Partidas[0].ActualizarRanking();

            Assert.AreEqual(1, nuevojuego.Partidas[0].Resultado.VecesQueGanoElJugador2);
            Assert.AreEqual(0, nuevojuego.Partidas[0].Resultado.VecesQueGanoElJugador1);
            Assert.AreEqual("Riquelme", nuevojuego.Partidas[0].Resultado.NombreJugador1);
            Assert.AreEqual("Palermo", nuevojuego.Partidas[0].Resultado.NombreJugador2);
        }

        [TestMethod]
        public void DeberiaCrearPartidaHub_CartaHub_MazoHub_JugadorHub_DibujarTableroHub()
        {
            var partidaHub = new PartidasHub {  Mazo="Xmen", Nombre="Los xmen", Usuario="Juan"};
            var cartaHub = new CartaHub { Codigo= "11", Nombre = "Batata" };
            var mazoHub = new MazoHub { Nombre="Aviones" }; var x1 = new Atributo {  Nombre="zzzzz", Valor=11}; var x2 = new Atributo {  Nombre="qqqq", Valor=22};
            mazoHub.NombreAtributos.Add(x1.Nombre); mazoHub.NombreAtributos.Add(x2.Nombre);
            var jugadorHab = new JugadorHub { Nombre="Riquelme"}; jugadorHab.Cartas.Add(cartaHub);
            var jugadorHab1 = new JugadorHub { Nombre = "Palermo" }; jugadorHab.Cartas.Add(cartaHub);
            var dibujarTablero = new DibujarTableroHub {  Jugador1=jugadorHab, Jugador2= jugadorHab1 , Mazo= mazoHub};


            Assert.AreEqual(partidaHub.Mazo, "Xmen"); Assert.AreEqual(partidaHub.Nombre, "Los xmen"); Assert.AreEqual(partidaHub.Usuario, "Juan");
            Assert.AreEqual(cartaHub.Nombre, "Batata"); Assert.AreEqual(cartaHub.Codigo, "11");
            Assert.AreEqual(2, mazoHub.NombreAtributos.Count); Assert.AreEqual(mazoHub.Nombre, "Aviones");
            Assert.AreEqual(2, jugadorHab.Cartas.Count); Assert.AreEqual("Riquelme",jugadorHab.Nombre);
            Assert.AreEqual(dibujarTablero.Jugador1,jugadorHab); Assert.AreEqual(dibujarTablero.Jugador2, jugadorHab1); Assert.AreEqual(dibujarTablero.Mazo, mazoHub);
        }

        [TestMethod]
        public void DeberiaGanarElJugadorDelTurnoCuandoHayEmpate()
        {
            var partida = new Partida();
            List<Atributo> atributos = new List<Atributo>();
            var atributo1 = new Atributo { Nombre = "Velocidad", Valor = 40 };

            atributos.Add(atributo1);

            var carta1 = new Carta { IdCarta = "1", Nombre = "CARTA1", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };
            var carta2 = new Carta { IdCarta = "1", Nombre = "CARTA2", TipoCarta = TipoDeCarta.Normal, Atributos = atributos };

            Jugador jugador1 = new Jugador().Nombre("Maxi").Numero(NumJugador.uno).IdConexion("1");
            Jugador jugador2 = new Jugador().Nombre("Juan").Numero(NumJugador.dos).IdConexion("2");

            partida.Jugador(jugador1).Jugador(jugador2);
            partida.jugadores[0].Cartas.Add(carta1);
            partida.jugadores[1].Cartas.Add(carta2);

            partida.AnalizarCartas(carta1, carta2, "Velocidad");

            Assert.AreEqual(0, jugador2.Cartas.Count);
            Assert.AreEqual(2, jugador1.Cartas.Count);


        }


    }
}