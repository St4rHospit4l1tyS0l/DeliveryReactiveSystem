using System;
using System.Collections.Generic;
using System.Linq;
using Drs.Infrastructure.Resources;
using Drs.Model.Constants;
using Drs.Model.Notification;
using Drs.Model.Shared;
using Drs.Model.Store;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Notification 
{
    public class NotificationRepository : BaseOneRepository
    {
        public List<NotificationModel> GetNotifications(int storeId)
        {
            var today = DateTime.Today;

            return Db.StoreMessageDate.Where(e => e.FranchiseStoreId == storeId && (e.DateApplied == today || e.IsIndefinite))
                .Select(e => new NotificationModel
                {
                    Message = e.StoreMessage.Message,
                    CategoryMessageId = e.CategoryMessageId,
                    FranchiseStoreId = e.FranchiseStoreId,
                    IsIndefinite = e.IsIndefinite,
                    Resource = e.Resource
                }).ToList();
        }

        public List<ItemCategory> GetCatMessages()
        {
            return Db.CategoryMessage.Select(e => new ItemCategory
            {
                Id = e.CategoryMessageId,
                Name =  e.Category,
                Color = e.Color,
                Position = e.Position
            }).OrderBy(e => e.Position).ToList();
        }

        public string GetStoreName(int storeId)
        {
            return Db.FranchiseStore.Where(e => e.FranchiseStoreId == storeId)
                .Select(e => e.Name).Single();
        }

        public ResponseMessageModel InsertNotification(StoreNotificationModel model, string userId)
        {
            var storeMessageId = GetStoreMessageIdByMessageIfExists(model);

            var today = DateTime.Today;
            if (storeMessageId == SharedConstants.DEFAULT_INT_VALUE)
            {
                storeMessageId = InsertStoreMessage(model.Message, userId);
            }
            else
            {
                if (IsAnyNotificationForDate(today, storeMessageId, model))
                {
                    return new ResponseMessageModel
                    {
                        HasError = true,
                        Message = "La notificación ya fue agregada para este día, para la categoría y la sucursal seleccionada"
                    };
                }
            }

            var storeMessageDate = new StoreMessageDate
            {
                CategoryMessageId = model.CategoryMessageId,
                DateApplied = today,
                FranchiseStoreId = model.FranchiseStoreId,
                StoreMessageId = storeMessageId,
                IsIndefinite = model.IsIndefinite,
                Resource = model.Resource,
                UserIdIns = userId
            };

            Db.StoreMessageDate.Add(storeMessageDate);
            Db.SaveChanges();
            
            return new ResponseMessageModel
            {
                HasError = false,
                Data = model.Message
            };
        }

        private bool IsAnyNotificationForDate(DateTime date, int storeMessageId, StoreNotificationModel model)
        {
            return Db.StoreMessageDate.Any(e => e.FranchiseStoreId == model.FranchiseStoreId 
                && e.StoreMessageId == storeMessageId
                && e.CategoryMessageId == model.CategoryMessageId && (e.IsIndefinite || e.DateApplied == date));
        }

        private int InsertStoreMessage(string notification, string userId)
        {
            var storeMessage = new StoreMessage
            {
                Message = notification,
                UserIdIns = userId,
            };
            Db.StoreMessage.Add(storeMessage);
            Db.SaveChanges();
            return storeMessage.StoreMessageId;
        }

        private int GetStoreMessageIdByMessageIfExists(StoreNotificationModel model)
        {
            var storeMessageId =
                Db.StoreMessage.Where(e => e.Message == model.Message).Select(e => e.StoreMessageId).SingleOrDefault();
            return storeMessageId;
        }

        public ResponseMessageModel DeleteNotification(StoreNotificationModel model)
        {
            var today = DateTime.Today;
            var storeMessage = Db.StoreMessageDate.SingleOrDefault(e => e.FranchiseStoreId == model.FranchiseStoreId
                                                               && (e.IsIndefinite || e.DateApplied == today)
                                                               && e.CategoryMessageId == model.CategoryMessageId
                                                               && e.StoreMessage.Message == model.Message);

            if (storeMessage == null)
                return new ResponseMessageModel
                {
                    HasError = true,
                    Message = "La notificación que desea eliminar ya no existe, por favor refresque su navegador para ver las notificaciones activas para el día actual"
                };

            Db.StoreMessageDate.Remove(storeMessage);
            Db.SaveChanges();

            return new ResponseMessageModel {HasError = false, Data = model.Message};
        }

        public List<string> GetNotifications(string notification, int limit)
        {
            return Db.StoreMessage.Where(e => e.Message.Contains(notification)).Select(e => e.Message).Take(limit).ToList();
        }
    }
}