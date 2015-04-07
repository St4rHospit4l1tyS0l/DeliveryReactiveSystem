using System;
using Drs.Infrastructure.Extensions;
using Drs.Model.Annotations;

namespace Drs.Repository.Entities.Metadata
{
    public static class UserInfoJson
    {
        [UsedImplicitly] private static ViewUserInfo _modelEnt = null;
        public static readonly string Key = _modelEnt.PropertyName(e => e.Id);

        public static readonly string Columns = String.Join(",", new[]
        {
            _modelEnt.PropertyName(e => e.Id),
            _modelEnt.PropertyName(e => e.UserName),
            _modelEnt.PropertyName(e => e.FirstName),
            _modelEnt.PropertyName(e => e.LastName),
            _modelEnt.PropertyName(e => e.Role),
            _modelEnt.PropertyName(e => e.Email),
            _modelEnt.PropertyName(e => e.IsObsolete),
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
