using System;
using System.Text;
using System.Collections.Generic;
using GarageLogic;

namespace ConsoleUI
{
    internal class UI
    {
        public static void Run()
        {
            Garage myGarage = new Garage();
            Console.WriteLine(string.Format("Welcome To The Garage!"));
            Garage.eGarageOptions? clientChoosement = null;
            string licensePlate = null;
            const bool k_GarageExist = true;
            do
            {
                Console.WriteLine();
                printActionOptionsToClient();
                clientChoosement = getClientChoosement();
                if (clientChoosement != Garage.eGarageOptions.ShowLicensePlates)
                {
                    licensePlate = GetLicensePlate();
                    if (clientChoosement != Garage.eGarageOptions.EnterVehicle)
                    {
                        if (myGarage.IsVehicleInGarage(licensePlate) == false)
                        {
                            Console.WriteLine("This Vehicle Is Not In The Garage.");
                            continue;
                        }
                    }
                }

                switch (clientChoosement)
                {
                    case Garage.eGarageOptions.EnterVehicle:
                        {
                            tryEnterVehicleToTheGarage(myGarage, licensePlate);
                            break;
                        }

                    case Garage.eGarageOptions.ShowLicensePlates:
                        {
                            showLicensePlates(myGarage);
                            break;
                        }

                    case Garage.eGarageOptions.ChangeVehicleStatus:
                        {
                            Garage.eVehicleStatus vehicleStatus = getVehicleStatus();
                            myGarage.ChangeVehicleStatus(licensePlate, vehicleStatus);
                            break;
                        }

                    case Garage.eGarageOptions.InflateTires:
                        {
                            myGarage.InflateTires(licensePlate);
                            break;
                        }

                    case Garage.eGarageOptions.FuelTank:
                        {
                            tryFuelTank(myGarage, licensePlate);
                            break;
                        }

                    case Garage.eGarageOptions.ChargeElectricVehicle:
                        {
                            tryChargeElectricVehicle(myGarage, licensePlate);
                            break;
                        }

                    case Garage.eGarageOptions.ShowVehicleDetails:
                        {
                            Console.WriteLine(myGarage.GetVehicleDetailsString(licensePlate));
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            while (k_GarageExist);
        }

        private static void showLicensePlates(Garage i_Garage)
        {
            bool toFilter = getIfFilterNeeded();
            if (toFilter == false)
            {
                printLicensePlates(i_Garage);
            }
            else
            {
                Garage.eVehicleStatus vehicleStatusFilter = getVehicleStatus();
                printLicensePlates(i_Garage, vehicleStatusFilter);
            }
        }

        private static void tryEnterVehicleToTheGarage(Garage i_Garage, string i_LicensePlate)
        {
            if (i_Garage.IsVehicleInGarage(i_LicensePlate))
            {
                Console.WriteLine("Your Vehicle Already In The Garage.");
                i_Garage.ChangeVehicleStatus(i_LicensePlate, Garage.eVehicleStatus.InProgress);
            }
            else
            {
                string ownerName, ownerPhone;
                getOwnerDetails(out ownerName, out ownerPhone);
                i_Garage.EnterNewVehicle(ownerName, ownerPhone, getVehicleFromClient(i_LicensePlate, i_Garage));
            }
        }

        private static void tryChargeElectricVehicle(Garage i_Garage, string i_LicensePlate)
        {
            if (i_Garage.IsElectricVehicle(i_LicensePlate))
            {
                float chargingTime = getEnergyAmountToAdd(i_Garage.VehiclesInTheGarage[i_LicensePlate].OwnersVehicle);
                i_Garage.FillEnergy(i_LicensePlate, chargingTime);
            }
            else
            {
                Console.WriteLine("Can't Charge Non-Electric Vehicle");
            }
        }

        public static void printVehicleDetails(Garage i_Garage, string i_LicensePlate)
        {
            Console.WriteLine(i_Garage.VehiclesInTheGarage[i_LicensePlate]);
        }

        private static float getChargingTime(Garage i_Garage, string i_LicensePlate)
        {
            string chargingTimetStr, error = null;
            bool firstTry = true;
            do
            {
                Console.WriteLine("Please Enter Changing Time:");
                chargingTimetStr = Console.ReadLine();
                if (firstTry == false)
                {
                    Console.WriteLine("{0}. Please Try Again.", error);
                }
            }
            while (!Engine.IsCorrectEnergyAmount(i_Garage.VehiclesInTheGarage[i_LicensePlate].OwnersVehicle, chargingTimetStr, ref error));

            return float.Parse(chargingTimetStr);
        }

        private static float getEnergyAmountToAdd(Vehicle i_Vehicle)
        {
            string enegryAmountStr, error = null;
            bool firstTry = true;
            do
            {
                if (firstTry == false)
                {
                    Console.WriteLine("{0}. Please Try Again.", error);
                }

                Console.WriteLine("Please Enter Energy Amount You Would Like To Add: ");
                enegryAmountStr = Console.ReadLine();
                firstTry = false;
            }
            while (!Engine.IsCorrectEnergyAmount(i_Vehicle, enegryAmountStr, ref error));

            return float.Parse(enegryAmountStr);
        }

        private static void tryFuelTank(Garage i_Garage, string i_LicensePlate)
        {
            if (i_Garage.IsGasVehicle(i_LicensePlate))
            {
                Engine.eFuelType fuelType = getFuelType(i_Garage.VehiclesInTheGarage[i_LicensePlate].OwnersVehicle);
                float fuelAmount = getEnergyAmountToAdd(i_Garage.VehiclesInTheGarage[i_LicensePlate].OwnersVehicle);
                i_Garage.FillEnergy(i_LicensePlate, fuelAmount, fuelType);
            }
            else
            {
                Console.WriteLine("Can't Fuel Non-Gas Vehicle");
            }
        }

        private static Garage.eVehicleStatus getVehicleStatus()
        {
            string userChoice, error = null;
            do
            {
                if (error != null)
                {
                    Console.WriteLine(error);
                }

                Console.WriteLine("Please Enter Vehicle Status: ");
                printMenu(Enum.GetNames(typeof(Garage.eVehicleStatus)));
                userChoice = Console.ReadLine();
            }
            while (Garage.IsValidVehicleStatus(userChoice, ref error) == false);

            return (Garage.eVehicleStatus)int.Parse(userChoice);
        }

        private static bool getIfFilterNeeded()
        {
            bool? toFilter = null;
            string userAnswer, error = null;
            Console.WriteLine("Do You Want To Filter The List? (Press Y/N)");
            do
            {
                if (error != null)
                {
                    Console.WriteLine(error);
                }

                userAnswer = Console.ReadLine();
            }
            while (Garage.CheckIfYesOrNo(userAnswer, ref error, ref toFilter) == false);

            return toFilter.Value;
        }

        private static void getOwnerDetails(out string o_OwnerName, out string o_OwnerPhone)
        {
            o_OwnerName = getOwnerName();
            o_OwnerPhone = getOwnerPhoneNumber();
        }

        private static string getOwnerPhoneNumber()
        {
            bool isValidString;
            string phoneNumber;
            Console.WriteLine("Please Enter Owner's Phone Number: ");
            do
            {
                phoneNumber = Console.ReadLine();
                if (Garage.CheckIfLegalPhoneNumber(phoneNumber) == false)
                {
                    Console.WriteLine("You Entered a Non-Digit Character. Please Try Again.");
                    isValidString = false;
                }
                else
                {
                    isValidString = true;
                }
            }
            while (isValidString == false);

            return phoneNumber;
        }

        private static string getOwnerName()
        {
            bool isValidString;
            string name;
            Console.WriteLine("Please Enter Owner's Name: ");
            do
            {
                name = Console.ReadLine();
                if (Garage.CheckIfLegalName(name) == false)
                {
                    Console.WriteLine("You Entered a Non-Alphabet Character. Please Try Again.");
                    isValidString = false;
                }
                else
                {
                    isValidString = true;
                }
            }
            while (isValidString == false);

            return name;
        }

        private static void printMenu(string[] i_Menu)
        {
            int optionNumber = 1;
            foreach (string option in i_Menu)
            {
                Console.WriteLine("{0}. {1}", optionNumber, option);
                ++optionNumber;
            }
        }

        private static Vehicle.eVehicleType getVehicleType()
        {
            Console.WriteLine("Please Select Vehicle Type: ");
            printMenu(Enum.GetNames(typeof(Vehicle.eVehicleType)));
            string vehicleTypeStr, error = null;
            int? clientChoiceInt = null;
            do
            {
                if (error != null)
                {
                    Console.WriteLine(error);
                }

                vehicleTypeStr = Console.ReadLine();
            }
            while (Vehicle.CheckVehicleType(vehicleTypeStr, ref clientChoiceInt, ref error) == false);

            return (Vehicle.eVehicleType)clientChoiceInt;
        }

        public static string GetLicensePlate()
        {
            Console.WriteLine("Please Enter Vehicle License Plate(Only Digits And Letters): ");
            string licensePlate;
            bool firstTry = true;
            do
            {
                if (firstTry == false)
                {
                    Console.WriteLine("You Entered Invalid Character. Please Try Again.");
                }

                licensePlate = Console.ReadLine();
                firstTry = false;
            }
            while (Vehicle.CheckLicensePlate(licensePlate) == false);

            return licensePlate;
        }

        private static void printLicensePlates(Garage i_MyGarage, Garage.eVehicleStatus? i_VehicleStatusFilter = null)
        {
            List<string> licensePlatesToPrint = i_MyGarage.FilteredLicensePlatesOfVehiclesInGarage(i_VehicleStatusFilter);
            if (licensePlatesToPrint.Count == 0)
            {
                Console.WriteLine("There are no vehicles in the garage that match your search");
            }
            else
            {
                Console.WriteLine("The License Plates Of The Vehicles In The Garage Are:");
                foreach (string licensePlate in licensePlatesToPrint)
                {
                    Console.WriteLine(licensePlate);
                }
            }
        }

        private static void printActionOptionsToClient()
        {
            Console.WriteLine(
@"Please Select An Action:
--------------------------
1. Enter Vehicle To The Garage
2. Show The Vehicles In The Garage
3. Change Vehicle Status
4. Inflate Vehicle Tires
5. Fuel Vehicle
6. Charge Vehicle
7. Show Vehicle Details");
        }

        private static Garage.eGarageOptions getClientChoosement()
        {
            int clientChoiceInt;
            string clientChoiceStr, error = null;
            do
            {
                if (error != null)
                {
                    Console.WriteLine(error);
                }

                clientChoiceStr = Console.ReadLine();
            }
            while (!Garage.CheckClientChoosement(clientChoiceStr, out clientChoiceInt, ref error));

            return (Garage.eGarageOptions)clientChoiceInt;
        }

        private static Vehicle getVehicleFromClient(string i_LicensePlate, Garage i_Garage)
        {
            Vehicle.eVehicleType vehicleType = getVehicleType();
            Vehicle vehicle = Vehicle.VehicleBuilder.BuildVehicle(i_LicensePlate, vehicleType);
            Console.WriteLine("Please Enter Vehicle Details: ");
            List<string> wheelMakers;
            List<float> wheelsAirPressure;
            Dictionary<Vehicle.eProperties, string> answers = getVehicleDetails(vehicle.Requests, vehicle, out wheelMakers, out wheelsAirPressure);
            vehicle.UpdateProperties(answers, wheelMakers, wheelsAirPressure);

            return vehicle;
        }

        private static Dictionary<Vehicle.eProperties, string> getVehicleDetails(Dictionary<Vehicle.eProperties, string> i_Requests, Vehicle i_Vehicle, out List<string> o_WheelsMakers, out List<float> o_WheelsAirPressure)////////////////////////////////////////////להוציא לפונקציות
        {
            o_WheelsMakers = new List<string>();
            o_WheelsAirPressure = new List<float>();
            Dictionary<Vehicle.eProperties, string> requestsAnswers = getProperties(i_Requests, i_Vehicle);
            o_WheelsMakers = i_Vehicle.CreateWheelsList<string>();
            o_WheelsAirPressure = i_Vehicle.CreateWheelsList<float>();
            getWheelsInfo(ref o_WheelsMakers, ref o_WheelsAirPressure, i_Vehicle);

            return requestsAnswers;
        }

        private static void getWheelsInfo(ref List<string> o_WheelsMakers, ref List<float> o_WheelsAirPressure, Vehicle i_Vehicle)
        {
            for (int i = 0; i < i_Vehicle.NumOfWheels; ++i)
            {
                o_WheelsMakers.Add(getWheelMaker(i + 1, i_Vehicle));
                o_WheelsAirPressure.Add(getWheelAirPressure(i + 1, i_Vehicle));
            }
        }

        private static string getWheelMaker(int i_WheelNumber, Vehicle i_Vehicle)
        {
            string error = null, wheelMaker;
            Console.WriteLine("Wheel Number {0}: ", i_WheelNumber);
            do
            {
                if (error != null)
                {
                    Console.WriteLine("{0}. Please Try Again.", error);
                }

                Console.WriteLine("Please Enter Wheel Maker: ");
                wheelMaker = Console.ReadLine();
                error = null;
            }
            while (i_Vehicle.CheckPropertyValidity(Vehicle.eProperties.WheelMaker, wheelMaker, ref error) == false);

            return wheelMaker;
        }

        private static Dictionary<Vehicle.eProperties, string> getProperties(Dictionary<Vehicle.eProperties, string> i_Requests, Vehicle i_Vehicle)
        {
            Dictionary<Vehicle.eProperties, string> answers = new Dictionary<Vehicle.eProperties, string>();
            string answer, error = null;
            foreach (KeyValuePair<Vehicle.eProperties, string> request in i_Requests)
            {
                do
                {
                    if (error != null)
                    {
                        Console.WriteLine("{0}. Please Try Again.", error);
                    }

                    Console.WriteLine(request.Value);
                    answer = Console.ReadLine();
                    error = null;
                }
                while (i_Vehicle.CheckPropertyValidity(request.Key, answer, ref error) == false);
                answers[request.Key] = answer;
            }

            return answers;
        }

        private static float getWheelAirPressure(int i_wheelNumber, Vehicle i_Vehicle)
        {
            string error = null, wheelAirPressure;
            Console.WriteLine("Wheel Number {0}: ", i_wheelNumber);
            do
            {
                if (error != null)
                {
                    Console.WriteLine("{0}. Please Try Again.", error);
                }

                Console.WriteLine("Please Enter Wheel AirPressure: ");
                wheelAirPressure = Console.ReadLine();
                error = null;
            }
            while (i_Vehicle.CheckPropertyValidity(Vehicle.eProperties.WheelCurrentAirPressure, wheelAirPressure, ref error) == false);

            return float.Parse(wheelAirPressure);
        }

        private static Engine.eFuelType getFuelType(Vehicle i_Vehicle)
        {
            string fuelTypeStr, error = null;
            bool firstTry = true;
            do
            {
                if (firstTry == false)
                {
                    Console.WriteLine("{0}. Please Try Again.", error);
                }

                Console.WriteLine("Please Choose The Fuel Type:");
                printMenu(Enum.GetNames(typeof(Engine.eFuelType)));
                fuelTypeStr = Console.ReadLine();
                firstTry = false;
            }
            while (!Engine.IsCorrectFuelType(i_Vehicle, fuelTypeStr, ref error));

            return (Engine.eFuelType)int.Parse(fuelTypeStr);
        }
    }
}