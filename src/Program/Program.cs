//-----------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Universidad Católica del Uruguay">
//     Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Ucu.Poo.Locations.Client;

namespace Ucu.Poo.LocationApi.Demo
{
    /// <summary>
    /// Un programa que demuestra el uso del cliente de la API REST de localización.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Punto de entrada al programa.
        /// </summary>
        public static async Task Main()
        {
            const string addressCentral = "Av. 8 de Octubre 2738";
            const string addressMullin = "Comandante Braga 2715";
            LocationApiClient client = new LocationApiClient();

            // Versión asincrónica

            Location locationCentral = await client.GetLocationAsync(addressCentral);
            Console.WriteLine($"Las coordenadas de '{addressCentral}' son " +
                $"'{locationCentral.Latitude}:{locationCentral.Longitude}'");

            Location locationMullin = await client.GetLocationAsync(addressMullin);
            Console.WriteLine($"Las coordenadas de '{addressMullin}' son " +
                $"'{locationMullin.Latitude}:{locationMullin.Longitude}'");

            Distance distance = await client.GetDistanceAsync(locationCentral, locationMullin);
            Console.WriteLine($"La distancia entre '{locationCentral.Latitude},{locationCentral.Longitude}' y "+
                $"'{locationMullin.Latitude},{locationMullin.Longitude}' es de {distance.TravelDistance} kilómetros.");

            distance = await client.GetDistanceAsync(addressCentral, addressMullin);
            Console.WriteLine($"La distancia entre '{addressCentral}' y '{addressMullin}' " +
                $"es de {distance.TravelDistance} kilómetros.");

            await client.DownloadMapAsync(locationCentral.Latitude, locationCentral.Longitude, @"map-a.png");
            Console.WriteLine($"Descargado asincrónicamente el mapa de '{addressCentral}'");

            await client.DownloadRouteAsync(
                locationCentral.Latitude,
                locationCentral.Longitude,
                locationMullin.Latitude,
                locationMullin.Longitude,
                @"route-a.png");
            Console.WriteLine($"Descargado asincrónicamente el mapa de '{addressCentral}' a '{addressMullin}'");

            // Versión sincrónica

            locationCentral = client.GetLocation(addressCentral);
            Console.WriteLine($"Las coordenadas de '{addressCentral}' son " +
                $"'{locationCentral.Latitude}:{locationCentral.Longitude}'");

            locationMullin = client.GetLocation(addressMullin);
            Console.WriteLine($"Las coordenadas de '{addressMullin}' son " +
                $"'{locationMullin.Latitude}:{locationMullin.Longitude}'");

            distance = client.GetDistance(locationCentral, locationMullin);
            Console.WriteLine($"La distancia entre '{locationCentral.Latitude},{locationCentral.Longitude}' y "+
                $"'{locationMullin.Latitude},{locationMullin.Longitude}' es de {distance.TravelDistance} kilómetros.");

            distance = client.GetDistance(addressCentral, addressMullin);
            Console.WriteLine($"La distancia entre '{addressCentral}' y '{addressMullin}' " +
                $"es de {distance.TravelDistance} kilómetros.");

            client.DownloadMap(locationCentral.Latitude, locationCentral.Longitude, @"map-s.png");
            Console.WriteLine($"Descargado sincrónicamente el mapa de '{addressCentral}'");

            client.DownloadRoute(
                locationCentral.Latitude,
                locationCentral.Longitude,
                locationMullin.Latitude,
                locationMullin.Longitude,
                @"route-s.png");
            Console.WriteLine($"Descargado sincrónicamente el mapa de '{addressCentral}' a '{addressMullin}'");
        }
    }
}
