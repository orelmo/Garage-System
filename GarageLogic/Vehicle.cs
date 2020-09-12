using System;
using System.Collections.Generic;
using System.Text;

namespace GarageLogic
{
    public abstract class Vehicle
    {
        private readonly string r_LicensePlate;
        private readonly Dictionary<eProperties, string> m_Requests;
        private readonly float r_MaxAirPressure;
        private Engine m_Engine;
        protected List<Wheel> m_Wheels;
        private string m_Model;
        private float m_CurrentEnergyPercentage = 0;

        protected Vehicle(string i_LicensePlate, Engine i_Engine, float i_MaxAirPressure)
        {
            if (CheckLicensePlate(i_LicensePlate) == false)
            {
                throw new FormatException("Invalid license plate");
            }

            r_LicensePlate = i_LicensePlate;
            m_Engine = i_Engine;
            if (i_MaxAirPressure <= 0)
            {
                throw new ArgumentException("Non positive max air pressure");
            }

            r_MaxAirPressure = i_MaxAirPressure;
            m_Requests = new Dictionary<eProperties, string>();
        }

        public float CurrentEnergyPercentage
        {
            get
            {
                return m_CurrentEnergyPercentage;
            }

            set
            {
                string error = null;
                if (CheckStringIsPrecentage(value.ToString(), ref error) == false)
                {
                    throw new ValueOutOfRangeException(0, 100);
                }

                m_CurrentEnergyPercentage = value;
            }
        }

        public int NumOfWheels
        {
            get
            {
                return m_Wheels.Capacity;
            }
        }

        public float MaxAirPressure
        {
            get
            {
                return r_MaxAirPressure;
            }
        }

        public string Model
        {
            get
            {
                return m_Model;
            }

            set
            {
                if (value.Length == 0)
                {
                    throw new FormatException("Invalid model name");
                }

                m_Model = value;
            }
        }

        internal List<Wheel> VehicleWheels
        {
            get
            {
                return m_Wheels;
            }
        }

        internal Engine VehicleEngine
        {
            get
            {
                return m_Engine;
            }
        }

        public Dictionary<eProperties, string> Requests
        {
            get
            {
                return m_Requests;
            }
        }

        internal string LicensePlate
        {
            get
            {
                return r_LicensePlate;
            }
        }

        public enum eVehicleType
        {
            GasBike = 1,
            ElectricBike,
            GasCar,
            ElectricCar,
            Truck
        }

        public enum eWheelsNumber
        {
            Two = 2,
            Four = 4,
            Sixteen = 16
        }

        public enum eProperties
        {
            LicenseType = 1,
            EngineVolume,
            Color,
            Doors,
            IsDangerousMaterials,
            CargoVolume,
            CurrentEnergyAmountPercentage,
            WheelMaker,
            WheelCurrentAirPressure,
            Model
        }

        public static bool CheckLicensePlate(string i_LicensePlateToCheck)
        {
            bool isValidString = true;
            if (i_LicensePlateToCheck.Length == 0)
            {
                isValidString = false;
            }

            if (isValidString == true)
            {
                foreach (char character in i_LicensePlateToCheck)
                {
                    if (!char.IsLetterOrDigit(character))
                    {
                        isValidString = false;
                        break;
                    }
                }
            }

            return isValidString;
        }

        public static bool CheckVehicleType(string i_vehicleTypeStr, ref int? o_ClientChoiceInt, ref string o_Error)
        {
            bool isClientChoosementValid = false;
            int clientChoiceInt;
            if (i_vehicleTypeStr.Length == 0)
            {
                o_Error = "Empty Value. Please Try Again.";
            }
            else if (!int.TryParse(i_vehicleTypeStr, out clientChoiceInt))
            {
                o_Error = "You Entered Non-Numeric Character. Please Try Again.";
            }
            else
            {
                foreach (int enumValue in Enum.GetValues(typeof(eVehicleType)))
                {
                    if (clientChoiceInt == enumValue)
                    {
                        isClientChoosementValid = true;
                        o_ClientChoiceInt = enumValue;
                        break;
                    }
                }

                if (isClientChoosementValid == false)
                {
                    o_Error = "Out Of Limits Value. Please Try Again.";
                }
            }

            return isClientChoosementValid;
        }

        public static bool CheckRemainingEnergyValueValidity(string i_ValueToCheck, Vehicle i_Vehicle, ref string o_Error)
        {
            bool isValid = false;
            if (CheckStringIsPrecentage(i_ValueToCheck, ref o_Error) == true)
            {
                isValid = true;                
            }
            else if (i_ValueToCheck.Length == 0)
            {
                o_Error = "Empty Value";
            }
            else
            {
                o_Error = "Value Don't Represent Percentage";
            }

            return isValid;
        }

