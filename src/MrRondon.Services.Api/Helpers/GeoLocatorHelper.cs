using System;
using MrRondon.Services.Api.Extensions;

namespace MrRondon.Services.Api.Helpers
{
    public static class GeoLocatorHelper
    {
        // Metodo que retorna a distancia entre locais num raio de X metros
        public static double PlacesAround(double latitudeFrom, double longitudeFrom, double latitudeTo, double longitudeTo, int precision)
        {
            const int earthRadius = 6371;//Km
            //calcula o raio de distancia entre os dois pontos
            var latitudeInRadian = (latitudeTo - latitudeFrom).ToRadian();
            var longitudeInRadian = (longitudeTo - longitudeFrom).ToRadian();
            //Usa a formula de Haversine para conferir as posicoes geograficas dos pontos no globo terreste
            var tmp = (Math.Sin(latitudeInRadian / 2) * Math.Sin(latitudeInRadian / 2)) +
                      (Math.Cos(latitudeFrom.ToRadian()) * Math.Cos(latitudeTo.ToRadian()) *
                       Math.Sin(longitudeInRadian / 2) * Math.Sin(longitudeInRadian / 2));

            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(tmp)));
            var d = earthRadius * c;
            var distanceInMeters = d * 1000;
            return distanceInMeters;
        }
    }
}