using System;
using System.Collections.Generic;
using System.Text;

namespace GarageLogic
{
    public class Car : Vehicle
    {
        private eDoorsNumber? m_DoorsNumber = null;
        private eCarColors? m_CarColor = null;

        public Car(string i_LicensePlate, Engine i_Engine, float i_MaxAirPressure) : base(i_LicensePlate, i_Engine, i_MaxAirPressure)
        {
            m_Wheels = new List<Wheel>((int)eWheelsNumber.Four);
        }

        public eCarColors CarColor
        {
            get
            {
                return m_CarColor.Value;
            }

            set
            {
                m_CarColor = value;
            }
        }

        public eDoorsNumber DoorsNumber
        {
            get
            {
                return m_DoorsNumber.Value;
            }

            set
            {
                m_DoorsNumber = value;
            }
        }

        public enum eCarColors
        {
            Red = 1,
            White,
            Black,
            Silver
        }

        public enum eDoorsNumber
        {
            Two = 2,
            Three,
            Four,
            Five
        }

        public override string ToString()
        {
            StringBuilder vehicleDetails = new StringBuilder();
            vehicleDetails.Append(base.ToString());
            vehicleDetails.Append(string.Format("Color: {0}", Enum.GetName(typeof(eCarColors), CarColor)));
            vehicleDetails.Append(Environment.NewLine);
            vehicleDetails.Append(string.Format("Number Of Doors: {0}", DoorsNumber));

            return vehicleDetails.ToString();
        }

        public override bool CheckPropertyValidity(eProperties i_PropertyToCheck, string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = base.CheckPropertyValidity(i_PropertyToCheck, i_ValueToCheck, ref o_Error);
            switch (i_PropertyToCheck)
            {
                case eProperties.Color:
                    {
                        isValidProperty = checkColor(i_ValueToCheck, ref o_Error);
                        break;
                    }

                case eProperties.Doors:
                    {
                        isValidProperty = checkDoorsNumber(i_ValueToCheck, ref o_Error);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return isValidProperty;
        }

        private static bool checkDoorsNumber(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = true;
            if (i_ValueToCheck.Length == 0)
            {
                isValidProperty = false;
                o_Error = "Empty Value";
            }
            else if (int.TryParse(i_ValueToCheck, out int doorsNumber) == false)
            {
                isValidProperty = false;
                o_Error = "Non Numeric Character";
            }
            else
            {
                if (doorsNumber < (int)eDoorsNumber.Two || doorsNumber > (int)eDoorsNumber.Five)
                {
                    isValidProperty = false;
                    o_Error = "Value Out Of Limit";
                }
            }

            return isValidProperty;
        }

        private static bool checkColor(string i_ValueToCheck, ref string o_Error)
        {
            bool isValidProperty = false, isLetters = true;
            if (i_ValueToCheck.Length == 0)
            {
                o_Error = "Empty Value Entered";
            }
            else
            {
                foreach (char ch in i_ValueToCheck)
                {
                    if (char.IsLetter(ch) == false)
                    {
                        o_Error = "Non-Alphabetic Character";
                        isLetters = false;
                        break;
                    }
                }

                if (isLetters == true)
                {
                    foreach (string color in Enum.GetNames(typeof(eCarColors)))
                    {
                        if (i_ValueToCheck.ToUpper() == color.ToUpper())
                        {
                            isValidProperty = true;
                            break;
                        }
                    }

                    if (isValidProperty == false)
                    {
                        o_Error = "Unavalible Color";
                    }
                }
            }

            return isValidProperty;
        }

        public override void UpdateProperties(Dictionary<Vehicle.eProperties, string> i_VehiclePropertiesValues, List<string> i_WheelsMakers, List<float> i_WheelsAirPressure)
        {
            base.UpdateProperties(i_VehiclePropertiesValues, i_WheelsMakers, i_WheelsAirPressure);
            CarColor = StringToEnum<eCarColors>(i_VehiclePropertiesValues[eProperties.Color]);
            DoorsNumber = StringToEnum<eDoorsNumber>(i_VehiclePropertiesValues[eProperties.Doors]);
            VehicleEngine.UpdateProperties(i_VehiclePropertiesValues, CurrentEnergyPercentage);
        }

        public override void FillRequests()
        {
            base.FillRequests();
            Requests[eProperties.Color] = "Please Enter Your Car Color: ";
            Requests[eProperties.Doors] = "Please Enter The Number Of Doors: ";
        }
    }
}