        internal static bool CheckStringIsPrecentage(string i_EnergyResoursePercentageStr, ref string o_Error)
        {
            bool isValidPercentage = true;
            isValidPercentage = float.TryParse(i_EnergyResoursePercentageStr, out float energyResoursePercentageFloat);
            if (isValidPercentage == false)
            {
                o_Error = "Value Is Not A Precantage";
            }
            else if (energyResoursePercentageFloat < 0 || energyResoursePercentageFloat > 100)
            {
                isValidPercentage = false;
                o_Error = "Value Is Not In Range";
            }

            return isValidPercentage;
        }

        public bool IsGasVehicle()
        {
            return VehicleEngine is GasEngine;
        }

        public bool IsElectricVehicle()
        {
            return VehicleEngine is ElectricEngine;
        }

        protected static T StringToEnum<T>(string i_StringToTranslate)
        {
            T retEnum = (T)Enum.GetValues(typeof(T)).GetValue(0);
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (i_StringToTranslate.Equals(Enum.GetName(typeof(T), enumValue)))
                {
                    retEnum = enumValue;
                    break;
                }
            }

            return retEnum;
        }

        private bool checkWheelPressure(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidValue = true;
            if (i_ValueToCheck.Length == 0)
            {
                isValidValue = false;
                o_Error = "Empty Value Entered";
            }
            else if (float.TryParse(i_ValueToCheck, out float airPressure) == false)
            {
                isValidValue = false;
                o_Error = "Non-Numeric Character";
            }
            else if (airPressure < 0 || airPressure > MaxAirPressure)
            {
                isValidValue = false;
                o_Error = "Air Pressure Is Not In Allowed Limits";
            }

            return isValidValue;
        }

        private bool CheckModel(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = true;
            if (i_ValueToCheck.Length == 0)
            {
                isValidProperty = false;
                o_Error = "Empty Model Name";
            }

            return isValidProperty;
        }

        public override string ToString()
        {
            StringBuilder vehicleDetails = new StringBuilder();
            List<Wheel> wheelsDetails = VehicleWheels;
            vehicleDetails.Append(string.Format("License Plate: {0}{1}", LicensePlate, Environment.NewLine));
            vehicleDetails.Append(string.Format("Vehicle Model: {0}{1}", Model, Environment.NewLine));
            int wheelNum = 1;
            foreach (Wheel wheel in wheelsDetails)
            {
                vehicleDetails.Append(string.Format("Wheel Number {0}: {1}{2}", wheelNum, wheel.ToString(), Environment.NewLine));
                ++wheelNum;
            }

            if(VehicleEngine is GasEngine)
            {
                vehicleDetails.Append(VehicleEngine.ToString());
                vehicleDetails.Append(Environment.NewLine);
            }

            vehicleDetails.Append(string.Format("Current Energy Percentage: {0}", CurrentEnergyPercentage));
            vehicleDetails.Append(Environment.NewLine);

            return vehicleDetails.ToString();
        }

        public List<T> CreateWheelsList<T>()
        {
            return new List<T>(VehicleWheels.Capacity);
        }

        public void AssignWheelsToVehicle(List<string> i_WheelsMakers, List<float> i_WheelsAirPressure)
        {
            for (int i = 0; i < VehicleWheels.Capacity; ++i)
            {
                VehicleWheels.Add(new Wheel(i_WheelsMakers[i], i_WheelsAirPressure[i]));
            }
        }

        public void FillEnergy(float i_EnergyAmount, Engine.eFuelType? i_FuelType = null)
        {
            if (VehicleEngine is GasEngine && i_FuelType != (VehicleEngine as GasEngine).FuelType)
            {
                if (i_FuelType == null)
                {
                    throw new ArgumentException("No fuel type recived");
                }
                else
                {
                    throw new ArgumentException("Fuel type not match");
                }
            }

            VehicleEngine.FillEnergy(i_EnergyAmount, this, i_FuelType);
        }

        public virtual void FillRequests()
        {
            Requests[eProperties.Model] = "Please Enter The Vehicle Model: ";
            Requests[eProperties.CurrentEnergyAmountPercentage] = "Please Enter The Current Energy Amount Percentage: ";
        }

