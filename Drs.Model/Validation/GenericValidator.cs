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

            if (sValue.Length < iMinLength)
                return String.Format("La longitud mínima para este campo es de {0}", iMinLength);

            if (sValue.Length > iMaxLength)
                return String.Format("La longitud máxima para este campo es de {0}", iMaxLength);

            return null;
        }
    }
}
