using System;
using System.Collections.Generic;
using System.Text;

namespace GarageLogic
{
    public class Truck : Vehicle
    {
        private bool m_ContainDangerousMaterials = false;
        private float m_CargoVolume = 0;

        public Truck(string i_LicensePlate, Engine i_Engine, float i_MaxAirPressure) :
            base(i_LicensePlate, i_Engine, i_MaxAirPressure)
        {
            m_Wheels = new List<Wheel>((int)eWheelsNumber.Sixteen);
        }

        public float CargoVolume
        {
            get
            {
                return m_CargoVolume;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Negative cargo volume");
                }

                m_CargoVolume = value;
            }
        }

        public bool ContainDangerousMaterials
        {
            get
            {
                return m_ContainDangerousMaterials;
            }

            set
            {
                m_ContainDangerousMaterials = value;
            }
        }

        public override string ToString()
        {
            StringBuilder vehicleDetails = new StringBuilder();

            vehicleDetails.Append(base.ToString());
            vehicleDetails.Append(string.Format("Is Contain Dangerous Materials? {0}", ContainDangerousMaterials));
            vehicleDetails.Append(Environment.NewLine);
            vehicleDetails.Append(string.Format("Cargo Volume: {0}", CargoVolume));

            return vehicleDetails.ToString();
        }

        public override void UpdateProperties(Dictionary<Vehicle.eProperties, string> i_VehiclePropertiesValues, List<string> i_WheelsMakers, List<float> i_WheelsAirPressure)
        {
            base.UpdateProperties(i_VehiclePropertiesValues, i_WheelsMakers, i_WheelsAirPressure);
            ContainDangerousMaterials = i_VehiclePropertiesValues[eProperties.IsDangerousMaterials].Equals('Y') ? true : false;
            CargoVolume = float.Parse(i_VehiclePropertiesValues[eProperties.CargoVolume]);
            VehicleEngine.UpdateProperties(i_VehiclePropertiesValues, CurrentEnergyPercentage);
        }

        public override bool CheckPropertyValidity(eProperties i_PropertyToCheck, string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = base.CheckPropertyValidity(i_PropertyToCheck, i_ValueToCheck, ref o_Error);
            switch (i_PropertyToCheck)
            {
                case eProperties.IsDangerousMaterials:
                    {
                        isValidProperty = checkIsDangerousMaterials(i_ValueToCheck, ref o_Error);
                        break;
                    }

                case eProperties.CargoVolume:
                    {
                        isValidProperty = checkCargoVolume(i_ValueToCheck, ref o_Error);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return isValidProperty;
        }

        private bool checkIsDangerousMaterials(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = true;
            if (!i_ValueToCheck.ToUpper().Equals("Y") && !i_ValueToCheck.ToUpper().Equals("N"))
            {
                isValidProperty = false;
                o_Error = "Value Is Not Y\\N";
            }

            return isValidProperty;
        }

        private bool checkCargoVolume(string i_ValueToCheck, ref string o_Error)
        {
            return IsPositiveFloat(i_ValueToCheck, ref o_Error);
        }

        public override void FillRequests()
        {
            base.FillRequests();
            Requests[eProperties.IsDangerousMaterials] = "The Truck Contains Dangerous Materials?(Y\\N): ";
            Requests[eProperties.CargoVolume] = "Please Enter The Cargo Volume: ";
        }
    }
}