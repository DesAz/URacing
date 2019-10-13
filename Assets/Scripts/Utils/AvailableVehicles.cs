using System;
using System.Linq;
using UnityEngine;
using URacing.Controls;

namespace URacing
{
    public class AvailableVehicles : ScriptableObject
    {
        [Serializable]
        public class Vehicle
        {
            public string VehicleKey = default;
            public PlayerCarController CarController = default;
        }

        public static string CachedVehicleKey;

        public Vehicle[] Vehicles = default;

        public PlayerCarController SelectedCarController => GetCarController(CachedVehicleKey);

        private PlayerCarController GetCarController(string key)
        {
            var vehicle = Vehicles.FirstOrDefault(x => x.VehicleKey == key) ?? Vehicles.First();

            return vehicle.CarController;
        }
    }
}