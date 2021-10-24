using System;
using System.Globalization;
using System.Web;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace LocationApi
{
    public class LocationApiClient
    {
        private const string BaseUrl = "https://pii-locationapi.azurewebsites.net";

        private HttpClient client = new HttpClient();

        private string DistanceUrl { get { return BaseUrl + "/distance"; } }

        private string LocationUrl { get { return BaseUrl + "/location"; } }

        private string MapUrl { get { return BaseUrl + "/map"; } }

        private string RouteUrl { get { return BaseUrl + "/route"; } }

        private Uri GetUri(string baseUrl, IDictionary<string, string> parameters)
        {
            return new Uri(string.Format("{0}?{1}",
                baseUrl,
                string.Join("&",
                    parameters.Select(kvp =>
                        string.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(kvp.Value))))));
        }

        public async Task<Location> GetLocation(string address, string city = "Montevideo",
            string department = "Montevideo", string country = "Uruguay")
        {
            var parameters = new Dictionary<string, string>
            {
                { "address", address },
                { "city", city },
                { "department", department },
                { "country", country }
            };

            var requestUri = GetUri(LocationUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Location result = JsonSerializer.Deserialize<Location>(content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }

        public async Task<Distance> GetDistance(Location from, Location to)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fromLatitude", from.Latitude.ToString(CultureInfo.InvariantCulture) },
                { "fromLongitude", from.Longitude.ToString(CultureInfo.InvariantCulture) },
                { "toLatitude", to.Latitude.ToString(CultureInfo.InvariantCulture) },
                { "toLongitude", to.Longitude.ToString(CultureInfo.InvariantCulture) }
            };

            var requestUri = GetUri(DistanceUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Distance result = JsonSerializer.Deserialize<Distance>(content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }


        public async Task<Distance> GetDistance(string from, string to)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fromAddress", from },
                { "toAddress", to }
            };

            var requestUri = GetUri(DistanceUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Distance result = JsonSerializer.Deserialize<Distance>(content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return result;
        }

        public async Task DownloadMap(double latitude, double longitude, string path, int zoomLevel = 15)
        {
            var parameters = new Dictionary<string, string>
            {
                { "latitude", latitude.ToString(CultureInfo.InvariantCulture) },
                { "longitude", longitude.ToString(CultureInfo.InvariantCulture) },
                { "zoomLevel", zoomLevel.ToString(CultureInfo.InvariantCulture) }
            };

            var requestUri = GetUri(MapUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                await response.Content.CopyToAsync(fs);
                var stream = await response.Content.ReadAsStreamAsync();
            }
        }

        public async Task DownloadRoute(double fromLatitude, double fromLongitude,
            double toLatitude, double toLongitude, string path)
        {
            var parameters = new Dictionary<string, string>
            {
                { "fromLatitude", fromLatitude.ToString(CultureInfo.InvariantCulture) },
                { "fromLongitude", fromLongitude.ToString(CultureInfo.InvariantCulture) },
                { "toLatitude", toLatitude.ToString(CultureInfo.InvariantCulture) },
                { "toLongitude", toLongitude.ToString(CultureInfo.InvariantCulture) }
            };

            var requestUri = GetUri(RouteUrl, parameters);
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                await response.Content.CopyToAsync(fs);
                var stream = await response.Content.ReadAsStreamAsync();
            }
        }
    }
}
