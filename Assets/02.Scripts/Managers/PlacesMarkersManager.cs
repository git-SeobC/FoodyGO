using FoddyGo.Services;
using FoodyGo.Controllers;
using FoodyGo.Mapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoddyGo.Services.GooglePlacesService;

namespace FoodyGo.Managers
{
    class PlacesMarkersManager : MonoBehaviour
    {
        [SerializeField] private PlaceMarkerController _placeMarkerController;
        [SerializeField] private PlaceMarkerController _markerPrefab;
        [SerializeField] private GooglePlacesService _googlePlacesService;
        [SerializeField] private GPSLocationService _gpsLocationService;
        [SerializeField] int _markerCount = 10;
        List<PlaceMarkerController> _markers;

        private void Awake()
        {
            // Reserving한다 = _markerCount 만큼의 공간을 미리 할당해둠
            // 메모리 관리를 미리 생각
            _markers = new List<PlaceMarkerController>(_markerCount);
        }

        IEnumerator Start()
        {
            yield return new WaitUntil(() => _gpsLocationService.isReady);
            RefreshMarkers();
        }

        public void RefreshMarkers()
        {
            _googlePlacesService.SearchNearbyRequest(_gpsLocationService.latitude,
                                _gpsLocationService.longitude,
                                200,
                                new List<string> { "restaurant" },
                                null,
                                null,
                                null,
                                _markerCount,
                                "POPULARITY",
                                "ko",
                                "KR",
                                PlacesFields.DisplayName | PlacesFields.Types | PlacesFields.FormattedAddress | PlacesFields.Location,
                                RespawnMarkers);

        }

        void RespawnMarkers(IEnumerable<(string name, double latitude, double longitude)> places)
        {
            // 앞 인덱스 부터 지우게 되면 배열 RemoveAt에서 당기는 작업으로 인해 시간복잡도 상승하기 때문에((O(n^2)),
            // 전체 배열 삭제는 뒤에서 부터 진행해야 O(n)으로 삭제 가능
            for (int i = _markers.Count - 1; i >= 0; i--)
            {
                Destroy(_markers[i]);
                _markers.RemoveAt(i);
            }

            foreach (var place in places)
            {
                PlaceMarkerController marker = Instantiate(_markerPrefab);
                marker.RefreshPlace(place.name);
                marker.transform.position = new Vector3(GoogleMapUtils.LonToUnityX(place.longitude, _gpsLocationService.mapOrigin.longitude, _gpsLocationService.mapTileZoomLevel),
                    0f,
                    GoogleMapUtils.LatToUnityY(place.latitude, _gpsLocationService.mapOrigin.latitude, _gpsLocationService.mapTileZoomLevel));
                _markers.Add(marker);
            }
        }
    }
}
