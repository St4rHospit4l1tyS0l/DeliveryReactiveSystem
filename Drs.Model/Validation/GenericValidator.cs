using System;

namespace Drs.Model.Validation
{
    public static class GenericValidator
    {
        public static string ValidateEmptyAndLength(object value, int iMinLength, int iMaxLength)
        {
            var sValue = value as String;

            if (String.IsNullOrWhiteSpace(sValue))
                return "Este es un campo requerido";

            string format;
            return ValidateLength(iMinLength, iMaxLength, sValue, out format) ? format : null;
        }

        private static bool ValidateLength(int iMinLength, int iMaxLength, string sValue, out string format)
        {
            format = String.Empty;
            if (sValue.Length < iMinLength)
            {
                format = String.Format("La longitud mínima para este campo es de {0}", iMinLength);
                return true;
            }

            if (sValue.Length > iMaxLength)
            {
                format = String.Format("La longitud máxima para este campo es de {0}", iMaxLength);
                return true;
            }
            return false;
        }

        public static string ValidateLength(object value, int iMinLength, int iMaxLength)
        {
            if (value == null)
                return null;

            var sValue = value as String;

            if (String.IsNullOrEmpty(sValue))
                return null;

            if (sValue.Length < iMinLength)
                return String.Format("La longitud mínima para este campo es de {0}", iMinLength);

            if (sValue.Length > iMaxLength)
                return String.Format("La longitud máxima para este campo es de {0}", iMaxLength);

            return null;
        }
    }
}
