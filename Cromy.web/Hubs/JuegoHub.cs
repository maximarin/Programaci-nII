﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using EntidadesJuego;

namespace Cromy.web.Hubs
{
    public class JuegoHub : Hub
    {
        private static Juego juego = new Juego();


        public void CrearPartida(string usuario, string partida, string mazo)
        {
            var partidaCreada = new Partida();
            var jugador1 = new Jugador();
            jugador1.Nombre(usuario).Numero(NumJugador.uno).IdConexion(Context.ConnectionId);

            partidaCreada.Nombre(partida).Jugador(jugador1);
            partidaCreada.Mazo(juego.BuscarMazo(mazo));
            juego.AgregarPartida(partidaCreada);                    

            // Notifico a los otros usuarios de la nueva partida.
            Clients.Others.agregarPartida(partidaCreada);

            Clients.Caller.esperarJugador();                      
        }

        public void UnirsePartida(string usuario, string partida)
        {            
            var jugador2 = new Jugador();
            jugador2.Nombre(usuario).IdConexion(Context.ConnectionId).Numero(NumJugador.dos);

            //AgregarAlSegundoJugador, agrega al jugador a la partida que elige y devuelve la partida que es
            var partidaEncontrada = juego.AgregarSegundoJugador(jugador2 , partida);

            partidaEncontrada.RepartirCartas();
            
            //Dibujar
            var x = juego.DibujarTablero(partidaEncontrada);

            Clients.All.eliminarPartida(partidaEncontrada.nombre);

            Clients.Client(partidaEncontrada.jugadores[0].idConexion).dibujarTablero(x.Jugador1,x.Jugador2,x.Mazo);
            Clients.Client(partidaEncontrada.jugadores[1].idConexion).dibujarTablero(x.Jugador1, x.Jugador2, x.Mazo);

               
        }

        public void ObtenerPartidas()
        {

            Clients.Caller.agregarPartidas(juego.RetornarPartidas());        

        }

        public void ObtenerMazos()
        {
            Clients.Caller.agregarMazos(juego.NombreDeLosMazos());

        }

        //public void Cantar(string idAtributo, string idCarta)
        //{

        //    if (jugada.connectionIdGanador == Context.ConnectionId)
        //    {
        //        Clients.Caller.ganarMano(resultado, false);
        //        Clients.Client(jugada.connectionIdPerdedor).perderMano(resultado, false);

        //    }
        //    else
        //    {
        //        Clients.Client(jugada.connectionIdGanador).ganarMano(resultado, false);
        //        Clients.Caller.perderMano(resultado, false);

        //    }
        //    if (jugada.finalizoJuego)
        //    {
        //        Clients.Caller.ganar();
        //        Clients.Client(jugada.connectionIdPerdedor).perder();
        //    }
        //}
    }
}