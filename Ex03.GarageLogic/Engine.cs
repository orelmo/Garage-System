using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        private float m_CurrentEnergy = 0;
        private float m_MaximumEnergyCapacity;

        protected Engine(float i_MaxEnergy)
        {
            MaximumEnergyCapacity = i_MaxEnergy;
        }

        public float MaximumEnergyCapacity
        {
            get
            {
                return m_MaximumEnergyCapacity;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Not positive maximum energy capacity");
                }

                m_MaximumEnergyCapacity = value;
            }
        }

        public float CurrentEnergy
        {
            get
            {
                return m_CurrentEnergy;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Negative current energy amount");
                }

                m_CurrentEnergy = value;
            }
        }

        public enum eFuelType
        {
            Soler = 1,
            Octan95,
            Octan96,
            Octan98
        }

        public static bool IsCorrectEnergyAmount(Vehicle i_Vehicle, string i_EnergyAmountStr, ref string o_Error)
        {
            bool isValidEnergyAmount = true;
            if (i_EnergyAmountStr.Length == 0)
            {
                isValidEnergyAmount = false;
                o_Error = "Empty Value";
            }
            else if (float.TryParse(i_EnergyAmountStr, out float energyAmountFlt) == false)
            {
                isValidEnergyAmount = false;
                o_Error = "Non Numeric Character";
            }
            else if (energyAmountFlt < 0)
            {
                isValidEnergyAmount = false;
                o_Error = "Negative Amount";
            }
            else if (energyAmountFlt > i_Vehicle.VehicleEngine.MaximumEnergyCapacity - i_Vehicle.VehicleEngine.CurrentEnergy)
            {
                isValidEnergyAmount = false;
                o_Error = "The Value Is Too Hight";
            }

            return isValidEnergyAmount;
        }

        public static bool IsCorrectFuelType(Vehicle i_Vehicle, string i_FuelTypeStr, ref string o_Error)
        {
            bool isValidFuelType = true;
            if (int.TryParse(i_FuelTypeStr, out int fuelTypeInt) == false)
            {
                isValidFuelType = false;
                o_Error = "Non Numeric Character";
            }
            else if (fuelTypeInt > Enum.GetValues(typeof(Engine.eFuelType)).Length || fuelTypeInt < 1)
            {
                isValidFuelType = false;
                o_Error = "Value Out Of Limits";
            }
            else if ((eFuelType)fuelTypeInt != (i_Vehicle.VehicleEngine as GasEngine).FuelType)
            {
                isValidFuelType = false;
                o_Error = "Fuel Type Don't Match";
            }

            return isValidFuelType;
        }

        public void UpdateProperties(Dictionary<Vehicle.eProperties, string> i_VehiclePropertiesValues, float i_CurrentEnergyPercentage)
        {
            CurrentEnergy = (i_CurrentEnergyPercentage / 100) * MaximumEnergyCapacity;
        }

        public virtual void FillEnergy(float i_EnergyAmount, Vehicle i_Vehicle, Engine.eFuelType? i_FuelType)
        {
            if (i_EnergyAmount < 0)
            {
                throw new ArgumentException("Negative energy to add amount");
            }
            else if (i_EnergyAmount > MaximumEnergyCapacity - CurrentEnergy)
            {
                throw new ValueOutOfRangeException(MaximumEnergyCapacity - CurrentEnergy, 0);
            }

            CurrentEnergy += i_EnergyAmount;
            i_Vehicle.CurrentEnergyPercentage = (CurrentEnergy / MaximumEnergyCapacity) * 100;
        }
    }
}