        public virtual bool CheckPropertyValidity(eProperties i_PropertyToCheck, string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = false;
            switch (i_PropertyToCheck)
            {
                case eProperties.WheelMaker:
                    {
                        isValidProperty = Wheel.CheckWheelMakerName(i_ValueToCheck, ref o_Error);
                        break;
                    }

                case eProperties.WheelCurrentAirPressure:
                    {
                        isValidProperty = checkWheelPressure(i_ValueToCheck, ref o_Error);
                        break;
                    }

                case eProperties.Model:
                    {
                        isValidProperty = CheckModel(i_ValueToCheck, ref o_Error);
                        break;
                    }

                case eProperties.CurrentEnergyAmountPercentage:
                    {
                        isValidProperty = Vehicle.CheckRemainingEnergyValueValidity(i_ValueToCheck, this, ref o_Error);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return isValidProperty;
        }

        public virtual void UpdateProperties(Dictionary<Vehicle.eProperties, string> i_VehiclePropertiesValues, List<string> i_WheelsMakers, List<float> i_WheelsAirPressure)
        {
            Model = i_VehiclePropertiesValues[eProperties.Model];
            CurrentEnergyPercentage = float.Parse(i_VehiclePropertiesValues[eProperties.CurrentEnergyAmountPercentage]);
            AssignWheelsToVehicle(i_WheelsMakers, i_WheelsAirPressure);
        }

        public void InflateTires()
        {
            foreach (Wheel wheel in VehicleWheels)
            {
                wheel.Inflate(MaxAirPressure - wheel.CurrentAirPressure);
            }
        }

        public class Wheel
        {
            private string m_Maker = null;
            private float m_CurrentAirPressure;

            public Wheel(string i_WheelMaker, float i_AirPressure)
            {
                Maker = i_WheelMaker;
                CurrentAirPressure = i_AirPressure;
            }

            internal float CurrentAirPressure
            {
                get
                {
                    return m_CurrentAirPressure;
                }

                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Negativ current air pressure");
                    }

                    m_CurrentAirPressure = value;
                }
            }

            internal string Maker
            {
                get
                {
                    return m_Maker;
                }

                set
                {
                    if (value.Length == 0)
                    {
                        throw new ArgumentException("Empty wheel maker name");
                    }

                    m_Maker = value;
                }
            }

            public static bool CheckWheelMakerName(string i_ValueToCheck, ref string o_Error)
            {
                bool isValidValue = i_ValueToCheck.Length > 0 ? true : false;
                if (isValidValue == false)
                {
                    o_Error = "Wheel Maker Name Can't Be Empty";
                }

                return isValidValue;
            }

            public override string ToString()
            {
                return string.Format("Manufacturer: {0}  Current Air Pressure: {1}", Maker, CurrentAirPressure);
            }

            public void Inflate(float i_AirToAdd)
            {
                CurrentAirPressure += i_AirToAdd;
            }
        }

        public class VehicleBuilder
        {
            public static Vehicle BuildVehicle(string i_LicensePlate, Vehicle.eVehicleType i_VehicleType)
            {
                const float k_BikeMaxAirPressure = 30;
                const float k_CarMaxAirPressure = 32;
                const float k_TruckMaxAirPressure = 28;

                const float k_GasBikeMaxTankSize = 7;
                const float k_GasCarMaxTankSize = 60;
                const float k_ElectricCarMaxBaterySize = 2.1f;
                const float k_ElectricBikeMaxBaterySize = 1.2f;
                const float k_TruckMaxTankSize = 120;

                Vehicle newVehicle = null;
                switch (i_VehicleType)
                {
                    case Vehicle.eVehicleType.ElectricBike:
                        {
                            newVehicle = new Bike(i_LicensePlate, new ElectricEngine(k_ElectricBikeMaxBaterySize), k_BikeMaxAirPressure);
                            break;
                        }

                    case Vehicle.eVehicleType.ElectricCar:
                        {
                            newVehicle = new Car(i_LicensePlate, new ElectricEngine(k_ElectricCarMaxBaterySize), k_CarMaxAirPressure);
                            break;
                        }

                    case Vehicle.eVehicleType.GasBike:
                        {
                            newVehicle = new Bike(i_LicensePlate, new GasEngine(Engine.eFuelType.Octan95, k_GasBikeMaxTankSize), k_BikeMaxAirPressure);
                            break;
                        }

                    case Vehicle.eVehicleType.GasCar:
                        {
                            newVehicle = new Car(i_LicensePlate, new GasEngine(Engine.eFuelType.Octan96, k_GasCarMaxTankSize), k_CarMaxAirPressure);
                            break;
                        }

                    case Vehicle.eVehicleType.Truck:
                        {
                            newVehicle = new Truck(i_LicensePlate, new GasEngine(Engine.eFuelType.Soler, k_TruckMaxTankSize), k_TruckMaxAirPressure);
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }

                newVehicle.FillRequests();

                return newVehicle;
            }
        }

        internal static bool IsPositiveFloat(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidValue = true;
            if (float.TryParse(i_ValueToCheck, out float cargoVolume) == false)
            {
                isValidValue = false;
                o_Error = "Non-Numeric Value";
            }
            else if (cargoVolume < 0)
            {
                isValidValue = false;
                o_Error = "Negative Value";
            }

            return isValidValue;
        }
    }
}