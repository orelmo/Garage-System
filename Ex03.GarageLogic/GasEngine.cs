using System;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class GasEngine : Engine
    {
        private eFuelType m_FuelType;

        internal GasEngine(eFuelType i_FuelType, float i_MaxEnergy) : base(i_MaxEnergy)
        {
            m_FuelType = i_FuelType;
        }

        internal eFuelType FuelType
        {
            get
            {
                return m_FuelType;
            }
        }

        public override string ToString()
        {
            StringBuilder engineDetails = new StringBuilder();
            engineDetails.Append(string.Format("Fuel Type: {0}", Enum.GetName(typeof(eFuelType), (int)FuelType)));

            return engineDetails.ToString();
        }

        public override void FillEnergy(float i_EnergyAmount, Vehicle i_Vehicle, Engine.eFuelType? i_FuelType)
        {
            if (i_FuelType == null)
            {
                throw new ArgumentException("No fuel type recived");
            }
            else if (i_FuelType != FuelType)
            {
                throw new ArgumentException("Fuel don't match", i_FuelType.ToString());
            }

            base.FillEnergy(i_EnergyAmount, i_Vehicle, i_FuelType);
        }
    }
}
