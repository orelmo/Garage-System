using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Bike : Vehicle
    {
        private int m_EngineVolume;
        private eDrivingLicenseType m_DrivingLicenseType;

        public Bike(string i_LicensePlate, Engine i_Engine, float i_MaxAirPressure) :
            base(i_LicensePlate, i_Engine, i_MaxAirPressure)
        {
            m_Wheels = new List<Wheel>((int)eWheelsNumber.Two);
        }

        public int EngineVolume
        {
            get
            {
                return m_EngineVolume;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Non positive engine volume");
                }

                m_EngineVolume = value;
            }
        }

        public eDrivingLicenseType DrivingLicenseType
        {
            get
            {
                return m_DrivingLicenseType;
            }

            set
            {
                m_DrivingLicenseType = value;
            }
        }

        public enum eDrivingLicenseType
        {
            A = 1,
            A1,
            AA,
            B
        }

        public override string ToString()
        {
            StringBuilder vehicleDetails = new StringBuilder();
            vehicleDetails.Append(base.ToString());
            vehicleDetails.Append(string.Format("Driving License Type: {0}", Enum.GetName(typeof(eDrivingLicenseType), DrivingLicenseType)));
            vehicleDetails.Append(Environment.NewLine);
            vehicleDetails.Append(string.Format("Engine Volume: {0}", EngineVolume));

            return vehicleDetails.ToString();
        }

        public override bool CheckPropertyValidity(eProperties i_PropertyToCheck, string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = base.CheckPropertyValidity(i_PropertyToCheck, i_ValueToCheck, ref o_Error);
            switch (i_PropertyToCheck)
            {
                case eProperties.LicenseType:
                    {
                        isValidProperty = checkLicenseType(i_ValueToCheck, ref o_Error);
                        break;
                    }

                case eProperties.EngineVolume:
                    {
                        isValidProperty = checkEngineVolume(i_ValueToCheck, ref o_Error);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return isValidProperty;
        }

        private static bool checkEngineVolume(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = false;
            if (i_ValueToCheck.Length == 0)
            {
                o_Error = "Empty Value";
            }
            else if (int.TryParse(i_ValueToCheck, out int engineVolume) == false)
            {
                o_Error = "Non-Numeric Engine Volume";
            }
            else if (engineVolume < 1)
            {
                o_Error = "Non-Positive Engine Volume";
            }
            else
            {
                isValidProperty = true;
            }

            return isValidProperty;
        }

        private static bool checkLicenseType(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = false;
            if (i_ValueToCheck.Length == 0)
            {
                o_Error = "Empty Value";
            }
            else
            {
                foreach (string drivingLicenseType in Enum.GetNames(typeof(eDrivingLicenseType)))
                {
                    if (i_ValueToCheck.ToUpper() == drivingLicenseType)
                    {
                        isValidProperty = true;
                        break;
                    }
                }

                if (isValidProperty == false)
                {
                    o_Error = "Driving License Not Exist";
                }
            }

            return isValidProperty;
        }

        public override void UpdateProperties(Dictionary<Vehicle.eProperties, string> i_VehiclePropertiesValues, List<string> i_WheelsMakers, List<float> i_WheelsAirPressure)
        {
            base.UpdateProperties(i_VehiclePropertiesValues, i_WheelsMakers, i_WheelsAirPressure);
            DrivingLicenseType = StringToEnum<eDrivingLicenseType>(i_VehiclePropertiesValues[eProperties.LicenseType]);
            EngineVolume = int.Parse(i_VehiclePropertiesValues[eProperties.EngineVolume]);
            VehicleEngine.UpdateProperties(i_VehiclePropertiesValues, CurrentEnergyPercentage);
        }

        public override void FillRequests()
        {
            base.FillRequests();
            Requests[eProperties.LicenseType] = "Please Enter Your License Type: ";
            Requests[eProperties.EngineVolume] = "Please Enter The Engine Volume: ";
        }
    }
}
