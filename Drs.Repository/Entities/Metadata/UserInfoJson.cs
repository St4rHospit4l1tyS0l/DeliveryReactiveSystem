using System;
using Drs.Infrastructure.Extensions;
using Drs.Model.Constants;

namespace Drs.Repository.Entities.Metadata
{
    public static class UserInfoJson
    {
        private static ViewUserInfo ModelEnt;
        public static readonly string Key = ModelEnt.PropertyName(e => e.Id);

        public static readonly string Columns = String.Join(",", new[]
        {
            ModelEnt.PropertyName(e => e.Id),
            ModelEnt.PropertyName(e => e.UserName),
            ModelEnt.PropertyName(e => e.FirstName),
            ModelEnt.PropertyName(e => e.LastName),
            ModelEnt.PropertyName(e => e.Role),
            ModelEnt.PropertyName(e => e.Email),
            ModelEnt.PropertyName(e => e.IsObsolete),
        });

        public static UserInfoDto DynamicToDto(dynamic data)
        {
            var model = new UserInfoDto
            {
                Id = data.Id,
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                IsObsolete = data.IsObsolete,
                UserName = data.UserName,
                Role = data.Role
            };

            return model;
        }
    }
}
