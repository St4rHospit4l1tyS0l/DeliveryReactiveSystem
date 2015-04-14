using System.Collections.Generic;

namespace Drs.Model.Shared
{
    public static class AccountConstants
    {
        public const int CODE_NEW = 2811;
        public const int CODE_NOT_ACTIVE_ST = 1287;
        public const int CODE_NOT_ACTIVE_ET = 8123;
        public const int CODE_NOT_ACTIVE = 2938;
        public const int CODE_NOT_ACTIVE_BY_UPDATE = 1762;
        public const int CODE_VALID = 1028;

        public static readonly Dictionary<int, string> LstCodes = new Dictionary<int, string>
        {
            {CODE_NEW, "Esta terminal no tienen licencia, por ello es necesario que sea activada en el administrador central para comenzar a operar"},
            {CODE_NOT_ACTIVE_ST, "La licencia aún no está activa en esta terminal, es necesario que revise el periodo de activación en el administrador central"},
            {CODE_NOT_ACTIVE_ET, "Ha expirado la licencia en esta terminal, por ello es necesario que sea activada en el administrador central para comenzar a operar"},
            {CODE_NOT_ACTIVE, "La licencia no está activa en esta terminal, revise que la activación se realizó con éxito en el administrador central"},
            {CODE_NOT_ACTIVE_BY_UPDATE, "El hardware del equipo a cambiado, es necesario que active de nuevo la licencia en el administrador central"},
            {CODE_VALID, string.Empty}
        };

        public static readonly Dictionary<int, string> LstBadges = new Dictionary<int, string>
        {
            {CODE_NEW, "Equipo nuevo"},
            {CODE_NOT_ACTIVE_ST, "Equipo pendiente"},
            {CODE_NOT_ACTIVE_ET, "Equipo expirado"},
            {CODE_NOT_ACTIVE, "Equipo no activo"},
            {CODE_VALID, "Equipo activo"}
        };

    }
}
