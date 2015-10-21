using System;
using System.Linq;
using System.Linq.Dynamic;
using Drs.Repository.Entities;

namespace Drs.Repository.Shared
{
    public class ResourceRepository : BaseOneRepository, IResourceRepository
    {
        public ResourceRepository()
        {

        }

        public ResourceRepository(CallCenterEntities dbEntities)
            : base(dbEntities)
        {

        }


        public void Save(string originalFileName, Guid uidFileName, string pathFileName, string userId, int fileType, string checkSum)
        {
            var model = new Resource
            {
                CheckSum = checkSum,
                CurrentPath = pathFileName,
                FileType = fileType,
                IsObsolete = false,
                OriginalName = originalFileName,
                UploadDateTime = DateTime.Now,
                UploadUserId = userId,
                UidFileName = uidFileName.ToString(),
            };

            DbEntities.Resource.Add(model);
            DbEntities.SaveChanges();
        }

        public string GetFileNameByStoreName(Guid resource)
        {
            var sResource = resource.ToString();
            return DbEntities.Resource.Where(e => e.UidFileName == sResource).Select(e => e.OriginalName).FirstOrDefault();
        }

        public string GetResourcePath(string fileName)
        {
            return DbEntities.Resource.Where(e => e.UidFileName == fileName).Select(e => e.CurrentPath).FirstOrDefault();
        }
    }
}
