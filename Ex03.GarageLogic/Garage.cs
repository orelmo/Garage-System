using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private Dictionary<string, VehicleTicket> m_VehiclesInTheGarage = new Dictionary<string, VehicleTicket>();

        public Dictionary<string, VehicleTicket> VehiclesInTheGarage
        {
            get
            {
                return m_VehiclesInTheGarage;
            }
        }

        public enum eGarageOptions
        {
            EnterVehicle = 1,
            ShowLicensePlates,
            ChangeVehicleStatus,
            InflateTires,
            FuelTank,
            ChargeElectricVehicle,
            ShowVehicleDetails
        }

        public enum eVehicleStatus
        {
            InProgress = 1,
            Finished,
            Paied
        }

        public static bool CheckIfLegalName(string i_Name)
        {
            return VehicleTicket.CheckIfLegalName(i_Name);
        }

        public static bool CheckIfYesOrNo(string i_UserAnswer, ref string o_Error, ref bool? o_ToFilter)
        {
            bool validAnswer = false;
            if (i_UserAnswer.ToUpper().Equals("Y") == false && i_UserAnswer.ToUpper().Equals("N") == false)
            {
                o_Error = "Please Enter Y\\N.";
            }
            else
            {
                validAnswer = true;
                o_ToFilter = i_UserAnswer.ToUpper().Equals("Y") ? true : false;
            }

            return validAnswer;
        }

        public static bool CheckClientChoosement(string i_ClientChoiceStr, out int o_ClientChoiceInt, ref string o_Error)
        {
            bool isClientChoosementValid = true;
            if (!int.TryParse(i_ClientChoiceStr, out o_ClientChoiceInt))
            {
                o_Error = "You Entered Non-Numeric Character. Please Try Again.";
                isClientChoosementValid = false;
            }
            else
            {
                if (o_ClientChoiceInt < 1 || o_ClientChoiceInt > 7)
                {
                    o_Error = "Please Enter Number Between 1 To 7. Please Try Again.";
                    isClientChoosementValid = false;
                }
            }

            return isClientChoosementValid;
        }

        public static bool IsValidVehicleStatus(string i_Value, ref string o_Error)
        {
            bool isValidValue = false;
            int status;
            if (int.TryParse(i_Value, out status) == false)
            {
                o_Error = "Non-Numeric Character. Please Try Again.";
            }
            else
            {
                foreach (int statusAsInt in Enum.GetValues(typeof(Garage.eVehicleStatus)))
                {
                    if (statusAsInt == status)
                    {
                        isValidValue = true;
                        break;
                    }
                }
            }

            return isValidValue;
        }

        public static bool CheckIfLegalPhoneNumber(string i_PhoneNumberToCheck)
        {
            return VehicleTicket.CheckIfLegalPhoneNumber(i_PhoneNumberToCheck);
        }

        public string GetVehicleDetailsString(string i_LicensePlate)
        {
            if (Vehicle.CheckLicensePlate(i_LicensePlate) == false)
            {
                throw new FormatException("Invalid license plate");
            }

            return VehiclesInTheGarage[i_LicensePlate].ToString();
        }

        public void EnterNewVehicle(string i_OwnerName, string i_OwnerPhone, Vehicle i_NewVehicle)
        {
            VehiclesInTheGarage[i_NewVehicle.LicensePlate] = new VehicleTicket(i_OwnerName, i_OwnerPhone, i_NewVehicle);
        }      

        public void ChangeVehicleStatus(string i_LicensePlate, Garage.eVehicleStatus i_VehicleNewStatus)
        {
            if (Vehicle.CheckLicensePlate(i_LicensePlate) == false)
            {
                throw new FormatException("Invalid license plate");
            }

            m_VehiclesInTheGarage[i_LicensePlate].VehicleStatus = i_VehicleNewStatus;
        }

        public bool IsVehicleInGarage(string i_LicensePlate)
        {
            if (Vehicle.CheckLicensePlate(i_LicensePlate) == false)
            {
                throw new FormatException("Invalid license plate");
            }

            return m_VehiclesInTheGarage.ContainsKey(i_LicensePlate);
        }

        public void InflateTires(string i_LicensePlate)
        {
            if (Vehicle.CheckLicensePlate(i_LicensePlate) == false)
            {
                throw new FormatException("Invalid license plate");
            }

            VehiclesInTheGarage[i_LicensePlate].OwnersVehicle.InflateTires();
        }

        public void FillEnergy(string i_LicensePlate, float i_EnergyAmount, Engine.eFuelType? i_FuelType = null)
        {
            if (Vehicle.CheckLicensePlate(i_LicensePlate) == false)
            {
                throw new FormatException("Invalid license plate");
            }

            VehiclesInTheGarage[i_LicensePlate].OwnersVehicle.FillEnergy(i_EnergyAmount, i_FuelType);
        }

        public List<string> FilteredLicensePlatesOfVehiclesInGarage(Garage.eVehicleStatus? i_VehicleStatusFilter = null)
        {
            List<string> filteredLicensePlates = new List<string>();
            foreach (KeyValuePair<string, VehicleTicket> vehicleInGarage in VehiclesInTheGarage)
            {
                if (i_VehicleStatusFilter == null || i_VehicleStatusFilter == vehicleInGarage.Value.VehicleStatus)
                {
                    filteredLicensePlates.Add(vehicleInGarage.Key);
                }
            }

            return filteredLicensePlates;
        }

        public bool IsGasVehicle(string i_LicensePlate)
        {
            return VehiclesInTheGarage[i_LicensePlate].OwnersVehicle.IsGasVehicle();
        }

        public bool IsElectricVehicle(string i_LicensePlate)
        {
            return VehiclesInTheGarage[i_LicensePlate].OwnersVehicle.IsElectricVehicle();
        }

        public class VehicleTicket
        {
            private string m_OwnerName;
            private string m_OwnerPhone;
            private eVehicleStatus m_VehicleStatus = eVehicleStatus.InProgress;
            private Vehicle m_OwnersVehicle;

            public Vehicle OwnersVehicle
            {
                get
                {
                    return m_OwnersVehicle;
                }

                set
                {
                    m_OwnersVehicle = value;
                }
            }

            public string OwnerName
            {
                get
                {
                    return m_OwnerName;
                }

                set
                {
                    if (value.Length == 0)
                    {
                        throw new FormatException("Invalid owner name");
                    }

                    m_OwnerName = value;
                }
            }

            public eVehicleStatus VehicleStatus
            {
                get
                {
                    return m_VehicleStatus;
                }

                set
                {
                    m_VehicleStatus = value;
                }
            }

            public string OwnerPhone
            {
                get
                {
                    return m_OwnerPhone;
                }

                set
                {
                    if (CheckIfLegalPhoneNumber(value) == false)
                    {
                        throw new FormatException("Invalid phone number");
                    }

                    m_OwnerPhone = value;
                }
            }

            public VehicleTicket(string i_OwnerName, string i_OwnerPhone, Vehicle i_Vehicle)
            {
                OwnerName = i_OwnerName;
                OwnerPhone = i_OwnerPhone;
                OwnersVehicle = i_Vehicle;
            }

            public static bool CheckIfLegalName(string i_Name)
            {
                bool isLegalName = true;
                if (i_Name.Length == 0)
                {
                    isLegalName = false;
                }

                if (isLegalName)
                {
                    foreach (char ch in i_Name)
                    {
                        if (char.IsLetter(ch) == false && char.IsSeparator(ch) == false)
                        {
                            isLegalName = false;
                            break;
                        }
                    }
                }

                return isLegalName;
            }

            public static bool CheckIfLegalPhoneNumber(string i_PhoneNumber)
            {
                bool isLegalPhoneNumber = true;
                if (i_PhoneNumber.Length == 0)
                {
                    isLegalPhoneNumber = false;
                }

                if (isLegalPhoneNumber == true)
                {
                    foreach (char ch in i_PhoneNumber)
                    {
                        if (char.IsDigit(ch) == false)
                        {
                            isLegalPhoneNumber = false;
                            break;
                        }
                    }
                }

                return isLegalPhoneNumber;
            }

            public override string ToString()
            {
                StringBuilder vehicleDetails = new StringBuilder();
                vehicleDetails.Append(string.Format(
@"Owner Name: {0}
Status: {1}",
OwnerName,
Enum.GetName(typeof(eVehicleStatus), (int)VehicleStatus)));
                vehicleDetails.Append(string.Format("{0}--------------------{0}", Environment.NewLine));
                vehicleDetails.Append(OwnersVehicle.ToString());

                return vehicleDetails.ToString();
            }          
        }
    }
}