using System;

namespace Juce.Utils.Contracts
{
    public static class Contract
    {
        public static void HandleFailedContract(string errorMessage, string infoMessage = "")
        {
            if (string.IsNullOrEmpty(infoMessage))
            {
                throw new Exception($"{errorMessage}");
            }
            else
            {
                throw new Exception($"{errorMessage} [Info: {infoMessage}]");
            }
        }

        public static void IsNotNull(object obj, string infoMessage = "")
        {
            if (obj == null)
            {
                string errorMessage = $"{nameof(IsNotNull)} contract has failed. Expected: not null, got null";
                HandleFailedContract(errorMessage, infoMessage);
            }
        }

        public static void IsNull(object obj, string infoMessage = "")
        {
            if (obj != null)
            {
                string errorMessage = $"{nameof(IsNull)} contract has failed. Expected: null, got: {obj}";
                HandleFailedContract(errorMessage, infoMessage);
            }
        }

        public static void IsTrue(bool obj, string infoMessage = "")
        {
            if (!obj)
            {
                string errorMessage = $"{nameof(IsTrue)} contract has failed. Expected: true, got: {obj}";
                HandleFailedContract(errorMessage, infoMessage);
            }
        }

        public static void IsFalse(bool obj, string infoMessage = "")
        {
            if (obj)
            {
                string errorMessage = $"{nameof(IsFalse)} contract has failed. Expected: false, got: {obj}";
                HandleFailedContract(errorMessage, infoMessage);
            }
        }

        public static void IsNotZero(int obj, string infoMessage = "")
        {
            if (obj == 0)
            {
                string errorMessage = $"{nameof(IsNotZero)} contract has failed. Expected: 0, got: {obj}";
                HandleFailedContract(errorMessage, infoMessage);
            }
        }

        public static void IsZero(int obj, string infoMessage = "")
        {
            if (obj != 0)
            {
                string errorMessage = $"{nameof(IsZero)} contract has failed. Expected: non zero, got: {obj}";
                HandleFailedContract(errorMessage, infoMessage);
            }
        }
    }
}