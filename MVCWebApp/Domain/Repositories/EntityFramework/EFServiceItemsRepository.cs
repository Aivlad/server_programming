using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Domain.Entities;
using MVCWebApp.Domain.Repositories.Abstract;

namespace MVCWebApp.Domain.Repositories.EntityFramework
{
    public class EFServiceItemsRepository : IServiceItemsRepository
    {
        private readonly AppDbContext context;
        public EFServiceItemsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ServiceItem> GetServiceItems()
        {
            return context.ServiceItems;
        }

        public ServiceItem GetServiceItemById(Guid id)
        {
            return context.ServiceItems.FirstOrDefault(x => x.Id == id);
        }

        public void SaveServiceItem(ServiceItem entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteServiceItem(Guid id)
        {
            context.ServiceItems.Remove(new ServiceItem() { Id = id }); // немного костылим: удаляем через фейк объект (т.к. по сути нам нужно иметь id, остальное неважно)
            context.SaveChanges();
        }
    }
}
